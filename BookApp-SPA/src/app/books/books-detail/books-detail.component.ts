import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Params } from "@angular/router";

import { BookService } from "../../_services/book.service";
import { Meta, Title } from "@angular/platform-browser";
import { AlertifyService } from "../../_services/alertify.service";
import { settings } from "../../_shared/settings";

@Component({
  selector: "app-books-detail",
  templateUrl: "./books-detail.component.html",
  styleUrls: ["./books-detail.component.scss"]
})
export class BooksDetailComponent implements OnInit {
  friendlyUrl: string;
  book: any;
  similiarBooks: [];
  dummyBookDescription = settings.dummy_book_description;

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
          this.setSeoMetaTags(this.book.title);
        },
        error => {
          this.alertify.error(error);
        }
      );

      this.bookService
        .RecommendSimiliarBooks(this.friendlyUrl)
        .subscribe(data => {
          this.similiarBooks = data;
        });
    });
  }

  setSeoMetaTags(bookTitle: string = "") {
    this.titleService.setTitle(bookTitle + settings.seo_appName_title);
    this.metaTagService.updateTag({
      name: "description",
      content: bookTitle + "| Books" + settings.seo_appName_title
    });
  }
}
