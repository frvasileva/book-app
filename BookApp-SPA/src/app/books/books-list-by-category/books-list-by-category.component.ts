import { Component, OnInit } from "@angular/core";
import { Location } from "@angular/common";

import { Book } from "../book.model";
import { Store } from "@ngrx/store";
import { BookService } from "src/app/_services/book.service";
import { ActivatedRoute, Params } from "@angular/router";
import { SeoHelperService } from "src/app/_shared/seo-helper.service";

@Component({
  selector: "app-books-list-by-category",
  templateUrl: "./books-list-by-category.component.html",
  styleUrls: ["./books-list-by-category.component.scss"]
})
export class BooksListByCategoryComponent implements OnInit {
  books: any;

  url: string;
  currentPage: number;
  totalItems: number;
  queryMade = false;

  isPageChanged = false;
  startItem = 0;
  currentGridPage = 0;
  itemsPerPage = 12;

  constructor(
    private store: Store<{ bookState: { books: Book[]; totalNumber: number } }>,
    private bookService: BookService,
    private route: ActivatedRoute,
    private location: Location,
    private seoService: SeoHelperService
  ) {}

  ngOnInit() {
    this.route.params.subscribe((params: Params) => {
      this.url = params.category.trim().toUpperCase();
      this.currentPage = params.pageNumber;
      this.selectCategoryToShow();
    });

    this.seoService.setSeoMetaTags(this.url);
  }

  pageChanged(event: any): void {
    this.currentGridPage = event.page - 1;

    let buildUrl =
      "/book/" + this.url.toLocaleLowerCase() + "/" + this.currentGridPage;
    this.location.replaceState(buildUrl);

    this.isPageChanged = true;
    this.selectCategoryToShow();
  }

  selectCategoryToShow() {
    this.store.subscribe(state => {
      this.totalItems = state.bookState.totalNumber;
      if (this.url === undefined) {
        return;
      }

      this.startItem = this.itemsPerPage * this.currentGridPage;

      this.books = state.bookState.books
        .filter(b => b.recommendationCategory === this.url)
        .slice(this.startItem, this.startItem + this.itemsPerPage);

      if ((this.books.length === 0 && !this.queryMade) || this.isPageChanged) {
        this.queryMade = true;
        this.isPageChanged = false;
        this.bookService.getBooksByCategory(this.url, this.currentGridPage);
      }
    });
  }
}
