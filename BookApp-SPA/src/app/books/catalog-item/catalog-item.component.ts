import { Component, OnInit, Input } from "@angular/core";

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
    this.bookCount = this.catalog.books.length;
  }
}
