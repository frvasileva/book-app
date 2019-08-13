using System.Collections.Generic;
using BookApp.API.Dtos;

namespace BookApp.API.Data {
  public interface IGraphRepository {
    List<BookDetailsDto> GetAll ();
    BookDetailsDto GetBook (string friendlyUrl);
    List<BookDetailsDto> GetBooksAddedByUser (int userId);
    BookDetailsDto AddBook (BookCreateDto book);
    BookDetailsDto AddBookCover (int bookId, string photoPath);
    CatalogCreateDto AddCatalog (CatalogCreateDto catalogDto, bool isFavorite);
    List<CatalogPureDto> GetPureCatalogs (long userId);

    BookCatalogItemDto AddBookToCatalog (BookCatalogCreateDto item);
    BookCatalogItemDto RemoveBookToCatalog (int catalogId, int bookId);

    List<CatalogItemDto> GetCatalogsForUser (int userId, bool isCurrentUser);

    List<CatalogItemDto> GetAllPublicCatalogs ();
    List<CatalogItemDto> GetCatalog (string friendlyUrl);

    #region User

    void RegisterUser (ProfileDto user);
    UserFollowersDto FollowUser (int userIdToFollow, int userIdFollower);
    UserFollowersDto UnfollowUser (int userIdToFollow, int userIdFollower);
    #endregion User

  }
}