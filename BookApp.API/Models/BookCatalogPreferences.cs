namespace BookApp.API.Models {

  public class BookCatalogPreferences {
    public int Id { get; set; }
    public string Name { get; set; }
    public string IconPath { get; set; }
    public int FkTag { get; set; }
  }
}