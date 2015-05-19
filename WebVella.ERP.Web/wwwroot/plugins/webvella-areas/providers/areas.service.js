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
        serviceInstance.getViewMetaByName = getViewMetaByName;
        serviceInstance.getEntityRecord = getEntityRecord;

        ///////////////////////
        function getAreaByName(areaName, successCallback, errorCallback) {
            $log.debug('webvellaAreas>providers>areas.service>getAreaByName> function called');
            $http({ method: 'GET', url: wvAppConstants.apiBaseUrl + '/meta/entity/' + areaName }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }


        ///////////////////////
        function getViewMetaByName(viewName, entityName, successCallback, errorCallback) {
            $log.debug('webvellaAreas>providers>areas.service>getViewMetaByName> function called');
            var response = {};
            response.success = true;
            response.message = "success";
            response.object = {
                "id": guid(),
                "name": "",
                "label": "",
                "default": true,
                "system": false,
                "weight": 1.0,
                "type": "details_general",
                "regions": [
                    {
                        "id": guid(),
                        "name": "content",
                        "sections": [
                            {
                                "id": guid(),
                                "title": "Basic info",
                                "showTitle": true,
                                "collapsed": false,
                                "place": 0,
                                "tabOrder": "left-right",
                                "rows": [
                                    {
                                        "id": guid(),
                                        "place": 1,
                                        "columns": [{
                                            "gridColCount": 12,
                                            "items": [
                                                {
                                                    "id": "60b9dfe5-6dc7-4445-9481-83a1655b9a60",
                                                    "name": "title",
                                                    "originFieldId": null,
                                                    "originFieldName": null,
                                                    "type": "field"
                                                },
                                                 {
                                                     "id": "e878470b-492e-43af-af87-a137049eef9f",
                                                     "name": "author",
                                                     "originFieldId": "083bd9b0-fac0-4f55-852e-9f73f345af2f",
                                                     "originFieldName": "name",
                                                     "type": "field"
                                                 }
                                            ]
                                        }],
                                    }],

                            }],
                    },
                    {
                        "id": null,
                        "name": "sidebar",
                        "sections": []
                    }
                ],

            };
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
                      "content": "post 3 content",
                      "author": {
                          "id": "031cca7c-1da4-48d3-a8e6-8e9315643b13",
                          "name": "Test author name"
                      }
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