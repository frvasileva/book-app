export interface DiscussionItemDto {
  id: number;
  body: string;
  addedOn: Date;
  userId: number;
  userAvatarPath: string;
  userFriendlyUrl: string;
  username: string;
  discussionId: number;
}
