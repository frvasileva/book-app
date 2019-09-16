import { Component, OnInit, Input } from "@angular/core";
import { BookCatalogService } from "src/app/_services/book-catalog.service";
import { CatalogEditItemDto } from "src/app/_models/catalogEditItemDto";

@Component({
  selector: "app-catalog-item",
  templateUrl: "./catalog-item.component.html",
  styleUrls: ["./catalog-item.component.scss"]
})
export class CatalogItemComponent implements OnInit {
  @Input() catalog: any;
  @Input() isCurrentUser: boolean;
  bookCount: number;
  maxBooksToBeShown = 5;

  constructor(private catalogService: BookCatalogService) {}

  ngOnInit() {
    this.bookCount = this.catalog.books.length;
    console.log("is current user", this.isCurrentUser);
  }

  editCatalog(catalogId: number, isPublic: boolean) {
    const model = {} as CatalogEditItemDto;
    model.id = catalogId;
    model.isPublic = !isPublic;
    this.catalogService.editCatalog(model).subscribe();
    this.catalog.isPublic = !isPublic;
    console.log("item", model);
  }
}
