using System;
using System.Collections.Generic;
using BookApp.API.Models;

namespace BookApp.API.Dtos {
  public class DiscussionDetailsDto {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public DateTime AddedOn { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; }
    public string UserFriendlyUrl { get; set; }
    public string UserAvatarPath { get; set; }
    public int BookId { get; set; }
    public string BookTitle { get; set; }
    public string BookFriendlyUrl { get; set; }
    public string FriendlyUrl { get; set; }
    public List<DiscussionItemDto> DiscussionItems { get; set; }
  }
}