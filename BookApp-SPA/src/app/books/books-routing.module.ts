import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { BooksComponent } from "./books.component";
import { BooksListComponent } from "./books-list/books-list.component";
import { BooksDetailComponent } from "./books-detail/books-detail.component";
import { AddBookComponent } from "./add-book/add-book.component";
import { RequestBookComponent } from "./request-book/request-book.component";

const booksRoutes: Routes = [
  {
    path: "",
    component: BooksComponent,
    children: [
      { path: "", component: BooksListComponent },
      { path: "details/:id", component: BooksDetailComponent },
      { path: "add", component: AddBookComponent },
      { path: "request-book/:id", component: RequestBookComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(booksRoutes)],
  exports: [RouterModule]
})
export class BooksRoutingModule {}
