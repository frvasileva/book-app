import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { NotFoundComponent } from "../ui-core/not-found/not-found.component";
import { AuthorsComponent } from "./authors.component";
import { AuthorDetailsComponent } from "./author-details/author-details.component";
import { AuthorsListComponent } from "./authors-list/authors-list.component";


const authorsRoutes: Routes = [
  {
    path: "", component: AuthorsComponent,
    children: [
      { path: "", component: AuthorsListComponent },
      { path: "details/:id", component: AuthorDetailsComponent },
    ]
  }
];

@NgModule({
  imports: [
    RouterModule.forChild(authorsRoutes)
  ],
  exports: [RouterModule]
})
export class AuthorsRoutingModule {

}
