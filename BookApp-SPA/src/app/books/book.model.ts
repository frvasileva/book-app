import { Author } from "../authors/author.model";

export class Book {
  public id: number;
  public userId: string;
  public title: string;
  public bookType: string;
  public description?: string;
  public coverPath?: string;
  public publisher: string;
  public recommendationCategory: string;

  public author?: Author;
  public tags: string[];

  public bookCatalogs: any;

  constructor(
    id: number,
    userId: string,
    title: string,
    bookType: string,
    description: string,
    coverPath: string,
    publisher: string,
    recommendationCategory: string,
    author: Author,
    tags: string[],
    bookCatalogs: any
  ) {
    this.id = id;
    this.userId = userId;
    this.title = title;
    this.bookType = bookType;
    this.description = description;
    this.coverPath = coverPath;
    this.publisher = publisher;
    this.recommendationCategory = recommendationCategory;
    this.author = author;
    this.tags = tags;
    this.bookCatalogs = bookCatalogs;
  }
}
