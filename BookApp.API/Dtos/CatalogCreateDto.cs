using IdGen;

namespace BookApp.API.Dtos {
  public class CatalogCreateDto {

    public CatalogCreateDto () {
      var generator = new IdGenerator (0);
      this.Id = generator.CreateId ();     
    }
    public long Id { get; set; }

    public string Name { get; set; }

    public bool IsPublic { get; set; }

    public int UserId { get; set; }

    public string FriendlyUrl { get; set; }

  }
}