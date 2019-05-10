import { Injectable } from "@angular/core";
import { JwtHelperService } from "@auth0/angular-jwt";
import { HttpClient } from "@angular/common/http";
import { Router } from "@angular/router";

@Injectable()
export class UserService {
  baseUrl = "http://localhost:5000/api/profile/";
  jwtHelper = new JwtHelperService();
  constructor(private http: HttpClient, private router: Router) {}

  getUserProfile(userId: string) {
    console.log("get user profile");
    return this.http.get(this.baseUrl + "get/" + userId);
  }

  followUser(userIdToFollow: number, currentUserId: number) {}
}
