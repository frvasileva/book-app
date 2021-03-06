import { Component, OnInit } from "@angular/core";
import { DiscussionService } from "../../_services/discussion.service";
import { DiscussionDto } from "../../_models/discussionDto";
import { ActivatedRoute, Params } from "@angular/router";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { AlertifyService } from "../../_services/alertify.service";
import { JwtHelperService } from "@auth0/angular-jwt";
import { SeoHelperService } from "../../_shared/seo-helper.service";
import { AuthService } from "../../_services/auth.service";

@Component({
  selector: "app-discussion-details",
  templateUrl: "./discussion-details.component.html",
  styleUrls: ["./discussion-details.component.scss"]
})
export class DiscussionDetailsComponent implements OnInit {
  discussion: DiscussionDto;
  friendlyUrl: string;

  jwtHelper = new JwtHelperService();
  discussionReplyForm: FormGroup;

  constructor(
    private discussionService: DiscussionService,
    private route: ActivatedRoute,
    private alertify: AlertifyService,
    private seoService: SeoHelperService,
    private authService: AuthService
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
          const item = next;
          item.username = item.username;
          item.userAvatarPath = this.discussion.userAvatarPath;
          item.userFriendlyUrl = this.discussion.userFriendlyUrl;

          this.discussion.discussionItems.push(item);
          this.discussionReplyForm.reset();

          this.seoService.setSeoMetaTags(this.discussion.title);
        },
        error => {
          this.alertify.error("Failed to create discussion");
        }
      );
  }

  getCurrentUserName(): string {
    const token = this.authService.getDecodedToken();
    return token.unique_name;
  }
}
