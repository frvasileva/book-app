using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BookApp.API.Dtos;
using BookApp.API.Helpers;
using BookApp.API.Models;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace BookApp.API.Data {
  public partial class GraphRepository : IGraphRepository {

    #region Recommendations

    public List<string> GetFavoriteCatalogsForUser (int userId) {
      var result = _graphClient.Cypher
        .Match ("(catalog:Catalog)")
        .OptionalMatch ("(book:Book)-[r:BOOK_ADDED_TO_CATALOG]->(catalog:Catalog)")
        .With ("book, catalog")
        .Where ((CatalogItemDto catalog) => catalog.UserId == userId)
        .ReturnDistinct ((catalog) => new { catalogs = Return.As<string> ("{name:catalog.name }") });

      var rerere = result.Results;
      var strings = new List<string> ();
      foreach (var itm in result.Results) {
        var item = itm.catalogs.Replace ("{\r\n  \"name\": \"", "").Replace ("\"\r\n}", "");
        strings.Add ("'" + item + "'");
      }

      return strings;
    }
    public Helpers.PagedList<BookDetailsDto> RecommendationByRelevance (int currentPage, int userId) {

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

    #endregion Recommendations

  }
}