using System;

namespace BookApp.API.Dtos
{
  public class CatalogCreateDto
  {
    public int Id { get; set; }

    public string Name { get; set; }

    public bool IsPublic { get; set; }

    public int UserId { get; set; }
  }
}