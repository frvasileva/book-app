import { Action } from "@ngrx/store";
import { User } from "../_models/User";

export const SET_CURRENT_USER = "SET_CURRENT_USER";
export const SET_USER = "SET_USER";
export const GET_USER = "GET_USER";
export const GET_CURRENT_USER = "GET_CURRENT_USER";
export const CREATE_USER = "CREATE_USER";
export const UPDATE_USER = "UPDATE_USER";
export const UPDATE_USER_AVATAR = "UPDATE_USER_AVATAR";
export const LOGOUT = "LOGOUT";

export class SetCurrentUser implements Action {
  readonly type = SET_CURRENT_USER;
  constructor(public payload: String) {}
}
export class SetUser implements Action {
  readonly type = SET_USER;
  constructor(public payload: User) {}
}
// export class LoginUserAction implements Action {
//   readonly type = LOGIN_USER;
//   constructor(public payload: Profile) {}
// }
export class GetUserAction implements Action {
  readonly type = GET_USER;
  constructor(public payload: User) {}
}
export class GetCurrentUserAction implements Action {
  readonly type = GET_CURRENT_USER;
  constructor(public payload: User) {}
}
export class CreateUserAction implements Action {
  readonly type = CREATE_USER;
  constructor(public payload: User) {}
}
export class UpdateUserAction implements Action {
  readonly type = UPDATE_USER;
  constructor(public payload: User) {}
}

export class UpdateUserAvatarAction implements Action {
  readonly type = UPDATE_USER_AVATAR;
  constructor(public payload: string) {}
}

export class Logout implements Action {
  readonly type = LOGOUT;
}

export type UserActions =
  | SetCurrentUser
  | SetUser
  | GetUserAction
  | GetCurrentUserAction
  | CreateUserAction
  | UpdateUserAction
  | UpdateUserAvatarAction
  | Logout;
