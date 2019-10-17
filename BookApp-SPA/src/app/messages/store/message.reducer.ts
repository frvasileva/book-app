import * as MessageActions from "./message.action";
import { Message } from '../model/message.model';

const initialMessagePreviewState = {
  messages: [
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
      // tslint:disable-next-line:max-line-length
      "Здравейте, пиша ви относно книги на издателство ИБИС.. бла бла бла Здравейте, пиша ви относно книги на издателство ИБИС.. бла бла бла Здравейте, пиша ви относно книги на издателство ИБИС.. бла бла бла Здравейте, пиша ви относно книги на издателство ИБИС.. бла бла бла ",
      "We’re sorry to see you go—learn how to save your data"
    )
  ]
};

export function messageReducer(
  state = initialMessagePreviewState,
  action: MessageActions.MessageActions
) {
  switch (action.type) {
    case MessageActions.SEND_MESSAGE: {
      console.log({ ...state, messages: [state.messages], action });
      return {
        ...state,
        messages: [...state.messages, action.payload]
      };
    }
    // case BookListActions.UPDATE_BOOK:
    //   return { ...state, books: [state.books], action };
    default:
      return state;
  }
}
