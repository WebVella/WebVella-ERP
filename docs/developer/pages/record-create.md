<!--{"sort_order":9, "name": "record-create", "label": "Record create"}-->
# Record create

This page type is used for record create pages. Its url is like `/{AppName}/{AreaName}/{NodeName}/c/{PageName?}`. If a `PageName` is not provided, the system will automatically open the record create page that is connected to the selected entity by the node and has the lowest page sort order.

There is one variation of this page, when you need to create the record as related to another parent record, from the same or another entity. We call this page "record related record create" and it is accessible on an url like: `/{AppName}/{AreaName}/{NodeName}/r/{RecordId}/rl/{RelationId}/c/{PageName?}`

