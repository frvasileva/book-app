import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { Store } from "@ngrx/store";

import { Profile } from "src/app/_models/profile";
import { Book } from "src/app/books/book.model";
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: "app-profile",
  templateUrl: "./profile.component.html",
  styleUrls: ["./profile.component.scss"]
})
export class ProfileComponent implements OnInit {
  profile: Profile;
  books: Book[];
  friendlyUrl: string;
  isCurrentUser: boolean;

  constructor(
    private userService: UserService,
    private route: ActivatedRoute,
    private store: Store<{ userProfile: Profile }>
  ) {}

  ngOnInit() {
    this.friendlyUrl = this.route.snapshot.params["friendlyUrl"];

    this.store
      .select(state => state)
      .subscribe(res => {
        this.profile = res.userProfile;
        this.isCurrentUser = this.profile.friendlyUrl === this.friendlyUrl;
      });

    if (!this.isCurrentUser) {
      this.userService.getUser(this.friendlyUrl);
    }
  }
}
