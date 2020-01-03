import { Component, OnInit } from "@angular/core";
import { Store } from "@ngrx/store";
import { User } from "../../_models/user";
import { UserState } from "../../_store/user.reducer";
import { SeoHelperService } from "../../_shared/seo-helper.service";

@Component({
  selector: "app-users-list",
  templateUrl: "./users-list.component.html",
  styleUrls: ["./users-list.component.scss"]
})
export class UsersListComponent implements OnInit {
  userList: User[];

  constructor(
    private store: Store<{ userState: UserState }>,
    private seoHelper: SeoHelperService
  ) {}

  ngOnInit() {
    this.seoHelper.setSeoMetaTags("Users");

    this.store
      .select(state => state)
      .subscribe(res => {
        this.userList = Object.values(res.userState.users) as User[];

        // Remove current user
        this.userList = this.userList.filter(
          item => item.friendlyUrl !== res.userState.currentUser
        );
      });
  }
}
