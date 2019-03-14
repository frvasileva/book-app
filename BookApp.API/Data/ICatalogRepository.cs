using System.Collections.Generic;
using System.Threading.Tasks;
using BookApp.API.Dtos;
using BookApp.API.Models;
using Microsoft.AspNetCore.Mvc;
namespace BookApp.API.Data
{
  public interface ICatalogRepository
  {
    Task<Catalog> Create(CatalogCreateDto bookList);
    Task<BookCatalog> Update();
    Task<List<BookListItemDto>> GetAll();
    Task<BookListItemDto> Get(int id);
    Task<BookCatalog> Delete(int id);
  }
}