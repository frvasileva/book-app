using System.Threading.Tasks;
using BookApp.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BookApp.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class BookController : ControllerBase
  {
    private readonly IConfiguration _config;
    private readonly IBookRepository _repo;
    public BookController(IConfiguration config, IBookRepository repo)
    {
      _repo = repo;
      _config = config;
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


  }
}