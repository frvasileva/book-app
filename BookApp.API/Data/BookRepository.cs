using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using BookApp.API.Dtos;
using BookApp.API.Helpers;
using BookApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BookApp.API.Data {
  public class BookRepository : IBookRepository {
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly IAuthorRepository _repoAuthor;

    public BookRepository (DataContext context, IMapper mapper, IAuthorRepository repoAuthor) {
      _context = context;
      _mapper = mapper;
      _repoAuthor = repoAuthor;
    }

    public static DataTable ConvertCSVtoDataTable (string strFilePath = "") {

      StreamReader sr = new StreamReader (strFilePath);
      string[] headers = sr.ReadLine ().Split (',');
      DataTable dt = new DataTable ();
      foreach (string header in headers) {
        dt.Columns.Add (header);
      }
      while (!sr.EndOfStream) {
        string[] rows = Regex.Split (sr.ReadLine (), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
        DataRow dr = dt.NewRow ();
        for (int i = 0; i < headers.Length; i++) {
          dr[i] = rows[i];
        }

        dt.Rows.Add (dr);
      }
      return dt;
    }

    public void Add<T> (T entity) where T : class {

      throw new Exception ("not implemented");
    }

    public async Task<Book> ImportBooks () {
      var strFilePath = "D:\\diploma\\diploma\\BookApp.API\\BookDataImports\\books.csv";
      var data = ConvertCSVtoDataTable (strFilePath);
      var fakeBook = new Book ();
      for (int i = 0; i < data.Rows.Count; i++) {
        var item = data.Rows[i];

        var book = new Book ();
        book.Title = item.ItemArray[9].ToString ();
        book.Description = item.ItemArray[10].ToString ();
        book.PhotoPath = item.ItemArray[21].ToString ();
        book.FriendlyUrl = Url.GenerateFriendlyUrl (item.ItemArray[9].ToString ());
        book.PublisherId = 1;
        book.AuthorId = 33;
        book.AddedOn = DateTime.Now;
        book.Description = item.ItemArray[10].ToString ();
        book.ExternalId = Int32.Parse (item.ItemArray[1].ToString ());
        book.ISBN = item.ItemArray[5].ToString ();
        book.AvarageRating = Convert.ToDouble (item.ItemArray[12]);
        book.UserId = 33;

        var authorName = item.ItemArray[7].ToString ();
        var author = _context.Authors.Where (ath => ath.Name == authorName).FirstOrDefault ();
        if (author != null) {
          book.AuthorId = author.Id;
          book.Author = author;
        } else {
          var createAuthor = new Author ();
          createAuthor.Name = authorName;
          createAuthor.AddedOn = DateTime.Now;
          createAuthor.FriendlyUrl = Url.GenerateFriendlyUrl (authorName.ToString ());
          book.Author = createAuthor;
          book.AuthorId = createAuthor.Id;

          await _context.Authors.AddAsync (createAuthor);
        }

        await _context.Books.AddAsync (book);
      }

      await _context.SaveChangesAsync ();
      return fakeBook;
    }

    public async Task<Tag> ImportTags () {
      var strFilePath = "BookApp.API\\BookDataImports\\tags.csv";
      var data = ConvertCSVtoDataTable (strFilePath);
      var fakeTag = new Tag ();
      for (int i = 0; i < data.Rows.Count; i++) {
        var item = data.Rows[i];
        var tag = new Tag ();

        tag.ExternalId = Int32.Parse (item.ItemArray[0].ToString ());
        tag.Name = item.ItemArray[1].ToString ();
        tag.FriendlyUrl = Url.GenerateFriendlyUrl (item.ItemArray[1].ToString ());
        tag.AddedOn = DateTime.Now;

        await _context.Tags.AddAsync (tag);
      }

      await _context.SaveChangesAsync ();
      return fakeTag;
    }

    public async Task<BookTags> ImportBookTags () {
      var strFilePath = "BookApp.API\\BookDataImports\\book_tags.csv";
      var data = ConvertCSVtoDataTable (strFilePath);
      var fakeBookTag = new BookTags ();
      for (int i = 0; i < data.Rows.Count; i++) {
        var item = data.Rows[i];
        var bookTag = new BookTags ();

        bookTag.BookExternalId = Int32.Parse (item.ItemArray[0].ToString ());
        bookTag.TagExternalId = Int32.Parse (item.ItemArray[1].ToString ());
        bookTag.Count = Int32.Parse (item.ItemArray[2].ToString ());
        bookTag.AddedOn = DateTime.Now;

        await _context.BookTags.AddAsync (bookTag);
      }

      await _context.SaveChangesAsync ();
      return fakeBookTag;
    }

    public async Task<Book> AddBook (BookCreateDto bookDto) {

      var result = _mapper.Map<Book> (bookDto);
      result.AddedOn = DateTime.Now;

      if (await _repoAuthor.CheckAuthorExists (bookDto.AuthorName)) {
        var authorResult = await _repoAuthor.Get (bookDto.AuthorName);
        result.AuthorId = authorResult.Id;
      } else {
        var authorResult = await _repoAuthor.Create (bookDto.AuthorName);
        result.AuthorId = authorResult.Id;
      }

      await _context.Books.AddAsync (result);
      await _context.SaveChangesAsync ();

      return result;
    }

    public async Task<BookCatalog> AddBookToCatalog (BookCatalogCreateDto bookCatalogDto) {

      var bookCatalogEntity = new BookCatalog () {
        BookId = bookCatalogDto.BookId,
        Created = DateTime.Now,
        UserId = bookCatalogDto.UserId
      };

      if (bookCatalogDto.CatalogId.HasValue) {
        // bookCatalogEntity.CatalogId = bookCatalogDto.CatalogId.Value;
      } else {
        var catalog = new Catalog ();
        catalog.UserId = bookCatalogDto.UserId;
        catalog.IsPublic = true;
        catalog.Name = bookCatalogDto.CatalogName;
        catalog.FriendlyUrl = Url.GenerateFriendlyUrl (catalog.Name + "-" + StringHelper.GenerateRandomNo ());
        catalog.AddedOn = DateTime.Now;

        _context.Catalogs.Add (catalog);
        await _context.SaveChangesAsync ();

        bookCatalogEntity.CatalogId = catalog.Id;

      }
      await _context.BookCatalog.AddAsync (bookCatalogEntity);
      await _context.SaveChangesAsync ();

      return bookCatalogEntity;
    }

    public void Delete<T> (T entity) where T : class {
      throw new NotImplementedException ();
    }

    public async Task<bool> RemoveBookFromCatalog (int catalogId, int bookId) {
      var result = _context.BookCatalog.Where (item => item.CatalogId == catalogId && item.BookId == bookId).ToList ().FirstOrDefault ();

      _context.BookCatalog.Attach (result);
      _context.BookCatalog.Remove (result);

      return await _context.SaveChangesAsync () > 0;
    }

    public async Task<Book> Get (string friendlyUrl) {
      var book = await _context.Books.Include (itm => itm.BookCatalogs).Where (item => item.FriendlyUrl == friendlyUrl).FirstOrDefaultAsync ();
      return book;
    }

    public async Task<List<BookPreviewDto>> GetAll () {
      var bookList = await _context.Books.Include (itm => itm.BookCatalogs).OrderByDescending (item => item.AddedOn).Take (100).ToListAsync ();
      //  var mappedBookList = _mapper.Map<List<BookPreviewDto>> (bookList);
      var mappedBookList = new List<BookPreviewDto> ();

      foreach (var item in bookList) {
        var bookPreview = new BookPreviewDto () {
          Id = item.Id,
          UserId = item.UserId,
          Title = item.Title,
          Description = item.Description,
          PhotoPath = item.PhotoPath,
          FriendlyUrl = item.FriendlyUrl
        };

        foreach (var itm in item.BookCatalogs) {
          var bookCatalogListDto = new BookCatalogListDto () {
            CatalogId = itm.CatalogId
          };

          bookPreview.BookCatalogs.Add (bookCatalogListDto);
        }

        mappedBookList.Add (bookPreview);
      }

      return mappedBookList;
    }

    public async Task<List<BookPreviewDto>> GetBooksAddedByUser (string friendlyUrl) {
      var bookList = await _context.Books.Include (itm => itm.BookCatalogs)
        .Where (item => item.User.FriendlyUrl == friendlyUrl).OrderByDescending (item => item.AddedOn).ToListAsync ();

      var mappedBookList = new List<BookPreviewDto> ();

      foreach (var item in bookList) {
        var bookPreview = new BookPreviewDto () {
          Id = item.Id,
          UserId = item.UserId,
          Title = item.Title,
          Description = item.Description,
          PhotoPath = item.PhotoPath,
          FriendlyUrl = item.FriendlyUrl
        };

        foreach (var itm in item.BookCatalogs) {
          var bookCatalogListDto = new BookCatalogListDto () {
            CatalogId = itm.CatalogId
          };

          bookPreview.BookCatalogs.Add (bookCatalogListDto);
        }

        mappedBookList.Add (bookPreview);
      }

      return mappedBookList;
    }

    public async Task<BookDetailsDto> GetBook (string friendlyUrl) {

      var book = await _context.Books.Include (itm => itm.BookCatalogs).Where (item => item.FriendlyUrl == friendlyUrl).FirstOrDefaultAsync ();

      var mappedBook = new BookDetailsDto () {
        Id = book.Id,
        UserId = book.UserId,
        Title = book.Title,
        Description = book.Description,
        PhotoPath = book.PhotoPath,
        FriendlyUrl = book.FriendlyUrl
      };

      foreach (var itm in book.BookCatalogs) {
        var bookCatalogListDto = new BookCatalogListDto () {
          CatalogId = itm.CatalogId
        };

        mappedBook.BookCatalogs.Add (bookCatalogListDto);
      }

      return mappedBook;
    }

    public async Task<bool> SaveAll () {
      var res = await _context.SaveChangesAsync ();
      return res > 0;
    }

  }
}