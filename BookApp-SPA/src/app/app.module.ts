import { BrowserModule } from "@angular/platform-browser";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { NgModule } from "@angular/core";
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import { StoreModule } from "@ngrx/store";
import { BsDropdownModule, TabsModule, PaginationModule } from "ngx-bootstrap";
import { FacebookModule } from "ngx-facebook";

import { StoreDevtoolsModule } from "@ngrx/store-devtools";
import { environment } from "../environments/environment"; // Angular CLI environemnt

import { AppComponent } from "./app.component";
import { LayoutComponent } from "./ui-core/layout/layout.component";
import { HeaderComponent } from "./ui-core/header/header.component";
import { FooterComponent } from "./ui-core/footer/footer.component";
import { HomeComponent } from "./ui-core/home/home.component";
import { AboutComponent } from "./ui-core/about/about.component";
import { AppRoutingModule } from "./app-routing.module";
import { NotFoundComponent } from "./ui-core/not-found/not-found.component";
import { ContactComponent } from "./ui-core/contact/contact.component";
import { QuotesComponent } from "./quotes/quotes/quotes.component";

import { bookReducer } from "./_store/book.reducer";
import { catalogReducer } from "./_store/catalog.reducer";
import { userReducer } from "./_store/user.reducer";

import { AuthorService } from "./authors/author.service";
import { UserService } from "./user/user.service";
import { AuthService } from "./_services/auth.service";
import { AlertifyService } from "./_services/alertify.service";
import { BookService } from "./_services/book.service";
import { BookSaverService } from "./_services/bookSaver.service";
import { AuthenticationGuard } from "./_guards/authentication.guard";
import { FileUploadModule } from "ng2-file-upload";
import { JwtInterceptorHelper } from "./_helpers/jwtInterceptor";
import { ErrorInterceptor } from "./_helpers/error.interceptor";
import { LoadingScreenComponent } from "./_shared/loading-screen/loading-screen.component";
import { LoadingScreenInterceptor } from "./_helpers/loading.interceptor";

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    HomeComponent,
    AboutComponent,
    LayoutComponent,
    NotFoundComponent,
    ContactComponent,
    QuotesComponent,
    LoadingScreenComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'serverApp' }),
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    ReactiveFormsModule,
    BsDropdownModule.forRoot(),
    FileUploadModule,
    TabsModule.forRoot(),
    PaginationModule.forRoot(),
    StoreModule.forRoot({
      bookState: bookReducer,
      catalogState: catalogReducer,
      userState: userReducer
    }),
    StoreDevtoolsModule.instrument({
      maxAge: 25, // Retains last 25 states
      logOnly: environment.production // Restrict extension to log-only mode
    }),
    FacebookModule.forRoot()
  ],
  providers: [
    AuthService,
    AuthorService,
    BookService,
    UserService,
    BookSaverService,
    AlertifyService,
    AuthenticationGuard,
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptorHelper, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LoadingScreenInterceptor,
      multi: true
    }

    // ProfileResolver
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
