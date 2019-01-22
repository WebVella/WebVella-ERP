<!--{"sort_order":1, "name": "overview", "label": "Overview"}-->
# Overview

The Web API gives you access to the content management features you see in your web application and lets you extend them for use in your own applications. It is a RESTful and is organized around the content types and functionalities, of which you are familiar with in the WebVella ERP software.

The WebVella ERP Web API is work in progress and we will gradually implement all available features.

## Date Format

All dates (both those sent in requests and those returned in responses) should be formatted as presented in the examples. We support dates formatted ISO 8601 String. All dates are and should be in UTC time zone. (ex. "2013-02-04T22:44:30.652Z")

## CORS

CORS, or cross-origin resource sharing, is a way to make XMLHttpRequests to another domain, different from the one that the script is loaded from. CORS is supported in most modern browsers.

## API changes

All extensions of the API will be added only to the latest supported version. Bug fixes and optimizations will be applied to all relevant API versions. The API version is part of the base URL, so you will be able to choose which of API version you use for each of your requests

## API Base URL

You can make you RESTful requests by adding to your HiveSocial install domain the API path, based on the API version, locale, content item and methods. It should look like similarly to the following example:

```http
https://<YOUR_DOMAIN>/api/v3/en_US/meta/relation
```
**IMPORTANT:** Secure certificate (https) is recommendable for the WebVella Erp Web API

## Authorization

In order to provide the same level of security that we provide on our web software, many API requests are requiring authorization. This is done by a authorization cookie.