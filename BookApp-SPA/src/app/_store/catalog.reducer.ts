import * as CatalogActions from "./catalog.actions";
import * as BookActions from "./book.actions";
import { CatalogItemDto } from "../_models/catalogItem";

// const initialState = {
//   catalog: []
// };

export interface CatalogState {
  catalog: CatalogItemDto[];
}

export const initialState: CatalogState = {
  catalog: []
};

// TODO: Initial version! Change what the state returns

export function catalogReducer(
  state = initialState,
  action: CatalogActions.CatalogActions | BookActions.BookActions
) {
  switch (action.type) {
    case CatalogActions.GET_CATALOGS: {
      return {
        ...state,
        catalog: action.payload
      };
    }
    case CatalogActions.ADD_CATALOG: {
      return {
        ...state,
        catalog: [...state.catalog, action.payload]
      };
    }
    case BookActions.REMOVE_BOOK_FROM_CATALOG: {
      const { bookId, catalogId } = action.payload;
      const catalog = state.catalog.find(catalogItem => catalogItem.id === catalogId);
      catalog.books = catalog.books.filter(bookItem => bookItem.id !== bookId);
      return state;
    }
    default:
      return state;
  }
}
