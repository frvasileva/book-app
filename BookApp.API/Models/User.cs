using System;
using System.Collections;
using System.Collections.Generic;

namespace BookApp.API.Models
{
  public class User
  {
    public int Id { get; set; }
    public string Email { get; set; }
    public string AvatarPath { get; set; }
    public string KnownAs { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastActive { get; set; }
    public string Introduction { get; set; }
    public string Interests { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public ICollection<Book> Books { get; set; }
    // public ICollection<Message> MessagesSent { get; set; }
    // public ICollection<Message> MessagesReceived { get; set; }
  }
}