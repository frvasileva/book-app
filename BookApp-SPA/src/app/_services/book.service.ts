import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { JwtHelperService } from "@auth0/angular-jwt";
import { Router } from "@angular/router";
import { map } from "rxjs/operators";

import { BookCreateDto } from "../_models/bookCreateDto";

@Injectable({
  providedIn: "root"
})
export class BooksService {
  baseUrl = "http://localhost:5000/api/book/";
  jwtHelper = new JwtHelperService();

  constructor(private http: HttpClient, private router: Router) {}

  addBook(model: BookCreateDto) {
    console.log("posted model: ", model);
    return this.http.post(this.baseUrl + "add", model).pipe(
      map((response: any) => {
        const book = response;

        console.log(book);
      })
    );
  }

  getBook(friendlyUrl: string = "dummy url") {
    console.log(this.http.get(this.baseUrl + "get/" + friendlyUrl));
    return this.http.get(this.baseUrl + "get/" + friendlyUrl);
  }
}
