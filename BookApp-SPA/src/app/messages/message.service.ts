import { Injectable } from "@angular/core";
import { MessagePreview } from "./model/message-preview.model";
import { Message } from "./model/message.model";

@Injectable()
export class MessageService {
  private messages: Message[] = [
    new Message(
      1,
      null,
      "vasil@vasilbachev.com",
      "fanka.bacheva@gmail.com",
      new Date(),
      new Date(),
      true,
      "Скоро ще можеш да четеш Алхимикът",
      "subject mail"
    ),
    new Message(
      2,
      1,
      "petar@vasilbachev.com",
      "fanka.bacheva@gmail.com",
      new Date(),
      new Date(),
      false,
      "книги на издателство Тиара букс",
      "subject mail"
    ),
    new Message(
      3,
      1,
      "maria@vasilbachev.com",
      "fanka.bacheva@gmail.com",
      new Date(),
      new Date(),
      false,
      "Здравейте, пиша ви относно книги на издателство ИБИС.. бла бла бла Здравейте, пиша ви относно книги на издателство ИБИС.. бла бла бла Здравейте, пиша ви относно книги на издателство ИБИС.. бла бла бла Здравейте, пиша ви относно книги на издателство ИБИС.. бла бла бла ",
      "We’re sorry to see you go—learn how to save your data"
    )
  ];

  private messagesPreview: MessagePreview[] = [
    new MessagePreview(
      1,
      "vasil@vasilbachev.com",
      "fanka.bacheva@gmail.com",
      new Date(),
      new Date(),
      true,
      "Скоро ще можеш да четеш Алхимикът"
    ),
    new MessagePreview(
      2,
      "petar@vasilbachev.com",
      "fanka.bacheva@gmail.com",
      new Date(),
      new Date(),
      false,
      "книги на издателство Тиара букс"
    ),
    new MessagePreview(
      3,
      "maria@vasilbachev.com",
      "fanka.bacheva@gmail.com",
      new Date(),
      new Date(),
      false,
      "книги на издателство ИБИС"
    )
  ];

  getPreviewMessagesByReceiver(username: string) {
    return this.messagesPreview.filter(message =>
      message.receiver.includes(username)
    );
  }

  getMessagesById(id: string): Message {
    console.log({ id });
    console.log(this.messages);
    return this.messages.find(message => message.id == id);
  }

  sendMessage(message: Message) {
    this.messages.push(message);
  }

  deleteMessage(messageId: number) {
    this.messages.splice(messageId, 1);
  }
}
