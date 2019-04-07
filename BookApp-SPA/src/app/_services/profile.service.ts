import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { JwtHelperService } from "@auth0/angular-jwt";
import { Router } from "@angular/router";
import { Store } from "@ngrx/store";
import { Profile } from "../_models/profile";
import { AlertifyService } from "./alertify.service";

import * as UserProfileActions from "../_store/user.actions";
import * as UsersActions from "../_store/users.actions";
import { map } from "rxjs/operators";

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

  getUserProfile(friendlyUrl: string) {
    return this.http.get(this.baseUrl + "get/" + friendlyUrl).subscribe(
      data => {
        this.store.dispatch(
          new UserProfileActions.GetUserAction(<Profile>data)
        );
      },
      error => {
        this.alertify.error(error);
      }
    );
  }
  getAll() {
    return this.http.get(this.baseUrl + "get-all").subscribe(
      data => {
        this.store.dispatch(new UsersActions.GetUsersAction(data as Profile[]));
      },
      error => {
        this.alertify.error(error);
      }
    );
  }

  updateProfile(model: Profile) {
    return this.http.post(this.baseUrl + "edit-user", model).pipe(
      map((data: any) => {
        console.log("dataaa", data);
        this.store.dispatch(
          new UserProfileActions.UpdateUserAction(data as Profile)
        );
      })
    );
  }
}
