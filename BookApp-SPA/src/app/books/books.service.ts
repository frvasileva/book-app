import { Injectable } from "@angular/core";

import { Author } from "../authors/author.model";
import { Book } from "./book.model";
import { BookAction } from "./bookAction.model";

@Injectable()
export class BookService {
  private books: Book[] = [
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
    ),
    new Book(
      "3",
      "The perfect dutch",
      "paper",
      "The best book ever",
      "https://s3-eu-west-1.amazonaws.com/books.readingagency/images/978/00/0737/9780007371464.jpg",
      "Изток-Запад",
      new Author("3", "Kathian Brands", "Nice bio", ""),
      ["dutch people"],
      [new BookAction(true, "like", true, 5)]
    ),
    new Book(
      "4",
      "Ян Бибиян",
      "paper",
      // tslint:disable-next-line:max-line-length
      "Известен факт е, че читателя обръща внимание на съдържанието, което чете, а не на оформлението му. Свойството на Lorem Ipsum е, че до голяма степен има нормално разпределение на буквите и се чете по-лесно, за разлика от нормален текст на английски език като Много системи за публикуване и редактори на Уеб страници използват Lorem Ipsum като примерен текстов модел ади което при търсене на фразата в Интернет ще бъдат открити много сайтове в процес на разработка. Някой от тези сайтове биват променяни с времето, а други по случайност или нарочно(за забавление и пр.) биват оставяни в този си незавършен вид. а в Интернет ще бъдат открити много сайтове в процес на разработка. Някой от тези сайтове биват променяни с времето, а други по случайност или нарочно(за забавление и пр.) биват оставяни в този си незавършен вид.а в Интернет ще бъдат открити много сайтове в процес на разработка. Някой от тези сайтове биват променяни с времето, а други по случайност или нарочно(за забавление и пр.) биват оставяни в този си незавършен вид.",
      "https://static.framar.bg/product/scorpio-qn-bibiqn.jpg",
      "Кръгозор",
      new Author("4", "Елин Пелин", "Nice bio", ""),
      ["dutch people"],
      [new BookAction(true, "like", true, 5)]
    )
  ];

  getBooks() {
    return this.books.slice();
  }

  getBook(id: number) {
    return this.books.find(x => x.id === id.toString());
  }

  // TODO: Implement the method
  addToWantToReadList(bookId: number) {
    console.log("book {0} added to favorites", bookId);
  }

  addToAlreadyRead(bookId: number) {
    console.log("book {0} added to already read books", bookId);
  }

  wantToShare(bookId: number) {
    console.log("User want to share book {0}", bookId);
  }
}
