import { Component, OnInit } from "@angular/core";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { Store } from "@ngrx/store";
import { Book } from "../book.model";
import * as BookListActions from "../books-list/store/bookList.actions";
import { Author } from "src/app/authors/author.model";
import { BookAction } from "../bookAction.model";
import { Router } from "@angular/router";

@Component({
  selector: "app-add-book",
  templateUrl: "./add-book.component.html",
  styleUrls: ["./add-book.component.scss"]
})
export class AddBookComponent implements OnInit {
  addBookForm: FormGroup;
  bookTypes = ["paper", "ebook"];

  fakeBook: Book = new Book(
    "1",
    "Winnie the pooh",
    "ebook",
    "The best book ever",
    "https://about.canva.com/wp-content/uploads/sites/3/2015/01/children_bookcover.png",
    "Сиела",
    new Author("1", "Bla bla bla ", "Nice bio", ""),
    ["dutch people"],
    [new BookAction(true, "like", true, 5)]
  );

  constructor(
    private store: Store<{ bookList: { books: Book[] } }>,
    private router: Router
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
    console.log("form values: ", this.addBookForm.value.bookData.title);
    this.fakeBook.title = this.addBookForm.value.bookData.title;
    this.fakeBook.author.name = this.addBookForm.value.author;
    this.store.dispatch(new BookListActions.AddBookAction(this.fakeBook));

    this.router.navigateByUrl("/books");
  }
}
