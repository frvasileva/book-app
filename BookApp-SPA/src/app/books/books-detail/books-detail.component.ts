import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Params } from "@angular/router";
import { Store, select } from "@ngrx/store";

import { BookService } from "src/app/_services/book.service";
import { bookDetailsDto } from "src/app/_models/bookDetailsDto";
import { Meta, Title } from "@angular/platform-browser";
import { BookSaverService } from "src/app/_services/bookSaver.service";
import { AlertifyService } from "src/app/_services/alertify.service";

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
    private alertify: AlertifyService,
    private titleService: Title,
    private metaTagService: Meta
  ) {}

  ngOnInit() {
    this.route.params.subscribe((params: Params) => {
      this.friendlyUrl = params.url;

      this.bookService.getBook(this.friendlyUrl).subscribe(
        data => {
          this.book = data;
        },
        error => {
          this.alertify.error(error);
        }
      );
    });
  }
}
