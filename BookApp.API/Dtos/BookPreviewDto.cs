namespace BookApp.API.Dtos
{
  public class BookPreviewDto
  {
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string PhotoPath { get; set; }
    
    public string FriendlyUrl { get; set; }

  }
}