using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BookApp.API.Data;
using BookApp.API.Dtos;
using BookApp.API.Helpers;
using BookApp.API.Models;
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
    private readonly IBookRepository _repo;
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
    private Cloudinary _cloudinary;
    private readonly IGraphRepository _bookGraph;

    public BookController (IBookRepository repo, DataContext context, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig, IGraphRepository bookGraph) {
      _repo = repo;
      _context = context;
      _mapper = mapper;
      _cloudinaryConfig = cloudinaryConfig;
      _bookGraph = bookGraph;

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

      var bookList = _bookGraph.GetAll ();
      // var books = await _repo.GetAll ();

      // if (books == null)
      //   return BadRequest ("No books");

      // foreach (var item in books) {
      //   item.PhotoPath = CloudinaryHelper.TransformUrl (item.PhotoPath, TransformationType.Book_Thumb_Preset);
      // }

      return Ok (bookList);
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

      // await _repo.ImportBooks ();
      // await _repo.ImportTags ();
      await _repo.ImportBookTags ();

      return Ok ();
    }

    [HttpGet ("get-books-added-by-user/{friendlyUrl?}")]
    public async Task<IActionResult> GetBooksAddedByUser (string friendlyUrl) {
      var books = await _repo.GetBooksAddedByUser (friendlyUrl);

      var bookToReturn = _mapper.Map<List<BookDetailsDto>> (books);
      foreach (var item in bookToReturn) {
        item.PhotoPath = CloudinaryHelper.TransformUrl (item.PhotoPath, TransformationType.Book_Details_Preset);
      }

      return Ok (bookToReturn);
    }

    [HttpPost ("add")]
    public async Task<IActionResult> Add (BookCreateDto bookDto) {
      var result = await _repo.AddBook (bookDto);

      var identity = HttpContext.User.Identity as ClaimsIdentity;
      int userId = 0;
      if (identity != null)
        userId = Int32.Parse (identity.FindFirst (ClaimTypes.NameIdentifier).Value);

      await _context.SaveChangesAsync ();

      bookDto.UserId = userId;
      bookDto.Id = result.Id;
      _bookGraph.AddBook (bookDto);

      return Ok (result);
    }

    [HttpGet ("delete-book-from-catalog/{catalogId}/{bookId}")]
    public async Task<IActionResult> RemoveBookFromCatalog (int catalogId, int bookId) {
      await _repo.RemoveBookFromCatalog (catalogId, bookId);
      await _context.SaveChangesAsync ();

      return Ok ();
    }
    #endregion

    #region BookToCatalog Actions
    [HttpPost ("add-to-catalog")]
    public async Task<IActionResult> AddBookToCatalog (BookCatalogCreateDto bookCatalogDto) {

      var identity = HttpContext.User.Identity as ClaimsIdentity;
      int userId = 0;
      if (identity != null)
        userId = Int32.Parse (identity.FindFirst (ClaimTypes.NameIdentifier).Value);

      bookCatalogDto.UserId = userId;
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
  }
}