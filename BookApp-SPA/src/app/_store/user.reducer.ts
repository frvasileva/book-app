import * as UserActions from "./user.actions";
import { Profile } from "../_models/profile";

export interface UserState {
  loaded: boolean;
  loading: boolean;
  data: Profile;
}

export const initialState: UserState = {
  loaded: false,
  loading: false,
  data: null
};

export function userReducer(
  state = initialState,
  action: UserActions.UserActions
) {
  switch (action.type) {
    case UserActions.GET_USER: {
      return {
        ...state.data,
        ...action.payload
      };
    }
    case UserActions.GET_CURRENT_USER: {
      return {
        ...state.data,
        ...action.payload
      };
    }
    case UserActions.CREATE_USER: {
      return {
        ...state.data,
        ...action.payload
      };
    }
    case UserActions.UPDATE_USER: {
      console.log("update user: ");
      console.log(state.data);
      console.log(action.payload);
      return {
        ...state.data,
        ...action.payload
      };
    }
    case UserActions.DELETE_USER: {
      return {};
    }
    default:
      return state;
  }
}
