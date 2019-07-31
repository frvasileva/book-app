using System.Collections.Generic;
using System.Threading.Tasks;
using BookApp.API.Dtos;

namespace BookApp.API.Data {
  public interface IGraphRepository {
    Task<List<BookPreviewDto>> GetAll ();
    Task<BookDetailsDto> GetBook (string friendlyUrl);
    Task<List<BookPreviewDto>> GetBooksAddedByUser (string friendlyUrl);
    BookItemDto AddBook (BookCreateDto book);
    void AddCatalog (CatalogCreateDto book);
    void AddBookToCatalog (BookCatalogCreateDto item);
  }
}