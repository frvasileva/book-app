import * as BookActions from "./book.actions";

const initialState = {
  books: []
};

export function bookReducer(
  state = initialState,
  action: BookActions.BookActions
) {
  switch (action.type) {
    case BookActions.SET_BOOKS: {
      return {
        ...state,
        books: action.payload
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
    default:
      return state;
  }
}
// ...state.users,
// [userFriendlyUrl]: {
//   ...state.users[userFriendlyUrl],
//   isFollowedByCurrentUser
// }
