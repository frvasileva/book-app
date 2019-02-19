import * as BookListActions from "./bookList.actions";

import { BookAction } from "../../bookAction.model";
import { Book } from "../../book.model";
import { Author } from "src/app/authors/author.model";

const initialState = {
  books: [
    new Book(
      "1",
      "Winnie the pooh",
      "ebook",
      "The best book ever",
      "https://about.canva.com/wp-content/uploads/sites/3/2015/01/children_bookcover.png",
      "Сиела",
      new Author("1", "Bla bla bla ", "Nice bio", ""),
      ["dutch people"],
      [new BookAction(true, "like", true, 5)]
    ),
    new Book(
      "2",
      "Dutch people",
      "ebook",
      // tslint:disable-next-line:max-line-length
      "Stuff Dutch People Like is a bestselling, humorous cultural study of the Netherlands and its peculiar inhabitants. It investigates and highlights the idiosyncrasies of the Dutch, their culture and their uncanny ability to talk on a mobile phone while carrying 2.5 children, 6 bags of groceries and a mattress balanced on a gear-less bicycle. This book is a must read for: 1) People with Dutch ancestry wondering why they are so strange. / 2) Anybody who wants to survive their next trip to Amsterdam or Holland / 3) People with a Dutch partner who will finally understand that all their relationship issues can be boiled down to one fact: Their partner is Dutch!",
      "https://spark.adobe.com/images/landing/examples/how-to-book-cover.jpg",
      "Сиела",
      new Author("2", "Alan Miln", "Nice bio", ""),
      ["dutch people"],
      [new BookAction(true, "like", true, 5)]
    )
  ]
};

export function bookListReducer(
  state = initialState,
  action: BookListActions.BookListActions
) {
  switch (action.type) {
    case BookListActions.ADD_BOOK: {
      console.log({ ...state, books: [state.books], action });
      return {
        ...state,
        books: [...state.books, action.payload]
      };
    }
    // case BookListActions.UPDATE_BOOK:
    //   return { ...state, books: [state.books], action };
    default:
      return state;
  }
}
