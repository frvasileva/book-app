using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApp.API.Dtos;
using BookApp.API.Models;
using Microsoft.EntityFrameworkCore;
namespace BookApp.API.Data {
  public class DiscussionRepository : IDiscussionRepository {

    private readonly DataContext _context;
    private readonly IGraphRepository _graphRepo;

    public DiscussionRepository (DataContext context, IGraphRepository graphRepo) {
      _context = context;
      _graphRepo = graphRepo;
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

    public DiscussionDetailsDto GetDiscussion (string friendlyUrl) {
      var result = _context.Discussions.Include (item => item.DiscussionItems).Where (item => item.FriendlyUrl == friendlyUrl).ToList ().FirstOrDefault ();
      var user = _context.Users.Where (u => u.Id == result.UserId).FirstOrDefault ();
      var book = _graphRepo.GetBookInfo (result.BookId);
      var discussionDetailsItems = new List<DiscussionItemDto> ();

      var discussion = new DiscussionDetailsDto () {
        Id = result.Id,
        Title = result.Title,
        Body = result.Body,
        //  DiscussionItems = result.DiscussionItems,
        AddedOn = result.AddedOn,
        FriendlyUrl = result.FriendlyUrl,
        UserId = result.UserId,
        UserAvatarPath = user.AvatarPath,
        UserFriendlyUrl = user.FriendlyUrl,
        Username = user.UserName,
        BookFriendlyUrl = book.FriendlyUrl,
        BookId = result.BookId,
        BookTitle = book.Title,
        BookPhotoPath = book.PhotoPath
      };
 
      foreach (var disc in result.DiscussionItems) {
        var usr = _context.Users.Where (u => u.Id == disc.UserId).FirstOrDefault ();
        var d = new DiscussionItemDto () {
          Id = disc.Id,
          Username = usr.UserName,
          UserAvatarPath = usr.AvatarPath,
          UserFriendlyUrl = usr.FriendlyUrl,
          AddedOn = disc.AddedOn,
          Body = disc.Body,
          DiscussionId = disc.DiscussionId,
          UserId = usr.Id
        };

        discussionDetailsItems.Add (d);
      }

      discussion.DiscussionItems = discussionDetailsItems;

      return discussion;
    }

    public List<Discussion> GetDiscussions () {
      var result = _context.Discussions.Include (item => item.DiscussionItems).OrderByDescending (item => item.AddedOn).ToList ();
      return result;
    }

    public List<Discussion> GetDiscussionsByBook (int bookId) {
      var result = _context.Discussions.Include (item => item.DiscussionItems).Where (item => item.BookId == bookId).OrderByDescending (item => item.AddedOn).ToList ();
      return result;
    }

    public List<Discussion> GetDiscussionsByUser (int userId) {
      var result = _context.Discussions.Include (item => item.DiscussionItems).Where (item => item.UserId == userId).OrderByDescending (item => item.AddedOn).ToList ();
      return result;
    }

    public async Task<bool> SaveAll () {
      var res = await _context.SaveChangesAsync ();
      return res > 0;
    }

  }
}