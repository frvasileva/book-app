import { Component, OnInit } from "@angular/core";
import { BookSaverService } from "../bookSaver.service";
import { FormGroup, FormControl, Validators } from "@angular/forms";

@Component({
  selector: "app-book-saver",
  templateUrl: "./book-saver.component.html",
  styleUrls: ["./book-saver.component.scss"]
})
export class BookSaverComponent implements OnInit {
  userList: any;
  addToListForm: FormGroup;

  constructor(private bookSaverService: BookSaverService) {}

  ngOnInit() {
    this.addToListForm = new FormGroup({
      bookSaverListItem: new FormControl(null, Validators.required)
    });
  }

  onSaverFocus() {
    this.userList = this.bookSaverService.getUserLists(
      this.addToListForm.value.bookSaverListItem
    );
    console.log("focus in", this.userList);
    console.log("values: ", this.addToListForm.value.bookSaverListItem);
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
