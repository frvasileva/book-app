import { Component, OnInit } from "@angular/core";
import { Store, select } from "@ngrx/store";
import { Title, Meta } from "@angular/platform-browser";
import { CatalogItemDto } from "src/app/_models/catalogItem";
import { ActivatedRoute, Params } from "@angular/router";
import { bookDetailsDto } from "src/app/_models/bookDetailsDto";
import { BookService } from "src/app/_services/book.service";
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
    private store: Store<{ catalogState: { catalog: CatalogItemDto[] } }>,
    private titleService: Title,
    private metaTagService: Meta,
    private catalogService: BookCatalogService
  ) {}

  ngOnInit() {
    this.catalogService.getCatalogList();

    this.route.params.subscribe((params: Params) => {
      this.friendlyUrl = params["friendly-url"];
      this.store
        .pipe(
          select(
            (state: { catalogState: { catalog: CatalogItemDto[] } }) =>
              state.catalogState.catalog
          )
        )
        .subscribe(catalogs => {
          this.catalog = catalogs.find(
            cat => cat.friendlyUrl === this.friendlyUrl
          );

          if (this.catalog) {
            this.titleService.setTitle(this.catalog.name);
            this.metaTagService.updateTag({
              name: "description",
              content: this.catalog.description
            });
          } else {
            this.catalogService.getCatalog(1); //TODO - put friendly url here
          }
        });
    });
  }
}