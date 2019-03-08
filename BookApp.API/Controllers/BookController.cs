using System.Threading.Tasks;
using BookApp.API.Data;
using BookApp.API.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using AutoMapper;

namespace BookApp.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class BookController : ControllerBase
  {
    private readonly IBookRepository _repo;
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public BookController(IBookRepository repo, DataContext context, IMapper mapper)
    {
      _repo = repo;
      _context = context;
      _mapper = mapper;
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
      var book = await _repo.GetBook(friendlyUrl);

      if (book == null)
        return BadRequest("No books");

      var bookToReturn = _mapper.Map<BookDetailsDto>(book);

      return Ok(bookToReturn);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add(BookCreateDto bookDto)
    {
      //TODO: Check if author exists 
      //TODO: If not -> create it
      //TODO: Else: asign it to the correct author

      await _repo.AddBook(bookDto);
      await _context.SaveChangesAsync();

      return Ok(bookDto);
    }

    [HttpPost("add-book-action")]
    public async Task<IActionResult> AddBookAction(BookActionDto bookActionDto)
    {
      await _repo.AddBookAction(bookActionDto);
      await _context.SaveChangesAsync();

      return Ok(bookActionDto);
    }
  }
}