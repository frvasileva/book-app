using System.Collections.Generic;
using System.Threading.Tasks;
using BookApp.API.Dtos;

namespace BookApp.API.Data {
  public interface IGraphRepository {
    List<BookDetailsDto> GetAll ();
    BookDetailsDto GetBook (string friendlyUrl);
    Task<List<BookPreviewDto>> GetBooksAddedByUser (string friendlyUrl);
    BookDetailsDto AddBook (BookCreateDto book);
    void AddCatalog (CatalogCreateDto book);
    List<CatalogPureDto> GetPureCatalogs (long userId);
    BookCatalogItemDto AddBookToCatalog (BookCatalogCreateDto item);

    #region User

    void RegisterUser (ProfileDto user);
    UserFollowersDto FollowUser (int userIdToFollow, int userIdFollower);
    UserFollowersDto UnfollowUser (int userIdToFollow, int userIdFollower);
    #endregion User

  }
}