import { Component, OnInit } from "@angular/core";
import { Book } from "../book.model";
import { Observable } from "rxjs";
import { Store } from "@ngrx/store";
import { BookCatalogService } from "src/app/_services/book-catalog.service";
import { CatalogItemDto } from "src/app/_models/catalogItem";
import { Title, Meta } from "@angular/platform-browser";
import { settings } from "src/app/_shared/settings";

@Component({
  selector: "app-books-list",
  templateUrl: "./books-list.component.html",
  styleUrls: ["./books-list.component.scss"]
})
export class BooksListComponent implements OnInit {
  bookListState: Observable<{ books: Book[] }>;
  catalogsState: Observable<{ catalogs: CatalogItemDto[] }>;

  constructor(
    private store: Store<{
      bookState: { books: Book[] };
      catalog: { catalogs: CatalogItemDto[] };
    }>,
    private catalogService: BookCatalogService,
    private titleService: Title,
    private metaTagService: Meta
  ) {}

  ngOnInit() {
    this.titleService.setTitle("Books" + settings.seo_appName_title);
    this.metaTagService.updateTag({
      name: "description",
      content: "Books" + settings.seo_appName_title
    });

    this.catalogService.getCatalog(1);
    this.bookListState = this.store.select("bookState");
    this.catalogsState = this.store.select("catalog");
  }
}
