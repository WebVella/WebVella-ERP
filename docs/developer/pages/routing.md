<!--{"sort_order":2, "name": "routing", "label": "Routing"}-->

# Page Routing Convention

In the following table is presented the general routing convention for the ERP system generated pages. All routes except the `login` require authentication.

**Note**: If `PageName` is not defined the system will sort and present the corresponding page type with the least weight

| page type | routing |
|---------|-------|
| login page | `/login` |
| logout page | `/logout` |
| home page | `/` |
| site page | `/s/{PageName?}` |
| application home page | `/{AppName}/a/{PageName?}` |
| application page | `/{AppName}/{AreaName}/{NodeName}/a/{PageName?}` |
| record list | `/{AppName}/{AreaName}/{NodeName}/l/{PageName?}`|
| record details | `/{AppName}/{AreaName}/{NodeName}/r/{RecordId}/{PageName?}`|
| record create | `/{AppName}/{AreaName}/{NodeName}/c/{PageName?}`|
| record manage | `/{AppName}/{AreaName}/{NodeName}/m/{RecordId}/{PageName?}`|
| record related records list | `/{AppName}/{AreaName}/{NodeName}/r/{RecordId}/rl/{RelationId}/l/{PageName?}`|
| record related records details | `/{AppName}/{AreaName}/{NodeName}/r/{RecordId}/rl/{RelationId}/r/{RelatedRecordId}/{PageName?}`|
| record related record create | `/{AppName}/{AreaName}/{NodeName}/r/{RecordId}/rl/{RelationId}/c/{PageName?}`|
| record related record manage | `/{AppName}/{AreaName}/{NodeName}/r/{RecordId}/rl/{RelationId}/m/{RelatedRecordId}/{PageName?}`|


Often is needed a certain page to be rendered on a specific URL that is not following the above convention. You can do this by using the "Custom body" page functionality which provides you with the option to completely replace the generated page and includes the usage of the `@page` attribute. With its help you can present this page in all route scenarios that may occur.

Another way is by using a static page files in a plugin.