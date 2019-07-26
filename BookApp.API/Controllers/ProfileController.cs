using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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
    }

    [HttpGet ("get/{friendlyUrl}")]
    public async Task<IActionResult> GetUserProfile (string friendlyUrl) {

      var profile = await _userRepository.GetUserProfile (friendlyUrl);

      if (profile == null)
        return BadRequest (string.Format ("No user with such friendlyUrl {0}", friendlyUrl));

      if (!String.IsNullOrEmpty (profile.AvatarPath))
        profile.AvatarPath = CloudinaryHelper.TransformUrl (profile.AvatarPath, TransformationType.User_Thumb_Preset);;

      return Ok (profile);
    }

    [HttpGet ("get-all")]
    public async Task<IActionResult> GetAllProfiles () {
      var profileList = await _userRepository.GetAllProfiles ();

      return Ok (profileList);
    }

    [HttpPost ("edit-user")]
    public async Task<IActionResult> Update (UserProfileEditDto profileForUpdate) {

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

    [HttpGet ("follow-user/{userIdToFollow}")]
    public IActionResult FollowUser (int userIdToFollow) {

      var identity = HttpContext.User.Identity as ClaimsIdentity;
      int userId = 0;
      if (identity != null)
        userId = Int32.Parse (identity.FindFirst (ClaimTypes.NameIdentifier).Value);

      var result = _userRepository.FollowUser (userIdToFollow, userId);

      return Ok (result);
    }

    [HttpGet ("unfollow-user/{userIdToFollow}")]
    public async Task<IActionResult> UnfollowUser (int userIdToFollow) {

       _userRepository.UnfollowUser (userIdToFollow, GetUserId ());

      return Ok ();
    }

    [HttpPost ("add-book-catalog-preferences")]
    public async Task<IActionResult> BookCatalogPreferences (List<BookCatalogPreferencesDto> bookCatalogPreferences) {

      
      return Ok ();
    }

    [HttpGet ("get-catalog-for-preferences")]
    public async Task<IActionResult> GetCatalogForPreferences () {
      var result = await _userRepository.GetCatalogForPreferences ();
      return Ok (result);
    }

    private int GetUserId () {
      var identity = HttpContext.User.Identity as ClaimsIdentity;

      int userId = 0;
      if (identity != null)
        userId = Int32.Parse (identity.FindFirst (ClaimTypes.NameIdentifier).Value);

      return userId;
    }
  }
}