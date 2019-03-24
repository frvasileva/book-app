import { Component, OnInit } from "@angular/core";
import { ProfileService } from "./_services/profile.service";
import { BookService } from './_services/book.service';

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.scss"]
})
export class AppComponent implements OnInit {
  constructor(
    private profileService: ProfileService,
    private bookService: BookService
  ) {}

  ngOnInit(): void {
    this.profileService.getUserProfile("16");
    this.bookService.getBooks();
  }

  title = "Book Application";
}
