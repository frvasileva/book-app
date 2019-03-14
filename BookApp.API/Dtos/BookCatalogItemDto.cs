using System;
using BookApp.API.Models;

namespace BookApp.API.Dtos {
    public class BookCatalogItemDto {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int CatalogId { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }
        public DateTime Created { get; set; }
    }
}