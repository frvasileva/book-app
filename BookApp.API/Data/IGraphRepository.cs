using System.Collections.Generic;
using System.Threading.Tasks;
using BookApp.API.Dtos;

namespace BookApp.API.Data {
  public interface IGraphRepository {
    List<BookDetailsDto> GetAll ();
    BookDetailsDto GetBook (string friendlyUrl);
    List<BookPreviewDto> GetBooksAddedByUser (int userId);
    BookDetailsDto AddBook (BookCreateDto book);
    BookDetailsDto AddBookCover (int bookId, string photoPath);
    CatalogCreateDto AddCatalog (CatalogCreateDto catalogDto, bool isFavorite);
    List<CatalogPureDto> GetPureCatalogs (long userId);

    BookCatalogItemDto AddBookToCatalog (BookCatalogCreateDto item);
    BookCatalogItemDto RemoveBookToCatalog (int catalogId, int bookId);

    #region User

    void RegisterUser (ProfileDto user);
    UserFollowersDto FollowUser (int userIdToFollow, int userIdFollower);
    UserFollowersDto UnfollowUser (int userIdToFollow, int userIdFollower);
    #endregion User

  }
}