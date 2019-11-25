import { Component, OnInit } from "@angular/core";
import { BookCatalogService } from "src/app/_services/book-catalog.service";
import { UserState } from "src/app/_store/user.reducer";
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
  catalogNumber: number;
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
            this.catalogNumber = 1;
          });
      } else {
        this.catalogService.getPublicCatalogs().subscribe(data => {
          console.log(data);
          this.catalogList = data.items;
          this.catalogNumber = 1;
        });
      }
    });
  }
}
