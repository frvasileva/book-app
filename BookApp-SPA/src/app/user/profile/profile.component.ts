import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { Store } from "@ngrx/store";

import { Profile } from "src/app/_models/profile";
import { Book } from "src/app/books/book.model";
import { UserService } from "src/app/_services/user.service";
import { UserState } from "src/app/_store/user.reducer";

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
    private store: Store<{ userState: UserState }>
  ) {}

  ngOnInit() {
    this.friendlyUrl = this.route.snapshot.params["friendlyUrl"];

    this.store
      .select(state => state.userState)
      .subscribe(userState => {
        this.profile = userState.users[this.friendlyUrl];
        this.isCurrentUser = userState.currentUser === this.friendlyUrl;

        if (!this.isCurrentUser && this.profile == null) {
          this.userService.getUser(this.friendlyUrl);
        }
      });
  }
}
