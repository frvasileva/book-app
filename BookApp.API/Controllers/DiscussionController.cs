using System;
using System.Security.Claims;
using BookApp.API.Data;
using BookApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookApp.API.Controllers {

  [Authorize]
  [Route ("api/[controller]")]
  [ApiController]
  public class DiscussionController : ControllerBase {

    private IDiscussionRepository _discussionRepo;

    private int UserId {
      get {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        int userId = 0;
        if (identity != null)
          userId = Int32.Parse (identity.FindFirst (ClaimTypes.NameIdentifier).Value);

        return userId;
      }
    }

    public DiscussionController (IDiscussionRepository discussionRepo) {
      _discussionRepo = discussionRepo;
    }

    [HttpPost ("create-discussion")]
    public IActionResult CreateDiscussion (Discussion item) {

      item.UserId = UserId;
      item.FriendlyUrl = Helpers.Url.GenerateFriendlyUrl (item.Title);
      var discussion = _discussionRepo.Create (item);

      return Ok (discussion);
    }

    [HttpPost ("add-discussion-item")]
    public IActionResult CreateDiscussionItem (DiscussionItem item) {

      item.UserId = UserId;
      var discussionItem = _discussionRepo.CreateDiscussionItem (item);

      return Ok (discussionItem);
    }

    [HttpGet ("get/{friendlyUrl}")]
    public IActionResult GetDiscussion (string friendlyUrl) {

      var discussion = _discussionRepo.GetDiscussion (friendlyUrl);

      return Ok (discussion);
    }

    [HttpGet ("get-discussions")]
    public IActionResult GetDiscussions () {

      var discussion = _discussionRepo.GetDiscussions (null, null);

      return Ok (discussion);
    }

    [HttpGet ("get-by-book/{id}")]
    public IActionResult GetDiscussionByBook (int id) {

      var discussion = _discussionRepo.GetDiscussionsByBook (id);

      return Ok (discussion);
    }

    [HttpGet ("get-by-user/{id}")]
    public IActionResult GetDiscussionsByUser (int id) {

      var discussion = _discussionRepo.GetDiscussionsByUser (id);

      return Ok (discussion);
    }

  }
}