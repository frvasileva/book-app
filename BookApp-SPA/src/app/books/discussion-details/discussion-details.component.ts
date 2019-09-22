import { Component, OnInit } from "@angular/core";
import { DiscussionService } from "src/app/_services/discussion.service";
import { DiscussionDto } from "src/app/_models/discussionDto";
import { ActivatedRoute, Params } from "@angular/router";
import { Title, Meta } from "@angular/platform-browser";
import { DiscussionItemDto } from "src/app/_models/discussionItemDto";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { AlertifyService } from "src/app/_services/alertify.service";

@Component({
  selector: "app-discussion-details",
  templateUrl: "./discussion-details.component.html",
  styleUrls: ["./discussion-details.component.scss"]
})
export class DiscussionDetailsComponent implements OnInit {
  discussion: DiscussionDto;
  friendlyUrl: string;

  discussionReplyForm: FormGroup;

  constructor(
    private discussionService: DiscussionService,
    private route: ActivatedRoute,
    private titleService: Title,
    private metaTagService: Meta,
    private alertify: AlertifyService
  ) {}

  ngOnInit() {
    this.route.params.subscribe((params: Params) => {
      this.friendlyUrl = params["friendlyUrl"];

      this.discussionService.getDiscussion(this.friendlyUrl).subscribe(data => {
        this.discussion = data as DiscussionDto;
      });
    });

    this.discussionReplyForm = new FormGroup({
      body: new FormControl(null, Validators.required)
    });
  }

  addReply() {
    const body: string = this.discussionReplyForm.value.body.toString();

    this.discussionService
      .createDiscussionReply({ body, discussionId: this.discussion.id })
      .subscribe(
        next => {
          this.discussion.discussionItems.push(next);
          this.discussionReplyForm.reset();
        },
        error => {
          this.alertify.error("Failed to create discussion");
        }
      );
  }
}
