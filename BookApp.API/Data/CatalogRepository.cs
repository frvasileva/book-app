using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookApp.API.Dtos;
using BookApp.API.Helpers;
using BookApp.API.Models;
using Microsoft.EntityFrameworkCore;
namespace BookApp.API.Data {
  public class CatalogRepository : ICatalogRepository {

    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public CatalogRepository (DataContext context, IMapper mapper) {
      _context = context;
      _mapper = mapper;
    }

    public async Task<Catalog> Create (CatalogCreateDto catalogDto) {
      var result = _mapper.Map<Catalog> (catalogDto);
      result.Created = DateTime.Now;
      result.FriendlyUrl = Url.GenerateFriendlyUrl (catalogDto.Name + "-" + StringHelper.GenerateRandomNo ());
      await _context.Catalogs.AddAsync (result);
      await _context.SaveChangesAsync ();

      return result;
    }

    public async Task<BookCatalog> Delete (int id) {
      var result = _context.BookCatalog.Find (id);

      _context.BookCatalog.Attach (result);
      _context.BookCatalog.Remove (result);
      await _context.SaveChangesAsync ();

      return result;
    }

    public async Task<CatalogItemDto> Get (string friendlyUrl) {
      var catalogs = await _context.Catalogs.Include (item => item.BookCatalogs).ThenInclude (itm => itm.Book).ThenInclude (itm => itm.User).
      Where (item => item.FriendlyUrl == friendlyUrl).OrderByDescending (item => item.Created).ToListAsync ();
   
      var catalogItem = catalogs.FirstOrDefault ();
      var catalog = new CatalogItemDto ();
      catalog.Id = catalogItem.Id;
      catalog.Name = catalogItem.Name;
      catalog.IsPublic = catalogItem.IsPublic;
      catalog.UserId = catalogItem.UserId;
      catalog.FriendlyUrl = catalogItem.FriendlyUrl;
      catalog.Created = catalogItem.Created;
      catalog.UserFriendlyUrl = catalogItem.User.FriendlyUrl;

      foreach (var itm in catalogItem.BookCatalogs) {
        var book = _mapper.Map<BookItemDto> (itm.Book);
        catalog.Books.Add (book);
      }

      return catalog;
    }

    public async Task<List<BookListItemDto>> GetAll () {
      var catalogList = await _context.Catalogs.Include (item => item.BookCatalogs).ThenInclude (itm => itm.Book).ThenInclude (itm => itm.User).OrderByDescending (item => item.Created).ToListAsync ();
      var mappedBookList = _mapper.Map<List<BookListItemDto>> (catalogList);

      return mappedBookList;
    }

    public async Task<List<Catalog>> GetAllPure () {
      var catalogList = await _context.Catalogs.Include (item => item.BookCatalogs).ThenInclude (itm => itm.Book).ThenInclude (itm => itm.User)
        .OrderByDescending (item => item.Created).ToListAsync ();

      return catalogList;
    }

    public async Task<List<CatalogItemDto>> GetForUser (string friendlyUrl, bool isCurrentUser) {

      var user = await _context.Users.Where (item => item.FriendlyUrl == friendlyUrl).ToListAsync ();
      var bookList = new List<Catalog> ();
      var catalogs = new List<CatalogItemDto> ();

      if (isCurrentUser) {
        bookList = await _context.Catalogs.Include (item => item.BookCatalogs).ThenInclude (itm => itm.Book).ThenInclude (itm => itm.User).
        Where (item => item.UserId == user.FirstOrDefault ().Id).OrderByDescending (item => item.Created).ToListAsync ();
      } else {
        bookList = await _context.Catalogs.Include (item => item.BookCatalogs).ThenInclude (itm => itm.Book).ThenInclude (itm => itm.User).
        Where (item => item.UserId == user.FirstOrDefault ().Id && item.IsPublic == true).OrderByDescending (item => item.Created).ToListAsync ();
      }

      foreach (var item in bookList) {
        var catalog = new CatalogItemDto ();
        catalog.Id = item.Id;
        catalog.Name = item.Name;
        catalog.IsPublic = item.IsPublic;
        catalog.UserId = item.UserId;
        catalog.FriendlyUrl = item.FriendlyUrl;
        catalog.Created = item.Created;
        catalog.UserFriendlyUrl = item.User.FriendlyUrl;

        foreach (var itm in item.BookCatalogs) {
          var book = _mapper.Map<BookItemDto> (itm.Book);
          catalog.Books.Add (book);
        }

        catalogs.Add (catalog);
      }

      var mappedBookList = _mapper.Map<List<BookListItemDto>> (bookList);
      var test = catalogs;
      return catalogs;
    }

    public async Task<List<CatalogPureDto>> GetPureForUser (string friendlyUrl, bool isCurrentUser) {
      var user = await _context.Users.Where (item => item.FriendlyUrl == friendlyUrl).ToListAsync ();

      var bookList = await _context.Catalogs.Include (item => item.BookCatalogs).ThenInclude (itm => itm.Book).ThenInclude (itm => itm.User).
      Where (item => item.UserId == user.FirstOrDefault ().Id).OrderByDescending (item => item.Created).ToListAsync ();

      var mappedBookList = _mapper.Map<List<CatalogPureDto>> (bookList);
      if (!isCurrentUser) {
        mappedBookList = mappedBookList.Where (item => item.IsPublic == true).ToList ();
      }
      return mappedBookList;
    }

    public async Task<Catalog> Update (CatalogCreateDto catalogDto) {

      var catalog = _context.Catalogs.Find (catalogDto.Id);
      catalog.Name = catalogDto.Name;
      catalog.IsPublic = catalogDto.IsPublic;

      _context.Catalogs.Attach (catalog);
      _context.Catalogs.Update (catalog);
      _context.SaveChangesAsync ();

      return catalog;

    }
  }
}