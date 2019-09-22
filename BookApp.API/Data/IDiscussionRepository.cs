using System.Collections.Generic;
using System.Threading.Tasks;
using BookApp.API.Dtos;
using BookApp.API.Models;

namespace BookApp.API.Data {
    public interface IDiscussionRepository {
        Discussion Create (Discussion model);
        DiscussionItem CreateDiscussionItem (DiscussionItem model);
        void Add<T> (T entity) where T : class;
        void Edit<T> (T entity) where T : class;
        void Delete<T> (T entity) where T : class;

        DiscussionDetailsDto GetDiscussion (string friendlyUrl);
        List<Discussion> GetDiscussions ();
        List<Discussion> GetDiscussionsByBook (int bookId);
        List<Discussion> GetDiscussionsByUser (int userId);

        Task<bool> SaveAll ();

    }
}