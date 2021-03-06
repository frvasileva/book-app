import { Component, OnInit } from "@angular/core";
import { AuthService } from "../../_services/auth.service";
import { Store } from "@ngrx/store";
import { UserState } from "../../_store/user.reducer";

@Component({
  selector: "app-header",
  templateUrl: "./header.component.html",
  styleUrls: ["./header.component.scss"]
})
export class HeaderComponent implements OnInit {
  friendlyUrl: String;
  isLoggedIn: Boolean;

  navbarOpen = false;

  constructor(
    public authService: AuthService,
    private store: Store<{ userState: UserState }>
  ) {}

  ngOnInit() {
    this.store
      .select(state => state.userState)
      .subscribe(userState => {
        this.friendlyUrl = userState.currentUser;
        this.isLoggedIn = userState.currentUser != null;
      });
  }

  toggleNavbar() {
    this.navbarOpen = !this.navbarOpen;
  }
}
