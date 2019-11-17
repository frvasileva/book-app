import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Params } from "@angular/router";
import { BookCatalogService } from "src/app/_services/book-catalog.service";
import { SeoHelperService } from "src/app/_shared/seo-helper.service";

@Component({
  selector: "app-catalog-details",
  templateUrl: "./catalog-details.component.html",
  styleUrls: ["./catalog-details.component.scss"]
})
export class CatalogDetailsComponent implements OnInit {
  friendlyUrl: string;
  catalog: any;

  constructor(
    private route: ActivatedRoute,
    private seoService: SeoHelperService,
    private catalogService: BookCatalogService
  ) {}

  ngOnInit() {
    this.route.params.subscribe((params: Params) => {
      this.friendlyUrl = params["friendly-url"];
      this.catalogService.getCatalog(this.friendlyUrl).subscribe(data => {
        this.catalog = data[0];
        this.seoService.setSeoMetaTags(this.catalog.name);
      });
    });
  }
}
