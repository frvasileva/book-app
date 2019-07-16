import { Component, OnInit } from "@angular/core";
import { CatalogItemDto } from "src/app/_models/catalogItem";
import { Store } from "@ngrx/store";
import { Observable } from "rxjs";
import { BookCatalogService } from "src/app/_services/book-catalog.service";
import { ActivatedRoute, Params } from "@angular/router";

@Component({
  selector: "app-catalog-list",
  templateUrl: "./catalog-list.component.html",
  styleUrls: ["./catalog-list.component.scss"]
})
export class CatalogListComponent implements OnInit {
  catalogState: any;
  friendlyUrl: string;

  constructor(
    private route: ActivatedRoute,
    private store: Store<{ catalogState: { catalog: CatalogItemDto[] } }>,
    private catalogService: BookCatalogService
  ) {}

  ngOnInit() {
    this.route.params.subscribe((params: Params) => {
      this.friendlyUrl = params["friendlyUrl"];

      console.log("catalog friendlyUrl", this.friendlyUrl);
      if (this.friendlyUrl) {
        this.catalogService.getUserCatalogs(this.friendlyUrl);
        this.store
          .select(state => state.catalogState)
          .subscribe(catState => {
            this.catalogState = catState.catalog.filter(c => {
              return c.userFriendlyUrl === this.friendlyUrl;
            });

            // if (this.catalogState.length === 0) {
            //   this.catalogService.getUserCatalogs(this.friendlyUrl);
            // }
          });
      }
    });
  }
}
