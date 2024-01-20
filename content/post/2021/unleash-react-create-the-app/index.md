---
date: "2021-06-24T03:21:17Z"
description: ""
draft: false
slug: unleash-react-create-the-app
title: Unleash React - Create the App
---


This post is [part of a series](__GHOST_URL__/unleash-react/) on adding feature flagging to a React application using [Unleash](https://www.getunleash.io/).

{{< bookmark url="__GHOST_URL__/unleash-react/" title="Unleash React - What is Feature Flagging?" description="Feature flags are a powerful tool in the developer’s toolbox. Instead of releasing code in large features and risky deployment operations, new features can be developing incrementally and turned on and off in production by toggling a flag, selecting certain users, or other similar strategies. This m…" icon="__GHOST_URL__/favicon.ico" author="Joshua Marble" publisher="CODEcisions" thumbnail="https://gitlab.com/assets/touch-icon-ipad-retina-8ebe416f5313483d9c1bc772b5bbe03ecad52a54eba443e5215a22caed2a16a2.png" caption="" >}}

In this post, we're going to create the [React application](https://reactjs.org) and hide a feature behind a feature flag.

---

To get started, we need a sample application. Any React app will do, but we'll just use the `.NET5` template for React. Make sure you have the `.NET5`  [SDK](https://dotnet.microsoft.com/download/dotnet/5.0) installed.

```bash
dotnet new react -o unleash-react
cd unleash-react
dotnet run
```

After it starts, go to `[https://localhost:5001](https://localhost:5001)` and see the React app running.

At the top-right is a navigation link to _Fetch data_ that exercises the sample Weather controller in the template. We're going to hide that behind a feature flag.

---

We need to make a couple of changes to our application to use the feature flag. Press `Ctrl+C` to stop the server.

First, we need to disable `https` redirection in `Startup.cs` because the Unleash proxy is not running behind `https` and we can't mix secure and insecure calls in the browser.

```csharp
//app.UseHttpsRedirection();
```

---

Next, we need to install the `unleash-proxy-client`  [package](https://github.com/unleash-hosted/unleash-proxy-client-js) from `npm`.

```bash
cd ClientApp
npm install unleash-proxy-client --save
```

---

Now we can add the code to use the feature flag. We'll start by creating a React component call `Unleash` and initialize it with the connection to our Unleash Proxy.

```bash
cd ClientApp/src
touch Unleash.js
```

Save the below `javascript` code to `Unleash.js`. Change `'some-secret'` if you changed it when [configuring the Unleash Proxy](__GHOST_URL__/unleash-react-configuring-unleash-proxy/).

```javascript
import { UnleashClient } from 'unleash-proxy-client';

const unleash = new UnleashClient({
    url: 'http://[docker_host_ip_address]:3000/proxy',
    clientKey: 'some-secret',
    appName: 'unleash-react',
  });

unleash.start();

export default unleash;
```

---

Now that the unleash component is available for use, we'll add it to the `src/components/NavMenu.js` file to hide the _Fetch data_ link unless the feature flag is enabled.

```javascript
import unleash from '../Unleash';
```

```javascript
this.state = {
  collapsed: true,
  showWeather: false
};
```

```javascript
unleash.on('update', () => {
  this.setState({
    showWeather: unleash.isEnabled('weather')
  });
});
```

```javascript
{this.state.showWeather && 
  <NavItem>
    <NavLink tag={Link} className="text-dark" to="/fetch-data">Fetch data</NavLink>
  </NavItem>
}
```

---

Now execute `dotnet run` again from the root project folder and navigate to `[http://localhost:5000](http://localhost:5000)`.

You should not see the _Fetch data_ link appear (unless you already enabled the `weather` feature toggle).

Go to the Unleash Server web UI and toggle the feature flag. Now wait for the proxy refresh interval to pass and the _Fetch data_ link should show according to the toggle state.

---

There you have it! You just enabled feature flags in your React application. Start releasing code more often and rolling out features more safely!

I suggest you go read about the various [activation strategies](https://docs.getunleash.io/user_guide/activation_strategy) that Unleash supports and get creative.

You may also want to [get creative](https://www.claimcompass.eu/blog/feature-flags-in-react-with-unleash/) with how you use the flags in your app.

I hope you have enjoyed this series. Happy developing!

