import { Action } from "@ngrx/store";
import { Book } from "../_models/books";

export const SET_BOOKS = "SET_BOOKS";
export const SET_BOOK = "SET_BOOK";
export const SET_BOOK_PHOTO = "SET_BOOK_PHOTO";
export const UPDATE_BOOK = "UPDATE_BOOK";
export const DELETE_BOOK = "DELETE_BOOK";

export class SetBooksAction implements Action {
  readonly type = SET_BOOKS;
  constructor(public payload: any) {}
}
export class SetBookAction implements Action {
  readonly type = SET_BOOK;
  constructor(public payload: any) {}
}
export class SetBookPhotoAction implements Action {
  readonly type = SET_BOOK_PHOTO;
  constructor(public payload: any) {}
}
export class UpdateBookAction implements Action {
  readonly type = UPDATE_BOOK;
  constructor(public payload: Book) {}
}
export class DeleteBookAction implements Action {
  readonly type = DELETE_BOOK;
  constructor(public payload: Book) {}
}

export type BookActions =
  | SetBooksAction
  | SetBookPhotoAction
  | SetBookAction
  | UpdateBookAction
  | DeleteBookAction;
