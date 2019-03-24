import { Action } from "@ngrx/store";
import { Profile } from "../_models/profile";

export const GET_USERS = "GET_USERS";

export class GetUsersAction implements Action {
  readonly type = GET_USERS;
  constructor(public payload: Profile[]) {}
}

export type UsersActions = GetUsersAction;
