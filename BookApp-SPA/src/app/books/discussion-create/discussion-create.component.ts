import { Component, OnInit } from "@angular/core";
import { FormGroup, Validators, FormControl } from "@angular/forms";
import { AlertifyService } from "../../_services/alertify.service";
import { Router, ActivatedRoute, Params } from "@angular/router";
import { DiscussionService } from "../../_services/discussion.service";

@Component({
  selector: "app-discussion-create",
  templateUrl: "./discussion-create.component.html",
  styleUrls: ["./discussion-create.component.scss"]
})
export class DiscussionCreateComponent implements OnInit {
  addDiscussionForm: FormGroup;
  bookId: number;

  constructor(
    private discussionService: DiscussionService,
    private alertify: AlertifyService,
    private router: Router,
    private route: ActivatedRoute
  ) {}
  ngOnInit() {
    this.addDiscussionForm = new FormGroup({
      title: new FormControl(null, Validators.required),
      body: new FormControl(null, Validators.required)
    });

    this.route.params.subscribe((params: Params) => {
      this.bookId = params.bookId;
    });
  }

  onSubmit() {
    const title: string = this.addDiscussionForm.value.title.toString();
    const body: string = this.addDiscussionForm.value.body.toString();

    this.discussionService
      .createDiscussion({ title, body, bookId: this.bookId })
      .subscribe(
        next => {
          this.router.navigateByUrl("/books/discussions");
          this.alertify.success("Discussion created");
        },
        error => {
          this.alertify.error("Failed to create discussion");
        }
      );
  }
}
