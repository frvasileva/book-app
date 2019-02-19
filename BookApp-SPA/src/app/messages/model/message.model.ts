export class Message {
  id: string;
  replyId?: string;
  sender: string;
  receiver: string;
  subject: string;
  body: string;
  sentOn: Date;
  readOn: Date;
  isRead: boolean;

  constructor(id, replyId, sender, receiver, sentOn, readOn, isRead, body, subject) {
    this.id = id;
    this.replyId = replyId;
    this.sender = sender;
    this.receiver = receiver;
    this.body = body;
    this.subject = subject;
    this.sentOn = sentOn;
    this.readOn = readOn;
    this.isRead = isRead;
  }
}
