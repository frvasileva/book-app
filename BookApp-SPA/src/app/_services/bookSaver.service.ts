import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { HttpClient } from "@angular/common/http";
import { Router } from "@angular/router";
import { Store } from "@ngrx/store";
import { Book } from "../_models/books";
import { AlertifyService } from "./alertify.service";

@Injectable()
export class BookSaverService {
  baseUrl = environment.apiUrl + "catalog/";

  private bookSaverItemList = [
    {
      id: 1,
      label: "Favorites books"
    },
    {
      id: 2,
      label: "Want to read"
    },
    {
      id: 3,
      label: "Want to share"
    }
  ];
  constructor(
    private http: HttpClient,
    private router: Router,
    private store: Store<{ bookState: { books: Book[] } }>,
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
    return this.http.get(this.baseUrl + "user-catalogs-pure-list/" + friendlyUrl);
  }
}
