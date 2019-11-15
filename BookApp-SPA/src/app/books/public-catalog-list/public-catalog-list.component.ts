import { Component, OnInit } from "@angular/core";
import { BookCatalogService } from "src/app/_services/book-catalog.service";

@Component({
  selector: "app-public-catalog-list",
  templateUrl: "./public-catalog-list.component.html",
  styleUrls: ["./public-catalog-list.component.scss"]
})
export class PublicCatalogListComponent implements OnInit {
  catalogList: any;

  constructor(private catalogService: BookCatalogService) {}

  ngOnInit() {
    this.catalogService.getPublicCatalogs().subscribe(data => {
      this.catalogList = data;
    });
  }
}
