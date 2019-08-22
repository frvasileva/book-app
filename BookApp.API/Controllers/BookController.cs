using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BookApp.API.Data;
using BookApp.API.Dtos;
using BookApp.API.Helpers;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BookApp.API.Controllers {

  [Route ("api/[controller]")]
  [ApiController]
  [AllowAnonymous]
  public class BookController : ControllerBase {

    private int UserId {
      get {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        int userId = 0;
        if (identity != null)
          userId = Int32.Parse (identity.FindFirst (ClaimTypes.NameIdentifier).Value);

        return userId;
      }
    }
    private readonly IBookRepository _repo;
    private readonly IUserRepository _userRepo;
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
    private Cloudinary _cloudinary;
    private readonly IGraphRepository _bookGraph;

    public BookController (IBookRepository repo, IUserRepository userRepo, DataContext context, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig, IGraphRepository bookGraph) {
      _repo = repo;
      _context = context;
      _mapper = mapper;
      _cloudinaryConfig = cloudinaryConfig;
      _bookGraph = bookGraph;
      _userRepo = userRepo;

      Account acc = new Account (
        _cloudinaryConfig.Value.CloudName,
        _cloudinaryConfig.Value.ApiKey,
        _cloudinaryConfig.Value.ApiSecret
      );

      _cloudinary = new Cloudinary (acc);
    }

    #region Book CRUD Actions

    [HttpGet ("get-books")]
    public async Task<IActionResult> GetAllBooks () {

      var books = _bookGraph.GetAll ();
      var booksss = _bookGraph.RecommendationByRelevance (UserId);

      foreach (var item in booksss) {
        if (item.PhotoPath != null && item.PhotoPath.Contains ("cloudinary"))
          item.PhotoPath = CloudinaryHelper.TransformUrl (item.PhotoPath, TransformationType.Book_Thumb_Preset);
      }

      return Ok (booksss);
    }

    [HttpGet ("recommend-relevance")]
    public async Task<IActionResult> RecommendationByRelevance () {

      var books = _bookGraph.RecommendationByRelevance (UserId);

      foreach (var item in books) {
        if (item.PhotoPath != null && item.PhotoPath.Contains ("cloudinary"))
          item.PhotoPath = CloudinaryHelper.TransformUrl (item.PhotoPath, TransformationType.Book_Thumb_Preset);
      }

      return Ok (books);
    }

    [HttpGet ("recommend-novelty")]
    public async Task<IActionResult> RecommendByNovelty () {

      var books = _bookGraph.RecommendByNovelty (UserId);

      foreach (var item in books) {
        if (item.PhotoPath != null && item.PhotoPath.Contains ("cloudinary"))
          item.PhotoPath = CloudinaryHelper.TransformUrl (item.PhotoPath, TransformationType.Book_Thumb_Preset);
      }

      return Ok (books);
    }

    [HttpGet ("recommend-serendipity")]
    public async Task<IActionResult> RecommendBySerendepity () {

      var books = _bookGraph.RecommendBySerendipity (UserId);

      foreach (var item in books) {
        if (item.PhotoPath != null && item.PhotoPath.Contains ("cloudinary"))
          item.PhotoPath = CloudinaryHelper.TransformUrl (item.PhotoPath, TransformationType.Book_Thumb_Preset);
      }

      return Ok (books);
    }

    [HttpGet ("get/{friendlyUrl?}")]
    public async Task<IActionResult> GetBook (string friendlyUrl) {
      var book = _bookGraph.GetBook (friendlyUrl);
      if (book == null)
        return BadRequest ("No books");

      var bookToReturn = _mapper.Map<BookDetailsDto> (book);
      bookToReturn.PhotoPath = CloudinaryHelper.TransformUrl (bookToReturn.PhotoPath, TransformationType.Book_Details_Preset);

      return Ok (bookToReturn);
    }

    [HttpGet ("import-books")]
    public async Task<IActionResult> ImportBooks () {

      //  _bookGraph.ImportBooks ();
      // var result =   _bookGraph.GetFavoriteCatalogsForUser (108);
      var result = _bookGraph.RecommendationByRelevance (UserId);
      return Ok (result);
    }

    [HttpGet ("import-categories")]
    public async Task<IActionResult> ImportCategories () {

      _bookGraph.ImportTags ();
      return Ok ();
    }

    [HttpGet ("import-books-categories")]
    public async Task<IActionResult> ImportBookCategories () {

      _bookGraph.ImportBookTags ();
      return Ok ();
    }

    [HttpGet ("get-books-added-by-user/{friendlyUrl?}")]
    public async Task<IActionResult> GetBooksAddedByUser (string friendlyUrl) {
      var user = _userRepo.GetUser (friendlyUrl);
      if (user == null) {
        return NotFound ();
      }

      var books = _bookGraph.GetBooksAddedByUser (user.Id);
      foreach (var item in books) {
        if (!String.IsNullOrEmpty (item.PhotoPath))
          item.PhotoPath = CloudinaryHelper.TransformUrl (item.PhotoPath, TransformationType.Book_Details_Preset);
      }
      return Ok (books);
    }

    [HttpPost ("add")]
    public async Task<IActionResult> Add (BookCreateDto bookDto) {

      bookDto.UserId = UserId;
      var result = _bookGraph.AddBook (bookDto);

      return Ok (result);
    }

    [HttpGet ("delete-book-from-catalog/{catalogId}/{bookId}")]
    public async Task<IActionResult> RemoveBookFromCatalog (int catalogId, int bookId) {
      _bookGraph.RemoveBookToCatalog (catalogId, bookId);

      return Ok ();
    }
    #endregion

    #region BookToCatalog Actions
    [HttpPost ("add-to-catalog")]
    public async Task<IActionResult> AddBookToCatalog (BookCatalogCreateDto bookCatalogDto) {

      bookCatalogDto.UserId = UserId;
      _bookGraph.AddBookToCatalog (bookCatalogDto);
      return Ok ();
    }

    #endregion

    [HttpPost ("add-photo/{friendlyUrl}")]
    public async Task<IActionResult> AddPhotoForBook (string friendlyUrl, [FromForm] PhotoForCreationDto photoForCreationDto) {

      var book = _bookGraph.GetBook (friendlyUrl);
      var file = photoForCreationDto.File;

      var uploadResult = new ImageUploadResult ();

      if (file.Length > 0) {
        using (var stream = file.OpenReadStream ()) {
          var uploadParams = new ImageUploadParams () {
          File = new FileDescription (file.Name, stream),
          Transformation = new Transformation ().Width (500).Crop ("fill").Gravity ("face")
          };

          uploadResult = _cloudinary.Upload (uploadParams);
        }
      }

      book.PhotoPath = uploadResult.Uri.ToString ();

      var res = _bookGraph.AddBookCover (book.Id, book.PhotoPath);
      if (res != null) {
        return Ok (new { book.FriendlyUrl, book.PhotoPath });
      }

      return BadRequest ("Could not add the photo");
    }

    // private int UserId () {
    //   var identity = HttpContext.User.Identity as ClaimsIdentity;
    //   int userId = 0;
    //   if (identity != null)
    //     userId = Int32.Parse (identity.FindFirst (ClaimTypes.NameIdentifier).Value);

    //   return userId;
    // }
  }
}