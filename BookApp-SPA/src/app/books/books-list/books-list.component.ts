import { Component, OnInit } from "@angular/core";
import { Title, Meta } from "@angular/platform-browser";
import { settings } from "src/app/_shared/settings";
import { BookService } from "src/app/_services/book.service";

@Component({
  selector: "app-books-list",
  templateUrl: "./books-list.component.html",
  styleUrls: ["./books-list.component.scss"]
})
export class BooksListComponent implements OnInit {
  booksByRelevance: any;
  booksByNovelty: any;
  booksBySerendipity: any;

  constructor(
    private bookService: BookService,
    private titleService: Title,
    private metaTagService: Meta
  ) {}

  ngOnInit() {
    this.bookService.RecommendByNovelty(0);
    this.bookService.RecommendByRelevance(0);
    this.bookService.RecommendBySerendipity(0);

    this.titleService.setTitle("Books" + settings.seo_appName_title);
    this.metaTagService.updateTag({
      name: "description",
      content: "Books" + settings.seo_appName_title
    });

    this.bookService.RecommendByRelevance(0).subscribe(data => {
      this.booksByRelevance = data.items;
    });
    this.bookService.RecommendByNovelty(0).subscribe(data => {
      this.booksByNovelty = data.items;
    });
    this.bookService.RecommendBySerendipity(0).subscribe(data => {
      this.booksBySerendipity = data.items;
    });
  }
}
