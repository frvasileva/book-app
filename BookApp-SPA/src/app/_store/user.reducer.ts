import * as UserActions from "./user.actions";

export interface UserState {
  currentUser: string;
  users: Object;
}

export const initialState: UserState = {
  currentUser: null,
  users: {}
};

export function userReducer(
  state = initialState,
  action: UserActions.UserActions
) {
  switch (action.type) {
    case UserActions.SET_CURRENT_USER: {
      return {
        ...state,
        currentUser: action.payload
      };
    }
    case UserActions.SET_USER: {
      return {
        ...state,
        users: {
          ...state.users,
          [action.payload.friendlyUrl]: action.payload
        }
      };
    }
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
    case UserActions.UPDATE_USER_AVATAR: {
      return {
        ...state,
        avatarPath: action.payload
      };
    }
    case UserActions.LOGOUT: {
      return {
        ...state,
        currentUser: null
      };
    }
    default:
      return state;
  }
}
