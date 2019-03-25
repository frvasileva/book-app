import { Component, OnInit } from "@angular/core";
import { ProfileService } from "../_services/profile.service";

@Component({
  selector: "app-user",
  templateUrl: "./user.component.html",
  styleUrls: ["./user.component.scss"]
})
export class UserComponent implements OnInit {
  constructor(private profileService: ProfileService) {}

  ngOnInit() {
    this.profileService.getAll();
  }
}
