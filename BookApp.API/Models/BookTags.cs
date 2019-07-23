using System;

namespace BookApp.API.Models {
    public class BookTags {
        public int Id { get; set; }
        public int BookExternalId { get; set; }
        public int TagExternalId { get; set; }
        public int Count { get; set; }
        public DateTime AddedOn { get; set; }
    }
}