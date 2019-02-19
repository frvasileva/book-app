import { Component, OnInit, Output, EventEmitter } from "@angular/core";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { MessageService } from "../message.service";
import { Message } from "../model/message.model";

@Component({
  selector: "app-send-message",
  templateUrl: "./send-message.component.html",
  styleUrls: ["./send-message.component.scss"]
})
export class SendMessageComponent implements OnInit {
  sendMessageForm: FormGroup;
  message: Message = new Message("", "", "", "", "", "", false, "", "");

  @Output() messageAddedEvent = new EventEmitter<Message>();

  constructor(private messageService: MessageService) {}

  ngOnInit() {
    this.sendMessageForm = new FormGroup({
      messageData: new FormGroup({
        subject: new FormControl("This is email subject", Validators.required),
        body: new FormControl(null, [Validators.required])
      })
    });
  }

  onSubmit() {
    this.message.body = this.sendMessageForm.value.messageData.body;
    this.message.subject = this.sendMessageForm.value.messageData.subject;
    this.messageService.sendMessage(this.message);
    this.messageAddedEvent.emit({ ...this.message });

    this.sendMessageForm.reset();
  }
}
