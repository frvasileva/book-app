import { Component, OnInit } from "@angular/core";
import { Book } from "../book.model";
import { Observable } from "rxjs";
import { Store } from "@ngrx/store";
import { CatalogItemDto } from "src/app/_models/catalogItem";
import { Title, Meta } from "@angular/platform-browser";
import { settings } from "src/app/_shared/settings";
import { BookService } from "src/app/_services/book.service";

@Component({
  selector: "app-books-list",
  templateUrl: "./books-list.component.html",
  styleUrls: ["./books-list.component.scss"]
})
export class BooksListComponent implements OnInit {
  bookListState: Observable<{ books: Book[] }>;
  catalogsState: Observable<{ catalogs: CatalogItemDto[] }>;
  booksByRelevance: any;
  booksByNovelty: any;
  booksBySerendipity: any;

  constructor(
    private store: Store<{
      bookState: { books: Book[] };
      catalog: { catalogs: CatalogItemDto[] };
    }>,
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

    this.store.subscribe(state => {
      this.booksByRelevance = state.bookState.books.filter(
        b => b.recommendationCategory === "RELEVANCE"
      );
      this.booksByNovelty = state.bookState.books.filter(
        b => b.recommendationCategory === "NOVELTY"
      );
      this.booksBySerendipity = state.bookState.books.filter(
        b => b.recommendationCategory === "SERENDIPITY"
      );
    });

    // this.catalogService.getCatalog('1');
    this.bookListState = this.store.select("bookState");
    this.catalogsState = this.store.select("catalog");
  }
}
