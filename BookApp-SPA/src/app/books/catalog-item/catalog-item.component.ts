import { Component, OnInit, Input } from "@angular/core";
import { CatalogItemDto } from "src/app/_models/catalogItem";

@Component({
  selector: "app-catalog-item",
  templateUrl: "./catalog-item.component.html",
  styleUrls: ["./catalog-item.component.scss"]
})
export class CatalogItemComponent implements OnInit {
  @Input() catalog: any;

  constructor() {}

  ngOnInit() {
    console.log("input: ", this.catalog);
  }
}
