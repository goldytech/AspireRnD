"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
Object.defineProperty(exports, "__esModule", { value: true });
const process_1 = require("process");
const fetchAlphaBroderProducts = () => __awaiter(void 0, void 0, void 0, function* () {
    try {
        const graphQLServiceAddress = process_1.env['services__graphql-api__http__0'];
        console.log(graphQLServiceAddress);
        const response = yield fetch(`${graphQLServiceAddress}/graphql`, {
            headers: {
                authorization: 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InQxIiwiVGVuYW50SWQiOiIxMjMiLCJuYmYiOjE3MjI5NTAwNzIsImV4cCI6MTcyMjk1MzY3MiwiaWF0IjoxNzIyOTUwMDcyLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MDA2IiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAwIn0.i4IyJvWpETzIQd3p3-bgORoZxrXEAGOCzdoK8UiLzOM',
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
        const json = yield response.json();
        if (json.data &&
            json.data.alphaBroderProducts &&
            json.data.alphaBroderProducts.nodes) {
            json.data.alphaBroderProducts.nodes.forEach(product => {
                console.log(`Category: ${product.alphaBroderCategory.name}`);
                console.log(`Description: ${product.description}`);
                console.log(`Name: ${product.name}`);
                console.log(`Price: $${product.price}`);
                console.log('---');
            });
        }
        else {
            console.error('No products found or invalid response format.');
        }
    }
    catch (error) {
        console.error(error);
    }
});
fetchAlphaBroderProducts()
    .then(() => {
    console.log('Products fetched successfully.');
})
    .catch(error => {
    console.error('Error fetching products:', error);
});
