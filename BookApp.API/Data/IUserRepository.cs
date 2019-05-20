using System.Collections.Generic;
using System.Threading.Tasks;
using BookApp.API.Dtos;
using BookApp.API.Helpers;
using BookApp.API.Models;

namespace BookApp.API.Data {
    public interface IUserRepository {

        void Add<T> (T entity) where T : class;
        void Delete<T> (T entity) where T : class;

        Task<bool> SaveAll ();

        Task<User> GetUser (string friendlyUrl);

        Task<User> GetUser (int userId);

        Task<UserFollowers> GetFollower (int followerId);

        UserFollowersDto FollowUser (int userIdToFollow, int userIdFollower);

        void UnfollowUser (int userIdToFollow, int userIdFollower);

        Task<PagedList<User>> GetUsers (UserParams userParams);

        Task<UserProfileDto> GetUserProfile (string friendlyUrl);

        Task<List<UserProfileDto>> GetAllProfiles ();

    }
}