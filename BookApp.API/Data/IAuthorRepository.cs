using System.Collections.Generic;
using System.Threading.Tasks;
using BookApp.API.Dtos;
using BookApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookApp.API.Data
{
  public interface IAuthorRepository
  {
    Task<Author> Get(string authorName);
    Task<Author> Get(int id);
    Task<Author> Create(string authorName);
    Task<Author> Create(AuthorCreateDto authorName);
    Task<Author> Update(string authorName);
    Task<Author> Delete(string authorName);
    Task<bool> CheckAuthorExists(string authorName);
  }
}
