import { Component, OnInit } from "@angular/core";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { BookSaverService } from "src/app/_services/bookSaver.service";
import { CatalogPureDto } from "src/app/_models/catalogPureDto";
import { Profile } from "selenium-webdriver/firefox";
import { Store } from "@ngrx/store";
import { UserState } from "src/app/_store/user.reducer";

@Component({
  selector: "app-book-saver",
  templateUrl: "./book-saver.component.html",
  styleUrls: ["./book-saver.component.scss"]
})
export class BookSaverComponent implements OnInit {
  userList: any;
  addToListForm: FormGroup;
  currentUserUrl: string;

  constructor(
    private bookSaverService: BookSaverService,
    private store: Store<{ userState: UserState }>
  ) {}

  ngOnInit() {
    this.store.subscribe(next => {
      this.currentUserUrl = next.userState.currentUser;
      // this.bookSaverService.getUserCatalogList(this.currentUserUrl).subscribe(
      //   data => {
      //     console.log("user catalogs ", data);
      //     this.userList = data;
      //   },
      //   error => {
      //     // this.alertify.error(error);
      //   }
      // );
    });

    this.addToListForm = new FormGroup({
      bookSaverListItem: new FormControl(null, Validators.required)
    });
  }

  onSaverFocus() {
    // this.userList = this.bookSaverService.getUserLists(
    //   this.addToListForm.value.bookSaverListItem
    // );
  }

  onSaverFocusOut() {
    // this.userList = [];
    // console.log("focus out");
  }

  itemSelected(label) {
    this.addToListForm.controls["bookSaverListItem"].setValue(label);
  }

  onSubmit() {
    console.log(this.addToListForm.value);
  }
}
