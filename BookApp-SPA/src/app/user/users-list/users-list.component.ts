import { Component, OnInit } from "@angular/core";
import { Store } from "@ngrx/store";
import { User } from "src/app/_models/user";
import { UserState } from "src/app/_store/user.reducer";
import { Title, Meta } from "@angular/platform-browser";
import { settings } from "src/app/_shared/settings";

@Component({
  selector: "app-users-list",
  templateUrl: "./users-list.component.html",
  styleUrls: ["./users-list.component.scss"]
})
export class UsersListComponent implements OnInit {
  userList: User[];

  constructor(
    private store: Store<{ userState: UserState }>,
    private titleService: Title,
    private metaTagService: Meta
  ) {}

  ngOnInit() {
    this.titleService.setTitle("Users " + settings.seo_appName_title);
    this.metaTagService.updateTag({
      name: "description",
      content: "Book App Users"
    });


    this.store
      .select(state => state)
      .subscribe(res => {
        console.log("gossip");
        this.userList = Object.values(res.userState.users) as User[];
      });
  }
}
