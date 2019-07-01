import { Component, OnInit, Input } from "@angular/core";
import { UserService } from "src/app/_services/user.service";
import { User } from "src/app/_models/User";
import {
  FacebookService,
  UIParams,
  UIResponse,
  InitParams
} from "ngx-facebook";

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

  constructor(private userService: UserService, private fb: FacebookService) {
    let initParams: InitParams = {
      appId: "237494102958046",
      xfbml: true,
      version: "v2.8"
    };

    fb.init(initParams);
  }

  sharePage(url: string) {
    let params: UIParams = {
      href: "https://github.com/zyra/ngx-facebook",
      method: "send",
      link: url
    };

    this.fb
      .ui(params)
      .then((res: UIResponse) => console.log(res))
      .catch((e: any) => console.error(e));
  }

  ngOnInit() {
    this.isFollowing = this.profile.isFollowedByCurrentUser;
  }

  followUser() {
    this.userService.followUser(this.profile.id);
    this.isFollowing = this.currentUser.isFollowedByCurrentUser;
  }

  unFollowUser() {
    this.userService.unFollowUser(this.profile.id, this.profile.friendlyUrl);
    this.isFollowing = !this.currentUser.isFollowedByCurrentUser;
  }
}
