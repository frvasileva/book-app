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
    console.log(
      this.http.get(
        this.baseUrl +
          "get/" +
          this.jwtHelper.decodeToken(localStorage.getItem("token"))
      )
    );

    const token = this.jwtHelper.decodeToken(localStorage.getItem("token"));
    return this.http.get(this.baseUrl + "get/" + token.nameid);
    
  }
}
