import { Component, OnInit } from "@angular/core";
import { AlertifyService } from "src/app/_services/alertify.service";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { Store } from "@ngrx/store";
import { Profile } from "src/app/_models/profile";
import { ProfileService } from "src/app/_services/profile.service";

@Component({
  selector: "app-profile-edit",
  templateUrl: "./profile-edit.component.html",
  styleUrls: ["./profile-edit.component.scss"]
})
export class ProfileEditComponent implements OnInit {
  profileEditForm: FormGroup;
  profile: Profile;
  profileToSubmit: Profile;

  constructor(
    private alertify: AlertifyService,
    private profileService: ProfileService,
    private store: Store<{ userProfile: Profile }>
  ) {}

  ngOnInit() {
    this.store
      .select(state => state)
      .subscribe(res => {
        this.profile = res.userProfile;

        // if (this.profile === null || this.friendlyUrl !== this.profile.friendlyUrl) {
        //   this.profileService.getUserProfile(this.friendlyUrl);
        // }
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
    this.profileService.updateProfile(this.profileToSubmit);
  }
}