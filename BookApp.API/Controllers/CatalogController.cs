using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
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

    private readonly IUserRepository _userRepo;
    private readonly IGraphRepository _graphRepo;

    public CatalogController (IMapper mapper, IGraphRepository graphRepo, IUserRepository userRepo) {
      _userRepo = userRepo;
      _graphRepo = graphRepo;
    }

    [HttpPost ("add")]
    public async Task<IActionResult> Add (CatalogCreateDto catalogCteateDto) {

      var identity = HttpContext.User.Identity as ClaimsIdentity;
      int userId = 0;
      if (identity != null)
        userId = Int32.Parse (identity.FindFirst (ClaimTypes.NameIdentifier).Value);

      catalogCteateDto.UserId = userId;
      catalogCteateDto.FriendlyUrl = BookApp.API.Helpers.Url.GenerateFriendlyUrl (catalogCteateDto.Name + "-" + Guid.NewGuid ());

      var result = _graphRepo.AddCatalog (catalogCteateDto, false);

      return Ok (result);
    }

    [HttpGet ("get/{friendlyUrl}")]
    public async Task<IActionResult> Get (string friendlyUrl) {
      var bookListItem = _graphRepo.GetCatalog (friendlyUrl);

      return Ok (bookListItem);
    }

    [HttpGet ("user-catalogs/{friendlyUrl}")]
    public async Task<IActionResult> GetForUser (string friendlyUrl) {

      var identity = HttpContext.User.Identity as ClaimsIdentity;
      string userFriendlyUrl = "";
      if (identity != null)
        userFriendlyUrl = identity.FindFirst (ClaimTypes.Name).Value;

      var isCurrentUser = friendlyUrl == userFriendlyUrl;

      var user = _userRepo.GetUser (friendlyUrl);
      if (user == null) {
        return NotFound ();
      }
      var bookListItems = _graphRepo.GetCatalogsForUser (user.Id, true);

      foreach (var item in bookListItems) {
        foreach (var book in item.Books) {
          if (!String.IsNullOrEmpty (book.PhotoPath) && book.PhotoPath.Contains ("cloudinary"))
            book.PhotoPath = CloudinaryHelper.TransformUrl (book.PhotoPath, TransformationType.Book_Details_Preset);
        }

        item.UserFriendlyUrl = friendlyUrl;
      }

      return Ok (bookListItems);
    }

    [HttpGet ("catalogs")]
    public async Task<IActionResult> GetPublicCatalogs () {

      var bookListItems = _graphRepo.GetAllPublicCatalogs ();

      foreach (var item in bookListItems) {
        foreach (var book in item.Books) {
          if (book.PhotoPath != null && book.PhotoPath.Contains ("cloudinary"))
            book.PhotoPath = CloudinaryHelper.TransformUrl (book.PhotoPath, TransformationType.Book_Details_Preset);
        }
      }

      return Ok (bookListItems);
    }

    [HttpGet ("user-catalogs-pure-list/{friendlyUrl}")]
    public async Task<IActionResult> GetPureCatalogsForUser (string friendlyUrl) {

      var identity = HttpContext.User.Identity as ClaimsIdentity;
      int userId = 0;
      if (identity != null)
        userId = Int32.Parse (identity.FindFirst (ClaimTypes.NameIdentifier).Value);

      var catalogs = _graphRepo.GetPureCatalogs (userId);
      return Ok (catalogs);
    }
  }
}