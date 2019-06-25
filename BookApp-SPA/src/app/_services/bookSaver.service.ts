import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { HttpClient } from "@angular/common/http";
import { Router } from "@angular/router";
import { Store } from "@ngrx/store";
import { Book } from "../_models/books";
import { AlertifyService } from "./alertify.service";
import * as UserActions from "../_store/user.actions";
import { CatalogPureDto } from "../_models/catalogPureDto";
import { map } from "rxjs/operators";
import * as CatalogActions from "../_store/catalog.actions";
import { CatalogCreateDto } from "../_models/catalogCreateDto";
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

  // getUserLists(keyword: string) {
  //   if (keyword === null) {
  //     return this.bookSaverItemList;
  //   } else {
  //     return this.bookSaverItemList.filter(item =>
  //       item.label.includes(keyword)
  //     );
  //   }
  // }
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

  addBookToCatalog(catalogId: number, catalogName: string, bookId: number) {
    const model = { catalogId, bookId, catalogName };

    return this.http
      .post(this.baseUrlBookService + "add-to-catalog", model)
      .pipe(
        map((response: any) => {
          this.store.dispatch(
            new CatalogActions.AddCatalogAction(<CatalogCreateDto>response)
          );

          if (catalogId == null) {
            console.log("catalog id =  null");
            // this.store.dispatch(
            //   new UserActions.SetCurrentUserCatalogsAction(<CatalogPureDto[]>(
            //     response
            //   ))
            // );
          }
          return response;
        })
      );
  }
}
