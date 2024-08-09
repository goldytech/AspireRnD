import fetch from 'node-fetch';
import { AlphaBroderProduct } from './AlphaBroderProduct.js';
import { fetchToken } from './fetchToken.js';

const baseUrl = process.env['services__graphql-api__https__0'];

export async function fetchAlphaBroderProducts(username, password) {
  const token = await fetchToken(username, password);

  const response = await fetch(`${baseUrl}/graphql`, {
    method: 'POST',
    headers: {
      'authorization': `Bearer ${token}`,
      'content-type': 'application/json'
    },
    body: JSON.stringify({
      query: `query alphaborder {
        alphaBroderProducts {
          nodes {
            alphaBroderCategory {
              description
              id
              lastModifiedAt
              name
            }
            description
            name
            price
          }
        }
      }`
    })
  });

  const jsonResponse = await response.json();
  const products = jsonResponse.data.alphaBroderProducts.nodes.map(node =>
    new AlphaBroderProduct(
      node.alphaBroderCategory,
      node.description,
      node.name,
      node.price
    )
  );

  return products;
}
