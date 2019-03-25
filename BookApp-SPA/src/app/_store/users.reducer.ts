import * as UsersActions from "./users.actions";

const initialState = [];

export function usersReducer(
  state = initialState,
  action: UsersActions.UsersActions
) {
  switch (action.type) {
    case UsersActions.GET_USERS: {
      return [...action.payload];
    }
    default:
      return state;
  }
}
