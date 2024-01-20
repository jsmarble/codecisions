---
date: "2021-06-23T22:50:55Z"
description: ""
draft: false
slug: unleash-react-running-the-server
title: Unleash React - Running the Server
---


This post is [part of a series](__GHOST_URL__/unleash-react/) on adding feature flagging to a React application using [Unleash](https://www.getunleash.io/).

{{< bookmark url="__GHOST_URL__/unleash-react/" title="Unleash React - What is Feature Flagging?" description="Feature flags are a powerful tool in the developer’s toolbox. Instead of releasing code in large features and risky deployment operations, new features can be developing incrementally and turned on and off in production by toggling a flag, selecting certain users, or other similar strategies. This m…" icon="__GHOST_URL__/favicon.ico" author="Joshua Marble" publisher="CODEcisions" thumbnail="" caption="" >}}

In this post, we'll learn how to get Unleash server running in docker.

---

To use Unleash in your application, you must have an [Unleash server](https://docs.getunleash.io/#unleash-server) running. Unleash provides [hosted plans](https://www.getunleash.io/plans), but for this series we will be running it ourselves using [the docker image](https://hub.docker.com/r/unleashorg/unleash-server/). This post assumes you have a docker host available and have a working understanding of [docker-compose](https://docs.docker.com/compose/).

Unleash uses [PostgreSQL](https://www.postgresql.org/) as its database. For this example, we will be running it in docker as well. The easiest way to run both together is to use docker-compose.

Save the `yaml` below into a file named `docker-compose.yml` then run `docker-compose up`. This will start two containers, one for PostgreSQL and one for the Unleash server. The Unleash server can reach the database on a virtualized network by using its service name `db` in the `DATABASE_URL` environment variable.

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
```

---

Once your `docker-compose` command completes, you will have an Unleash server running on port `4242` on your docker host. Navigate to `http://[docker_host_ip]:4242` in your browser and you will see the Unleash server login page. Login with username `admin` and password `unleash4all`.

At this point you can play around with the features of Unleash server, but we will continue on to [configuring Unleash Proxy](__GHOST_URL__/unleash-react-configuring-unleash-proxy/).

