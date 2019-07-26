using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BookApp.API.Data;
using BookApp.API.Dtos;
using BookApp.API.Helpers;
using BookApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers {
  [AllowAnonymous]
  [Route ("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase {
    private readonly IConfiguration _config;
    private readonly IMapper _mapper;

    private UserManager<User> _userManager;
    private SignInManager<User> _signInManager;
    private ICatalogRepository _catalogRepo;

    public AuthController (IConfiguration config, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, ICatalogRepository catalogRepo) {
      _config = config;
      _mapper = mapper;
      _userManager = userManager;
      _signInManager = signInManager;
      _catalogRepo = catalogRepo;
    }

    [HttpPost ("register")]
    [AllowAnonymous]

    public async Task<IActionResult> Register (UserForRegisterDto userForRegisterDto) {

      var userToCreate = _mapper.Map<User> (userForRegisterDto);
      userToCreate.UserName = userForRegisterDto.Email;
      userToCreate.FriendlyUrl = BookApp.API.Helpers.Url.GenerateFriendlyUrl (userForRegisterDto.Email);
      userToCreate.KnownAs = userForRegisterDto.KnownAs;
      userToCreate.AvatarPath = "https://res.cloudinary.com/deumq4qrd/image/upload/v1563181144/book-avatar-anonymous.png";
      userToCreate.Created = DateTime.Now;
      
      var result = await _userManager.CreateAsync (userToCreate, userForRegisterDto.Password);
      var userToReturn = _mapper.Map<UserProfileDto> (userToCreate);
      var user = _mapper.Map<User> (userToReturn);

      if (result.Succeeded) {

        var defaultCatalog = new CatalogCreateDto ();
        defaultCatalog.Name = "Want to read";
        defaultCatalog.IsPublic = true;
        defaultCatalog.UserId = user.Id;
        _catalogRepo.Create (defaultCatalog);

        return Ok (new {
          token = GenerateJwtToken (user)
        });
      }

      return BadRequest (result.Errors);
    }

    [HttpPost ("login")]
    public async Task<IActionResult> Login (UserForLoginDto userForLoginDto) {

      userForLoginDto.Email = userForLoginDto.Email.ToLower ();

      var user = await _userManager.FindByEmailAsync (userForLoginDto.Email);

      var result = await _signInManager.CheckPasswordSignInAsync (user, userForLoginDto.Password, false);

      if (result.Succeeded) {

        user.LastActive = DateTime.UtcNow;
        await _userManager.UpdateAsync (user);

        var appUser = await _userManager.Users.Include (p => p.Books).FirstOrDefaultAsync (u => u.NormalizedEmail == userForLoginDto.Email.ToUpper ());
        var userToReturn = _mapper.Map<UserProfileDto> (appUser);

        return Ok (new {
          token = GenerateJwtToken (appUser),
            user = userToReturn
        });
      }

      return Unauthorized ();
    }

    private string GenerateJwtToken (User user) {

      var claims = new [] {
        new Claim (ClaimTypes.NameIdentifier, user.Id.ToString ()),
        new Claim (ClaimTypes.Name, user.FriendlyUrl)
      };

      var key = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (_config.GetSection ("AppSettings:Token").Value));
      var creds = new SigningCredentials (key, SecurityAlgorithms.HmacSha512Signature);

      var tokenDescriptor = new SecurityTokenDescriptor {
        Subject = new ClaimsIdentity (claims),
        Expires = DateTime.Now.AddDays (10),
        SigningCredentials = creds
      };

      var tokenHandler = new JwtSecurityTokenHandler ();
      var token = tokenHandler.CreateToken (tokenDescriptor);

      return tokenHandler.WriteToken (token);
    }
  }
}