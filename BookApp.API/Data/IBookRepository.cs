using System.Collections.Generic;
using System.Threading.Tasks;
using BookApp.API.Dtos;

namespace BookApp.API.Data
{
  public interface IBookRepository
  {
    Task<List<BookPreviewDto>> GetAll();
    Task<BookDetailsDto> GetBook(string friendlyUrl);
  }
}