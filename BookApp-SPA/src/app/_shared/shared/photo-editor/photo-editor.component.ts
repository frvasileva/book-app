import { Component, OnInit, Input } from "@angular/core";
import { Photo } from "../../../_models/photo";
import { environment } from "../../../../environments/environment";
import { FileUploader } from "ng2-file-upload";
import { AlertifyService } from "../../../_services/alertify.service";
import { Router } from "@angular/router";
import { Store } from "@ngrx/store";
import { Profile } from "../../../_models/profile";
import * as UserProfileActions from "../../../_store/user.actions";

@Component({
  selector: "app-photo-editor",
  templateUrl: "./photo-editor.component.html",
  styleUrls: ["./photo-editor.component.scss"]
})
export class PhotoEditorComponent implements OnInit {
  @Input() photos: Photo[];
  @Input() apiDestinationUrl: string;
  @Input() uploaderType: string;
  uploader: FileUploader;
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;
  currentMain: Photo;
  profile$: Profile;

  constructor(
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
      url: this.apiDestinationUrl,
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
        if (this.uploaderType === "profile-photo") {
          this.store.dispatch(
            new UserProfileActions.UpdateUserAvatarAction(response)
          );

          this.router.navigate([
            "/user/profile/",
            JSON.parse(response).friendlyUrl
          ]);
          this.alertify.success("Photo updated");
        } else if (this.uploaderType === "book-cover-photo") {
          this.router.navigate(["/books"]);
        }
      }
    };
  }
}
