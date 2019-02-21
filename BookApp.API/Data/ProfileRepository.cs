using System.Linq;
using System.Threading.Tasks;
using BookApp.API.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BookApp.API.Data
{
  public class ProfileRepository : IProfileRepository
  {

    private readonly DataContext _context;
    public ProfileRepository(DataContext context)
    {
      _context = context;
    }

    public async Task<UserProfileDto> Get(int id)
    {
      var user = await _context.Users.Include(userr => userr.Books).FirstOrDefaultAsync();

      var profile = new UserProfileDto
      {
        Id = user.Id,
        KnownAs = user.KnownAs,
        AvatarPath = user.AvatarPath,
        City = user.City,
        Country = user.Country,
        Email = user.Email,
        Introduction = user.Introduction,
        Books = user.Books
      };

      return profile;
    }

    public async Task<UserProfileDto> Update(UserProfileDto profile)
    {
      var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == profile.Id);

      // TOOD: make real update!
      user.Introduction = profile.Introduction;

      await _context.Users.AddAsync(user);
      await _context.SaveChangesAsync();

      // return really updated object!
      return profile;
    }
  }
}