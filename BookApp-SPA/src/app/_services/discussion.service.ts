import { Injectable } from "@angular/core";
import { environment } from "../../environments/environment";
import { AlertifyService } from "./alertify.service";
import { HttpClient } from "@angular/common/http";
import { map } from "rxjs/internal/operators/map";

@Injectable({
  providedIn: "root"
})
export class DiscussionService {
  baseUrl = environment.apiUrl + "discussion/";

  constructor(
    private http: HttpClient,
    private alertify: AlertifyService
  ) {}

  createDiscussion(model: any) {
    return this.http.post(this.baseUrl + "create-discussion", model).pipe(
      map((response: any) => {
        return response;
      })
    );
  }

  createDiscussionReply(model: any) {
    return this.http.post(this.baseUrl + "add-discussion-item", model).pipe(
      map((response: any) => {
        return response;
      })
    );
  }

  getDiscussion(friendlyUrl: string) {
    return this.http.get(this.baseUrl + "get/" + friendlyUrl);
  }

  getDiscussions() {
    return this.http.get(this.baseUrl + "get-discussions");
  }

  getDiscussionByBook(bookId: number) {
    return this.http.get(this.baseUrl + "get-by-book/" + bookId);
  }

  getDiscussionsByUser(userId: number) {
    return this.http.get(this.baseUrl + "get-by-user" + userId).subscribe(
      data => {
        return data;
      },
      error => {
        this.alertify.error(error);
      }
    );
  }
}
