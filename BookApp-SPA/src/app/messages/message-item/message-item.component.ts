import { Component, OnInit, Input } from "@angular/core";
import { MessageService } from "../message.service";
import { ActivatedRoute, Params } from "@angular/router";
import { Message } from "../model/message.model";

@Component({
  selector: "app-message-item",
  templateUrl: "./message-item.component.html",
  styleUrls: ["./message-item.component.scss"]
})
export class MessageItemComponent implements OnInit {
  message: Message;
  messageId: string;

  @Input() listPost: Message[];

  constructor(
    private messageService: MessageService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {}

  onDeleteMessage(messageId) {
    this.messageService.deleteMessage(messageId);
  }
}
