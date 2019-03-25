import { Component, OnInit } from "@angular/core";
import { ProfileService } from "src/app/_services/profile.service";
import { ActivatedRoute } from "@angular/router";
import { Profile } from "src/app/_models/profile";
import { AlertifyService } from "src/app/_services/alertify.service";
import { Store } from "@ngrx/store";
import { Book } from "src/app/books/book.model";

@Component({
  selector: "app-profile",
  templateUrl: "./profile.component.html",
  styleUrls: ["./profile.component.scss"]
})
export class ProfileComponent implements OnInit {
  profile: Profile;
  books: Book[];

  constructor(
    private profileService: ProfileService,
    private route: ActivatedRoute,
    private alertify: AlertifyService,
    private store: Store<{
      userProfile: Profile;
      bookList: { books: Book[] };
    }>
  ) {}

  ngOnInit() {
    // TODO get userId from route ( get friendly URL)
    // this.profileService.getUserProfile("16");

    this.store
      .select(state => state)
      .subscribe(res => {
        this.profile = res.userProfile as Profile;

        const result = res.bookList.books as Book[];
        this.books = result.filter(
          item => item.userId == this.profile.id.toString()
        );
      });
  }
}
