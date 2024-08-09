import { fetchAlphaBroderProducts } from './fetchAlphaBroderProducts.js';

const username = 't1';
const password = 'secret';

fetchAlphaBroderProducts(username, password).then(products => console.log(products));
