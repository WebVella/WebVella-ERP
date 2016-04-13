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
					controllerAs: 'contentData'
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
		var contentData = this;
		contentData.stateParams = $stateParams;
		contentData.validation = {};
		//#region <<Set pageTitle>>
		contentData.pageTitle = "Area Entities | " + pageTitle;
		webvellaRootService.setPageTitle(contentData.pageTitle);
		contentData.siteMap = fastCopy(resolvedSitemap);
		contentData.currentArea = null;
		for (var i = 0; i < contentData.siteMap.data.length; i++) {
			if (contentData.siteMap.data[i].name == $state.params.areaName) {
				contentData.currentArea = contentData.siteMap.data[i];
			};
		}
		webvellaRootService.setBodyColorClass(contentData.currentArea.color);
		//#endregion

		//#region << Initialize current entity >>
		contentData.currentEntity = fastCopy(resolvedCurrentEntityMeta);
		//#endregion

		//#region << Set environment >> /////////////////////

		contentData.createViewRegion = null;
		contentData.createView = null;
		for (var i = 0; i < contentData.currentEntity.recordViews.length; i++) {
			if (contentData.currentEntity.recordViews[i].type === "create" && contentData.currentEntity.recordViews[i].default) {
				contentData.createView = contentData.currentEntity.recordViews[i];
				for (var j = 0; j < contentData.currentEntity.recordViews[i].regions.length; j++) {
					if (contentData.currentEntity.recordViews[i].regions[j].name === "content") {
						contentData.createViewRegion = contentData.currentEntity.recordViews[i].regions[j];
					}
				}
			}
		}

		//Initialize entityRecordData
		contentData.entityData = {};
		contentData.files = {}; // this is the data wrapper for the temporary upload objects that will be used in the html and for which we will generate watches below
		contentData.progress = {}; //Needed for file and image uploads
		var availableViewFields = [];
		//Init default values of fields
		if (contentData.createViewRegion != null) {
			availableViewFields = webvellaAdminService.getItemsFromRegion(contentData.createViewRegion);
			for (var j = 0; j < availableViewFields.length; j++) {
				if (availableViewFields[j].type === "field") {
					switch (availableViewFields[j].meta.fieldType) {

						case 2: //Checkbox
							contentData.entityData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							break;

						case 3: //Currency
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								contentData.entityData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 4: //Date
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								if (availableViewFields[j].meta.useCurrentTimeAsDefaultValue) {
									contentData.entityData[availableViewFields[j].meta.name] = moment().toISOString();
								}
								else if (availableViewFields[j].meta.defaultValue) {
									contentData.entityData[availableViewFields[j].meta.name] = moment(availableViewFields[j].meta.defaultValue).toISOString();
								}
							}
							break;
						case 5: //Date
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								if (availableViewFields[j].meta.useCurrentTimeAsDefaultValue) {
									contentData.entityData[availableViewFields[j].meta.name] = moment().toISOString();
								}
								else if (availableViewFields[j].meta.defaultValue) {
									contentData.entityData[availableViewFields[j].meta.name] = moment(availableViewFields[j].meta.defaultValue).toISOString();
								}
							}
							break;
						case 6: //Email
							break;
						case 7: //File
							contentData.progress[availableViewFields[j].meta.name] = 0;
							if (availableViewFields[j].meta.required) {
								contentData.entityData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 8: //HTML
							if (availableViewFields[j].meta.required) {
								contentData.entityData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 9: //Image
							contentData.progress[availableViewFields[j].meta.name] = 0;
							if (availableViewFields[j].meta.required) {
								contentData.entityData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 10: //TextArea
							if (availableViewFields[j].meta.required) {
								contentData.entityData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 11: //Multiselect
							if (availableViewFields[j].meta.required) {
								contentData.entityData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 12: //Number
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								contentData.entityData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 13: //Password
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								//Does not have default value
								//contentData.entityData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
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
								//contentData.entityData[availableViewFields[j].meta.name] = resultPercentage;
							}
							break;
						case 15: //Phone
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								contentData.entityData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 16: //Guid
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								contentData.entityData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 17: //Dropdown
							if (availableViewFields[j].meta.required && availableViewFields[j].meta.defaultValue) {
								contentData.entityData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 18: //Text
							//if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
							//	contentData.entityData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							//}
							break;
						case 19: //URL
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								contentData.entityData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
					}


				}
			}
		}



		// << File >>
		contentData.uploadedFileName = "";
		contentData.upload = function (file, item) {
			if (file != null) {
				contentData.uploadedFileName = item.dataName;
				contentData.moveSuccessCallback = function (response) {
					$timeout(function () {
						contentData.entityData[contentData.uploadedFileName] = response.object.url;
					}, 1);
				}

				contentData.uploadSuccessCallback = function (response) {
					var tempPath = response.object.url;
					var fileName = response.object.filename;
					var targetPath = "/fs/" + contentData.currentEntity.name + "/" + newGuid() + "/" + fileName;
					var overwrite = false;
					webvellaAdminService.moveFileFromTempToFS(tempPath, targetPath, overwrite, contentData.moveSuccessCallback, contentData.uploadErrorCallback);
				}
				contentData.uploadErrorCallback = function (response) {
					alert(response.message);
				}
				contentData.uploadProgressCallback = function (response) {
					$timeout(function () {
						contentData.progress[contentData.uploadedFileName] = parseInt(100.0 * response.loaded / response.total);
					}, 1);
				}
				webvellaAdminService.uploadFileToTemp(file, item.meta.name, contentData.uploadProgressCallback, contentData.uploadSuccessCallback, contentData.uploadErrorCallback);
			}
		};

		contentData.deleteFileUpload = function (item) {
			var fieldName = item.dataName;
			var filePath = contentData.entityData[fieldName];

			function deleteSuccessCallback(response) {
				$timeout(function () {
					contentData.entityData[fieldName] = null;
					contentData.progress[fieldName] = 0;
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
		//Should use scope as it is not working with contentData
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


		contentData.toggleSectionCollapse = function (section) {
			section.collapsed = !section.collapsed;
		}

		contentData.calendars = {};
		contentData.openCalendar = function (event, name) {
			contentData.calendars[name] = true;
		}

		//#endregion

		//#region << Entity relations functions >>
		contentData.relationsList = fastCopy(resolvedEntityRelationsList);

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

		contentData.hasFieldFromRelationValue = function (itemDataName) {
			var dataNameArray =  itemDataName.split('$');
			if (contentData.entityData["$" + dataNameArray[2] + "." + "id"]) {
				return true;
			}
			else {
				return false;
			}
		}

		contentData.removeFieldFromRelationValue = function (itemDataName) {
			var dataNameArray =  itemDataName.split('$');
			$timeout(function () {
				delete contentData.entityData["$" + dataNameArray[2] + "." + "id"];
				delete contentData.dummyFields[itemDataName];
			}, 10);
		}


		//#endregion

		//#region << Render >>
		contentData.calculatefieldWidths = webvellaAdminService.calculateViewFieldColsFromGridColSize;
		contentData.checkUserEntityPermissions = function (permissionsCsv) {
			return fastCopy(webvellaRootService.userHasEntityPermissions(contentData.currentEntity, permissionsCsv));
		}


		contentData.getRelationLabel = function (item) {
			if (item.fieldLabel) {
				return item.fieldLabel
			}
			else {
				var relationName = item.relationName;
				var relation = findInArray(contentData.relationsList, "name", relationName);
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
		contentData.selectedList = {};
		for (var j = 0; j < contentData.currentEntity.recordLists.length; j++) {
			if (contentData.currentEntity.recordLists[j].name === $stateParams.listName) {
				contentData.selectedList = contentData.currentEntity.recordLists[j];
				break;
			}
		}



		//Generate the proper view name for the record details screen (needed in the redirect)
		contentData.generateViewName = function (record) {
			//default is the selected view in the area
			var result = fastCopy(contentData.selectedList.name);

			if (contentData.selectedList.viewNameOverride && ccontentData.selectedList.viewNameOverride.length > 0) {
				var arrayOfTemplateKeys = contentData.selectedList.viewNameOverride.match(/\{([\$\w]+)\}/g); //Include support for matching also data from relations which include $ symbol
				var resultStringStorage = fastCopy(contentData.selectedList.viewNameOverride);

				for (var i = 0; i < arrayOfTemplateKeys.length; i++) {
					if (arrayOfTemplateKeys[i] === "{areaName}" || arrayOfTemplateKeys[i] === "{entityName}" || arrayOfTemplateKeys[i] === "{filter}" || arrayOfTemplateKeys[i] === "{page}" || arrayOfTemplateKeys[i] === "{searchQuery}") {
						switch (arrayOfTemplateKeys[i]) {
							case "{areaName}":
								resultStringStorage = resultStringStorage.replace(arrayOfTemplateKeys[i], convertToSlug($stateParams.areaName));
								break;
							case "{entityName}":
								resultStringStorage = resultStringStorage.replace(arrayOfTemplateKeys[i], convertToSlug($stateParams.entityName));
								break;
							case "{filter}":
								resultStringStorage = resultStringStorage.replace(arrayOfTemplateKeys[i], convertToSlug($stateParams.filter));
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
		contentData.create = function (redirectTarget) {
			//Validate 
			contentData.validation = {};
			for (var k = 0; k < availableViewFields.length; k++) {
				if (availableViewFields[k].type === "field" && availableViewFields[k].meta.required) {
					if (contentData.entityData[availableViewFields[k].dataName] == null || contentData.entityData[availableViewFields[k].dataName] == "") {
						contentData.validation[availableViewFields[k].dataName] = true;
						contentData.validation.hasError = true;
						contentData.validation.errorMessage = "A required data is missing!";
					}
				}
			}
			if (!contentData.validation.hasError) {
				//Alter some data before save
				for (var k = 0; k < availableViewFields.length; k++) {
					if (availableViewFields[k].type === "field") {
						switch (availableViewFields[k].meta.fieldType) {
							case 4: //Date
								if (contentData.entityData[availableViewFields[k].dataName] != null) {
									contentData.entityData[availableViewFields[k].dataName] = moment(contentData.entityData[availableViewFields[k].dataName]).startOf('day').utc().toISOString();
								}
								break;
							case 5: //Date & Time
								if (contentData.entityData[availableViewFields[k].dataName] != null) {
									contentData.entityData[availableViewFields[k].dataName] = moment(contentData.entityData[availableViewFields[k].dataName]).startOf('minute').utc().toISOString();
								}
								break;
							case 14: //Percent
								//need to convert to decimal 0 <= val <= 100 Divide by 100
								//Hack for proper javascript division
								$scope.Math = window.Math;
								var helpNumber = 10000000;
								var multipliedValue = $scope.Math.round(contentData.entityData[availableViewFields[k].dataName] * helpNumber);
								contentData.entityData[availableViewFields[k].dataName] = multipliedValue / (100 * helpNumber);
								break;
						}
					}
					else if (availableViewFields[k].type === "fieldFromRelation") {
						//Currently no need to remove field from relations that are not id, as they are not attached to the entityData anyway
					}
				}
				contentData.entityData["created_on"] = moment().utc().toISOString();
				//popupData.entityData["created_by"] = ""; //TODO: put the current user id after the users are implemented
				switch (redirectTarget) {
					case "details":
						webvellaAdminService.createRecord(contentData.currentEntity.name, contentData.entityData, successCallback, errorCallback);
						break;
					case "list":
						webvellaAdminService.createRecord(contentData.currentEntity.name, contentData.entityData, successCallbackList, errorCallback);
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

		contentData.cancel = function () {
			$timeout(function () {
				$state.go("webvella-entity-records", {
					areaName: $stateParams.areaName,
					entityName: $stateParams.entityName,
					listName: $stateParams.listName,
					filter: $stateParams.filter,
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
				var detailsViewName = contentData.generateViewName(response.object);

				$state.go("webvella-areas-record-view", {
					areaName: $stateParams.areaName,
					entityName: $stateParams.entityName,
					recordId: response.object.data[0].id,
					viewName: detailsViewName,
					auxPageName: "*",
					filter: $stateParams.filter,
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
				var detailsViewName = contentData.generateViewName(response.object);

				$state.go("webvella-entity-records", {
					areaName: $stateParams.areaName,
					entityName: $stateParams.entityName,
					listName: $stateParams.listName,
					filter: $stateParams.filter,
					page: $stateParams.page

				}, { reload: true });
			}, 0);
		}

		function errorCallback(response) {


		}

		//#endregion

		//#region << Modals >>

		//#region << Relation field >>
		contentData.dummyFields = {};  //These fields will present data to the user that will not be submitted
		////////////////////
		// Single selection modal used in 1:1 relation and in 1:N when the currently viewed entity is a target in this relation
		contentData.openManageRelationFieldModal = function (item, relationType, dataKind) {
			//relationType = 1 (one-to-one) , 2(one-to-many), 3(many-to-many)
			//dataKind - target, origin, origin-target

			//Select ONE item modal
			if (relationType == 1 || (relationType == 2 && dataKind == "target")) {
				var modalInstance = $uibModal.open({
					animation: false,
					templateUrl: 'manageRelationFieldModal.html',
					controller: 'CreateRelationFieldModalController',
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
						contentData.dummyFields[item.dataName] = webvellaAreasService.renderFieldValue(response.object.data[0][dummyFieldName], item.meta);
						//4.set in the create model $field$relation_name$id -> is this is the only way to be sure that the value will be unique and the api will not produce error
						contentData.entityData["$" + dataNameArray[2] + "." + "id"] = returnObject.selectedRecordId;
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
						contentData.dummyFields[item.dataName] = [];
						contentData.entityData[idFieldPrefix + "id"] = [];
						for (var i = 0; i < response.object.data.length; i++) {
							contentData.dummyFields[item.dataName].push(webvellaAreasService.renderFieldValue(response.object.data[i][dummyFieldName], item.meta));
							//4.set in the create model $field$relation_name$id -> is this is the only way to be sure that the value will be unique and the api will not produce error
							contentData.entityData[idFieldPrefix + "id"].push(response.object.data[i]["id"]);
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
					   	contentData.dummyFields[item.dataName] = [];
						contentData.entityData[idFieldPrefix + "id"] = [];
					}
					else {
						var recordIdCSV = returnObject.selectedRecordIdArray.join(',');
						var fieldCSV = fieldsArray.join(',');
						webvellaAdminService.getRecords(recordIdCSV, fieldCSV, item.entityName, modalCase1SuccessCallback, modalCase1ErrorCallback);
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
			contentData.modalSelectedItem = fastCopy(item);
			contentData.modalRelationType = fastCopy(relationType);
			contentData.modalDataKind = fastCopy(dataKind);
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
				var selectedLookupListName = contentData.modalSelectedItem.fieldLookupList;
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

					//var gg = contentData.modalSelectedItem;
					//contentData.modalRelationType;
					//contentData.modalDataKind;
					if (selectedLookupList != null) {
						defaultLookupList = selectedLookupList;
					}

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
								object: null
							}
							getListRecordsSuccessCallback(lockedChangeResponse);
						}
						else {
							webvellaAreasService.getListRecords(defaultLookupList.name, entityMeta.name, "all", 1, null, getListRecordsSuccessCallback, errorCallback);
						}
					}
					else if (contentData.modalDataKind == "target") {
						//Current records is Target
						webvellaAreasService.getListRecords(defaultLookupList.name, entityMeta.name, "all", 1, null, getListRecordsSuccessCallback, errorCallback);
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
	CreateRelationFieldModalController.$inject = ['contentData', '$uibModalInstance', '$log', '$q', '$stateParams', 'modalMode', 'resolvedLookupRecords',
        'selectedDataKind', 'selectedItem', 'selectedRelationType', 'webvellaAdminService', 'webvellaAreasService', 'webvellaRootService', 'ngToast', '$timeout', '$state'];
	/* @ngInject */
	function CreateRelationFieldModalController(contentData, $uibModalInstance, $log, $q, $stateParams, modalMode, resolvedLookupRecords,
        selectedDataKind, selectedItem, selectedRelationType, webvellaAdminService, webvellaAreasService, webvellaRootService, ngToast, $timeout, $state) {

		$log.debug('webvellaAdmin>entities>deleteFieldModal> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var popupData = this;
		popupData.currentPage = 1;
		popupData.parentData = fastCopy(contentData);
		popupData.selectedItem = fastCopy(selectedItem);
		popupData.modalMode = fastCopy(modalMode);
		popupData.hasWarning = false;
		popupData.warningMessage = "";
		//Init
		var index = selectedItem.dataName.lastIndexOf('$') + 1;
		var dummyFieldName = selectedItem.dataName.slice(index, selectedItem.dataName.length);
		var idFieldPrefix = selectedItem.dataName.slice(0, index);
		popupData.currentlyAttachedIds = [];
		if (popupData.parentData.entityData[idFieldPrefix + "id"] && popupData.parentData.entityData[idFieldPrefix + "id"].length > 0) {
			popupData.currentlyAttachedIds = popupData.parentData.entityData[idFieldPrefix + "id"];
		}
		popupData.getRelationLabel = contentData.getRelationLabel;


		//Get the default lookup list for the entity
		if (resolvedLookupRecords.success) {
			popupData.relationLookupList = fastCopy(resolvedLookupRecords.object);
		}
		else {
			popupData.hasWarning = true;
			popupData.warningMessage = resolvedLookupRecords.message;
		}

		//#region << Search >>
		popupData.checkForSearchEnter = function (e) {
			var code = (e.keyCode ? e.keyCode : e.which);
			if (code == 13) { //Enter keycode
				popupData.submitSearchQuery();
			}
		}
		popupData.submitSearchQuery = function () {
			function successCallback(response) {
				popupData.relationLookupList = fastCopy(response.object);
			}
			function errorCallback(response) { }

			if (popupData.searchQuery) {
				popupData.searchQuery = popupData.searchQuery.trim();
			}
			webvellaAreasService.getListRecords(popupData.relationLookupList.meta.name, popupData.selectedItem.entityName, "all", 1, popupData.searchQuery, successCallback, errorCallback);
		}
		//#endregion

		//#region << Paging >>
		popupData.selectPage = function (page) {
			// Process
			function successCallback(response) {
				popupData.relationLookupList = fastCopy(response.object);
				popupData.currentPage = page;
			}

			function errorCallback(response) {

			}

			webvellaAreasService.getListRecords(popupData.relationLookupList.meta.name, popupData.selectedItem.entityName, "all", page, null, successCallback, errorCallback);
		}

		//#endregion

		//#region << Logic >>

		//Render field values
		popupData.renderFieldValue = webvellaAreasService.renderFieldValue;

		popupData.isSelectedRecord = function (recordId) {
			if (popupData.currentlyAttachedIds) {
				return popupData.currentlyAttachedIds.indexOf(recordId) > -1
			}
			else {
				return false;
			}
		}

		//Single record before save
		popupData.selectSingleRecord = function (record) {
			var returnObject = {
				relationName: popupData.selectedItem.relationName,
				dataKind: selectedDataKind,
				selectedRecordId: record.id
			};
			$uibModalInstance.close(returnObject);
		};

		// Multiple records before save
		popupData.attachRecord = function (record) {
			//Update the currentlyAttachedIds
			var elementIndex = popupData.currentlyAttachedIds.indexOf(record.id);
			if (elementIndex == -1) {
				//this is the normal case
				popupData.currentlyAttachedIds.push(record.id);
			}
			else {
				//if it is already in the highligted list there is probably some miscalculation from previous operation, but for now we will do nothing
			}

		}
		popupData.detachRecord = function (record) {
			//Update the currentlyAttachedIds for highlight
			var elementIndex = popupData.currentlyAttachedIds.indexOf(record.id);
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
				selectedRecordIdArray: popupData.currentlyAttachedIds
			};
			$uibModalInstance.close(returnObject);
			//category_id
		};


		//#endregion


		popupData.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};

		//#endregion

		$log.debug('webvellaAdmin>entities>createEntityModal> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	};
	//#endregion 

	//#endregion


})();



