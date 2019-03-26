import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { map } from "rxjs/operators";
import { Router } from "@angular/router";
import { JwtHelperService } from "@auth0/angular-jwt";
import { Profile } from "../_models/profile";

import * as UserProfileActions from "../_store/user.actions";
import { Store } from "@ngrx/store";

@Injectable({
  providedIn: "root"
})
export class AuthService {
  baseUrl = "http://localhost:5000/api/auth/";
  jwtHelper = new JwtHelperService();

  constructor(
    private http: HttpClient,
    private router: Router,
    private store: Store<{ userProfile: { profile: Profile } }>
  ) {}

  login(model: any) {
    console.log({ model });
    return this.http.post(this.baseUrl + "login", model).pipe(
      map((response: any) => {
        const user = response;

        this.store.dispatch(
          new UserProfileActions.GetCurrentUserAction(<Profile>response.user)
        );

        if (user) {
          localStorage.setItem("token", user.token);
          localStorage.setItem("currentUser", JSON.stringify(user.user));
          this.router.navigate(["/user/profile/", user.user.friendlyUrl]);
        }
      })
    );
  }

  isUserLoggedIn() {
    const token = localStorage.getItem("token");
    return !this.jwtHelper.isTokenExpired(token);
  }

  logout() {
    localStorage.removeItem("token");
    localStorage.removeItem("currentUser");
    this.router.navigate(["/"]);
  }

  reigster(model: any) {
    return this.http.post(this.baseUrl + "register", model).pipe(
      map((response: any) => {
        const user = response;

        if (user) {
          localStorage.setItem("token", user.token);
          localStorage.setItem("currentUser", JSON.stringify(user.user));
          this.router.navigate(["/user/profile"]);
        }
      })
    );
  }
}
