import * as BookListActions from "./bookList.actions";

const initialState = {
  books: []
};

export function bookListReducer(
  state = initialState,
  action: BookListActions.BookListActions
) {
  switch (action.type) {
    case BookListActions.GET_BOOKS: {
      return {
        ...state,
        books: action.payload
      };
    }
    case BookListActions.ADD_BOOK: {
      return {
        ...state,
        books: [...state.books, action.payload]
      };
    }
    // case BookListActions.UPDATE_BOOK:
    //   return { ...state, books: [state.books], action };
    default:
      return state;
  }
}
