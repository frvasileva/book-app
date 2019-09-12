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
  public class GraphRepository : IGraphRepository {
    private readonly IGraphClient _graphClient;
    private readonly IMapper _mapper;
    private readonly int SHOW_MAX_RESULTS_PER_PAGE = 12;

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
          catalogs = Return.As<IEnumerable<BookCatalogListDto>> ("collect({catalogId:catalog.id, name:catalog.name, friendlyUrl:catalog.friendlyUrl})"),
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

    public List<CatalogItemDto> GetCatalog (string friendlyUrl) {
      var result = _graphClient.Cypher
        .Match ("(catalog:Catalog)")
        .OptionalMatch ("(book:Book)-[r:BOOK_ADDED_TO_CATALOG]->(catalog:Catalog)")
        .With ("book, catalog")
        .Where ((CatalogItemDto catalog) => catalog.FriendlyUrl == friendlyUrl)
        .Return ((catalog, book) => new {
          catalogs = catalog.As<CatalogItemDto> (),
            boooks = Return.As<IEnumerable<BookItemDto>>
            ("collect({id:book.id, title: book.title,description:book.description, photoPath:book.photoPath, friendlyUrl:book.friendlyUrl, createdOn:book.createdOn, userId: book.userId })")
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

      return catalogList;
    }

    public List<CatalogItemDto> GetAllPublicCatalogs () {
      var result = _graphClient.Cypher
        .Match ("(catalog:Catalog)")
        .OptionalMatch ("(book:Book)-[r:BOOK_ADDED_TO_CATALOG]->(catalog:Catalog)")
        .With ("book, catalog")
        .Where ((CatalogItemDto catalog) => catalog.IsPublic == true)
        .Return ((catalog, book) => new {
          catalogs = catalog.As<CatalogItemDto> (),
            boooks = Return.As<IEnumerable<BookItemDto>>
            ("collect({id:book.id, title: book.title,description:book.description, photoPath:book.photoPath, friendlyUrl:book.friendlyUrl, createdOn:book.createdOn, userId: book.userId })")
        }).Limit (40);

      var catalogList = new List<CatalogItemDto> ();

      foreach (var item in result.Results) {

        var catList = new CatalogItemDto ();
        catList = item.catalogs;

        foreach (var bk in item.boooks) {
          catList.Books.Add (bk);
        }
        catalogList.Add (catList);
      }

      return catalogList;
    }

    #region ImportData
    public void ImportBooks () {
      var strFilePath = "D:\\diploma\\diploma\\BookApp.API\\BookDataImports\\books.csv";
      var data = BookRepository.ConvertCSVtoDataTable (strFilePath);
      var fakeBook = new Book ();
      for (int i = 0; i < data.Rows.Count; i++) {
        var item = data.Rows[i];

        var book = new Book ();
        book.Title = item.ItemArray[9].ToString ();
        book.Description = item.ItemArray[10].ToString ();
        book.PhotoPath = item.ItemArray[21].ToString ();
        book.FriendlyUrl = Url.GenerateFriendlyUrl (item.ItemArray[9].ToString ());
        book.PublisherId = 0;
        book.AuthorId = 0;
        book.AddedOn = DateTime.Now;
        book.Description = item.ItemArray[10].ToString ();
        book.ExternalId = Int32.Parse (item.ItemArray[1].ToString ());
        book.ISBN = item.ItemArray[5].ToString ();
        book.AvarageRating = Convert.ToDouble (item.ItemArray[12]);
        book.UserId = 0;

        var authorName = item.ItemArray[7].ToString ();
        //var author = this.AddAuthor (authorName, book.Id, book.UserId);
        //book.AuthorId = author.Id;

        this.AddBook (book);

        var author = this.AddAuthor (authorName, book.Id, book.UserId);
      }
    }
    public void ImportTags () {
      var strFilePath = "D:\\diploma\\diploma\\BookApp.API\\BookDataImports\\tags.csv";
      var data = BookRepository.ConvertCSVtoDataTable (strFilePath);
      var fakeTag = new Tag ();
      for (int i = 0; i < data.Rows.Count; i++) {
        var item = data.Rows[i];

        var catalog = new Catalog ();
        catalog.AddedOn = DateTime.Now;
        catalog.Name = item.ItemArray[1].ToString ();
        catalog.FriendlyUrl = Url.GenerateFriendlyUrl (item.ItemArray[1].ToString ());
        catalog.ExternalId = Int32.Parse (item.ItemArray[0].ToString ());
        catalog.UserId = 0;
        catalog.IsPublic = true;

        var result = _graphClient.Cypher
          .Create ("(catalog:Catalog {catalog})")
          .WithParam ("catalog", catalog)
          .Return<Node<CatalogCreateDto>> ("catalog").Results.Single ().Data;

        _graphClient.Cypher
          .Match ("(profile:Profile)", "(cat:Catalog)")
          .Where ((ProfileDto profile) => profile.Id == catalog.UserId)
          .AndWhere ((CatalogCreateDto cat) => cat.Id == catalog.Id)
          .CreateUnique ("(profile)-[r:CATALOG_ADDED {date}]->(cat)")
          .WithParam ("date", new { addedOn = DateTime.Now }).ExecuteWithoutResults ();
      }
    }
    public void ImportBookTags () {
      var strFilePath = "D:\\diploma\\diploma\\BookApp.API\\BookDataImports\\book_tags.csv";
      var data = BookRepository.ConvertCSVtoDataTable (strFilePath);
      var fakeBookTag = new BookTags ();
      for (int i = 0; i < data.Rows.Count; i++) {
        var item = data.Rows[i];
        var bookTag = new BookTags ();

        var bookExternalId = Int32.Parse (item.ItemArray[0].ToString ());
        var categoryExternalId = Int32.Parse (item.ItemArray[1].ToString ());
        bookTag.Count = Int32.Parse (item.ItemArray[2].ToString ());
        bookTag.AddedOn = DateTime.Now;

        _graphClient.Cypher
          .Match ("(book:Book)", "(catalog:Catalog)")
          .Where ((Book book) => book.ExternalId == bookExternalId)
          .AndWhere ((Catalog catalog) => catalog.ExternalId == categoryExternalId)
          .Create ("(book)-[r:BOOK_ADDED_TO_CATALOG {info}]->(catalog)")
          .WithParam ("info", new { addedOn = DateTime.Now, userId = 0 })
          .ExecuteWithoutResults ();
        // .Return ((catalog, book, r) => new {
        //   cat = catalog.As<Catalog> (),
        //     bk = book.As<Book> ()
        // });
        // var res = result.Return.result.;
        //   this.AddCatalog()
      }
    }
    #endregion ImportData

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