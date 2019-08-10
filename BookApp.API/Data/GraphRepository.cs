using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookApp.API.Dtos;
using BookApp.API.Helpers;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace BookApp.API.Data {
  public class GraphRepository : IGraphRepository {
    private readonly IGraphClient _graphClient;
    private readonly IMapper _mapper;

    public GraphRepository (IGraphClient graphClient, IMapper mapper) {
      _graphClient = graphClient;
      _graphClient.Connect ();
      _mapper = mapper;
    }

    public BookDetailsDto AddBook (BookCreateDto bookDto) {

      var bookItem = _mapper.Map<BookDetailsDto> (bookDto);
      bookItem.FriendlyUrl = Url.GenerateFriendlyUrl (bookDto.Title);

      var result = _graphClient.Cypher
        .Create ("(book:Book {bookDto})")
        .WithParam ("bookDto", bookItem)
        .Return<Node<BookDetailsDto>> ("book").Results.Single ();

      var mappedResult = _mapper.Map<BookDetailsDto> (result.Data);

      _graphClient.Cypher
        .Match ("(profile:Profile)", "(book:Book)")
        .Where ((ProfileDto profile) => profile.Id == bookDto.UserId)
        .AndWhere ((BookItemDto book) => book.Id == bookDto.Id)
        .Create ("(profile)-[r:BOOK_ADDED {message}]->(book)")
        .WithParam ("message", new { addedOn = DateTime.Now })
        .ExecuteWithoutResults ();

      return mappedResult;
    }

    public BookDetailsDto AddBookCover (int bookId, string photoPath) {
      var result = _graphClient.Cypher
        .Match ("(book:Book)")
        .Where ((BookItemDto book) => book.Id == bookId)
        .WithParam ("photoPath", photoPath)
        .Set ("book.photoPath = {photoPath}")
        .Return<Node<BookDetailsDto>> ("book").Results.Single ();

      return result.Data;
    }

    public CatalogCreateDto AddCatalog (CatalogCreateDto catalogDto, bool isFavorite = false) {

      var result = new CatalogCreateDto ();

      if (isFavorite) {
        result = _graphClient.Cypher
          .Create ("(catalog:Catalog {catalog})")
          .WithParam ("catalog", catalogDto)
          .Return<Node<CatalogCreateDto>> ("catalog").Results.Single ().Data;
      } else {
        result = _graphClient.Cypher
          .Create ("(catalog:Catalog:Favorite {catalog})")
          .WithParam ("catalog", catalogDto)
          .Return<Node<CatalogCreateDto>> ("catalog").Results.Single ().Data;
      }

      _graphClient.Cypher
        .Match ("(profile:Profile)", "(catalog:Catalog)")
        .Where ((ProfileDto profile) => profile.Id == catalogDto.UserId)
        .AndWhere ((CatalogCreateDto catalog) => catalog.Id == catalogDto.Id)
        .CreateUnique ("(profile)-[r:CATALOG_ADDED {date}]->(catalog)")
        .WithParam ("date", new { addedOn = DateTime.Now }).ExecuteWithoutResults ();

      return result;
    }

    public BookCatalogItemDto AddBookToCatalog (BookCatalogCreateDto item) {
      var result = _graphClient.Cypher
        .Match ("(book:Book)", "(catalog:Catalog)")
        .Where ((BookItemDto book) => book.Id == item.BookId)
        .AndWhere ((CatalogCreateDto catalog) => catalog.Id == item.CatalogId)
        .Create ("(book)-[r:BOOK_ADDED_TO_CATALOG {info}]->(catalog)")
        .WithParam ("info", new { addedOn = DateTime.Now, userId = item.UserId })
        .Return ((catalog, book, r) => new {
          cat = catalog.As<CatalogCreateDto> (),
            bk = book.As<BookItemDto> ()
        });

      var bbb = result.Results.ToList ();

      var bookCatalog = new BookCatalogItemDto ();
      // bookCatalog.BookId = result.Results.ToList ().FirstOrDefault ().bk.Id;
      // bookCatalog.CatalogId = result.Results.ToList ().FirstOrDefault ().cat.Id;
      // bookCatalog.UserId = result.Results.ToList ().FirstOrDefault ().bk.UserId;
      // bookCatalog.IsPublic = result.Results.ToList ().FirstOrDefault ().cat.IsPublic;
      // bookCatalog.Name = result.Results.ToList ().FirstOrDefault ().cat.Name;

      return bookCatalog;
    }

    public BookCatalogItemDto RemoveBookToCatalog (int catalogId, int bookId) {
      _graphClient.Cypher
        .Match ("(book:Book)-[r:BOOK_ADDED_TO_CATALOG]->(catalog:Catalog)")
        .Where ((BookCatalogItemDto book) => book.Id == bookId)
        .AndWhere ((CatalogItemDto catalog) => catalog.Id == catalogId)
        .Delete ("r")
        .ExecuteWithoutResults ();

      return new BookCatalogItemDto ();
    }

    public List<BookDetailsDto> GetAll () {

      var bookDetails = new List<BookDetailsDto> ();

      var result =
        _graphClient.Cypher
        .Match ("(book:Book)")
        .OptionalMatch ("(book:Book)-->(catalog:Catalog)")
        .ReturnDistinct ((catalog, book) => new {
          catalogs = Return.As<IEnumerable<string>> ("collect([catalog.id])"),
            bk = book.As<BookDetailsDto> ()
        });

      var results = result.Results.ToList ();
      foreach (var itm in results) {
        var bookCatalogList = new List<BookCatalogListDto> ();
        var bookDetail = itm.bk;

        foreach (var i in itm.catalogs) { //[\r\n  1156930\r\n]
          if (i != "[\r\n  null\r\n]")
            bookCatalogList.Add (new BookCatalogListDto () { CatalogId = Int32.Parse (i.Replace ("[\r\n  ", "").Replace ("\r\n]", "")) });
        }

        bookDetail.BookCatalogs = bookCatalogList;
        bookDetails.Add (bookDetail);
      }
      bookDetails = bookDetails.OrderByDescending (item => item.CreatedOn).ToList ();
      return bookDetails;
    }

    public BookDetailsDto GetBook (string friendlyUrl) {

      var result = _graphClient.Cypher
        .Match ("(book:Book)")
        .OptionalMatch ("(book:Book)-->(catalog:Catalog)")
        .With ("book, catalog")
        .Where ((BookDetailsDto book) => book.FriendlyUrl == friendlyUrl)
        .ReturnDistinct ((catalog, book) => new {
          catalogs = Return.As<IEnumerable<string>> ("collect([catalog.id])"),
            bk = book.As<BookDetailsDto> ()
        });

      var item = result.Results.ToList ().FirstOrDefault ();
      var bookDetails = new BookDetailsDto ();

      if (item != null) {

        var bookCatalogList = new List<BookCatalogListDto> ();
        bookDetails = item.bk;

        foreach (var i in item.catalogs) {
          if (i != "[\r\n  null\r\n]")
            bookCatalogList.Add (new BookCatalogListDto () { CatalogId = Int32.Parse (i.Replace ("[\r\n  ", "").Replace ("\r\n]", "")) });
        }

        bookDetails.BookCatalogs = bookCatalogList;
      }

      return bookDetails;
    }

    public List<CatalogPureDto> GetPureCatalogs (long userId) {
      var result =
        _graphClient.Cypher.Match ("(catalog:Catalog)")
        .Where ((CatalogPureDto catalog) => catalog.UserId == userId)
        .Return ((catalog) => new {
          cat = catalog.As<CatalogPureDto> ()
        });
      var catalogList = new List<CatalogPureDto> ();

      var results = result.Results.ToList ();
      foreach (var item in results) {
        var itm = item.cat;
        catalogList.Add (itm);
      }
      catalogList = catalogList.OrderByDescending (item => item.Created).ToList ();
      return catalogList;
    }

    public List<BookPreviewDto> GetBooksAddedByUser (int userId) {
      var result = _graphClient.Cypher
        .Match ("(profile:Profile)-[r:BOOK_ADDED]->(book:Book)")
        .Where ((ProfileDto profile) => profile.Id == userId)
        .Return<BookPreviewDto> ("book").Results;

      return result.ToList();
    }

    public UserFollowersDto FollowUser (int userIdToFollow, int userIdFollower) {
      _graphClient.Cypher
        .Match ("(profile:Profile)", "(follower:Profile)")
        .Where ((ProfileDto profile) => profile.Id == userIdFollower)
        .AndWhere ((ProfileDto follower) => follower.Id == userIdToFollow)
        .CreateUnique ("(profile)-[r:FOLLOW_USER {date}]->(follower)")
        .WithParam ("date", new { addedOn = DateTime.Now }).ExecuteWithoutResults ();

      return new UserFollowersDto ();
    }

    public UserFollowersDto UnfollowUser (int userIdToFollow, int userIdFollower) {
      _graphClient.Cypher
        .Match ("(profile:Profile)-[r:FOLLOW_USER]->(follower:Profile)")
        .Where ((ProfileDto profile) => profile.Id == userIdFollower)
        .AndWhere ((ProfileDto follower) => follower.Id == userIdToFollow)
        .Delete ("r")
        .ExecuteWithoutResults ();

      return new UserFollowersDto ();
    }

    public void RegisterUser (ProfileDto user) {
      var result = _graphClient.Cypher
        .Create ("(profile:Profile {profile})")
        .WithParam ("profile", user)
        .Return<Node<ProfileDto>> ("profile")
        .Results.Single ();

      var mappedResult = _mapper.Map<ProfileDto> (result.Data);
    }

  }
}

