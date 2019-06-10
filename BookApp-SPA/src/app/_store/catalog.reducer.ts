import * as CatalogActions from "./catalog.actions";

const initialState = {
  catalog: []
};

//TODO: Initial version! Change what the state returns

export function catalogReducer(
  state = initialState,
  action: CatalogActions.CatalogActions
) {
  switch (action.type) {
    case CatalogActions.GET_CATALOGS: {
      return {
        ...state,
        catalog: action.payload
      };
    }
    case CatalogActions.ADD_CATALOG: {

      console.log("current state", state.catalog);
      console.log("current payload", action.payload);

      return {
        ...state,
        catalog: [...state.catalog, action.payload]
      };
    }
    default:
      return state;
  }
}
