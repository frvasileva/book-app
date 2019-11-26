import { Component, OnInit } from "@angular/core";
import { AlertifyService } from "src/app/_services/alertify.service";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { Store } from "@ngrx/store";
import { Router, ActivatedRoute } from "@angular/router";

import { UserService } from "src/app/_services/user.service";
import { User } from "src/app/_models/user";
import { UserState } from "src/app/_store/user.reducer";

import { settings } from "src/app/_shared/settings";
import { SeoHelperService } from "src/app/_shared/seo-helper.service";

@Component({
  selector: "app-profile-edit",
  templateUrl: "./profile-edit.component.html",
  styleUrls: ["./profile-edit.component.scss"]
})
export class ProfileEditComponent implements OnInit {
  profileEditForm: FormGroup;
  profile: any;
  profileToSubmit: User;
  friendlyUrl: string;

  constructor(
    private alertify: AlertifyService,
    private router: Router,
    private route: ActivatedRoute,
    private userService: UserService,
    private store: Store<{ userState: UserState }>,
    private seoHelper: SeoHelperService
  ) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.friendlyUrl = params["friendlyUrl"];

      this.userService.getUser(this.friendlyUrl).subscribe(data => {
        this.profile = data;
        this.createForm();

        this.seoHelper.setSeoMetaTags(
          "Edit " + this.profile.knownAs + settings.seo_appName_title
        );
      });
    });
  }

  createForm() {
    this.profileEditForm = new FormGroup({
      knownAs: new FormControl(this.profile.knownAs, [Validators.required]),
      introduction: new FormControl(this.profile.introduction),
      city: new FormControl(this.profile.city),
      country: new FormControl(this.profile.country)
    });
  }

  onSubmit() {
    this.profileToSubmit = this.profileEditForm.value;
    this.profileToSubmit.friendlyUrl = this.profile.friendlyUrl;

    this.userService.updateUser(this.profileToSubmit).subscribe(
      next => {
        this.alertify.success("Your profile has been updated!");
        this.router.navigate(["/user/profile/", this.profile.friendlyUrl]);
      },
      error => {
        this.alertify.error("Failed to update profile");
      }
    );
  }
}
