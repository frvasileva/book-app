import { Injectable } from "@angular/core";

@Injectable()
export class BookSaverService {
  private bookSaverItemList = [
    {
      id: 1,
      label: "Favorites books"
    },
    {
      id: 2,
      label: "Want to read"
    },
    {
      id: 3,
      label: "Want to share"
    }
  ];

  getUserLists(keyword: string) {
    if (keyword === null) {
      return this.bookSaverItemList;
    } else {
      return this.bookSaverItemList.filter(item =>
        item.label.includes(keyword)
      );
    }
  }
}
