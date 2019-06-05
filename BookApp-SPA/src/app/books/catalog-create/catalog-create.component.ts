import { Component, OnInit } from "@angular/core";
import { FormGroup, FormControl, Validators } from "@angular/forms";

@Component({
  selector: "app-catalog-create",
  templateUrl: "./catalog-create.component.html",
  styleUrls: ["./catalog-create.component.scss"]
})
export class CatalogCreateComponent implements OnInit {
  addCatalogForm: FormGroup;

  constructor() {}

  ngOnInit() {
    this.addCatalogForm = new FormGroup({
      name: new FormControl(null, Validators.required),
      visibility: new FormControl(false, [])
    });
  }

  onSubmit() {
    console.log("submit", this.addCatalogForm.value);
    // this.addBookModel.title = this.addBookForm.value.bookData.title;
    // this.addBookModel.description = this.addBookForm.value.bookData.description;
    // this.addBookModel.authorName = this.addBookForm.value.author;

    // this.bookService.addBook(this.addBookModel).subscribe(
    //   next => {
    //     this.alertify.success("Book added!");
    //     this.router.navigateByUrl("/books/add-cover/" + next.friendlyUrl);
    //   },
    //   error => {
    //     this.alertify.error("Failed to add book");
    //   }
    // );
  }
}
