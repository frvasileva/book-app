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

    public BookController (IBookRepository repo, DataContext context, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig) {
      _repo = repo;
      _context = context;
      _mapper = mapper;
      _cloudinaryConfig = cloudinaryConfig;

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
      var books = await _repo.GetAll ();

      if (books == null)
        return BadRequest ("No books");

      return Ok (books);
    }

    [HttpGet ("get/{friendlyUrl?}")]
    public async Task<IActionResult> GetBook (string friendlyUrl) {
      var book = await _repo.GetBook (friendlyUrl);

      if (book == null)
        return BadRequest ("No books");

      var bookToReturn = _mapper.Map<BookDetailsDto> (book);

      return Ok (bookToReturn);
    }

    [HttpPost ("add")]
    public async Task<IActionResult> Add (BookCreateDto bookDto) {
      var result = await _repo.AddBook (bookDto);
      await _context.SaveChangesAsync ();

      return Ok (result);
    }

    [HttpPost ("add-book-action")]
    public async Task<IActionResult> AddBookAction (BookActionDto bookActionDto) {
      await _repo.AddBookAction (bookActionDto);
      await _context.SaveChangesAsync ();

      //TODO: fix to return really updated result!
      return Ok (bookActionDto);
    }

    [HttpPost ("delete-book-action/{bookId}")]
    public async Task<IActionResult> DeleteBookAction (int bookId) {
      await _repo.DeleteBookAction (bookId);
      await _context.SaveChangesAsync ();

      return Ok ();
    }
    #endregion

    #region BookToCatalog Actions
    [HttpPost ("add-to-catalog")]
    public async Task<IActionResult> AddBookToCatalog (BookCatalogCreateDto bookCatalogDto) {

      await _repo.AddBookToCatalog (bookCatalogDto);
      await _context.SaveChangesAsync ();

      return Ok ();
    }

    #endregion

    [HttpPost ("add-photo/{friendlyUrl}")]
    public async Task<IActionResult> AddPhotoForUser (string friendlyUrl, [FromForm] PhotoForCreationDto photoForCreationDto) {

      var book = await _repo.Get (friendlyUrl);
      var file = photoForCreationDto.File;

      var uploadResult = new ImageUploadResult ();

      if (file.Length > 0) {
        using (var stream = file.OpenReadStream ()) {
          var uploadParams = new ImageUploadParams () {
          File = new FileDescription (file.Name, stream),
          Transformation = new Transformation ()
          .Width (500).Crop ("fill").Gravity ("face")
          };

          uploadResult = _cloudinary.Upload (uploadParams);
        }
      }

      photoForCreationDto.Url = uploadResult.Uri.ToString ();
      photoForCreationDto.PublicId = uploadResult.PublicId;
      book.PhotoPath = uploadResult.Uri.ToString ();

      var photo = _mapper.Map<Photo> (photoForCreationDto);

      if (await _repo.SaveAll ()) {
        return Ok (book.PhotoPath);
      }

      return BadRequest ("Could not add the photo");
    }
  }
}