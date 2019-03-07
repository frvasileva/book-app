import * as BookDetailsActions from "./book-detail.actions";

const initialState = {
  book: {

  }
};

export function bookDetailsReducer(
  state = initialState,
  action: BookDetailsActions.BookDetailsActions
) {
    switch (action.type) {
    case BookDetailsActions.GET_BOOK_DETAILS: {
      console.log("GET_BOOK_DETAILS");
      console.log("GET_BOOK_DETAILS payload", action.payload);
      return {
        ...state,
        books: action.payload
      };
    }
    default:
      return state;
  }
}
