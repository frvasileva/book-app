import { Component, OnInit } from "@angular/core";
import { ProfileService } from "src/app/_services/profile.service";
import { ActivatedRoute } from "@angular/router";
import { Profile } from "src/app/_models/profile";
import { AlertifyService } from "src/app/_services/alertify.service";

@Component({
  selector: "app-profile",
  templateUrl: "./profile.component.html",
  styleUrls: ["./profile.component.scss"]
})
export class ProfileComponent implements OnInit {
  profile: Profile;

  constructor(
    private profileService: ProfileService,
    private route: ActivatedRoute,
    private alertify: AlertifyService
  ) {}

  ngOnInit() {
    this.getUserProfile("1");
  }

  getUserProfile(userId: string) {
    this.route.data.subscribe(
      data => {
        this.profile = data.profile;
      },
      error => {
        this.alertify.error(error);
      }
    );
  }
}
