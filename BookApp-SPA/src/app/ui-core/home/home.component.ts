import { Component, OnInit } from "@angular/core";
import { Title, Meta } from "@angular/platform-browser";
import { settings } from "src/app/_shared/settings";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
  styleUrls: ["./home.component.scss"]
})
export class HomeComponent implements OnInit {
  constructor(private titleService: Title, private metaTagService: Meta) {}

  ngOnInit() {
    this.titleService.setTitle("📖 " + settings.seo_appName_title);
    this.metaTagService.updateTag({
      name: "description",
      content: settings.seo_appName_title
    });
  }
}
