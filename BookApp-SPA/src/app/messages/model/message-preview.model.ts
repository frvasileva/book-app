export class MessagePreview {
  id: string;
  sender: string;
  receiver: string;
  subject: string;
  sentOn: Date;
  readOn: Date;
  isRead: boolean;

  constructor(id, sender, receiver, sentOn, readOn, isRead, subject) {
    this.id = id;
    this.sender = sender;
    this.receiver = receiver;
    this.subject = subject;
    this.sentOn = sentOn;
    this.readOn = readOn;
    this.isRead = isRead;
  }
}
