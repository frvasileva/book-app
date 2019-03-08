import { Action } from "@ngrx/store";
import { bookDetailsDto } from "../_models/bookDetailsDto";

export const GET_BOOK_DETAILS = "GET_BOOK_DETAILS";

export class GetBookDetailAction implements Action {
  readonly type = GET_BOOK_DETAILS;
  constructor(public payload: bookDetailsDto) {
   // console.log("payload", payload);
  }
}

export type BookDetailsActions = GetBookDetailAction;
