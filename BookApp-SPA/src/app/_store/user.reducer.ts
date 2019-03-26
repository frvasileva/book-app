import * as UserActions from "./user.actions";

const initialState = {};

export function userReducer(
  state = initialState,
  action: UserActions.UserActions
) {
  switch (action.type) {
    case UserActions.GET_USER: {
      return {
        ...state,
        ...action.payload
      };
    }
    case UserActions.GET_CURRENT_USER: {
      return {
        ...state,
        ...action.payload
      };
    }
    case UserActions.CREATE_USER: {
      return {
        ...state,
        ...action.payload
      };
    }
    case UserActions.UPDATE_USER: {
      return {
        ...state,
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
