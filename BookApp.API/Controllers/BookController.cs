using System;
using System.Security.Claims;
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
    private readonly IUserRepository _userRepo;
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
    private Cloudinary _cloudinary;
    private readonly IGraphRepository _bookGraph;

    public BookController (IUserRepository userRepo, DataContext context, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig, IGraphRepository bookGraph) {
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

    [HttpGet ("get-books/{currentPage}")]
    public IActionResult GetAllBooks (int currentPage) {

      var books = _bookGraph.GetAll ();
      var booksss = _bookGraph.RecommendationByRelevance (currentPage, UserId);

      foreach (var item in booksss) {
        if (item.PhotoPath != null && item.PhotoPath.Contains ("cloudinary"))
          item.PhotoPath = CloudinaryHelper.TransformUrl (item.PhotoPath, TransformationType.Book_Thumb_Preset);
      }

      return Ok (booksss);
    }

    [AllowAnonymous]
    [HttpGet ("recommend-similiar-book/{similiarToFriendlyUrl}")]
    public IActionResult RecommendSimiliarBooks (string similiarToFriendlyUrl) {
      var res = _bookGraph.RecommendSimiliarBooks (similiarToFriendlyUrl);

      return Ok (res);
    }

    [HttpGet ("recommend-relevance/{currentPage}")]
    public IActionResult RecommendationByRelevance (int currentPage = 0) {
      var result = _bookGraph.RecommendationByRelevance (currentPage, UserId);
      foreach (var item in result.Items) {
        if (item.PhotoPath != null && item.PhotoPath.Contains ("cloudinary"))
          item.PhotoPath = CloudinaryHelper.TransformUrl (item.PhotoPath, TransformationType.Book_Thumb_Preset);
      }

      return Ok (new { items = result.Items, totalNumber = result.TotalCount, currentPage = result.CurrentPage });
    }

    [HttpGet ("recommend-novelty/{currentPage}")]
    public IActionResult RecommendByNovelty (int currentPage = 0) {

      var result = _bookGraph.RecommendByNovelty (currentPage, UserId);

      foreach (var item in result.Items) {
        if (item.PhotoPath != null && item.PhotoPath.Contains ("cloudinary"))
          item.PhotoPath = CloudinaryHelper.TransformUrl (item.PhotoPath, TransformationType.Book_Thumb_Preset);
      }

      return Ok (new { items = result.Items, totalNumber = result.TotalCount, currentPage = result.CurrentPage });
    }

    [HttpGet ("recommend-serendipity/{currentPage}")]
    public IActionResult RecommendBySerendepity (int currentPage) {

      var result = _bookGraph.RecommendBySerendipity (currentPage, UserId);

      foreach (var item in result.Items) {
        if (item.PhotoPath != null && item.PhotoPath.Contains ("cloudinary"))
          item.PhotoPath = CloudinaryHelper.TransformUrl (item.PhotoPath, TransformationType.Book_Thumb_Preset);
      }

      return Ok (new { items = result.Items, totalNumber = result.TotalCount, currentPage = result.CurrentPage });
    }

    [HttpGet ("get/{friendlyUrl?}")]
    public IActionResult GetBook (string friendlyUrl) {
      var book = _bookGraph.GetBook (friendlyUrl);
      if (book == null)
        return BadRequest ("No books");

      var bookToReturn = _mapper.Map<BookDetailsDto> (book);
      if (bookToReturn.PhotoPath != null && bookToReturn.PhotoPath.Contains ("cloudinary"))
        bookToReturn.PhotoPath = CloudinaryHelper.TransformUrl (bookToReturn.PhotoPath, TransformationType.Book_Details_Preset);

      return Ok (bookToReturn);
    }

    [HttpGet ("import-books")]
    public IActionResult ImportBooks () {
      _bookGraph.ImportBooks ();
      return Ok ();
    }

    [HttpGet ("import-categories")]
    public IActionResult ImportCategories () {

      _bookGraph.ImportTags ();
      return Ok ();
    }

    [HttpGet ("import-books-categories")]
    public IActionResult ImportBookCategories () {

      _bookGraph.ImportBookTags ();
      return Ok ();
    }

    [HttpGet ("get-books-added-by-user/{friendlyUrl?}")]
    public IActionResult GetBooksAddedByUser (string friendlyUrl) {
      var user = _userRepo.GetUser (friendlyUrl);
      if (user == null) {
        return NotFound ();
      }

      var books = _bookGraph.GetBooksAddedByUser (user.Id);
      foreach (var item in books) {
        if (item.PhotoPath != null && item.PhotoPath.Contains ("cloudinary"))
          item.PhotoPath = CloudinaryHelper.TransformUrl (item.PhotoPath, TransformationType.Book_Details_Preset);
      }
      return Ok (books);
    }

    [HttpPost ("add")]
    public IActionResult Add (BookCreateDto bookDto) {

      bookDto.UserId = UserId;
      var result = _bookGraph.AddBook (bookDto);

      return Ok (result);
    }

    [HttpGet ("delete-book-from-catalog/{catalogId}/{bookId}")]
    public IActionResult RemoveBookFromCatalog (int catalogId, int bookId) {
      _bookGraph.RemoveBookToCatalog (catalogId, bookId);

      return Ok ();
    }
    #endregion

    #region BookToCatalog Actions
    [HttpPost ("add-to-catalog")]
    public IActionResult AddBookToCatalog (BookCatalogCreateDto bookCatalogDto) {
      bookCatalogDto.UserId = UserId;
      var result = _bookGraph.AddBookToCatalog (bookCatalogDto);

      return Ok (result);
    }

    #endregion

    [HttpPost ("add-photo/{friendlyUrl}")]
    public IActionResult AddPhotoForBook (string friendlyUrl, [FromForm] PhotoForCreationDto photoForCreationDto) {

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

  }
}