using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookApp.API.Dtos;
using BookApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BookApp.API.Data
{
  public class AuthorRepository : IAuthorRepository
  {
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public AuthorRepository(DataContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<Author> Get(string authorName)
    {
      var author = await this._context.Authors.Where(item => item.Name == authorName).FirstOrDefaultAsync();
      return author;
    }

    public async Task<Author> Get(int id)
    {
      var author = await this._context.Authors.FindAsync(id);
      return author;
    }

    public async Task<bool> CheckAuthorExists(string authorName)
    {
      var author = await Get(authorName);
      return author != null;
    }

    public async Task<Author> Create(string authorName)
    {
      var author = new Author
      {
        Name = authorName,
        FriendlyUrl = BookApp.API.Helpers.Url.GenerateFriendlyUrl(authorName),
        AddedOn = DateTime.Now
      };

      await _context.Authors.AddAsync(author);
      await _context.SaveChangesAsync();

      return author;
    }

    public async Task<Author> Create(AuthorCreateDto author)
    {
      var result = _mapper.Map<Author>(author);

      result.AddedOn = DateTime.Now;
      await _context.Authors.AddAsync(result);
      await _context.SaveChangesAsync();

      return result;
    }

    public async Task<Author> Delete(int id)
    {
      var result = _context.Authors.Find(id);

      _context.Authors.Attach(result);
      _context.Authors.Remove(result);
      await _context.SaveChangesAsync();

      return result;
    }

    public Task<Author> Delete(string authorName)
    {
      throw new NotImplementedException();
    }

    public Task<Author> Update(string authorName)
    {
      throw new System.NotImplementedException();
    }
  }
}