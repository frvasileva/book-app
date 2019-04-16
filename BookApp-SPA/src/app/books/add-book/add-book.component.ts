import { Component, OnInit } from "@angular/core";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { JwtHelperService } from "@auth0/angular-jwt";
import { HttpEvent, HttpEventType, HttpResponse, HttpClient} from "@angular/common/http";
import { tap, filter, map } from "rxjs/operators";
import { pipe } from "rxjs";

import { BookCreateDto } from "src/app/_models/bookCreateDto";
import { BookService } from "src/app/_services/book.service";
import { AlertifyService } from "src/app/_services/alertify.service";
import { requiredFileType } from "../file-upload/upload-file-validators";

export function uploadProgress<T>(cb: (progress: number) => void) {
  return tap((event: HttpEvent<T>) => {
    if (event.type === HttpEventType.UploadProgress) {
      cb(Math.round((100 * event.loaded) / event.total));
    }
  });
}

export function toResponseBody<T>() {
  return pipe(
    filter((event: HttpEvent<T>) => event.type === HttpEventType.Response),
    map((res: HttpResponse<T>) => res.body)
  );
}

@Component({
  selector: "app-add-book",
  templateUrl: "./add-book.component.html",
  styleUrls: ["./add-book.component.scss"]
})
export class AddBookComponent implements OnInit {
  addBookForm: FormGroup;
  bookTypes = ["paper", "ebook"];
  addBookModel = {} as BookCreateDto;
  jwtHelper = new JwtHelperService();

  progress = 0;
  signup = new FormGroup({
    email: new FormControl(null, Validators.required),
    image: new FormControl(null, [Validators.required, requiredFileType("png")])
  });
  success = false;

  constructor(
    // private store: Store<{ bookList: { books: Book[] } }>,
    private router: Router,
    private bookService: BookService,
    private alertify: AlertifyService,
    private http: HttpClient
  ) {}

  ngOnInit() {
    this.addBookForm = new FormGroup({
      bookData: new FormGroup({
        title: new FormControl(null, Validators.required),
        description: new FormControl(null, [Validators.required]),
        bookType: new FormControl("ebook", Validators.required),
        photoPath: new FormControl(null, Validators.required)
      }),
      author: new FormControl(null, Validators.required)
    });
  }

  onSubmit() {
    console.log(this.addBookForm.value);
    this.addBookModel.title = this.addBookForm.value.bookData.title;
    this.addBookModel.description = this.addBookForm.value.bookData.description;
    this.addBookModel.authorName = this.addBookForm.value.author;
    this.addBookModel.photoPath =
      "https://www.bookbaby.com/images/book-cover-design-basic.png";

    this.bookService.addBook(this.addBookModel).subscribe(
      next => {
        this.alertify.success("Book added!");
        this.router.navigateByUrl("/books");
      },
      error => {
        this.alertify.error("Failed to add book");
      }
    );
  }

  submit() {
    this.success = false;
    if (!this.signup.valid) {
      markAllAsDirty(this.signup);
      return;
    }

    this.http
      .post("http://localhost:8080/signup", toFormData(this.signup.value), {
        reportProgress: true,
        observe: "events"
      })
      .pipe(
        uploadProgress(progress => (this.progress = progress)),
        toResponseBody()
      )
      .subscribe(res => {
        this.progress = 0;
        this.success = true;
        this.signup.reset();
      });
  }

  hasError(field: string, error: string) {
    const control = this.signup.get(field);
    return control.dirty && control.hasError(error);
  }
}

export function markAllAsDirty(form: FormGroup) {
  for (const control of Object.keys(form.controls)) {
    form.controls[control].markAsDirty();
  }
}

export function toFormData<T>(formValue: T) {
  const formData = new FormData();

  for (const key of Object.keys(formValue)) {
    const value = formValue[key];
    formData.append(key, value);
  }

  return formData;
}
