import * as BookDetailsActions from "./book-detail.actions";

const initialState = {};

export function bookDetailsReducer(
  state = initialState,
  action: BookDetailsActions.BookDetailsActions
) {
  switch (action.type) {
    case BookDetailsActions.GET_BOOK_DETAILS: {
      return action.payload;
    }
    default:
      return state;
  }
}
