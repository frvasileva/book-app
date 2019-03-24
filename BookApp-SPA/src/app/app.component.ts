import { Component, OnInit } from "@angular/core";
import { ProfileService } from "./_services/profile.service";
import { BookService } from "./_services/book.service";

import { JwtHelperService } from "@auth0/angular-jwt";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.scss"]
})
export class AppComponent implements OnInit {
  title = "Book Application";
  jwtHelper = new JwtHelperService();
  currentUserId: string;

  constructor(
    private profileService: ProfileService,
    private bookService: BookService
  ) {}

  ngOnInit(): void {
    this.currentUserId = this.jwtHelper.decodeToken(localStorage.getItem("token")).nameid;

    this.profileService.getUserProfile(this.currentUserId);
    this.bookService.getBooks();
  }
}
