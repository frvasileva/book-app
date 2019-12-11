import * as CatalogActions from "./catalog.actions";
import * as BookActions from "./book.actions";
import { CatalogItemDto } from "../_models/catalogItem";

export interface CatalogState {
  catalog: CatalogItemDto[];
}

export const initialState: CatalogState = {
  catalog: []
};

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
    case CatalogActions.UPDATE_CATALOG: {
      const { id, isPublic } = action.payload;
      const catalog = state.catalog.find(item => {
        return item.id === id;
      });
      catalog.isPublic = isPublic;
      return state;
    }
    case BookActions.REMOVE_BOOK_FROM_CATALOG: {
      const { bookId, catalogId } = action.payload;
      const catalog = state.catalog.find(
        catalogItem => catalogItem.id === catalogId
      );
      if (catalog) {
        catalog.books = catalog.books.filter(
          bookItem => bookItem.id !== bookId
        );
      }
      return state;
    }
    default:
      return state;
  }
}
