import { Book } from "../books/book.model";

export interface Profile {
  id: number;
  email: string;
  userName: string;
  avatarPath: string;
  knownAs: string;
  introduction: string;
  city: string;
  country: string;
  friendlyUrl: string;
  books: Book[];
}
