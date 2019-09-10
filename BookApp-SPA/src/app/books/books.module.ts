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
import { CatalogListComponent } from './catalog-list/catalog-list.component';
import { CatalogItemComponent } from './catalog-item/catalog-item.component';
import { CatalogCreateComponent } from './catalog-create/catalog-create.component';
import { CatalogDetailsComponent } from './catalog-details/catalog-details.component';
import { PublicCatalogListComponent } from './public-catalog-list/public-catalog-list.component';
import { CatalogListPureComponent } from './catalog-list-pure/catalog-list-pure.component';
import { BooksListByCategoryComponent } from './books-list-by-category/books-list-by-category.component';


import { PaginationModule } from "ngx-bootstrap";

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
    AddBookCoverComponent,
    CatalogListComponent,
    CatalogItemComponent,
    CatalogCreateComponent,
    CatalogDetailsComponent,
    PublicCatalogListComponent,
    CatalogListPureComponent,
    BooksListByCategoryComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    BooksRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    FileUploadModule,
    SharedModule,
    PaginationModule.forRoot()
  ],
  exports: [BookCardItemComponent, CatalogListComponent]
})
export class BooksModule {}
