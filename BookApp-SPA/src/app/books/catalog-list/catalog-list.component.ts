import { Component, OnInit } from "@angular/core";
import { BookCatalogService } from "../../_services/book-catalog.service";
import { UserState } from "../../_store/user.reducer";
import { ActivatedRoute, Params } from "@angular/router";
import { Store } from "@ngrx/store";
import { Router } from "@angular/router";

@Component({
  selector: "app-catalog-list",
  templateUrl: "./catalog-list.component.html",
  styleUrls: ["./catalog-list.component.scss"]
})
export class CatalogListComponent implements OnInit {
  catalogState: any;
  friendlyUrl: string;
  catalogNumber = 0;
  isCurrentUser = false;
  catalogList: any;
  path: string;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private catalogService: BookCatalogService,
    private store: Store<{
      userState: UserState;
    }>
  ) {}

  ngOnInit() {
    this.path = this.router.url;
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

      if (this.path.indexOf("profile")) {
        this.catalogService
          .getUserCatalogs(this.friendlyUrl)
          .subscribe(data => {
            this.catalogList = data;
            if (this.catalogList.length > 0) {
              this.catalogNumber = this.catalogList.length;
            }
          });
      } else {
        this.catalogService.getPublicCatalogs().subscribe(data => {
          this.catalogList = data.items;
          if (this.catalogList.length > 0) {
            this.catalogNumber = this.catalogList.length;
          }
        });
      }
    });
  }
}
