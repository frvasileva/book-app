import { Injectable } from "@angular/core";
import { Title, Meta } from "@angular/platform-browser";
import { settings } from "./settings";

@Injectable({
  providedIn: "root"
})
export class SeoHelperService {
  constructor(private titleService: Title, private metaTagService: Meta) {}

  setSeoMetaTags(title: string = "", content: string = "") {
    this.titleService.setTitle(title + settings.seo_appName_title);
    if (content === "") {
      this.metaTagService.updateTag({
        name: "description",
        content: title + "|" + settings.seo_appName_title
      });
    } else {
      this.metaTagService.updateTag({
        name: "description",
        content: content + "|" + settings.seo_appName_title
      });
    }
  }
}
