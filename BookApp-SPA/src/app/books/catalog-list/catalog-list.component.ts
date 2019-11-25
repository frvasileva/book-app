import { Component, OnInit } from "@angular/core";
import { BookCatalogService } from "src/app/_services/book-catalog.service";
import { ActivatedRoute, Params } from "@angular/router";
import { Store } from "@ngrx/store";
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
  catalogList: any;

  constructor(
    private route: ActivatedRoute,
    private catalogService: BookCatalogService,
    private store: Store<{
      userState: UserState;
    }>
  ) {}

  ngOnInit() {
    this.route.params.subscribe((params: Params) => {
      this.friendlyUrl = params["friendlyUrl"];

      if (this.friendlyUrl) {
        this.store
          .select(state => state.userState)
          .subscribe(userState => {
            if (this.friendlyUrl !== "") {
              this.isCurrentUser = userState.currentUser === this.friendlyUrl;
            }
          });
      }

      // TODO: Check if current user, if not - show only public catalogs
      this.catalogService.getPublicCatalogs().subscribe(data => {
        this.catalogList = data;
        this.catalogNumber = 1;
      });

    });
  }
}
