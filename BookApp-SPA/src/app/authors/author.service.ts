import { Injectable } from "@angular/core";
import { Author } from "./author.model";

@Injectable()
export class AuthorService {
  private authors: Author[] = [
    new Author(
      "1",
      "Lorem Ipsum ",
      "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.    ",
      "https://i.dailymail.co.uk/i/pix/2012/04/12/article-0-11E0FAB4000005DC-379_468x581.jpg"
    ),

    new Author(
      "2",
      "Alan Miln",
      "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using 'Content here, content here', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).    ",
      "https://i.dailymail.co.uk/i/pix/2012/04/12/article-0-11E0FAB4000005DC-379_468x581.jpg"
    ),
    new Author(
      "3",
      "Kathian Brands",
      "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet. It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, injected humour, or non-characteristic words etc.    ",
      "https://i.dailymail.co.uk/i/pix/2012/04/12/article-0-11E0FAB4000005DC-379_468x581.jpg"
    )
  ];

  getAuthors() {
    return this.authors.slice();
  }

  getAuthor(id: string) {
    return this.authors.find(x => x.id === id.toString());
  }
}
