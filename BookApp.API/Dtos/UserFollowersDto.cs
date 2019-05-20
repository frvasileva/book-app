namespace BookApp.API.Dtos {
    public class UserFollowersDto {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FollowerUserId { get; set; }
        public string FollowerFriendlyUrl { get; set; }
    }
}