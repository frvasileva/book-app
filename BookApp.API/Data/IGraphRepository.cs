using System.Collections.Generic;
using System.Threading.Tasks;
using BookApp.API.Dtos;

namespace BookApp.API.Data {
  public interface IGraphRepository {
    Task<List<BookPreviewDto>> GetAll ();
    BookDetailsDto GetBook (string friendlyUrl);
    Task<List<BookPreviewDto>> GetBooksAddedByUser (string friendlyUrl);
    BookItemDto AddBook (BookCreateDto book);
    void AddCatalog (CatalogCreateDto book);
    void AddBookToCatalog (BookCatalogCreateDto item);

    #region User

    void RegisterUser (ProfileDto user);
    UserFollowersDto FollowUser (int userIdToFollow, int userIdFollower);
    UserFollowersDto UnfollowUser (int userIdToFollow, int userIdFollower);
    #endregion User

  }
}