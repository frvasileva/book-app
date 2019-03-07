import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router, Params } from "@angular/router";
import { BookService } from "src/app/_services/book.service";
import { Observable } from "rxjs";
import { Store, select } from "@ngrx/store";
import { tap, catchError } from "rxjs/operators";
import { bookDetailsDto } from "src/app/_models/bookDetailsDto";

@Component({
  selector: "app-books-detail",
  templateUrl: "./books-detail.component.html",
  styleUrls: ["./books-detail.component.scss"]
})
export class BooksDetailComponent implements OnInit {
  friendlyUrl: string;
  book: any;
  bookDetailsState: Observable<{ details: bookDetailsDto }>;
  test: Observable<{ bookDetails: { details: {} } }>;

  // bookListState: Observable<{ books: Book[] }>;


  constructor(
    private route: ActivatedRoute,
    private bookService: BookService,
    private store: Store<{ bookDetails: { details: bookDetailsDto } }>

  ) {}

  ngOnInit() {
    // this.bookService.getBook(this.friendlyUrl);
    //
    this.route.params.subscribe((params: Params) => {
      this.friendlyUrl = params["url"];
      this.bookService.getBook(this.friendlyUrl);
      this.bookDetailsState = this.store.select("bookDetails");

      this.test = this.store.select<{ bookDetails: { details: {} } }>("bookDetails");

      //  console.log(this.store.select(st => st.bookDetails.details).subscribe((data: any) => this.book = data ));

      //   this.store.pipe(select((state: any) => state.academy.academy)).subscribe((academy) => {
      //     this.test = academy;
      //  });

      this.store
        .pipe(select((state: any) => state.bookDetails.book))
        .subscribe(bookDetailsState => {
          this.book = bookDetailsState;
          console.log("from component", this.book);
          console.log("from bookDetailsState", bookDetailsState);
        });

      // const todos$ = _store.select<Observable<Todo[]>>('todos');

      console.log("test", this.test);
    });
  }
}
