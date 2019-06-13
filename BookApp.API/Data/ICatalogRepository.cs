using System.Collections.Generic;
using System.Threading.Tasks;
using BookApp.API.Dtos;
using BookApp.API.Models;
using Microsoft.AspNetCore.Mvc;
namespace BookApp.API.Data {
  public interface ICatalogRepository {
    Task<Catalog> Create (CatalogCreateDto bookList);
    Task<Catalog> Update (CatalogCreateDto catalog);
    Task<List<BookListItemDto>> GetAll ();
    Task<List<Catalog>> GetAllPure();
    Task<BookListItemDto> Get (int id);
    Task<BookCatalog> Delete (int id);

    Task<List<BookListItemDto>> GetForUser (int userId);

  }
}