import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { FileUploadModule } from "ng2-file-upload";
import { RouterModule } from "@angular/router";

import { SharedModule } from "../_shared/shared/shared.module";

import { BooksListComponent } from "./books-list/books-list.component";
import { BooksDetailComponent } from "./books-detail/books-detail.component";
import { BooksComponent } from "./books.component";
import { BooksRoutingModule } from "./books-routing.module";
import { AddBookComponent } from "./add-book/add-book.component";
import { RequestBookComponent } from "./request-book/request-book.component";
import { BookSaverComponent } from "./book-saver/book-saver.component";
import { BookCardItemComponent } from "./book-card-item/book-card-item.component";
import { ProgressComponent } from "./progress/progress.component";
import { AddBookCoverComponent } from "./add-book-cover/add-book-cover.component";

@NgModule({
  declarations: [
    BooksListComponent,
    BooksDetailComponent,
    BooksComponent,
    AddBookComponent,
    RequestBookComponent,
    BookSaverComponent,
    BookCardItemComponent,
    ProgressComponent,
    AddBookCoverComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    BooksRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    FileUploadModule,
    SharedModule
  ],
  exports: [BookCardItemComponent]
})
export class BooksModule {}
