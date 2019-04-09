import { Injectable } from "@angular/core";
import { User } from "../_models/user";
import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { environment } from "src/environments/environment";
import { Store } from "@ngrx/store";
import { AlertifyService } from "./alertify.service";
import * as UsersActions from "../_store/users.actions";

@Injectable({
  providedIn: "root"
})
export class UserService {
  baseUrl = environment.apiUrl;

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

  getUser(id): Observable<User> {
    return this.http.get<User>(this.baseUrl + "users/" + id);
  }

  updateUser(id: number, user: User) {
    return this.http.put(this.baseUrl + "users/" + id, user);
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
