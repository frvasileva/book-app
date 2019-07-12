using System.Collections.Generic;
using System.Threading.Tasks;
using BookApp.API.Dtos;
using BookApp.API.Models;

namespace BookApp.API.Data {
  public interface ICatalogRepository {
    Task<Catalog> Create (CatalogCreateDto bookList);
    Task<Catalog> Update (CatalogCreateDto catalog);
    Task<List<BookListItemDto>> GetAll ();
    Task<List<Catalog>> GetAllPure();
    Task<CatalogItemDto> Get (string friendlyUrl);
    Task<BookCatalog> Delete (int id);

    Task<List<CatalogItemDto>> GetForUser (string friendlyUrl, bool isCurrentUser);
    Task<List<CatalogPureDto>> GetPureForUser (string friendlyUrl, bool isCurrentUser);

  }
}