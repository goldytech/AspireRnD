import asyncio
import aiohttp
import os
from pydantic import BaseModel, Field
from typing import List, Optional
from datetime import datetime


class AlphaBroderCategory(BaseModel):
    description: str
    id: str
    lastModifiedAt: datetime
    name: str


class AlphaBroderProduct(BaseModel):
    alphaBroderCategory: AlphaBroderCategory
    description: str
    name: str
    price: float


class AlphaBroderProductsResponse(BaseModel):
    nodes: List[AlphaBroderProduct]


class DataResponse(BaseModel):
    alphaBroderProducts: AlphaBroderProductsResponse


class GraphQLResponse(BaseModel):
    data: DataResponse


async def fetch_token():
    url = os.getenv('services__graphql-api__https__0') + '/api/v1/auth/token'
    headers = {
        'Content-Type': 'application/json'
    }
    payload = {
        "username": "t1",
        "password": "secret"
    }

    async with aiohttp.ClientSession() as session:
        async with session.post(url, headers=headers, json=payload,ssl=False) as response:
            token = await response.text()
            token = token.strip('"')  # Remove double quotes
            return token


async def fetch_data(token):
    # token ="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InQxIiwiVGVuYW50SWQiOiIxMjMiLCJuYmYiOjE3MjM0NjU5NDEsImV4cCI6MTcyMzQ2OTU0MSwiaWF0IjoxNzIzNDY1OTQxLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MDA2IiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzAwNiJ9.sFvLOHojFqvcr2q3sQ94PIBnjYYmGIjq302kiztS4nc"
    url = os.getenv('services__graphql-api__https__0')
    headers = {
        'authorization': f'Bearer {token}',
        'content-type': 'application/json'
    }
    payload = {
        "query": """
        query alphaborder {
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
        }
        """
    }

    async with aiohttp.ClientSession() as session:
        async with session.post(f'{url}/graphql', headers=headers, json=payload,ssl=False) as response:
            response_json = await response.json()
            print(response_json)
            return GraphQLResponse(**response_json)


async def main():
    token = await fetch_token()
    print(token)
    data = await fetch_data(token)
    print(data)


if __name__ == "__main__":
    asyncio.run(main())
