export class BookRequest {
  bookId: string;
  currentUserId: string;
  requestedUserId: string;
  message: string;

  constructor(bookId, message) {
    this.bookId = bookId;
    this.message = message;
  }
}
