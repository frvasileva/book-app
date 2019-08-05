using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookApp.API.Dtos;
using BookApp.API.Helpers;
using Neo4jClient;

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
        .WithParam ("bookDto", bookItem).Return<Node<BookDetailsDto>> ("book").Results.Single ();

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

    public void AddCatalog (CatalogCreateDto catalogDto) {
      var result = _graphClient.Cypher
        .Create ("(catalog:Catalog:Favorite {catalog})")
        .WithParam ("catalog", catalogDto)
        .Return<Node<CatalogCreateDto>> ("catalog").Results.Single ();

      _graphClient.Cypher
        .Match ("(profile:Profile)", "(catalog:Catalog)")
        .Where ((ProfileDto profile) => profile.Id == catalogDto.UserId)
        .AndWhere ((CatalogCreateDto catalog) => catalog.Id == catalogDto.Id)
        .CreateUnique ("(profile)-[r:CATALOG_ADDED {date}]->(catalog)")
        .WithParam ("date", new { addedOn = DateTime.Now }).ExecuteWithoutResults ();
    }

    public void AddBookToCatalog (BookCatalogCreateDto item) {
      _graphClient.Cypher
        .Match ("(book:Book)", "(catalog:Catalog)")
        .Where ((BookItemDto book) => book.Id == item.BookId)
        .AndWhere ((CatalogCreateDto catalog) => catalog.Id == item.CatalogId)
        .Create ("(book)-[r:BOOK_ADDED_TO_CATALOG {info}]->(catalog)")
        .WithParam ("info", new { addedOn = DateTime.Now, userId = item.UserId }).ExecuteWithoutResults ();
    }

    public Task<List<BookPreviewDto>> GetAll () {
      throw new NotImplementedException ();
    }

    public BookDetailsDto GetBook (string friendlyUrl) {
      var result =
        _graphClient.Cypher.Match ("(book:Book)")
        .Where ((BookDetailsDto book) => book.FriendlyUrl == friendlyUrl)
        .Return<Node<BookDetailsDto>> ("profile")
        .Results.Single ();

      var mappedResult = _mapper.Map<BookDetailsDto> (result.Data);

      return mappedResult;
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
      return catalogList;
    }

    public Task<List<BookPreviewDto>> GetBooksAddedByUser (string friendlyUrl) {
      throw new NotImplementedException ();
    }

    public UserFollowersDto FollowUser (int userIdToFollow, int userIdFollower) {
      var result = _graphClient.Cypher
        .Match ("(profile:Profile)", "(follower:Profile)")
        .Where ((ProfileDto profile) => profile.Id == userIdFollower)
        .AndWhere ((ProfileDto follower) => follower.Id == userIdToFollow)
        .Create ("(profile)-[r:FOLLOW_USER {date}]->(follower)")
        .WithParam ("date", new { addedOn = DateTime.Now });

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