import { Component, OnInit } from "@angular/core";
import { UserService } from "../_services/user.service";

@Component({
  selector: "app-user",
  templateUrl: "./user.component.html",
  styleUrls: ["./user.component.scss"]
})
export class UserComponent implements OnInit {
  constructor(private userService: UserService) {}

  ngOnInit() {
    console.log("module on init");
    this.userService.getUsers();
  }
}
