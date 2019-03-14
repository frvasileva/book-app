namespace BookApp.API.Dtos {
    public class BookCatalogCreateDto {
        public int BookId { get; set; }
        public int? CatalogId { get; set; }
        public string CatalogName { get; set; }

    }
}