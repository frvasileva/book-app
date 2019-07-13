import * as UserActions from "./user.actions";
import { CatalogPureDto } from "../_models/catalogPureDto";

export interface UserState {
  currentUser: string;
  currentUserCatalogs: CatalogPureDto[];
  users: any[];
}

export const initialState: UserState = {
  currentUser: null,
  currentUserCatalogs: [],
  users: []
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
      const newUsers = state.users.filter(user => user.friendlyUrl !== action.payload.friendlyUrl);
      newUsers.push(action.payload);
      return {
        ...state,
        users: newUsers
      };
    }
    case UserActions.SET_USERS: {
      const userFriendlyUrls = action.payload.map(user => user.friendlyUrl);
      const newUsers = state.users.filter(user => !userFriendlyUrls.includes(user.friendlyUrl));
      newUsers.push(...action.payload);
      return {
        ...state,
        users: newUsers
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
    case UserActions.ADD_CURRENT_USER_CATALOG: {
      return {
        ...state,
        currentUserCatalogs: [action.payload, ...state.currentUserCatalogs]
      };
    }
    case UserActions.UPDATE_USER_FOLLOWER: {
      const { isFollowedByCurrentUser, userFriendlyUrl } = action.payload;
      const newUsers = state.users.filter(user => user.friendlyUrl !== userFriendlyUrl);
      newUsers.push({
        ...state.users.find(user => user.friendlyUrl === userFriendlyUrl),
        isFollowedByCurrentUser
      });
      return {
        ...state,
        users: newUsers
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
