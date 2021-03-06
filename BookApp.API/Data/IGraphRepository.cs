using System.Collections.Generic;
using BookApp.API.Dtos;
using BookApp.API.Models;

namespace BookApp.API.Data {
  public interface IGraphRepository {
    List<BookDetailsDto> GetAll ();
    BookDetailsDto GetBook (string friendlyUrl);
    BookDetailsDto GetBookInfo (int bookId);
    List<BookDetailsDto> GetBooksAddedByUser (int userId);
    BookDetailsDto AddBook (BookCreateDto book);
    BookDetailsDto AddBook (Book book);
    BookDetailsDto AddBookCover (int bookId, string photoPath);
    CatalogCreateDto AddCatalog (CatalogCreateDto catalogDto, bool isFavorite);
    List<CatalogPureDto> GetPureCatalogs (long userId);

    int GetBooksAddedToCatalogs (long userId);
    int GetUsersFollowingCount (long userId);
    BookCatalog AddBookToCatalog (BookCatalogCreateDto item);
    BookCatalogItemDto RemoveBookToCatalog (int catalogId, int bookId, int userId);

    void ToggleUserCatalogFromFavorites (int userId, int catalogId, string catalogName, bool IsSelected);
    List<CatalogItemDto> GetCatalogsForUser (int userId, bool isCurrentUser);

    List<UserBookCategoriesPreferencesDto> GetFavoriteCatalogsForUser_Enriched (int userId);

    Helpers.PagedList<CatalogItemDto> GetAllPublicCatalogs (int currentPage = 0);
    Helpers.PagedList<CatalogItemDto> GetCatalog (string friendlyUrl, int currentPage = 0);

    CatalogEditDto EditCatalog (int catalogId, bool isPublic, string name, int userId);
    #region User

    void RegisterUser (ProfileDto user);
    UserFollowersDto FollowUser (int userIdToFollow, int userIdFollower);
    UserFollowersDto UnfollowUser (int userIdToFollow, int userIdFollower);
    #endregion User

    #region ImportData
    void ImportBooks ();
    void ImportBookTags ();
    void ImportTags ();
    #endregion ImportData

    #region Recommendations
    Helpers.PagedList<BookDetailsDto> RecommendationByRelevance (int currentPage, int userId);
    Helpers.PagedList<BookDetailsDto> RecommendBySerendipity (int currentPage);
    Helpers.PagedList<BookDetailsDto> RecommendByNovelty (int currentPage);
    List<BookDetailsDto> RecommendSimiliarBooks (string bookFriendlyUrl);
    List<string> GetFavoriteCatalogsForUser (int userId);
    #endregion Recommendations
  }
}