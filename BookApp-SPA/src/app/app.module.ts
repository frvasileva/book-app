import { BrowserModule } from "@angular/platform-browser";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { NgModule } from "@angular/core";
import { HttpClientModule } from "@angular/common/http";
import { StoreModule } from "@ngrx/store";
import { BsDropdownModule } from "ngx-bootstrap";

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
import { ArticlesComponent } from "./articles/articles/articles.component";
import { ContactComponent } from "./ui-core/contact/contact.component";
import { QuotesComponent } from "./quotes/quotes/quotes.component";

import { bookListReducer } from "./books/books-list/store/bookList.reducer";
import { messageReducer } from "./messages/store/message.reducer";
import { bookDetailsReducer } from "./_store/book-detail.reducer";
import { catalogReducer } from "./_store/catalog.reducer";
import { userReducer } from "./_store/user.reducer";

import { AuthorService } from "./authors/author.service";
import { UserService } from "./user/user.service";
import { AuthService } from "./_services/auth.service";
import { AlertifyService } from "./_services/alertify.service";
// import { ProfileResolver } from "./_resolvers/profile.resolver";
import { BookService } from "./_services/book.service";
import { MessageService } from "./messages/message.service";
import { BookSaverService } from "./books/bookSaver.service";
import { AuthenticationGuard } from "./_guards/authentication.guard";
import { usersReducer } from "./_store/users.reducer";
import { FileUploadModule } from "ng2-file-upload";

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    HomeComponent,
    AboutComponent,
    LayoutComponent,
    NotFoundComponent,
    ArticlesComponent,
    ContactComponent,
    QuotesComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    ReactiveFormsModule,
    BsDropdownModule.forRoot(),
    FileUploadModule,
    StoreModule.forRoot({
      bookList: bookListReducer,
      bookDetails: bookDetailsReducer,
      messageList: messageReducer,
      catalog: catalogReducer,
      userProfile: userReducer,
      userProfiles: usersReducer
    }),
    StoreDevtoolsModule.instrument({
      maxAge: 25, // Retains last 25 states
      logOnly: environment.production // Restrict extension to log-only mode
    })
  ],
  providers: [
    AuthService,
    AuthorService,
    BookService,
    UserService,
    MessageService,
    BookSaverService,
    AlertifyService,
    AuthenticationGuard
    // ProfileResolver
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
