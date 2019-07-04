import { Component, OnInit, Input } from "@angular/core";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { BookSaverService } from "src/app/_services/bookSaver.service";
import { CatalogPureDto } from "src/app/_models/catalogPureDto";
import { Store } from "@ngrx/store";
import { UserState } from "src/app/_store/user.reducer";

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
    private store: Store<{ userState: UserState }>
  ) {}

  ngOnInit() {
    this.store
      .select(next => next.userState)
      .subscribe(userState => {
        this.catalogs = userState.currentUserCatalogs;
      });

    this.addToListForm = new FormGroup({
      bookSaverListItem: new FormControl(null, Validators.required)
    });
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

  itemSelected(catalogName, catalogId) {
    this.bookSaverService
      .addBookToCatalog(catalogId, catalogName, this.bookId)
      .subscribe(next => {
        console.log("next", next);
      });

    this.addToListForm.controls["bookSaverListItem"].setValue(catalogName);
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
}
