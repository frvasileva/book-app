import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { LayoutComponent } from "./layout/layout.component";
import { HeaderComponent } from "./header/header.component";
import { FooterComponent } from "./footer/footer.component";
import { ContactComponent } from "./contact/contact.component";
import { ReactiveFormsModule } from "@angular/forms";
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [
    LayoutComponent,
    HeaderComponent,
    FooterComponent,
    ContactComponent
  ],
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  exports: [
    UiCoreModule
  ]
})
export class UiCoreModule {

}
