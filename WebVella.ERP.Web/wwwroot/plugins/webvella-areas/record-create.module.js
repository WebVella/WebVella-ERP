/* entities.module.js */

/**
* @desc this module manages the application home desktop screen
*/

(function () {
	'use strict';

	angular
        .module('webvellaAreas') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
		.controller('CreateRelationFieldModalController', CreateRelationFieldModalController)
        .controller('WebVellaAreasRecordCreateController', controller);

	// Configuration ///////////////////////////////////
	config.$inject = ['$stateProvider'];

	/* @ngInject */
	function config($stateProvider) {
		$stateProvider.state('webvella-areas-record-create', {
			parent: 'webvella-areas-base',
			url: '/create?listName&filter&page',
			views: {
				"topnavView": {
					controller: 'WebVellaAreasTopnavController',
					templateUrl: '/plugins/webvella-areas/topnav.view.html',
					controllerAs: 'topnavData'
				},
				"sidebarView": {
					controller: 'WebVellaAreasSidebarController',
					templateUrl: '/plugins/webvella-areas/sidebar.view.html',
					controllerAs: 'sidebarData'
				},
				"contentView": {
					controller: 'WebVellaAreasRecordCreateController',
					templateUrl: '/plugins/webvella-areas/record-create.view.html',
					controllerAs: 'ngCtrl'
				}
			},
			resolve: {},
			data: {

			}
		});
	};


	//#region << Run >> //////////////////////////////////////
	run.$inject = ['$log'];
	/* @ngInject */
	function run($log) {
		$log.debug('webvellaAreas>record-create> BEGIN module.run ' + moment().format('HH:mm:ss SSSS'));

		$log.debug('webvellaAreas>record-create> END module.run ' + moment().format('HH:mm:ss SSSS'));
	};
	//#endregion

	//#region << Resolve Function >> /////////////////////////

	//#endregion


	// Controller ///////////////////////////////
	function multiplyDecimals(val1, val2, decimalPlaces) {
		var helpNumber = 100;
		for (var i = 0; i < decimalPlaces; i++) {
			helpNumber = helpNumber * 10;
		}
		var temp1 = $scope.Math.round(val1 * helpNumber);
		var temp2 = $scope.Math.round(val2 * helpNumber);
		return (temp1 * temp2) / (helpNumber * helpNumber);
	}


	controller.$inject = ['$filter', '$uibModal', '$log', '$q', '$rootScope', '$state', '$stateParams', '$scope', 'pageTitle', 'webvellaRootService', 'webvellaAdminService', 'webvellaAreasService',
        'resolvedSitemap', '$timeout', 'ngToast', 'wvAppConstants', 'resolvedCurrentEntityMeta', 'resolvedEntityRelationsList', '$anchorScroll', '$location'];

	/* @ngInject */
	function controller($filter, $uibModal, $log, $q, $rootScope, $state, $stateParams, $scope, pageTitle, webvellaRootService, webvellaAdminService, webvellaAreasService,
        resolvedSitemap, $timeout, ngToast, wvAppConstants, resolvedCurrentEntityMeta, resolvedEntityRelationsList, $anchorScroll, $location) {
		$log.debug('webvellaAreas>record-create> BEGIN controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var ngCtrl = this;
		ngCtrl.stateParams = $stateParams;
		ngCtrl.validation = {};
		//#region <<Set pageTitle>>
		ngCtrl.pageTitle = "Area Entities | " + pageTitle;
		webvellaRootService.setPageTitle(ngCtrl.pageTitle);
		ngCtrl.siteMap = fastCopy(resolvedSitemap);
		ngCtrl.currentArea = null;
		for (var i = 0; i < ngCtrl.siteMap.data.length; i++) {
			if (ngCtrl.siteMap.data[i].name == $state.params.areaName) {
				ngCtrl.currentArea = ngCtrl.siteMap.data[i];
			};
		}
		webvellaRootService.setBodyColorClass(ngCtrl.currentArea.color);
		//#endregion

		//#region << Initialize current entity >>
		ngCtrl.currentEntity = fastCopy(resolvedCurrentEntityMeta);
		//#endregion

		//#region << Set environment >> /////////////////////

		ngCtrl.createViewRegion = null;
		ngCtrl.createView = null;
		for (var i = 0; i < ngCtrl.currentEntity.recordViews.length; i++) {
			if (ngCtrl.currentEntity.recordViews[i].type === "create" && ngCtrl.currentEntity.recordViews[i].default) {
				ngCtrl.createView = ngCtrl.currentEntity.recordViews[i];
				for (var j = 0; j < ngCtrl.currentEntity.recordViews[i].regions.length; j++) {
					if (ngCtrl.currentEntity.recordViews[i].regions[j].name === "content") {
						ngCtrl.createViewRegion = ngCtrl.currentEntity.recordViews[i].regions[j];
					}
				}
			}
		}

		//Initialize entityRecordData
		ngCtrl.entityData = {};
		ngCtrl.files = {}; // this is the data wrapper for the temporary upload objects that will be used in the html and for which we will generate watches below
		ngCtrl.progress = {}; //Needed for file and image uploads
		var availableViewFields = [];
		//Init default values of fields
		if (ngCtrl.createViewRegion != null) {
			availableViewFields = webvellaAdminService.getItemsFromRegion(ngCtrl.createViewRegion);
			for (var j = 0; j < availableViewFields.length; j++) {
				if (availableViewFields[j].type === "field") {
					switch (availableViewFields[j].meta.fieldType) {

						case 2: //Checkbox
							ngCtrl.entityData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							break;

						case 3: //Currency
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								ngCtrl.entityData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 4: //Date
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								if (availableViewFields[j].meta.useCurrentTimeAsDefaultValue) {
									ngCtrl.entityData[availableViewFields[j].meta.name] = moment().toISOString();
								}
								else if (availableViewFields[j].meta.defaultValue) {
									ngCtrl.entityData[availableViewFields[j].meta.name] = moment(availableViewFields[j].meta.defaultValue).toISOString();
								}
							}
							break;
						case 5: //Date
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								if (availableViewFields[j].meta.useCurrentTimeAsDefaultValue) {
									ngCtrl.entityData[availableViewFields[j].meta.name] = moment().toISOString();
								}
								else if (availableViewFields[j].meta.defaultValue) {
									ngCtrl.entityData[availableViewFields[j].meta.name] = moment(availableViewFields[j].meta.defaultValue).toISOString();
								}
							}
							break;
						case 6: //Email
							break;
						case 7: //File
							ngCtrl.progress[availableViewFields[j].meta.name] = 0;
							if (availableViewFields[j].meta.required) {
								ngCtrl.entityData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 8: //HTML
							if (availableViewFields[j].meta.required) {
								ngCtrl.entityData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 9: //Image
							ngCtrl.progress[availableViewFields[j].meta.name] = 0;
							if (availableViewFields[j].meta.required) {
								ngCtrl.entityData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 10: //TextArea
							if (availableViewFields[j].meta.required) {
								ngCtrl.entityData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 11: //Multiselect
							if (availableViewFields[j].meta.required) {
								ngCtrl.entityData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 12: //Number
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								ngCtrl.entityData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 13: //Password
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								//Does not have default value
								//ngCtrl.entityData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 14: //Percent
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								//Need to convert the default value to percent x 100
								//TODO: apply this after the defaultValue, maxValue and minValue properties are stored as decimals - Ref. Issue #18

								//JavaScript has bugs when multiplying decimals
								//The way to correct this is to multiply the decimals before multiple their values,
								//var resultPercentage = 0.00;
								//resultPercentage = multiplyDecimals(availableViewFields[j].meta.defaultValue, 100, 3);
								//ngCtrl.entityData[availableViewFields[j].meta.name] = resultPercentage;
							}
							break;
						case 15: //Phone
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								ngCtrl.entityData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 16: //Guid
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								ngCtrl.entityData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 17: //Dropdown
							if (availableViewFields[j].meta.required && availableViewFields[j].meta.defaultValue) {
								ngCtrl.entityData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 18: //Text
							//if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
							//	ngCtrl.entityData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							//}
							break;
						case 19: //URL
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								ngCtrl.entityData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
					}


				}
			}
		}



		// << File >>
		ngCtrl.uploadedFileName = "";
		ngCtrl.upload = function (file, item) {
			if (file != null) {
				ngCtrl.uploadedFileName = item.dataName;
				ngCtrl.moveSuccessCallback = function (response) {
					$timeout(function () {
						ngCtrl.entityData[ngCtrl.uploadedFileName] = response.object.url;
					}, 1);
				}

				ngCtrl.uploadSuccessCallback = function (response) {
					var tempPath = response.object.url;
					var fileName = response.object.filename;
					var targetPath = "/fs/" + ngCtrl.currentEntity.name + "/" + newGuid() + "/" + fileName;
					var overwrite = false;
					webvellaAdminService.moveFileFromTempToFS(tempPath, targetPath, overwrite, ngCtrl.moveSuccessCallback, ngCtrl.uploadErrorCallback);
				}
				ngCtrl.uploadErrorCallback = function (response) {
					alert(response.message);
				}
				ngCtrl.uploadProgressCallback = function (response) {
					$timeout(function () {
						ngCtrl.progress[ngCtrl.uploadedFileName] = parseInt(100.0 * response.loaded / response.total);
					}, 1);
				}
				webvellaAdminService.uploadFileToTemp(file, item.meta.name, ngCtrl.uploadProgressCallback, ngCtrl.uploadSuccessCallback, ngCtrl.uploadErrorCallback);
			}
		};

		ngCtrl.deleteFileUpload = function (item) {
			var fieldName = item.dataName;
			var filePath = ngCtrl.entityData[fieldName];

			function deleteSuccessCallback(response) {
				$timeout(function () {
					ngCtrl.entityData[fieldName] = null;
					ngCtrl.progress[fieldName] = 0;
				}, 0);
				return true;
			}

			function deleteFailedCallback(response) {
				ngToast.create({
					className: 'error',
					content: '<span class="go-red">Error:</span> ' + response.message,
					timeout: 7000
				});
				return "validation error";
			}

			webvellaAdminService.deleteFileFromFS(filePath, deleteSuccessCallback, deleteFailedCallback);

		}


		// << Html >>
		//Should use scope as it is not working with ngCtrl
		$scope.editorOptions = {
			language: 'en',
			'skin': 'moono',
			height: '160',
			'extraPlugins': "sourcedialog",//"imagebrowser",//"imagebrowser,mediaembed",
			//imageBrowser_listUrl: '/api/v1/ckeditor/gallery',
			//filebrowserBrowseUrl: '/api/v1/ckeditor/files',
			//filebrowserImageUploadUrl: '/api/v1/ckeditor/images',
			//filebrowserUploadUrl: '/api/v1/ckeditor/files',
			allowedContent: true,
			toolbarLocation: 'top',
			toolbar: 'full',
			toolbar_full: [
				{ name: 'basicstyles', items: ['Save', 'Bold', 'Italic', 'Strike', 'Underline'] },
				{ name: 'paragraph', items: ['BulletedList', 'NumberedList', 'Blockquote'] },
				{ name: 'editing', items: ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'] },
				{ name: 'links', items: ['Link', 'Unlink', 'Anchor'] },
				{ name: 'tools', items: ['SpellChecker', 'Maximize'] },
				{ name: 'clipboard', items: ['Undo', 'Redo'] },
				{ name: 'styles', items: ['Format', 'FontSize', 'TextColor', 'PasteText', 'PasteFromWord', 'RemoveFormat'] },
				{ name: 'insert', items: ['Image', 'Table', 'SpecialChar', 'Sourcedialog'] }, '/',
			]
		};


		ngCtrl.toggleSectionCollapse = function (section) {
			section.collapsed = !section.collapsed;
		}

		ngCtrl.calendars = {};
		ngCtrl.openCalendar = function (event, name) {
			ngCtrl.calendars[name] = true;
		}

		//#endregion

		//#region << Entity relations functions >>
		ngCtrl.relationsList = fastCopy(resolvedEntityRelationsList);

		ngCtrl.getRelation = function (relationName) {
			for (var i = 0; i < ngCtrl.relationsList.length; i++) {
				if (ngCtrl.relationsList[i].name == relationName) {
					//set current entity role
					if (ngCtrl.currentEntity.id == ngCtrl.relationsList[i].targetEntityId && ngCtrl.currentEntity.id == ngCtrl.relationsList[i].originEntityId) {
						ngCtrl.relationsList[i].currentEntityRole = 3; //both origin and target
					}
					else if (ngCtrl.currentEntity.id == ngCtrl.relationsList[i].targetEntityId && ngCtrl.currentEntity.id != ngCtrl.relationsList[i].originEntityId) {
						ngCtrl.relationsList[i].currentEntityRole = 2; //target
					}
					else if (ngCtrl.currentEntity.id != ngCtrl.relationsList[i].targetEntityId && ngCtrl.currentEntity.id == ngCtrl.relationsList[i].originEntityId) {
						ngCtrl.relationsList[i].currentEntityRole = 1; //origin
					}
					else if (ngCtrl.currentEntity.id != ngCtrl.relationsList[i].targetEntityId && ngCtrl.currentEntity.id != ngCtrl.relationsList[i].originEntityId) {
						ngCtrl.relationsList[i].currentEntityRole = 0; //possible problem
					}
					return ngCtrl.relationsList[i];
				}
			}
			return null;
		}

		ngCtrl.hasFieldFromRelationValue = function (itemDataName) {
			var dataNameArray =  itemDataName.split('$');
			if (ngCtrl.entityData["$" + dataNameArray[2] + "." + "id"]) {
				return true;
			}
			else {
				return false;
			}
		}

		ngCtrl.removeFieldFromRelationValue = function (itemDataName) {
			var dataNameArray =  itemDataName.split('$');
			$timeout(function () {
				delete ngCtrl.entityData["$" + dataNameArray[2] + "." + "id"];
				delete ngCtrl.dummyFields[itemDataName];
			}, 10);
		}


		//#endregion

		//#region << Render >>
		ngCtrl.calculatefieldWidths = webvellaAdminService.calculateViewFieldColsFromGridColSize;
		ngCtrl.checkUserEntityPermissions = function (permissionsCsv) {
			return fastCopy(webvellaRootService.userHasEntityPermissions(ngCtrl.currentEntity, permissionsCsv));
		}


		ngCtrl.getRelationLabel = function (item) {
			if (item.fieldLabel) {
				return item.fieldLabel
			}
			else {
				var relationName = item.relationName;
				var relation = findInArray(ngCtrl.relationsList, "name", relationName);
				if (relation) {
					return relation.label;
				}
				else {
					return "";
				}
			}
		}

		//#region << Methods to generate the record details view name >>

		//Select default view for the area
		ngCtrl.selectedList = {};
		for (var j = 0; j < ngCtrl.currentEntity.recordLists.length; j++) {
			if (ngCtrl.currentEntity.recordLists[j].name === $stateParams.listName) {
				ngCtrl.selectedList = ngCtrl.currentEntity.recordLists[j];
				break;
			}
		}



		//Generate the proper view name for the record details screen (needed in the redirect)
		ngCtrl.generateViewName = function (record) {
			//default is the selected view in the area
			var result = fastCopy(ngCtrl.selectedList.name);

			if (ngCtrl.selectedList.viewNameOverride && cngCtrl.selectedList.viewNameOverride.length > 0) {
				var arrayOfTemplateKeys = ngCtrl.selectedList.viewNameOverride.match(/\{([\$\w]+)\}/g); //Include support for matching also data from relations which include $ symbol
				var resultStringStorage = fastCopy(ngCtrl.selectedList.viewNameOverride);

				for (var i = 0; i < arrayOfTemplateKeys.length; i++) {
					if (arrayOfTemplateKeys[i] === "{areaName}" || arrayOfTemplateKeys[i] === "{entityName}" || arrayOfTemplateKeys[i] === "{page}" || arrayOfTemplateKeys[i] === "{searchQuery}") {
						switch (arrayOfTemplateKeys[i]) {
							case "{areaName}":
								resultStringStorage = resultStringStorage.replace(arrayOfTemplateKeys[i], convertToSlug($stateParams.areaName));
								break;
							case "{entityName}":
								resultStringStorage = resultStringStorage.replace(arrayOfTemplateKeys[i], convertToSlug($stateParams.entityName));
								break;
							case "{page}":
								resultStringStorage = resultStringStorage.replace(arrayOfTemplateKeys[i], convertToSlug($stateParams.page));
								break;
							case "{searchQuery}":
								resultStringStorage = resultStringStorage.replace(arrayOfTemplateKeys[i], convertToSlug($stateParams.searchQuery));
								break;
						}
					}
					else {
						//Extract the dataName from string by removing the leading and the closing {}
						var dataName = arrayOfTemplateKeys[i].replace('{', '').replace('}', '');
						//Check template has corresponding list data value
						if (record[dataName] != undefined) {
							//YES -> check the value of this dataName and substitute with it in the string, even if it is null (toString)
							//Case 1 - data is not from relation (not starting with $)
							if (!dataName.startsWith('$')) {
								resultStringStorage = resultStringStorage.replace(arrayOfTemplateKeys[i], convertToSlug(record[dataName].toString()));
							}
							else {
								//Case 2 - relation field
								resultStringStorage = resultStringStorage.replace(arrayOfTemplateKeys[i], convertToSlug(record[dataName][0].toString()));
							}

						}
						else {
							//NO -> substitute the template key with the dataName only, as no value could be extracted
							resultStringStorage = resultStringStorage.replace(arrayOfTemplateKeys[i], convertToSlug(dataName));
						}
					}

				}
				result = resultStringStorage;
			}

			return result;
		}
		//#endregion

		//#endregion

		//#region << Save >>
		ngCtrl.create = function (redirectTarget) {
			//Validate 
			ngCtrl.validation = {};
			for (var k = 0; k < availableViewFields.length; k++) {
				if (availableViewFields[k].type === "field" && availableViewFields[k].meta.required) {
					if (ngCtrl.entityData[availableViewFields[k].dataName] == null || ngCtrl.entityData[availableViewFields[k].dataName] == "") {
						ngCtrl.validation[availableViewFields[k].dataName] = true;
						ngCtrl.validation.hasError = true;
						ngCtrl.validation.errorMessage = "A required data is missing!";
					}
				}
			}
			if (!ngCtrl.validation.hasError) {
				//Alter some data before save
				for (var k = 0; k < availableViewFields.length; k++) {
					if (availableViewFields[k].type === "field") {
						switch (availableViewFields[k].meta.fieldType) {
							case 4: //Date
								if (ngCtrl.entityData[availableViewFields[k].dataName] != null) {
									ngCtrl.entityData[availableViewFields[k].dataName] = moment(ngCtrl.entityData[availableViewFields[k].dataName]).startOf('day').utc().toISOString();
								}
								break;
							case 5: //Date & Time
								if (ngCtrl.entityData[availableViewFields[k].dataName] != null) {
									ngCtrl.entityData[availableViewFields[k].dataName] = moment(ngCtrl.entityData[availableViewFields[k].dataName]).startOf('minute').utc().toISOString();
								}
								break;
							case 14: //Percent
								//need to convert to decimal 0 <= val <= 100 Divide by 100
								//Hack for proper javascript division
								$scope.Math = window.Math;
								var helpNumber = 10000000;
								var multipliedValue = $scope.Math.round(ngCtrl.entityData[availableViewFields[k].dataName] * helpNumber);
								ngCtrl.entityData[availableViewFields[k].dataName] = multipliedValue / (100 * helpNumber);
								break;
						}
					}
					else if (availableViewFields[k].type === "fieldFromRelation") {
						//Currently no need to remove field from relations that are not id, as they are not attached to the entityData anyway
					}
				}
				ngCtrl.entityData["created_on"] = moment().utc().toISOString();
				//popupCtrl.entityData["created_by"] = ""; //TODO: put the current user id after the users are implemented
				switch (redirectTarget) {
					case "details":
						webvellaAdminService.createRecord(ngCtrl.currentEntity.name, ngCtrl.entityData, successCallback, errorCallback);
						break;
					case "list":
						webvellaAdminService.createRecord(ngCtrl.currentEntity.name, ngCtrl.entityData, successCallbackList, errorCallback);
						break;
				}
			}
			else {
				//Scroll top
				// set the location.hash to the id of
				// the element you wish to scroll to.
				$location.hash('page-title');

				// call $anchorScroll()
				$anchorScroll();

			}
		};

		ngCtrl.cancel = function () {
			$timeout(function () {
				$state.go("webvella-entity-records", {
					areaName: $stateParams.areaName,
					entityName: $stateParams.entityName,
					listName: $stateParams.listName,
					page: $stateParams.page

				}, { reload: true });
			}, 0);
		};

		/// Aux
		function successCallback(response) {
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success:</span> ' + response.message
			});
			$timeout(function () {
				var detailsViewName = ngCtrl.generateViewName(response.object);

				$state.go("webvella-areas-record-view", {
					areaName: $stateParams.areaName,
					entityName: $stateParams.entityName,
					recordId: response.object.data[0].id,
					viewName: detailsViewName,
					auxPageName: "*",
					page: $stateParams.page

				}, { reload: true });
			}, 0);
		}

		function successCallbackList(response) {
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success:</span> ' + response.message
			});
			$timeout(function () {
				var detailsViewName = ngCtrl.generateViewName(response.object);

				$state.go("webvella-entity-records", {
					areaName: $stateParams.areaName,
					entityName: $stateParams.entityName,
					listName: $stateParams.listName,
					page: $stateParams.page

				}, { reload: true });
			}, 0);
		}

		function errorCallback(response) {


		}

		//#endregion

		//#region << Modals >>

		//#region << Relation field >>
		ngCtrl.dummyFields = {};  //These fields will present data to the user that will not be submitted
		////////////////////
		// Single selection modal used in 1:1 relation and in 1:N when the currently viewed entity is a target in this relation
		ngCtrl.openManageRelationFieldModal = function (item, relationType, dataKind) {
			//relationType = 1 (one-to-one) , 2(one-to-many), 3(many-to-many)
			//dataKind - target, origin, origin-target

			//Select ONE item modal
			if (relationType == 1 || (relationType == 2 && dataKind == "target")) {
				var modalInstance = $uibModal.open({
					animation: false,
					templateUrl: 'manageRelationFieldModal.html',
					controller: 'CreateRelationFieldModalController',
					controllerAs: "popupCtrl",
					size: "lg",
					resolve: {
						ngCtrl: function () {
							return ngCtrl;
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
							return "single-selection";
						},
					}
				});
				//On modal exit
				modalInstance.result.then(function (returnObject) {
					var selectedRecord = {};
					//1.the field name needed for the view (item.fieldName) and the other's entity name	(item.entityName)
					//2.get the record by returnObject.selectedRecordId, with the value of the field in the view 
					function modalCase1SuccessCallback(response) {
						//3.set the value of the dummy field (dummyFields[item.dataName] in the create to match the found view field value
						var dummyFieldValue = null;
						var dataNameArray =  item.dataName.split('$');
						var dummyFieldName = dataNameArray[3];
						ngCtrl.dummyFields[item.dataName] = webvellaAreasService.renderFieldValue(response.object.data[0][dummyFieldName], item.meta);
						//4.set in the create model $field$relation_name$id -> is this is the only way to be sure that the value will be unique and the api will not produce error
						ngCtrl.entityData["$" + dataNameArray[2] + "." + "id"] = returnObject.selectedRecordId;
					}
					function modalCase1ErrorCallback(response) {
						ngToast.create({
							className: 'error',
							content: '<span class="go-red">Error:</span> ' + response.message,
							timeout: 7000
						});
					}
					webvellaAdminService.getRecord(returnObject.selectedRecordId,"*", item.entityName, modalCase1SuccessCallback, modalCase1ErrorCallback);
				});
			}
				//Select MULTIPLE item modal
			else if ((relationType == 2 && dataKind == "origin") || relationType == 3) {
				var modalInstance = $uibModal.open({
					animation: false,
					templateUrl: 'manageRelationFieldModal.html',
					controller: 'CreateRelationFieldModalController',
					controllerAs: "popupCtrl",
					size: "lg",
					resolve: {
						ngCtrl: function () {
							return ngCtrl;
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
					var selectedRecords = [];
					var dataNameArray =  item.dataName.split('$');
					var dummyFieldName = dataNameArray[3];
					var idFieldPrefix = "$" + dataNameArray[2] + ".";
					//1.the field name needed for the view (item.fieldName) and the other's entity name	(item.entityName)
					//2.get the record by returnObject.selectedRecordId, with the value of the field in the view 
					//2.get the record by returnObject.selectedRecordId, with the value of the field in the view 
					function modalCase1SuccessCallback(response) {
						//3.set the value of the dummy field (dummyFields[item.dataName] in the create to match the found view field value
						var dummyFieldValue = null;
						ngCtrl.dummyFields[item.dataName] = [];
						ngCtrl.entityData[idFieldPrefix + "id"] = [];
						for (var i = 0; i < response.object.data.length; i++) {
							ngCtrl.dummyFields[item.dataName].push(webvellaAreasService.renderFieldValue(response.object.data[i][dummyFieldName], item.meta));
							//4.set in the create model $field$relation_name$id -> is this is the only way to be sure that the value will be unique and the api will not produce error
							ngCtrl.entityData[idFieldPrefix + "id"].push(response.object.data[i]["id"]);
						}
					}
					function modalCase1ErrorCallback(response) {
						ngToast.create({
							className: 'error',
							content: '<span class="go-red">Error:</span> ' + response.message,
							timeout: 7000
						});
					}
					var fieldsArray = [];
					fieldsArray.push(dummyFieldName);

					if (returnObject.selectedRecordIdArray.length == 0) {
					   	ngCtrl.dummyFields[item.dataName] = [];
						ngCtrl.entityData[idFieldPrefix + "id"] = [];
					}
					else {
						var recordIdCSV = returnObject.selectedRecordIdArray.join(',');
						var fieldCSV = fieldsArray.join(',');
						webvellaAdminService.getRecords(recordIdCSV, fieldCSV, item.entityName, modalCase1SuccessCallback, modalCase1ErrorCallback);
					}
				});
			}
		}
		ngCtrl.modalSelectedItem = {};
		ngCtrl.modalRelationType = -1;
		ngCtrl.modalDataKind = "";

		//Resolve function lookup records
		var resolveLookupRecords = function (item, relationType, dataKind) {
			// Initialize
			var defer = $q.defer();
			ngCtrl.modalSelectedItem = fastCopy(item);
			ngCtrl.modalRelationType = fastCopy(relationType);
			ngCtrl.modalDataKind = fastCopy(dataKind);
			// Process
			function errorCallback(response) {
				ngToast.create({
					className: 'error',
					content: '<span class="go-red">Error:</span> ' + response.message,
					timeout: 7000
				});
				defer.reject();
			}
			function getListRecordsSuccessCallback(response) {
				defer.resolve(response); //Submitting the whole response to manage the error states
			}

			function getEntityMetaSuccessCallback(response) {
				var entityMeta = response.object;
				var defaultLookupList = null;
				var selectedLookupListName = ngCtrl.modalSelectedItem.fieldLookupList;
				var selectedLookupList = null;
				//Find the default lookup field if none return null.
				for (var i = 0; i < entityMeta.recordLists.length; i++) {
					//Check if the selected lookupList Exists
					if (entityMeta.recordLists[i].name == selectedLookupListName) {
						selectedLookupList = entityMeta.recordLists[i];
					}
					if (entityMeta.recordLists[i].default && entityMeta.recordLists[i].type == "lookup") {
						defaultLookupList = entityMeta.recordLists[i];
					}
				}

				if (selectedLookupList == null && defaultLookupList == null) {
					response.message = "This entity does not have selected or default lookup list";
					response.success = false;
					errorCallback(response);
				}
				else {

					//var gg = ngCtrl.modalSelectedItem;
					//ngCtrl.modalRelationType;
					//ngCtrl.modalDataKind;
					if (selectedLookupList != null) {
						defaultLookupList = selectedLookupList;
					}

					//Current record is Origin
					if (ngCtrl.modalDataKind == "origin") {
						//Find if the target field is required
						var targetRequiredField = false;
						var modalCurrrentRelation = ngCtrl.getRelation(ngCtrl.modalSelectedItem.relationName);
						for (var m = 0; m < entityMeta.fields.length; m++) {
							if (entityMeta.fields[m].id == modalCurrrentRelation.targetFieldId) {
								targetRequiredField = entityMeta.fields[m].required;
							}
						}
						if (targetRequiredField && ngCtrl.modalRelationType == 1) {
							//Case 1 - Solves the problem when the target field is required, but we are currently looking on the origin field holding record. 
							//In this case we cannot allow this relation to be managed from this origin record as the change will leave the old target record with null for its required field
							var lockedChangeResponse = {
								success: false,
								message: "This is a relation field, that cannot be managed from this record. Try managing it from the related <<" + entityMeta.label + ">> entity record",
								object: null
							}
							getListRecordsSuccessCallback(lockedChangeResponse);
						}
						else {
							webvellaAreasService.getListRecords(defaultLookupList.name, entityMeta.name, 1, null, getListRecordsSuccessCallback, errorCallback);
						}
					}
					else if (ngCtrl.modalDataKind == "target") {
						//Current records is Target
						webvellaAreasService.getListRecords(defaultLookupList.name, entityMeta.name, 1, null, getListRecordsSuccessCallback, errorCallback);
					}
				}
			}

			webvellaAdminService.getEntityMeta(item.entityName, getEntityMetaSuccessCallback, errorCallback);

			return defer.promise;
		}

		//#endregion
		//#endregion

		$log.debug('webvellaAreas>record-create> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	}


	//#region < Modal Controllers >

	//#region << Manage relation Modal >>
	//Test to unify all modals - Single select, multiple select, click to select
	CreateRelationFieldModalController.$inject = ['ngCtrl', '$uibModalInstance', '$log', '$q', '$stateParams', 'modalMode', 'resolvedLookupRecords',
        'selectedDataKind', 'selectedItem', 'selectedRelationType', 'webvellaAdminService', 'webvellaAreasService', 'webvellaRootService', 'ngToast', '$timeout', '$state'];
	/* @ngInject */
	function CreateRelationFieldModalController(ngCtrl, $uibModalInstance, $log, $q, $stateParams, modalMode, resolvedLookupRecords,
        selectedDataKind, selectedItem, selectedRelationType, webvellaAdminService, webvellaAreasService, webvellaRootService, ngToast, $timeout, $state) {

		$log.debug('webvellaAdmin>entities>deleteFieldModal> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var popupCtrl = this;
		popupCtrl.currentPage = 1;
		popupCtrl.parentData = fastCopy(ngCtrl);
		popupCtrl.selectedItem = fastCopy(selectedItem);
		popupCtrl.modalMode = fastCopy(modalMode);
		popupCtrl.hasWarning = false;
		popupCtrl.warningMessage = "";
		//Init
		var index = selectedItem.dataName.lastIndexOf('$') + 1;
		var dummyFieldName = selectedItem.dataName.slice(index, selectedItem.dataName.length);
		var idFieldPrefix = selectedItem.dataName.slice(0, index);
		popupCtrl.currentlyAttachedIds = [];
		if (popupCtrl.parentData.entityData[idFieldPrefix + "id"] && popupCtrl.parentData.entityData[idFieldPrefix + "id"].length > 0) {
			popupCtrl.currentlyAttachedIds = popupCtrl.parentData.entityData[idFieldPrefix + "id"];
		}
		popupCtrl.getRelationLabel = ngCtrl.getRelationLabel;


		//Get the default lookup list for the entity
		if (resolvedLookupRecords.success) {
			popupCtrl.relationLookupList = fastCopy(resolvedLookupRecords.object);
		}
		else {
			popupCtrl.hasWarning = true;
			popupCtrl.warningMessage = resolvedLookupRecords.message;
		}

		//#region << Search >>
		popupCtrl.checkForSearchEnter = function (e) {
			var code = (e.keyCode ? e.keyCode : e.which);
			if (code == 13) { //Enter keycode
				popupCtrl.submitSearchQuery();
			}
		}
		popupCtrl.submitSearchQuery = function () {
			function successCallback(response) {
				popupCtrl.relationLookupList = fastCopy(response.object);
			}
			function errorCallback(response) { }

			if (popupCtrl.searchQuery) {
				popupCtrl.searchQuery = popupCtrl.searchQuery.trim();
			}
			webvellaAreasService.getListRecords(popupCtrl.relationLookupList.meta.name, popupCtrl.selectedItem.entityName, 1, popupCtrl.searchQuery, successCallback, errorCallback);
		}
		//#endregion

		//#region << Paging >>
		popupCtrl.selectPage = function (page) {
			// Process
			function successCallback(response) {
				popupCtrl.relationLookupList = fastCopy(response.object);
				popupCtrl.currentPage = page;
			}

			function errorCallback(response) {

			}

			webvellaAreasService.getListRecords(popupCtrl.relationLookupList.meta.name, popupCtrl.selectedItem.entityName, page, null, successCallback, errorCallback);
		}

		//#endregion

		//#region << Logic >>

		//Render field values
		popupCtrl.renderFieldValue = webvellaAreasService.renderFieldValue;

		popupCtrl.isSelectedRecord = function (recordId) {
			if (popupCtrl.currentlyAttachedIds) {
				return popupCtrl.currentlyAttachedIds.indexOf(recordId) > -1
			}
			else {
				return false;
			}
		}

		//Single record before save
		popupCtrl.selectSingleRecord = function (record) {
			var returnObject = {
				relationName: popupCtrl.selectedItem.relationName,
				dataKind: selectedDataKind,
				selectedRecordId: record.id
			};
			$uibModalInstance.close(returnObject);
		};

		// Multiple records before save
		popupCtrl.attachRecord = function (record) {
			//Update the currentlyAttachedIds
			var elementIndex = popupCtrl.currentlyAttachedIds.indexOf(record.id);
			if (elementIndex == -1) {
				//this is the normal case
				popupCtrl.currentlyAttachedIds.push(record.id);
			}
			else {
				//if it is already in the highligted list there is probably some miscalculation from previous operation, but for now we will do nothing
			}

		}
		popupCtrl.detachRecord = function (record) {
			//Update the currentlyAttachedIds for highlight
			var elementIndex = popupCtrl.currentlyAttachedIds.indexOf(record.id);
			if (elementIndex > -1) {
				//this is the normal case
				popupCtrl.currentlyAttachedIds.splice(elementIndex, 1);
			}
			else {
				//if it is already not in the highligted list there is probably some miscalculation from previous operation, but for now we will do nothing
			}
		}
		popupCtrl.saveRelationChanges = function () {
			var returnObject = {
				relationName: popupCtrl.selectedItem.relationName,
				dataKind: selectedDataKind,
				selectedRecordIdArray: popupCtrl.currentlyAttachedIds
			};
			$uibModalInstance.close(returnObject);
			//category_id
		};


		//#endregion


		popupCtrl.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};

		//#endregion

		$log.debug('webvellaAdmin>entities>createEntityModal> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	};
	//#endregion 

	//#endregion


})();



