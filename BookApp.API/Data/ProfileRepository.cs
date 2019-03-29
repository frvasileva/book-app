using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookApp.API.Dtos;
using BookApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

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

    public async Task<UserProfileDto> Get (string friendlyUrl) {

      var currentUser = await _context.Users.Include (itm => itm.Books).Where (item => item.FriendlyUrl == friendlyUrl).FirstOrDefaultAsync ();
      var mappedProfile = _mapper.Map<UserProfileDto> (currentUser);

      return mappedProfile;
    }

    public async Task<User> GetUser (string friendlyUrl) {

      var user = await _context.Users.FirstOrDefaultAsync (u => u.FriendlyUrl == friendlyUrl);
      return user;
    }

    public async Task<List<UserProfileDto>> GetAll () {

      var allUsers = await _context.Users.Include (item => item.Books).OrderByDescending (u => u.Created).ToListAsync ();

      List<UserProfileDto> userList = _mapper.Map<List<User>, List<UserProfileDto>> (allUsers);
      return userList;
    }

    public async Task<bool> SaveAll () {
      return await _context.SaveChangesAsync () > 0;
    }
  }
}