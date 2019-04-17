import { Component, OnInit, Input } from "@angular/core";
import { Photo } from "src/app/_models/photo";
import { environment } from "src/environments/environment";
import { FileUploader } from "ng2-file-upload";
import { AuthService } from "src/app/_services/auth.service";
import { AlertifyService } from "src/app/_services/alertify.service";
import { Router } from "@angular/router";
import { Store } from "@ngrx/store";
import { Profile } from "src/app/_models/profile";
import * as UserProfileActions from "../../../_store/user.actions";

@Component({
  selector: "app-photo-editor",
  templateUrl: "./photo-editor.component.html",
  styleUrls: ["./photo-editor.component.scss"]
})
export class PhotoEditorComponent implements OnInit {
  @Input() photos: Photo[];
  uploader: FileUploader;
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;
  currentMain: Photo;
  profile$: Profile;

  constructor(
    private authService: AuthService,
    private alertify: AlertifyService,
    private router: Router,
    private store: Store<{ userState: Profile }>
  ) {}

  ngOnInit() {
    this.store
      .select(state => state)
      .subscribe(res => {
        this.profile$ = res.userState;
      });

    this.initializeUploader();
  }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + "profile/add-photo/" + this.profile$.friendlyUrl,
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
        this.store.dispatch(
          new UserProfileActions.UpdateUserAvatarAction(response)
        );
        this.router.navigate(["/user/profile/teodor-url"]);
        this.alertify.success("Photo updated");
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
