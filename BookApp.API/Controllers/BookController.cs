using System.Threading.Tasks;
using BookApp.API.Data;
using BookApp.API.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BookApp.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class BookController : ControllerBase
  {
    private readonly IBookRepository _repo;

    private readonly DataContext _context;

    public BookController(IBookRepository repo, DataContext context)
    {
      _repo = repo;
      _context = context;
    }

    [HttpGet("get-books")]
    public async Task<IActionResult> GetAllBooks()
    {
      var books = await _repo.GetAll();

      if (books == null)
        return BadRequest("No books");

      return Ok(books);
    }

    [HttpGet("get/{friendlyUrl?}")]
    public async Task<IActionResult> GetBook(string friendlyUrl)
    {
      var books = await _repo.GetBook(friendlyUrl);

      if (books == null)
        return BadRequest("No books");

      return Ok(books);
    }


    [HttpPost("add")]
    public async Task<IActionResult> Add(BookCreateDto bookDto)
    {
      await _repo.AddBook(bookDto);
      await _context.SaveChangesAsync();

      return Ok(bookDto);
    }

  }
}