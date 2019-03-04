import { Component, OnInit } from "@angular/core";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { Store } from "@ngrx/store";
import { Book } from "../book.model";
import * as BookListActions from "../books-list/store/bookList.actions";
import { Author } from "src/app/authors/author.model";
import { BookAction } from "../bookAction.model";
import { Router } from "@angular/router";
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

  constructor(
    //private store: Store<{ bookList: { books: Book[] } }>,
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
    console.log("form values: ", this.addBookForm.value.bookData);

    this.addBookModel.title = this.addBookForm.value.bookData.title;
    this.addBookModel.description = this.addBookForm.value.bookData.description;
    this.addBookModel.authorName = this.addBookForm.value.author;
    this.addBookModel.photoPath =
      "https://www.bookbaby.com/images/book-cover-design-basic.png";

    this.bookService.addBook(this.addBookModel).subscribe(
      next => {
        this.alertify.success("Book added!");
        //this.store.dispatch(new BookListActions.AddBookAction(this.fakeBook));
        this.router.navigateByUrl("/books");
      },
      error => {
        this.alertify.error("Failed to add book");
      }
    );
  }
}
