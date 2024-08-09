export class AlphaBroderCategory {
  constructor(description, id, lastModifiedAt, name) {
    this.description = description;
    this.id = id;
    this.lastModifiedAt = new Date(lastModifiedAt);
    this.name = name;
  }
}
