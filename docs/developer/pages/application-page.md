<!--{"sort_order":6, "name": "application-page", "label": "Application page"}-->
# Application page

Application pages are meant to present data that is not tightly connected to a single entity such as dashboards in example. They can be two kinds of application pages based on whether or not they are connected to an application sitemap node.

## Application home page

When a page is not connected to a sitemap node, its url will look like this `/{AppName}/a/{PageName?}`. If the url is opened without the `PageName`, the system will automatically redirect to an application home page with the lowest sort weight.

## Application node page

When a page is connected to a sitemap node it is part of the application's sitemap, which is influencing its url: `	/{AppName}/{AreaName}/{NodeName}/a/{PageName?}`. Again, if the url is opened without the `PageName`, the system will automatically redirect to an application page that is connected to the current node with lowest sort weight.