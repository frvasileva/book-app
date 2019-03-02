using System.Security.Claims;
using System.Threading.Tasks;
using BookApp.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BookApp.API.Dtos;
using BookApp.API.Models;
using System;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using AutoMapper;


namespace DatingApp.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly IAuthRepository _repo;
    private readonly IConfiguration _config;
    private readonly IMapper _mapper;

    public AuthController(IAuthRepository repo, IConfiguration config, IMapper mapper)
    {
      _config = config;
      _repo = repo;
      _mapper = mapper;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
    {
      // TODO Uncomment later
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      userForRegisterDto.Email = userForRegisterDto.Email.ToLower();

      if (await _repo.UserExists(userForRegisterDto.Email))
        return BadRequest("Username already exists");

      var userToCreate = new User
      {
        Email = userForRegisterDto.Email
      };

      var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);

      var claims = new[]
      {
                new Claim(ClaimTypes.NameIdentifier, createdUser.Id.ToString()),
                new Claim(ClaimTypes.Name, createdUser.Email)
            };

      var key = new SymmetricSecurityKey(Encoding.UTF8
          .GetBytes(_config.GetSection("AppSettings:Token").Value));

      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.Now.AddDays(1),
        SigningCredentials = creds
      };

      var tokenHandler = new JwtSecurityTokenHandler();

      var token = tokenHandler.CreateToken(tokenDescriptor);

      return Ok(new
      {
        token = tokenHandler.WriteToken(token)
      });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
    {
      userForLoginDto.Email = userForLoginDto.Email.ToLower();

      var userFromRepo = await _repo.Login(userForLoginDto.Email, userForLoginDto.Password);

      if (userFromRepo == null)
        return Unauthorized();

      var claims = new[]
      {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Email)
            };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.Now.AddDays(1),
        SigningCredentials = creds
      };

      var tokenHandler = new JwtSecurityTokenHandler();

      var token = tokenHandler.CreateToken(tokenDescriptor);

      return Ok(new
      {
        token = tokenHandler.WriteToken(token)
      });
    }
  }
}