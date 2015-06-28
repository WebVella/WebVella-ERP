/* area.service.js */

/**
* @desc all actions with site area
*/

(function () {
    'use strict';

    angular
        .module('webvellaRoot')
        .service('webvellaRootService', service);

    service.$inject = ['$http', 'wvAppConstants', '$log', '$rootScope', '$window', '$location', '$anchorScroll', 'ngToast', '$timeout'];

    /* @ngInject */
    function service($http, wvAppConstants, $log, $rootScope, $window, $location, $anchorScroll, ngToast, $timeout) {
        var serviceInstance = this;

        serviceInstance.registerHookListener = registerHookListener;
        serviceInstance.launchHook = launchHook;
        serviceInstance.getEntityRecordsByName = getEntityRecordsByName;
        serviceInstance.setPageTitle = setPageTitle;
        serviceInstance.setBodyColorClass = setBodyColorClass;
        serviceInstance.getSitemap = getSitemap;
        serviceInstance.getSitemapSample = getSitemapSample;
        serviceInstance.toggleSidebar = toggleSidebar;
        serviceInstance.generateValidationMessages = generateValidationMessages;
        serviceInstance.reloadCurrentState = reloadCurrentState;


        ///////////////////////
        function registerHookListener(eventHookName, currentScope, executeOnHookFunction) {
            if (executeOnHookFunction === undefined || typeof (executeOnHookFunction) != "function") {
                $log.error('webvellaRoot>providers>root.service>registerHookListener> result failure: The executeOnHookFunction argument is not a function or missing ');
                alert("The executeOnHookFunction argument is not a function or missing ");
                return;
            }
            //When registering listener with $on, it returns automatically a function that can remove this listener. We will use it later
            var unregisterFunc = $rootScope.$on(eventHookName, function (event, data) {
                executeOnHookFunction(event, data);
            });
            //The listener should be manually removed as the rootScope is never destroyed, and this will lead to duplication the next time the controller is loaded
            currentScope.$on("$destroy", function () {
                unregisterFunc();
            });

            $log.debug('rootScope>events> "' + eventHookName + '" hook registered');
        }

        /////////////////////
        function launchHook(eventHookName, data) {
            $rootScope.$emit(eventHookName, data);
            $log.debug('rootScope>events> "'+ eventHookName + '" emitted');
        }

        ///////////////////////
        function setPageTitle(pageTitle) {
            $log.debug('webvellaRoot>providers>root.service>setPageTitle> function called');
            $rootScope.$emit("application-pageTitle-update", pageTitle);
            $log.debug('rootScope>events> "application-pageTitle-update" emitted');
        }

        //////////////////////
        function setBodyColorClass(color) {
            $log.debug('webvellaRoot>providers>root.service>setBodyColorClass> function called');
            $rootScope.$emit("application-body-color-update", color);
            $log.debug('rootScope>events> "application-body-color-update" emitted');
        }

        //////////////////////
        function toggleSidebar() {
            $log.debug('webvellaRoot>providers>root.service>setBodyColorClass> function called');
            $rootScope.$emit("application-sidebar-mini-toggle");
            $log.debug('rootScope>events> "application-body-color-update" emitted');
        }

        ////////////////////
        function getEntityRecordsByName(entityName,successCallback, errorCallback) {
            $log.debug('webvellaRoot>providers>root.service>getEntityRecords> function called');
            $http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/list' }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }

        ///////////////////
        function generateValidationMessages(response, scopeObj, formObject, location) {
            $log.debug('webvellaRoot>providers>root.service>generateValidationMessages> function called');
            //Fill in validationError boolean and message for each field according to the template
            // scopeDate.fieldNameError => boolean; scopeDate.fieldNameMessage => the error from the api; 
            for (var i = 0; i < response.errors.length; i++) {
                scopeObj[response.errors[i].key + "Message"] = response.errors[i].message;
                scopeObj[response.errors[i].key + "Error"] = true;
            }
            //Rebind the form with the data returned from the server
            formObject = response.object;
            //Notify with a toast about the error and show the server response.message
            ngToast.create({
                className: 'error',
                content: '<h4>Error</h4><p>' + response.message +'</p>'
            });
            //Scroll top
            // set the location.hash to the id of
            // the element you wish to scroll to.
            location.hash('modal-top');

            // call $anchorScroll()
            $anchorScroll();
        }

        //////////////////
        function reloadCurrentState(state,params) {
        	$log.debug('webvellaRoot>providers>root.service>reloadCurrentState> function called');

            $timeout(function () {
        	    state.go(state.current, params, { reload: true });
            }, 0);
        }

    	////////////////////
        function getSitemap(successCallback, errorCallback) {
        	$log.debug('webvellaRoot>providers>root.service>getAreaEntities> function called');
        	$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'sitemap' }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }

        function getSitemapSample(successCallback, errorCallback) {
        	var response = {
        		"object": {
        			"fieldsMeta": null,
        			"data": [
						{
							"id": "37eebe3e-c5b0-4079-8844-2f9e1604a035",
							"name": "sales",
							"label": "Sales",
							"color": "green",
							"icon_name": "money",
							"weight": 5.0,
							"roles": "[]",
							"$areas_area_relation": [{"entity_id": "8e1e6b37-d840-458e-afc9-51474c463fec"},{"entity_id": "c10e40a0-3520-41f5-bdb6-ec05f76194c5"}],
							"entities": [
								{
									"id": "8e1e6b37-d840-458e-afc9-51474c463fec",
									"name": "document",
									"label": "Document",
									"label_plural": "Documents",
									"icon_name": "database",
									"weight": 100.0,
									"lists": [],
									"views": null
								},
								{ 
									"id": "c10e40a0-3520-41f5-bdb6-ec05f76194c5",
									"name": "customer",
									"label": "Customer",
									"label_plural": "Customers",
									"icon_name": "users",
									"weight": 1.0,
									"lists": [],
									"views": null
								}]
						},
						{
							"id": "9d6203bf-db07-4f54-835c-15ed4c167fce",
							"name": "logistics",
							"label": "Logistics",
							"color": "brown",
							"icon_name": "truck",
							"weight": 3.0,
							"roles": "[]",
							"$areas_area_relation": [{ "entity_id": "90a8c644-fad9-4254-8177-d7d83b262426" }],
							"entities": [
									{
										"id": "90a8c644-fad9-4254-8177-d7d83b262426",
										"name": "software",
										"label": "Software",
										"label_plural": "Softwares",
										"icon_name": "database",
										"weight": 1.0,
										"lists": [
											{
												"id": "7937a4a3-e074-4e2f-aca2-1467a29bb433",
												"name": "recent_orders",
												"label": "Recent Orders",
												"default": true,
												"system": true,
												"weight": 1,
												"type": "general",
												"cssClass": "",
												"recordsLimit": 100,
												"pageSize": 10,
												"columns": [{
													"_t": "RecordViewFieldItem",
													"type": "field",
													"fieldId": "48818fa7-77b4-cedd-71e4-80e106038abf",
													"fieldName": "username",
													"fieldLabel": "Username",
													"fieldTypeId": 18
												},
															{
																"_t": "RecordViewRelationFieldItem",
																"type": "fieldFromRelation",
																"relationId": "48818fa7-77b4-cedd-71e4-80e106038ab1",
																"entityId": "48818fa7-77b4-cedd-71e4-80e106038ab2",
																"entityName": "account",
																"entityLabel": "Account",
																"fieldId": "48818fa7-77b4-cedd-71e4-80e106038ab3",
																"fieldName": "email",
																"fieldLabel": "Email",
																"fieldTypeId": 18
															}],
												"query": {
													"queryType": "AND",
													"fieldName": "",
													"fieldValue": "",
													"subQueries": [
														{
															"queryType": "EQ",
															"fieldName": "username",
															"fieldValue": "mozart",
															"subQueries": []
														},
														{
															"queryType": "CONTAINS",
															"fieldName": "email",
															"fieldValue": "domain.com",
															"subQueries": []
														}
													]
												},
												"sorts": [
													{
														"fieldName": "username",
														"sortType": "Descending"
													}
												]
											}
										],
										"views": null
									}]
						}]
        		}, "timestamp": "0001-01-01T00:00:00", "success": true, "message": "Query successfully executed", "errors": []
        	}
        	handleSuccessResult(response, 200, successCallback, errorCallback)
        }


        //// Aux methods //////////////////////////////////////////////////////

        // Global functions for result handling for all methods of this service
        function handleErrorResult(data, status, errorCallback) {
            switch (status) {
                case 400:
                    if (errorCallback === undefined || typeof (errorCallback) != "function") {
                        $log.debug('webvellaRoot>providers>root.service> result failure: errorCallback not a function or missing ');
                        alert("The errorCallback argument is not a function or missing");
                        return;
                    }
                    data.success = false;
                    errorCallback(data);
                    break;
                default:
                    $log.debug('webvellaRoot>providers>root.service> result failure: API finished with error: ' + status);
                    alert("An API call finished with error: " + status);
                    break;
            }
        }

        function handleSuccessResult(data, status, successCallback, errorCallback) {
            if (successCallback === undefined || typeof (successCallback) != "function") {
                $log.debug('webvellaRoot>providers>root.service> result failure: successCallback not a function or missing ');
                alert("The successCallback argument is not a function or missing");
                return;
            }

            if (!data.success) {
                //when the validation errors occurred
                if (errorCallback === undefined || typeof (errorCallback) != "function") {
                    $log.debug('webvellaRoot>providers>root.service> result failure: errorCallback not a function or missing ');
                    alert("The errorCallback argument in handleSuccessResult is not a function or missing");
                    return;
                }
                errorCallback(data);
            }
            else {
                 $log.debug('webvellaRoot>providers>root.service> result success: get object ');
                successCallback(data);
            }
        }

    }
})();