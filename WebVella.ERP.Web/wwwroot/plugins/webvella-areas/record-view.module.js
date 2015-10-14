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
			url: '/:recordId/:viewName/:auxPageName/:filter/:page',
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
				resolvedCurrentView: resolveCurrentView,
				pluginAuxPageName: function () {
					//The pluginAuxPageName is used from plugins in order to properly set the active navigation menu item in the sidebar
					return "";
				}
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

	//#endregion


	// Controller ///////////////////////////////

	function multiplyDecimals(val1, val2, decimalPlaces, $scope) {
		var helpNumber = 100;
		for (var i = 0; i < decimalPlaces; i++) {
			helpNumber = helpNumber * 10;
		}
		var temp1 = $scope.Math.round(val1 * helpNumber);
		var temp2 = $scope.Math.round(val2 * helpNumber);
		return (temp1 * temp2) / (helpNumber * helpNumber);
	}


	controller.$inject = ['$filter', '$uibModal', '$log', '$q', '$rootScope', '$state', '$stateParams', '$scope', '$window', 'pageTitle', 'webvellaRootService', 'webvellaAdminService', 'webvellaAreasService',
        'resolvedSitemap', '$timeout', 'resolvedCurrentView', 'ngToast', 'wvAppConstants', 'resolvedCurrentEntityMeta', 'resolvedEntityRelationsList', 'resolvedCurrentUser'];

	/* @ngInject */
	function controller($filter, $uibModal, $log, $q, $rootScope, $state, $stateParams, $scope, $window, pageTitle, webvellaRootService, webvellaAdminService, webvellaAreasService,
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
		contentData.siteMap = fastCopy(resolvedSitemap);
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
		contentData.defaultRecordView = fastCopy(resolvedCurrentView.meta);

		//2. Load the sidebar
		contentData.sidebarRegion = contentData.defaultRecordView.sidebar;

		//3. Find and load the selected page meta and data
		function getViewOrListMetaAndData(name) {
			var returnObject = {
				data: null,
				meta: null,
				isView: true,
				isEdit: true
			};

			if (name === "") {
				for (var i = 0; i < contentData.defaultRecordView.regions.length; i++) {
					if (contentData.defaultRecordView.regions[i].name === "content") {
						returnObject.meta = fastCopy(contentData.defaultRecordView.regions[i]);
						returnObject.meta.label = "General";
					}
				}
				returnObject.isView = true;
				returnObject.isEdit = true;
				returnObject.data = fastCopy(resolvedCurrentView.data[0]);
			} else {
				var selectedDataName = "";
				returnObject.isEdit = false;
				for (var i = 0; i < contentData.defaultRecordView.sidebar.items.length; i++) {
					if (contentData.defaultRecordView.sidebar.items[i].dataName === name) {
						//Set meta
						// If in edit mode (view from the current entity) the data should be different -> we need the content region meta, not the view meta as in recursive-view directive
						if (contentData.defaultRecordView.sidebar.items[i].type === "view") {
							for (var j = 0; j < contentData.defaultRecordView.sidebar.items[i].meta.regions.length; j++) {
								if (contentData.defaultRecordView.sidebar.items[i].meta.regions[j].name === "content") {
									returnObject.isEdit = true;
									returnObject.meta = fastCopy(contentData.defaultRecordView.sidebar.items[i].meta.regions[j]);
									returnObject.meta.label = fastCopy(contentData.defaultRecordView.sidebar.items[i].meta.label);
									break;
								}
							}
						}
						else {
							returnObject = contentData.defaultRecordView.sidebar.items[i];
						}

						//Set data
						selectedDataName = contentData.defaultRecordView.sidebar.items[i].dataName;
						if (contentData.defaultRecordView.sidebar.items[i].type === "view") {
							returnObject.isView = true;
							returnObject.data = fastCopy(resolvedCurrentView.data[0][selectedDataName][0]);
						}
						else if (contentData.defaultRecordView.sidebar.items[i].type === "viewFromRelation") {
							returnObject.isView = true;
							returnObject.data = fastCopy(resolvedCurrentView.data[0][selectedDataName]);
						} else if (contentData.defaultRecordView.sidebar.items[i].type === "list") {
							returnObject.isView = false;
							returnObject.data = fastCopy(resolvedCurrentView.data[0][selectedDataName]);
						} else if (contentData.defaultRecordView.sidebar.items[i].type === "listFromRelation") {
							returnObject.isView = false;
							returnObject.data = fastCopy(resolvedCurrentView.data[0][selectedDataName]);
						}
					}
				}

			}

			return returnObject;
		};

		var returnedObject = {};
		contentData.selectedSidebarPage = {};
		if ($stateParams.auxPageName === "*") {
			//The default view meta is active
			returnedObject = getViewOrListMetaAndData("");
			contentData.selectedSidebarPage = returnedObject;
		}
		else {
			//One of the sidebar view or lists is active
			//Load the data
			returnedObject = getViewOrListMetaAndData($stateParams.auxPageName);
			contentData.selectedSidebarPage = returnedObject;
			contentData.selectedSidebarPage.data = returnedObject.data;
		}

		//#endregion

		//#region << Initialize current entity >>
		contentData.currentEntity = fastCopy(resolvedCurrentEntityMeta);
		contentData.viewSection = {};
		contentData.viewSection.label = contentData.selectedSidebarPage.meta.label;
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
		//#endregion

		if (contentData.selectedSidebarPage.isEdit) {

			//#region << Edit View Rendering Logic fields>>

			contentData.toggleSectionCollapse = function (section) {
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
							contentData.selectedSidebarPage.data[item.dataName] = fastCopy(contentData.lastEnabledHtmlFieldData);
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
				contentData.lastEnabledHtmlFieldData = fastCopy(contentData.selectedSidebarPage.data[item.dataName]);
			}



			contentData.fieldUpdate = function (item, data) {
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
					contentData.selectedSidebarPage.data = fastCopy(response.object.data[0]);
					defer.resolve();
				}

				function patchFailedCallback(response) {
					ngToast.create({
						className: 'error',
						content: '<span class="go-red">Error:</span> ' + response.message,
						timeout: 7000
					});
					defer.resolve("validation error");
				}

				webvellaAdminService.patchRecord($stateParams.recordId, contentData.currentEntity.name, contentData.patchObject, patchSuccessCallback, patchFailedCallback);

				return defer.promise;
			}

			//Auto increment
			contentData.getAutoIncrementString = function (item) {
				return webvellaAreasService.getAutoIncrementString(contentData.selectedSidebarPage.data[item.dataName],item.meta);
			};
			//Checkbox
			contentData.getCheckboxString = function (item) {
				return webvellaAreasService.getCheckboxString(contentData.selectedSidebarPage.data[item.dataName], item.meta);
			};
			//Currency
			contentData.getCurrencyString = function (item) {
				return webvellaAreasService.getCurrencyString(contentData.selectedSidebarPage.data[item.dataName], item.meta);
			};
			//Date & DateTime 
			contentData.getDateString = function (item) {
				return webvellaAreasService.getDateString(contentData.selectedSidebarPage.data[item.dataName], item.meta);
			};
			contentData.getTimeString = function (item) {
				var fieldValue = contentData.selectedSidebarPage.data[item.dataName];
				if (!fieldValue) {
					return "";
				} else {
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

			contentData.getProgressStyle = function (name) {
				return "width: " + contentData.progress[name] + "%;";
			}

			contentData.uploadedFileName = "";
			contentData.upload = function (file, item) {
				if (file != null) {
					contentData.uploadedFileName = item.dataName;
					contentData.moveSuccessCallback = function (response) {
						$timeout(function () {
							contentData.selectedSidebarPage.data[contentData.uploadedFileName] = response.object.url;
							contentData.fieldUpdate(item, response.object.url);
						}, 1);
					}

					contentData.uploadSuccessCallback = function (response) {
						var tempPath = response.object.url;
						var fileName = response.object.filename;
						var targetPath = "/fs/" + item.fieldId + "/" + fileName;
						var overwrite = true;
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
				var filePath = contentData.selectedSidebarPage.data[fieldName];

				function deleteSuccessCallback(response) {
					$timeout(function () {
						contentData.selectedSidebarPage.data[fieldName] = null;
						contentData.progress[fieldName] = 0;
						contentData.fieldUpdate(item, null);
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
					{ name: 'basicstyles', items: ['Save', 'Bold', 'Italic', 'Strike', 'Underline'] },
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
			contentData.getMultiselectString = function (item) {
				return webvellaAreasService.getMultiselectString(contentData.selectedSidebarPage.data[item.dataName], item.meta);
			};

			//Password
			contentData.dummyPasswordModels = {}; //as the password value is of know use being encrypted, we will assign dummy models
			//Dropdown
			contentData.getDropdownString = function (item) {
				return webvellaAreasService.getDropdownString(contentData.selectedSidebarPage.data[item.dataName], item.meta);
			};
			//Percent
			$scope.Math = window.Math;

			contentData.getPercentString = function (item) {
				return webvellaAreasService.getPercentString(contentData.selectedSidebarPage.data[item.dataName], item.meta);
			};

			contentData.currentUserRoles = fastCopy(resolvedCurrentUser.roles);
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
				var modalInstance = $uibModal.open({
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
							content: '<span class="go-red">Error:</span> ' + messageHtml,
							timeout: 7000
						});
					}

					// ** Post relation change between the two records
					var recordsToBeAttached = [];
					var recordsToBeDettached = [];
					if (returnObject.dataKind == "origin") {
						recordsToBeAttached.push(returnObject.selectedRecordId);
						recordsToBeDettached.push(oldRelationRecordId);
						webvellaAdminService.manageRecordsRelation(returnObject.relationName, displayedRecordId, recordsToBeAttached, recordsToBeDettached, successCallback, errorCallback);
					}
					else if (returnObject.dataKind == "target") {
						recordsToBeAttached.push(displayedRecordId);
						webvellaAdminService.manageRecordsRelation(returnObject.relationName, returnObject.selectedRecordId, recordsToBeAttached, recordsToBeDettached, successCallback, errorCallback);
					}
					else {
						alert("the <<origin-target>> dataKind is still not implemented. Contact the system administrator");
					}
				});
			}
				//Select MULTIPLE item modal
			else if ((relationType == 2 && dataKind == "origin") || (relationType == 3 && dataKind == "origin")) {
				var modalInstance = $uibModal.open({
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
							content: '<span class="go-red">Error:</span> ' + messageHtml,
							timeout: 7000
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
				var modalInstance = $uibModal.open({
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
								object: null
							}
							getListRecordsSuccessCallback(lockedChangeResponse);
						}
						else {
							webvellaAreasService.getListRecords(defaultLookupList.name, entityMeta.name, "all", 1,null, getListRecordsSuccessCallback, errorCallback);
						}
					}
					else if (contentData.modalDataKind == "target") {
						//Current records is Target
						webvellaAreasService.getListRecords(defaultLookupList.name, entityMeta.name, "all", 1,null, getListRecordsSuccessCallback, errorCallback);
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
	ManageRelationFieldModalController.$inject = ['contentData', '$modalInstance', '$log', '$q', '$stateParams', 'modalMode', 'resolvedLookupRecords',
        'selectedDataKind', 'selectedItem', 'selectedRelationType', 'webvellaAdminService', 'webvellaAreasService', 'webvellaRootService', 'ngToast', '$timeout', '$state'];
	/* @ngInject */
	function ManageRelationFieldModalController(contentData, $modalInstance, $log, $q, $stateParams, modalMode, resolvedLookupRecords,
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
		popupData.currentlyAttachedIds = fastCopy(popupData.parentData.selectedSidebarPage.data["$field$" + popupData.selectedItem.relationName + "$id"]); // temporary object in order to highlight
		popupData.dbAttachedIds = fastCopy(popupData.currentlyAttachedIds);
		popupData.attachedRecordIdsDelta = [];
		popupData.detachedRecordIdsDelta = [];


		//Get the default lookup list for the entity
		if (resolvedLookupRecords.success) {
			popupData.relationLookupList = fastCopy(resolvedLookupRecords.object);
		}
		else {
			popupData.hasWarning = true;
			popupData.warningMessage = resolvedLookupRecords.message;
		}

		//#region << Paging >>
		popupData.selectPage = function (page) {
			// Process
			function successCallback(response) {
				popupData.relationLookupList = fastCopy(response.object);
				popupData.currentPage = page;
			}

			function errorCallback(response) {

			}

			webvellaAreasService.getListRecords(popupData.relationLookupList.meta.name, popupData.selectedItem.entityName, "all", page,null, successCallback, errorCallback);
		}

		//#endregion

		//#region << Render Logic >>

		//1.Auto increment
		popupData.getAutoIncrementString = webvellaAreasService.getAutoIncrementString;
		//2.Checkbox
		popupData.getCheckboxString = webvellaAreasService.getCheckboxString;
		//3.Currency
		popupData.getCurrencyString = webvellaAreasService.getCurrencyString;
		//4.Date
		popupData.getDateString = webvellaAreasService.getDateString;
		//5.Datetime
		popupData.getDateTimeString = webvellaAreasService.getDateTimeString;
		//6.Email
		popupData.getEmailString = webvellaAreasService.getEmailString;
		//7.File
		popupData.getFileString = webvellaAreasService.getFileString;
		//8.Html
		popupData.getHtmlString = webvellaAreasService.getHtmlString;
		//9.Image
		popupData.getImageString = webvellaAreasService.getImageString;
		//10.Textarea
		popupData.getTextareaString = webvellaAreasService.getTextareaString;
		//11.Multiselect
		popupData.getMultiselectString = webvellaAreasService.getMultiselectString;
		//12.Number
		popupData.getNumberString = webvellaAreasService.getNumberString;
		//13.Password
		popupData.getPasswordString = webvellaAreasService.getPasswordString;
		//14.Percent
		popupData.getPercentString = webvellaAreasService.getPercentString;
		//15.Phone
		popupData.getPhoneString = webvellaAreasService.getPhoneString;
		//15.Guid
		popupData.getGuidString = webvellaAreasService.getGuidString;
		//17.Dropdown
		popupData.getDropdownString = webvellaAreasService.getDropdownString;
		//18. Text
		popupData.getTextString = webvellaAreasService.getTextString;
		//18.Url
		popupData.getUrlString = webvellaAreasService.getUrlString;
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
				var currentRecordId = fastCopy(popupData.processingRecordId);
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
					content: '<span class="go-red">Error:</span> ' + messageHtml,
					timeout: 7000
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



