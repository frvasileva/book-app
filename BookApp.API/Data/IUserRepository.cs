using System.Threading.Tasks;
using BookApp.API.Models;

namespace BookApp.API.Data {
    public interface IUserRepository {
        void Add<T> (T entity) where T : class;
        void Delete<T> (T entity) where T : class;
        Task<bool> SaveAll ();
        // Task<PagedList<User>> GetUsers (UserParams userParams);
        Task<User> GetUser (string friendlyUrl);
    }
}