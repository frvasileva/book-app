import { Injectable } from "@angular/core";
import { Resolve, Router, ActivatedRouteSnapshot } from "@angular/router";
import { AlertifyService } from "../_services/alertify.service";
import { Observable, of } from "rxjs";
import { catchError } from "rxjs/operators";
import { Profile } from "../_models/profile";
import { UserService } from "../_services/user.service";
import { User } from "../_models/user";

@Injectable()
export class ProfileResolver implements Resolve<User> {
  resolve(
    route: ActivatedRouteSnapshot,
    state: import("@angular/router").RouterStateSnapshot
  ): Profile | Observable<Profile> | Promise<Profile> {
    throw new Error("Method not implemented.");
  }
  constructor(
    private userService: UserService,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  // resolve(route: ActivatedRouteSnapshot): Observable<Profile> {
  // //   return this.profileService.getUserProfile("1").pipe(
  // //     catchError(error => {
  // //       this.alertify.error("Problem retrieving data");
  // // //      this.router.navigate(["/user/profile"]);
  // //       return of(null);
  // //     })
  // //   );
  // }
}
