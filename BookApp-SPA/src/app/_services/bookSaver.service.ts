import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { HttpClient } from "@angular/common/http";
import { Router } from "@angular/router";
import { Store } from "@ngrx/store";
import { map } from "rxjs/operators";

import { AlertifyService } from "./alertify.service";
import { Book } from "../_models/books";
import * as UserActions from "../_store/user.actions";
import { CatalogPureDto } from "../_models/catalogPureDto";
import * as BookActions from "../_store/book.actions";
import { CatalogItemDto } from "../_models/catalogItem";

@Injectable()
export class BookSaverService {
  baseUrl = environment.apiUrl + "catalog/";
  baseUrlBookService = environment.apiUrl + "book/";

  constructor(
    private http: HttpClient,
    private router: Router,
    private store: Store<{
      bookState: { books: Book[] };
      catalogState: { catalog: CatalogItemDto[] };
    }>,
    private alertify: AlertifyService
  ) {}

  getUserCatalogList(friendlyUrl: string) {
    return this.http
      .get(this.baseUrl + "user-catalogs-pure-list/" + friendlyUrl)
      .subscribe(
        data => {
          this.store.dispatch(
            new UserActions.SetCurrentUserCatalogsAction(<CatalogPureDto[]>data)
          );
        },
        error => {
          this.alertify.error(error);
        }
      );
  }

  addBookToCatalog(catalogId: number, bookId: number) {
    const model = { catalogId, bookId, catalogName: "" };
    console.log(model);
    return this.http
      .post(this.baseUrlBookService + "add-to-catalog", model)
      .pipe(
        map((response: any) => {
          console.log({ bookId, catalogId });
          this.store.dispatch(
            new BookActions.AddBookToCatalogAction({ bookId, catalogId })
          );

          return response;
        })
      )
      .subscribe();
  }

  removeBookFromCatalog(catalogId: number, bookId: number) {
    return this.http
      .get(
        this.baseUrlBookService +
          "delete-book-from-catalog/" +
          catalogId +
          "/" +
          bookId
      )
      .subscribe(
        data => {
          this.store.dispatch(
            new BookActions.RemoveBookFromCatalogAction({ bookId, catalogId })
          );
        },
        error => {
          this.alertify.error(error);
        }
      );
  }
}
