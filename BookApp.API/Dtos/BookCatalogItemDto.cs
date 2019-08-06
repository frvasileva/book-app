using System;

namespace BookApp.API.Dtos {
    public class BookCatalogItemDto {
        public long Id { get; set; }
        public long BookId { get; set; }
        public long CatalogId { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }
        public DateTime Created { get; set; }
        public int UserId { get; set; }

    }
}