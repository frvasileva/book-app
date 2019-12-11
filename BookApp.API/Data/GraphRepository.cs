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
    private readonly IGraphClient _graphClient;
    private readonly IMapper _mapper;
    private readonly int SHOW_MAX_RESULTS_PER_PAGE = 18;

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

    public BookDetailsDto AddBook (Book bookItem) {

      var result = _graphClient.Cypher
        .Create ("(book:Book {bookDto})")
        .WithParam ("bookDto", bookItem)
        .Return<Node<BookDetailsDto>> ("book").Results.Single ();

      var mappedResult = _mapper.Map<BookDetailsDto> (result.Data);

      _graphClient.Cypher
        .Match ("(profile:Profile)", "(book:Book)")
        .Where ((ProfileDto profile) => profile.Id == bookItem.UserId)
        .AndWhere ((BookItemDto book) => book.Id == bookItem.Id)
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

      if (!isFavorite) {
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

    public BookCatalog AddBookToCatalog (BookCatalogCreateDto item) {
      var result = _graphClient.Cypher
        .Match ("(book:Book)", "(catalog:Catalog)")
        .Where ((BookItemDto book) => book.Id == item.BookId)
        .AndWhere ((CatalogCreateDto catalog) => catalog.Id == item.CatalogId)
        .Create ("(book)-[r:BOOK_ADDED_TO_CATALOG {info}]->(catalog)")
        .WithParam ("info", new { addedOn = DateTime.Now, userId = item.UserId })
        .Return<CatalogCreateDto> ("catalog").Results.Single ();

      var bookCatalogEntity = new BookCatalog () {
        BookId = item.BookId,
        Created = DateTime.Now,
        UserId = item.UserId,
        CatalogId = result.Id
      };

      return bookCatalogEntity;
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
          catalogs = Return.As<IEnumerable<BookCatalogListDto>> ("collect({catalogId:catalog.id, name:catalog.name, friendlyUrl:catalog.friendlyUrl})"),
            bk = book.As<BookDetailsDto> ()
        })
        .Limit (50);

      var results = result.Results.ToList ();
      foreach (var itm in results) {
        var bookCatalogList = new List<BookCatalogListDto> ();
        var bookDetail = itm.bk;

        foreach (var cat in itm.catalogs) { //[\r\n  1156930\r\n]
          bookDetail.BookCatalogs.Add (cat);
        }

        bookDetail.BookCatalogs = bookCatalogList;
        bookDetails.Add (bookDetail);
      }
      bookDetails = bookDetails.OrderByDescending (item => item.AddedOn).ToList ();
      return bookDetails;
    }

    public BookDetailsDto GetBook (string friendlyUrl) {

      var result = _graphClient.Cypher
        .Match ("(book:Book)")
        .OptionalMatch ("(book:Book)-->(catalog:Catalog)")
        .With ("book, catalog")
        .Where ((BookDetailsDto book) => book.FriendlyUrl == friendlyUrl)
        .ReturnDistinct ((catalog, book) => new {
          catalogs = Return.As<IEnumerable<BookCatalogListDto>> ("collect({catalogId:catalog.id, name:catalog.name, friendlyUrl:catalog.friendlyUrl})[..20]"),
            bk = book.As<BookDetailsDto> ()
        });

      var item = result.Results.ToList ().FirstOrDefault ();
      var bookDetails = new BookDetailsDto ();

      if (item != null) {

        var bookCatalogList = new List<BookCatalogListDto> ();
        bookDetails = item.bk;

        foreach (var cat in item.catalogs) {
          bookDetails.BookCatalogs.Add (cat);
        }

        return bookDetails;
      } else return new BookDetailsDto ();
    }

    public BookDetailsDto GetBookInfo (int bookId) {

      var result = _graphClient.Cypher
        .Match ("(book:Book)")
        .Where ((BookDetailsDto book) => book.Id == bookId)
        .ReturnDistinct ((book) => new {
          bk = book.As<BookDetailsDto> ()
        });

      var item = result.Results.ToList ().FirstOrDefault ();
      if (item != null) {
        var bookDetails = new BookDetailsDto ();

        return item.bk;
      }
      return new BookDetailsDto ();
    }

    public List<CatalogPureDto> GetPureCatalogs (long userId) {
      var result =
        _graphClient.Cypher.Match ("(catalog:Catalog)-[r:CATALOG_ADDED]-(profile:Profile)")
        .Where ((ProfileDto profile) => profile.Id == userId)
        .Return ((catalog) => new {
          cat = catalog.As<CatalogPureDto> ()
        });
      var catalogList = new List<CatalogPureDto> ();

      var results = result.Results.ToList ();
      foreach (var item in results) {
        var itm = item.cat;
        catalogList.Add (itm);
      }

      catalogList = catalogList.OrderByDescending (item => item.Name == "Want to read").ThenBy (item => item.Created).ToList ();

      return catalogList;
    }

    public List<BookDetailsDto> GetBooksAddedByUser (int userId) {
      var bookDetails = new List<BookDetailsDto> ();

      var result =
        _graphClient.Cypher
        .Match ("(profile)-[r:BOOK_ADDED]->(book)")
        .OptionalMatch ("(book:Book)-->(catalog:Catalog)")
        .With ("profile, book, catalog")
        .Where ((ProfileDto profile) => profile.Id == userId)
        .ReturnDistinct ((catalog, book) => new {
          catalogs = Return.As<IEnumerable<string>> ("collect([catalog.id])"),
            bk = book.As<BookDetailsDto> ()
        }).OrderByDescending ("book.addedOn");

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
      bookDetails = bookDetails.OrderByDescending (item => item.AddedOn).ToList ();
      return bookDetails;
    }

    public UserFollowersDto FollowUser (int userIdToFollow, int userIdFollower) {
      _graphClient.Cypher
        .Match ("(profile:Profile)", "(follower:Profile)")
        .Where ((ProfileDto profile) => profile.Id == userIdFollower)
        .AndWhere ((ProfileDto follower) => follower.Id == userIdToFollow)
        .CreateUnique ("(profile)-[r:FOLLOW_USER {date}]->(follower)")
        .WithParam ("date", new { addedOn = DateTime.Now }).ExecuteWithoutResults ();

      var followerDto = new UserFollowersDto () {
        Id = userIdToFollow,
        FollowerUserId = userIdFollower,
        UserId = userIdToFollow
      };

      return followerDto;
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

    public List<CatalogItemDto> GetCatalogsForUser (int userId, bool isCurrentUser) {
      var result = _graphClient.Cypher
        .Match ("(catalog:Catalog)")
        .OptionalMatch ("(book:Book)-[r:BOOK_ADDED_TO_CATALOG]->(catalog:Catalog)")
        .With ("book, catalog")
        .Where ((CatalogItemDto catalog) => catalog.UserId == userId)
        .Return ((catalog, book) => new {
          catalogs = catalog.As<CatalogItemDto> (),
            boooks = Return.As<IEnumerable<BookItemDto>>
            ("collect({id:book.id, title: book.title,description:book.description, photoPath:book.photoPath, friendlyUrl:book.friendlyUrl, createdOn:book.createdOn, userId: book.userId })")
        })
        .OrderByDescending ("catalog.centrality");

      var catalogList = new List<CatalogItemDto> ();

      foreach (var item in result.Results) {

        var catList = new CatalogItemDto ();
        catList = item.catalogs;
        foreach (var bk in item.boooks.Where (b => b.Id != 0)) {
          catList.Books.Add (bk);
        }

        //Show catalog only if there are books assigned to it
        if (catList.Books.Count > 0)
          catalogList.Add (catList);
      }

      return catalogList;
    }

    public Helpers.PagedList<CatalogItemDto> GetCatalog (string friendlyUrl, int currentPage) {
      const int ITEMS_PER_PAGE = 24;
      int skipResults = currentPage * ITEMS_PER_PAGE;
      int startFrom = skipResults - ITEMS_PER_PAGE;

      string queryString;
      if (currentPage == 0) {
        queryString = String.Format ("[..{0}]", ITEMS_PER_PAGE);
      } else {
        queryString = String.Format ("[{0}..{1}]", startFrom, startFrom + ITEMS_PER_PAGE);
      }

      var collectBookQuery = @"collect({id:book.id, title: book.title,description:book.description, 
      photoPath:book.photoPath, friendlyUrl:book.friendlyUrl, createdOn:book.createdOn, userId: book.userId })" + queryString;

      var result = _graphClient.Cypher
        .Match ("(catalog:Catalog)")
        .OptionalMatch ("(book:Book)-[r:BOOK_ADDED_TO_CATALOG]->(catalog:Catalog)")
        .With ("book, catalog")
        .Where ((CatalogItemDto catalog) => catalog.FriendlyUrl == friendlyUrl)
        .Return ((catalog, book) => new {
          catalogs = catalog.As<CatalogItemDto> (),
            boooks = Return.As<IEnumerable<BookItemDto>> (collectBookQuery)
        });

      var catalogList = new List<CatalogItemDto> ();

      foreach (var item in result.Results) {

        var catList = new CatalogItemDto ();
        catList = item.catalogs;

        foreach (var bk in item.boooks) {
          catList.Books.Add (bk);
        }
        catalogList.Add (catList);
      }

      var pagedList = new Helpers.PagedList<CatalogItemDto> (catalogList, 100, currentPage, SHOW_MAX_RESULTS_PER_PAGE);
      return pagedList;
    }

    public Helpers.PagedList<CatalogItemDto> GetAllPublicCatalogs (int currentPage = 0) {

      var skipResults = currentPage * SHOW_MAX_RESULTS_PER_PAGE;

      var result = _graphClient.Cypher
        .Match ("(catalog:Catalog)")
        .OptionalMatch ("(book:Book)-[r:BOOK_ADDED_TO_CATALOG]->(catalog:Catalog)")
        .With ("book, catalog")
        .Where ((CatalogItemDto catalog) => catalog.IsPublic == true)
        .Return ((catalog, book) => new {
          catalogs = catalog.As<CatalogItemDto> (),
            boooks = Return.As<IEnumerable<BookItemDto>>
            ("collect({id:book.id, title: book.title,description:book.description, photoPath:book.photoPath, friendlyUrl:book.friendlyUrl, createdOn:book.createdOn, userId: book.userId })[..6]"),
            countBooksInCatalog = Return.As<int> ("count (book.id)")
        })
        .OrderByDescending ("countBooksInCatalog")
        .Skip (skipResults)
        .Limit (SHOW_MAX_RESULTS_PER_PAGE);

      var totalCatalogs =
        _graphClient.Cypher
        .Match ("(catalog:Catalog)")
        .Return ((catalog) => new {
          Count = Return.As<int> ("count (catalog.id)")
        });

      var totalCatalogsCount = totalCatalogs.Results.FirstOrDefault ().Count;

      var catalogList = new List<CatalogItemDto> ();

      foreach (var item in result.Results) {

        var catList = new CatalogItemDto ();
        catList = item.catalogs;

        foreach (var bk in item.boooks) {
          catList.Books.Add (bk);
        }
        catalogList.Add (catList);
      }

      var pagedList = new Helpers.PagedList<CatalogItemDto> (catalogList, totalCatalogsCount, currentPage, SHOW_MAX_RESULTS_PER_PAGE);
      return pagedList;
    }

    private Author AddAuthor (string authorName, int? bookId, int? userId) {
      var resultAuthor = _graphClient.Cypher
        .Match ("(author:Author)")
        .Where ((Author author) => author.Name == authorName)
        .Return<Author> ("author");

      var authorDto = new Author () {
        Name = authorName.Trim (),
        FriendlyUrl = Url.GenerateFriendlyUrl (authorName)
      };
      var authors = resultAuthor.Results.FirstOrDefault ();

      if (authors == null) {
        authors = _graphClient.Cypher
          .Create ("(author:Author {author})")
          .WithParam ("author", authorDto)
          .Return<Node<Author>> ("author").Results.Single ().Data;
      }

      _graphClient.Cypher
        .Match ("(book:Book)", "(author:Author)")
        .Where ((Book book) => book.Id == bookId)
        .AndWhere ((Author author) => author.Id == authors.Id)
        .Create ("(book)-[r:AUTHOR_ASSIGNED_TO_BOOK {info}]->(author)")
        .WithParam ("info", new { addedOn = DateTime.Now, userId = userId })
        .ExecuteWithoutResults ();

      // _graphClient.Cypher
      //   .Match ("(profile:Profile)", "(book:Book)")
      //   .Where ((ProfileDto profile) => profile.Id == bookItem.UserId)
      //   .AndWhere ((BookItemDto book) => book.Id == bookItem.Id)
      //   .Create ("(profile)-[r:BOOK_ADDED {message}]->(book)")
      //   .WithParam ("message", new { addedOn = DateTime.Now })
      //   .ExecuteWithoutResults ();

      return authors;
    }

    public int GetBooksAddedToCatalogs (long userId) {
      var totalBooks =
        _graphClient.Cypher
        .Match ("(b:Book)-[r:BOOK_ADDED_TO_CATALOG {userId: " + userId + "}]->(c:Catalog)")
        .Return ((book) => new {
          Count = Return.As<int> ("count (b.id)")
        });

      var totalBooksCount = totalBooks.Results.FirstOrDefault ().Count;

      return totalBooksCount;
    }
    public int GetUsersFollowingCount (long userId) {
      var totalFollower =
        _graphClient.Cypher
        .Match ("(p:Profile)-[r:FOLLOW_USER]->(u:Profile {id: " + userId + "})").Return ((r) => new {
          Count = Return.As<int> ("count (r)")
        });

      var totalFollowersCount = totalFollower.Results.FirstOrDefault ().Count;

      return totalFollowersCount;
    }

    public CatalogEditDto EditCatalog (int catalogId, bool isPublic, string name, int userId) {

      var result = _graphClient.Cypher
        .Match ("(catalog:Catalog)")
        .Where ((CatalogItemDto catalog) => catalog.Id == catalogId)
        .WithParam ("isPublic", isPublic)
        .WithParam ("name", name)
        .Set ("catalog.isPublic = {isPublic}, catalog.name = {name}")
        .Return<CatalogEditDto> ("catalog").Results.Single ();

      return result;
    }

    public List<string> GetFavoriteCatalogsForUser (int userId) {
      var result = _graphClient.Cypher
        .Match ("(catalog:Favorite)")
        .Where ((CatalogItemDto catalog) => catalog.UserId == userId)
        .ReturnDistinct ((catalog) => new { catalogs = Return.As<string> ("{name:catalog.name }") });

      var strings = new List<string> ();
      foreach (var itm in result.Results) {
        var item = itm.catalogs.Replace ("{\r\n  \"name\": \"", "").Replace ("\"\r\n}", "");
        strings.Add ("'" + item + "'");
      }

      return strings;
    }

    public List<UserBookCategoriesPreferencesDto> GetFavoriteCatalogsForUser_Enriched (int userId) {
      var result = _graphClient.Cypher
        .Match ("(catalog:Favorite)-[r:CATALOG_ADDED]-(p:Profile)")
        .Where ((CatalogItemDto catalog) => catalog.UserId == userId)
        .Return ((catalog) => new {
          catalogs = catalog.As<UserBookCategoriesPreferencesDto> ()
        });

      var catalogs = new List<UserBookCategoriesPreferencesDto> ();
      foreach (var itm in result.Results) {
        itm.catalogs.IsSelected = true;

        catalogs.Add (itm.catalogs);
      }

      return catalogs;
    }

    public void ToggleUserCatalogFromFavorites (int userId, int catalogId, string catalogName, bool IsSelected) {
      if (IsSelected) {
        _graphClient.Cypher
          .Match ("(fCatalog:Favorite)")
          .Where ((CatalogItemDto fCatalog) => fCatalog.Id == catalogId)
          .Remove ("fCatalog:Favorite")
          .ExecuteWithoutResults ();
      } else {
        _graphClient.Cypher
          .Match ("(fCatalog:Catalog)")
          .Where ((CatalogItemDto fCatalog) => fCatalog.Id == catalogId)
          .Set ("fCatalog:Favorite")
          .ExecuteWithoutResults ();
      }
    }
  }
}