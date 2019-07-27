import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Params } from "@angular/router";
import { Store } from "@ngrx/store";
import { BookCatalogService } from "src/app/_services/book-catalog.service";
import { CatalogItemDto } from "src/app/_models/catalogItem";

@Component({
  selector: "app-public-catalog-list",
  templateUrl: "./public-catalog-list.component.html",
  styleUrls: ["./public-catalog-list.component.scss"]
})
export class PublicCatalogListComponent implements OnInit {
  catalogState: any;

  constructor(
    private route: ActivatedRoute,
    private store: Store<{ catalogState: { catalog: CatalogItemDto[] } }>,
    private catalogService: BookCatalogService
  ) {}

  ngOnInit() {
    this.catalogService.getPublicCatalogs();
    this.store
      .select(state => state.catalogState)
      .subscribe(catState => {
        this.catalogState = catState.catalog;
      });
  }
}
