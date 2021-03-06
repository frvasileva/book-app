import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { map } from "rxjs/operators";

import { BookCreateDto } from "../_models/bookCreateDto";
import { environment } from "../../environments/environment";
import { AuthService } from "./auth.service";

@Injectable({
  providedIn: "root"
})
export class BookService {
  baseUrl = environment.apiUrl + "book/";

  constructor(private http: HttpClient, private authService: AuthService) {}

  addBook(model: BookCreateDto) {
    model.userId = this.authService.getDecodedToken().nameid;
    return this.http.post(this.baseUrl + "add", model).pipe(
      map((response: any) => {
        return response;
      })
    );
  }

  getBook(friendlyUrl: string = "") {
    return this.http.get(this.baseUrl + "get/" + friendlyUrl);
  }

  getBooksAddedByUser(userFriendlyUrl: string) {
    return this.http.get(
      this.baseUrl + "get-books-added-by-user/" + userFriendlyUrl
    );
  }

  getBooksByCategory(category: string, currentPage: number) {
    switch (category.toLowerCase()) {
      case "relevance": {
        return this.RecommendByRelevance(currentPage);
      }
      case "serendipity": {
        return this.RecommendBySerendipity(currentPage);
      }
      case "novelty": {
        return this.RecommendByNovelty(currentPage);
      }
    }
  }

  RecommendByRelevance(currentPage: number) {
    return this.http.get(
      this.baseUrl + "recommend-relevance/" + currentPage
    ) as any;
  }
  RecommendBySerendipity(currentPage: number) {
    return this.http.get(
      this.baseUrl + "recommend-serendipity/" + currentPage
    ) as any;
  }
  RecommendByNovelty(currentPage: number) {
    return this.http.get(
      this.baseUrl + "recommend-novelty/" + currentPage
    ) as any;
  }
  RecommendSimiliarBooks(friendlyUrl: string) {
    return this.http.get(
      this.baseUrl + "recommend-similiar-book/" + friendlyUrl
    ) as any;
  }
}
