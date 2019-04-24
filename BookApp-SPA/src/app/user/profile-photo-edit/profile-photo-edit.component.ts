import { Component, OnInit } from "@angular/core";
import { environment } from "src/environments/environment";
import { Store } from "@ngrx/store";
import { UserState } from "src/app/_store/user.reducer";

@Component({
  selector: "app-profile-photo-edit",
  templateUrl: "./profile-photo-edit.component.html",
  styleUrls: ["./profile-photo-edit.component.scss"]
})
export class ProfilePhotoEditComponent implements OnInit {
  baseUrl = environment.apiUrl;
  apiDestinationUrl: string;
  currentUserUrl: string;
  uploaderType = "profile-photo";

  constructor(private store: Store<{ userState: UserState }>) {}

  ngOnInit() {
    this.store
      .select(state => state)
      .subscribe(res => {
        this.currentUserUrl = res.userState.currentUser;
        this.apiDestinationUrl = this.baseUrl + "profile/add-photo/" + this.currentUserUrl;
      });
  }
}
