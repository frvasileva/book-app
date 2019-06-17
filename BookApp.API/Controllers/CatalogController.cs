using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
using BookApp.API.Data;
using BookApp.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using BookApp.API.Helpers;

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

    [HttpGet ("get/{id}")]
    public async Task<IActionResult> Get (int id) {
      var bookListItem = await _repo.Get (id);
      return Ok (bookListItem);
    }

    [HttpGet ("user-catalogs/{friendlyUrl}")]
    public async Task<IActionResult> GetForUser (string friendlyUrl) {
      var bookListItems = await _repo.GetForUser (friendlyUrl);

       foreach (var item in bookListItems) {
        foreach (var catalog in item.BookCatalogs) {
          catalog.Book.PhotoPath = CloudinaryHelper.TransformUrl (catalog.Book.PhotoPath, TransformationType.Book_Details_Preset);
        }
      }
      
      return Ok (bookListItems);
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