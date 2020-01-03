import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { FileUploadModule } from "ng2-file-upload";
import { PhotoEditorComponent } from "./photo-editor/photo-editor.component";

import { ToggleButtonComponent } from "../../shared/toggle-button/toggle-button.component";
import { SeoHelperService } from "../seo-helper.service";

@NgModule({
  declarations: [PhotoEditorComponent, ToggleButtonComponent],
  imports: [CommonModule, FileUploadModule],
  exports: [PhotoEditorComponent, ToggleButtonComponent],
  providers: [SeoHelperService]
})
export class SharedModule {}
