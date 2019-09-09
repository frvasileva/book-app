import { Component, OnInit, Input } from "@angular/core";
import { UserService } from "src/app/_services/user.service";
import { User } from "src/app/_models/User";


@Component({
  selector: "app-profile-preview-card",
  templateUrl: "./profile-preview-card.component.html",
  styleUrls: ["./profile-preview-card.component.scss"]
})
export class ProfilePreviewCardComponent implements OnInit {
  @Input() profile: User;
  @Input() currentUser: User;
  @Input() isCurrentUser: boolean;
  isFollowing: boolean;

  constructor(private userService: UserService) {}

  ngOnInit() {
    this.isFollowing = this.profile.isFollowedByCurrentUser;
  }

  followUser() {
    this.userService.followUser(this.profile.id);
    this.isFollowing = true;
  }

  unFollowUser() {
    this.userService.unFollowUser(this.profile.id, this.profile.friendlyUrl);
    this.isFollowing = false;
  }
}
