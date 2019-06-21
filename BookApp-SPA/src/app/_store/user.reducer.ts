import * as UserActions from "./user.actions";
import { CatalogPureDto } from "../_models/catalogPureDto";

export interface UserState {
  currentUser: string;
  currentUserCatalogs: CatalogPureDto[];
  users: Object;
}

export const initialState: UserState = {
  currentUser: null,
  currentUserCatalogs: [],
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
    case UserActions.SET_USERS: {
      const usersMap = action.payload.reduce((result, user) => {
        result[user.friendlyUrl] = user;
        return result;
      }, {});
      return {
        ...state,
        users: {
          ...state.users,
          ...usersMap
        }
      };
    }
    case UserActions.UPDATE_USER_AVATAR: {
      return {
        ...state,
        avatarPath: action.payload
      };
    }
    case UserActions.SET_CURRENT_USER_CATALOG: {
      return {
        ...state,
        currentUserCatalogs: action.payload
      };
    }
    case UserActions.UPDATE_USER_FOLLOWER: {
      const { isFollowedByCurrentUser, userFriendlyUrl } = action.payload;
      return {
        ...state,
        users: {
          ...state.users,
          [userFriendlyUrl]: {
            ...state.users[userFriendlyUrl],
            isFollowedByCurrentUser
          }
        }
        // state.users[0].isFollowedByCurrentUser: action.payload,
        //isFollowedByCurrentUser: action.payload.isFollowedByCurrentUser
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
