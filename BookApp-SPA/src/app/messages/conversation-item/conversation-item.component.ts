import { OnInit, Component } from "@angular/core";
import { Observable } from "rxjs";
import { Message } from "../model/message.model";

@Component({
  selector: "app-conversation-item",
  templateUrl: "./conversation-item.component.html",
  styleUrls: ["./conversation-item.component.scss"]
})
export class ConversationItemComponent implements OnInit {
  constructor() {}

  ngOnInit() {
    //  this.bookList = this.store.select(messageList);
  }
}
