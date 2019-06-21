import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { Store } from "@ngrx/store";

import { Profile } from "src/app/_models/profile";
import { Book } from "src/app/books/book.model";
import { UserService } from "src/app/_services/user.service";
import { UserState } from "src/app/_store/user.reducer";
import { Title, Meta } from "@angular/platform-browser";
import { settings } from "src/app/_shared/settings";

@Component({
  selector: "app-profile",
  templateUrl: "./profile.component.html",
  styleUrls: ["./profile.component.scss"]
})
export class ProfileComponent implements OnInit {
  profile: Profile;
  currentUser: Profile;
  books: Book[];
  friendlyUrl: string;
  isCurrentUser: boolean;

  constructor(
    private userService: UserService,
    private route: ActivatedRoute,
    private store: Store<{ userState: UserState }>,
    private titleService: Title,
    private metaTagService: Meta
  ) {}

  ngOnInit() {
    this.friendlyUrl = this.route.snapshot.params["friendlyUrl"];

    this.store
      .select(state => state.userState)
      .subscribe(userState => {
        this.profile = userState.users[this.friendlyUrl];

        console.log("profile", this.profile);

        this.currentUser = userState.users[userState.currentUser];
        this.isCurrentUser = userState.currentUser === this.friendlyUrl;

        if (!this.isCurrentUser && this.profile == null) {
          this.userService.getUser(this.friendlyUrl);
        }
      });

    this.setSeoMetaTags();
  }

  setSeoMetaTags() {
    this.titleService.setTitle(
      this.profile.knownAs + settings.seo_appName_title
    );
    this.metaTagService.updateTag({
      name: "description",
      content: this.profile.knownAs
    });
  }
}
