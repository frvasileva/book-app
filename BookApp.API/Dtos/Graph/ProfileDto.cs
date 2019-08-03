using System;

public class ProfileDto {

  ProfileDto () {
    this.CreatedOn = DateTime.Now;
  }
  
  public int Id { get; set; }
  public string Email { get; set; }

  public DateTime CreatedOn { get; set; }
}