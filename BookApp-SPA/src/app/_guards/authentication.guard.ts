import { Injectable } from "@angular/core";
import {
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  UrlTree,
  Router
} from "@angular/router";
import { Observable } from "rxjs";
import { Store } from "@ngrx/store";
import { Profile } from "../_models/profile";

@Injectable({
  providedIn: "root"
})
export class AuthenticationGuard implements CanActivate {
  currentUserProfile: Profile;

  constructor(
    private store: Store<{ userProfile: Profile }>,
    private router: Router
  ) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    this.store
      .select(state => state.userProfile)
      .subscribe(res => {
        this.currentUserProfile = res as Profile;
      });

    console.log("GUARD CALLED");

    if (localStorage.getItem("token") === null) {
      this.router.navigate(["/user/login"]);
      return false;
    }
    return true;
  }
}
