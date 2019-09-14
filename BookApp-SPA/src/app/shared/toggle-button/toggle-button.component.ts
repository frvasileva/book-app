import { Component, OnInit, Input } from "@angular/core";

@Component({
  selector: "app-toggle-button",
  templateUrl: "./toggle-button.component.html",
  styleUrls: ["./toggle-button.component.scss"]
})
export class ToggleButtonComponent implements OnInit {
  @Input() isChecked: boolean;

  constructor() {}

  ngOnInit() {}
}
