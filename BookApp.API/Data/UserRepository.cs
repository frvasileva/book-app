using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookApp.API.Dtos;
using BookApp.API.Helpers;
using BookApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BookApp.API.Data {
    public class UserRepository : IUserRepository {

        private readonly DataContext _context;

        private readonly IMapper _mapper;

        private readonly IGraphRepository _graphRepository;

        public UserRepository (DataContext context, IMapper mapper, IGraphRepository graphRepository) {
            _context = context;
            _mapper = mapper;
            _graphRepository = graphRepository;
        }

        public UserFollowersDto FollowUser (int userIdToFollow, int currentUserId) {

            UserFollowers followUser = new UserFollowers ();
            followUser.FollowerUserId = userIdToFollow;
            followUser.UserId = currentUserId;
            followUser.Created = DateTime.Now;

            _context.Add (followUser);

            var user = _context.Users.Where (item => item.Id == userIdToFollow).ToList ();

            var followerDto = new UserFollowersDto () {
                Id = followUser.Id,
                FollowerUserId = currentUserId,
                UserId = userIdToFollow,
                FollowerFriendlyUrl = user.FirstOrDefault ().FriendlyUrl
            };

            _context.SaveChangesAsync ();

            return followerDto;
        }

        public void UnfollowUser (int userIdToFollow, int userIdFollower) {

            var followerRelation = _context.UserFollowers.Where (item => item.FollowerUserId == userIdToFollow && item.UserId == userIdFollower).ToList ();
            var itm = followerRelation.FirstOrDefault ();

            if (itm != null) {
                _context.Remove (itm);
                _context.SaveChanges ();
            }
        }

        public async Task<List<UserProfileDto>> GetAllProfiles (int currentUserId) {

            var allUsers = await _context.Users.Include (user => user.Books).Where (item => item.Id != currentUserId).OrderByDescending (u => u.Created).ToListAsync ();
            var mappedUsers = _mapper.Map<List<User>, List<UserProfileDto>> (allUsers);
            var userFollowers = await _context.UserFollowers.Where (item => item.UserId == currentUserId).ToListAsync ();

            foreach (var user in mappedUsers) {
                foreach (var follower in userFollowers) {
                    if (user.Id == follower.FollowerUserId) {
                        user.IsFollowedByCurrentUser = true;
                    }
                }
            }
            return mappedUsers;
        }

        public User GetUser (string friendlyUrl) {
            var user = _context.Users.ToList ().Where (item => item.FriendlyUrl == friendlyUrl).FirstOrDefault ();
            return user;
        }

        public User GetUser (int userId) {
            var user = _context.Users.ToList ().Where (item => item.Id == userId).FirstOrDefault ();
            return user;
        }

        public async Task<UserProfileDto> GetUserProfile (string friendlyUrl, int currentUserId) {
            var userProfile = await _context.Users.Include (itm => itm.Books).Where (item => item.FriendlyUrl == friendlyUrl).FirstOrDefaultAsync ();
            var mappedProfile = _mapper.Map<UserProfileDto> (userProfile);

            var userFollowers = await _context.UserFollowers.Where (itm => itm.FollowerUserId == userProfile.Id && itm.UserId == currentUserId).ToListAsync ();
            if (userFollowers.Count > 0) {
                mappedProfile.IsFollowedByCurrentUser = true;
            }

            mappedProfile.ProfileActivities.UsersFollowingCount = 5;
            mappedProfile.ProfileActivities.BooksAddedToCatalogsCount = 5;

            return mappedProfile;
        }

        // TODO Refactor this method to get default values from db?! Move it to another repository
        public async Task<List<BookCatalogPreferences>> GetCatalogForPreferences () {
            var result = await _context.BookCatalogPreferences.ToListAsync ();

            return result;
        }

        public async Task<bool> SaveAll () {
            var res = await _context.SaveChangesAsync ();
            return res > 0;
        }

    }
}