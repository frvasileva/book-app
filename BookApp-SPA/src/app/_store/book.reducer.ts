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
    case BookActions.SET_BOOKS: {
      console.log("...action.payload.items", action.payload.items);
      return {
        ...state,
        books: [...state.books, ...action.payload.items],
        totalNumber: action.payload.totalNumber
      };
    }
    case BookActions.SET_BOOK: {
      // @TODO: avoid duplicate books in the list
      return {
        ...state,
        books: [action.payload, ...state.books]
      };
    }
    case BookActions.SET_BOOK_PHOTO: {
      const book = state.books.find(item => {
        return item.friendlyUrl === action.payload.friendlyUrl;
      });
      book.photoPath = action.payload.photoPath;
      return state;
    }
    case BookActions.ADD_BOOK_TO_CATALOG: {
      const book = state.books.find(b => b.id === action.payload.bookId);
      book.bookCatalogs.push({ catalogId: action.payload.catalogId });
      console.log(state);
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
// ...state.users,
// [userFriendlyUrl]: {
//   ...state.users[userFriendlyUrl],
//   isFollowedByCurrentUser
// }
