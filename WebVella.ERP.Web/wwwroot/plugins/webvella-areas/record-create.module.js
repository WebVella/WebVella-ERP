/* entities.module.js */

/**
* @desc this module manages the application home desktop screen
*/

(function () {
	'use strict';

	angular
        .module('webvellaAreas') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
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
					templateUrl: '/plugins/webvella-areas/topnav.view.html?v=' + htmlCacheBreaker,
					controllerAs: 'topnavData'
				},
				"sidebarView": {
					controller: 'WebVellaAreasSidebarController',
					templateUrl: '/plugins/webvella-areas/sidebar.view.html?v=' + htmlCacheBreaker,
					controllerAs: 'sidebarData'
				},
				"contentView": {
					controller: 'WebVellaAreasRecordCreateController',
					templateUrl: '/plugins/webvella-areas/record-create.view.html?v=' + htmlCacheBreaker,
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
        'resolvedSitemap', '$timeout', 'ngToast', 'wvAppConstants', 'resolvedCurrentEntityMeta', 'resolvedEntityRelationsList'];

	/* @ngInject */
	function controller($filter, $uibModal, $log, $q, $rootScope, $state, $stateParams, $scope, pageTitle, webvellaRootService, webvellaAdminService, webvellaAreasService,
        resolvedSitemap, $timeout, ngToast, wvAppConstants, resolvedCurrentEntityMeta, resolvedEntityRelationsList) {
		$log.debug('webvellaAreas>record-create> BEGIN controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var contentData = this;
		contentData.stateParams = $stateParams;

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
		for (var l = 0; l < contentData.currentEntity.fields.length; l++) {
			contentData.entityData[contentData.currentEntity.fields[l].name] = null;
		}
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
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								contentData.entityData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
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
			//'extraPlugins': "imagebrowser",//"imagebrowser,mediaembed",
			//imageBrowser_listUrl: '/api/v1/ckeditor/gallery',
			//filebrowserBrowseUrl: '/api/v1/ckeditor/files',
			//filebrowserImageUploadUrl: '/api/v1/ckeditor/images',
			//filebrowserUploadUrl: '/api/v1/ckeditor/files',
			toolbarLocation: 'top',
			toolbar: 'full',
			toolbar_full: [
				{
					name: 'basicstyles',
					items: ['Bold', 'Italic', 'Strike', 'Underline']
				},
				{ name: 'paragraph', items: ['BulletedList', 'NumberedList', 'Blockquote'] },
				{ name: 'editing', items: ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'] },
				{ name: 'links', items: ['Link', 'Unlink', 'Anchor'] },
				{ name: 'tools', items: ['SpellChecker', 'Maximize'] },
				{ name: 'clipboard', items: ['Undo', 'Redo'] },
				{ name: 'styles', items: ['Format', 'FontSize', 'TextColor', 'PasteText', 'PasteFromWord', 'RemoveFormat'] },
				{ name: 'insert', items: ['Image', 'Table', 'SpecialChar', 'MediaEmbed'] }, '/',
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


		//#region << Render >>
		contentData.checkUserEntityPermissions = function(permissionsCsv){
			return fastCopy(webvellaRootService.userHasEntityPermissions(contentData.currentEntity,permissionsCsv));
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
							if(!dataName.startsWith('$')){
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
			//Alter some data before save
			for (var k = 0; k < availableViewFields.length; k++) {
				if (availableViewFields[k].type === "field") {
					switch (availableViewFields[k].meta.fieldType) {
						case 4: //Date
							if(contentData.entityData[availableViewFields[k].meta.name] != null){
								contentData.entityData[availableViewFields[k].meta.name] = moment(contentData.entityData[availableViewFields[k].meta.name]).startOf('day').utc().toISOString();
							}
							break;
						case 5: //Date & Time
							if(contentData.entityData[availableViewFields[k].meta.name] != null){
								contentData.entityData[availableViewFields[k].meta.name] = moment(contentData.entityData[availableViewFields[k].meta.name]).startOf('minute').utc().toISOString();
							}
							break;
						case 14: //Percent
							//need to convert to decimal 0 <= val <= 100 Divide by 100
							//Hack for proper javascript division
							$scope.Math = window.Math;
							var helpNumber = 10000000;
							var multipliedValue = $scope.Math.round(contentData.entityData[availableViewFields[k].meta.name] * helpNumber);
							contentData.entityData[availableViewFields[k].meta.name] = multipliedValue / (100 * helpNumber);
							break;
					}
				}
			}
			contentData.entityData["created_on"] = moment().utc().toISOString();
			//popupData.entityData["created_by"] = ""; //TODO: put the current user id after the users are implemented
			switch(redirectTarget){
				case "details":
					webvellaAdminService.createRecord(contentData.currentEntity.name, contentData.entityData, successCallback, errorCallback);
					break;
				case "list":
					webvellaAdminService.createRecord(contentData.currentEntity.name, contentData.entityData, successCallbackList, errorCallback);
					break;
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
					auxPageName:"*",
					filter: $stateParams.filter,
					page: $stateParams.page

				}, { reload: true });

				//$state.go("webvella-entity-records", {
				//	areaName: $stateParams.areaName,
				//	entityName: $stateParams.entityName,
				//	listName: $stateParams.listName,
				//	filter: $stateParams.filter,
				//	page: $stateParams.page

				//}, { reload: true });
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
			popupData.hasError = true;
			popupData.errorMessage = response.message;
			popupData["currencyErrorMessage"] = "Bad new message";
			popupData["currencyError"] = true;

		}

		//#engregion


		$log.debug('webvellaAreas>record-create> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	}


	//#region < Modal Controllers >

	//#endregion


})();



