<!--{"sort_order":10, "name": "record-manage", "label": "Record manage"}-->
# Record manage

This page type is used for record manage pages. Its url is like `/{AppName}/{AreaName}/{NodeName}/m/{RecordId}/{PageName?}`. If a `PageName` is not provided, the system will automatically open the record manage page that is connected to the selected entity by the node and has the lowest page sort order.

There is one variation of this page, when you need to manage the record as related to another parent record, from the same or another entity. We call this page "record related record manage" and it is accessible on an url like: `/{AppName}/{AreaName}/{NodeName}/r/{RecordId}/rl/{RelationId}/m/{RelatedRecordId}/{PageName?}`