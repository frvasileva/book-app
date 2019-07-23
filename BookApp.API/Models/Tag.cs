using System;

namespace BookApp.API.Models {
    public class Tag {
        public int Id { get; set; }
        public int ExternalId { get; set; }
        public string Name { get; set; }
        public DateTime AddedOn { get; set; }
        public string FriendlyUrl { get; set; }

    }
}