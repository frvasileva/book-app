using System.Collections.Generic;
using System.Linq;
using BookApp.API.Dtos;
using Neo4jClient.Cypher;

namespace BookApp.API.Data {
  public partial class GraphRepository : IGraphRepository {

    private const int MIN_BOOKS_ADDED_TO_CATALOGS = 30;
    private const int MIN_USERS_FOLLOWING = 30;

    public Helpers.PagedList<BookDetailsDto> RecommendationByRelevance (int currentPage, int userId) {

      var userBooksAddedToCatalogsCount = this.GetBooksAddedToCatalogs (userId);
      var userFollowingCount = this.GetUsersFollowingCount (userId);
      if (userBooksAddedToCatalogsCount > MIN_BOOKS_ADDED_TO_CATALOGS)
        return this.RecommendByFavoriteCategories (currentPage, userId);
      else
        return this.RecommendByBooksAddedCategories (currentPage, userId);

      if (userFollowingCount > MIN_USERS_FOLLOWING) {

      }
    }

    private Helpers.PagedList<BookDetailsDto> RecommendByFavoriteCategories (int currentPage, int userId) {
      var skipResults = currentPage * SHOW_MAX_RESULTS_PER_PAGE;

      var getFavCatalogs = GetFavoriteCatalogsForUser (userId);
      string combindedString = "[" + string.Join (",", getFavCatalogs.ToArray ()) + "]";

      var whereClause = "catalog.name in " + combindedString;
      var result =
        _graphClient.Cypher
        .Match ("(book:Book)-[r:BOOK_ADDED_TO_CATALOG]->(catalog:Catalog)")
        .Where (whereClause)
        .Return ((catalog, book, count) => new {
          catalogs = Return.As<IEnumerable<BookCatalogListDto>> ("collect({catalogId:catalog.id, name:catalog.name, friendlyUrl:catalog.friendlyUrl})"),
            bk = book.As<BookDetailsDto> ()
        })
        .Skip (skipResults).Limit (SHOW_MAX_RESULTS_PER_PAGE);

      var totalBooks =
        _graphClient.Cypher
        .Match ("(book:Book)-[r:BOOK_ADDED_TO_CATALOG]->(catalog:Catalog)")
        .Where (whereClause)
        .Return ((book) => new {
          Count = Return.As<int> ("count (distinct book.title)")
        });

      var totalBooksCount = totalBooks.Results.FirstOrDefault ().Count;

      var res = result.Results;
      var bookList = new List<BookDetailsDto> ();

      foreach (var b in result.Results) {
        var bd = b.bk;
        bd.RecommendationCategory = "RELEVANCE";
        foreach (var c in b.catalogs) {
          bd.BookCatalogs.Add (c);
        }

        bookList.Add (bd);
      }

      var pagedList = new Helpers.PagedList<BookDetailsDto> (bookList, totalBooksCount, currentPage, SHOW_MAX_RESULTS_PER_PAGE);
      return pagedList;
    }

