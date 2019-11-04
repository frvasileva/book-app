import { Component, OnInit } from "@angular/core";
import { Location } from "@angular/common";

import { Book } from "../book.model";
import { Store } from "@ngrx/store";
import { Title, Meta } from "@angular/platform-browser";
import { BookService } from "src/app/_services/book.service";
import { settings } from "src/app/_shared/settings";
import { ActivatedRoute, Params, Router } from "@angular/router";

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
    private store: Store<{
      bookState: { books: Book[]; totalNumber: number };
    }>,
    private bookService: BookService,
    private titleService: Title,
    private metaTagService: Meta,
    private route: ActivatedRoute,
    private router: Router,
    private location: Location
  ) {}

  ngOnInit() {
    this.route.params.subscribe((params: Params) => {
      this.url = params.category.trim().toUpperCase();
      this.currentPage = params.pageNumber;
      this.selectCategoryToShow();
    });

    this.setSeoMetaTags();
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

  setSeoMetaTags() {
    this.titleService.setTitle("Books" + settings.seo_appName_title);
    this.metaTagService.updateTag({
      name: "description",
      content: "Books" + settings.seo_appName_title
    });
  }
}
