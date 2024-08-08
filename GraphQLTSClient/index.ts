import {env} from 'process';

type AlphaBroderCategory = {
  description: string;
  id: string;
  lastModifiedAt: string;
  name: string;
};

type AlphaBroderProduct = {
  alphaBroderCategory: AlphaBroderCategory;
  description: string;
  name: string;
  price: number;
};

type AlphaBroderProductsResponse = {
  data: {
    alphaBroderProducts: {
      nodes: AlphaBroderProduct[];
    };
  };
};

const fetchAlphaBroderProducts = async (): Promise<void> => {
  try {
    const graphQLServiceAddress = env['services__graphql-api__http__0'];
    console.log(graphQLServiceAddress);
    const response: Response = await fetch(`${graphQLServiceAddress}/graphql`, {
      headers: {
        authorization:
          'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InQxIiwiVGVuYW50SWQiOiIxMjMiLCJuYmYiOjE3MjI5NTAwNzIsImV4cCI6MTcyMjk1MzY3MiwiaWF0IjoxNzIyOTUwMDcyLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MDA2IiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAwIn0.i4IyJvWpETzIQd3p3-bgORoZxrXEAGOCzdoK8UiLzOM',
        'content-type': 'application/json',
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
        }`,
      }),
      method: 'POST',
    });

    if (!response.ok) {
      console.error(`Failed to fetch products: ${response.statusText}`);
      return;
    }
    const json: AlphaBroderProductsResponse = await response.json();

    if (
      json.data &&
      json.data.alphaBroderProducts &&
      json.data.alphaBroderProducts.nodes
    ) {
      json.data.alphaBroderProducts.nodes.forEach(product => {
        console.log(`Category: ${product.alphaBroderCategory.name}`);
        console.log(`Description: ${product.description}`);
        console.log(`Name: ${product.name}`);
        console.log(`Price: $${product.price}`);
        console.log('---');
      });
    } else {
      console.error('No products found or invalid response format.');
    }
  } catch (error) {
    console.error(error);
  }
};

fetchAlphaBroderProducts()
  .then(() => {
    console.log('Products fetched successfully.');
  })
  .catch(error => {
    console.error('Error fetching products:', error);
  });
