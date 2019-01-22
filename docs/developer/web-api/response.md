<!--{"sort_order":1, "name": "response", "label": "Response Format"}-->
# Web API Response Format

All responses are in JSON formatted in a specific way.

## Responding to GET a single entity record

```json
{
  "success": true,
  "message": "Nisi proident tempor cillum sint duis eu elit dolor Lorem amet qui officia occaecat.",
  "timestamp": "2014-03-03T23:20:23Z",
  "errors": [],
  "object": {
    "id": 1,
  }
}
```

## Returning to GET a list of objects

```json
{
  "success": true,
  "message": "Nisi proident tempor cillum sint duis eu elit dolor Lorem amet qui officia occaecat.",
  "timestamp": "2014-03-03T23:20:23Z",
  "errors": [],
  "object": [
	{
		"id": 1,
	},
	{
		"id": 1,
	}
  ]
}
```

## Returning to POST,PUT,DELETE an object

```json
{
  "success": false,
  "message": "Nisi proident tempor cillum sint duis eu elit dolor Lorem amet qui officia occaecat.",
  "timestamp": "2014-03-03T23:20:23Z",
  "errors": [
    {
      "key": "url",
      "value": "",
      "message": "URL cannot be blank"
    }  
  ],
  "object": {
		"id": 1,
	}
}
```

## Properties

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `errors`                      | *object type*: `List<ErrorModel>`                         
|                               |         
|                               | *default value*: ``
|                               |                                   
|                               | list of error objects returned during the method execution. It is empty when no errors are reported. The object format is:
|                               | * key - the property name, if any, which validation or execution returned an error
|                               | * value - the property value, that causes the problem       
|                               | * message - human readable message of the error       
+-------------------------------+-----------------------------------+
| `message`                     | *object type*: `string`                         
|                               |         
|                               | *default value*: `Success`
|                               |                                   
|                               | Method execution result in human readable form. Often provided to the end-user as a feedback
+-------------------------------+-----------------------------------+
| `object`                      | *object type*: `object`                         
|                               |         
|                               | *default value*: `Success`
|                               |                                   
|                               | The object returned by the method.
+-------------------------------+-----------------------------------+
| `success`                     | *object type*: `bool`                         
|                               |         
|                               | *default value*: `true`
|                               |                                   
|                               | Whether the method execution is successfully completed
+-------------------------------+-----------------------------------+
| `timestamp`                   | *object type*: `DateTime`                         
|                               |         
|                               | *default value*: ``
|                               |                                   
|                               | when the method was executed in ISO 8601 date string and UTC time zone
+-------------------------------+-----------------------------------+
