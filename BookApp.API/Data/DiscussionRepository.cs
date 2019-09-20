using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApp.API.Models;
using Microsoft.EntityFrameworkCore;
namespace BookApp.API.Data {
  public class DiscussionRepository : IDiscussionRepository {

    private readonly DataContext _context;

    public DiscussionRepository (DataContext context) {
      _context = context;
    }

    public Discussion Create (Discussion model) {

      _context.Discussions.AddAsync (model);
      _context.SaveChangesAsync ();

      return model;
    }

    public DiscussionItem CreateDiscussionItem (DiscussionItem model) {

      _context.DiscussionItem.AddAsync (model);
      _context.SaveChangesAsync ();

      return model;
    }

    public void Add<T> (T entity) where T : class {
      throw new NotImplementedException ();
    }

    public void Delete<T> (T entity) where T : class {
      throw new NotImplementedException ();
    }

    public void Edit<T> (T entity) where T : class {
      throw new NotImplementedException ();
    }

    public Discussion GetDiscussion (int id) {
      var result = _context.Discussions.Include (item => item.DiscussionItems).Where (item => item.Id == id).ToList ().FirstOrDefault ();
      // var discussions = _context.DiscussionItem.Where (item => item.DiscussionId == id).ToList ();
      // result.DiscussionItems = discussions;
      return result;
    }

    public List<Discussion> GetDiscussions () {
      var result = _context.Discussions.Include (item => item.DiscussionItems).ToList ();
      return result;
    }

    public List<Discussion> GetDiscussionsByBook (int bookId) {
      var result = _context.Discussions.Include (item => item.DiscussionItems).Where (item => item.BookId == bookId).ToList ();
      return result;
    }

    public List<Discussion> GetDiscussionsByUser (int userId) {
      var result = _context.Discussions.Include (item => item.DiscussionItems).Where (item => item.UserId == userId).ToList ();
      return result;
    }

    public async Task<bool> SaveAll () {
      var res = await _context.SaveChangesAsync ();
      return res > 0;
    }

  }
}