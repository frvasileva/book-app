using AutoMapper;
using BookApp.API.Data;
using Microsoft.AspNetCore.Mvc;

namespace BookApp.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthorController : ControllerBase
  {
    private readonly IAuthorRepository _repo;
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public AuthorController(IAuthorRepository repo, DataContext context, IMapper mapper)
    {
      _repo = repo;
      _context = context;
      _mapper = mapper;
    }

    

  }
}