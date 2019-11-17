import { Component, OnInit } from "@angular/core";
import { AuthService } from "src/app/_services/auth.service";
import { SeoHelperService } from "src/app/_shared/seo-helper.service";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
  styleUrls: ["./home.component.scss"]
})
export class HomeComponent implements OnInit {
  isAuthenticated: boolean;

  constructor(
    private authService: AuthService,
    private seoService: SeoHelperService
  ) {}

  ngOnInit() {
    this.isAuthenticated = this.authService.isAuthenticated();
    this.seoService.setSeoMetaTags("📖");
  }
}
