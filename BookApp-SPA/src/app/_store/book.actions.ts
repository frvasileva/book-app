import { Action } from "@ngrx/store";

export const ADD_BOOK_TO_CATALOG = "ADD_BOOK_TO_CATALOG";
export const REMOVE_BOOK_FROM_CATALOG = "REMOVE_BOOK_FROM_CATALOG";

export class AddBookToCatalogAction implements Action {
  readonly type = ADD_BOOK_TO_CATALOG;
  constructor(public payload: any) {}
}
export class RemoveBookFromCatalogAction implements Action {
  readonly type = REMOVE_BOOK_FROM_CATALOG;
  constructor(public payload: any) {}
}

export type BookActions = AddBookToCatalogAction | RemoveBookFromCatalogAction;
