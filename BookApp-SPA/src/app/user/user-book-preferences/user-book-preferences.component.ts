import { Component, OnInit } from "@angular/core";
import { UserService } from "src/app/_services/user.service";
import { UserBookCategoryPreferences } from "src/app/_models/userBookCategoryPreferences";

@Component({
  selector: "app-user-book-preferences",
  templateUrl: "./user-book-preferences.component.html",
  styleUrls: ["./user-book-preferences.component.scss"]
})
export class UserBookPreferencesComponent implements OnInit {
  bookCategoriesList: any = [
    "Fiction",
    "History",
    "Science",
    "Business",
    "Self help",
    "Mystery",
    "Literature",
    "Teens",
    "Romance",
    "Computers",
    "Fantasy",
    "Cookbooks"
  ];

  bookCategories: UserBookCategoryPreferences[];

  selectedCategories = new Array();
  enableSubmitting: boolean;
  constructor(private userService: UserService) {}

  ngOnInit() {
    this.userService.getUserBookCategoryPreferences().subscribe(data => {
      this.bookCategories = data as UserBookCategoryPreferences[];
      console.log("from server ", this.bookCategories);
    });
  }

  selectPreferences(item: UserBookCategoryPreferences) {
    this.selectedCategories.indexOf(item.name) === -1
      ? this.selectedCategories.push(item.name)
      : console.log("This item already exists");

    if (this.selectedCategories.length >= 5) {
      this.enableSubmitting = true;
      this.userService.setUserBookCategoryPreferences(this.selectedCategories);
    }
  }
}
