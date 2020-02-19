import { Component, OnInit } from "@angular/core";
import { BookService } from "../../_services/book.service";
import { SeoHelperService } from "../../_shared/seo-helper.service";
import { AuthService } from "src/app/_services/auth.service";

@Component({
  selector: "app-books-list",
  templateUrl: "./books-list.component.html",
  styleUrls: ["./books-list.component.scss"]
})
export class BooksListComponent implements OnInit {
  booksByRelevance: any;
  booksByNovelty: any;
  booksBySerendipity: any;
  isUserAuthenticated: boolean;
  
  constructor(
    private bookService: BookService,
    private seoService: SeoHelperService,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.isUserAuthenticated = this.authService.isAuthenticated();

    this.bookService.RecommendByRelevance(0).subscribe(data => {
      this.booksByRelevance = data.items;
    });

    this.bookService.RecommendByNovelty(0).subscribe(dataNovelty => {
      this.booksByNovelty = dataNovelty.items;
    });

    this.bookService.RecommendBySerendipity(0).subscribe(dataSerendepity => {
      this.booksBySerendipity = dataSerendepity.items;
    });
    this.seoService.setSeoMetaTags("Books");
  }
}
