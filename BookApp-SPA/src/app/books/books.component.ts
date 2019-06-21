import { Component, OnInit } from "@angular/core";
import { BookService } from "../_services/book.service";
import { BookSaverService } from "../_services/bookSaver.service";
import { Store } from "@ngrx/store";
import { UserState } from "../_store/user.reducer";
import { CatalogPureDto } from "../_models/catalogPureDto";

@Component({
  selector: "app-books",
  templateUrl: "./books.component.html",
  styleUrls: ["./books.component.scss"]
})
export class BooksComponent implements OnInit {
  currentUserUrl: string;

  constructor(
    private bookService: BookService,
    private bookSaverService: BookSaverService,
    private store: Store<{ userState: UserState }>
  ) {}

  ngOnInit() {
    this.bookService.getBooks();
    this.bookSaverService.getUserCatalogList(this.currentUserUrl);

    this.store.subscribe(next => {
      this.currentUserUrl = next.userState.currentUser;

      // if (this.currentUserUrl !== "") {
      //   this.bookSaverService.getUserCatalogList(this.currentUserUrl);
      // }
    });
  }
}
