namespace BookApp.API.Dtos {

  public class BookCatalogPreferencesDto {
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsSelected { get; set; }
    public int IconPath { get; set; }
  }
}