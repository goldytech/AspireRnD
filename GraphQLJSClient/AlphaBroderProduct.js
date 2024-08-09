import { AlphaBroderCategory } from './AlphaBroderCategory.js';

export class AlphaBroderProduct {
  constructor(alphaBroderCategory, description, name, price) {
    this.alphaBroderCategory = new AlphaBroderCategory(
      alphaBroderCategory.description,
      alphaBroderCategory.id,
      alphaBroderCategory.lastModifiedAt,
      alphaBroderCategory.name
    );
    this.description = description;
    this.name = name;
    this.price = price;
  }
}
