import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Params, Router } from "@angular/router";
import { BookCatalogService } from "src/app/_services/book-catalog.service";
import { SeoHelperService } from "src/app/_shared/seo-helper.service";

@Component({
  selector: "app-catalog-details",
  templateUrl: "./catalog-details.component.html",
  styleUrls: ["./catalog-details.component.scss"]
})
export class CatalogDetailsComponent implements OnInit {
  friendlyUrl: string;
  pageNumber: number;
  catalog: any;
  user: any;
  currentPage: number;
  totalItems: number;
  currentGridPage: number;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private seoService: SeoHelperService,
    private catalogService: BookCatalogService
  ) {}

  ngOnInit() {
    console.log("faaani");

    this.route.params.subscribe((params: Params) => {
      this.friendlyUrl = params["friendly-url"];
      this.pageNumber = params["pageNumber"];

      this.catalogService
        .getCatalog(this.friendlyUrl, this.pageNumber)
        .subscribe(data => {
          this.catalog = data.items[0];
          this.totalItems = data.totalNumber;

          this.seoService.setSeoMetaTags(this.catalog.name);
        });
    });
  }

  pageChanged(event: any): void {
    this.currentGridPage = event.page;
    this.router.navigate([
      "/books/catalog/details/" + this.friendlyUrl + "/",
      this.currentGridPage
    ]);
  }
}
