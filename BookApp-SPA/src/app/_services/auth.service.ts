import { Injectable, PLATFORM_ID, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { map } from "rxjs/operators";
import { Router } from "@angular/router";
import { Store } from "@ngrx/store";
import { JwtHelperService } from "@auth0/angular-jwt";
import { isPlatformBrowser } from '@angular/common';

import * as UserProfileActions from "../_store/user.actions";
import { environment } from "../../environments/environment";
import { User } from "../_models/user";
import { UserService } from "./user.service";
import { BookSaverService } from "./bookSaver.service";

@Injectable({
  providedIn: "root"
})
export class AuthService {
  baseUrl = environment.apiUrl + "auth/";
  jwtHelper = new JwtHelperService();
  user$: User;
  isBrowser: boolean;

  constructor(
    private http: HttpClient,
    private router: Router,
    private store: Store<{ userState: { user: User } }>,
    private userService: UserService,
    private bookSaverService: BookSaverService,
    @Inject(PLATFORM_ID) platformId: Object
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
  }

  getToken() {
    if (!this.isBrowser) return null;
    return localStorage.getItem("token") || null;
  }

  getDecodedToken() {
    return this.jwtHelper.decodeToken(this.getToken());
  }

  setToken(value: string) {
    if (!this.isBrowser) return;
    localStorage.setItem("token", value);
  }

  removeToken() {
    if (!this.isBrowser) return;
    localStorage.removeItem("token");
  }

  login(model: any) {
    return this.http.post(this.baseUrl + "login", model).pipe(
      map((response: any) => {
        this.store.dispatch(
          new UserProfileActions.SetCurrentUserAction(<String>(
            response.user.friendlyUrl
          ))
        );

        this.store.dispatch(
          new UserProfileActions.SetUserAction(<User>response.user)
        );

        this.setToken(response.token);

        this.bookSaverService.getUserCatalogList(response.user.friendlyUrl);

        this.router.navigate(["/books"]);
      })
    );
  }

  logout() {
    localStorage.removeItem("token");
    this.store.dispatch(new UserProfileActions.Logout());
    this.router.navigate(["/"]);
  }

  getCurrentUser() {
    const token = this.getDecodedToken();
    if (!token) {
      return;
    }
    const currentUserId = token.unique_name;
    this.store.dispatch(
      new UserProfileActions.SetCurrentUserAction(<String>currentUserId)
    );
    this.userService.getUser(currentUserId);
  }

  isAuthenticated() {
    const token = this.getDecodedToken();

    if (token && !token.isTokenExpired) {
      return true;
    } else {
      return false;
    }
  }

  reigster(model: any) {
    return this.http.post(this.baseUrl + "register", model).pipe(
      map((response: any) => {
        this.setToken(response.token);
        const friendlyUrl = this.getDecodedToken().unique_name;

        this.store.dispatch(
          new UserProfileActions.SetCurrentUserAction(<String>friendlyUrl)
        );

        this.router.navigate(["/user/book-preferences"]);
      })
    );
  }

  setUserBookCategoryPreferences(catalogPreferences: any) {
    return this.http
      .post(
        environment.apiUrl + "profile/add-book-catalog-preferences",
        catalogPreferences
      )
      .pipe(
        map((response: any) => {
          if (response) {
            const friendlyUrl = this.getDecodedToken().unique_name;
            this.router.navigate(["/user/profile", friendlyUrl]);
          } else {
            console.log("can not navigate");
          }
        })
      );
  }

  getUserBookCategoryPreferences() {
    return this.http.get(
      environment.apiUrl + "profile/get-preferences-catalog-list"
    );
  }
}
