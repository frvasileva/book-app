import { Component, OnInit, Input } from "@angular/core";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { Store } from "@ngrx/store";

import { BookSaverService } from "src/app/_services/bookSaver.service";
import { CatalogPureDto } from "src/app/_models/catalogPureDto";
import { UserState } from "src/app/_store/user.reducer";
import { Book } from "../book.model";

@Component({
  selector: "app-book-saver",
  templateUrl: "./book-saver.component.html",
  styleUrls: ["./book-saver.component.scss"]
})
export class BookSaverComponent implements OnInit {
  @Input() bookId: number;
  catalogs: CatalogPureDto[];
  addToListForm: FormGroup;
  currentUserUrl: string;

  constructor(
    private bookSaverService: BookSaverService,
    private store: Store<{
      userState: UserState;
      bookState: { books: Book[] };
    }>
  ) {}

  ngOnInit() {
    this.store.subscribe(state => {
      const book = state.bookState.books.find(b => b.id === this.bookId);
      if (!book) {
        return;
      }
      this.catalogs = state.userState.currentUserCatalogs.map(catalog => ({
        ...catalog,
        isSelected: book.bookCatalogs.some(
          item => item.catalogId === catalog.id
        )
      }));
    });

    this.addToListForm = new FormGroup({
      bookSaverListItem: new FormControl(null, Validators.required)
    });
  }

  addToCatalog(catalogId) {
    this.bookSaverService.addBookToCatalog(catalogId, this.bookId);

    this.store.subscribe(state => {
      const book = state.bookState.books.find(b => b.id === this.bookId);
      this.catalogs = state.userState.currentUserCatalogs.map(catalog => ({
        ...catalog,
        isSelected: book.bookCatalogs.some(
          item => item.catalogId === catalog.id
        )
      }));
    });
  }

  removeFromCatalog(catalogId) {
    this.bookSaverService.removeBookFromCatalog(catalogId, this.bookId);

    this.store.subscribe(state => {
      const book = state.bookState.books.find(b => b.id === this.bookId);
      this.catalogs = state.userState.currentUserCatalogs.map(catalog => ({
        ...catalog,
        isSelected: book.bookCatalogs.some(
          item => item.catalogId === catalog.id
        )
      }));
    });
  }

  onSubmit() {
    // const item = this.addToListForm.value;
    // this.bookSaverService
    //   .addBookToCatalog(null, item.bookSaverListItem, this.bookId)
    //   .subscribe(next => {
    //     console.log("next", next);
    //   });
    // this.addToListForm.reset();
  }

  onSaverFocus() {
    // this.catalogs = this.bookSaverService.getUserLists(
    //   this.addToListForm.value.bookSaverListItem
    // );
  }

  onSaverFocusOut() {
    // this.catalogs = [];
    // console.log("focus out");
  }
}
