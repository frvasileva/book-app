import { Component, OnInit, Input } from "@angular/core";
import { AuthService } from "src/app/_services/auth.service";

@Component({
  selector: "app-book-card-item",
  templateUrl: "./book-card-item.component.html",
  styleUrls: ["./book-card-item.component.scss"]
})
export class BookCardItemComponent implements OnInit {
  @Input() book: any;

  isUserAuthenticated: boolean;

  constructor(private authService: AuthService) {}

  ngOnInit() {
    this.isUserAuthenticated = this.authService.isAuthenticated();
  }
}
