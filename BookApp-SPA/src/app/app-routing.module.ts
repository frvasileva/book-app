import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";

import { HomeComponent } from "./ui-core/home/home.component";
import { AboutComponent } from "./ui-core/about/about.component";
import { NotFoundComponent } from "./ui-core/not-found/not-found.component";

const appRoutes: Routes = [
  { path: "", component: HomeComponent },
  {
    path: "books",
    loadChildren: "./books/books.module#BooksModule"
  },
  {
    path: "authors",
    loadChildren: "./authors/author.module#AuthorModule"
  },
  {
    path: "user",
    loadChildren: "./user/user.module#UserModule"
  },
  { path: "about-us", component: AboutComponent },
  { path: "404", component: NotFoundComponent },
  { path: "**", component: NotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(appRoutes, { enableTracing: false })],
  exports: [RouterModule]
})
export class AppRoutingModule {}
