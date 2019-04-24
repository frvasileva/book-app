import { Profile } from "./_models/profile";
import { Message } from "./messages/model/message.model";
import { CatalogItemDto } from "./_models/catalogItem";

export interface AppState {
  readonly isUserLoggedIn: boolean;
  readonly messageList: Message[];
  readonly catalog: CatalogItemDto;
  readonly userProfile: Profile;
}
