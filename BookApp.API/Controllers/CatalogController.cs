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
  public class CatalogController : ControllerBase
  {
    private readonly ICatalogRepository _repo;
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public CatalogController(ICatalogRepository repo, DataContext context, IMapper mapper)
    {
      _repo = repo;
      _context = context;
      _mapper = mapper;
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add(CatalogCreateDto catalogCteateDto)
    {
      var result = await _repo.Create(catalogCteateDto);
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