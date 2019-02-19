import { Component, OnInit } from "@angular/core";
import { UserService } from "../user.service";
import { UserProfile } from "../profile/UserProfile.model";

@Component({
  selector: "app-users-list",
  templateUrl: "./users-list.component.html",
  styleUrls: ["./users-list.component.scss"]
})
export class UsersListComponent implements OnInit {
  userList: UserProfile[];
  constructor(private userService: UserService) {}

  ngOnInit() {
    this.userList = this.userService.getAllUsers();
  }
}
