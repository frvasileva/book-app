import { Component, OnInit } from "@angular/core";
import { DiscussionDto } from "src/app/_models/discussionDto";
import { DiscussionService } from "src/app/_services/discussion.service";
import { ActivatedRoute, Params } from "@angular/router";

@Component({
  selector: "app-discussions-per-book",
  templateUrl: "./discussions-per-book.component.html",
  styleUrls: ["./discussions-per-book.component.scss"]
})
export class DiscussionsPerBookComponent implements OnInit {
  discussions: DiscussionDto[];
  bookTitle: string;
  bookFriendlyUrl: string;
  bookId: number;

  constructor(
    private discussionService: DiscussionService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.route.params.subscribe((params: Params) => {
      this.bookId = params["bookId"];
      this.discussionService
        .getDiscussionByBook(this.bookId)
        .subscribe(data => {
          console.log(data);
          this.discussions = data as DiscussionDto[];
          this.bookTitle = this.discussions[0].bookTitle;
          this.bookFriendlyUrl = this.discussions[0].bookFriendlyUrl;
        });
    });
  }
}
