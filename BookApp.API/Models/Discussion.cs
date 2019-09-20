using System;
using System.Collections.Generic;

namespace BookApp.API.Models {
    public class Discussion {
        public Discussion () {
            this.AddedOn = DateTime.Now;
        }
        
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime AddedOn { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public string FriendlyUrl { get; set; }
        public List<DiscussionItem> DiscussionItems { get; set; }
    }
}