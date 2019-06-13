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

    public async Task<BookListItemDto> Get (int id) {
      var catalogList = await _context.Catalogs.Where (item => item.Id == id).FirstOrDefaultAsync ();
      var mappedBook = _mapper.Map<BookListItemDto> (catalogList);

      return mappedBook;
    }

    public async Task<List<BookListItemDto>> GetAll () {
      var catalogList = await _context.Catalogs.Include (item => item.BookCatalogs).ThenInclude (itm => itm.Book).OrderByDescending (item => item.Created).ToListAsync ();
      var bookList = await _context.Books.Include (item => item.BookCatalogs).ThenInclude (itm => itm.Catalog).ToListAsync ();

      var mappedBookList = _mapper.Map<List<BookListItemDto>> (catalogList);

      return mappedBookList;
    }

    public async Task<List<BookListItemDto>> GetForUser (int userId) {

      var bookList = await _context.Catalogs.Where (item => item.UserId == userId).OrderByDescending (item => item.Created).ToListAsync ();
      var mappedBookList = _mapper.Map<List<BookListItemDto>> (bookList);

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