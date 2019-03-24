import { Component, OnInit } from "@angular/core";
import { ProfileService } from "src/app/_services/profile.service";
import { ActivatedRoute } from "@angular/router";
import { Profile } from "src/app/_models/profile";
import { AlertifyService } from "src/app/_services/alertify.service";
import { Observable } from "rxjs";
import { Store } from "@ngrx/store";
import { AppState } from "src/app/app.state";

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
    private alertify: AlertifyService,
    private store: Store<{
      userProfile: Profile;
    }>
  ) {}

  ngOnInit() {
    //TODO get userId from route ( get friendly URL)
    //    this.profileService.getUserProfile("16");

    this.store
      .select(state => state.userProfile)
      .subscribe(res => {
        this.profile = res as Profile;
      });
  }
}
