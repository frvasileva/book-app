import { Component, OnInit } from "@angular/core";
import { Profile } from "src/app/_models/profile";
import { Store } from "@ngrx/store";
import { ProfileService } from "src/app/_services/profile.service";

@Component({
  selector: "app-users-list",
  templateUrl: "./users-list.component.html",
  styleUrls: ["./users-list.component.scss"]
})
export class UsersListComponent implements OnInit {
  userList: Profile[];

  constructor(
    private store: Store<{
      userProfiles: Profile[];
    }>,
    private profileService: ProfileService
  ) {}

  ngOnInit() {
    this.profileService.getAll();

    this.store
      .select(state => state)
      .subscribe(res => {
        this.userList = res.userProfiles as Profile[];
      });
  }
}
