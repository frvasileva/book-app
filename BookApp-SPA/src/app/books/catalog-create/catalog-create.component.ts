import { Component, OnInit } from "@angular/core";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { BookCatalogService } from "src/app/_services/book-catalog.service";
import { CatalogCreateDto } from "src/app/_models/catalogCreateDto";
import { AlertifyService } from "src/app/_services/alertify.service";

@Component({
  selector: "app-catalog-create",
  templateUrl: "./catalog-create.component.html",
  styleUrls: ["./catalog-create.component.scss"]
})
export class CatalogCreateComponent implements OnInit {
  addCatalogForm: FormGroup;

  constructor(
    private catalogService: BookCatalogService,
    private alertify: AlertifyService
  ) {}

  ngOnInit() {
    this.addCatalogForm = new FormGroup({
      name: new FormControl(null, Validators.required),
      visibility: new FormControl(false, [])
    });
  }

  onSubmit() {
    const name: string = this.addCatalogForm.value.name.toString();
    const isPublic: boolean = <boolean>this.addCatalogForm.value.visibility;
    const catalogItem: CatalogCreateDto = { name, isPublic };

    this.catalogService.addCatalog(catalogItem);

    // this.addBookModel.title = this.addBookForm.value.bookData.title;
    // this.addBookModel.description = this.addBookForm.value.bookData.description;
    // this.addBookModel.authorName = this.addBookForm.value.author;

    this.catalogService.addCatalog(catalogItem).subscribe(
      next => {
        this.alertify.success("Catalog created!");
      },
      error => {
        this.alertify.error("Failed to create catalog");
      }
    );
  }
}
