namespace BookApp.API.Dtos {
  public class CatalogCreateDto {

    public CatalogCreateDto () {
      var generator = new IdGeneratorHelper ();
      this.Id = generator.Generate ();
    }
    public int Id { get; set; }

    public string Name { get; set; }

    public bool IsPublic { get; set; }

    public int UserId { get; set; }

    public string FriendlyUrl { get; set; }

  }
}