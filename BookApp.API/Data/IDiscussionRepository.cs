using System.Collections.Generic;
using BookApp.API.Dtos;
using BookApp.API.Models;

namespace BookApp.API.Data {
    public interface IDiscussionRepository {
        Discussion Create (Discussion model);
        DiscussionItemDto CreateDiscussionItem (DiscussionItem model);
        DiscussionDetailsDto GetDiscussion (string friendlyUrl);
        List<DiscussionDetailsDto> GetDiscussions (int? bookId, int? userId);
        List<DiscussionDetailsDto> GetDiscussionsByBook (int bookId);
        List<DiscussionDetailsDto> GetDiscussionsByUser (int userId);
    }
}