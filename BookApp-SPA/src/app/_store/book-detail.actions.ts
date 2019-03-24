import { Action } from "@ngrx/store";
import { bookDetailsDto } from "../_models/bookDetailsDto";

export const GET_BOOK_DETAILS = "GET_BOOK_DETAILS";
export const ADD_BOOK = "ADD_BOOK";

export class GetBookDetailAction implements Action {
  readonly type = GET_BOOK_DETAILS;
  constructor(public payload: bookDetailsDto) {
    // console.log("payload", payload);
  }
}
export class AddBookAction implements Action {
  readonly type = ADD_BOOK;
  constructor(public payload: bookDetailsDto) {}
}

export type BookDetailsActions = GetBookDetailAction | AddBookAction;
