/* entities.module.js */

/**
* @desc this module manages the application home desktop screen
*/

(function () {
	'use strict';

	angular
        .module('webvellaAreas') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAreasRecordViewController', controller);

	// Configuration ///////////////////////////////////
	config.$inject = ['$stateProvider'];

	/* @ngInject */
	function config($stateProvider) {
		$stateProvider.state('webvella-areas-record-view', {
			parent: 'webvella-areas-base',
			url: '/:areaName/:entityName/:recordId/:viewName', // /areas/areaName/sectionName/entityName after the parent state is prepended
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
				resolvedExtendedViewData: resolveExtendedViewData
			},
			data: {

			}
		});
	};


	// Run //////////////////////////////////////
	run.$inject = ['$log'];

	/* @ngInject */
	function run($log) {
		$log.debug('webvellaAreas>entities> BEGIN module.run');

		$log.debug('webvellaAreas>entities> END module.run');
	};


	//#region << Resolve Function >>
	resolveExtendedViewData.$inject = ['$q', '$log', 'webvellaAreasService', '$stateParams'];
	/* @ngInject */
	function resolveExtendedViewData($q, $log, webvellaAreasService, $stateParams) {
		$log.debug('webvellaAreas>entities> BEGIN state.resolved');
		// Initialize
		var defer = $q.defer();
		var record = {};
		var extendedView = {};
		//// Process
		function getRecordSuccessCallback(response) {
			record = response.object;
			//Cycle through the view, find all fields and attach their data and meta info
			for (var regionIndex = 0; regionIndex < extendedView.regions.length; regionIndex++) {
				if (extendedView.regions[regionIndex].name == "content") {
					for (var sectionIndex = 0; sectionIndex < extendedView.regions[regionIndex].sections.length; sectionIndex++) {
						for (var rowIndex = 0; rowIndex < extendedView.regions[regionIndex].sections[sectionIndex].rows.length; rowIndex++) {
							for (var columnIndex = 0; columnIndex < extendedView.regions[regionIndex].sections[sectionIndex].rows[rowIndex].columns.length; columnIndex++) {
								for (var itemIndex = 0; itemIndex < extendedView.regions[regionIndex].sections[sectionIndex].rows[rowIndex].columns[columnIndex].items.length; itemIndex++) {
									for (var metaIndex = 0; metaIndex < record.fieldsMeta.length; metaIndex++) {
										if (record.fieldsMeta[metaIndex].id === extendedView.regions[regionIndex].sections[sectionIndex].rows[rowIndex].columns[columnIndex].items[itemIndex].fieldId) {
											extendedView.regions[regionIndex].sections[sectionIndex].rows[rowIndex].columns[columnIndex].items[itemIndex].meta = record.fieldsMeta[metaIndex];
										}
									}
								}
							}
						}
					}
				}
			}
			extendedView.data = record.data;
			defer.resolve(extendedView);
		}

		//// Process
		function getViewSuccessCallback(response) {
			extendedView = response.object;
			webvellaAreasService.getEntityRecord($stateParams.recordId, $stateParams.entityName, getRecordSuccessCallback, errorCallback);
		}

		function errorCallback(response) {
			alert("Error getting the view");
		}

		webvellaAreasService.getViewByName($stateParams.viewName, $stateParams.entityName, getViewSuccessCallback, errorCallback);

		// Return
		$log.debug('webvellaAreas>entities> END state.resolved');
		return defer.promise;
	}

	//#endregion


	// Controller ///////////////////////////////
	controller.$inject = ['$filter', '$log', '$rootScope', '$state', '$scope', 'pageTitle', 'webvellaRootService', 'webvellaAdminService',
        'resolvedSitemap', '$timeout', 'resolvedExtendedViewData', 'ngToast', 'wvAppConstants', 'Upload'];

	/* @ngInject */
	function controller($filter, $log, $rootScope, $state, $scope, pageTitle, webvellaRootService, webvellaAdminService,
        resolvedSitemap, $timeout, resolvedExtendedViewData, ngToast, wvAppConstants, Upload) {
		$log.debug('webvellaAreas>entities> BEGIN controller.exec');
		/* jshint validthis:true */
		var contentData = this;

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
		contentData.recordView = angular.copy(resolvedExtendedViewData);
		contentData.contentRegion = null;
		contentData.sidebarRegion = null;
		for (var i = 0; i < contentData.recordView.regions.length; i++) {
			if (contentData.recordView.regions[i].name === "content") {
				contentData.contentRegion = contentData.recordView.regions[i];
			}
			else if (contentData.recordView.regions[i].name === "sidebar") {
				contentData.sidebarRegion = contentData.recordView.regions[i];
			}
		}
		contentData.viewData = contentData.recordView.data[0];
		//#endregion

		//#region << Intialize current entity >>
		contentData.currentEntity = null;
		for (var i = 0; i < contentData.currentArea.entities.length; i++) {
			if (contentData.currentArea.entities[i].name === $state.params.entityName) {
				contentData.currentEntity = contentData.currentArea.entities[i];
			}
		}
		//#endregion

		//#region << Logic >>

		contentData.toggleSectionCollapse = function (section) {
			section.collapsed = !section.collapsed;
		}

		contentData.fieldUpdate = function (item, data) {
			data = data.toString().trim();
			contentData.patchObject = {};
			var validation = {
				success: true,
				message: "successful validation"
			};
			switch (item.fieldTypeId) {

				//Auto increment number
				case 1:
					//Readonly
					break;
					//Checkbox
				case 2:
					data = (data === "true"); // convert string to boolean
					break;
					//Auto increment number
				case 3:
					validation = checkDecimal(data);
					if (!validation.success) {
						return validation.message;
					}
					if (decimalPlaces(data) > item.meta.currency.decimalDigits) {
						return "Decimal places should be " + item.meta.currency.decimalDigits + " or less";
					}
					break;
				case 4:
					data = moment(data).toISOString();
					break;
				case 5:
					data = moment(data).toISOString();
					break;
				case 6:
					validation = checkEmail(data);
					if (!validation.success) {
						return validation.message;
					}

					break;
			}
			contentData.patchObject[item.fieldName] = data;
			//patchRecord(recordId, entityName, patchObject, successCallback, errorCallback)
			webvellaAdminService.patchRecord(contentData.viewData.id, contentData.currentEntity.name, contentData.patchObject, patchSuccessCallback, patchFailedCallback);
		}

		function patchSuccessCallback(response) {
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success:</span> ' + response.message
			});
			return true;
		}
		function patchFailedCallback(response) {
			ngToast.create({
				className: 'error',
				content: '<span class="go-red">Error:</span> ' + response.message
			});
			return false;
		}

		//Auto increment
		contentData.getAutoIncrementString = function (item) {
			var fieldValue = contentData.viewData[item.fieldName];
			if (!fieldValue) {
				return "empty";
			}
			else if (item.meta.displayFormat) {
				return item.meta.displayFormat.replace("{0}", fieldValue);
			}
			else {
				return fieldValue;
			}
		}
		//Checkbox
		contentData.getCheckboxString = function (item) {
			var fieldValue = contentData.viewData[item.fieldName];
			if (fieldValue) {
				return "true";
			}
			else {
				return "false";
			}
		}
		//Currency
		contentData.getCurrencyString = function (item) {
			var fieldValue = contentData.viewData[item.fieldName];
			if (!fieldValue) {
				return "empty";
			}
			else if (item.meta.currency != null && item.meta.currency != {} && item.meta.currency.symbol) {
				if (item.meta.currency.symbolPlacement == 1) {
					return item.meta.currency.symbol + " " + fieldValue
				}
				else {
					return fieldValue + " " + item.meta.currency.symbol
				}
			}
			else {
				return fieldValue;
			}
		}
		//Date & DateTime 
		contentData.getDateString = function (item) {
			var fieldValue = contentData.viewData[item.fieldName];
			if (!fieldValue) {
				return "";
			}
			else {
				return $filter('date')(fieldValue, "dd MMM yyyy");
			}
		}
		contentData.getTimeString = function (item) {
			var fieldValue = contentData.viewData[item.fieldName];
			if (!fieldValue) {
				return "";
			}
			else {
				return $filter('date')(fieldValue, "HH:mm");
			}
		}
		$scope.picker = { opened: false };
		$scope.openPicker = function () {
			$timeout(function () {
				$scope.picker.opened = true;
			});
		};
		$scope.closePicker = function () {
			$scope.picker.opened = false;
		};

		//File upload
		contentData.files = {}; // this is the data wrapper for the temporary upload objects that will be used in the html and for which we will generate watches below
		contentData.progress = {}; //data wrapper for the progress percentage for each upload

		/////////Register watches
		for (var sectionIndex = 0; sectionIndex < contentData.contentRegion.sections.length; sectionIndex++) {
			for (var rowIndex = 0; rowIndex < contentData.contentRegion.sections[sectionIndex].rows.length; rowIndex++) {
				for (var columnIndex = 0; columnIndex < contentData.contentRegion.sections[sectionIndex].rows[rowIndex].columns.length; columnIndex++) {
					for (var itemIndex = 0; itemIndex < contentData.contentRegion.sections[sectionIndex].rows[rowIndex].columns[columnIndex].items.length; itemIndex++) {
						if (contentData.contentRegion.sections[sectionIndex].rows[rowIndex].columns[columnIndex].items[itemIndex].fieldTypeId === 7) {
							var FieldName = contentData.contentRegion.sections[sectionIndex].rows[rowIndex].columns[columnIndex].items[itemIndex].fieldName;
							var DataName = "contentData.files." + FieldName;
							contentData.progress[FieldName] = 0;
							$scope.$watch(DataName, function () {
								contentData.upload(contentData.files[FieldName], FieldName);
							});
						}
					}
				}
			}
		}
		contentData.upload = function (files,fieldName) {
			if (files && files.length) {
				for (var i = 0; i < files.length; i++) {
					var file = files[i];
					$log.info(file);
					Upload.upload({
						url: 'http://localhost:2202/fs/upload/',
						fields: {},
						file: file
					}).progress(function (evt) {
						contentData.progress[fieldName] = parseInt(100.0 * evt.loaded / evt.total);
					}).success(function (data, status, headers, config) {
						$timeout(function () {
							//$scope.log = 'file: ' + config.file.name + ', Response: ' + JSON.stringify(data) + '\n' + $scope.log;
							//$log.info(data.url);
							//$log.info(status);
							//$log.info(headers);
							//$log.info(config);
							contentData.viewData[fieldName] = data.url;
							//$log.info("result is -> " + contentData.viewData[fieldName]);

						});
					});
				}
			}
		};
		contentData.deleteFileUpload = function (fieldName) {
			$timeout(function () {
				contentData.viewData[fieldName] = null;
				contentData.progress[fieldName] = 0;
			}, 0);
		}

		//Html
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

		$log.debug('webvellaAreas>entities> END controller.exec');
	}

})();
