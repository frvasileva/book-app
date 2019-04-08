import { Component, OnInit, Input, Output, EventEmitter } from "@angular/core";
import { Photo } from "src/app/_models/photo";
import { environment } from "src/environments/environment";
import { FileUploader } from "ng2-file-upload";
import { AuthService } from "src/app/_services/auth.service";
import { UserService } from "src/app/user/user.service";
import { AlertifyService } from "src/app/_services/alertify.service";
import { Router } from '@angular/router';

@Component({
  selector: "app-photo-editor",
  templateUrl: "./photo-editor.component.html",
  styleUrls: ["./photo-editor.component.scss"]
})
export class PhotoEditorComponent implements OnInit {
  @Input() photos: Photo[];
  @Output() getMemberPhotoChange = new EventEmitter<string>();
  uploader: FileUploader;
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;
  currentMain: Photo;

  constructor(
    private authService: AuthService,
    private userService: UserService,
    private alertify: AlertifyService,
    private router: Router
  ) {}

  ngOnInit() {
    this.initializeUploader();
  }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url:
        this.baseUrl +
        "profile/add-photo/" +
        this.authService.decodedToken.unique_name,
      authToken: "Bearer " + localStorage.getItem("token"),
      isHTML5: true,
      allowedFileType: ["image"],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = file => {
      file.withCredentials = false;
    };

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const res: Photo = JSON.parse(response);
        this.router.navigate(["/user/profile/teodor-url"]);

        //TODO: UPDATE STATE
      }
    };
  }

  // deletePhoto(id: number) {
  //   this.alertify.confirm("Are you sure you want to delete this photo?", () => {
  //     this.userService
  //       .deletePhoto(this.authService.decodedToken.nameid, id)
  //       .subscribe(
  //         () => {
  //           this.photos.splice(this.photos.findIndex(p => p.id === id), 1);
  //           this.alertify.success("Photo has been deleted");
  //         },
  //         error => {
  //           this.alertify.error("Failed to delete the photo");
  //         }
  //       );
  //   });
  // }
}
