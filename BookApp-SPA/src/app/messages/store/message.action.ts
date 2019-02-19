import { Action } from "@ngrx/store";
import { Message } from "../model/message.model";

export const SEND_MESSAGE = "SEND_MESSAGE";
export const DELETE_MESSAGE = "DELETE_MESSAGE";
export const MARK_AS_READ_MESSAGE = "MARK_AS_READ_MESSAGE";

export class SendMessageAction implements Action {
  readonly type = SEND_MESSAGE;
  constructor(public payload: Message) {}
}
export class DeleteMessageAction implements Action {
  readonly type = DELETE_MESSAGE;
  payload: Message;
}

export class MarkAsReadMessageAction implements Action {
  readonly type = MARK_AS_READ_MESSAGE;
  payload: Message;
}

export type MessageActions =
  | SendMessageAction
  | DeleteMessageAction
  | MarkAsReadMessageAction;
