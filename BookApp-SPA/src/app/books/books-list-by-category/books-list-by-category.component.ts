import { Component, OnInit } from "@angular/core";
import { Book } from "../book.model";
import { Store } from "@ngrx/store";
import { CatalogItemDto } from "src/app/_models/catalogItem";
import { Title, Meta } from "@angular/platform-browser";
import { BookService } from "src/app/_services/book.service";
import { Observable } from "rxjs";
import { settings } from "src/app/_shared/settings";
import { ActivatedRoute, Params, Router } from "@angular/router";

@Component({
  selector: "app-books-list-by-category",
  templateUrl: "./books-list-by-category.component.html",
  styleUrls: ["./books-list-by-category.component.scss"]
})
export class BooksListByCategoryComponent implements OnInit {
  bookListState: Observable<{ books: Book[] }>;
  catalogsState: Observable<{ catalogs: CatalogItemDto[] }>;
  booksByRelevance: any;
  booksByNovelty: any;
  booksBySerendipity: any;


  url: string;
  currentPage: number;
  totalItems: number;
  queryMade = false;

  currentGridPage: number;

  constructor(
    private store: Store<{
      bookState: { books: Book[]; totalNumber: number };
    }>,
    private bookService: BookService,
    private titleService: Title,
    private metaTagService: Meta,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    this.route.params.subscribe((params: Params) => {
      this.url = params.category;
      this.currentPage = params.pageNumber;
      console.log("current page", params.pageNumber);
    });

    this.store.subscribe(state => {
      this.totalItems = state.bookState.totalNumber;
      this.selectCategoryToShow(state, this.currentPage);
    });

    this.setSeoMetaTags();
  }

  pageChanged(event: any): void {
    this.currentGridPage = event.page;
    this.bookService.RecommendByRelevance(this.currentGridPage - 1);
  }

  selectCategoryToShow(state, currentPage) {
    console.log("URL", this.url.trim().toUpperCase());
    if (this.url === undefined) {
      return;
    }

    switch (this.url.trim().toUpperCase()) {
      case "RELEVANCE": {
        this.booksByRelevance = state.bookState.books
          .filter(b => b.recommendationCategory === "RELEVANCE")
          .slice(this.currentGridPage + 12, 12);

        if (this.booksByRelevance.length === 0 && !this.queryMade) {
          this.queryMade = true;
          this.bookService.RecommendByRelevance(currentPage);
        }
        break;
      }
      case "NOVELTY": {
        this.booksByNovelty = state.bookState.books.filter(
          b => b.recommendationCategory === "NOVELTY"
        );

        if (this.booksByRelevance.length === 0 && !this.queryMade) {
          this.queryMade = true;
          this.bookService.RecommendByNovelty(currentPage);
        }
        break;
      }
      case "SERENDIPITY": {
        this.booksBySerendipity = state.bookState.books.filter(
          b => b.recommendationCategory === "SERENDIPITY"
        );

        if (this.booksByRelevance.length === 0 && !this.queryMade) {
          this.queryMade = true;
          this.bookService.RecommendBySerendipity(currentPage);
        }
        break;
      }
      default:
        this.router.navigate(["/books"]);
        break;
    }
  }

  setSeoMetaTags() {
    this.titleService.setTitle("Books" + settings.seo_appName_title);
    this.metaTagService.updateTag({
      name: "description",
      content: "Books" + settings.seo_appName_title
    });
  }
}
