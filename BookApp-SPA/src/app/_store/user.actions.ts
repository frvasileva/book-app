import { Action } from "@ngrx/store";
import { User } from "../_models/user";
import { CatalogPureDto } from "../_models/catalogPureDto";

export const SET_CURRENT_USER = "SET_CURRENT_USER";
export const SET_CURRENT_USER_CATALOG = "SET_CURRENT_USER_CATALOG";
export const ADD_CURRENT_USER_CATALOG = "ADD_CURRENT_USER_CATALOG";
export const SET_USER = "SET_USER";
export const SET_USERS = "SET_USERS";
export const GET_CURRENT_USER = "GET_CURRENT_USER";
export const CREATE_USER = "CREATE_USER";
export const UPDATE_USER = "UPDATE_USER";

export const UPDATE_USER_AVATAR = "UPDATE_USER_AVATAR";
export const UPDATE_USER_FOLLOWER = "UPDATE_USER_FOLLOWER";

export const LOGOUT = "LOGOUT";

export class SetCurrentUserAction implements Action {
  readonly type = SET_CURRENT_USER;
  constructor(public payload: String) {}
}
export class SetCurrentUserCatalogsAction implements Action {
  readonly type = SET_CURRENT_USER_CATALOG;
  constructor(public payload: CatalogPureDto[]) {}
}
export class AddCurrentUserCatalogsAction implements Action {
  readonly type = ADD_CURRENT_USER_CATALOG;
  constructor(public payload: CatalogPureDto) {}
}
export class SetUserAction implements Action {
  readonly type = SET_USER;
  constructor(public payload: User) {}
}
export class SetUsersAction implements Action {
  readonly type = SET_USERS;
  constructor(public payload: User[]) {}
}
export class UpdateUserFollowerAction implements Action {
  readonly type = UPDATE_USER_FOLLOWER;
  constructor(public payload: any) {}
}
export class UpdateUserAvatarAction implements Action {
  readonly type = UPDATE_USER_AVATAR;
  constructor(public payload: string) {}
}

export class Logout implements Action {
  readonly type = LOGOUT;
}

export type UserActions =
  | SetCurrentUserAction
  | SetCurrentUserCatalogsAction
  | AddCurrentUserCatalogsAction
  | SetUserAction
  | SetUsersAction
  | UpdateUserAvatarAction
  | UpdateUserFollowerAction
  | Logout;
