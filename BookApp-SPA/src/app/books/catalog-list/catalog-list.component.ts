import { Component, OnInit } from "@angular/core";
import { BookCatalogService } from "src/app/_services/book-catalog.service";
import { ActivatedRoute, Params } from "@angular/router";

@Component({
  selector: "app-catalog-list",
  templateUrl: "./catalog-list.component.html",
  styleUrls: ["./catalog-list.component.scss"]
})
export class CatalogListComponent implements OnInit {
  catalogList: any;

  constructor(
    private route: ActivatedRoute,
    private catalogService: BookCatalogService
  ) {}

  ngOnInit() {
    this.route.params.subscribe((params: Params) => {});
    this.catalogService.getPublicCatalogs().subscribe(data => {
      this.catalogList = data;
    });
  }
}
