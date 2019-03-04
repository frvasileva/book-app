import * as BookListActions from "./bookList.actions";

import { BookAction } from "../../bookAction.model";
import { Book } from "../../book.model";
import { Author } from "src/app/authors/author.model";

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
      console.log({ ...state, books: [state.books], action });
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
