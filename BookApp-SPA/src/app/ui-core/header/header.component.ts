import { Component, OnInit } from "@angular/core";
import { AuthService } from "src/app/_services/auth.service";
import { Store } from "@ngrx/store";
import { Profile } from "src/app/_models/profile";

@Component({
  selector: "app-header",
  templateUrl: "./header.component.html",
  styleUrls: ["./header.component.scss"]
})
export class HeaderComponent implements OnInit {
  profile: any;

  constructor(
    public authService: AuthService,
    private store: Store<{ userProfile: { profile: Profile } }>
  ) {}

  ngOnInit() {
    this.store
      .select(state => state)
      .subscribe(res => {
        this.profile = res.userProfile;
      });
  }
}
