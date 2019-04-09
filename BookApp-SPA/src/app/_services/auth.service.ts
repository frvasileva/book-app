import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { map } from "rxjs/operators";
import { Router } from "@angular/router";
import { JwtHelperService } from "@auth0/angular-jwt";

import * as UserProfileActions from "../_store/user.actions";
import { Store } from "@ngrx/store";
import { environment } from "src/environments/environment";
import { User } from "../_models/user";

@Injectable({
  providedIn: "root"
})
export class AuthService {
  baseUrl = environment.apiUrl + "auth/";
  jwtHelper = new JwtHelperService();
  user$: User;

  constructor(
    private http: HttpClient,
    private router: Router,
    private store: Store<{ userProfile: { user: User } }>
  ) {}

  login(model: any) {
    console.log({ model });
    return this.http.post(this.baseUrl + "login", model).pipe(
      map((response: any) => {
        this.store.dispatch(
          new UserProfileActions.SetCurrentUser(<String>(
            response.user.friendlyUrl
          ))
        );

        this.store.dispatch(
          new UserProfileActions.SetUser(<User>response.user)
        );

        localStorage.setItem("token", response.token);
        this.router.navigate(["/user/profile/", response.user.friendlyUrl]);
      })
    );
  }

  logout() {
    localStorage.removeItem("token");
    this.store.dispatch(new UserProfileActions.Logout());
    this.router.navigate(["/"]);
  }

  reigster(model: any) {
    return this.http.post(this.baseUrl + "register", model).pipe(
      map((response: any) => {
        localStorage.setItem("token", response.token);
        this.router.navigate(["/user/profile"]);
      })
    );
  }
}
