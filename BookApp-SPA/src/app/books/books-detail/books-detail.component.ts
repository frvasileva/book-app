import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Params } from "@angular/router";
import { Store, select } from "@ngrx/store";

import { BookService } from "src/app/_services/book.service";
import { bookDetailsDto } from "src/app/_models/bookDetailsDto";
import { Meta, Title } from "@angular/platform-browser";

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
    private store: Store<{ bookDetails: { details: bookDetailsDto } }>,
    private titleService: Title,
    private metaTagService: Meta
  ) {}

  ngOnInit() {
    this.route.params.subscribe((params: Params) => {
      this.friendlyUrl = params["url"];
      this.bookService.getBook(this.friendlyUrl);

      this.store
        .pipe(
          select((state: { bookDetails: { details: {} } }) => state.bookDetails)
        )
        .subscribe(result => {
          this.book = result;
          this.titleService.setTitle(this.book.title);
          this.metaTagService.updateTag({
            name: "description",
            content: this.book.description
          });
        });
    });
  }
}
