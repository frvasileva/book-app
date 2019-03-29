using System.Threading.Tasks;
using BookApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BookApp.API.Data {
    public class UserRepository : IUserRepository {

        private readonly DataContext _context;

        public UserRepository (DataContext context) {
            _context = context;
        }
        public void Add<T> (T entity) where T : class {
            _context.Add (entity);
        }

        public void Delete<T> (T entity) where T : class {
            _context.Remove (entity);
        }

        public async Task<User> GetUser (string friendlyUrl) {
            var user = await _context.Users.Include (p => p.Books).FirstOrDefaultAsync (u => u.FriendlyUrl == friendlyUrl);

            return user;
        }

        public async Task<bool> SaveAll () {
            var res = await _context.SaveChangesAsync ();
            return res > 0;
        }
    }
}