---
date: "2021-06-24T02:03:12Z"
description: ""
draft: false
slug: unleash-react-configuring-unleash-proxy
title: Unleash React - Configuring Unleash Proxy
---


This post is part of [a series](/unleash-react/) on adding feature
flagging to a React application using [Unleash](https://www.getunleash.io/).

--------------------------------------------------------------------------------

To use Unleash in server-side code, all you need is [the server running](/unleash-react-running-the-server/) and one of the [Unleash SDKs](https://docs.getunleash.io/sdks). However, lightweight frontend applications,
like React or mobile apps, need to take a different approach to remain highly
performant. This is why Unleash has developed the [Unleash Proxy](https://www.unleash-hosted.com/articles/the-unleash-proxy/). It essentially
acts as a client SDK to the Unleash server, allowing lightweight apps to proxy
requests through it.

Unleash Proxy is also available as a [docker image](https://hub.docker.com/r/unleashorg/unleash-proxy). Just like when we 
configured the [Unleash Server](/unleash-react-running-the-server/),
we will be using docker-compose to run the proxy.


--------------------------------------------------------------------------------

We will be creating another docker-compose.yml file for the Unleash Proxy. You
may ask yourself why we don't just add it to the previous one and start them all
together. Good question!

Unfortunately, there is no way to specify an API key for the Unleash Server when
starting it, so it can't be known ahead of time to provide when starting the
Unleash Proxy. This means we need to start the server, configure the API key,
then start the proxy using that key.

So, let's go ahead and get that API key ready.

--------------------------------------------------------------------------------

First, login to the server that [you started previously](/unleash-react-running-the-server/). 
You will see a menu icon in the top left corner. Open the menu and go to Admin. 

--------------------------------------------------------------------------------

Navigate to API ACCESS and click the button to ADD NEW API KEY.

--------------------------------------------------------------------------------

Give the API key a name and select `Client` for the API type.

--------------------------------------------------------------------------------

Once you have created the key, you will see it listed. Copy the Secret value,
which is the API token.

--------------------------------------------------------------------------------

Now we will start the Unleash Proxy in docker. First, since we will be using two
separate docker-compose files, we need to create a shared virtual network they
can use to communicate.

`docker network create unleash-net`

--------------------------------------------------------------------------------

Now update the docker-compose.yml file for the [Unleash Server](/unleash-react-running-the-server/) to include the network and
re-run docker-compose up.

```yaml
version: "3.4"
services:
  db:
    image: postgres:10-alpine
    environment:
      POSTGRES_DB: "db"
      POSTGRES_HOST_AUTH_METHOD: "trust"
    expose:
      - 5432
  unleash:
    image: unleashorg/unleash-server
    ports:
      - "4242:4242"
    environment:
      DATABASE_URL: "postgres://postgres:unleash@db/postgres"
      DATABASE_SSL: "false"
    depends_on:
      - db

networks:
  default:
    external:
      name: unleash-net
```

--------------------------------------------------------------------------------

Next, in a new folder, save the yaml below into a file called docker-compose.yml. 
Replace `[paste_api_token]` with the copied secret value from Unleash. Replace 
`some-secret` with any secret of your choice, to later be used to authenticate to
the proxy from the React app.

```yaml
version: "3.4"
services:
  unleash-proxy:
    image: unleashorg/unleash-proxy
    ports:
      - "3000:3000"
    environment:
      UNLEASH_URL: "http://unleash:4242/api"
      UNLEASH_API_TOKEN: "[paste_api_token]"
      UNLEASH_PROXY_SECRETS: "some-secret"

networks:
  default:
    external:
      name: unleash-net
```

Now run `docker-compose up`. This will start the Unleash Proxy to communicate on
the virtual network with the Unleash Server via its service name unleash from
the other `docker-compose.yml` file.


--------------------------------------------------------------------------------

Once Unleash Proxy is running, you can test it out using curl.

```bash
curl http://[docker_host_ip_address]:3000/proxy -H "Authorization: some-secret"
```

You should see a json response containing all defined toggles.

```json
{"toggles":[]}%
```

--------------------------------------------------------------------------------

Let's go ahead and create a feature toggle in Unleash to make sure we see it
here.

Navigate to the Unleash Server web UI and click the CREATE FEATURE TOGGLE 
button. Create the feature like below. We will use it next in the React app. The
default Standard strategy will be created.


--------------------------------------------------------------------------------

Try the curl command again and see the new feature toggle included in the json 
response.

```bash
curl http://[docker_host_ip_address]:3000/proxy -H "Authorization: some-secret"
```
<br>

```json
{"toggles":[{"name":"weather","enabled":true,"variant":{"name":"disabled","enabled":false}}]}%
```

--------------------------------------------------------------------------------

Now that the Unleash Proxy is running and we have a feature toggle created, we
can [start using it from a React app](/unleash-react-create-the-app/).