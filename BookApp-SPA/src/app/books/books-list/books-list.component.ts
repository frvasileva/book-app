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
  items: any;
  constructor(
    private bookService: BookService,
    private store: Store<{
      bookList: { books: Book[] };
      catalog: { catalogs: CatalogItemDto[] };
    }>,
    private catalogService: BookCatalogService
  ) {}

  ngOnInit() {
    this.bookService.getBooks();
    this.catalogService.getCatalog(1);

    this.bookListState = this.store.select("bookList");
    this.catalogsState = this.store.select("catalog");

    this.store.select(state => state.catalog).subscribe(res => {
        this.items = res.catalogs as CatalogItemDto[];
      });

    console.log("catalog store on init", this.items);
    console.log("catalog STATE", this.catalogsState);
  }

  addToWantToReadList(bookId) {
    console.log(bookId);
    // this.bookService.addToWantToReadList(+bookId);
  }

  addToAlreadyRead(bookId) {
    console.log("book {0} added to already read books", +bookId);
  }

  wantToShare(bookId) {
    console.log("User want to share book {0}", +bookId);
  }
}
