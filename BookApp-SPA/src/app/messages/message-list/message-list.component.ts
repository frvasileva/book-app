import { Component, OnInit } from "@angular/core";
import { MessageService } from "../message.service";
import { MessagePreview } from "../model/message-preview.model";

@Component({
  selector: "app-message-list",
  templateUrl: "./message-list.component.html",
  styleUrls: ["./message-list.component.scss"]
})
export class MessageListComponent implements OnInit {

  messages: MessagePreview[];

  constructor(private messageService: MessageService) { }

  ngOnInit() {
    this.messages = this.messageService.getPreviewMessagesByReceiver("fanka.bacheva@gmail.com");
    console.log("messages", this.messages);
  }

}
