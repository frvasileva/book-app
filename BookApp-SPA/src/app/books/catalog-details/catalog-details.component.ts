import { Component, OnInit } from "@angular/core";
import { Title, Meta } from "@angular/platform-browser";
import { ActivatedRoute, Params } from "@angular/router";
import { BookCatalogService } from "src/app/_services/book-catalog.service";

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
    private titleService: Title,
    private metaTagService: Meta,
    private catalogService: BookCatalogService
  ) {}

  ngOnInit() {
    this.route.params.subscribe((params: Params) => {
      this.friendlyUrl = params["friendly-url"];
      this.catalogService.getCatalog(this.friendlyUrl).subscribe(data => {
        this.catalog = data[0];
      });
    });
  }
}
