import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";
import { ReactiveFormsModule } from "@angular/forms";

import { UserComponent } from "./user.component";
import { SignupComponent } from "./signup/signup.component";
import { LoginComponent } from "./login/login.component";
import { ProfileComponent } from "./profile/profile.component";
import { UserRoutingModule } from "./user-routing.module";
import { UsersListComponent } from "./users-list/users-list.component";
import { BooksModule } from "../books/books.module";
import { ProfileCardComponent } from "./profile-card/profile-card.component";
import { ProfileEditComponent } from "./profile-edit/profile-edit.component";
import { PhotoEditorComponent } from "../shared/photo-editor/photo-editor.component";
import { FileUploadModule } from "ng2-file-upload";
import { ProfilePhotoEditComponent } from './profile-photo-edit/profile-photo-edit.component';

@NgModule({
  declarations: [
    LoginComponent,
    SignupComponent,
    ProfileComponent,
    UserComponent,
    UsersListComponent,
    ProfileCardComponent,
    ProfileEditComponent,
    PhotoEditorComponent,
    ProfilePhotoEditComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    UserRoutingModule,
    BooksModule,
    FileUploadModule
  ]
})
export class UserModule {}
