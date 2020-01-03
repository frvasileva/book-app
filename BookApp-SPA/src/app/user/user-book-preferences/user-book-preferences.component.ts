import { Component, OnInit } from "@angular/core";
import { UserBookCategoryPreferences } from "../../_models/userBookCategoryPreferences";
import { AuthService } from "../../_services/auth.service";

@Component({
  selector: "app-user-book-preferences",
  templateUrl: "./user-book-preferences.component.html",
  styleUrls: ["./user-book-preferences.component.scss"]
})
export class UserBookPreferencesComponent implements OnInit {
  bookCategoriesTemp: UserBookCategoryPreferences[];
  bookCategories: UserBookCategoryPreferences[] = [];

  selectedCategories = new Array();
  enableSubmitting: boolean;
  isMoreThan5AreSelected: boolean;

  constructor(private authService: AuthService) {}

  ngOnInit() {
    this.authService.getUserBookCategoryPreferences().subscribe(data => {
      this.bookCategoriesTemp = data as UserBookCategoryPreferences[];

      this.bookCategoriesTemp.forEach(element => {
        this.bookCategories.push({
          id: element.id,
          isSelected: false,
          iconPath: element.iconPath,
          name: element.name
        });
      });
    });
  }

  selectPreferences(item: UserBookCategoryPreferences) {
    if (item.isSelected) {
      this.selectedCategories = this.selectedCategories.filter(
        i => i.id !== item.id
      );
    } else {
      this.selectedCategories.push(item);
    }

    item.isSelected = !item.isSelected;
    this.enableSubmitting = this.selectedCategories.length === 5;
    this.isMoreThan5AreSelected = this.selectedCategories.length > 5;
  }

  sendCategoriesPreferences() {
    this.authService
      .setUserBookCategoryPreferences(
        this.selectedCategories.map(item => item.name)
      )
      .subscribe();
  }
}
