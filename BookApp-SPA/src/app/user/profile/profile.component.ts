import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { Store } from "@ngrx/store";
import { Title, Meta } from "@angular/platform-browser";

import { Profile } from "src/app/_models/profile";
import { Book } from "src/app/books/book.model";
import { UserService } from "src/app/_services/user.service";
import { UserState } from "src/app/_store/user.reducer";
import { settings } from "src/app/_shared/settings";
import { BookService } from "src/app/_services/book.service";
import { TabDirective } from "ngx-bootstrap/tabs";

import * as BookActions from "../../_store/book.actions";

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

  userBooks: any;

  constructor(
    private userService: UserService,
    private bookService: BookService,
    private route: ActivatedRoute,
    private store: Store<{ userState: UserState }>,
    private titleService: Title,
    private metaTagService: Meta
  ) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.friendlyUrl = params["friendlyUrl"];
      console.log(" this.friendlyUrl", this.friendlyUrl);
      this.store
        .select(state => state.userState)
        .subscribe(userState => {
          this.profile = userState.users.find(
            u => u.friendlyUrl === this.friendlyUrl
          );
          this.currentUser = userState.users.find(
            u => u.friendlyUrl === userState.currentUser
          );
          this.isCurrentUser = userState.currentUser === this.friendlyUrl;

          if (!this.profile) {
            this.userService.getUser(this.friendlyUrl);
          }
        });

      this.setSeoMetaTags();
    });
  }

  getUserBooks() {
    const result = this.bookService
      .getBooksAddedByUser(this.friendlyUrl)
      .subscribe(data => {
        console.log("this.friendlyUrl", this.friendlyUrl);
        this.store.dispatch(new BookActions.SetBooksAction(data));
        this.userBooks = data;
        console.log("books data", data);
      });
  }

  onSelect(data: TabDirective): void {
    if (data.id === "booksTab") {
      this.getUserBooks();
    }
  }
  setBooks() {
    console.log("dataaa books");
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
