using System.ComponentModel.DataAnnotations;

namespace BookApp.API.Dtos {
  public class BookCreateDto {
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    public string Description { get; set; }

    public string PhotoPath { get; set; }

    public string AuthorName { get; set; }

    public int UserId { get; set; }
  }
}