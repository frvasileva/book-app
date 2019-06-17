import { Book } from "../books/book.model";

export interface CatalogItemDto {
  id?: number;
  name: string;
  isPublic: boolean;
  userId: number;
  books: Book[];
  friendlyUrl: string;
}
