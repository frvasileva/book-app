import { Book } from "src/app/books/book.model";
import { Art } from "src/app/shared/DTO/Art";

export class UserProfile {
  username: string;
  avatarPath: string;
  city: string;
  registeredSince: Date;
  books: Book[];
  arts: Art[];

  constructor(username, avatarPath, city, registeredSince, books, arts) {
    this.username = username;
    this.avatarPath = avatarPath;
    this.city = city;
    this.registeredSince = registeredSince;
    this.books = books;
    this.arts = arts;
  }
}
