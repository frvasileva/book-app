import { DiscussionItemDto } from "./discussionItemDto";

export interface DiscussionDto {
  id: number;
  title: string;
  body: string;
  addedOn: Date;
  userId: number;
  bookId: number;
  bookFriendlyUrl: string;
  friendlyUrl: string;
  discussionItems: DiscussionItemDto[];
}
