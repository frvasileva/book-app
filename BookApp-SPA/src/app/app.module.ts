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
import { AuthorService } from "./authors/author.service";
import { UserService } from "./user/user.service";
import { ContactComponent } from "./ui-core/contact/contact.component";
import { QuotesComponent } from "./quotes/quotes/quotes.component";
import { MessageService } from "./messages/message.service";
import { BookSaverService } from "./books/bookSaver.service";

import { bookListReducer } from "./books/books-list/store/bookList.reducer";
import { messageReducer } from "./messages/store/message.reducer";
import { bookDetailsReducer } from "./_store/book-detail.reducer";

import { AuthService } from "./_services/auth.service";
import { AlertifyService } from "./_services/alertify.service";
import { ProfileService } from "./_services/profile.service";
import { ProfileResolver } from "./_resolvers/profile.resolver";
import { BookService } from "./_services/book.service";

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

    StoreModule.forRoot({
      bookList: bookListReducer,
      bookDetails: bookDetailsReducer,
      messageList: messageReducer
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
    ProfileService,
    MessageService,
    BookSaverService,
    AlertifyService,
    ProfileResolver
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
