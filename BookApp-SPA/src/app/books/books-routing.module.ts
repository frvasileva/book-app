import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { BooksComponent } from "./books.component";
import { BooksListComponent } from "./books-list/books-list.component";
import { BooksDetailComponent } from "./books-detail/books-detail.component";
import { AddBookComponent } from "./add-book/add-book.component";
import { RequestBookComponent } from "./request-book/request-book.component";
import { AddBookCoverComponent } from "./add-book-cover/add-book-cover.component";
import { CatalogCreateComponent } from "./catalog-create/catalog-create.component";
import { CatalogListComponent } from "./catalog-list/catalog-list.component";
import { CatalogDetailsComponent } from "./catalog-details/catalog-details.component";
import { AuthenticationGuard } from "../_guards/authentication.guard";
import { PublicCatalogListComponent } from "./public-catalog-list/public-catalog-list.component";
import { BooksListByCategoryComponent } from "./books-list-by-category/books-list-by-category.component";

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
        path: ":category/:pageNumber",
        component: BooksListByCategoryComponent,
        canActivate: [AuthenticationGuard]
      },
      {
        path: "add",
        component: AddBookComponent,
        canActivate: [AuthenticationGuard]
      },
      {
        path: "add-cover/:friendly-url",
        component: AddBookCoverComponent,
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
        path: "catalog/details/:friendly-url",
        component: CatalogDetailsComponent,
        canActivate: [AuthenticationGuard]
      },
      {
        path: "catalogs",
        component: PublicCatalogListComponent,
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
