import { Component, OnInit } from "@angular/core";
import { UserService } from "src/app/_services/user.service";
import { UserBookCategoryPreferences } from "src/app/_models/userBookCategoryPreferences";
import { AuthService } from 'src/app/_services/auth.service';

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
  constructor(private authService: AuthService) {}

  ngOnInit() {
    this.authService.getUserBookCategoryPreferences().subscribe(data => {
      this.bookCategoriesTemp = data as UserBookCategoryPreferences[];
      console.log("from server ", this.bookCategoriesTemp);

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
      this.selectedCategories.push(item.id);
    }

    item.isSelected = !item.isSelected;

    if (this.selectedCategories.length === 5) {
      this.enableSubmitting = true;
    }
  }

  sendCategoriesPreferences() {
    this.authService.setUserBookCategoryPreferences(this.selectedCategories).subscribe();
  }
}
