import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { MessagesComponent } from "./messages.component";
import { MessageListComponent } from "./message-list/message-list.component";
import { ConversationComponent } from "./conversation/conversation.component";
import { ConversationItemComponent } from "./conversation-item/conversation-item.component";



const authorsRoutes: Routes = [
  {
    path: "", component: MessagesComponent,
    children: [
      { path: "", component: MessageListComponent },
      { path: "message-item", component: ConversationItemComponent },
      { path: "details/:id", component: ConversationComponent },
      { path: "send-new", component: ConversationComponent },
    ]
  }
];

@NgModule({
  imports: [
    RouterModule.forChild(authorsRoutes)
  ],
  exports: [RouterModule]
})
export class MessageRoutingModule {

}
