using System;
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
    private readonly IGraphRepository _graphRepo;

    private int UserId {
      get {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        int userId = 0;
        if (identity != null)
          userId = Int32.Parse (identity.FindFirst (ClaimTypes.NameIdentifier).Value);

        return userId;
      }
    }
    private string UserFriendlyUrl {
      get {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        string friendlyUrl = "";
        if (identity != null)
          friendlyUrl = identity.FindFirst (ClaimTypes.Name).Value;

        return friendlyUrl;
      }
    }

    public ProfileController (IConfiguration config, IMapper mapper,
      IUserRepository userRepository,
      IOptions<CloudinarySettings> cloudinaryConfig,
      IHttpContextAccessor httpContextAccessor,
      IGraphRepository graphRepo) {
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

      _graphRepo = graphRepo;

    }

    [HttpGet ("get/{friendlyUrl}")]
    public async Task<IActionResult> GetUserProfile (string friendlyUrl) {

      var profile = await _userRepository.GetUserProfile (friendlyUrl, UserId);

      if (profile == null)
        return BadRequest (string.Format ("No user with such friendlyUrl {0}", friendlyUrl));

      if (!String.IsNullOrEmpty (profile.AvatarPath))
        profile.AvatarPath = CloudinaryHelper.TransformUrl (profile.AvatarPath, TransformationType.User_Thumb_Preset);;

      return Ok (profile);
    }

    [HttpGet ("get-all")]
    public async Task<IActionResult> GetAllProfiles () {
      var profileList = await _userRepository.GetAllProfiles (UserId);

      return Ok (profileList);
    }

    [HttpPost ("edit-user")]
    public async Task<IActionResult> Update (UserProfileEditDto profileForUpdate) {

      var userFromRepo = _userRepository.GetUser (profileForUpdate.FriendlyUrl);

      _mapper.Map (profileForUpdate, userFromRepo);

      if (await _userRepository.SaveAll ())
        return Ok (userFromRepo);

      throw new Exception ($"Updating user {profileForUpdate.FriendlyUrl} failed on save");
    }

    [HttpPost ("add-photo/{friendlyUrl}")]
    public async Task<IActionResult> AddPhotoForUser (string friendlyUrl, [FromForm] PhotoForCreationDto photoForCreationDto) {

      var userFromRepo = _userRepository.GetUser (friendlyUrl);
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
        return Ok (new { avararPath = userFromRepo.AvatarPath, friendlyUrl = userFromRepo.FriendlyUrl });
      }

      return BadRequest ("Could not add the photo");
    }

    [HttpGet ("follow-user/{userIdToFollow}")]
    public IActionResult FollowUser (int userIdToFollow) {

      var result = _graphRepo.FollowUser (userIdToFollow, UserId);
      var followUser = _userRepository.FollowUser (userIdToFollow, UserId);

      return Ok (followUser);
    }

    [HttpGet ("unfollow-user/{userIdToFollow}")]
    public IActionResult UnfollowUser (int userIdToFollow) {

      _graphRepo.UnfollowUser (userIdToFollow, UserId);
      _userRepository.UnfollowUser (userIdToFollow, UserId);

      return Ok ();
    }

    [HttpPost ("add-book-catalog-preferences")]
    public IActionResult BookCatalogPreferences (string[] bookCatalogPreferencesIds) {

      foreach (var item in bookCatalogPreferencesIds) {
        var catalogItemDto = new CatalogCreateDto {
          Name = item,
          IsPublic = true,
          UserId = UserId,
          FriendlyUrl = BookApp.API.Helpers.Url.GenerateFriendlyUrl (item + "-" + UserId)
        };

        _graphRepo.AddCatalog (catalogItemDto, true);
      }
      return Ok (new CatalogCreateDto ());
    }

    [HttpGet ("get-preferences-catalog-list")]
    public async Task<IActionResult> GetDefaultCatalogForPreferences () {
      var result = await _userRepository.GetCatalogForPreferences ();
      return Ok (result);
    }

    [HttpGet ("get-user-selected-preferences-catalog-list")]
    public IActionResult GetUserSelectedCatalogForPreferences () {
      var result = _graphRepo.GetFavoriteCatalogsForUser_Enriched (UserId);
      var defaultCategories = _userRepository.GetCatalogForPreferences ();
      return Ok (new { userSelectedCategories = result, defaultCategories = defaultCategories.Result });
    }

    [HttpGet ("toggle-preferences-catalog/{catalogId}/{catalogName}/{isSelected}")]
    public IActionResult TogglePreferencedCatalogs (int catalogId, string catalogName, int isSelected) {
      var selected = isSelected == 1;

      if (catalogId > 0) {
        _graphRepo.ToggleUserCatalogFromFavorites (UserId, catalogId, catalogName, selected);
      } else {
        var catalogItemDto = new CatalogCreateDto {
          Name = catalogName,
          IsPublic = true,
          UserId = UserId,
          FriendlyUrl = BookApp.API.Helpers.Url.GenerateFriendlyUrl (catalogName + "-" + Guid.NewGuid ())
        };

        _graphRepo.AddCatalog (catalogItemDto, true);
      }
      return Ok ();
    }
  }
}