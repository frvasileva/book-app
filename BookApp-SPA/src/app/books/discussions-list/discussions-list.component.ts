import { Component, OnInit } from "@angular/core";
import { DiscussionService } from "../../_services/discussion.service";
import { DiscussionDto } from "../../_models/discussionDto";

@Component({
  selector: "app-discussions-list",
  templateUrl: "./discussions-list.component.html",
  styleUrls: ["./discussions-list.component.scss"]
})
export class DiscussionsListComponent implements OnInit {
  discussions: DiscussionDto[];

  constructor(private discussionService: DiscussionService) {}

  ngOnInit() {
    this.discussionService.getDiscussions().subscribe(data => {
      this.discussions = data as DiscussionDto[];
    });
  }
}
