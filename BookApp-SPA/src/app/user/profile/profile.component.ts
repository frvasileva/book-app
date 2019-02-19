import { Component, OnInit } from "@angular/core";
import { UserService } from "../user.service";
import { UserProfile } from "./UserProfile.model";

@Component({
  selector: "app-profile",
  templateUrl: "./profile.component.html",
  styleUrls: ["./profile.component.scss"]
})
export class ProfileComponent implements OnInit {
  userProfile: UserProfile;

  constructor(private userService: UserService) {}

  ngOnInit() {
    this.userProfile = this.userService.get();
    console.log("user profile", this.userProfile);
    console.log(this.userProfile.books);
  }
}
