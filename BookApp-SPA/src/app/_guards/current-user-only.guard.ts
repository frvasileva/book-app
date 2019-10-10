import { Injectable } from "@angular/core";
import {
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  UrlTree,
  Router,
  ActivatedRoute,
  Params
} from "@angular/router";
import { Observable } from "rxjs";
import { Store } from "@ngrx/store";
import { Profile } from "../_models/profile";
import { JwtHelperService } from "@auth0/angular-jwt";
import { User } from '../_models/user';

@Injectable({
  providedIn: "root"
})
export class CurrentUserOnlyGuard implements CanActivate {
  currentUserProfile: Profile;
  jwtHelper = new JwtHelperService();
  token: any;

  constructor(
    private store: Store<{ userState: User }>,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    this.store
      .select(state => state.userState)
      .subscribe(res => {
        this.currentUserProfile = res as Profile;
      });

    this.token = localStorage.getItem("token");

    const friendlyUrl = this.jwtHelper.decodeToken(this.token).unique_name;
    const currentFriendlyUrl = state.url.split("/")[4];
    console.log(currentFriendlyUrl, friendlyUrl);

    if (this.token !== null && currentFriendlyUrl === friendlyUrl) {
      console.log("should activate");
      return true;
    }

    this.router.navigate(["/user/login"]);
    return false;
  }
}
