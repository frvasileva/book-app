using System;
using System.Threading.Tasks;
using BookApp.API.Helpers;
using BookApp.API.Models;
using Microsoft.EntityFrameworkCore;
using Neo4jClient;

namespace BookApp.API.Data {
    public class AuthRepository : IAuthRepository {
        private readonly DataContext _context;

        private readonly IGraphClient _graphClient;

        public AuthRepository (DataContext context, IGraphClient graphClient) {
            _context = context;
            _graphClient = graphClient;
            _graphClient.Connect ();
        }

        public async Task<User> Login (string email, string password) {
            var user = await _context.Users.Include (p => p.Photos).FirstOrDefaultAsync (x => x.Email == email);

            if (user == null)
                return null;
            return user;
        }

        public async Task<User> Register (User user, string password) {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash (password, out passwordHash, out passwordSalt);

            //Init default catalog on registering a new user
            var catalog = new Catalog ();
            catalog.Name = "Want to read";
            catalog.IsPublic = true;
            catalog.Created = DateTime.Now;
            catalog.FriendlyUrl = Url.GenerateFriendlyUrl (catalog.Name + "-" + StringHelper.GenerateRandomNo ());
            catalog.UserId = user.Id;

            await _context.Users.AddAsync (user);
         //   await _context.Catalogs.AddAsync (catalog);
            var result = await _context.SaveChangesAsync ();

            if (result > 0) {
                _graphClient.Cypher
                    .Create ("(profile:Profile {profileId})")
                    .WithParam ("profileId", user.Id).ExecuteWithoutResults ();
            }

            return user;
        }

        private void CreatePasswordHash (string password, out byte[] passwordHash, out byte[] passwordSalt) {
            using (var hmac = new System.Security.Cryptography.HMACSHA512 ()) {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash (System.Text.Encoding.UTF8.GetBytes (password));
            }
        }

        public async Task<bool> UserExists (string email) {
            if (await _context.Users.AnyAsync (x => x.Email == email))
                return true;

            return false;
        }
    }
}