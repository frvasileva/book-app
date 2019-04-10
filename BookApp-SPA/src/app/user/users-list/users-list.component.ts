import { Component, OnInit } from "@angular/core";
import { Store } from "@ngrx/store";
import { User } from 'src/app/_models/user';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: "app-users-list",
  templateUrl: "./users-list.component.html",
  styleUrls: ["./users-list.component.scss"]
})
export class UsersListComponent implements OnInit {
  userList: User[];

  constructor(
    private store: Store<{
      userProfiles: User[];
    }>,
    private userService: UserService
  ) {}

  ngOnInit() {
    this.userService.getUsers();

    this.store
      .select(state => state)
      .subscribe(res => {
        this.userList = res.userProfiles as User[];
      });
  }
}
