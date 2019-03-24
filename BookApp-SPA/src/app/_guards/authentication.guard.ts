import { Injectable } from "@angular/core";
import {
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  UrlTree
} from "@angular/router";
import { Observable } from "rxjs";
import { Store } from "@ngrx/store";
import { Profile } from "../_models/profile";

@Injectable({
  providedIn: "root"
})
export class AuthenticationGuard implements CanActivate {
  profile: Profile;

  constructor(private store: Store<{ userProfile: Profile }>) {}

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
        console.log("респонсе",res);
        this.profile = res as Profile;
      });

    console.log("Auth guard is here", this.profile);
    console.log("type: ", Object.keys(this.profile).length);
    console.log("type: ", Object.keys(this.profile).length !== 0);

    // return this.profile.id !== undefined;
    return Object.keys(this.profile).length !== 0;
  }
}
