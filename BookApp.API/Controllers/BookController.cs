using System;
using System.Threading.Tasks;
using AutoMapper;
using BookApp.API.Data;
using BookApp.API.Dtos;
using BookApp.API.Helpers;
using BookApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BookApp.API.Controllers {

  [Route ("api/[controller]")]
  [ApiController]
  [AllowAnonymous]
  public class BookController : ControllerBase {
    private readonly IBookRepository _repo;
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public BookController (IBookRepository repo, DataContext context, IMapper mapper) {
      _repo = repo;
      _context = context;
      _mapper = mapper;
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
  }
}