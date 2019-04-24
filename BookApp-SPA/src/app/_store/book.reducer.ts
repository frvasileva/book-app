import * as BookActions from "./book.actions";

const initialState = {
  books: []
};

export function bookReducer (
  state = initialState,
  action: BookActions.BookActions
) {
  switch (action.type) {
    case BookActions.SET_BOOKS: {
      return {
        ...state,
        books: action.payload
      };
    }
    case BookActions.SET_BOOK: {
      // @TODO: avoid duplicate books in the list
      return {
        ...state,
        books: [...state.books, action.payload]
      };
    }
    default:
      return state;
  }
}
