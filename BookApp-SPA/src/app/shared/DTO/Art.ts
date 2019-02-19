export class Art {
  title: string;
  body: string;
  addedOn: Date;
  addedBy: string;

  constructor(title, body, addedOn, addedBy) {
    this.title = title;
    this.body = body;
    this.addedBy = addedBy;
    this.addedOn = addedOn;
  }
}
