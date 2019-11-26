import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { JwtHelperService } from "@auth0/angular-jwt";
import { Router } from "@angular/router";
import { AlertifyService } from "./alertify.service";
import { HttpClient } from "@angular/common/http";
import { map } from "rxjs/internal/operators/map";
import { DiscussionDto } from "../_models/discussionDto";

@Injectable({
  providedIn: "root"
})
export class DiscussionService {
  baseUrl = environment.apiUrl + "discussion/";
  jwtHelper = new JwtHelperService();
  token = this.jwtHelper.decodeToken(localStorage.getItem("token"));

  constructor(
    private http: HttpClient,
    private router: Router,
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
