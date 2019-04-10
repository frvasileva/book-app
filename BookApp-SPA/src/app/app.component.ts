import { Component, OnInit } from "@angular/core";
import { JwtHelperService } from "@auth0/angular-jwt";

import { UserService } from './_services/user.service';

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.scss"]
})
export class AppComponent implements OnInit {
  title = "Book Application";
  jwtHelper = new JwtHelperService();
  currentUserId: string;
  token: any;
  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.token = localStorage.getItem("token");
    if (this.token !== null) {
      this.currentUserId = this.jwtHelper.decodeToken(this.token).unique_name;
      this.userService.getUser(this.currentUserId);
    }
  }
}
