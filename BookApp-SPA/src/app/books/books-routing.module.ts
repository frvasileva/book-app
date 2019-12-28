import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { BooksComponent } from "./books.component";
import { BooksListComponent } from "./books-list/books-list.component";
import { BooksDetailComponent } from "./books-detail/books-detail.component";
import { AddBookComponent } from "./add-book/add-book.component";
import { RequestBookComponent } from "./request-book/request-book.component";
import { AddBookCoverComponent } from "./add-book-cover/add-book-cover.component";
import { CatalogCreateComponent } from "./catalog-create/catalog-create.component";
import { CatalogDetailsComponent } from "./catalog-details/catalog-details.component";
import { AuthenticationGuard } from "../_guards/authentication.guard";
import { PublicCatalogListComponent } from "./public-catalog-list/public-catalog-list.component";
import { BooksListByCategoryComponent } from "./books-list-by-category/books-list-by-category.component";
import { DiscussionCreateComponent } from "./discussion-create/discussion-create.component";
import { DiscussionDetailsComponent } from "./discussion-details/discussion-details.component";
import { DiscussionsListComponent } from "./discussions-list/discussions-list.component";
import { DiscussionsPerBookComponent } from "./discussions-per-book/discussions-per-book.component";

const booksRoutes: Routes = [
  {
    path: "",
    component: BooksComponent,
    children: [
      {
        path: "",
        component: BooksListComponent,
        canActivate: [AuthenticationGuard]
      },
      {
        path: "details/:url",
        component: BooksDetailComponent,
        canActivate: [AuthenticationGuard]
      },
      {
        path: "add-cover/:friendly-url",
        component: AddBookCoverComponent,
        canActivate: [AuthenticationGuard]
      },
      {
        path: "category/:category/:pageNumber?",
        component: BooksListByCategoryComponent,
        canActivate: [AuthenticationGuard]
      },
      {
        path: "add",
        component: AddBookComponent,
        canActivate: [AuthenticationGuard]
      },

      { path: "request-book/:id", component: RequestBookComponent },
      {
        path: "catalog/add",
        component: CatalogCreateComponent,
        canActivate: [AuthenticationGuard]
      },
      {
        path: "catalog/edit/:friendly-url",
        component: CatalogCreateComponent,
        canActivate: [AuthenticationGuard]
      },
      {
        path: "catalog/details/:friendly-url/:pageNumber",
        component: CatalogDetailsComponent,
        canActivate: [AuthenticationGuard],
        pathMatch: 'full'
      },
      {
        path: "catalogs/:pageNumber",
        component: PublicCatalogListComponent,
        canActivate: [AuthenticationGuard]
      },
      {
        path: "discussions/add/:bookId",
        component: DiscussionCreateComponent,
        canActivate: [AuthenticationGuard]
      },
      {
        path: "discussions",
        component: DiscussionsListComponent,
        canActivate: [AuthenticationGuard]
      },
      {
        path: "discussions/details/:friendlyUrl",
        component: DiscussionDetailsComponent,
        canActivate: [AuthenticationGuard]
      },
      {
        path: "discussions/per-book/:bookId",
        component: DiscussionsPerBookComponent,
        canActivate: [AuthenticationGuard]
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(booksRoutes)],
  exports: [RouterModule]
})
export class BooksRoutingModule {}
