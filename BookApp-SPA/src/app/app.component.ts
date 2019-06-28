import { Component, OnInit } from "@angular/core";
import { AuthService } from "./_services/auth.service";
import {
  Router,
  Event,
  NavigationStart,
  NavigationEnd,
  NavigationError
} from "@angular/router";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.scss"]
})
export class AppComponent implements OnInit {
  title = "Book Application";

  moduleClass: string;
  currentUrl: string;
  currentModule: string;
  currentComponentPath: string;

  constructor(private authService: AuthService, private router: Router) {
    this.router.events.subscribe((event: Event) => {
      if (event instanceof NavigationStart) {
        this.moduleClass = "helllooooooo";
        // Show loading indicator
      }

      if (event instanceof NavigationEnd) {
        console.warn("loading navigator end", this.router);
        this.currentUrl = this.router.url;
        this.currentModule = this.currentUrl.split("/")[1];
        this.currentComponentPath = this.currentUrl.split("/")[2];
        if (
          this.currentModule === "user" &&
          (this.currentComponentPath === "login" ||
            this.currentComponentPath === "sign-up")
        ) {
          this.moduleClass = this.currentModule + "-module-wrapper";
        }
      }

      if (event instanceof NavigationError) {
        // Hide loading indicator

        // Present error to user
        console.warn(event.error);
      }
    });
  }

  ngOnInit(): void {
    this.authService.getCurrentUser();
  }
}
