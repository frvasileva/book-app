namespace BookApp.API.Models {
  public class UserBookCategoriesPreferences {
    public int Id { get; set; }
    public string UserId { get; set; }
    public int BookCatalogPreferencesId { get; set; }
  }
}