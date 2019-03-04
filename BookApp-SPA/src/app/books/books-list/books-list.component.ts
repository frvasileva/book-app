import { Component, OnInit } from "@angular/core";
import { Book } from "../book.model";
import { Subscription, Observable } from "rxjs";
import { Store } from "@ngrx/store";
import { BookService } from "src/app/_services/book.service";

@Component({
  selector: "app-books-list",
  templateUrl: "./books-list.component.html",
  styleUrls: ["./books-list.component.scss"]
})
export class BooksListComponent implements OnInit {
  bookListState: Observable<{ books: Book[] }>;
  subscription: Subscription;

  constructor(
    private bookService: BookService,
    private store: Store<{ bookList: { books: Book[] } }>
  ) {}

  ngOnInit() {
    this.bookService.getBooks();

    this.bookListState = this.store.select("bookList");

    console.log("book store on init", this.bookListState);
  }

  addToWantToReadList(bookId) {
    console.log(bookId);
    //this.bookService.addToWantToReadList(+bookId);
  }

  addToAlreadyRead(bookId) {
    console.log("book {0} added to already read books", +bookId);
  }

  wantToShare(bookId) {
    console.log("User want to share book {0}", +bookId);
  }
}
