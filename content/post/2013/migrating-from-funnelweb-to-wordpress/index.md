---
date: "2013-04-18T15:42:41Z"
description: ""
draft: false
slug: migrating-from-funnelweb-to-wordpress
title: Migrating From FunnelWeb To Wordpress
---


You probably already know that FunnelWeb has no ability to export your posts. However, I have devised a way to export your FunnelWeb data from the database into a WordPress export file format. This then allows you to import your posts directly into WordPress by tricking it into thinking your posts came from another WordPress blog.

The intention of this tutorial is not to get your site perfectly setup and ready for visitors. Its purpose is to get your FunnelWeb data out of FunnelWeb and into WordPress so that your posts are available on the new site. You will still need to work on your new WordPress site to get it visitor-ready.

To follow this tutorial you will need to have direct access to your FunnelWeb database and a free program (or C# programming skills to convert my script into your own program).

### Prerequisites

1. Direct access to your FunnelWeb database. 1. This might be a local copy of a backup from your hosted database.
2. LINQPad ([www.linqpad.net](http://www.linqpad.net))
3. My custom LINQPad script ([download](http://static.codecisions.com/funnelwebtowordpress.zip))

### Step 1 – Database Connection

1. Setup the LINQPad connection to your database by clicking the ‘Add Connection’ link in the top left.
2. Choose the ‘Default (LINQ to SQL)’ data context and click the Next button.
3. Enter your server information and credentials and click the OK button.

### Step 2 – Run The Script

1. Extract the funnelwebtowordpress.zip file you downloaded.
2. In LINQPad, click File->Open and browse to the extracted file and open it.
3. In the LINQPad script window, select the database connection from the ‘Connection’ dropdown.
4. Change the baseUrl value at the top of the script to your website URL.
5. Optional: Determine whether you want the post URLs to be cleaned during the export (see ‘Cleaning Up URLs’ below).
6. Optional: Change the output file path and redirects file path at the top of the script to your chosen locations.
7. Click the green button with the play icon to run the script.
8. When it’s finished, you will see the words ‘Export Complete’ displayed.
9. The export file and the redirects file will be available at the locations specified at the top of the script.

#### Cleaning Up URLs

In my particular case, I had some posts whose URL had the date in the URL, such as “/post/2010/04/18/”. I also had some that were just the post name. I decided that I wanted to only have the post name in my URL, so I modified my script to only include the actual name of the post.

If you do not want the script to perform this cleanup on the post name, set the cleanPostURLs value to false by modifying the line at the top of the script.

`bool cleanPostURLs = false;`

### Importing To WordPress

Now that you have a WordPress formatted export file, follow the [instructions](http://codex.wordpress.org/Importing_Content#WordPress) about import content into WordPress. If I recall correctly, you may need to set the posts to published once you have them ready.

### Redirects

You will want your old site URLs and search results (both posts and tags) to still get visitors to your new site. To do this, you will need the [Quick Page/Post Redirect Plugin](http://wordpress.org/extend/plugins/quick-pagepost-redirect-plugin/) for WordPress. This plugin has the ability to import a list of your redirects, which my script will create for you. If you have chosen to not modify your post URLs, the redirects file will only contain redirects for your FunnelWeb tags to WordPress categories.

Once the plugin is installed, on the admin screen navigation, visit Redirect Options to import the file.

### Finishing Up

**Formatting**

You likely will have some formatting issues depending on the content of your posts. This will vary depending on your content, so I cannot offer a generic solution. I went through my posts one by one until I had straightened out the formatting.

**Attachments**

You also will need to fix any post attachments to use WordPress’s media library and correct your links to those attachments. Again, this will likely be a manual process.

**Comments**

If you have comment spam fitering enabled, you may wish to make sure that none of your comments are marked as spam. On the other hand, if you had old spam comments in your FunnelWeb posts and want them removed, you can tell WordPress to check your comments for spam and it will flag those.

