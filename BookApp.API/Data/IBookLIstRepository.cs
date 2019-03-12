using System.Collections.Generic;
using System.Threading.Tasks;
using BookApp.API.Dtos;
using BookApp.API.Models;
using Microsoft.AspNetCore.Mvc;
namespace BookApp.API.Data
{
  public interface IBookListRepository
  {
    Task<BookList> Create(BookListCreateDto bookList);
    Task<BookList> Update();
    Task<List<BookListItemDto>> GetAll();
    Task<BookListItemDto> Get(int id);
    Task<BookList> Delete(int id);
  }
}