import { Component, OnInit, Input } from "@angular/core";
import { BookCatalogService } from "../../_services/book-catalog.service";
import { CatalogEditItemDto } from "../../_models/catalogEditItemDto";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { settings } from "../../_shared/settings";

@Component({
  selector: "app-catalog-item",
  templateUrl: "./catalog-item.component.html",
  styleUrls: ["./catalog-item.component.scss"]
})
export class CatalogItemComponent implements OnInit {
  @Input() catalog: any;
  @Input() isCurrentUser: boolean;
  bookCount: number;
  maxBooksToBeShown = 5;
  model: CatalogEditItemDto;
  editCatalogNameForm: FormGroup;
  showCatalogName = false;
  isWantToReadCategory = false;

  constructor(private catalogService: BookCatalogService) {}

  ngOnInit() {
    this.bookCount = this.catalog.books.length;
    this.model = this.catalog;

    this.editCatalogNameForm = new FormGroup({
      name: new FormControl(this.catalog.name, Validators.required)
    });

    this.isWantToReadCategory =
      settings.defaultCatalogNames.filter(x => x === this.catalog.name).length >
      0;
  }

  editCatalog(isPublic: boolean) {
    this.model.isPublic = !isPublic;
    this.catalog.isPublic = !isPublic;

    this.catalogService.editCatalog(this.model).subscribe();
  }

  showChangeCatalogNameForm() {
    this.showCatalogName = true;
  }

  changeCatalogName() {
    const name: string = this.editCatalogNameForm.value.name.toString();
    this.model.name = name;
    this.catalogService.editCatalog(this.model).subscribe();
    this.showCatalogName = false;
  }
}
