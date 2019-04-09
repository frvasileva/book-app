import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { JwtHelperService } from "@auth0/angular-jwt";
import { Router } from "@angular/router";
import { map } from "rxjs/operators";

import { BookCreateDto } from "../_models/bookCreateDto";
import { Store } from "@ngrx/store";
import { Book } from "../_models/books";

import { bookDetailsDto } from "../_models/bookDetailsDto";
import { AlertifyService } from "./alertify.service";

import * as BookListActions from "../books/books-list/store/bookList.actions";
import * as BookDetailsActions from "../_store/book-detail.actions";
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: "root"
})
export class BookService {
  baseUrl = environment.apiUrl + "book/";
  jwtHelper = new JwtHelperService();
  token = this.jwtHelper.decodeToken(localStorage.getItem("token"));

  constructor(
    private http: HttpClient,
    private router: Router,
    private store: Store<{ bookList: { books: Book[] } }>,
    private alertify: AlertifyService
  ) {}

  addBook(model: BookCreateDto) {
    model.userId = this.token.nameid;
    return this.http.post(this.baseUrl + "add", model).pipe(
      map((response: any) => {
        this.store.dispatch(
          new BookDetailsActions.AddBookAction(<bookDetailsDto>response)
        );
      })
    );
  }

  getBook(friendlyUrl: string = "dummy url") {
    return this.http.get(this.baseUrl + "get/" + friendlyUrl).subscribe(
      data => {
        this.store.dispatch(
          new BookDetailsActions.GetBookDetailAction(<bookDetailsDto>data)
        );
      },
      error => {
        this.alertify.error(error);
      }
    );
  }

  getBooks() {
    return this.http.get(this.baseUrl + "get-books").subscribe(
      data => {
        this.store.dispatch(new BookListActions.GetBooksAction(data));
      },
      error => {
        this.alertify.error(error);
      }
    );
  }
}
