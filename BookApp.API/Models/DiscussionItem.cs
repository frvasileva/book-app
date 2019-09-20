using System;

namespace BookApp.API.Models {
    public class DiscussionItem {
        public DiscussionItem () {
            this.AddedOn = DateTime.Now;
        }
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime AddedOn { get; set; }
        public int UserId { get; set; }
        public int DiscussionId { get; set; }
        public Discussion Discussion { get; set; }
    }
}