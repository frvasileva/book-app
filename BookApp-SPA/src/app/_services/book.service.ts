import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { JwtHelperService } from "@auth0/angular-jwt";
import { Router } from "@angular/router";

@Injectable({
  providedIn: "root"
})
export class BooksService {
  baseUrl = "http://localhost:5000/api/book/";
  jwtHelper = new JwtHelperService();

  constructor(private http: HttpClient, private router: Router) {}

  getBook(friendlyUrl: string = "dummy url") {
    console.log(this.http.get(this.baseUrl + "get/" + friendlyUrl));
    return this.http.get(this.baseUrl + "get/" + friendlyUrl);
  }
}
