<!--{"sort_order":2, "name": "eql", "label": "EQL Syntax"}-->
# EQL Syntax for database query

WebVella ERP support entity query language (EQL) for retrieving data from database. 
To make easier understanding of that topic, lets assume we have the following imaginary 
entity structures and relation between them. 

Entity: `demo_customer`

|id|name|contact|
|---|---|---|---|---|---|
|9e1c2d3c-8ce4-4c8f-a651-e54421baa09c| Alfreds Futterkiste | Maria Anders  |
|fc1ca2ea-f3d6-4bf5-8853-bd4a44bf37f9| Alessandro Moratti  | Alessandro Moratti |
|d9b17096-dfa1-4a8d-a7f6-00b74ee329f6| Antonio Moreno Taquería | Antonio Moreno |
|a919418d-bc36-4673-9f2b-839ae6ac8e38| Around the Horn  |  Thomas Hardy |


Entity: `demo_address`

|id|customer_id|address|city|country|
|---|---|---|---|---|---|---|
|2c104d0f-d40f-48ee-8964-33265176a70c|9e1c2d3c-8ce4-4c8f-a651-e54421baa09c| Tauentzienstrasse 98 | Berlin | Germany |
|65349ebd-f6ac-4612-a127-f32ebe4f23fd|9e1c2d3c-8ce4-4c8f-a651-e54421baa09c| Anry Barbuys 12 | Plovdiv | Bulgaria |
|86344903-513b-4fb3-a14e-9931083c2bd6|fc1ca2ea-f3d6-4bf5-8853-bd4a44bf37f9| Viale Cassala 1  |  Milano | Italy |
|81991d8b-32f2-4d5d-9854-55407cd03026|d9b17096-dfa1-4a8d-a7f6-00b74ee329f6| Mataderos 2312  | México D.F.  | Mexico |
|0ecaa5f0-c846-4d33-9ba0-ad032205511e|a919418d-bc36-4673-9f2b-839ae6ac8e38| 120 Hanover Sq.  |  London | UK |

Entity relation: `customer_1n_address`

This is one to many relation between both entities on field `demo_customer.id` and `demo_address.customer_id`


## The SELECT Statement
The select statement is used to select data from ERP entities. Its syntax is very similar to the SQL Select statement, 
so for developers with SQL knowledge will be very easy to use it. 

```SQL
SELECT field1,field2,...
FROM entity
```
Here, `field1, field2` .. are the field names of the entity you want to select from. if you want to select all the fields available in the `entity`, 
use the following syntax:
```SQL
SELECT * FROM entity
```
 
 ### Example
 
 The following EQL statement selects the only `name` field from `demo_customer` entity.
  ```sql
SELECT name FROM demo_customer
```

The result from eql select queries will be list of EntityRecord objects. Each EntityRecord property can be a List of EntityRecords.
So the result of eql execution is a *tree* structure and cannot be represented by table printing (like sql results).
That's why we will print result as serialized to json list of objects. 

The execution of the specified above query produces the following result:

```json
[
	{
		"id" : "9e1c2d3c-8ce4-4c8f-a651-e54421baa09c",
		"name": "Alfreds Futterkiste"
	},
	{
		"id" : "fc1ca2ea-f3d6-4bf5-8853-bd4a44bf37f9",
		"name": "Alessandro Moratti"
	},
	{
		"id" : "d9b17096-dfa1-4a8d-a7f6-00b74ee329f6",
		"name": "Antonio Moreno Taquería"
	},
	{
		"id" : "a919418d-bc36-4673-9f2b-839ae6ac8e38",
		"name": "Around the Horn", 
	}
] 
```
You probably notice that we wanted to get only `name` field from entity demo_customer, but result contains 2 fields (`id` field included). 
Thats because, internally ERP always need the `id` field and adds it to select query. 
 
 Unlike sql, eql select doesn't support joins syntax. 
 We use relations to include related records in each record. 
 Relations are used by their name and $ character decoration in front of the name (`$relation_name`).

```SQL
SELECT field1,field2, $relation_name.fieldX1, $relation_name.fieldX2
FROM entity
```

The result will look like:

```json
[
	{
		"id": "...",
		"field1": "...",
		"field2": "...",
		"$relation_name": 
			[
				{	"id": "...", 
					"fieldX1" : "...",
					"fieldX2" : "..." 
				},
				{	"id": "...", 
					"fieldX1" : "...",
					"fieldX2" : "..." 
				},
				...
			]
	},
	...
]
```

Here we select `fields1,field2` from entity and in each entity record in the result we have an additional property with name `$relation_name` which is a list of entity records. 
That property contains the list of records, related to upper record, using the relation rules. 

Using more real looking data from structure specified above will make sample more simple to perception. 
So lets say we want to select all customers with their addresses. The eql select statement looks like that:

