using System.Collections.Generic;
using System.Threading.Tasks;
using BookApp.API.Dtos;

namespace BookApp.API.Data
{
  public interface IProfileRepository
  {
    Task<UserProfileDto> Get(int id);
    Task<List<UserProfileDto>> GetAll();
    Task<UserProfileDto> Update(UserProfileDto profile);
  }
}