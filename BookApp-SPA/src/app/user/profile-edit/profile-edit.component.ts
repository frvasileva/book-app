import { Component, OnInit } from "@angular/core";
import { AlertifyService } from "src/app/_services/alertify.service";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { Store } from "@ngrx/store";
import { Router } from '@angular/router';

import { UserService } from 'src/app/_services/user.service';
import { User } from 'src/app/_models/user';

@Component({
  selector: "app-profile-edit",
  templateUrl: "./profile-edit.component.html",
  styleUrls: ["./profile-edit.component.scss"]
})
export class ProfileEditComponent implements OnInit {
  profileEditForm: FormGroup;
  profile: User;
  profileToSubmit: User;

  constructor(
    private alertify: AlertifyService,
    private router: Router,
    private userService: UserService,
    private store: Store<{ userProfile: User }>
  ) {}

  ngOnInit() {
    this.store
      .select(state => state)
      .subscribe(res => {
        this.profile = res.userProfile;
      });

    this.createForm();
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
    console.log("submitted", this.profileEditForm.value);
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
