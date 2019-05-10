import { Component, OnInit, Input } from "@angular/core";
import { UserService } from "src/app/_services/user.service";
import { User } from "src/app/_models/User";
import { Store } from '@ngrx/store';
import { UserState } from 'src/app/_store/user.reducer';

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

  constructor(
    private userService: UserService  ) {}

  ngOnInit() {}

  followUser() {
    this.userService.followUser(this.profile.id, this.currentUser.id);
    this.currentUser.isFollowedByCurrentUser = true;
    this.isFollowing = this.currentUser.isFollowedByCurrentUser;
  }

  unFollowUser() {
    this.userService.unFollowUser(this.profile.id, this.currentUser.id);
    this.isFollowing = this.currentUser.isFollowedByCurrentUser;
  }
}
