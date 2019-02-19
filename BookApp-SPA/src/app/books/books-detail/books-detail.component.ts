import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router, Params } from "@angular/router";
import { Book } from "../book.model";
import { BookService } from "../books.service";

@Component({
  selector: "app-books-detail",
  templateUrl: "./books-detail.component.html",
  styleUrls: ["./books-detail.component.scss"]
})
export class BooksDetailComponent implements OnInit {
  id: number;
  book: Book;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private bookService: BookService
  ) {}

  ngOnInit() {
    this.route.params.subscribe((params: Params) => {
      this.id = +params["id"];
      this.book = this.bookService.getBook(this.id);
      console.log(this.book);
    });
  }

  addToWantToReadList(bookId) {
    console.log(bookId);
    this.bookService.addToWantToReadList(+bookId);
  }

  addToAlreadyRead(bookId) {
    console.log("book {0} added to already read books", +bookId);
  }

  wantToShare(bookId) {
    console.log("User want to share book {0}", +bookId);
  }
}
