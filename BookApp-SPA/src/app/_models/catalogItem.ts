import { Book } from "../books/book.model";

export interface rpCatalogItemDto {
  id?: number;
  name: string;
  isPublic: boolean;
  userId: number;
  userFriendlyUrl: string;
  books: Book[];
  friendlyUrl: string;
  user: any;
  created: Date;
}
