import { Component, OnInit, Input } from "@angular/core";

@Component({
  selector: "app-book-card-item",
  templateUrl: "./book-card-item.component.html",
  styleUrls: ["./book-card-item.component.scss"]
})
export class BookCardItemComponent implements OnInit {
  @Input() book: any;
  constructor() {}

  ngOnInit() {
    console.log(this.book);
  }
}
