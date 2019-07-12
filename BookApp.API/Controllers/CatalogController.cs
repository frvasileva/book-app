using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
using BookApp.API.Data;
using BookApp.API.Dtos;
using BookApp.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookApp.API.Controllers {
  [Route ("api/[controller]")]
  [ApiController]
  [Authorize]
  public class CatalogController : ControllerBase {
    private readonly ICatalogRepository _repo;
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public CatalogController (ICatalogRepository repo, DataContext context, IMapper mapper) {
      _repo = repo;
      _context = context;
      _mapper = mapper;
    }

    [HttpPost ("add")]
    public async Task<IActionResult> Add (CatalogCreateDto catalogCteateDto) {

      var identity = HttpContext.User.Identity as ClaimsIdentity;
      int userId = 0;
      if (identity != null)
        userId = Int32.Parse (identity.FindFirst (ClaimTypes.NameIdentifier).Value);

      catalogCteateDto.UserId = userId;

      var result = await _repo.Create (catalogCteateDto);
      await _context.SaveChangesAsync ();

      return Ok (result);
    }

    [HttpGet ("get/{friendlyUrl}")]
    public async Task<IActionResult> Get (string friendlyUrl) {
      var bookListItem = await _repo.Get (friendlyUrl);
      return Ok (bookListItem);
    }

    [HttpGet ("user-catalogs/{friendlyUrl}")]
    public async Task<IActionResult> GetForUser (string friendlyUrl) {

      var identity = HttpContext.User.Identity as ClaimsIdentity;
      string userFriendlyUrl = "";
      if (identity != null)
        userFriendlyUrl = identity.FindFirst (ClaimTypes.Name).Value;

      var isCurrentUser = friendlyUrl == userFriendlyUrl;

      var bookListItems = await _repo.GetForUser (friendlyUrl, isCurrentUser);

      foreach (var item in bookListItems) {
        foreach (var book in item.Books) {
         book.PhotoPath = CloudinaryHelper.TransformUrl (book.PhotoPath, TransformationType.Book_Details_Preset);
        }
      }

      return Ok (bookListItems);
    }

    [HttpGet ("user-catalogs-pure-list/{friendlyUrl}")]
    public async Task<IActionResult> GetPureCatalogsForUser (string friendlyUrl) {

      var identity = HttpContext.User.Identity as ClaimsIdentity;
      string userFriendlyUrl = "";
      if (identity != null)
        userFriendlyUrl = identity.FindFirst (ClaimTypes.Name).Value;

      var isCurrentUser = friendlyUrl == userFriendlyUrl;

      var catalogs = await _repo.GetPureForUser (friendlyUrl, isCurrentUser);
      return Ok (catalogs);
    }

    [HttpGet ("get-all")]
    public async Task<IActionResult> GetAll () {

      var bookListItems = await _repo.GetAllPure ();

      foreach (var item in bookListItems) {
        foreach (var catalog in item.BookCatalogs) {
          catalog.Book.PhotoPath = CloudinaryHelper.TransformUrl (catalog.Book.PhotoPath, TransformationType.Book_Details_Preset);
        }
      }
      return Ok (bookListItems);
    }

    [HttpPost ("update")]
    public async Task<IActionResult> Update (CatalogCreateDto catalogCteateDto) {
      var result = await _repo.Update (catalogCteateDto);

      return Ok (result);
    }
  }
}