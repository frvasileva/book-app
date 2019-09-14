using System;

namespace BookApp.API.Dtos {
    public class CatalogEditDto {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }

    }
}