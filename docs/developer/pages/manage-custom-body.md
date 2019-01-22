<!--{"sort_order":13, "name": "manage-custom-body", "label": "Manage custom body"}-->

# Manage generated body

There are two kinds of page body generation methods. 
* generated body - the body is generated with the help of page components, which you can add, move, put in different containers or one in another. Each page component also has its own set of options you can configure to best fit your needs.
* custom body - when enabled, it will completely replace the rendered page with the razor code template that you provide. You can set you own layout or change which one is used. You can use the platform's tag helpers for quicker templating.

Both methods are managed through the SDK plugin in the page management screen. 

The manage the generated page body you need to:

##### Step 1: Navigate to the page management screen

You can navigate to the page management screen by opening the SDK application > Page List > Page details or by using the shortcut while on the page as presented on the next image.

![sdk page manage shortcut](/doc-images/sdk-page-manage-shortcut.png)


##### Step2: Click on the "custom body" tab

![sdk page custom body](/doc-images/sdk-page-custom-body.png)

Do not forget to start the Razor code with a `@page` declaration, which is required when a custom body is used.
