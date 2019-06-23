import { Component, OnInit } from "@angular/core";
import { BookService } from "../_services/book.service";
import { BookSaverService } from "../_services/bookSaver.service";
import { Store } from "@ngrx/store";
import { UserState } from "../_store/user.reducer";

@Component({
  selector: "app-books",
  templateUrl: "./books.component.html",
  styleUrls: ["./books.component.scss"]
})
export class BooksComponent implements OnInit {
  constructor(
    private bookService: BookService,
    private bookSaverService: BookSaverService,
    private store: Store<{ userState: UserState }>
  ) {}

  ngOnInit() {
    this.bookService.getBooks();

    this.store
      .select(state => state.userState)
      .subscribe(userState => {
        if (
          userState.currentUser &&
          userState.currentUserCatalogs.length === 0
        ) {
          this.bookSaverService.getUserCatalogList(userState.currentUser);
        }
      });
  }
}
