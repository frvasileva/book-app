import { Action } from "@ngrx/store";
import { CatalogCreateDto } from "../_models/catalogCreateDto";

export const GET_CATALOGS = "GET_CATALOGS";
export const ADD_CATALOG = "ADD_CATALOG";
export const UPDATE_CATALOG = "UPDATE_CATALOG";
export const DELETE_CATALOG = "DELETE_CATALOG";

export class GetCatalogsAction implements Action {
  readonly type = GET_CATALOGS;
  constructor(public payload: any) {}
}
export class AddCatalogAction implements Action {
  readonly type = ADD_CATALOG;
  constructor(public payload: CatalogCreateDto) {}
}
export class UpdateCatalogAction implements Action {
  readonly type = UPDATE_CATALOG;
  payload: CatalogCreateDto;
}
export class DeleteCatalogAction implements Action {
  readonly type = DELETE_CATALOG;
}

export type CatalogActions =
  | GetCatalogsAction
  | AddCatalogAction
  | UpdateCatalogAction
  | DeleteCatalogAction;
