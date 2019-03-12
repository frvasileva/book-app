import { Component, OnInit } from "@angular/core";
import { Book } from "../book.model";
import { Subscription, Observable } from "rxjs";
import { Store } from "@ngrx/store";
import { BookService } from "src/app/_services/book.service";
import { BookCatalogService } from "src/app/_services/book-catalog.service";
import { CatalogItemDto } from "src/app/_models/catalogItem";

@Component({
  selector: "app-books-list",
  templateUrl: "./books-list.component.html",
  styleUrls: ["./books-list.component.scss"]
})
export class BooksListComponent implements OnInit {
  bookListState: Observable<{ books: Book[] }>;
  catalogsState: Observable<{ catalogs: CatalogItemDto[] }>;

  constructor(
    private bookService: BookService,
    private store: Store<{ bookList: { books: Book[] } }>,
    private bookCatalog: BookCatalogService
  ) {}

  ngOnInit() {
    this.bookService.getBooks();
    this.bookCatalog.getBooks();

    this.bookListState = this.store.select("bookList");
    this.catalogsState = this.store.select("catalog");

    console.log("book store on init", this.bookListState);
    console.log("catalog store on init", this.catalogsState);
  }

  addToWantToReadList(bookId) {
    console.log(bookId);
    // this.bookService.addToWantToReadList(+bookId);
  }

  addToAlreadyRead(bookId) {
    console.log("book {0} added to already read books", +bookId);
  }

  wantToShare(bookId) {
    console.log("User want to share book {0}", +bookId);
  }
}
