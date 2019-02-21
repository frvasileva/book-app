import { Component, OnInit } from "@angular/core";
import { ProfileService } from "src/app/_services/profile.service";

@Component({
  selector: "app-profile",
  templateUrl: "./profile.component.html",
  styleUrls: ["./profile.component.scss"]
})
export class ProfileComponent implements OnInit {
  profile: any;

  constructor(
    private profileService: ProfileService
  ) {}

  ngOnInit() {
    this.getUserProfile("1");
  }

  getUserProfile(userId: string) {
    this.profileService.getUserProfile(userId).subscribe(response => {
      console.log("user profile Fani: ", response);
      this.profile = response;
    });
  }
}
