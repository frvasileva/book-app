import { Injectable } from "@angular/core";
import { UserProfile } from "./profile/UserProfile.model";
import { Book } from "../books/book.model";
import { Author } from "../authors/author.model";
import { Art } from "../shared/DTO/Art";
import { BookAction } from "../books/bookAction.model";

@Injectable()
export class UserService {
  //  constructor(username, avatarPath, city, registeredSince, books, arts) {

  private userProfile: UserProfile = new UserProfile(
    "frvasileva",
    // tslint:disable-next-line:max-line-length
    "https://scontent.fsof6-1.fna.fbcdn.net/v/t1.0-9/27971791_10155503500733095_4511725722546809828_n.jpg?_nc_cat=106&_nc_ht=scontent.fsof6-1.fna&oh=c7f378de151674d7ecc0d66b3f711d9c&oe=5C8EEA32",
    "Sofia",
    new Date(),
    [
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
        "1",
        "Winnie the pooh 2",
        "ebook",
        "The best book ever",
        "https://about.canva.com/wp-content/uploads/sites/3/2015/01/children_bookcover.png",
        "Сиела",
        new Author("1", "Bla bla bla ", "Nice bio", ""),
        ["dutch people"],
        [new BookAction(true, "like", true, 5)]
      )
    ],
    [
      new Art(
        "Равносметка",
        "Здравей, мой суров Декември",
        new Date(),
        "frvasileva"
      )
    ]
  );

  private userList: UserProfile[] = [this.userProfile, this.userProfile];

  get() {
    return this.userProfile;
  }

  getAllUsers() {
    return this.userList;
  }
}
