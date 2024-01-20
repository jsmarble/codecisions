---
date: "2013-04-08T19:48:40Z"
description: ""
draft: false
slug: arvixe-review
title: Arvixe Review
---


In my [previous post](__GHOST_URL__/blog-refresh/) I mentioned that [Arvixe](http://www.arvixe.com) was a good and cheap web host. They provide a good set of features for a low price for Windows hosting. I used Arvixe for almost two years to host the BlogEngine.net and FunnelWeb platforms for this site. This review is from the perspective of a low traffic website for low cost.

### Pros

1. Windows hosting on the cheap
2. Free SQL Server databases
3. Good support response times
4. Free domain name registration for a single domain
5. $10 domain registration with free privacy guard
6. Starts at $5/mo

### Cons

1. Barely adequate control panel software
2. Limited registrar access to your domains
3. Terms of service for your free domain
4. **Update:** They have access to your password (no hashing, see update section below)

The main benefit that Arvixe provides is good features for cheap. It’s hard to find a decent Windows host for $5/mo. Throw in the free domain and it’s easy to see the value of Arvixe.

My primary complaint with Arvixe was the quality of the control panel (powered by [WebsitePanel](http://www.websitepanel.net/). You can manage most everything from it, but it can be clunky. I dreaded having to perform any maintenance on my account because it was slow and many operations failed. I think I only once successfully got the Microsoft Web App Gallery to install something correctly. I ended up manually installing anything I wanted to use. Even if it got the job done, I never felt very reassured in the control panel.

Another concern about Arvixe is the [terms of service](http://www.arvixe.com/tos.php) surround the free domain. “Domains registered through this policy are the property of Arvixe.” Sure, they guarantee you have the right to transfer it, but your domain property is still not your property. This may never be a problem, but it’s still a concern to consider.

I had both a bad and good experience with transferring domains away from Arvixe. I transferred one domain without issue, but my other, the free domain, encountered multiple problems and took days to solve.

First, they won’t give you the transfer authorization code for the free domain on the website. I emailed support requesting this code and was met with the reply that I had to “unfree” my domain by paying them $10 before they would release it to me. I promptly engaged their QA department about this questionable policy and was issued an apology and sent an authorization code.

However, upon trying to use that code, I was told it was invalid. I contacted support again and they contacted the registrar (Enom) to request a new code. I was sent another code and again told it was invalid. After asking about this new code I was told that it indeed was the code received from Enom and to check with the receiving registrar (Gandi) about a problem on their side. Gandi investigated and believed the problem really was an invalid code.

At this point I contacted Enom directly to ask about the code. Since the domain was registered through Arvixe, they wouldn’t tell me anything, but I did convince him to confirm that the code I had was not valid. This is what I meant above in the cons that you don’t have access to your domain. I contacted Arvixe yet again and explained what I knew and asked that they have Enom send me the authorization code directly to avoid any issues in the middle. Dealing with a middle man for something like this is rarely a good experience.

This time I received a different variation of the same code, but with one of the characters HTML encoded. Incredibly, this authorization code actually worked. It seems Arvixe decoded those odd characters into a single character before sending it to me. I explained to Arvixe what the problem was for the sake of future customers, so hopefully this was just an oddity with my domain. I have no idea why Enom sent such a stupid character set in their authorization code, but at least that is resolved. I did receive good and prompt responses from Arvixe the whole time, so that was in their favor.

Really, for the money I was happy with Arvixe as a host. My main complaint was with the quality and reliability of the control panel. In the end I just wanted to limit my costs and get on a platform that was rock solid. FunnelWeb wasn’t going anywhere, BlogEngine.net had given me concerns, and as I said in the previous post, I was looking to host it myself. I just ended up liking what Gandi had to offer so I thought I’d give them a try.

Look for a Gandi review coming soon!

**Update:** When going to cancel my actual hosting account, I first simply removed my billing information. However, that only caused them to email me constant notices that my hosting account was past due. So I went ahead and emailed them (the only way you can cancel) and was greeted with a nice offer to correct any grievances prior to canceling. I explained my situation and requested to have my service canceled.

At this point, I received an email asking me to send the last 4 characters of my billing password and again received the statement about having to pay to get my domain name (which fortunately I had already transferred). As I said above, I take issue with this policy of holding domains hostage behind a fee. I would be fine if their policy was to charge a fee for free domains transferred within 60 days of registration or something, but at some point the free domain benefit should not come with any strings attached.

The password request raises some flags. First, if they are asking for a password (or a portion of it) that must mean they have the ability to compare what you give them with your actual password. This indicates they are either not encrypting passwords at all or encrypting them with reversible encryption rather than hashing. Either way, this is not a good password storage policy. The other problem with this cancellation process is it’s simply not a good customer experience. People are rightfully wary about handing out any portion of their password over email. A much better approach would be to have a cancellation request form in their authenticated billing management site. Better yet, let people cancel without having to submit a request.

Arvixe might provide a good value, but it’s not a polished experience. For low-cost hosting, I guess these might be reasonable trade-offs, but it would make me think twice about using Arvixe in the future. Not that I wouldn’t still potentially use them.

**Update 2:** I raised the concerns above to Arvixe and they responded to each. They indicated that they only store the last 4 characters of the password that way (which sounds fishy to me, but that’s speculation). They also indicated that the domain fee was only if I wanted to continue the domain registration after it expires, which again, sounds fishy, especially considering the email I received when trying to transfer it and their published terms and conditions on the subject. Regardless of my own speculation on these topics, I wanted to update this post to give that little bit of information from them.

