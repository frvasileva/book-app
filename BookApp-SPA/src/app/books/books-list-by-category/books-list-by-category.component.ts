import { Component, OnInit } from "@angular/core";
import { BookService } from "src/app/_services/book.service";
import { ActivatedRoute, Params, Router } from "@angular/router";
import { SeoHelperService } from "src/app/_shared/seo-helper.service";

@Component({
  selector: "app-books-list-by-category",
  templateUrl: "./books-list-by-category.component.html",
  styleUrls: ["./books-list-by-category.component.scss"]
})
export class BooksListByCategoryComponent implements OnInit {
  books: any;
  currentPage: number;
  totalItems: number;

  isPageChanged = false;
  startItem = 0;
  currentGridPage = 0;
  itemsPerPage = 12;
  category: string;

  constructor(
    private bookService: BookService,
    private router: Router,
    private route: ActivatedRoute,
    private seoService: SeoHelperService
  ) {}

  ngOnInit() {
    this.route.params.subscribe((params: Params) => {
      this.category = params.category.trim().toUpperCase();
      this.currentPage = params.pageNumber;
      this.bookService
        .getBooksByCategory(this.category, this.currentPage)
        .subscribe(data => {
          this.books = data.items;
          this.totalItems = data.totalNumber;
        });

      this.seoService.setSeoMetaTags(this.category);
    });
  }

  pageChanged(event: any): void {
    this.currentGridPage = event.page;
    this.router.navigate([
      "/books/category/" + this.category.toLowerCase() + "/",
      this.currentGridPage
    ]);
  }
}
