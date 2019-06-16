import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { BooksComponent } from "./books.component";
import { BooksListComponent } from "./books-list/books-list.component";
import { BooksDetailComponent } from "./books-detail/books-detail.component";
import { AddBookComponent } from "./add-book/add-book.component";
import { RequestBookComponent } from "./request-book/request-book.component";
import { AddBookCoverComponent } from "./add-book-cover/add-book-cover.component";
import { CatalogCreateComponent } from "./catalog-create/catalog-create.component";
import { CatalogListComponent } from './catalog-list/catalog-list.component';
import { CatalogDetailsComponent } from './catalog-details/catalog-details.component';

const booksRoutes: Routes = [
  {
    path: "",
    component: BooksComponent,
    children: [
      { path: "", component: BooksListComponent },
      { path: "details/:url", component: BooksDetailComponent },
      { path: "add", component: AddBookComponent },
      { path: "add-cover/:friendly-url", component: AddBookCoverComponent },
      { path: "request-book/:id", component: RequestBookComponent },
      { path: "catalog/add", component: CatalogCreateComponent },
      { path: "catalog/edit/:friendly-url", component: CatalogCreateComponent },
      { path: "catalog/details/:friendly-url", component: CatalogDetailsComponent },
      { path: "catalogs", component: CatalogListComponent },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(booksRoutes)],
  exports: [RouterModule]
})
export class BooksRoutingModule {}
