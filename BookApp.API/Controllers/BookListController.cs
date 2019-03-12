using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
using BookApp.API.Data;
using BookApp.API.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BookApp.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class BookListController : ControllerBase
  {
    private readonly IBookListRepository _repo;
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public BookListController(IBookListRepository repo, DataContext context, IMapper mapper)
    {
      _repo = repo;
      _context = context;
      _mapper = mapper;
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add(BookListCreateDto bookListDto)
    {
      var result = await _repo.Create(bookListDto);
      await _context.SaveChangesAsync();

      return Ok(result);
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(int id)
    {
      var bookListItem = await _repo.Get(id);

      return Ok(bookListItem);
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
    {
      var bookListItems = await _repo.GetAll();

      return Ok(bookListItems);
    }
  }
}