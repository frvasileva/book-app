import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";

import { AuthorDetailsComponent } from "./author-details/author-details.component";
import { AuthorsRoutingModule } from "./authors-routing.module";
import { AuthorsListComponent } from "./authors-list/authors-list.component";
import { AuthorsComponent } from "./authors.component";

@NgModule({
  declarations: [
    AuthorsComponent,
    AuthorDetailsComponent,
    AuthorsListComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    AuthorsRoutingModule],
  exports: []
})
export class AuthorModule {

}
