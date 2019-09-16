import { Component, OnInit } from "@angular/core";
import { CatalogItemDto } from "src/app/_models/catalogItem";
import { Store } from "@ngrx/store";
import { Observable } from "rxjs";
import { BookCatalogService } from "src/app/_services/book-catalog.service";
import { ActivatedRoute, Params } from "@angular/router";
import { UserState } from "src/app/_store/user.reducer";

@Component({
  selector: "app-catalog-list",
  templateUrl: "./catalog-list.component.html",
  styleUrls: ["./catalog-list.component.scss"]
})
export class CatalogListComponent implements OnInit {
  catalogState: any;
  friendlyUrl: string;
  catalogNumber: number;
  isCurrentUser = false;

  constructor(
    private route: ActivatedRoute,
    private store: Store<{
      catalogState: { catalog: CatalogItemDto[] };
      userState: UserState;
    }>,
    private catalogService: BookCatalogService
  ) {}

  ngOnInit() {
    this.route.params.subscribe((params: Params) => {
      this.friendlyUrl = params["friendlyUrl"];

      if (this.friendlyUrl) {
        this.catalogService.getUserCatalogs(this.friendlyUrl);
        this.store
          .select(state => state)
          .subscribe(catState => {
            if (this.friendlyUrl !== "") {
              this.catalogState = catState.catalogState.catalog.filter(c => {
                if (c.userFriendlyUrl === catState.userState.currentUser) {
                  this.isCurrentUser = true;
                }
                return c.userFriendlyUrl === this.friendlyUrl;
              });
            } else {
              this.catalogState = catState.catalogState.catalog;
            }

            this.catalogNumber = this.catalogState.length;
          });
      }
    });
  }
}
