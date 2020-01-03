import { Component, OnInit, Input } from "@angular/core";
import { UserService } from "../../_services/user.service";
import { User } from "../../_models/user";

@Component({
  selector: "app-profile-card",
  templateUrl: "./profile-card.component.html",
  styleUrls: ["./profile-card.component.scss"]
})
export class ProfileCardComponent implements OnInit {
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
