/* area.service.js */

/**
* @desc all actions with site area
*/

(function () {
    'use strict';

    angular
        .module('webvellaAreas')
        .service('webvellaAreasService', service);

    service.$inject = ['$log','$http', 'wvAppConstants','$timeout'];

    /* @ngInject */
    function service($log, $http, wvAppConstants, $timeout) {
        var serviceInstance = this;

        serviceInstance.getAreaByName = getAreaByName;
        serviceInstance.getCurrentAreaFromSitemap = getCurrentAreaFromSitemap;
        serviceInstance.getCurrentEntityFromArea = getCurrentEntityFromArea;
        serviceInstance.getViewMetaByName = getViewMetaByName;
        serviceInstance.getEntityRecord = getEntityRecord;
        serviceInstance.createEntityRecord = createEntityRecord;
        serviceInstance.getListRecords = getListRecords;
        ///////////////////////
        function getAreaByName(areaName, successCallback, errorCallback) {
            $log.debug('webvellaAreas>providers>areas.service>getAreaByName> function called');
            $http({ method: 'GET', url: wvAppConstants.apiBaseUrl + '/meta/entity/' + areaName }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }

		///////////////////////
        function getCurrentAreaFromSitemap(areaName, sitemap) {
        	var currentArea = {};

        	for (var i = 0; i < sitemap.length; i++) {
        		if (sitemap[i].name == areaName) {
        			currentArea = sitemap[i];
        		}
        	}
        	currentArea.entities.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });
        	return currentArea;
        }

    	///////////////////////
        function getCurrentEntityFromArea(entityName, area) {
        	var currentEntity = {};

        	for (var i = 0; i < area.entities.length; i++) {
        		if (area.entities[i].name == entityName) {
        			currentEntity = area.entities[i];
        		}
        	}

        	return currentEntity;
        }


        ///////////////////////
        function getViewMetaByName(viewName, entityName, successCallback, errorCallback) {
            $log.debug('webvellaAreas>providers>areas.service>getViewMetaByName> function called');
            var response = {};
            response.success = true;
            response.message = "success";

            var view = {
            	"id": "7937a4a3-e074-4e2f-aca2-1467a29bb433",
            	"name": "details",
            	"label": "Details",
            	"default": true,
            	"system": true,
            	"weight": 1,
            	"cssClass": "",
            	"type": "general",
            	"regions": [
                    {
                    	"name": "content",
                    	"render": false,
                    	"cssClass": "",
                    	"sections": [
							 {
							 	"id": "48818fa7-77b4-cedd-71e4-80e106038abf",
							 	"name": "general",
							 	"label": "General",
							 	"cssClass": "",
							 	"showLabel": true,
							 	"collapsed": false,
							 	"weight": 1,
							 	"tabOrder": "left-right",
							 	"rows": [
									{
										"id": "48818fa7-77b4-cedd-71e4-80e106038abf",
										"weight": 1,
										"columns": [
									{
										"gridColCount": 6,
										"items": [
										{
											"_t": "RecordViewFieldItem",
											"fieldId": "48818fa7-77b4-cedd-71e4-80e106038abf",
											"type": "field",
											"fieldName": "username",
											"fieldLabel": "Username",
											"fieldTypeId": 18
										}
										]
									},
										{
											"gridColCount": 6,
											"items": [
											{
												"_t": "RecordViewFieldItem",
												"fieldId": "48818fa7-77b4-cedd-71e4-80e106038abf",
												"type": "field",
												"fieldName": "title",
												"fieldLabel": "Title",
												"fieldTypeId": 18
											}
											]
										}
										]
									}
							 	]
							 }
                    	],
                    },
                    {
                    	"render": false,
                    	"cssClass": "",
                    	"lists": [
						  {
						  	"entityId": "48818fa7-77b4-cedd-71e4-80e106038abf",
						  	"listId": "48818fa7-77b4-cedd-71e4-80e106038abf",
						  	"relationId": "48818fa7-77b4-cedd-71e4-80e106038abf"
						  }
                    	]
                    }
            	],

            };
            response.object = view;
            handleSuccessResult(response, status, successCallback, errorCallback);
        }

        ///////////////////////
        function getEntityRecord(recordId, entityName, successCallback, errorCallback) {
            $log.debug('webvellaAreas>providers>areas.service>getViewMetaByName> function called');
            var response = {};
            response.success = true;
            response.message = "success";
            response.object = {
                "fieldsMeta": [
                  {
                      "fieldType": 16,
                      "defaultValue": "00000000-0000-0000-0000-000000000000",
                      "generateNewId": true,
                      "id": "3453795a-438f-4308-879f-d2f49c83e652",
                      "name": "id",
                      "label": "Id",
                      "placeholderText": "",
                      "description": "",
                      "helpText": "",
                      "required": true,
                      "unique": true,
                      "searchable": false,
                      "auditable": false,
                      "system": true
                  },
                  {
                      "fieldType": 18,
                      "defaultValue": "",
                      "maxLength": null,
                      "id": "60b9dfe5-6dc7-4445-9481-83a1655b9a60",
                      "name": "title",
                      "label": "Title",
                      "placeholderText": null,
                      "description": null,
                      "helpText": null,
                      "required": true,
                      "unique": false,
                      "searchable": false,
                      "auditable": false,
                      "system": false
                  },
                  {
                      "fieldType": 18,
                      "defaultValue": "",
                      "maxLength": null,
                      "id": "37964efb-f66b-4be9-b8ce-2820054946a6",
                      "name": "content",
                      "label": "Content",
                      "placeholderText": null,
                      "description": null,
                      "helpText": null,
                      "required": false,
                      "unique": false,
                      "searchable": false,
                      "auditable": false,
                      "system": false
                  },
                  {
                      "relationFields": [
                        {
                            "fieldType": 16,
                            "defaultValue": "00000000-0000-0000-0000-000000000000",
                            "generateNewId": true,
                            "id": "287ee4a4-41dc-4193-a27f-336b654a69f7",
                            "name": "id",
                            "label": "Id",
                            "placeholderText": "",
                            "description": "",
                            "helpText": "",
                            "required": true,
                            "unique": true,
                            "searchable": false,
                            "auditable": false,
                            "system": true
                        },
                        {
                            "fieldType": 18,
                            "defaultValue": "",
                            "maxLength": null,
                            "id": "083bd9b0-fac0-4f55-852e-9f73f345af2f",
                            "name": "name",
                            "label": "Name",
                            "placeholderText": null,
                            "description": null,
                            "helpText": null,
                            "required": true,
                            "unique": true,
                            "searchable": false,
                            "auditable": false,
                            "system": false
                        }
                      ],
                      "relation": {
                          "id": "2f7e77fd-a748-4cdd-8906-01bf8a46a664",
                          "name": "query_test_post_author",
                          "label": "Post author",
                          "description": null,
                          "system": false,
                          "relationType": 2,
                          "originEntityId": "59cc62ca-bf44-46f1-bdc8-daa4a538a567",
                          "originFieldId": "287ee4a4-41dc-4193-a27f-336b654a69f7",
                          "targetEntityId": "3fc00d85-7864-464c-8a8e-bfb85f7134e0",
                          "targetFieldId": "e878470b-492e-43af-af87-a137049eef9f"
                      },
                      "defaultValue": null,
                      "generateNewId": true,
                      "id": "e878470b-492e-43af-af87-a137049eef9f",
                      "name": "author",
                      "label": "Author",
                      "placeholderText": null,
                      "description": null,
                      "helpText": null,
                      "required": true,
                      "unique": false,
                      "searchable": false,
                      "auditable": false,
                      "system": false,
                      "fieldType": 16
                  }
                ],
                "data": [
                  {
                      "id": "14fe2908-bc68-4d6a-8bd3-667c20843a18",
                      "title": "post 3 title",
                      "username": "boz",
                      "author": {
                          "id": "031cca7c-1da4-48d3-a8e6-8e9315643b13",
                          "name": "Test author name"
                      }
                  }
                ]
            };
            handleSuccessResult(response, status, successCallback, errorCallback);
        }

        ///////////////////////
        function createEntityRecord(postObject, entityName, successCallback, errorCallback) {
            $log.debug('webvellaAdmin>providers>admin.service>createEntityRecord> function called');
            $http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'record/' + entityName, data: postObject }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }

		/////////////////////
        function getListRecords(listName, entityName,page, successCallback, errorCallback) {
        	$log.debug('webvellaAreas>providers>areas.service>getListRecords> function called');
        	var response = {};
        	response.success = true;
        	response.message = "success";
        	response.object = {
        		"fieldsMeta": [
                  {
                  	"fieldType": 18,
                  	"defaultValue": "",
                  	"maxLength": null,
                  	"id": "60b9dfe5-6dc7-4445-9481-83a1655b9a63",
                  	"name": "id",
                  	"label": "Id",
                  	"placeholderText": null,
                  	"description": null,
                  	"helpText": null,
                  	"required": true,
                  	"unique": false,
                  	"searchable": false,
                  	"auditable": false,
                  	"system": true
                  },
                  {
                  	"fieldType": 18,
                  	"defaultValue": "",
                  	"maxLength": null,
                  	"id": "60b9dfe5-6dc7-4445-9481-83a1655b9a60",
                  	"name": "title",
                  	"label": "Title",
                  	"placeholderText": null,
                  	"description": null,
                  	"helpText": null,
                  	"required": true,
                  	"unique": false,
                  	"searchable": false,
                  	"auditable": false,
                  	"system": false
                  },
                  {
                  	"fieldType": 18,
                  	"defaultValue": "",
                  	"maxLength": null,
                  	"id": "37964efb-f66b-4be9-b8ce-2820054946a6",
                  	"name": "content",
                  	"label": "Content",
                  	"placeholderText": null,
                  	"description": null,
                  	"helpText": null,
                  	"required": false,
                  	"unique": false,
                  	"searchable": false,
                  	"auditable": false,
                  	"system": false
                  }

        		],
        		"data": [
                  {
                  	"id": "47964efb-f66b-4be9-b8ce-2820054946a6",
                  	"username": "boz",
                  	"content": "post 3 content",
                  }
        		]
        	};
        	handleSuccessResult(response, status, successCallback, errorCallback);
        }

        //// Aux methods //////////////////////////////////////////////////////

        // Global functions for result handling for all methods of this service
        function handleErrorResult(data, status, errorCallback) {
            switch (status) {
                case 400:
                    if (errorCallback === undefined || typeof (errorCallback) != "function") {
                        alert("The errorCallback argument is not a function or missing");
                        return;
                    }
                    data.success = false;
                    errorCallback(data);
                    break;
                default:
                    alert("An API call finished with error: " + status);
                    break;
            }
        }

        function handleSuccessResult(data, status, successCallback, errorCallback) {
            if (successCallback === undefined || typeof (successCallback) != "function") {
                $log.debug('webvellaAreas>providers>areas.service>getAreaByName> result failure: successCallback not a function or missing ');
                alert("The successCallback argument is not a function or missing");
                return;
            }

            if (!data.success) {
                //when the validation errors occurred
                if (errorCallback === undefined || typeof (errorCallback) != "function") {
                    $log.debug('webvellaAreas>providers>areas.service>getAreaByName> result failure: errorCallback not a function or missing ');
                    alert("The errorCallback argument in handleSuccessResult is not a function or missing");
                    return;
                }
                errorCallback(data);
            }
            else {
                $log.debug('webvellaAreas>providers>areas.service>getAreaByName> result success: get object ');
                successCallback(data);
            }
        }

    }
})();