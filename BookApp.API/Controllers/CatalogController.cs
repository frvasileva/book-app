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

    private readonly IUserRepository _userRepo;
    private readonly IGraphRepository _graphRepo;

    private int UserId {
      get {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        int userId = 0;
        if (identity != null)
          userId = Int32.Parse (identity.FindFirst (ClaimTypes.NameIdentifier).Value);

        return userId;
      }
    }

    public CatalogController (IMapper mapper, IGraphRepository graphRepo, IUserRepository userRepo) {
      _userRepo = userRepo;
      _graphRepo = graphRepo;
    }

    [HttpPost ("add")]
    public IActionResult Add (CatalogCreateDto catalogCteateDto) {

      catalogCteateDto.UserId = UserId;
      catalogCteateDto.FriendlyUrl = BookApp.API.Helpers.Url.GenerateFriendlyUrl (catalogCteateDto.Name + "-" + Guid.NewGuid ());

      var result = _graphRepo.AddCatalog (catalogCteateDto, false);

      return Ok (result);
    }

    [HttpGet ("get/{friendlyUrl}/{currentPage}")]
    public IActionResult Get (string friendlyUrl, int currentPage) {
      var result = _graphRepo.GetCatalog (friendlyUrl, currentPage);

      foreach (var book in result) {
        var user = _userRepo.GetUser (book.UserId);
        if (user != null)
          book.UserFriendlyUrl = user.AvatarPath;
      }

      return Ok (new { items = result.Items, totalNumber = result.TotalCount, currentPage = result.CurrentPage });

    }

    [HttpGet ("user-catalogs/{friendlyUrl}")]
    public IActionResult GetForUser (string friendlyUrl) {

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

    [HttpGet ("public-catalogs/{currentPage}")]
    public IActionResult GetPublicCatalogs (int currentPage = 0) {

      var result = _graphRepo.GetAllPublicCatalogs (currentPage);

      foreach (var item in result.Items) {
        foreach (var book in item.Books) {
          if (book.PhotoPath != null && book.PhotoPath.Contains ("cloudinary"))
            book.PhotoPath = CloudinaryHelper.TransformUrl (book.PhotoPath, TransformationType.Book_Details_Preset);
        }
      }

      return Ok (new { items = result.Items, totalNumber = result.TotalCount, currentPage = result.CurrentPage });
    }

    [HttpGet ("user-catalogs-pure-list/{friendlyUrl}")]
    public IActionResult GetPureCatalogsForUser (string friendlyUrl) {
      var catalogs = _graphRepo.GetPureCatalogs (UserId);
      return Ok (catalogs);
    }

    [HttpPost ("edit-catalog")]
    public IActionResult EditCatalog (CatalogEditDto catalog) {
      var result = _graphRepo.EditCatalog (catalog.Id, catalog.IsPublic, catalog.Name, UserId);
      return Ok (result);
    }
  }
}