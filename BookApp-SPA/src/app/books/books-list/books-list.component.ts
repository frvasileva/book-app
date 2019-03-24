import { Component, OnInit } from "@angular/core";
import { Book } from "../book.model";
import { Subscription, Observable } from "rxjs";
import { Store } from "@ngrx/store";
import { BookService } from "src/app/_services/book.service";
import { BookCatalogService } from "src/app/_services/book-catalog.service";
import { CatalogItemDto } from "src/app/_models/catalogItem";

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
      bookList: { books: Book[] };
      catalog: { catalogs: CatalogItemDto[] };
    }>,
    private catalogService: BookCatalogService
  ) {}

  ngOnInit() {

    this.catalogService.getCatalog(1);
    this.bookListState = this.store.select("bookList");
    this.catalogsState = this.store.select("catalog");

  }


}
