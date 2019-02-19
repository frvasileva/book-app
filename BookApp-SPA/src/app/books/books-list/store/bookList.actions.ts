import { Action } from "@ngrx/store";
import { Book } from "../../book.model";

export const ADD_BOOK = "ADD_BOOK";
export const UPDATE_BOOK = "UPDATE_BOOK";
export const DELETE_BOOK = "DELETE_BOOK";

export class AddBookAction implements Action {
  readonly type = ADD_BOOK;
  // payload: Book;
  constructor(public payload: Book) {}
}
export class UpdateBookAction implements Action {
  readonly type = UPDATE_BOOK;
  payload: Book;
}
export class DeleteBookAction implements Action {
  readonly type = DELETE_BOOK;
  payload: Book;
}

export type BookListActions =
  | AddBookAction
  | UpdateBookAction
  | DeleteBookAction;
