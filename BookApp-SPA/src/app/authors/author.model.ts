export class Author {
  public id: string;
  public name: string;
  public biography: string;
  public imagePath?: string;

  public constructor(id: string, name: string, biography: string, imagePath: string) {
    this.id = id;
    this.name = name;
    this.biography = biography;
    this.imagePath = imagePath;
  }
}
