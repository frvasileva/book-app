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

    public DiscussionItemDto CreateDiscussionItem (DiscussionItem model) {

      _context.DiscussionItem.AddAsync (model);
      var user = _context.Users.Where (u => u.Id == model.UserId).FirstOrDefault ();

      var item = new DiscussionItemDto ();
      item.Username = user.UserName;
      item.UserFriendlyUrl = user.FriendlyUrl;
      item.UserId = user.Id;
      item.UserAvatarPath = user.AvatarPath;
      item.AddedOn = model.AddedOn;
      item.Body = model.Body;
      item.DiscussionId = model.DiscussionId;
      item.Id = model.DiscussionId;

      _context.SaveChangesAsync ();

      return item;
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

    public List<DiscussionDetailsDto> GetDiscussions (int? bookId = 0, int? userId = 0) {
      var result = new List<Discussion> ();
      if (bookId.HasValue && bookId != 0) {
        result = _context.Discussions.Include (item => item.DiscussionItems).Where (item => item.BookId == bookId.Value).OrderByDescending (item => item.AddedOn).ToList ();
      } else if (userId.HasValue && userId != 0) {
        result = _context.Discussions.Include (item => item.DiscussionItems).Where (item => item.UserId == userId).OrderByDescending (item => item.AddedOn).ToList ();
      } else {
        result = _context.Discussions.Include (item => item.DiscussionItems).OrderByDescending (item => item.AddedOn).ToList ();
      }

      var bookMapped = new List<DiscussionDetailsDto> ();

      foreach (var item in result) {
        var user = _context.Users.Where (u => u.Id == item.UserId).FirstOrDefault ();
        var book = _graphRepo.GetBookInfo (item.BookId);
        var discussionDetailsItems = new List<DiscussionItemDto> ();

        var discussion = new DiscussionDetailsDto () {
          Id = item.Id,
          Title = item.Title,
          Body = item.Body,
          AddedOn = item.AddedOn,
          FriendlyUrl = item.FriendlyUrl,
          UserId = item.UserId,
          UserAvatarPath = user.AvatarPath,
          UserFriendlyUrl = user.FriendlyUrl,
          Username = user.UserName,
          BookFriendlyUrl = book.FriendlyUrl,
          BookId = item.BookId,
          BookTitle = book.Title,
          BookPhotoPath = book.PhotoPath
        };

        foreach (var disc in item.DiscussionItems) {

          var d = new DiscussionItemDto () {
            Id = disc.Id,
            Username = user.UserName,
            UserAvatarPath = user.AvatarPath,
            UserFriendlyUrl = user.FriendlyUrl,
            AddedOn = disc.AddedOn,
            Body = disc.Body,
            DiscussionId = disc.DiscussionId,
            UserId = user.Id
          };

          discussionDetailsItems.Add (d);
        }

        discussion.DiscussionItems = discussionDetailsItems;
        bookMapped.Add (discussion);
      }

      return bookMapped;
    }

    public List<DiscussionDetailsDto> GetDiscussionsByBook (int bookId) {
      var result = GetDiscussions (bookId, 0);
      return result;
    }

    public List<DiscussionDetailsDto> GetDiscussionsByUser (int userId) {
      var result = GetDiscussions (0, userId);
      return result;
    }

    public async Task<bool> SaveAll () {
      var res = await _context.SaveChangesAsync ();
      return res > 0;
    }

  }
}