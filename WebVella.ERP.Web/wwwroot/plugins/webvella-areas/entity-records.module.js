/* entities.module.js */

/**
* @desc this module manages the application home desktop screen
*/

(function () {
	'use strict';

	angular
        .module('webvellaAreas') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAreaEntityRecordsController', controller)
		.controller('createRecordModalController', createRecordModalController);


	// Configuration ///////////////////////////////////
	config.$inject = ['$stateProvider'];

	/* @ngInject */
	function config($stateProvider) {
		$stateProvider.state('webvella-entity-records', {
			parent: 'webvella-areas-base',
			url: '/:areaName/:entityName/:listName/:filter/:page',
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
					controller: 'WebVellaAreaEntityRecordsController',
					templateUrl: '/plugins/webvella-areas/entity-records.view.html',
					controllerAs: 'contentData'
				}
			},
			resolve: {
				resolvedListRecords: resolveListRecords,
				resolvedCurrentEntityMeta: resolveCurrentEntityMeta,
				resolvedCurrentArea: resolveCurrentArea,
				resolvedEntityRelationsList: resolveEntityRelationsList
			},
			data: {

			}
		});
	};


	//#region << Run //////////////////////////////////////
	run.$inject = ['$log'];

	/* @ngInject */
	function run($log) {
		$log.debug('webvellaAreas>entities> BEGIN module.run');

		$log.debug('webvellaAreas>entities> END module.run');
	};

	//#endregion

	//#region << Resolve Function >>
	resolveListRecords.$inject = ['$q', '$log', 'webvellaAreasService', '$state', '$stateParams'];
	/* @ngInject */
	function resolveListRecords($q, $log, webvellaAreasService, $state, $stateParams) {
		$log.debug('webvellaDesktop>browse> BEGIN state.resolved');
		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			defer.resolve(response.object);
		}

		function errorCallback(response) {
			defer.resolve(response.object);
		}

		webvellaAreasService.getListRecords($stateParams.listName, $stateParams.entityName, $stateParams.filter, $stateParams.page, successCallback, errorCallback);

		// Return
		$log.debug('webvellaDesktop>browse> END state.resolved');
		return defer.promise;
	}

	resolveCurrentEntityMeta.$inject = ['$q', '$log', 'webvellaAdminService', '$state', '$stateParams'];
	/* @ngInject */
	function resolveCurrentEntityMeta($q, $log, webvellaAdminService, $state, $stateParams) {
		$log.debug('webvellaDesktop>resolveCurrentEntityMeta> BEGIN state.resolved');
		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			defer.resolve(response.object);
		}

		function errorCallback(response) {
			defer.resolve(response.object);
		}

		webvellaAdminService.getEntityMeta($stateParams.entityName, successCallback, errorCallback);

		// Return
		$log.debug('webvellaDesktop>resolveCurrentEntityMeta> END state.resolved');
		return defer.promise;
	}


	resolveCurrentArea.$inject = ['$q', '$log', 'webvellaAdminService', '$state', '$stateParams'];
	/* @ngInject */
	function resolveCurrentArea($q, $log, webvellaAdminService, $state, $stateParams) {
		$log.debug('webvellaDesktop>resolveCurrentEntityMeta> BEGIN state.resolved');
		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			defer.resolve(response.object);
		}

		function errorCallback(response) {
			defer.resolve(response.object);
		}

		webvellaAdminService.getAreaByName($stateParams.areaName, successCallback, errorCallback);

		// Return
		$log.debug('webvellaDesktop>resolveCurrentEntityMeta> END state.resolved');
		return defer.promise;
	}

	resolveEntityRelationsList.$inject = ['$q', '$log', 'webvellaAdminService', '$stateParams', '$state', '$timeout'];
	/* @ngInject */
	function resolveEntityRelationsList($q, $log, webvellaAdminService, $stateParams, $state, $timeout) {
		$log.debug('webvellaAdmin>entity-details> BEGIN state.resolved');
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
				defer.resolve(response.object);
			}
		}

		webvellaAdminService.getRelationsList(successCallback, errorCallback);

		// Return
		$log.debug('webvellaAdmin>entity-details> END state.resolved');
		return defer.promise;
	}



	//#endregion


	// Controller ///////////////////////////////
	controller.$inject = ['$filter', '$log', '$modal', '$rootScope', '$state', '$stateParams', 'pageTitle', 'webvellaRootService',
        'resolvedSitemap', '$timeout', 'webvellaAreasService', 'resolvedListRecords', 'resolvedCurrentEntityMeta', 'resolvedCurrentArea', 'resolvedEntityRelationsList'];

	/* @ngInject */
	function controller($filter, $log, $modal, $rootScope, $state, $stateParams, pageTitle, webvellaRootService,
        resolvedSitemap, $timeout, webvellaAreasService, resolvedListRecords, resolvedCurrentEntityMeta, resolvedCurrentArea, resolvedEntityRelationsList) {
		$log.debug('webvellaAreas>entities> BEGIN controller.exec');
		/* jshint validthis:true */
		var contentData = this;
		contentData.records = angular.copy(resolvedListRecords.data);
		contentData.recordsMeta = angular.copy(resolvedListRecords.meta);
		contentData.relationsMeta = resolvedEntityRelationsList;

		//#region << Set Environment >>
		contentData.pageTitle = "Area Entities | " + pageTitle;
		webvellaRootService.setPageTitle(contentData.pageTitle);
		contentData.currentArea = webvellaAreasService.getCurrentAreaFromSitemap($stateParams.areaName, resolvedSitemap.data);

		webvellaRootService.setBodyColorClass(contentData.currentArea.color);

		//Get the current meta
		contentData.entity = angular.copy(resolvedCurrentEntityMeta);
		contentData.area = angular.copy(resolvedCurrentArea.data[0]);
		contentData.area.subscriptions = angular.fromJson(contentData.area.subscriptions);
		contentData.areaEntitySubscription = {};
		for (var i = 0; i < contentData.area.subscriptions.length; i++) {
			if (contentData.area.subscriptions[i].name === contentData.entity.name) {
				contentData.areaEntitySubscription = contentData.area.subscriptions[i];
				break;
			}
		}


		//Select default details view
		contentData.selectedView = {};
		for (var j = 0; j < contentData.entity.recordViews.length; j++) {
			if (contentData.entity.recordViews[j].name === contentData.areaEntitySubscription.view.name) {
				contentData.selectedView = contentData.entity.recordViews[j];
				break;
			}
		}
		contentData.currentPage = parseInt($stateParams.page);
		//Select the current list view details
		contentData.currentListView = {};
		for (var i = 0; i < contentData.entity.recordLists.length; i++) {
			if (contentData.entity.recordLists[i].name === $stateParams.listName) {
				contentData.currentListView = contentData.entity.recordLists[i];
			}
		}

		//#endregion

		//#region << Logic >> //////////////////////////////////////

		contentData.goDesktopBrowse = function () {
			webvellaRootService.GoToState($state, "webvella-desktop-browse", {});
		}

		contentData.selectPage = function (page) {
			var params = {
				areaName: $stateParams.areaName,
				entityName: $stateParams.entityName,
				listName: $stateParams.listName,
				filter: $stateParams.filter,
				page: page
			};
			webvellaRootService.GoToState($state, $state.current.name, params);
		}

		//#endregion

		//#region << Columns render>> //////////////////////////////////////
		//1.Auto increment
		contentData.getAutoIncrementString = function (record, field) {
			var fieldValue = record[field.dataName];
			if (!fieldValue) {
				return "";
			}
			else if (field.meta.displayFormat) {
				return field.meta.displayFormat.replace("{0}", fieldValue);
			}
			else {
				return fieldValue;
			}
		}
		//2.Checkbox
		contentData.getCheckboxString = function (record, field) {
			var fieldValue = record[field.dataName];
			if (fieldValue) {
				return "true";
			}
			else {
				return "false";
			}
		}
		//3.Currency
		contentData.getCurrencyString = function (record, field) {
			var fieldValue = record[field.dataName];
			if (!fieldValue) {
				return "";
			}
			else if (field.meta.currency != null && field.meta.currency !== {} && field.meta.currency.symbol) {
				if (field.meta.currency.symbolPlacement === 1) {
					return field.meta.currency.symbol + " " + fieldValue;
				}
				else {
					return fieldValue + " " + field.meta.currency.symbol;
				}
			}
			else {
				return fieldValue;
			}
		}
		//4.Date
		contentData.getDateString = function (record, field) {
			var fieldValue = record[field.dataName];
			return moment(fieldValue).format("DD MMMM YYYY");
		}
		//5.Datetime
		contentData.getDateTimeString = function (record, field) {
			var fieldValue = record[field.dataName];
			return moment(fieldValue).format("DD MMMM YYYY HH:mm");
		}
		//6.Email
		contentData.getEmailString = function (record, field) {
			var fieldValue = record[field.dataName];
			if (fieldValue) {
				//There is a problem in Angular when having in href -> the href is not rendered
				//return "<a href='mailto:" + fieldValue + "' data-rel='external'>" + fieldValue + "</a>";
				return fieldValue;
			}
			else {
				return "";
			}
		}
		//7.File
		contentData.getFileString = function (record, field) {
			var fieldValue = record[field.dataName];
			if (fieldValue) {
				return "<a href='" + fieldValue + "' taget='_blank' class='link-icon'>view file</a>";
			}
			else {
				return "";
			}
		}
		//8.Html
		contentData.getHtmlString = function (record, field) {
			var fieldValue = record[field.dataName];
			if (fieldValue) {
				return fieldValue;
			}
			else {
				return "";
			}
		}
		//9.Image
		contentData.getImageString = function (record, field) {
			var fieldValue = record[field.dataName];
			if (fieldValue) {
				return "<img src='" + fieldValue + "' class='table-image'/>";
			}
			else {
				return "";
			}
		}
		//11.Multiselect
		contentData.getMultiselectString = function (record, field) {
			var fieldValueArray = record[field.dataName];
			var generatedStringArray = [];
			if (fieldValueArray.length === 0) {
				return "";
			}
			else {
				for (var i = 0; i < fieldValueArray.length; i++) {
					var selected = $filter('filter')(field.meta.options, { key: fieldValueArray[i] });
					generatedStringArray.push((fieldValueArray[i] && selected.length) ? selected[0].value : 'empty');
				}
				return generatedStringArray.join(', ');

			}

		}
		//14.Percent
		contentData.getPercentString = function (record, field) {
			var fieldValue = record[field.dataName];
			if (!fieldValue) {
				return "";
			}
			else {
				return fieldValue * 100 + "%";
			}
		}
		//15.Phone
		contentData.getPhoneString = function (record, field) {
			var fieldValue = record[field.dataName];
			if (!fieldValue) {
				return "";
			}
			else {
				return phoneUtils.formatInternational(fieldValue);
			}
		}
		//17.Dropdown
		contentData.getDropdownString = function (record, field) {
			var fieldValue = record[field.dataName];
			if (!fieldValue) {
				return "";
			}
			else {
				var selected = $filter('filter')(field.meta.options, { key: fieldValue });
				return (fieldValue && selected.length) ? selected[0].value : 'empty';
			}

		}
		//18.Url
		contentData.getUrlString = function (record, field) {
			var fieldValue = record[field.dataName];
			if (fieldValue) {
				return "<a href='" + fieldValue + "' target='_blank'>" + fieldValue + "</a>";
			}
			else {
				return "";
			}
		}
		//#endregion

		//#region << Modals >> ////////////////////////////////////

		contentData.createRecordModal = function () {
			var modalInstance = $modal.open({
				animation: false,
				templateUrl: "createRecordModal.html",
				controller: "createRecordModalController",
				controllerAs: "popupData",
				size: "lg",
				resolve: {
					parentData: function () { return contentData; }
				}
			});
		}

		//#endregion

		$log.debug('webvellaAreas>entities> END controller.exec');
	}


	//// Modal Controllers
	createRecordModalController.$inject = ["$scope","$stateParams", "parentData", "$modalInstance", "$log", "webvellaAdminService", "webvellaAreasService", "webvellaRootService", "ngToast", "$timeout", "$state"];

	/* @ngInject */
	function createRecordModalController($scope, $stateParams, parentData, $modalInstance, $log, webvellaAdminService, webvellaAreasService, webvellaRootService, ngToast, $timeout, $state) {
		$log.debug("webvellaAdmin>entities>deleteFieldModal> START controller.exec");
		/* jshint validthis:true */
		var popupData = this;

		//#region << Set enironment >> /////////////////////
		popupData.parentData = angular.copy(parentData);
		popupData.createViewRegion = null;
		for (var i = 0; i < popupData.parentData.entity.recordViews.length; i++) {
			if (popupData.parentData.entity.recordViews[i].type === "quickcreate" && popupData.parentData.entity.recordViews[i].default) {
				for (var j = 0; j < popupData.parentData.entity.recordViews[i].regions.length; j++) {
					if (popupData.parentData.entity.recordViews[i].regions[j].name === "content") {
						popupData.createViewRegion = popupData.parentData.entity.recordViews[i].regions[j];
					}
				}


			}
		}
		//Initialize entityRecordData
		popupData.entityData = {};
		for (var l = 0; l < popupData.parentData.entity.fields.length; l++) {
			popupData.entityData[popupData.parentData.entity.fields[l].name] = null;
		}

		popupData.files = {}; // this is the data wrapper for the temporary upload objects that will be used in the html and for which we will generate watches below
		popupData.progress = {}; //Needed for file and image uploads
		var availableViewFields = [];
		//Init default values of fields
		if (popupData.createViewRegion != null) {
			availableViewFields = webvellaAdminService.getItemsFromRegion(popupData.createViewRegion);
			for (var j = 0; j < availableViewFields.length; j++) {
				if (availableViewFields[j].type === "field") {
					switch (availableViewFields[j].meta.fieldType) {

						case 2: //Checkbox
							popupData.entityData[availableViewFields[j].meta.name] = angular.copy(availableViewFields[j].meta.defaultValue);
							break;

						case 3: //Currency
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								popupData.entityData[availableViewFields[j].meta.name] = angular.copy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 4: //Date
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								if (availableViewFields[j].meta.useCurrentTimeAsDefaultValue) {
									popupData.entityData[availableViewFields[j].meta.name] = moment().toISOString();
								}
								else if (availableViewFields[j].meta.defaultValue) {
									popupData.entityData[availableViewFields[j].meta.name] = moment(availableViewFields[j].meta.defaultValue).toISOString();
								}
							}
							break;
						case 5: //Date
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								if (availableViewFields[j].meta.useCurrentTimeAsDefaultValue) {
									popupData.entityData[availableViewFields[j].meta.name] = moment().toISOString();
								}
								else if (availableViewFields[j].meta.defaultValue) {
									popupData.entityData[availableViewFields[j].meta.name] = moment(availableViewFields[j].meta.defaultValue).toISOString();
								}
							}
							break;
						case 6: //Email
							break;
						case 7: //File
							popupData.progress[availableViewFields[j].meta.name] = 0;
							if (availableViewFields[j].meta.required) {
								popupData.entityData[availableViewFields[j].meta.name] = angular.copy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 8: //HTML
							if (availableViewFields[j].meta.required) {
								popupData.entityData[availableViewFields[j].meta.name] = angular.copy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 9: //Image
							popupData.progress[availableViewFields[j].meta.name] = 0;
							if (availableViewFields[j].meta.required) {
								popupData.entityData[availableViewFields[j].meta.name] = angular.copy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 10: //TextArea
							if (availableViewFields[j].meta.required) {
								popupData.entityData[availableViewFields[j].meta.name] = angular.copy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 11: //Multiselect
							if (availableViewFields[j].meta.required) {
								popupData.entityData[availableViewFields[j].meta.name] = angular.copy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 12: //Number
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								popupData.entityData[availableViewFields[j].meta.name] = angular.copy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 13: //Password
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								//Does not have default value
								//popupData.entityData[availableViewFields[j].meta.name] = angular.copy(availableViewFields[j].meta.defaultValue);
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
								//popupData.entityData[availableViewFields[j].meta.name] = resultPercentage;
							}
							break;
						case 15: //Phone
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								popupData.entityData[availableViewFields[j].meta.name] = angular.copy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 16: //Guid
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								popupData.entityData[availableViewFields[j].meta.name] = angular.copy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 17: //Dropdown
							if (availableViewFields[j].meta.required && availableViewFields[j].meta.defaultValue) {
								popupData.entityData[availableViewFields[j].meta.name] = angular.copy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 18: //Text
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								popupData.entityData[availableViewFields[j].meta.name] = angular.copy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 19: //URL
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								popupData.entityData[availableViewFields[j].meta.name] = angular.copy(availableViewFields[j].meta.defaultValue);
							}
							break;
					}


				}
			}
		}

		//Html
		//Should use scope as it is not working with popupData
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

		//#endregion

		//#region << Logic >>
		popupData.calendars = {};
		popupData.openCalendar = function(event, name) {
			popupData.calendars[name] = true;
		}

		popupData.getWidth = function(dataName) {
			console.log(popupData.progress[dataName] + "%;");
			return popupData.progress[dataName] + "%;";

		}

		popupData.validate = function(item) {
			//validate required
			if (item.meta.required && !popupData.entityData[item.dataName]) {
				popupData[item.meta.name + 'Error'] = true;
				popupData[item.meta.name + 'ErrorMessage'] = "Required.";
			}
		}

		popupData.uploadedFieldName = "";
		popupData.upload = function (file, item) {
			if (file != null) {
				popupData.uploadedFieldName = item.dataName;
				popupData.moveSuccessCallback = function(response) {
					$timeout(function () {
						popupData.entityData[popupData.uploadedFieldName] = response.object.url;
					}, 1);
				}

				popupData.uploadSuccessCallback = function(response) {
					var tempPath = response.object.url;
					var fileName = response.object.filename;
					var targetPath = "/fs/" + item.fieldId + "/" + fileName;
					var overwrite = true;
					webvellaAdminService.moveFileFromTempToFS(tempPath, targetPath, overwrite, popupData.moveSuccessCallback, popupData.uploadErrorCallback);
				}
				popupData.uploadErrorCallback = function(response) {
					alert(response.message);
				}
				popupData.uploadProgressCallback = function(response) {
					$timeout(function () {
						popupData.progress[popupData.uploadedFieldName] = parseInt(100.0 * response.loaded / response.total);
					}, 1);
				}
				webvellaAdminService.uploadFileToTemp(file, item.meta.name, popupData.uploadProgressCallback, popupData.uploadSuccessCallback, popupData.uploadErrorCallback);
			}
		};

		popupData.deleteFileUpload = function (item) {
			var fieldName = item.dataName;
			var filePath = popupData.entityData[fieldName];

			function deleteSuccessCallback(response) {
				$timeout(function () {
					popupData.entityData[fieldName] = null;
					popupData.progress[fieldName] = 0;
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


		//#endregion


		popupData.ok = function () {
			//Alter some data before save
			for (var k = 0; k < availableViewFields.length; k++) {
				if (availableViewFields[k].type === "field") {
					switch (availableViewFields[k].meta.fieldType) {
						case 4: //Date
							popupData.entityData[availableViewFields[k].meta.name] = moment(popupData.entityData[availableViewFields[k].meta.name]).startOf('day').utc().toISOString();
							break;
						case 5: //Date & Time
							popupData.entityData[availableViewFields[k].meta.name] = moment(popupData.entityData[availableViewFields[k].meta.name]).startOf('minute').utc().toISOString();
							break;
						case 14: //Persent
							//need to convert to decimal 0 <= val <= 100 Divide by 100
							//Hack for proper javascript devision
							$scope.Math = window.Math;
							var helpNumber = 10000000;
							var multipliedValue = $scope.Math.round(popupData.entityData[availableViewFields[k].meta.name] * helpNumber);
							popupData.entityData[availableViewFields[k].meta.name] = multipliedValue / (100 * helpNumber);
							break;
					}
				}
			}
			popupData.entityData.createdOn = moment().utc().toISOString();
			webvellaAdminService.createRecord(popupData.parentData.entity.name, popupData.entityData, successCallback, errorCallback);

		};

		popupData.cancel = function () {
			$modalInstance.dismiss("cancel");
		};

		/// Aux
		function successCallback(response) {
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success:</span> ' + response.message
			});
			$modalInstance.close('success');
			$timeout(function () {
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
		$log.debug('webvellaAdmin>entities>createEntityModal> END controller.exec');
	};


})();
