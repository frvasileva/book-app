import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { Store } from "@ngrx/store";

import { ProfileService } from "src/app/_services/profile.service";
import { Profile } from "src/app/_models/profile";
import { Book } from "src/app/books/book.model";

@Component({
  selector: "app-profile",
  templateUrl: "./profile.component.html",
  styleUrls: ["./profile.component.scss"]
})
export class ProfileComponent implements OnInit {
  profile: Profile;
  books: Book[];
  friendlyUrl: string;

  constructor(
    private profileService: ProfileService,
    private route: ActivatedRoute,
    private store: Store<{ userProfile: Profile }>
  ) {}

  ngOnInit() {
    this.friendlyUrl = this.route.snapshot.params["friendlyUrl"];

    this.store
      .select(state => state)
      .subscribe(res => {
        this.profile = res.userProfile;
        console.log("current user", this.profile);

        if (this.profile === null || this.friendlyUrl !== this.profile.friendlyUrl) {
          this.profileService.getUserProfile(this.friendlyUrl);
        }

        console.log(
          " this.profile === null",
          this.profile === null
        );
      });
  }
}