    private Helpers.PagedList<BookDetailsDto> RecommendByBooksAddedCategories (int currentPage, int userId) {
      var whereClause = "currentUser.id = 116";
      var skipResults = currentPage * SHOW_MAX_RESULTS_PER_PAGE;

      var result = _graphClient.Cypher
        .Match ("(currentUser:Profile)-[:FOLLOW_USER]->(p2:Profile)")
        .Where (whereClause)
        .With (@"collect(distinct p2.id) as followersIds 
                 match (b:Book)-[r:BOOK_ADDED_TO_CATALOG]->(c:Catalog) 
                 where r.userId  in followersIds with  collect(b) as BookTitles")
        .Unwind ("BookTitles", "Books")
        .Return ((Books) => new {
          Count = Return.As<int> ("count (*)"),
            Books = Return.As<BookDetailsDto> ("Books")
        })
        .OrderByDescending ("Count")
        .Skip (skipResults).Limit (SHOW_MAX_RESULTS_PER_PAGE);

      var aaa = result.Results;

      var bookList = new List<BookDetailsDto> ();

      foreach (var b in result.Results) {
        var bd = b.Books;
        bd.RecommendationCategory = "RELEVANCE";

        bookList.Add (bd);
      }

      var pagedList = new Helpers.PagedList<BookDetailsDto> (bookList, 10, currentPage, SHOW_MAX_RESULTS_PER_PAGE);
      return pagedList;
    }

    // private Helpers.PagedList<BookDetailsDto> RecommendByBooksLikedByFollower (int currentPage, int userId) {}

    public Helpers.PagedList<BookDetailsDto> RecommendBySerendipity (int currentPage, int userId) {

      var skipResults = currentPage * SHOW_MAX_RESULTS_PER_PAGE;

      var result =
        _graphClient.Cypher
        .Match ("(book:Book)")
        .OptionalMatch ("(book:Book)-->(catalog:Catalog)")
        .Where ((BookDetailsDto book) => book.AvarageRating > 3)
        .Return ((catalog, book, rand) => new {
          catalogs = Return.As<IEnumerable<BookCatalogListDto>> ("collect({catalogId:catalog.id, name:catalog.name, friendlyUrl:catalog.friendlyUrl})"),
            bk = book.As<BookDetailsDto> ()
        })
        .OrderByDescending ("rand()")
        .Skip (skipResults).Limit (SHOW_MAX_RESULTS_PER_PAGE);

      var totalBooks =
        _graphClient.Cypher
        .Match ("(book:Book)")
        .OptionalMatch ("(book:Book)-->(catalog:Catalog)")
        .Where ((BookDetailsDto book) => book.AvarageRating > 3)
        .Return ((catalog, book, rand) => new {
          Count = Return.As<int> ("count (distinct book.title)")
        });

      var totalBooksCount = totalBooks.Results.FirstOrDefault ().Count;

      var res = result.Results;
      var bookList = new List<BookDetailsDto> ();

      foreach (var b in result.Results) {
        var bd = b.bk;

        bd.RecommendationCategory = "SERENDIPITY";
        foreach (var c in b.catalogs) {
          bd.BookCatalogs.Add (c);
        }
        bookList.Add (bd);
      }

      var pagedList = new Helpers.PagedList<BookDetailsDto> (bookList, totalBooksCount, currentPage, SHOW_MAX_RESULTS_PER_PAGE);

      return pagedList;
    }

    public Helpers.PagedList<BookDetailsDto> RecommendByNovelty (int currentPage, int userId) {

      var skipResults = currentPage * SHOW_MAX_RESULTS_PER_PAGE;

      var result =
        _graphClient.Cypher
        .Match ("(book:Book)")
        .OptionalMatch ("(book:Book)-->(catalog:Catalog)")
        .Return ((catalog, book, rand) => new {
          catalogs = Return.As<IEnumerable<BookCatalogListDto>> ("collect({catalogId:catalog.id, name:catalog.name, friendlyUrl:catalog.friendlyUrl})"),
            bk = book.As<BookDetailsDto> ()
        })
        .OrderByDescending ("book.addedOn")
        .Skip (skipResults).Limit (SHOW_MAX_RESULTS_PER_PAGE);

      var totalBooks =
        _graphClient.Cypher
        .Match ("(book:Book)")
        .OptionalMatch ("(book:Book)-->(catalog:Catalog)")
        .Return ((book) => new {
          Count = Return.As<int> ("count (distinct book.title)")
        });

      var totalBooksCount = totalBooks.Results.FirstOrDefault ().Count;

      var res = result.Results;
      var bookList = new List<BookDetailsDto> ();

      foreach (var b in result.Results) {
        var bd = b.bk;
        bd.RecommendationCategory = "NOVELTY";
        foreach (var c in b.catalogs) {
          bd.BookCatalogs.Add (c);
        }

        bookList.Add (bd);
      }
      var pagedList = new Helpers.PagedList<BookDetailsDto> (bookList, totalBooksCount, currentPage, SHOW_MAX_RESULTS_PER_PAGE);

      return pagedList;
    }
  }
}