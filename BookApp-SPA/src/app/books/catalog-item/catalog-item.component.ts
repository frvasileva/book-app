import { Component, OnInit, Input } from "@angular/core";
import { CatalogItemDto } from "src/app/_models/catalogItem";

@Component({
  selector: "app-catalog-item",
  templateUrl: "./catalog-item.component.html",
  styleUrls: ["./catalog-item.component.scss"]
})
export class CatalogItemComponent implements OnInit {
  public catalog: any;

  constructor() {}

  ngOnInit() {
    console.log(this.catalog);
    // this.catalog.id = 1;
    this.catalog = { name: "My catalog is here" };
  }
}