// var results = client.Cypher
// .Match(
// "(actor:Person)-[:ACTED_IN]-&gt;(movie:Movie {title: {nameParam}})",
// "(movie)&lt;-[:DIRECTED]-(director:Person)"
// )
// .WithParam("nameParam", movieName)
// .Return((actor, director, movie) =&gt; new
// {
// Movie = movie.As&lt;Movie&gt;(),
// Actors = actor.CollectAs&lt;Person&gt;(),
// Director = director.As&lt;Person&gt;()
// })
// .Results.Single();

// ITransactionalGraphClient txClient = _graphClient;

// using (var tx = txClient.BeginTransaction ()) {
//   txClient.Cypher
//     .Match ("(m:Movie)")
//     .Where ((Movie m) = & m.title == originalMovieName)
//     .Set ("m.title = {newMovieNameParam}")
//     .WithParam ("newMovieNameParam", newMovieName)
//     .ExecuteWithoutResults ();

//   txClient.Cypher
//     .Match ("(m:Movie)")
//     .Where ((Movie m) = & gt; m.title == newMovieName)
//     .Create ("(p:Person {name: {actorNameParam}})-[:ACTED_IN]-&gt;(m)")
//     .WithParam ("actorNameParam", newActorName)
//     .ExecuteWithoutResults ();

//   tx.Commit ();
// }

//https://thenewstack.io/graph-database-neo4j-net-client/