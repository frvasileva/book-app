import { Action } from "@ngrx/store";
import { Book } from "../../book.model";

export const GET_BOOKS = "GET_BOOKS";
export const ADD_BOOK = "ADD_BOOK";
export const UPDATE_BOOK = "UPDATE_BOOK";
export const DELETE_BOOK = "DELETE_BOOK";

export class GetBooksAction implements Action {
  readonly type = GET_BOOKS;
  // payload: Book;
  constructor(public payload: any) {}
}
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
  | GetBooksAction
  | AddBookAction
  | UpdateBookAction
  | DeleteBookAction;
