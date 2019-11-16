import * as BookActions from "./book.actions";

const initialState = {
  books: [],
  totalNumber: 0
};

export function bookReducer(
  state = initialState,
  action: BookActions.BookActions
) {
  switch (action.type) {
    case BookActions.ADD_BOOK_TO_CATALOG: {
      const book = state.books.find(b => b.id === action.payload.bookId);
      book.bookCatalogs.push({ catalogId: action.payload.catalogId });
      return state;
    }
    case BookActions.REMOVE_BOOK_FROM_CATALOG: {
      const book = state.books.find(b => b.id === action.payload.bookId);
      book.bookCatalogs = book.bookCatalogs.filter(
        i => i.catalogId !== action.payload.catalogId
      );
      return state;
    }
    default:
      return state;
  }
}
