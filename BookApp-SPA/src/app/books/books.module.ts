import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { BooksListComponent } from "./books-list/books-list.component";
import { BooksDetailComponent } from "./books-detail/books-detail.component";
import { BooksComponent } from "./books.component";
import { BooksRoutingModule } from "./books-routing.module";
import { RouterModule } from "@angular/router";
import { AddBookComponent } from "./add-book/add-book.component";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { RequestBookComponent } from "./request-book/request-book.component";
import { BookSaverComponent } from "./book-saver/book-saver.component";
import { BookCardItemComponent } from "./book-card-item/book-card-item.component";

@NgModule({
  declarations: [
    BooksListComponent,
    BooksDetailComponent,
    BooksComponent,
    AddBookComponent,
    RequestBookComponent,
    BookSaverComponent,
    BookCardItemComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    BooksRoutingModule,
    FormsModule,
    ReactiveFormsModule
  ],
  exports: [BookCardItemComponent]
})
export class BooksModule {}
