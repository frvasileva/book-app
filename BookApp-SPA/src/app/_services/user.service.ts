import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "src/environments/environment";
import { Store } from "@ngrx/store";
import { map } from "rxjs/operators";

import { User } from "../_models/user";

import { AlertifyService } from "./alertify.service";
import * as UserActions from "../_store/user.actions";

@Injectable({
  providedIn: "root"
})
export class UserService {
  baseUrl = environment.apiUrl + "profile/";

  constructor(
    private http: HttpClient,
    private store: Store<{ users: { users: User } }>, //TODO: Refactor!
    private alertify: AlertifyService
  ) {}

  getUsers() {
    return this.http.get(this.baseUrl + "get-all").subscribe(
      data => {
        this.store.dispatch(new UserActions.SetUsersAction(data as User[]));
      },
      error => {
        this.alertify.error(error);
      }
    );
  }

  getUser(friendlyUrl: string) {
    return this.http.get(this.baseUrl + "get/" + friendlyUrl);
  }

  updateUser(user: User) {
    return this.http.post(this.baseUrl + "edit-user", user).pipe(
      map((data: any) => {
        this.store.dispatch(new UserActions.SetUserAction(data as User));
      })
    );
  }

  setAvatar(userId: number, id: number) {
    return this.http.post(
      this.baseUrl + "users/" + userId + "/photos/" + id + "/setMain",
      {}
    );
  }

  followUser(userIdToFollow: number) {
    let user: any;

    return this.http
      .get(this.baseUrl + "follow-user/" + userIdToFollow)
      .subscribe(
        data => {
          user = data;
          this.store.dispatch(
            new UserActions.UpdateUserFollowerAction({
              userFriendlyUrl: user.followerFriendlyUrl,
              isFollowedByCurrentUser: true
            })
          );
        },
        error => {
          this.alertify.error(error);
        }
      );
  }

  unFollowUser(userIdToUnfollow: number, friendlyUrl: string) {
    return this.http
      .get(this.baseUrl + "unfollow-user/" + userIdToUnfollow)
      .subscribe(
        data => {
          this.store.dispatch(
            new UserActions.UpdateUserFollowerAction({
              userFriendlyUrl: friendlyUrl,
              isFollowedByCurrentUser: false
            })
          );
        },
        error => {
          this.alertify.error(error);
        }
      );
  }

  getUserPreferencesCatalogs() {
    return this.http.get(this.baseUrl + "get-preferences-catalog-list");
  }

  getUserSelectedPreferencesCatalogs() {
    return this.http.get(
      this.baseUrl + "get-user-selected-preferences-catalog-list"
    ) as any;
  }

  toggleUserPreferencesCatalogs(
    catalogId: number,
    catalogName: string,
    isSelected: number
  ) {
    if (catalogId === undefined) {
      catalogId = 0;
    }
    return this.http.get(
      this.baseUrl +
        "toggle-preferences-catalog/" +
        catalogId +
        "/" +
        catalogName +
        "/" +
        isSelected
    );
  }
}
