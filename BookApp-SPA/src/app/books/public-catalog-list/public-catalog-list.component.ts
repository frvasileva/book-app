import { Component, OnInit } from "@angular/core";
import { BookCatalogService } from "src/app/_services/book-catalog.service";
import { Router, ActivatedRoute, Params } from "@angular/router";

@Component({
  selector: "app-public-catalog-list",
  templateUrl: "./public-catalog-list.component.html",
  styleUrls: ["./public-catalog-list.component.scss"]
})
export class PublicCatalogListComponent implements OnInit {
  catalogList: any;
  totalItems: 0;
  currentGridPage: 0;
  currentPage: 0;

  constructor(
    private catalogService: BookCatalogService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.route.params.subscribe((params: Params) => {
      this.currentPage = params.pageNumber;

      this.catalogService.getPublicCatalogs(this.currentPage).subscribe(data => {
        this.catalogList = data.items;
        this.totalItems = data.totalNumber;
      });
    });
  }

  pageChanged(event: any): void {
    this.currentGridPage = event.page;
    this.router.navigate(["/books/catalogs/", this.currentGridPage]);
  }
}
