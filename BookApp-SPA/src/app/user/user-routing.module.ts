import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { LoginComponent } from "./login/login.component";
import { SignupComponent } from "./signup/signup.component";
import { ProfileComponent } from "./profile/profile.component";
import { UserComponent } from "./user.component";
import { UsersListComponent } from "./users-list/users-list.component";
import { AuthenticationGuard } from "../_guards/authentication.guard";
import { ProfileEditComponent } from "./profile-edit/profile-edit.component";
import { ProfilePhotoEditComponent } from "./profile-photo-edit/profile-photo-edit.component";
import { InviteFriendComponent } from "./invite-friend/invite-friend.component";

const userRoutes: Routes = [
  {
    path: "",
    component: UserComponent,
    children: [
      { path: "login", component: LoginComponent },
      { path: "sign-up", component: SignupComponent },
      {
        path: "profile",
        component: ProfileComponent,
        canActivate: [AuthenticationGuard]
        // resolve: { profile: ProfileResolver }
      },
      {
        path: "profile/:friendlyUrl",
        component: ProfileComponent,
        canActivate: [AuthenticationGuard]
        // resolve: { profile: ProfileResolver }
      },
      {
        path: "profile/edit/:friendlyUrl",
        component: ProfileEditComponent,
        canActivate: [AuthenticationGuard]
        // resolve: { profile: ProfileResolver }
      },
      {
        path: "profile/edit-photo/:friendlyUrl",
        component: ProfilePhotoEditComponent,
        canActivate: [AuthenticationGuard]
      },
      {
        path: "user-list",
        component: UsersListComponent,
        canActivate: [AuthenticationGuard]
      },
      {
        path: "invite-friend",
        component: InviteFriendComponent,
        canActivate: [AuthenticationGuard]
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(userRoutes)],
  exports: [RouterModule]
})
export class UserRoutingModule {}
