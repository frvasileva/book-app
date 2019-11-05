import { Profile } from "./_models/profile";
import { CatalogItemDto } from "./_models/catalogItem";

export interface AppState {
  readonly isUserLoggedIn: boolean;
  readonly catalog: CatalogItemDto;
  readonly userProfile: Profile;
}
