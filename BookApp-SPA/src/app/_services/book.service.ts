import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { JwtHelperService } from "@auth0/angular-jwt";
import { Router } from "@angular/router";
import { map } from "rxjs/operators";

import { BookCreateDto } from "../_models/bookCreateDto";
import { Store } from "@ngrx/store";
import { Book } from "../_models/books";

import { AlertifyService } from "./alertify.service";

import * as BookActions from "../_store/book.actions";
import { environment } from "src/environments/environment";

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
    private store: Store<{ bookState: { books: Book[] } }>,
    private alertify: AlertifyService
  ) {}

  addBook(model: BookCreateDto) {
    model.userId = this.token.nameid;
    return this.http.post(this.baseUrl + "add", model).pipe(
      map((response: any) => {
        this.store.dispatch(new BookActions.SetBookAction(<Book>response));

        return response;
      })
    );
  }

  getBook(friendlyUrl: string = "dummy url") {
    return this.http.get(this.baseUrl + "get/" + friendlyUrl).subscribe(
      data => {
        this.store.dispatch(new BookActions.SetBookAction(<Book>data));
      },
      error => {
        this.alertify.error(error);
      }
    );
  }

  getBooks() {
    return this.http.get(this.baseUrl + "get-books").subscribe(
      data => {
        this.store.dispatch(new BookActions.SetBooksAction(data));
      },
      error => {
        this.alertify.error(error);
      }
    );
  }

  getBooksAddedByUser(userFriendlyUrl: string) {
    return this.http.get(
      this.baseUrl + "get-books-added-by-user/" + userFriendlyUrl
    );
  }

  RecommendByRelevance(currentPage: number) {
    return this.http
      .get(this.baseUrl + "recommend-relevance/" + currentPage)
      .subscribe(data => {
        this.store.dispatch(new BookActions.SetBooksAction(data));
      });
  }
  RecommendBySerendipity(currentPage: number) {
    return this.http
      .get(this.baseUrl + "recommend-serendipity/" + currentPage)
      .subscribe(data => {
        this.store.dispatch(new BookActions.SetBooksAction(data));
      });
  }
  RecommendByNovelty(currentPage: number) {
    return this.http
      .get(this.baseUrl + "recommend-novelty/" + currentPage)
      .subscribe(data => {
        this.store.dispatch(new BookActions.SetBooksAction(data));
      });
  }

  // List<BookDetailsDto> RecommendationByRelevance (int userId);
  // List<BookDetailsDto> RecommendBySerendepity (int userId);
  // List<BookDetailsDto> RecommendByNovelty (int userId);
}
