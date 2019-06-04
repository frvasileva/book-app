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

        public UserRepository (DataContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }
        public void Add<T> (T entity) where T : class {
            _context.Add (entity);
        }

        public void Delete<T> (T entity) where T : class {
            _context.Remove (entity);
        }

        public UserFollowersDto FollowUser (int userIdToFollow, int userIdFollower) {

            UserFollowers followUser = new UserFollowers ();
            followUser.FollowerUserId = userIdToFollow;
            followUser.UserId = userIdFollower;
            followUser.Created = DateTime.Now;

            _context.Add (followUser);

            var user = _context.Users.Where (item => item.Id == userIdToFollow).ToList ();

            var followerDto = new UserFollowersDto () {
                Id = followUser.Id,
                FollowerUserId = userIdFollower,
                UserId = userIdToFollow,
                FollowerFriendlyUrl = user.FirstOrDefault ().FriendlyUrl
            };

            _context.SaveChangesAsync ();

            return followerDto;
        }

        public void UnfollowUser (int userIdToFollow, int userIdFollower) {

            var followerRelation = _context.UserFollowers.Where (item => item.FollowerUserId ==userIdToFollow  && item.UserId == userIdFollower).ToList ();
            var itm = followerRelation.FirstOrDefault ();

            if (itm != null) {
                _context.Remove (itm);
                _context.SaveChanges ();
            }
        }

        public async Task<List<UserProfileDto>> GetAllProfiles () {
            var allUsers = await _context.Users.Include (item => item.Books).OrderByDescending (u => u.Created).ToListAsync ();

            List<UserProfileDto> userList = _mapper.Map<List<User>, List<UserProfileDto>> (allUsers);
            return userList;
        }

        public async Task<User> GetUser (string friendlyUrl) {
            var user = await _context.Users.Include (p => p.Books).FirstOrDefaultAsync (u => u.FriendlyUrl == friendlyUrl);

            return user;
        }

        public async Task<User> GetUser (int userId) {
            var user = await _context.Users.Include (p => p.Books).FirstOrDefaultAsync (u => u.Id == userId);

            return user;
        }

        public async Task<UserProfileDto> GetUserProfile (string friendlyUrl) {
            var currentUser = await _context.Users.Include (itm => itm.Books).Where (item => item.FriendlyUrl == friendlyUrl).FirstOrDefaultAsync ();
            var mappedProfile = _mapper.Map<UserProfileDto> (currentUser);

            return mappedProfile;
        }

        public Task<PagedList<User>> GetUsers (UserParams userParams) {
            throw new System.NotImplementedException ();
        }

        public async Task<UserFollowers> GetFollower (int followerId) {
            var result = await _context.UserFollowers.Where (item => item.Id == followerId).FirstOrDefaultAsync ();
            return result;
        }

        public async Task<bool> SaveAll () {
            var res = await _context.SaveChangesAsync ();
            return res > 0;
        }
    }
}