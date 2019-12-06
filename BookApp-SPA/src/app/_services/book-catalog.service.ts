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

import { AlertifyService } from "./alertify.service";
import { CatalogEditItemDto } from "../_models/catalogEditItemDto";

@Injectable({
  providedIn: "root"
})
export class BookCatalogService {
  baseUrl = environment.apiUrl + "catalog/";
  jwtHelper = new JwtHelperService();

  constructor(
    private http: HttpClient,
    private store: Store<{ catalog: { catalog: CatalogCreateDto[] } }>,
    private alertify: AlertifyService
  ) {}

  addCatalog(model: CatalogCreateDto) {
    return this.http.post(this.baseUrl + "add", model).pipe(
      map((response: CatalogItemDto) => {
        const catalogPure: CatalogPureDto = {
          id: response.id,
          created: response.created,
          isPublic: response.isPublic,
          name: response.name,
          userId: response.userId,
          isSelected: false
        };
        this.store.dispatch(
          new UserActions.AddCurrentUserCatalogsAction(catalogPure)
        );
        return response;
      })
    );
  }

  getCatalog(friendlyUrl: string) {
    return this.http.get(this.baseUrl + "get/" + friendlyUrl);
  }

  getUserCatalogs(userFriendlyUrl: string) {
    return this.http.get(this.baseUrl + "user-catalogs/" + userFriendlyUrl);
  }

  getPublicCatalogs(pageNumber: number = 0) {
    return this.http.get(this.baseUrl + "public-catalogs/" + pageNumber) as any;
  }

  editCatalog(model: CatalogEditItemDto) {
    return this.http.post(this.baseUrl + "edit-catalog", model).pipe(
      map((response: CatalogEditItemDto) => {
        this.store.dispatch(
          //TODO: Update user's pure catalogs if name changed
          new CatalogActions.UpdateCatalogAction(<CatalogEditItemDto>response)
        );
        return response;
      })
    );
  }
}
