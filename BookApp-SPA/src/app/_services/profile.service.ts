import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { JwtHelperService } from "@auth0/angular-jwt";
import { Router } from "@angular/router";
import { Store } from "@ngrx/store";
import { Profile } from "../_models/profile";
import { AlertifyService } from "./alertify.service";

import * as UserProfileActions from "../_store/user.actions";

@Injectable({
  providedIn: "root"
})
export class ProfileService {
  baseUrl = "http://localhost:5000/api/profile/";
  jwtHelper = new JwtHelperService();

  constructor(
    private http: HttpClient,
    private router: Router,
    private store: Store<{ userProfile: { profile: Profile } }>,
    private alertify: AlertifyService
  ) {}

  getUserProfile(userId: string) {
    return this.http
      .get(
        this.baseUrl +
          "get/" +
          //this.jwtHelper.decodeToken(localStorage.getItem("token")).nameid
          userId
      )
      .subscribe(
        data => {
          this.store.dispatch(
            new UserProfileActions.GetUserAction(<Profile>data)
          );
          console.log("get user profile", data);
        },
        error => {
          this.alertify.error(error);
        }
      );

    // const token = this.jwtHelper.decodeToken(localStorage.getItem("token"));
    // return this.http.get(this.baseUrl + "get/" + token.nameid);
  }
}
