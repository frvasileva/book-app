import { Injectable } from "@angular/core";
import { JwtHelperService } from "@auth0/angular-jwt";
import { Store } from "@ngrx/store";

import { map } from "rxjs/operators";

import { CatalogCreateDto } from "../_models/catalogCreateDto";
import { HttpClient } from "@angular/common/http";

import * as CatalogActions from "../_store/catalog.actions";
import * as UserActions from "../_store/user.actions";

import { environment } from "src/environments/environment";
import { CatalogItemDto } from "../_models/catalogItem";
import { CatalogPureDto } from "../_models/catalogPureDto";

@Injectable({
  providedIn: "root"
})
export class BookCatalogService {
  baseUrl = environment.apiUrl + "catalog/";
  jwtHelper = new JwtHelperService();
  alertify: any;

  constructor(
    private http: HttpClient,
    private store: Store<{ catalog: { catalog: CatalogCreateDto[] } }>
  ) {}

  addCatalog(model: CatalogCreateDto) {
    return this.http.post(this.baseUrl + "add", model).pipe(
      map((response: CatalogItemDto) => {
        this.store.dispatch(
          new CatalogActions.AddCatalogAction(<CatalogCreateDto>response)
        );
        const catalogPure: CatalogPureDto = {
          id: response.id,
          created: response.created,
          isPublic: response.isPublic,
          name: response.name,
          userId: response.userId
        };
        this.store.dispatch(
          new UserActions.AddCurrentUserCatalogsAction(catalogPure)
        );

        return response;
      })
    );
  }

  getCatalog(id: number) {
    // return this.http.get(this.baseUrl + "get/" + id).subscribe(
    //   data => {
    //     this.store.dispatch(
    //       new CatalogActions.GetCatalogsAction(<CatalogCreateDto>data)
    //     );
    //     console.log("get catalog data", data);
    //   },
    //   error => {
    //     this.alertify.error(error);
    //   }
    // );
  }

  getUserCatalogs(userFriendlyUrl: string) {
    return this.http
      .get(this.baseUrl + "user-catalogs/" + userFriendlyUrl)
      .subscribe(
        data => {
          this.store.dispatch(new CatalogActions.GetCatalogsAction(data));
        },
        error => {
          this.alertify.error(error);
        }
      );
  }
}
