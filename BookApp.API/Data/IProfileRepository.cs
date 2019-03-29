using System.Collections.Generic;
using System.Threading.Tasks;
using BookApp.API.Dtos;
using BookApp.API.Models;

namespace BookApp.API.Data {
  public interface IProfileRepository {

    Task<UserProfileDto> Get (string friendlyUrl);

    Task<User> GetUser (string friendlyUrl);

    Task<List<UserProfileDto>> GetAll ();

    Task<bool> SaveAll ();

  }
}