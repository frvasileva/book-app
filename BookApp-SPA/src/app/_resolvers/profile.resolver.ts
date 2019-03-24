import { Injectable } from "@angular/core";
import { Resolve, Router, ActivatedRouteSnapshot } from "@angular/router";
import { AlertifyService } from "../_services/alertify.service";
import { Observable, of } from "rxjs";
import { catchError } from "rxjs/operators";
import { ProfileService } from "../_services/profile.service";
import { Profile } from "../_models/profile";

@Injectable()
export class ProfileResolver implements Resolve<Profile> {
  resolve(
    route: ActivatedRouteSnapshot,
    state: import("@angular/router").RouterStateSnapshot
  ): Profile | Observable<Profile> | Promise<Profile> {
    throw new Error("Method not implemented.");
  }
  constructor(
    private profileService: ProfileService,
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
