import { Component, OnInit } from "@angular/core";
import { Title, Meta } from "@angular/platform-browser";
import { settings } from "src/app/_shared/settings";
import { AuthService } from "src/app/_services/auth.service";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
  styleUrls: ["./home.component.scss"]
})
export class HomeComponent implements OnInit {
  isAuthenticated: boolean;

  constructor(
    private titleService: Title,
    private metaTagService: Meta,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.isAuthenticated = this.authService.isAuthenticated();

    this.titleService.setTitle("📖 " + settings.seo_appName_title);
    this.metaTagService.updateTag({
      name: "description",
      content: settings.seo_appName_title
    });
  }
}
