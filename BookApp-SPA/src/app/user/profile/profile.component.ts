import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { Store } from "@ngrx/store";
import { Profile } from "src/app/_models/profile";
import { Book } from "src/app/books/book.model";
import { UserService } from "src/app/_services/user.service";
import { UserState } from "src/app/_store/user.reducer";
import { BookService } from "src/app/_services/book.service";
import { TabDirective } from "ngx-bootstrap/tabs";
import { SeoHelperService } from "src/app/_shared/seo-helper.service";
import { AuthService } from "src/app/_services/auth.service";

@Component({
  selector: "app-profile",
  templateUrl: "./profile.component.html",
  styleUrls: ["./profile.component.scss"]
})
export class ProfileComponent implements OnInit {
  profile: any;
  currentUser: Profile;
  books: Book[];
  friendlyUrl: string;
  isCurrentUser: boolean;
  bookNumber: number;
  userBookCategoryPreferences: [];

  userBooks: any;

  constructor(
    private userService: UserService,
    private authService: AuthService,
    private bookService: BookService,
    private route: ActivatedRoute,
    private store: Store<{ userState: UserState }>,
    private seoHelper: SeoHelperService
  ) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.friendlyUrl = params["friendlyUrl"];
      this.store
        .select(state => state.userState)
        .subscribe(userState => {
          this.userService.getUser(this.friendlyUrl).subscribe(data => {
            this.profile = data;
            this.seoHelper.setSeoMetaTags(this.profile.knownAs);
          });
          this.currentUser = userState.users.find(
            u => u.friendlyUrl === userState.currentUser
          );
          this.isCurrentUser = userState.currentUser === this.friendlyUrl;
        });
    });
  }

  getUserBooks() {
    this.bookService.getBooksAddedByUser(this.friendlyUrl).subscribe(data => {
      this.userBooks = data;
      console.log(data);
      this.bookNumber = this.userBooks.length;
    });
  }

  getUserPreferences() {
    this.userService.getUserSelectedPreferencesCatalogs().subscribe(data => {
      this.userBookCategoryPreferences = data as [];
      console.log(data);
    });
  }

  onSelect(data: TabDirective): void {
    if (data.id === "booksTab") {
      this.getUserBooks();
    }
    if (data.id === "preferencesTab") {
      this.getUserPreferences();
    }
  }

  togglePreferences(id: number) {
    this.userService.toggleUserPreferencesCatalogs(id, 1).subscribe();
    console.log("item clicked", id);
  }
}
