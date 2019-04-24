import { Component, OnInit } from "@angular/core";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { JwtHelperService } from "@auth0/angular-jwt";

import { BookCreateDto } from "src/app/_models/bookCreateDto";
import { BookService } from "src/app/_services/book.service";
import { AlertifyService } from "src/app/_services/alertify.service";

@Component({
  selector: "app-add-book",
  templateUrl: "./add-book.component.html",
  styleUrls: ["./add-book.component.scss"]
})
export class AddBookComponent implements OnInit {
  addBookForm: FormGroup;
  bookTypes = ["paper", "ebook"];
  addBookModel = {} as BookCreateDto;
  jwtHelper = new JwtHelperService();

  constructor(
    private router: Router,
    private bookService: BookService,
    private alertify: AlertifyService
  ) {}

  ngOnInit() {
    this.addBookForm = new FormGroup({
      bookData: new FormGroup({
        title: new FormControl(null, Validators.required),
        description: new FormControl(null, [Validators.required]),
        bookType: new FormControl("ebook", Validators.required)
      }),
      author: new FormControl(null, Validators.required)
    });
  }

  onSubmit() {
    this.addBookModel.title = this.addBookForm.value.bookData.title;
    this.addBookModel.description = this.addBookForm.value.bookData.description;
    this.addBookModel.authorName = this.addBookForm.value.author;

    this.bookService.addBook(this.addBookModel).subscribe(
      next => {
        this.alertify.success("Book added!");
        this.router.navigateByUrl("/books/add-cover/" + next.friendlyUrl);
      },
      error => {
        this.alertify.error("Failed to add book");
      }
    );
  }
}
