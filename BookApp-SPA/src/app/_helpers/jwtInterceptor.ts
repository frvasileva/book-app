import { Injectable } from "@angular/core";
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from "@angular/common/http";
import { Observable } from "rxjs";
import { UserService } from "../_services/user.service";

// import { AuthenticationService } from '@/_services';

@Injectable()
export class JwtInterceptorHelper implements HttpInterceptor {
  //constructor(private authenticationService: AuthenticationService) { }
  constructor(private userService: UserService) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    // add authorization header with jwt token if available
    let token = localStorage.getItem("token");

    console.log("JwtInterceptorHelper");

    if (token) {
      console.log("token: ", token);
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }

    return next.handle(request);
  }
}
