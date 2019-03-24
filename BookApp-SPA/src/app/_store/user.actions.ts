import { Action } from "@ngrx/store";
import { Profile } from "../_models/profile";

export const LOGIN_USER = "LOGIN_USER";
export const GET_USER = "GET_USER";
export const GET_CURRENT_USER = "GET_CURRENT_USER";
export const CREATE_USER = "CREATE_USER";
export const UPDATE_USER = "UPDATE_USER";
export const DELETE_USER = "DELETE_USER";

export class LoginUserAction implements Action {
  readonly type = LOGIN_USER;
  constructor(public payload: Profile) {}
}
export class GetUserAction implements Action {
  readonly type = GET_USER;
  constructor(public payload: Profile) {}
}
export class GetCurrentUserAction implements Action {
  readonly type = GET_CURRENT_USER;
  constructor(public payload: Profile) {}
}
export class CreateUserAction implements Action {
  readonly type = CREATE_USER;
  constructor(public payload: Profile) {}
}
export class UpdateUserAction implements Action {
  readonly type = UPDATE_USER;
  payload: Profile;
}
export class DeleteUserAction implements Action {
  readonly type = DELETE_USER;
  payload: Profile;
}

export type UserActions =
  | GetUserAction
  | GetCurrentUserAction
  | CreateUserAction
  | UpdateUserAction
  | DeleteUserAction;
