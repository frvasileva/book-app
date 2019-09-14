import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { FileUploadModule } from "ng2-file-upload";
import { PhotoEditorComponent } from "./photo-editor/photo-editor.component";

import { ToggleButtonComponent } from "src/app/shared/toggle-button/toggle-button.component";

@NgModule({
  declarations: [PhotoEditorComponent, ToggleButtonComponent],
  imports: [CommonModule, FileUploadModule],
  exports: [PhotoEditorComponent, ToggleButtonComponent]
})
export class SharedModule {}
