import { Component, OnInit, Input } from "@angular/core";

@Component({
  selector: "app-toggle-button",
  templateUrl: "./toggle-button.component.html",
  styleUrls: ["./toggle-button.component.scss"]
})
export class ToggleButtonComponent implements OnInit {
  @Input() isChecked: boolean;
  checkBoxId: string;
  constructor() {}

  ngOnInit() {
    console.log("ischekced", this.isChecked);
    this.checkBoxId = this.randomIntFromInterval(0, 10).toString();
  }

  randomIntFromInterval(min, max) {
    // min and max included
    return Math.floor(Math.random() * (max - min + 1) + min);
  }
}
