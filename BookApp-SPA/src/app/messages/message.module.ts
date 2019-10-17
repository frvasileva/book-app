import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";
import { MessageListComponent } from "./message-list/message-list.component";
import { MessageRoutingModule } from "./message-routing.module";
import { MessagesComponent } from "./messages.component";
import { MessageItemComponent } from "./message-item/message-item.component";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { SendMessageComponent } from "./send-message/send-message.component";
import { ConversationComponent } from "./conversation/conversation.component";
import { ConversationItemComponent } from "./conversation-item/conversation-item.component";
import { CreateMessageComponent } from "./create-message/create-message.component";

@NgModule({
  declarations: [
    MessageListComponent,
    MessagesComponent,
    MessageItemComponent,
    SendMessageComponent,
    ConversationComponent,
    ConversationItemComponent,
    CreateMessageComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    MessageRoutingModule,
    FormsModule,
    ReactiveFormsModule
  ],

  exports: []
})
export class MessagesModule {}
