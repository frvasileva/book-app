import { Author } from "../authors/author.model";
import { BookAction } from "./bookAction.model";

export class Book {
  public id: string;
  public title: string;
  public bookType: string;
  public description?: string;
  public coverPath?: string;
  public publisher: string;

  public author?: Author;
  public tags: string[];

  public bookActions: BookAction[];

  constructor(
    id: string,
    title: string,
    bookType: string,
    description: string,
    coverPath: string,
    publisher: string,
    author: Author,
    tags: string[],
    bookActions: BookAction[]
  ) {
    this.id = id;
    this.title = title;
    this.bookType = bookType;
    this.description = description;
    this.coverPath = coverPath;
    this.publisher = publisher;
    this.author = author;
    this.tags = tags;
    this.bookActions = bookActions;
  }

  bla(obj: { X: BookType }) {
    const aaaa = obj.X;
    console.log(aaaa);
  }
}
