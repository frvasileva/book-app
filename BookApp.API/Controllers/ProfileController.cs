using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BookApp.API.Data;
using BookApp.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BookApp.API.Controllers {

  [AllowAnonymous]
  [Route ("api/[controller]")]
  [ApiController]
  public class ProfileController : ControllerBase {
    private readonly IConfiguration _config;
    private readonly IProfileRepository _repo;

    private readonly IMapper _mapper;
    public ProfileController (IConfiguration config, IProfileRepository repo, IMapper mapper) {
      _repo = repo;
      _config = config;
      _mapper = mapper;
    }

    [HttpGet ("get/{friendlyUrl}")]
    public async Task<IActionResult> GetUserProfile (string friendlyUrl) {
      var profile = await _repo.Get (friendlyUrl);

      if (profile == null)
        return BadRequest (string.Format ("No user with such friendlyUrl {0}", friendlyUrl));

      return Ok (profile);
    }

    [HttpGet ("get-all")]
    public async Task<IActionResult> GetAllProfiles () {
      var profileList = await _repo.GetAll ();

      return Ok (profileList);
    }

    [HttpPost ("edit-user")]
    public async Task<IActionResult> Update (UserProfileEditDto profileForUpdate) {

      // if (id != int.Parse (User.FindFirst (ClaimTypes.NameIdentifier).Value))
      //   return Unauthorized ();

      var userFromRepo = await _repo.GetUser (profileForUpdate.FriendlyUrl);

      _mapper.Map (profileForUpdate, userFromRepo);

      if (await _repo.SaveAll ())
        return NoContent ();

      throw new Exception ($"Updating user {profileForUpdate.FriendlyUrl} failed on save");
    }
  }
}