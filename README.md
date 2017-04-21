# RssProxy
Outlook seems to have issues fetching RSS feeds via https. So I wrote this local proxy service that serves RSS feeds via http instead of https.

The InstallService.ps1 PowerShell script shows how to (un)install the service to listen for requests on `http://localhost:8080`.

Afterwards, just point Outlook at `http://localhost:8080/blogs.msdn.microsoft.com/oldnewthing/feed` to get the RSS feed hosted at https://blogs.msdn.microsoft.com/oldnewthing/feed, for example.
