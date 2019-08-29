import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Params } from "@angular/router";
import { Store, select } from "@ngrx/store";

import { BookService } from "src/app/_services/book.service";
import { bookDetailsDto } from "src/app/_models/bookDetailsDto";
import { Meta, Title } from "@angular/platform-browser";
import { BookSaverService } from "src/app/_services/bookSaver.service";

@Component({
  selector: "app-books-detail",
  templateUrl: "./books-detail.component.html",
  styleUrls: ["./books-detail.component.scss"]
})
export class BooksDetailComponent implements OnInit {
  friendlyUrl: string;
  book: any;

  constructor(
    private route: ActivatedRoute,
    private bookService: BookService,
    private bookSaverService: BookSaverService,
    private store: Store<{ bookState: { books: bookDetailsDto[] } }>,
    private titleService: Title,
    private metaTagService: Meta
  ) {}

  ngOnInit() {
    this.route.params.subscribe((params: Params) => {
      this.friendlyUrl = params.url;
      this.store
        .pipe(
          select(
            (state: { bookState: { books: bookDetailsDto[] } }) =>
              state.bookState.books
          )
        )
        .subscribe(books => {
          this.book = books.find(book => book.friendlyUrl === this.friendlyUrl);
          if (this.book) {
            this.titleService.setTitle(this.book.title);
            this.metaTagService.updateTag({
              name: "description",
              content: this.book.description
            });

            console.log("book?.catalogList", this.book.bookCatalogs);
          } else {
            this.bookService.getBook(this.friendlyUrl);
            this.bookSaverService.getUserCatalogList(this.friendlyUrl);
          }

          console.log("book?.catalogList", this.book);

        });
    });
  }
}
