import { Component, OnInit, Input } from "@angular/core";

@Component({
  selector: "app-catalog-list-pure",
  templateUrl: "./catalog-list-pure.component.html",
  styleUrls: ["./catalog-list-pure.component.scss"]
})
export class CatalogListPureComponent implements OnInit {
  @Input() bookCatalogs: any;
  constructor() {}

  ngOnInit() {}
}
