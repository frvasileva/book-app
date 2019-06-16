import { Component, OnInit, Input } from "@angular/core";
import { CatalogItemDto } from "src/app/_models/catalogItem";

@Component({
  selector: "app-catalog-item",
  templateUrl: "./catalog-item.component.html",
  styleUrls: ["./catalog-item.component.scss"]
})
export class CatalogItemComponent implements OnInit {
  @Input() catalog: any;

  bookCount: number;
  maxBooksToBeShown = 5;

  constructor() {}

  ngOnInit() {
    this.bookCount = this.catalog.bookCatalogs.length;
    console.log("length", this.bookCount);
    console.log("input: ", this.catalog);
  }
}
