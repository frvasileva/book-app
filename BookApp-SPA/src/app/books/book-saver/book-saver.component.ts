import { Component, OnInit, Input } from "@angular/core";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { Store } from "@ngrx/store";
import { ActivatedRoute, Params } from "@angular/router";

import { BookSaverService } from "src/app/_services/bookSaver.service";
import { CatalogPureDto } from "src/app/_models/catalogPureDto";
import { UserState } from "src/app/_store/user.reducer";

@Component({
  selector: "app-book-saver",
  templateUrl: "./book-saver.component.html",
  styleUrls: ["./book-saver.component.scss"]
})
export class BookSaverComponent implements OnInit {
  @Input() bookId: number;
  @Input() bookCatalogs: any = [];

  catalogs: CatalogPureDto[];
  addToListForm: FormGroup;
  currentUserUrl: string;
  friendlyUrl: string;

  constructor(
    private route: ActivatedRoute,
    private bookSaverService: BookSaverService,
    private store: Store<{ userState: UserState }>
  ) {}

  ngOnInit() {
    this.route.params.subscribe((params: Params) => {
      this.friendlyUrl = params["friendly-url"];
      this.store.subscribe(state => {
        this.catalogs = state.userState.currentUserCatalogs.map(catalog => ({
          ...catalog,
          isSelected: (this.bookCatalogs || []).some(
            item => item.catalogId === catalog.id
          )
        }));
      });
    });

    this.addToListForm = new FormGroup({
      bookSaverListItem: new FormControl(null, Validators.required)
    });
  }

  addToCatalog(catalogId) {
    this.bookSaverService.addBookToCatalog(catalogId, this.bookId);
    this.bookCatalogs.push({
      catalogId: catalogId,
      bookId: this.bookId,
      catalogName: ""
    });
  }

  removeFromCatalog(catalogId) {
    this.bookSaverService.removeBookFromCatalog(catalogId, this.bookId);
    this.bookCatalogs = this.bookCatalogs.filter(
      item => item.bookId !== this.bookId && item.catalogId !== catalogId
    );
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
