using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BookApp.API.Data;
using BookApp.API.Dtos;
using BookApp.API.Helpers;
using BookApp.API.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace BookApp.API.Controllers {
  
  [Authorize]
  [Route ("api/[controller]")]
  [ApiController]
  public class ProfileController : ControllerBase {
    private readonly IConfiguration _config;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
    private Cloudinary _cloudinary;

    private IHttpContextAccessor _httpContextAccessor;

    public ProfileController (IConfiguration config, IMapper mapper,
      IUserRepository userRepository,
      IOptions<CloudinarySettings> cloudinaryConfig,
      IHttpContextAccessor httpContextAccessor) {
      _config = config;
      _mapper = mapper;
      _userRepository = userRepository;
      _cloudinaryConfig = cloudinaryConfig;

      Account acc = new Account (
        _cloudinaryConfig.Value.CloudName,
        _cloudinaryConfig.Value.ApiKey,
        _cloudinaryConfig.Value.ApiSecret
      );

      _cloudinary = new Cloudinary (acc);

      _httpContextAccessor = httpContextAccessor;

      var authenticatedUser = _httpContextAccessor.HttpContext.User.Identity.Name;

    }

    [AllowAnonymous]
    [HttpGet ("get/{friendlyUrl}")]
    public async Task<IActionResult> GetUserProfile (string friendlyUrl) {

      var jwtHandler = new JwtSecurityTokenHandler ();
      var jwtInput = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxNiIsInVuaXF1ZV9uYW1lIjoidGVvZG9yLXVybCIsIm5iZiI6MTU1ODYwMzk5NSwiZXhwIjoxNTU4NjkwMzk1LCJpYXQiOjE1NTg2MDM5OTV9.SyJdoiW7RFJoBcTdi9zriaRUBq4XMrk-KNlh7YUBuDAoyeAdY4xv62xggIUi1SzL2cseuBdiZhjEJgmtY7YWag";
      var outputText = "";
      var readableToken = jwtHandler.CanReadToken (jwtInput);

      if (readableToken != true) {
        outputText = "The token doesn't seem to be in a proper JWT format.";
      }
      if (readableToken == true) {
        var token = jwtHandler.ReadJwtToken (jwtInput);

        //Extract the headers of the JWT
        var headers = token.Header;
        var jwtHeader = "{";
        foreach (var h in headers) {
          jwtHeader += '"' + h.Key + "\":\"" + h.Value + "\",";
        }
        jwtHeader += "}";
        outputText = "Header:\r\n" + JToken.Parse (jwtHeader).ToString ();

        //Extract the payload of the JWT
        var claims = token.Claims;
        var jwtPayload = "{";
        foreach (Claim c in claims) {
          jwtPayload += '"' + c.Type + "\":\"" + c.Value + "\",";
        }
        jwtPayload += "}";
        outputText += "\r\nPayload:\r\n" + JToken.Parse (jwtPayload).ToString ();
      }

      var profile = await _userRepository.GetUserProfile (friendlyUrl);

      if (profile == null)
        return BadRequest (string.Format ("No user with such friendlyUrl {0}", friendlyUrl));

      return Ok (profile);
    }

    [HttpGet ("get-all")]
    public async Task<IActionResult> GetAllProfiles () {
      var profileList = await _userRepository.GetAllProfiles ();

      return Ok (profileList);
    }

    [HttpPost ("edit-user")]
    public async Task<IActionResult> Update (UserProfileEditDto profileForUpdate) {

      var userrr = User.FindFirst (ClaimTypes.NameIdentifier).Value;
      //   return Unauthorized ();

      var userFromRepo = await _userRepository.GetUser (profileForUpdate.FriendlyUrl);

      _mapper.Map (profileForUpdate, userFromRepo);

      if (await _userRepository.SaveAll ())
        return Ok (userFromRepo);

      throw new Exception ($"Updating user {profileForUpdate.FriendlyUrl} failed on save");
    }

    [HttpPost ("add-photo/{friendlyUrl}")]
    public async Task<IActionResult> AddPhotoForUser (string friendlyUrl, [FromForm] PhotoForCreationDto photoForCreationDto) {

      //  if (userId != int.Parse (User.FindFirst (ClaimTypes.NameIdentifier).Value))
      //      return Unauthorized ();

      var userFromRepo = await _userRepository.GetUser (friendlyUrl);

      var file = photoForCreationDto.File;

      var uploadResult = new ImageUploadResult ();

      if (file.Length > 0) {
        using (var stream = file.OpenReadStream ()) {
          var uploadParams = new ImageUploadParams () {
          File = new FileDescription (file.Name, stream),
          Transformation = new Transformation ()
          .Width (500).Height (500).Crop ("fill").Gravity ("face")
          };

          uploadResult = _cloudinary.Upload (uploadParams);
        }
      }

      photoForCreationDto.Url = uploadResult.Uri.ToString ();
      photoForCreationDto.PublicId = uploadResult.PublicId;
      userFromRepo.AvatarPath = uploadResult.Uri.ToString ();

      var photo = _mapper.Map<Photo> (photoForCreationDto);
      userFromRepo.Photos.Add (photo);

      if (await _userRepository.SaveAll ()) {
        return Ok (userFromRepo.AvatarPath);
      }

      return BadRequest ("Could not add the photo");
    }

    [HttpGet ("follow-user/{userIdToFollow}/{userIdFollower}")]
    public async Task<IActionResult> FollowUser (int userIdToFollow, int userIdFollower) {

      var result = _userRepository.FollowUser (userIdFollower, userIdToFollow);

      return Ok (result);
    }

    [HttpGet ("unfollow-user/{userIdToFollow}/{userIdFollower}")]
    public async Task<IActionResult> UnfollowUser (int userIdToFollow, int userIdFollower) {

      _userRepository.UnfollowUser (userIdToFollow, userIdFollower);

      if (await _userRepository.SaveAll ()) {
        return Ok ();
      }

      return BadRequest ("Could not delete the following relation");
    }
  }
}