```SQL
SELECT *, $customer_1n_address.*
FROM demo_customer
```

and will produce following result: 

```json
[
	{
		"id" : "9e1c2d3c-8ce4-4c8f-a651-e54421baa09c",
		"name": "Alfreds Futterkiste",
		"contact": "Maria Anders",
		"$customer_1n_address": 
		[
			{	
				"id": "2c104d0f-d40f-48ee-8964-33265176a70c",
				"customer_id": "9e1c2d3c-8ce4-4c8f-a651-e54421baa09c",
				"address": "Tauentzienstrasse 98",
				"city": "Berlin",
				"country": "Germany"
			},
			{	
				"id": "65349ebd-f6ac-4612-a127-f32ebe4f23fd",
				"customer_id": "9e1c2d3c-8ce4-4c8f-a651-e54421baa09c",
				"address": "Anry Barbuys 12",
				"city": "Plovdiv",
				"country": "Bulgaria"
			}
		]
	},
	{
		"id" : "fc1ca2ea-f3d6-4bf5-8853-bd4a44bf37f9",
		"name": "Alessandro Moratti",
		"contact": "Alessandro Moratti",
		"$customer_1n_address": 
		[
			{	
				"id": "86344903-513b-4fb3-a14e-9931083c2bd6",
				"customer_id": "fc1ca2ea-f3d6-4bf5-8853-bd4a44bf37f9",
				"address": "Viale Cassala 1",
				"city": "Milano",
				"country": "Italy"
			}
		]
	},
	{
		"id" : "d9b17096-dfa1-4a8d-a7f6-00b74ee329f6",
		"name": "Antonio Moreno Taquería",
		"contact": "Antonio Moreno",
		"$customer_1n_address": 
		[
			{	
				"id": "81991d8b-32f2-4d5d-9854-55407cd03026",
				"customer_id": "d9b17096-dfa1-4a8d-a7f6-00b74ee329f6",
				"address": "Mataderos 2312",
				"city": "México D.F.",
				"country": "Mexico"
			}
		]
	},
	{
		"id" : "a919418d-bc36-4673-9f2b-839ae6ac8e38",
		"name": "Around the Horn", 
		"contact" : "Thomas Hardy",
		"$customer_1n_address": 
		[
			{	
				"id": "0ecaa5f0-c846-4d33-9ba0-ad032205511e",
				"customer_id": "a919418d-bc36-4673-9f2b-839ae6ac8e38",
				"address": "120 Hanover Sq.",
				"city": "London",
				"country": "UK"
			}
		]
	}
] 
```

The erp translate eql to sql ,with joins inside, according specified relation direction, fields and entities. Sql joins direction are automatically 
recognized by entity of the upper record. Only one specific case exists when direction should be specified - when a relation from one entity to same entity is used.
Then from origin to target direction is automatically used. In order to switch direction of the generated sql join use double $ with the relation name 
(e.g. `$$relation_name`).

Eql also supports multiple, connected through, relations. Here is an example

```SQL
SELECT *, 
	$relation1.fieldX1, $relation1.fieldX2, ...
	$relation.$relation2.fieldY1,$relation.$relation2.fieldY2...
FROM entity
```

Or if you want all field and extend with 3-rd relation

```SQL
SELECT *, $relation1.*, $relation.$relation2.*, $relation.$relation2.$relation3.*
FROM entity

```

Last query will result in something like that:

```json
[
	{
		"id": "...",
		"$relation1": 
			[
				{	
					"id": "...", 
					"$relation2" :  
						[
							{
								"id": "...", 
								"$relation3" :  
								[
									{
										"id": "...", 
										...
									}
									...
								]
								...							
							}
							...
						],
					...
				}
				...
			],
		...
	}
	...
]
```

## The WHERE Clause
The WHERE clause is used to filter records. The WHERE clause is used to extract only those records that fulfill a specified condition.
```SQL
SELECT field1,field2,...
FROM entity
WHERE condition
```

the following statement select all the customers with `contact` equals to 'Thomas Hardy', in the entity `demo_customer`
```SQL
SELECT *
FROM demo_customer
WHERE contact = 'Thomas Hardy'
```
Execution of the specified query will produce the following result:

```json
[
	{
		"id" : "a919418d-bc36-4673-9f2b-839ae6ac8e38",
		"name": "Around the Horn", 
		"contact" : "Thomas Hardy"
	}
] 
```

In our ERP application development, mostly parameterized statements are used. Parameters, like in sql, are specified by `@` character in front of their names. 
Here is the same example as previous, where parameter `@contact` replaces the literal 'Thomas Hardy'. The parameter values are provided by 
ERP datasources internally or by using EQL related classes and structures in API to execute queries.
```SQL
SELECT *
FROM demo_customer
WHERE contact = @contact
```

