import { Component, OnInit } from "@angular/core";
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
    private store: Store<{
      userProfiles: Profile[];
    }>
  ) {}

  ngOnInit() {
    this.store
      .select(state => state)
      .subscribe(res => {
        this.userList = res.userProfiles as Profile[];
        console.log("users", this.userList);
      });
  }
}
