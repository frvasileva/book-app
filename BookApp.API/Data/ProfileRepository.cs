using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookApp.API.Dtos;
using BookApp.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookApp.API.Data {
  public class ProfileRepository : IProfileRepository {
    private readonly DataContext _context;

    private UserManager<User> _userManager;

    private readonly IMapper _mapper;

    public ProfileRepository (DataContext context, UserManager<User> userManager, IMapper mapper) {
      _context = context;
      _userManager = userManager;
      _mapper = mapper;
    }

    public async Task<UserProfileDto> Get (int id) {

      var userrrr = await _context.Users.Include (userr => userr.Books).Where (us => us.Id == id).FirstOrDefaultAsync ();

      var user = await _userManager.FindByIdAsync (id.ToString ());

      var mappedProfile = _mapper.Map<UserProfileDto> (user);
      var mappedProfile2 = _mapper.Map<UserProfileDto> (userrrr);

      return mappedProfile;
    }

    public async Task<UserProfileDto> Update (UserProfileDto profile) {
      var user = await _context.Users.FirstOrDefaultAsync (x => x.Id == profile.Id);

      // TOOD: make real update!
      user.Introduction = profile.Introduction;

      await _context.Users.AddAsync (user);
      await _context.SaveChangesAsync ();

      // return really updated object!
      return profile;
    }
  }
}