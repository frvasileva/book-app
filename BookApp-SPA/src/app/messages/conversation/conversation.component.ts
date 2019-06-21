import { Component, OnInit } from "@angular/core";
import { Store } from "@ngrx/store";

import { Message } from "../model/message.model";
import * as MessageActions from "../../messages/store/message.action";
import { Observable } from "rxjs";

@Component({
  selector: "app-conversation",
  templateUrl: "./conversation.component.html",
  styleUrls: ["./conversation.component.scss"]
})
export class ConversationComponent implements OnInit {
  messageListState: Observable<{ messages: Message[] }>;

  constructor(private store: Store<{ messageList: { messages: Message[] } }>) {}
  postDataArr: Message[] = [];

  onAddPost(postData) {
    this.store.dispatch(new MessageActions.SendMessageAction(postData));
    this.postDataArr.push(postData);
  }

  ngOnInit() {
    this.messageListState = this.store.select("messageList");
  }
}
