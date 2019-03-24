import { Component, OnInit } from "@angular/core";
import { UserService } from "../user.service";
import { UserProfile } from "../profile/UserProfile.model";
import { ProfileService } from "src/app/_services/profile.service";
import { Profile } from "src/app/_models/profile";
import { Store } from "@ngrx/store";

@Component({
  selector: "app-users-list",
  templateUrl: "./users-list.component.html",
  styleUrls: ["./users-list.component.scss"]
})
export class UsersListComponent implements OnInit {
  userList: Profile[];

  constructor(
    private profileService: ProfileService,
    private store: Store<{
      userProfiles: Profile[];
    }>
  ) {}

  ngOnInit() {
    //TODO: Remove it from here
    this.profileService.getAll();

    this.store
      .select(state => state)
      .subscribe(res => {
        this.userList = res.userProfiles as Profile[];

        console.log("users", this.userList);
      });
  }
}
