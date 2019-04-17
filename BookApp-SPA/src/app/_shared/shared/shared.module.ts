import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { PhotoEditorComponent } from "./photo-editor/photo-editor.component";
import { FileUploadModule } from "ng2-file-upload";

@NgModule({
  declarations: [PhotoEditorComponent],
  imports: [CommonModule, FileUploadModule],
  exports: [PhotoEditorComponent]
})
export class SharedModule {}
