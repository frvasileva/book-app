import { Component, OnInit } from "@angular/core";
import { Store, select } from "@ngrx/store";
import { Title, Meta } from "@angular/platform-browser";
import { CatalogItemDto } from "src/app/_models/catalogItem";
import { ActivatedRoute, Params } from "@angular/router";
import { BookCatalogService } from "src/app/_services/book-catalog.service";
import { UserState } from "src/app/_store/user.reducer";

@Component({
  selector: "app-catalog-details",
  templateUrl: "./catalog-details.component.html",
  styleUrls: ["./catalog-details.component.scss"]
})
export class CatalogDetailsComponent implements OnInit {
  friendlyUrl: string;
  catalog: any;
  userAvatarPath: string;
  user: any;

  constructor(
    private route: ActivatedRoute,
    private store: Store<{
      catalogState: { catalog: CatalogItemDto[] };
      userState: UserState;
    }>,
    private titleService: Title,
    private metaTagService: Meta,
    private catalogService: BookCatalogService
  ) {}

  ngOnInit() {
    this.route.params.subscribe((params: Params) => {
      this.friendlyUrl = params["friendly-url"];
      this.store
        // .pipe(
        //   select(
        //     (state: {
        //       catalogState: { catalog: CatalogItemDto[] };
        //       userState: UserState;
        //     }) => state.catalogState.catalog
        //   )
        // )
        .subscribe(state => {
          this.catalog = state.catalogState.catalog.find(
            cat => cat.friendlyUrl === this.friendlyUrl
          );
          if (this.catalog) {
            console.log("hello", this.catalog);
            this.user = state.userState.users.find(
              item => item.friendlyUrl === this.catalog.userFriendlyUrl
            );
          }
          if (this.catalog) {
            this.titleService.setTitle(this.catalog.name);
            this.metaTagService.updateTag({
              name: "description",
              content: this.catalog.description
            });
          } else {
            this.catalogService.getCatalog(this.friendlyUrl); //TODO - put friendly url here
          }
        });
    });
  }
}
