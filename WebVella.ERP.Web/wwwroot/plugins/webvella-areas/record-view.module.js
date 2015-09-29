/* entities.module.js */

/**
* @desc this module manages the application home desktop screen
*/

(function () {
	'use strict';

	angular
        .module('webvellaAreas') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAreasRecordViewController', controller)
        .controller('ManageRelationFieldModalController', ManageRelationFieldModalController);

	// Configuration ///////////////////////////////////
	config.$inject = ['$stateProvider'];

	/* @ngInject */
	function config($stateProvider) {
		$stateProvider.state('webvella-areas-record-view', {
			parent: 'webvella-areas-base',
			url: '/:areaName/:entityName/:recordId/:viewName/:auxPageName/:filter/:page',
			views: {
				"topnavView": {
					controller: 'WebVellaAreasTopnavController',
					templateUrl: '/plugins/webvella-areas/topnav.view.html',
					controllerAs: 'topnavData'
				},
				"sidebarView": {
					controller: 'WebVellaAreasRecordViewSidebarController',
					templateUrl: '/plugins/webvella-areas/view-record-sidebar.view.html',
					controllerAs: 'sidebarData'
				},
				"contentView": {
					controller: 'WebVellaAreasRecordViewController',
					templateUrl: '/plugins/webvella-areas/record-view.view.html',
					controllerAs: 'contentData'
				}
			},
			resolve: {
				checkedAccessPermission: checkAccessPermission,
				resolvedCurrentEntityMeta: resolveCurrentEntityMeta,
				resolvedCurrentView: resolveCurrentView,
				resolvedEntityRelationsList: resolveEntityRelationsList
			},
			data: {

			}
		});
	};


	//#region << Run >> //////////////////////////////////////
	run.$inject = ['$log'];
	/* @ngInject */
	function run($log) {
		$log.debug('webvellaAreas>entities> BEGIN module.run ' + moment().format('HH:mm:ss SSSS'));

		$log.debug('webvellaAreas>entities> END module.run ' + moment().format('HH:mm:ss SSSS'));
	};
    //#endregion

	//#region << Resolve Function >> /////////////////////////
	checkAccessPermission.$inject = ['$q', '$log', 'webvellaRootService', '$stateParams', 'resolvedSitemap', 'resolvedCurrentUser', 'ngToast'];
	/* @ngInject */
	function checkAccessPermission($q, $log, webvellaRootService, $stateParams, resolvedSitemap, resolvedCurrentUser, ngToast) {
		$log.debug('webvellaAreas>entities> BEGIN check access permission ' + moment().format('HH:mm:ss SSSS'));
		var defer = $q.defer();
		var messageContent = '<span class="go-red">No access:</span> You do not have access to the <span class="go-red">' +$stateParams.areaName + '</span> area';
		var accessPermission = webvellaRootService.applyAreaAccessPolicy($stateParams.areaName, resolvedCurrentUser, resolvedSitemap);
		if(accessPermission) {
			defer.resolve();
		}
		else {

			ngToast.create({
				className: 'error',
		content: messageContent
		});
	defer.reject("No access");
		}

$log.debug('webvellaAreas>entities> BEGIN check access permission ' +moment().format('HH:mm:ss SSSS'));
return defer.promise;
}

	resolveCurrentView.$inject = ['$q', '$log', 'webvellaAreasService', '$stateParams', '$state', '$timeout'];
	/* @ngInject */
	function resolveCurrentView($q, $log, webvellaAreasService, $stateParams, $state, $timeout) {
		$log.debug('webvellaAdmin>entity-views>resolveCurrentView BEGIN state.resolved ' + moment().format('HH:mm:ss SSSS'));
		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			if (response.object == null) {
				$timeout(function () {
					$state.go("webvella-root-not-found");
				}, 0);
			}
			else {
					defer.resolve(response.object);
			}
		}

		function errorCallback(response) {
			if (response.object == null) {
				$timeout(function () {
					$state.go("webvella-root-not-found");
				}, 0);
			}
			else {
				defer.reject(response.message);
			}
		}

		webvellaAreasService.getViewRecord($stateParams.recordId, $stateParams.viewName, $stateParams.entityName, successCallback, errorCallback);

		// Return
		$log.debug('webvellaAdmin>entity-views>resolveCurrentView END state.resolved ' + moment().format('HH:mm:ss SSSS'));
		return defer.promise;
	}

	resolveCurrentEntityMeta.$inject = ['$q', '$log', '$rootScope', 'webvellaAdminService', '$stateParams', '$state', '$timeout'];
	/* @ngInject */
	function resolveCurrentEntityMeta($q, $log, $rootScope, webvellaAdminService, $stateParams, $state, $timeout) {
		$log.debug('webvellaAdmin>entity-details> BEGIN state.resolved ' + moment().format('HH:mm:ss SSSS'));
		// Initialize
		var defer = $q.defer();
		var entityResponse = null;
		var dynamicViewsSorted = [];
		// Process
		function successRecordCallback(response) {
			if (response.object == null) {
				$timeout(function () {
					$state.go("webvella-root-not-found");
				}, 0);
			}
			else {
				var recordData = response.object.data[0];
				//Find the first matching dynamic view
				for (var k = 0; k < dynamicViewsSorted.length; k++) {
					var splitArray = dynamicViewsSorted[k].name.split("~");
					if (splitArray.length == 3) {
						var areaName = splitArray[0];
						var fieldName = splitArray[1];
						var fieldValue = splitArray[2];
						if (recordData[fieldName].toString() == fieldValue && dynamicViewsSorted[k].name != $stateParams.viewName) {
							var stateParams = {
								areaName: $stateParams.areaName,
								entityName: $stateParams.entityName,
								recordId: $stateParams.recordId,
								viewName: dynamicViewsSorted[k].name,
								auxPageName: $stateParams.auxPageName,
								filter: $stateParams.filter,
								page: $stateParams.page
							}
							$rootScope.$emit("state-change-needed", "webvella-areas-record-view", stateParams);
							//defer.reject();
							break;
						}
					}
				}
				//Return the normal response as there were no matching dynamic views found
				defer.resolve(entityResponse.object);
			}
		}

		// Process
		function successCallback(response) {
			entityResponse = response;
			if (response.object == null) {
				$timeout(function () {
					$state.go("webvella-root-not-found");
				}, 0);
			}
			else {
				//Process dynamic views
				for (var i = 0; i < response.object.recordViews.length; i++) {
					//Check if there are dynamic views defined in this entity.
					if (response.object.recordViews[i].type == 'dynamic') {
						//Check if the designated view area is the current area
						var splitArray = response.object.recordViews[i].name.split("~");
						if (splitArray[0] == $stateParams.areaName) {
							dynamicViewsSorted.push(response.object.recordViews[i]);
						}
					}
				}
				if (dynamicViewsSorted.length > 0) {
					dynamicViewsSorted.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });
					//Get the record
					webvellaAdminService.getRecord($stateParams.recordId,$stateParams.entityName, successRecordCallback, errorCallback);
				}
				else {
					//No dynamic views continue to the default one
					defer.resolve(response.object);
				}
			}
		}

		function errorCallback(response) {
			if (response.object == null) {
				$timeout(function () {
					$state.go("webvella-root-not-found");
				}, 0);
			}
			else {
				defer.reject(response.message);
			}
		}

		webvellaAdminService.getEntityMeta($stateParams.entityName, successCallback, errorCallback);

		// Return
		$log.debug('webvellaAdmin>entity-details> END state.resolved ' + moment().format('HH:mm:ss SSSS'));
		return defer.promise;
	}

	resolveEntityRelationsList.$inject = ['$q', '$log', 'webvellaAdminService', '$stateParams', '$state', '$timeout'];
    /* @ngInject */
	function resolveEntityRelationsList($q, $log, webvellaAdminService, $stateParams, $state, $timeout) {
		$log.debug('webvellaAdmin>entity-details> BEGIN state.resolved ' + moment().format('HH:mm:ss SSSS'));
	    // Initialize
	    var defer = $q.defer();

	    // Process
	    function successCallback(response) {
	        if (response.object == null) {
	            $timeout(function () {
	                $state.go("webvella-root-not-found");
	            }, 0);
	        }
	        else {
	            defer.resolve(response.object);
	        }
	    }

	    function errorCallback(response) {
	        if (response.object == null) {
	            $timeout(function () {
	                $state.go("webvella-root-not-found");
	            }, 0);
	        }
	        else {
	        	defer.reject(response.message);
	        }
	    }

	    webvellaAdminService.getRelationsList(successCallback, errorCallback);

	    // Return
	    $log.debug('webvellaAdmin>entity-details> END state.resolved ' + moment().format('HH:mm:ss SSSS'));
	    return defer.promise;
	}


	//#endregion


	// Controller ///////////////////////////////
	
	function multiplyDecimals(val1, val2, decimalPlaces,$scope) {
		var helpNumber = 100;
		for (var i = 0; i < decimalPlaces; i++) {
			helpNumber = helpNumber * 10;
		}
		var temp1 = $scope.Math.round(val1 * helpNumber);
		var temp2 = $scope.Math.round(val2 * helpNumber);
		return (temp1 * temp2) / (helpNumber * helpNumber);
	}


	controller.$inject = ['$filter', '$modal', '$log', '$q', '$rootScope', '$state', '$stateParams', '$scope','$window', 'pageTitle', 'webvellaRootService', 'webvellaAdminService', 'webvellaAreasService',
        'resolvedSitemap', '$timeout', 'resolvedCurrentView', 'ngToast', 'wvAppConstants', 'resolvedCurrentEntityMeta', 'resolvedEntityRelationsList', 'resolvedCurrentUser'];

	/* @ngInject */
	function controller($filter,$modal, $log,$q, $rootScope, $state,$stateParams, $scope,$window, pageTitle, webvellaRootService, webvellaAdminService,webvellaAreasService,
        resolvedSitemap, $timeout, resolvedCurrentView, ngToast, wvAppConstants, resolvedCurrentEntityMeta, resolvedEntityRelationsList, resolvedCurrentUser) {
		$log.debug('webvellaAreas>entities> BEGIN controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var contentData = this;
		contentData.selectedSidebarPage = {};
		contentData.selectedSidebarPage.label = "";
		contentData.selectedSidebarPage.name = "*";
		contentData.selectedSidebarPage.isView = true;
		contentData.selectedSidebarPage.isEdit = true;
		contentData.selectedSidebarPage.meta = null;
		contentData.selectedSidebarPage.data = null;
		contentData.stateParams = $stateParams;

		//#region <<Set pageTitle>>
		contentData.pageTitle = "Area Entities | " + pageTitle;
		webvellaRootService.setPageTitle(contentData.pageTitle);
		contentData.siteMap = angular.copy(resolvedSitemap);
		contentData.currentArea = null;
		for (var i = 0; i < contentData.siteMap.data.length; i++) {
			if (contentData.siteMap.data[i].name == $state.params.areaName) {
				contentData.currentArea = contentData.siteMap.data[i];
			};
		}
		webvellaRootService.setBodyColorClass(contentData.currentArea.color);
		//#endregion

		//#region << Initialize view and regions>>

		//1. Get the current view
		contentData.defaultRecordView = angular.copy(resolvedCurrentView.meta);

		//2. Load the sidebar
		contentData.sidebarRegion = contentData.defaultRecordView.sidebar;

		//3. Find and load the selected page meta and data
		function getViewOrListMetaAndData(name) {
			var returnObject = {
				data: null,
				meta:null
			};

			if (name === "") {
				returnObject.data = angular.copy(resolvedCurrentView.data[0]);
				for (var i = 0; i < contentData.defaultRecordView.regions.length; i++) {
					if (contentData.defaultRecordView.regions[i].name === "content") {
						returnObject.meta = angular.copy(contentData.defaultRecordView.regions[i]);
					}
				}
				contentData.selectedSidebarPage.isView = true;
				contentData.selectedSidebarPage.isEdit = true;
			} else {
				var selectedDataName = "";
				contentData.selectedSidebarPage.isEdit = false;
				for (var i = 0; i < contentData.defaultRecordView.sidebar.items.length; i++) {
					if (contentData.defaultRecordView.sidebar.items[i].dataName === name) {
						// If in edit mode (view from the current entity) the data should be different -> we need the content region meta, not the view meta as in recursive-view directive
						if (contentData.defaultRecordView.sidebar.items[i].type === "view"
						|| contentData.defaultRecordView.sidebar.items[i].type === "viewFromRelation") {
							for (var j = 0; j < contentData.defaultRecordView.sidebar.items[i].meta.regions.length; j++) {
								if (contentData.defaultRecordView.sidebar.items[i].meta.regions[j].name === "content") {
									returnObject.meta = angular.copy(contentData.defaultRecordView.sidebar.items[i].meta.regions[j]);
								}
							}
						} else {
							returnObject.meta = contentData.defaultRecordView.sidebar.items[i].meta;
						}

						selectedDataName = contentData.defaultRecordView.sidebar.items[i].dataName;
						contentData.selectedSidebarPage.isView = true;
						if (contentData.defaultRecordView.sidebar.items[i].type === "view"
							|| contentData.defaultRecordView.sidebar.items[i].type === "viewFromRelation") {
							contentData.selectedSidebarPage.isEdit = true;
							returnObject.data = angular.copy(resolvedCurrentView.data[0][selectedDataName][0]);
						} else if (contentData.defaultRecordView.sidebar.items[i].type === "list"
							|| contentData.defaultRecordView.sidebar.items[i].type === "listFromRelation") {
							contentData.selectedSidebarPage.isView = false;
							returnObject.data = angular.copy(resolvedCurrentView.data[0][selectedDataName]);
						}
					}
				}
				
			}

			return returnObject;
		};

		var returnedObject = {};

		if ($stateParams.auxPageName === "*") {
			//The default view meta is active
			returnedObject = getViewOrListMetaAndData("");
			contentData.selectedSidebarPage.meta = returnedObject.meta;
			contentData.selectedSidebarPage.data = returnedObject.data;
		}
		else {
			//One of the sidebar view or lists is active
			//Load the data
			returnedObject = getViewOrListMetaAndData($stateParams.auxPageName);
			contentData.selectedSidebarPage.meta = returnedObject.meta;
			contentData.selectedSidebarPage.data = returnedObject.data;
		}

		//#endregion

		//#region << Intialize current entity >>
		contentData.currentEntity = angular.copy(resolvedCurrentEntityMeta);

	    //#endregion

	    //#region << Entity relations functions >>
		contentData.relationsList = angular.copy(resolvedEntityRelationsList);
		contentData.getRelation = function (relationName) {
		    for (var i = 0; i < contentData.relationsList.length; i++) {
		        if (contentData.relationsList[i].name == relationName) {
		            //set current entity role
		            if (contentData.currentEntity.id == contentData.relationsList[i].targetEntityId && contentData.currentEntity.id == contentData.relationsList[i].originEntityId) {
		                contentData.relationsList[i].currentEntityRole = 3; //both origin and target
		            }
		            else if (contentData.currentEntity.id == contentData.relationsList[i].targetEntityId && contentData.currentEntity.id != contentData.relationsList[i].originEntityId) {
		                contentData.relationsList[i].currentEntityRole = 2; //target
		            }
		            else if (contentData.currentEntity.id != contentData.relationsList[i].targetEntityId && contentData.currentEntity.id == contentData.relationsList[i].originEntityId) {
		                contentData.relationsList[i].currentEntityRole = 1; //origin
		            }
		            else if (contentData.currentEntity.id != contentData.relationsList[i].targetEntityId && contentData.currentEntity.id != contentData.relationsList[i].originEntityId) {
		                contentData.relationsList[i].currentEntityRole = 0; //possible problem
		            }
		            return contentData.relationsList[i];
		        }
		    }
		    return null;
		}
	    //#endregion

		if (contentData.selectedSidebarPage.isEdit) {

			//#region << Edit View Rendering Logic fields>>

			contentData.toggleSectionCollapse = function(section) {
				section.collapsed = !section.collapsed;
			}

			//Html
			//on #content check if mouse is clicked outside the editor, so to perform a possible field update
			contentData.viewCheckMouseButton = function ($event) {
				if (contentData.lastEnabledHtmlField != null) {
					contentData.fieldUpdate(contentData.lastEnabledHtmlField, contentData.selectedSidebarPage.data[contentData.lastEnabledHtmlField.dataName]);
					contentData.lastEnabledHtmlFieldData = null;
					contentData.lastEnabledHtmlField = null;
				}
				else {
					//Do nothing as this is a normal mouse click
				}
			}
			//on the editor textarea, prevent save when the mouse click is in the editor
			contentData.preventMouseSave = function ($event) {
				if ($event.currentTarget.className.indexOf("cke_focus") > -1) {
					$event.stopPropagation();
				}
			}
			//save without unblur on ctrl+S, prevent exiting the textarea on tab, cancel change on esc
			contentData.htmlFieldCheckEscapeKey = function ($event, item) {
				if ($event.keyCode == 27) { // escape key maps to keycode `27`
					//As the id is dynamic in our case and there is a problem with ckeditor and dynamic id-s we should use ng-attr-id in the html and here to cycle through all instances and find the current bye its container.$.id
					for (var property in CKEDITOR.instances) {
						if (CKEDITOR.instances[property].container.$.id == item.meta.name) {

							CKEDITOR.instances[property].editable().$.blur();
							//reinit the field
							contentData.selectedSidebarPage.data[item.dataName] = angular.copy(contentData.lastEnabledHtmlFieldData);
							contentData.lastEnabledHtmlField = null;
							contentData.lastEnabledHtmlFieldData = null;
							return false;
						}
					}
					var idd = 0;
				}
				else if ($event.keyCode == 9) { // tab key maps to keycode `9`
					$event.preventDefault();
					return false;
				}
				else if ($event.ctrlKey || $event.metaKey) {
					switch (String.fromCharCode($event.which).toLowerCase()) {
						case 's':
							
							$event.preventDefault();
							$timeout(function () {
								contentData.fieldUpdate(contentData.lastEnabledHtmlField, contentData.selectedSidebarPage.data[contentData.lastEnabledHtmlField.dataName]);
							}, 500);
							return false;
							break;
					}
				}
				return true;
			}

			contentData.lastEnabledHtmlField = null;
			contentData.lastEnabledHtmlFieldData = null;
			contentData.htmlFieldIsEnabled = function ($event, item) {
				contentData.lastEnabledHtmlField = item;
				contentData.lastEnabledHtmlFieldData = angular.copy(contentData.selectedSidebarPage.data[item.dataName]);
			}



			contentData.fieldUpdate = function(item, data) {
				var defer = $q.defer();
				contentData.patchObject = {};
				var validation = {
					success: true,
					message: "successful validation"
				};
				if (data != null) {
					data = data.toString().trim();
					switch (item.meta.fieldType) {

						//Auto increment number
					case 1:
						//Readonly
						break;
					//Checkbox
					case 2:
						data = (data === "true"); // convert string to boolean
						break;
					//Auto increment number
					case 3: //Currency
						if (!data && item.meta.required) {
							return "This is a required field";
						}
						validation = checkDecimal(data);
						if (!validation.success) {
							return validation.message;
						}
						if (decimalPlaces(data) > item.meta.currency.decimalDigits) {
							return "Decimal places should be " + item.meta.currency.decimalDigits + " or less";
						}
						break;
					case 4: //Date
						if (!data && item.meta.required) {
							return "This is a required field";
						}
						data = moment(data).startOf('day').utc().toISOString();
						break;
					case 5: //Datetime
						if (!data && item.meta.required) {
							return "This is a required field";
						}
						data = moment(data).startOf('minute').utc().toISOString();
						break;
					case 6: //Email
						if (!data && item.meta.required) {
							return "This is a required field";
						}
						validation = checkEmail(data);
						if (!validation.success) {
							return validation.message;
						}
						break;
					case 11: // Multiselect
						if (!data && item.meta.required) {
							return "This is a required field";
						}
						//We need to convert data which is "2,3" comma separated string to string array
						if (data !== '[object Array]') {
							data = data.split(',');
						}
						break;
					//Number
					case 12:
						if (!data && item.meta.required) {
							return "This is a required field";
						}
						validation = checkDecimal(data);
						if (!validation.success) {
							return validation.message;
						}
						if (!data) {
							data = null;
						}
						break;
					//Percent
					case 14:
						if (!data && item.meta.required) {
							return "This is a required field";
						}
						validation = checkPercent(data);
						if (!validation.success) {
							return validation.message;
						}
						if (!data) {
							data = null;
						}
						break;
					case 15: //Phone
						if (!data && item.meta.required) {
							return "This is a required field";
						}
						validation = checkPhone(data);
						if (!validation.success) {
							return validation.message;
						}
						break;
					case 17: // Dropdown
						if (!data && item.meta.required) {
							return "This is a required field";
						}
						break;
					}
				}
				contentData.patchObject[item.meta.name] = data;

				function patchSuccessCallback(response) {
					ngToast.create({
						className: 'success',
						content: '<span class="go-green">Success:</span> ' + response.message
					});
					contentData.selectedSidebarPage.data = angular.copy(response.object.data[0]);
					defer.resolve();
				}

				function patchFailedCallback(response) {
					ngToast.create({
						className: 'error',
						content: '<span class="go-red">Error:</span> ' + response.message
					});
					defer.resolve("validation error");
				}

				webvellaAdminService.patchRecord($stateParams.recordId, contentData.currentEntity.name, contentData.patchObject, patchSuccessCallback, patchFailedCallback);

				return defer.promise;
			}

			//Auto increment
			contentData.getAutoIncrementString = function(item) {
				var fieldValue = contentData.selectedSidebarPage.data[item.dataName];
				if (!fieldValue) {
					return "empty";
				} else if (item.meta.displayFormat) {
					return item.meta.displayFormat.replace("{0}", fieldValue);
				} else {
					return fieldValue;
				}
			}
			//Checkbox
			contentData.getCheckboxString = function(item) {
				var fieldValue = contentData.selectedSidebarPage.data[item.dataName];
				if (fieldValue) {
					return "<i class='fa fa-fw fa-check go-green'></i> true";
				} else {
					return "<i class='fa fa-fw fa-close go-red'></i> false";
				}
			}
			//Currency
			contentData.getCurrencyString = function(item) {
				var fieldValue = contentData.selectedSidebarPage.data[item.dataName];
				if (!fieldValue) {
					return "empty";
				} else if (item.meta.currency != null && item.meta.currency !== {} && item.meta.currency.symbol) {
					if (item.meta.currency.symbolPlacement === 1) {
						return item.meta.currency.symbol + " " + fieldValue;
					} else {
						return fieldValue + " " + item.meta.currency.symbol;
					}
				} else {
					return fieldValue;
				}
			}
			//Date & DateTime 
			contentData.getDateString = function(item) {
				var fieldValue = contentData.selectedSidebarPage.data[item.dataName];
				if (!fieldValue) {
					return "";
				} else {
					return $filter('date')(fieldValue, "dd MMM yyyy");
				}
			}
			contentData.getTimeString = function(item) {
				var fieldValue = contentData.selectedSidebarPage.data[item.dataName];
				if (!fieldValue) {
					return "";
				} else {
					return $filter('date')(fieldValue, "HH:mm");
				}
			}
			$scope.picker = { opened: false };
			$scope.openPicker = function() {
				$timeout(function() {
					$scope.picker.opened = true;
				});
			};
			$scope.closePicker = function() {
				$scope.picker.opened = false;
			};

			//File upload
			contentData.files = {}; // this is the data wrapper for the temporary upload objects that will be used in the html and for which we will generate watches below
			contentData.progress = {}; //data wrapper for the progress percentage for each upload

			/////////Register variables
			for (var sectionIndex = 0; sectionIndex < contentData.selectedSidebarPage.meta.sections.length; sectionIndex++) {
				for (var rowIndex = 0; rowIndex < contentData.selectedSidebarPage.meta.sections[sectionIndex].rows.length; rowIndex++) {
					for (var columnIndex = 0; columnIndex < contentData.selectedSidebarPage.meta.sections[sectionIndex].rows[rowIndex].columns.length; columnIndex++) {
						for (var itemIndex = 0; itemIndex < contentData.selectedSidebarPage.meta.sections[sectionIndex].rows[rowIndex].columns[columnIndex].items.length; itemIndex++) {
							if (contentData.selectedSidebarPage.meta.sections[sectionIndex].rows[rowIndex].columns[columnIndex].items[itemIndex].meta.fieldType === 7
								|| contentData.selectedSidebarPage.meta.sections[sectionIndex].rows[rowIndex].columns[columnIndex].items[itemIndex].meta.fieldType === 9) {
								var item = contentData.selectedSidebarPage.meta.sections[sectionIndex].rows[rowIndex].columns[columnIndex].items[itemIndex];
								var FieldName = item.dataName;
								contentData.progress[FieldName] = 0;
							}
						}
					}
				}
			}

			contentData.getProgressStyle = function(name) {
				return "width: " + contentData.progress[name] + "%;";
			}

			contentData.uploadedFileName = "";
			contentData.upload = function(file, item) {
				if (file != null) {
					contentData.uploadedFileName = item.dataName;
					contentData.moveSuccessCallback = function(response) {
						$timeout(function() {
							contentData.selectedSidebarPage.data[contentData.uploadedFileName] = response.object.url;
							contentData.fieldUpdate(item, response.object.url);
						}, 1);
					}

					contentData.uploadSuccessCallback = function(response) {
						var tempPath = response.object.url;
						var fileName = response.object.filename;
						var targetPath = "/fs/" + item.fieldId + "/" + fileName;
						var overwrite = true;
						webvellaAdminService.moveFileFromTempToFS(tempPath, targetPath, overwrite, contentData.moveSuccessCallback, contentData.uploadErrorCallback);
					}
					contentData.uploadErrorCallback = function(response) {
						alert(response.message);
					}
					contentData.uploadProgressCallback = function(response) {
						$timeout(function() {
							contentData.progress[contentData.uploadedFileName] = parseInt(100.0 * response.loaded / response.total);
						}, 1);
					}
					webvellaAdminService.uploadFileToTemp(file, item.meta.name, contentData.uploadProgressCallback, contentData.uploadSuccessCallback, contentData.uploadErrorCallback);
				}
			};

			contentData.deleteFileUpload = function(item) {
				var fieldName = item.dataName;
				var filePath = contentData.selectedSidebarPage.data[fieldName];

				function deleteSuccessCallback(response) {
					$timeout(function() {
						contentData.selectedSidebarPage.data[fieldName] = null;
						contentData.progress[fieldName] = 0;
						contentData.fieldUpdate(item, null);
					}, 0);
					return true;
				}

				function deleteFailedCallback(response) {
					ngToast.create({
						className: 'error',
						content: '<span class="go-red">Error:</span> ' + response.message
					});
					return "validation error";
				}

				webvellaAdminService.deleteFileFromFS(filePath, deleteSuccessCallback, deleteFailedCallback);

			}

			//Html
			$scope.editorOptions = {
				language: 'en',
				'skin': 'moono',
				height: '160',
				//'extraPlugins': "save",//"imagebrowser",//"imagebrowser,mediaembed",
				//imageBrowser_listUrl: '/api/v1/ckeditor/gallery',
				//filebrowserBrowseUrl: '/api/v1/ckeditor/files',
				//filebrowserImageUploadUrl: '/api/v1/ckeditor/images',
				//filebrowserUploadUrl: '/api/v1/ckeditor/files',
				toolbarLocation: 'top',
				toolbar: 'full',
				toolbar_full: [
					{name: 'basicstyles',items: ['Save','Bold', 'Italic', 'Strike', 'Underline']},
					{ name: 'paragraph', items: ['BulletedList', 'NumberedList', 'Blockquote'] },
					{ name: 'editing', items: ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'] },
					{ name: 'links', items: ['Link', 'Unlink', 'Anchor'] },
					{ name: 'tools', items: ['SpellChecker', 'Maximize'] },
					{ name: 'clipboard', items: ['Undo', 'Redo'] },
					{ name: 'styles', items: ['Format', 'FontSize', 'TextColor', 'PasteText', 'PasteFromWord', 'RemoveFormat'] },
					{ name: 'insert', items: ['Image', 'Table', 'SpecialChar', 'MediaEmbed'] }, '/',
				]
			};

			//Checkbox list
			contentData.getCheckboxlistString = function(fieldData, array) {
				if (fieldData) {
					var selected = [];
					angular.forEach(array, function(s) {
						if (fieldData.indexOf(s.key) >= 0) {
							selected.push(s.value);
						}
					});
					return selected.length ? selected.join(', ') : 'empty';
				} else {
					return 'empty';
				}
			}

			//Password
			contentData.dummyPasswordModels = {}; //as the password value is of know use being encrypted, we will assign dummy models
			//Dropdown
			contentData.getDropdownString = function(fieldData, array) {
				var selected = $filter('filter')(array, { key: fieldData });
				return (fieldData && selected.length) ? selected[0].value : 'empty';
			}

			//Percent
			$scope.Math = window.Math;

			contentData.getPercentString = function(item) {
				var fieldValue = contentData.selectedSidebarPage.data[item.dataName];

				if (!fieldValue) {
					return "empty";
				} else {
					//JavaScript has a bug when multiplying decimals
					//The way to correct this is to multiply the decimals before multiple their values,
					var resultPercentage = 0.00;
					resultPercentage = multiplyDecimals(fieldValue, 100, 3, $scope);
					return resultPercentage + "%";
				}

			}

			contentData.currentUserRoles = angular.copy(resolvedCurrentUser.roles);
			contentData.currentUserHasReadPermission = function (item) {
				var result = false;
				if (!item.meta.enableSecurity || item.meta.permissions == null) {
					return true;
				}
				for (var i = 0; i < contentData.currentUserRoles.length; i++) {
					for (var k = 0; k < item.meta.permissions.canRead.length; k++) {
						if (item.meta.permissions.canRead[k] == contentData.currentUserRoles[i]) {
							result = true;
						}
					}
				}
				return result;
			}


			contentData.currentUserHasUpdatePermission = function (item) {
				var result = false;
				if (!item.meta.enableSecurity || item.meta.permissions == null) {
					return true;
				}
				for (var i = 0; i < contentData.currentUserRoles.length; i++) {
					for (var k = 0; k < item.meta.permissions.canUpdate.length; k++) {
						if (item.meta.permissions.canUpdate[k] == contentData.currentUserRoles[i]) {
							result = true;
						}
					}
				}
				return result;
			}

			//#endregion

		}


		//#region << Modals >>

	    ////Relation field

	    ////////////////////
        // Single selection modal used in 1:1 relation and in 1:N when the currently viewed entity is a target in this relation
		contentData.openManageRelationFieldModal = function (item, relationType, dataKind) {
		    //relationType = 1 (one-to-one) , 2(one-to-many), 3(many-to-many)
		    //dataKind - target, origin, origin-target

            //Select ONE item modal
		    if (relationType == 1 || (relationType == 2 && dataKind == "target")) {
		        var modalInstance = $modal.open({
		            animation: false,
		            templateUrl: 'manageRelationFieldModal.html',
		            controller: 'ManageRelationFieldModalController',
		            controllerAs: "popupData",
		            size: "lg",
		            resolve: {
		                contentData: function () {
		                    return contentData;
		                },
		                selectedItem: function () {
		                    return item;
		                },
		                selectedRelationType: function () {
		                    return relationType;
		                },
		                selectedDataKind: function () {
		                    return dataKind;
		                },
		                resolvedLookupRecords: function () {
		                    return resolveLookupRecords(item,relationType, dataKind);
		                },
		                modalMode: function () {
		                    return "single-selection";
		                },
		            }
		        });
                //On modal exit
		        modalInstance.result.then(function (returnObject) {

		            // Initialize
		            var displayedRecordId = $stateParams.recordId;
		            var oldRelationRecordId = contentData.selectedSidebarPage.data["$field$" + returnObject.relationName + "$id"][0];
                    
		            function successCallback(response) {
		                ngToast.create({
		                    className: 'success',
		                    content: '<span class="go-green">Success:</span> Change applied'
		                });
		                webvellaRootService.GoToState($state, $state.current.name, contentData.stateParams);
		            }

		            function errorCallback(response) {
		                var messageHtml = response.message;
		                if (response.errors.length > 0) { //Validation errors
		                    messageHtml = "<ul>";
		                    for (var i = 0; i < response.errors.length; i++) {
		                        messageHtml += "<li>" + response.errors[i].message + "</li>";
		                    }
		                    messageHtml += "</ul>";
		                }
		                ngToast.create({
		                    className: 'error',
		                    content: '<span class="go-red">Error:</span> ' + messageHtml
		                });
		            }
		            
		            // ** Post relation change between the two records
		            var recordsToBeAttached = [];
		            var recordsToBeDettached = [];
		            if (returnObject.dataKind == "origin") {
		                recordsToBeAttached.push(returnObject.selectedRecordId);
		                recordsToBeDettached.push(oldRelationRecordId);
		                webvellaAdminService.manageRecordsRelation(returnObject.relationName, displayedRecordId, recordsToBeAttached,recordsToBeDettached, successCallback, errorCallback);
		            }
		            else if (returnObject.dataKind == "target") {
		                recordsToBeAttached.push(displayedRecordId);
		                webvellaAdminService.manageRecordsRelation(returnObject.relationName, returnObject.selectedRecordId, recordsToBeAttached,recordsToBeDettached, successCallback, errorCallback);
		            }
		            else {
		                alert("the <<origin-target>> dataKind is still not implemented. Contact the system administrator");
		            }
		        });
		    }
		    //Select MULTIPLE item modal
		else if ((relationType == 2 && dataKind == "origin") || (relationType == 3 && dataKind == "origin")) {
		        var modalInstance = $modal.open({
		            animation: false,
		            templateUrl: 'manageRelationFieldModal.html',
		            controller: 'ManageRelationFieldModalController',
		            controllerAs: "popupData",
		            size: "lg",
		            resolve: {
		                contentData: function () {
		                    return contentData;
		                },
		                selectedItem: function () {
		                    return item;
		                },
		                selectedRelationType: function () {
		                    return relationType;
		                },
		                selectedDataKind: function () {
		                    return dataKind;
		                },
		                resolvedLookupRecords: function () {
		                    return resolveLookupRecords(item, relationType, dataKind);
		                },
		                modalMode: function () {
		                    return "multi-selection";
		                },
		            }
		        });
		        //On modal exit
		        modalInstance.result.then(function (returnObject) {

		            // Initialize
		            var displayedRecordId = $stateParams.recordId;

		            function successCallback(response) {
		                ngToast.create({
		                    className: 'success',
		                    content: '<span class="go-green">Success:</span> Change applied'
		                });
		                webvellaRootService.GoToState($state, $state.current.name, contentData.stateParams);
		            }

		            function errorCallback(response) {
		                var messageHtml = response.message;
		                if (response.errors.length > 0) { //Validation errors
		                    messageHtml = "<ul>";
		                    for (var i = 0; i < response.errors.length; i++) {
		                        messageHtml += "<li>" + response.errors[i].message + "</li>";
		                    }
		                    messageHtml += "</ul>";
		                }
		                ngToast.create({
		                    className: 'error',
		                    content: '<span class="go-red">Error:</span> ' + messageHtml
		                });
		            }

		            // There are currently cases just for origin, error on else
		            if (returnObject.dataKind == "origin") {
		                webvellaAdminService.manageRecordsRelation(returnObject.relationName, displayedRecordId, returnObject.attachDelta, returnObject.detachDelta, successCallback, errorCallback);
		            }
		            else {
		                alert("the <<origin-target>> dataKind is still not implemented. Contact the system administrator");
		            }
		        });
		    }
		    else if ((relationType == 3 && dataKind == "target")) {
		        var modalInstance = $modal.open({
		            animation: false,
		            templateUrl: 'manageRelationFieldModal.html',
		            controller: 'ManageRelationFieldModalController',
		            controllerAs: "popupData",
		            size: "lg",
		            resolve: {
		                contentData: function () {
		                    return contentData;
		                },
		                selectedItem: function () {
		                    return item;
		                },
		                selectedRelationType: function () {
		                    return relationType;
		                },
		                selectedDataKind: function () {
		                    return dataKind;
		                },
		                resolvedLookupRecords: function () {
		                    return resolveLookupRecords(item, relationType, dataKind);
		                },
		                modalMode: function () {
		                    return "single-trigger-selection";
		                },
		            }
		        });

		    }
		}
		contentData.modalSelectedItem = {};
		contentData.modalRelationType = -1;
		contentData.modalDataKind = "";

	    //Resolve function lookup records
		var resolveLookupRecords = function (item, relationType, dataKind) {
		    // Initialize
		    var defer = $q.defer();
		    contentData.modalSelectedItem = item;
		    contentData.modalRelationType = relationType;
		    contentData.modalDataKind = dataKind;
		    // Process
		    function errorCallback(response) {
		        ngToast.create({
		            className: 'error',
		            content: '<span class="go-red">Error:</span> ' + response.message
		        });
		        defer.reject();
		    }
		    function getListRecordsSuccessCallback(response) {
		        defer.resolve(response); //Submitting the whole response to manage the error states
		    }

		    function getEntityMetaSuccessCallback(response) {
		        var entityMeta = response.object;
		        var defaultLookupList = null;

		        //Find the default lookup field if none return null.
		        for (var i = 0; i < entityMeta.recordLists.length; i++) {
		            if (entityMeta.recordLists[i].default && entityMeta.recordLists[i].type == "lookup") {
		                defaultLookupList = entityMeta.recordLists[i];
		                break;
		            }
		        }

		        if (defaultLookupList == null) {
		            response.message = "This entity does not have a default lookup list";
		            response.success = false;
		            errorCallback(response);
		        }
		        else {
		            var gg = contentData.modalSelectedItem;
		            //contentData.modalRelationType;
		            //contentData.modalDataKind;

                    //Current record is Origin
		            if (contentData.modalDataKind == "origin") {
		                //Find if the target field is required
		                var targetRequiredField = false;
		                var modalCurrrentRelation = contentData.getRelation(contentData.modalSelectedItem.relationName);
		                for (var m = 0; m < entityMeta.fields.length; m++) {
		                    if (entityMeta.fields[m].id == modalCurrrentRelation.targetFieldId) {
		                        targetRequiredField = entityMeta.fields[m].required;
		                    }
		                }
		                if (targetRequiredField && contentData.modalRelationType == 1) {
		                    //Case 1 - Solves the problem when the target field is required, but we are currently looking on the origin field holding record. 
		                    //In this case we cannot allow this relation to be managed from this origin record as the change will leave the old target record with null for its required field
		                    var lockedChangeResponse = {
		                        success: false,
		                        message: "This is a relation field, that cannot be managed from this record. Try managing it from the related <<" + entityMeta.label + ">> entity record",
                                object:null
		                    }
		                    getListRecordsSuccessCallback(lockedChangeResponse);
		                }
		                else {
		                    webvellaAreasService.getListRecords(defaultLookupList.name, entityMeta.name, "all", 1, getListRecordsSuccessCallback, errorCallback);
		                }
		            }
		            else if (contentData.modalDataKind == "target") {
                    //Current records is Target
		                webvellaAreasService.getListRecords(defaultLookupList.name, entityMeta.name, "all", 1, getListRecordsSuccessCallback, errorCallback);
		            }
		        }
		    }

		    webvellaAdminService.getEntityMeta(item.entityName, getEntityMetaSuccessCallback, errorCallback);

		    return defer.promise;
		}

        //#endregion


		$log.debug('webvellaAreas>entities> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	}


    //#region < Modal Controllers >
    //Test to unify all modals - Single select, multiple select, click to select
	ManageRelationFieldModalController.$inject = ['contentData', '$modalInstance', '$log', '$q','$stateParams', 'modalMode', 'resolvedLookupRecords',
        'selectedDataKind', 'selectedItem', 'selectedRelationType', 'webvellaAdminService', 'webvellaAreasService', 'webvellaRootService', 'ngToast', '$timeout', '$state'];
    /* @ngInject */
	function ManageRelationFieldModalController(contentData, $modalInstance, $log, $q, $stateParams, modalMode, resolvedLookupRecords,
        selectedDataKind, selectedItem, selectedRelationType, webvellaAdminService, webvellaAreasService, webvellaRootService, ngToast, $timeout, $state) {

		$log.debug('webvellaAdmin>entities>deleteFieldModal> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
	    /* jshint validthis:true */
	    var popupData = this;
	    popupData.currentPage = 1;
	    popupData.parentData = angular.copy(contentData);
	    popupData.selectedItem = angular.copy(selectedItem);
	    popupData.modalMode = angular.copy(modalMode);
	    popupData.hasWarning = false;
	    popupData.warningMessage = "";

        //Init
	    popupData.currentlyAttachedIds = angular.copy(popupData.parentData.selectedSidebarPage.data["$field$" + popupData.selectedItem.relationName + "$id"]); // temporary object in order to highlight
	    popupData.dbAttachedIds = angular.copy(popupData.currentlyAttachedIds);
	    popupData.attachedRecordIdsDelta = [];
	    popupData.detachedRecordIdsDelta = [];


	    //Get the default lookup list for the entity
	    if (resolvedLookupRecords.success) {
	        popupData.relationLookupList = angular.copy(resolvedLookupRecords.object);
	    }
	    else {
	        popupData.hasWarning = true;
	        popupData.warningMessage = resolvedLookupRecords.message;
	    }

	    //#region << Paging >>
	    popupData.selectPage = function (page) {
	        // Process
	        function successCallback(response) {
	            popupData.relationLookupList = angular.copy(response.object);
	            popupData.currentPage = page;
	        }

	        function errorCallback(response) {
	        	
	        }

	        webvellaAreasService.getListRecords(popupData.relationLookupList.meta.name, popupData.selectedItem.entityName, "all", page, successCallback, errorCallback);
	    }

	    //#endregion

	    //#region << Render Logic >>

	    //1.Auto increment
	    popupData.getAutoIncrementString = function (record, fieldMeta) {
	        var fieldValue = record[fieldMeta.name];
	        if (!fieldValue) {
	            return "";
	        }
	        else if (fieldMeta.displayFormat) {
	            return fieldMeta.displayFormat.replace("{0}", fieldValue);
	        }
	        else {
	            return fieldValue;
	        }
	    }
	    //2.Checkbox
	    popupData.getCheckboxString = function (record, fieldMeta) {
	        var fieldValue = record[fieldMeta.name];
	        if (fieldValue) {
	            return "true";
	        }
	        else {
	            return "false";
	        }
	    }
	    //3.Currency
	    popupData.getCurrencyString = function (record, fieldMeta) {
	        var fieldValue = record[fieldMeta.name];
	        if (!fieldValue) {
	            return "";
	        }
	        else if (fieldMeta.currency != null && fieldMeta.currency != {} && fieldMeta.currency.symbol) {
	            if (fieldMeta.currency.symbolPlacement == 1) {
	                return fieldMeta.currency.symbol + " " + fieldValue
	            }
	            else {
	                return fieldValue + " " + fieldMeta.currency.symbol
	            }
	        }
	        else {
	            return fieldValue;
	        }
	    }
	    //4.Date
	    popupData.getDateString = function (record, fieldMeta) {
	        var fieldValue = record[fieldMeta.name];
	        return moment(fieldValue).format('DD MMMM YYYY');
	    }
	    //5.Datetime
	    popupData.getDateTimeString = function (record, fieldMeta) {
	        var fieldValue = record[fieldMeta.name];
	        return moment(fieldValue).format('DD MMMM YYYY HH:mm');
	    }
	    //6.Email
	    popupData.getEmailString = function (record, fieldMeta) {
	        var fieldValue = record[fieldMeta.name];
	        if (fieldValue) {
	            return "<a href='mailto:" + fieldValue + "' target='_blank'>" + fieldValue + "</a>";
	        }
	        else {
	            return "";
	        }
	    }
	    //7.File
	    popupData.getFileString = function (record, fieldMeta) {
	        var fieldValue = record[fieldMeta.name];
	        if (fieldValue) {
	            return "<a href='" + fieldValue + "' taget='_blank' class='link-icon'>view file</a>";
	        }
	        else {
	            return "";
	        }
	    }
	    //8.Html
	    popupData.getHtmlString = function (record, fieldMeta) {
	        var fieldValue = record[fieldMeta.name];
	        if (fieldValue) {
	            return fieldValue;
	        }
	        else {
	            return "";
	        }
	    }
	    //9.Image
	    popupData.getImageString = function (record, fieldMeta) {
	        var fieldValue = record[fieldMeta.name];
	        if (fieldValue) {
	            return "<img src='" + fieldValue + "' class='table-image'/>";
	        }
	        else {
	            return "";
	        }
	    }
	    //11.Multiselect
	    popupData.getMultiselectString = function (record, fieldMeta) {
	        var fieldValueArray = record[fieldMeta.name];
	        var generatedStringArray = [];
	        if (fieldValueArray.length == 0) {
	            return "";
	        }
	        else {
	            for (var i = 0; i < fieldValueArray.length; i++) {
	                var selected = $filter('filter')(fieldMeta.options, { key: fieldValueArray[i] });
	                generatedStringArray.push((fieldValueArray[i] && selected.length) ? selected[0].value : 'empty');
	            }
	            return generatedStringArray.join(', ');

	        }

	    }
	    //14.Percent
	    popupData.getPercentString = function (record, fieldMeta) {
	        var fieldValue = record[fieldMeta.name];
	        if (!fieldValue) {
	            return "";
	        }
	        else {
	            return fieldValue * 100 + "%";
	        }
	    }
	    //15.Phone
	    popupData.getPhoneString = function (record, fieldMeta) {
	        var fieldValue = record[fieldMeta.name];
	        if (!fieldValue) {
	            return "";
	        }
	        else {
	            return phoneUtils.formatInternational(fieldValue);
	        }
	    }
	    //17.Dropdown
	    popupData.getDropdownString = function (record, fieldMeta) {
	        var fieldValue = record[fieldMeta.name];
	        if (!fieldValue) {
	            return "";
	        }
	        else {
	            var selected = $filter('filter')(fieldMeta.options, { key: fieldValue });
	            return (fieldValue && selected.length) ? selected[0].value : 'empty';
	        }

	    }
	    //18.Url
	    popupData.getUrlString = function (record, fieldMeta) {
	        var fieldValue = record[fieldMeta.name];
	        if (fieldValue) {
	            return "<a href='" + fieldValue + "' target='_blank'>" + fieldValue + "</a>";
	        }
	        else {
	            return "";
	        }
	    }

	    //#endregion 

	    popupData.isSelectedRecord = function (recordId) {
	        return popupData.currentlyAttachedIds.indexOf(recordId) > -1
	    }

        //Single record before save
	    popupData.selectSingleRecord = function (record) {
	        var returnObject = {
	            relationName: popupData.selectedItem.relationName,
	            dataKind: selectedDataKind,
	            selectedRecordId: record.id
	        };
	        $modalInstance.close(returnObject);
	    };

        // Multiple records before save
	    popupData.attachRecord = function (record) {
	        //Add record to delta  if it is NOT part of the original list
	        if (popupData.dbAttachedIds.indexOf(record.id) == -1) {
	            popupData.attachedRecordIdsDelta.push(record.id);
	        }

	        //Check and remove from the detachDelta if it is there
	        var elementIndex = popupData.detachedRecordIdsDelta.indexOf(record.id);
	        if (elementIndex > -1) {
	            popupData.detachedRecordIdsDelta.splice(elementIndex, 1);
	        }
	        //Update the currentlyAttachedIds for highlight
	        elementIndex = popupData.currentlyAttachedIds.indexOf(record.id);
	        if (elementIndex == -1) {
	            //this is the normal case
	            popupData.currentlyAttachedIds.push(record.id);
	        }
	        else {
	            //if it is already in the highligted list there is probably some miscalculation from previous operation, but for now we will do nothing
	        }

	    }
	    popupData.detachRecord = function (record) {
	        //Add record to detachDelta if it is part of the original list
	        if (popupData.dbAttachedIds.indexOf(record.id) > -1) {
	            popupData.detachedRecordIdsDelta.push(record.id);
	        }
	        //Check and remove from attachDelta if it is there
	        var elementIndex = popupData.attachedRecordIdsDelta.indexOf(record.id);
	        if (elementIndex > -1) {
	            popupData.attachedRecordIdsDelta.splice(elementIndex, 1);
	        }
	        //Update the currentlyAttachedIds for highlight
	        elementIndex = popupData.currentlyAttachedIds.indexOf(record.id);
	        if (elementIndex > -1) {
	            //this is the normal case
	            popupData.currentlyAttachedIds.splice(elementIndex, 1);
	        }
	        else {
	            //if it is already not in the highligted list there is probably some miscalculation from previous operation, but for now we will do nothing
	        }
	    }
	    popupData.saveRelationChanges = function () {
	        var returnObject = {
	            relationName: popupData.selectedItem.relationName,
	            dataKind: selectedDataKind,
	            attachDelta: popupData.attachedRecordIdsDelta,
	            detachDelta: popupData.detachedRecordIdsDelta
	        };
	        $modalInstance.close(returnObject);
	        //category_id
	    };

	    //Instant save on selection, keep popup open
	    popupData.processingRecordId = "";
	    popupData.processOperation = "";
	    popupData.processInstantSelection = function (returnObject) {

	        // Initialize
	        popupData.processingRecordId = returnObject.selectedRecordId;
	        popupData.processOperation = returnObject.operation;
	        var displayedRecordId = $stateParams.recordId;
	        var recordsToBeAttached = [];
	        var recordsToBeDettached = [];
	        if (returnObject.operation == "attach") {
	            recordsToBeAttached.push(displayedRecordId);
	        }
	        else if (returnObject.operation == "detach") {
	            recordsToBeDettached.push(displayedRecordId);
	        }

	        function successCallback(response) {
	            var currentRecordId = angular.copy(popupData.processingRecordId);
	            var elementIndex = popupData.currentlyAttachedIds.indexOf(currentRecordId);
	            if (popupData.processOperation == "attach" && elementIndex == -1) {
	                popupData.currentlyAttachedIds.push(currentRecordId);
	                popupData.processingRecordId = "";
	            }
	            else if (popupData.processOperation == "detach" && elementIndex > -1) {
	                popupData.currentlyAttachedIds.splice(elementIndex, 1);
	                popupData.processingRecordId = "";
	            }

	            ngToast.create({
	                className: 'success',
	                content: '<span class="go-green">Success:</span> Change applied'
	            });
	        }

	        function errorCallback(response) {
	            popupData.processingRecordId = "";
	            var messageHtml = response.message;
	            if (response.errors.length > 0) { //Validation errors
	                messageHtml = "<ul>";
	                for (var i = 0; i < response.errors.length; i++) {
	                    messageHtml += "<li>" + response.errors[i].message + "</li>";
	                }
	                messageHtml += "</ul>";
	            }
	            ngToast.create({
	                className: 'error',
	                content: '<span class="go-red">Error:</span> ' + messageHtml
	            });
	            
	        }

	        // ** Post relation change between the two records
	        if (returnObject.dataKind == "target") {
	            webvellaAdminService.manageRecordsRelation(returnObject.relationName, returnObject.selectedRecordId, recordsToBeAttached, recordsToBeDettached, successCallback, errorCallback);
	        }
	        else {
	            alert("the <<origin-target>> dataKind is still not implemented. Contact the system administrator");
	        }
	    }
	    popupData.instantAttachRecord = function (record) {
	        var returnObject = {
	            relationName: popupData.selectedItem.relationName,
	            dataKind: selectedDataKind,
	            selectedRecordId: record.id,
                operation: "attach"
	        };
	        if (!popupData.processingRecordId) {
	            popupData.processInstantSelection(returnObject);
	        }
	    };
	    popupData.instantDetachRecord = function (record) {
	        var returnObject = {
	            relationName: popupData.selectedItem.relationName,
	            dataKind: selectedDataKind,
	            selectedRecordId: record.id,
	            operation: "detach"

	        };
	        if (!popupData.processingRecordId) {
	            popupData.processInstantSelection(returnObject);
	        }
	    };


	    popupData.cancel = function () {
	        $modalInstance.dismiss('cancel');
	    };

	    /// Aux
	    function successCallback(response) {
	        ngToast.create({
	            className: 'success',
	            content: '<span class="go-green">Success:</span> ' + response.message
	        });
	        $modalInstance.close('success');
	        popupData.parentData.modalInstance.close('success');
	        //webvellaRootService.GoToState($state, $state.current.name, {});
	    }

	    function errorCallback(response) {
	        popupData.hasError = true;
	        popupData.errorMessage = response.message;


	    }


	    //#endregion

	    $log.debug('webvellaAdmin>entities>createEntityModal> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	};



    //#endregion


})();



