import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { Store } from "@ngrx/store";
import { Profile } from "../../_models/profile";
import { Book } from "../../books/book.model";
import { UserService } from "../../_services/user.service";
import { UserState } from "../../_store/user.reducer";
import { BookService } from "../../_services/book.service";
import { TabDirective } from "ngx-bootstrap/tabs";
import { SeoHelperService } from "../../_shared/seo-helper.service";

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
  userBookCategoryPreferences: any;
  defaultBookCategoryPreferences: any;
  mappedPreferences: [];
  userBooks: any;

  constructor(
    private userService: UserService,
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
      this.bookNumber = this.userBooks.length;
    });
  }

  getUserPreferences() {
    this.userService.getUserSelectedPreferencesCatalogs().subscribe(data => {
      this.userBookCategoryPreferences = data.userSelectedCategories as [];
      this.defaultBookCategoryPreferences = data.defaultCategories as [];
      this.defaultBookCategoryPreferences.forEach(value => {
        value.isSelected = this.userBookCategoryPreferences.some(
          item => item.name === value.name
        );

        if (value.isSelected) {
          value.catalogId = this.userBookCategoryPreferences.find(
            item => item.name === value.name
          ).id;
        }
      });
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

  togglePreferences(id: number, catalogName: string, isSelected: boolean) {
    const selected = isSelected ? 1 : 0;

    this.userService
      .toggleUserPreferencesCatalogs(id, catalogName, selected)
      .subscribe();

    let item = this.defaultBookCategoryPreferences.find(
      x => x.name === catalogName
    );

    item.isSelected = !isSelected;
  }
}
