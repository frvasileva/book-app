import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { map } from "rxjs/operators";
import { Router } from "@angular/router";

@Injectable({
  providedIn: "root"
})
export class AuthService {
  baseUrl = "http://localhost:5000/api/auth/";

  constructor(private http: HttpClient, private router: Router) {}

  login(model: any) {
    console.log({ model });
    return this.http.post(this.baseUrl + "login", model).pipe(
      map((response: any) => {
        const user = response;

        if (user) {
          localStorage.setItem("token", user.token);
          this.router.navigate(["/user/profile"]);
        }
      })
    );
  }

  isUserLoggedIn() {
    const token = localStorage.getItem("token");
    return !!token;
  }

  logout() {
    localStorage.removeItem("token");
    this.router.navigate(["/"]);
  }

  reigster(model: any) {
    console.log({ model });
    return this.http.post(this.baseUrl + "register", model).pipe(
      map((response: any) => {
        const user = response;

        if (user) {
          localStorage.setItem("token", user.token);
          this.router.navigate(["/user/profile"]);
        }
      })
    );
  }
}
