namespace BookApp.API.Dtos {
    public class BookCatalogCreateDto {
        public int BookId { get; set; }
        public long? CatalogId { get; set; }
        public string CatalogName { get; set; }

        public int UserId { get; set; }

    }
}