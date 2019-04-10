import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { environment } from "src/environments/environment";
import { Store } from "@ngrx/store";
import { map } from "rxjs/operators";

import { User } from "../_models/user";

import { AlertifyService } from "./alertify.service";
import * as UsersActions from "../_store/users.actions";
import * as UserProfileActions from "../_store/user.actions";

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
  // getUsers(page?, itemsPerPage?, userParams?, likesParam?): Observable<PaginatedResult<User[]>>
  getUsers() {
    return this.http.get(this.baseUrl + "get-all").subscribe(
      data => {
        this.store.dispatch(new UsersActions.GetUsersAction(data as User[]));
      },
      error => {
        this.alertify.error(error);
      }
    );
  }

  getUser(friendlyUrl: string) {
    return this.http.get(this.baseUrl + "get/" + friendlyUrl).subscribe(
      data => {
        this.store.dispatch(new UserProfileActions.GetUserAction(<User>data));
      },
      error => {
        this.alertify.error(error);
      }
    );
  }

  updateUser(user: User) {
    return this.http.post(this.baseUrl + "edit-user", user).pipe(
      map((data: any) => {
        console.log("dataaa", data);
        this.store.dispatch(
          new UserProfileActions.UpdateUserAction(data as User)
        );
      })
    );
  }

  setAvatar(userId: number, id: number) {
    return this.http.post(
      this.baseUrl + "users/" + userId + "/photos/" + id + "/setMain",
      {}
    );
  }

  // deletePhoto(userId: number, id: number) {
  //   return this.http.delete(this.baseUrl + "users/" + userId + "/photos/" + id);
  // }
}
