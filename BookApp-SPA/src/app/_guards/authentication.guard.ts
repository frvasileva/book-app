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
import { User } from "../_models/user";
import { AuthService } from "../_services/auth.service";

@Injectable({
  providedIn: "root"
})
export class AuthenticationGuard implements CanActivate {
  currentUserProfile: Profile;

  constructor(
    private authService: AuthService,
    private store: Store<{ userState: User }>,
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
      .select(state => state.userState)
      .subscribe(res => {
        this.currentUserProfile = res as Profile;
      });

    if (!this.authService.getDecodedToken()) {
      this.router.navigate(["/user/login"]);
      return false;
    }
    return true;
  }
}
