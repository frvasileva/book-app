import { Component, OnInit } from "@angular/core";
import { FormGroup, FormControl, Validators } from "@angular/forms";

@Component({
  selector: "app-request-book",
  templateUrl: "./request-book.component.html",
  styleUrls: ["./request-book.component.scss"]
})
export class RequestBookComponent implements OnInit {

  requestBookForm: FormGroup;

  ngOnInit() {
    this.requestBookForm = new FormGroup({
      "message": new FormControl("This is my book request for...", Validators.required)
    });
  }

  onSubmit()  {
    console.log(this.requestBookForm.value);
  }
}
