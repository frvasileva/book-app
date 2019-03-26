using System.Threading.Tasks;
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
    public ProfileController (IConfiguration config, IProfileRepository repo) {
      _repo = repo;
      _config = config;
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

    [HttpGet ("update/{userId}")]
    public async Task<IActionResult> Update (UserProfileDto profileForUpdate) {

      await _repo.Update (profileForUpdate);
      //TODO - create real profile update
      return Ok (profileForUpdate);
    }
  }
}