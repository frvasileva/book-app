using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookApp.API.Helpers;
using BookApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BookApp.API.Data {
    public class AuthRepository : IAuthRepository {
        private readonly DataContext _context;
        public AuthRepository (DataContext context) {
            _context = context;
        }

        public AuthRepository () { }

        public async Task<User> Login (string email, string password) {
            var user = await _context.Users.Include (p => p.Photos).FirstOrDefaultAsync (x => x.Email == email);

            if (user == null)
                return null;

            // if (!VerifyPasswordHash (password, user.PasswordHash, user.PasswordSalt))
            //     return null;

            return user;
        }

        private bool VerifyPasswordHash (string password, byte[] passwordHash, byte[] passwordSalt) {
            using (var hmac = new System.Security.Cryptography.HMACSHA512 (passwordSalt)) {
                var computedHash = hmac.ComputeHash (System.Text.Encoding.UTF8.GetBytes (password));
                for (int i = 0; i < computedHash.Length; i++) {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
                return true;
            }
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
            await _context.Catalogs.AddAsync (catalog);
            await _context.SaveChangesAsync ();

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