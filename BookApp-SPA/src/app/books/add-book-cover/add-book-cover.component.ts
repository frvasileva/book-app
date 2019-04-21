import { Component, OnInit } from "@angular/core";
import { environment } from "src/environments/environment";
import { ActivatedRoute, Params } from "@angular/router";

@Component({
  selector: "app-add-book-cover",
  templateUrl: "./add-book-cover.component.html",
  styleUrls: ["./add-book-cover.component.scss"]
})
export class AddBookCoverComponent implements OnInit {
  apiDestinationUrl: string;
  baseUrl = environment.apiUrl;
  bookFriendlyUrl: string;

  constructor(private route: ActivatedRoute) {}

  ngOnInit() {
    this.route.params.subscribe((params: Params) => {
      this.bookFriendlyUrl = params["friendly-url"];
      this.apiDestinationUrl =
        this.baseUrl + "book/add-photo/" + this.bookFriendlyUrl;
    });
  }
}
