using System;

namespace BookApp.API.Dtos {
  public class DiscussionItemDto {
    public int Id { get; set; }
    public int DiscussionId { get; set; }

    public string Body { get; set; }
    public DateTime AddedOn { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; }
    public string UserFriendlyUrl { get; set; }
    public string UserAvatarPath { get; set; }
  }
}