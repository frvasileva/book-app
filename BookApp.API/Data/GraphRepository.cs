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

    public BookItemDto AddBook (BookCreateDto bookDto) {

      var bookItem = _mapper.Map<BookItemDto> (bookDto);
      bookItem.FriendlyUrl = Url.GenerateFriendlyUrl (bookDto.Title);

      var result = _graphClient.Cypher
        .Create ("(book:Book {bookDto})")
        .WithParam ("bookDto", bookItem).Return<Node<BookItemDto>> ("book").Results.Single ();

      _graphClient.Cypher
        .Match ("(profile:Profile)", "(book:Book)")
        .Where ((ProfileDto profile) => profile.Id == bookDto.UserId)
        .AndWhere ((BookItemDto book) => book.Id == bookDto.Id)
        .Create ("(profile)-[r:BOOK_ADDED {message}]->(book)")
        .WithParam ("message", new { dateAdded = DateTime.Now }).ExecuteWithoutResults ();

      return new BookItemDto ();
    }

    public void AddCatalog (CatalogCreateDto catalogDto) {
      var result = _graphClient.Cypher
        .Create ("(catalog:Catalog {catalog})")
        .WithParam ("catalog", catalogDto).Return<Node<CatalogCreateDto>> ("catalog").Results.Single ();

      _graphClient.Cypher
        .Match ("(profile:Profile)", "(catalog:Catalog)")
        .Where ((ProfileDto profile) => profile.Id == catalogDto.UserId)
        .AndWhere ((CatalogCreateDto catalog) => catalog.Id == catalogDto.Id)
        .Create ("(profile)-[r:CATALOG_ADDED {date}]->(catalog)")
        .WithParam ("date", new { dateAdded = DateTime.Now }).ExecuteWithoutResults ();
    }

    public void AddBookToCatalog (BookCatalogCreateDto item) {
      _graphClient.Cypher
        .Match ("(book:Book)", "(catalog:Catalog)")
        .Where ((BookItemDto book) => book.Id == item.BookId)
        .AndWhere ((CatalogCreateDto catalog) => catalog.Id == item.CatalogId)
        .Create ("(book)-[r:BOOK_ADDED_TO_CATALOG {info}]->(catalog)")
        .WithParam ("info", new { dateAdded = DateTime.Now, userId = item.UserId }).ExecuteWithoutResults ();
    }

    public Task<List<BookPreviewDto>> GetAll () {
      throw new NotImplementedException ();
    }

    public Task<BookDetailsDto> GetBook (string friendlyUrl) {
      throw new NotImplementedException ();

      var bookitem =
        _graphClient.Cypher.Match ("(book:Book)")
        .Where ((BookDetailsDto book) => book.FriendlyUrl == friendlyUrl)
        .Return (book => book.As<BookDetailsDto> ())
        .Results
        .Single ();
    }

    public Task<List<BookPreviewDto>> GetBooksAddedByUser (string friendlyUrl) {
      throw new NotImplementedException ();
    }

    public UserFollowersDto FollowUser (int userIdToFollow, int userIdFollower) {
      _graphClient.Cypher
        .Match ("(profile:Profile)", "(follower:Profile)")
        .Where ((ProfileDto profile) => profile.Id == userIdFollower)
        .AndWhere ((ProfileDto follower) => follower.Id == userIdToFollow)
        .Create ("(profile)-[r:FOLLOW_USER {date}]->(follower)")
        .WithParam ("date", new { dateAdded = DateTime.Now }).ExecuteWithoutResults ();

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
      _graphClient.Cypher
        .Create ("(profile:Profile {profileId})")
        .WithParam ("profileId", new { user.Id }).ExecuteWithoutResults ();
    }
  }
}