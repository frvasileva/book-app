import { Component, OnInit } from "@angular/core";
import { AuthService } from "src/app/_services/auth.service";
import { SeoHelperService } from "src/app/_shared/seo-helper.service";
import { BookService } from "src/app/_services/book.service";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
  styleUrls: ["./home.component.scss"]
})
export class HomeComponent implements OnInit {
  isAuthenticated: boolean;
  bookList: any;

  constructor(
    private authService: AuthService,
    private seoService: SeoHelperService,
    private bookService: BookService
  ) {}

  ngOnInit() {
    this.isAuthenticated = this.authService.isAuthenticated();
    this.seoService.setSeoMetaTags("📖");

    this.bookService.RecommendByRelevance(0).subscribe(data => {
      this.bookList = data.items.splice(0, 6);
    });
  }
}
