import fetch from 'node-fetch';

const baseUrl = process.env['services__graphql-api__https__0'];

export async function fetchToken(username, password) {
  const response = await fetch(`${baseUrl}/api/v1/auth/token`, {
    method: 'POST',
    headers: {
      'accept': '*/*',
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      username,
      password
    })
  });

  const token = await response.text();
  console.log('Fetched Token:', token); // Log the token for debugging
  return token;
}
