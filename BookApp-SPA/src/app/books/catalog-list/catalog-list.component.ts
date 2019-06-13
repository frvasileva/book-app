import { Component, OnInit } from "@angular/core";
import { CatalogItemDto } from "src/app/_models/catalogItem";
import { Store } from "@ngrx/store";
import { Observable } from "rxjs";
import { BookCatalogService } from "src/app/_services/book-catalog.service";

@Component({
  selector: "app-catalog-list",
  templateUrl: "./catalog-list.component.html",
  styleUrls: ["./catalog-list.component.scss"]
})
export class CatalogListComponent implements OnInit {
  catalogsState: Observable<{ catalogs: CatalogItemDto[] }>;
  catalogState: any;

  constructor(
    private store: Store<{ catalogState: { catalog: CatalogItemDto[] } }>,
    private catalogService: BookCatalogService
  ) {}

  ngOnInit() {
    this.store
      .select(state => state.catalogState)
      .subscribe(catState => {
        this.catalogState = catState.catalog;

        if (this.catalogState.length === 0) {
          this.catalogService.getCatalogList();
        }
        console.log("state ", this.catalogState);
      });
  }
}