Here is the list of supported where condition operators:

|operator|description|
|---|---|
|=|  returns true, if the field value is **EQUAL** to right side literal, number or parameter |
|>|  returns true, if the field value is **GREATER** than right side literal, number or parameter |
|>=| returns true, if the field value is **GREATER OR EQUAL** than right side literal, number or parameter |
|< | returns true, if the field value is **LOWER** than right side literal, number or parameter |
|<=| returns true, if the field value is **LOWER OR EQUAL** than right side literal, number or parameter |
|<>| returns true, if the field value is **NOT EQUAL** to right side literal, number or parameter |
|!=| returns true, if the field value is **NOT EQUAL** to right side literal, number or parameter |
|AND| logical **AND** compares between two expressions or field value (if boolean) and expression and returns true when both are true.  |
|OR| Logical **OR** compares between two expressions or field value(if boolean) and expression and returns true when one of them is true |
|CONTAINS| if *text* field content contains text (right operand literal or parameter) returns true |
|STARTSWITH| if *text* field content value starts with  text (right operand literal or parameter), returns true|
|~| **case sensitive regex search** operator, which search field value for pattern (provided as right operand) matches, returns true if matches found |
|~*| **case insensitive regex search** operator, which search field value for pattern (provided as right operand) matches, returns true if matches found |
|!~| **case sensitive regex search** operator, which search field value for pattern (provided as right operand) matches, returns true if matches are not found |
|!~*| **case insensitive regex search** operator, which search field value for pattern (provided as right operand) matches, returns true if matches are not found |
|@@| **full-text search**, searches in field value for specified text (right operand) using FTS index. if match found returns true |

All the operators have equal precedence with only Logical AND and OR operators. 
The OR operator have lowest precedence. The logical AND operator have lower precedence than others, but higher than logical OR.
You can use standard braces to control operator precedence. Also the following example shows, how you can use relation in where clause. 
Only note that first level relations can be used only (`$relation1.$relation2`.... not supported). 

```SQL
SELECT *,  $customer_1n_address.*
FROM demo_customer
WHERE ( contact = 'Thomas Hardy'  OR name STARTSWITH 'Around'  ) AND $customer_1n_address.country CONTAINS 'UK'
```

You can use multiple expressions and combine them with logical operators.  In example

```SQL
SELECT *
FROM demo_customer
WHERE ( contact = 'Thomas Hardy' ) AND ( name STARTSWITH 'Around' )
```

In EQL compare to NULL (note its upper case only) is also done with operators `=`, `<>` and `!=`. 
```SQL
SELECT *,  $customer_1n_address.*
FROM demo_customer
WHERE contact = NULL
```


**Important note:** WHERE condition statements can only have a field name as left operand, so unlike SQL, the following code will produce error:
```SQL
SELECT *
FROM demo_customers
WHERE 'Germany' = country 

-- or

SELECT *
FROM demo_customers
WHERE @country = country

```


## The ORDER BY Keyword
The ORDER BY keyword is used to sort the result in ascending or descending order.
The ORDER BY keyword sorts the records in ascending order by default (ASC). 
To sort the records in descending order, use the DESC keyword.
You can also use multiple fields to order by, each by is its own direction. 
The sorting field precedence is from left to right. Left has highest precedence.
Ordering fall to next field sorting, if previous field record values are equal.


```SQL
SELECT *
FROM entity
ORDER BY field1, field2 DESC
```

The eql support parametrized ordering (which is not supported in sql). You can use parameter to specify field name or order direction.

```SQL
SELECT *
FROM entity
ORDER BY @order_field @order_direction
```

and even more customizable with multiple parameters and even field from relation

```SQL
SELECT *
FROM entity
ORDER BY @order_field1 @order_direction1, @order_field2 @order_direction2, $relationX.fieldX DESC ....
```

These parameters are verified during translation from eql to sql.

## The PAGE and PAGESIZE Keyword
These keywords allow you to retrieve just a portion of the records that are generated by the rest of the query.
Both keywords are always used in combination (using only one of them will result in error). Both values should 
be positive integer numbers.

```SQL
SELECT *
FROM entity
PAGE 1 -- number
PAGESIZE 10 --number
```

Keyword PAGE specify which page of records should be returned, while the PAGESIZE specify the
records count in single page. First page return records from 1 to PAGESIZE and so on. 
Similar to ORDER BY keyword, PAGE and PAGESIZE accept parameterized input

```SQL
SELECT *
FROM entity
PAGE @page
PAGESIZE @pagesize
```

Note: When using these keywords, it is important to use an ORDER BY that sorts the result records into a unique order. 
Otherwise you will get an unpredictable subset of the query's records. 
