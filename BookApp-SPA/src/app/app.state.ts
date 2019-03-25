import { Profile } from "./_models/profile";
import { Book } from "./_models/books";
import { bookDetailsDto } from "./_models/bookDetailsDto";
import { Message } from "./messages/model/message.model";
import { CatalogItemDto } from "./_models/catalogItem";

export interface AppState {
  readonly isUserLoggedIn: boolean;
  readonly bookList: Book[];
  readonly bookDetails: bookDetailsDto;
  readonly messageList: Message[];
  readonly catalog: CatalogItemDto;
  readonly userProfile: Profile;
  readonly userProfiles: Profile[];
}
