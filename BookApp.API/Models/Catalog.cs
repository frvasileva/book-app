using System;
using System.Collections.Generic;

namespace BookApp.API.Models {
    public class Catalog {

        public Catalog () {
            var idGen = new IdGeneratorHelper ();
            this.Id = idGen.Generate ();
        }

        public int Id { get; set; }
        public int? ExternalId { get; set; }

        public string Name { get; set; }

        public bool IsPublic { get; set; }

        public DateTime AddedOn { get; set; }

        public string FriendlyUrl { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public ICollection<BookCatalog> BookCatalogs { get; set; }
    }
}