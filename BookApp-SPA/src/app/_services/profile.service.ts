import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { JwtHelperService } from "@auth0/angular-jwt";
import { Router } from "@angular/router";

@Injectable({
  providedIn: "root"
})
export class ProfileService {
  baseUrl = "http://localhost:5000/api/profile/";
  jwtHelper = new JwtHelperService();

  constructor(private http: HttpClient, private router: Router) {}

  getUserProfile(userId: string) {
    return this.http.get(this.baseUrl + "get/1");
  }
}
