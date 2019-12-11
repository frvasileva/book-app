using System.Collections.Generic;
using System.Threading.Tasks;
using BookApp.API.Dtos;
using BookApp.API.Models;

namespace BookApp.API.Data {
    public interface IUserRepository {

        Task<bool> SaveAll ();

        User GetUser (string friendlyUrl);

        User GetUser (int userId);

        UserFollowersDto FollowUser (int userIdToFollow, int userIdFollower);

        void UnfollowUser (int userIdToFollow, int userIdFollower);

        Task<UserProfileDto> GetUserProfile (string friendlyUrl, int currentUserId);

        Task<List<UserProfileDto>> GetAllProfiles (int currentUserId);

        Task<List<BookCatalogPreferences>> GetCatalogForPreferences ();
    }
}