import fetch from 'node-fetch';
import { AlphaBroderProduct } from './AlphaBroderProduct.js';
import { fetchToken } from './fetchToken.js';

const baseUrl = process.env['services__graphql-api__https__0'];

export async function fetchAlphaBroderProducts(username, password) {
  let token = await fetchToken(username, password);
  // remove double quotes from token
  token = token.replace(/['"]+/g, '');
  //const token ="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InQxIiwiVGVuYW50SWQiOiIxMjMiLCJuYmYiOjE3MjM0NTEwMTEsImV4cCI6MTcyMzQ1NDYxMSwiaWF0IjoxNzIzNDUxMDExLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MDA2IiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzAwNiJ9.SbKlWtS31tF48Ze2fkWDcvllburo9Ima5hIpAb5JwsA";
  console.log('Using Token:', token); // Log the token for debugging

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
  console.log('GraphQL Response:', jsonResponse); // Log the response for debugging

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
