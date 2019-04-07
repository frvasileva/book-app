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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BookApp.API.Controllers {

  [AllowAnonymous]
  [Route ("api/[controller]")]
  [ApiController]
  public class ProfileController : ControllerBase {
    private readonly IConfiguration _config;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
    private Cloudinary _cloudinary;

    public ProfileController (IConfiguration config, IMapper mapper,
      IUserRepository userRepository,
      IOptions<CloudinarySettings> cloudinaryConfig) {
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
    }

    [HttpGet ("get/{friendlyUrl}")]
    public async Task<IActionResult> GetUserProfile (string friendlyUrl) {
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

      // if (id != int.Parse (User.FindFirst (ClaimTypes.NameIdentifier).Value))
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
        return Ok (userFromRepo);
      }

      return BadRequest ("Could not add the photo");
    }
  }
}