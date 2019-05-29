using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace BookApp.API.Models {
  public class User : IdentityUser<int> {
    public string AvatarPath { get; set; }
    public string KnownAs { get; set; }

    public string FriendlyUrl { get; set; }

   // public byte[] PasswordSalt { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastActive { get; set; }
    public string Introduction { get; set; }
    public string Interests { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public ICollection<Book> Books { get; set; }

    public ICollection<UserRole> UserRoles { get; set; }

    public ICollection<Photo> Photos { get; set; }

    // public ICollection<Message> MessagesSent { get; set; }
    // public ICollection<Message> MessagesReceived { get; set; }

    User () {
      this.Photos = new List<Photo> ();
    }
  }
}