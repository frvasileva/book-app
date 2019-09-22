import { DiscussionItemDto } from "./discussionItemDto";

export interface DiscussionDto {
  id: number;
  title: string;
  body: string;
  addedOn: Date;
  bookId: number;
  bookFriendlyUrl: string;
  bookTitle: string;
  friendlyUrl: string;
  discussionItems: DiscussionItemDto[];
  userAvatarPath: string;
  userFriendlyUrl: string;
  userId: number;
  username: string;
}
