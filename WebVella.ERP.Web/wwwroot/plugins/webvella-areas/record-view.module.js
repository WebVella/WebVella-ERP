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
        .controller('ManageRelationFieldModalController', ManageRelationFieldModalController)
		.controller('SelectTreeNodesModalController', SelectTreeNodesModalController);

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
					alert("error in response!")
				}, 0);
			}
			else {
				defer.resolve(response.object);
			}
		}

		function errorCallback(response) {
			if (response.object == null) {
				$timeout(function () {
					alert("error in response!")
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
        'resolvedSitemap', '$timeout', 'resolvedCurrentView', 'ngToast', 'wvAppConstants', 'resolvedCurrentEntityMeta', 'resolvedEntityRelationsList', 'resolvedCurrentUser',
		'resolvedCurrentUserEntityPermissions'];

	/* @ngInject */
	function controller($filter, $uibModal, $log, $q, $rootScope, $state, $stateParams, $scope, $window, pageTitle, webvellaRootService, webvellaAdminService, webvellaAreasService,
        resolvedSitemap, $timeout, resolvedCurrentView, ngToast, wvAppConstants, resolvedCurrentEntityMeta, resolvedEntityRelationsList, resolvedCurrentUser,
		resolvedCurrentUserEntityPermissions) {
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
		contentData.currentUserEntityPermissions = fastCopy(resolvedCurrentUserEntityPermissions);

		//#region << Dummy content for test - Remove afterwards >>

		//#region << Tree meta >>
		var treeMeta = {
			dataName: "$tree$item_n_n_category$category_tree",
			entityId: guid(),
			entityLabel: "Category",
			entityLabelPlural: "Categories",
			entityName: "category",
			fieldHelpText: null,
			fieldLabel: "Categories",
			fieldPlaceholder: null,
			fieldRequired: false,
			treeId: guid(),
			treeName: "category_tree",
			relationDirection: "origin-target",
			relationId: guid(),
			relationName: "item_n_n_category",
			type: "treeFromRelation"
		};

		var originalTreeMeta = { "id": "2c93028d-fef5-4763-86e6-df37adc438a8", "name": "test", "label": "tezst", "default": false, "system": false, "cssClass": "", "iconName": "", "relationId": "460e699c-6624-4238-bb17-12243cf5d56b", "depthLimit": 5, "nodeParentIdFieldId": "16672229-1694-468e-a363-c80effffe5d1", "nodeIdFieldId": "5df6bba4-061b-41ce-bf39-8f6b50fd023d", "nodeNameFieldId": "5df6bba4-061b-41ce-bf39-8f6b50fd023d", "nodeLabelFieldId": "5df6bba4-061b-41ce-bf39-8f6b50fd023d", "rootNodes": [], "nodeObjectProperties": ["5df6bba4-061b-41ce-bf39-8f6b50fd023d", "16672229-1694-468e-a363-c80effffe5d1"] };
		treeMeta.meta = originalTreeMeta;
		//#endregion

		//#region << Tree Data >>
		var treeData = [];
		var category1 = {
			"id": guid(),
			"recordId": guid(),
			"name": "clothes",
			"label": "Clothes",
			"parentId": guid()
		};
		var category2 = {
			"id": guid(),
			"recordId": guid(),
			"name": "shoes",
			"label": "Shoes",
			"parentId": guid()
		};

		treeData.push(category1);
		treeData.push(category2);
		//#endregion

		//#region << Insert json in meta view as field >>
		
		
		//for (var i = 0; i < resolvedCurrentView.meta.sidebar.items.length; i++) {
		//	if (resolvedCurrentView.meta.sidebar.items[i].viewName == "meta") {
		//		resolvedCurrentView.meta.sidebar.items[i].meta.regions[0].sections[0].rows[0].columns[0].items.push(treeMeta);
		//		resolvedCurrentView.data[0][resolvedCurrentView.meta.sidebar.items[i].dataName][0]["$tree$item_n_n_category$category_tree"] = treeData;
		//	}
		//}
		//#endregion

		//#region << Insert json in as sidebar item >>
		//Data should be already pushed from the previous seciton
		//resolvedCurrentView.data[0]["$tree$item_n_n_category$category_tree"] = treeData;
		//resolvedCurrentView.meta.sidebar.items.push(treeMeta);
		//#endregion


		//#endregion

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

		//#region << Logic >>

		//Is Edit logic
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

			contentData.emptyField = function (item) {
				var relation = contentData.getRelation(item.relationName);
				var presentedFieldId = item.meta.id;
				var currentEntityId = contentData.currentEntity.id;
				//Currently it is implemented only for 1:N relation type and the current entity should be target and field is required
				if (relation.relationType == 2 && relation.targetEntityId == currentEntityId) {
					var itemObject = {};
					itemObject.meta = null;
					for (var i = 0; i < contentData.currentEntity.fields.length; i++) {
						if (contentData.currentEntity.fields[i].id == relation.targetFieldId) {
							itemObject.meta = contentData.currentEntity.fields[i];
						}
					}
					if (itemObject.meta != null && !itemObject.meta.required) {
						contentData.fieldUpdate(itemObject, null);
					}
				}
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
				if (!item.meta.enableSecurity) {
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

			contentData.currentUserHasUpdatePermissionRelation = function (item) {
				var result = false;
				var relation = contentData.getRelation(item.relationName);
				var currentEntityId = contentData.currentEntity.id;
				var checkedFieldMeta = null;
				//Currently it is implemented only for 1:N & 1:1 relation type as it does not make much sense for N:N
				if (relation.relationType == 1 || relation.relationType == 2) {
					var checkedFieldId = null;
					if (relation.originEntityId != relation.targetEntityId) {
						//if the presented item from the current entity
						if (currentEntityId == item.entityId) {
							//we need to check this item permissions
							checkedFieldMeta = item.meta;
						}
						else {
							//we need to find the corresponding field from the current entity
							if (relation.originFieldId == item.meta.id) {
								//the field from the current entity is than target
								checkedFieldId = relation.targetFieldId;
							}
							else {
								//the field from the current entity is than origin
								checkedFieldId = relation.originFieldId;
							}
						}
					}
					else {
						var checkedFieldId = null;
						if (item.relationDirection == "target-origin") {
							//we need the target field
							checkedFieldId = relation.targetFieldId;
						}
						else {
							//we need the origin field
							checkedFieldId = relation.originFieldId;
						}
					}

					for (var i = 0; i < contentData.currentEntity.fields.length; i++) {
						if (contentData.currentEntity.fields[i].id == checkedFieldId) {
							checkedFieldMeta = contentData.currentEntity.fields[i];
						}
					}

					if (checkedFieldMeta == null) {
						return false;
					}
					else {
						if (!checkedFieldMeta.enableSecurity) {
							return true;
						}
						for (var i = 0; i < contentData.currentUserRoles.length; i++) {
							for (var k = 0; k < checkedFieldMeta.permissions.canUpdate.length; k++) {
								if (checkedFieldMeta.permissions.canUpdate[k] == contentData.currentUserRoles[i]) {
									return true;
								}
							}
						}
					}
				}
				return result;
			}

			//#endregion
		}

		//Render
		contentData.renderFieldValue = webvellaAreasService.renderFieldValue;
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

		//Date & DateTime 
		contentData.getTimeString = function (item) {
			if (item && item.dataName && contentData.selectedSidebarPage.data[item.dataName]) {
				var fieldValue = contentData.selectedSidebarPage.data[item.dataName];
				if (!fieldValue) {
					return "";
				} else {
					return $filter('date')(fieldValue, "HH:mm");
				}
			}
		}

		contentData.recursiveObjectCanDo = function (permissionName, relatedEntityName) {
			var currentEntityPermissions = {};
			var relatedEntityPermissions = {};
			for (var i = 0; i < contentData.currentUserEntityPermissions.length; i++) {
				if (contentData.currentUserEntityPermissions[i].entityName == contentData.currentEntity.name) {
					currentEntityPermissions = contentData.currentUserEntityPermissions[i];
				}
				else if (contentData.currentUserEntityPermissions[i].entityName == relatedEntityName) {
					relatedEntityPermissions = contentData.currentUserEntityPermissions[i];
				}
			}
			switch (permissionName) {
				case "can-add-existing":
					if(currentEntityPermissions.canUpdate){
						return true;
					}
					else {
						return false;
					}
					break;
				case "can-create":
					if (currentEntityPermissions.canUpdate && relatedEntityPermissions.canCreate) {
						return true;
					}
					else {
						return false;
					}
					break;
				case "can-edit":
					if (relatedEntityPermissions.canUpdate) {
						return true;
					}
					else {
						return false;
					}
					break;
				case "can-remove":
					if (currentEntityPermissions.canUpdate) {
						return true;
					}
					else {
						return false;
					}
					break;
			}
		}
		//#endregion

		//#region << Modals >>

		//#region << Relation field >>

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
					var oldRelationRecordId = null;
					if (contentData.selectedSidebarPage.data["$field$" + returnObject.relationName + "$id"]) {
						oldRelationRecordId = contentData.selectedSidebarPage.data["$field$" + returnObject.relationName + "$id"][0];
					}

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
						if (oldRelationRecordId != null) {
							recordsToBeDettached.push(oldRelationRecordId);
						}
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

		//#region << Tree select field >>

		contentData.openSelectTreeNodesModal = function (item) {

			var treeSelectModalInstance = $uibModal.open({
					animation: false,
					templateUrl: 'selectTreeNodesModal.html',
					controller: 'SelectTreeNodesModalController',
					controllerAs: "popupData",
					size: "width-100p",
					resolve: {
						contentData: function () {
							return contentData;
						},
						selectedItem: function () {
							return item;
						},
						selectedItemData: function () {
							return contentData.selectedSidebarPage.data[item.dataName];
						},
						resolvedTree: resolveTree(item),
						resolvedTreeRelation: resolveTreeRelation(item),
						resolvedCurrentUserPermissions: function () {
							return resolvedCurrentUserEntityPermissions;
						}
					}
				});
				//On modal exit
			treeSelectModalInstance.result.then(function (returnObject) {

					// Initialize

					function successCallback(response) {
						ngToast.create({
							className: 'success',
							content: '<span class="go-green">Success:</span> Change applied'
						});
						webvellaRootService.GoToState($state, $state.current.name, contentData.stateParams);
					}

					function errorCallback(response) {
						ngToast.create({
							className: 'error',
							content: '<span class="go-red">Error:</span> ' + response.message,
							timeout: 7000
						});
					}
					webvellaAdminService.manageRecordsRelation(returnObject.relationName, returnObject.selectedRecordId, recordsToBeAttached, recordsToBeDettached, successCallback, errorCallback);
				});
		}

		//Resolve function tree
		var resolveTree = function (item) {
			// Initialize
			var defer = $q.defer();

			// Process
			function errorCallback(response) {
				ngToast.create({
					className: 'error',
					content: '<span class="go-red">Error:</span> ' + response.message,
					timeout: 7000
				});
				defer.reject();
			}
			function successCallback(response) {
				defer.resolve(response.object);
			}

			webvellaAdminService.getRecordsByTreeName(item.treeName, item.entityName, successCallback, errorCallback);

			return defer.promise;
		}

		var resolveTreeRelation = function (item) {
			// Initialize
			var response = null;

			for (var i = 0; i < contentData.relationsList.length; i++) {
				if (contentData.relationsList[i].id == item.relationId) {
					response = contentData.relationsList[i];
					break;
				}
			}

			return response;

		}
		//#endregion

		//#endregion

		$log.debug('webvellaAreas>entities> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	}


	//#region < Modal Controllers >

	//#region << Manage relation Modal >>
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
		popupData.getRelationLabel = contentData.getRelationLabel;
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

			webvellaAreasService.getListRecords(popupData.relationLookupList.meta.name, popupData.selectedItem.entityName, "all", page,null, successCallback, errorCallback);
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

		//#endregion


		popupData.cancel = function () {
			$modalInstance.dismiss('cancel');
		};

		/// Aux
		//function successCallback(response) {
		//	ngToast.create({
		//		className: 'success',
		//		content: '<span class="go-green">Success:</span> ' + response.message
		//	});
		//	$modalInstance.close('success');
		//	popupData.parentData.modalInstance.close('success');
		//	//webvellaRootService.GoToState($state, $state.current.name, {});
		//}

		//function errorCallback(response) {
		//	popupData.hasError = true;
		//	popupData.errorMessage = response.message;


		//}


		//#endregion

		$log.debug('webvellaAdmin>entities>createEntityModal> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	};
	//#endregion 

	//#region << Select Tree >>
	SelectTreeNodesModalController.$inject = ['contentData', '$modalInstance', '$rootScope','$scope', '$log', '$q', '$stateParams', 'resolvedTree',
        'selectedItem', 'resolvedTreeRelation', 'selectedItemData', 'webvellaAdminService', 'ngToast', '$timeout', '$state', '$uibModal',
		'resolvedCurrentUserPermissions'];
	function SelectTreeNodesModalController(contentData, $modalInstance,$rootScope,$scope, $log, $q, $stateParams, resolvedTree,
			selectedItem, resolvedTreeRelation, selectedItemData, webvellaAdminService, ngToast, $timeout, $state, $uibModal, 
			resolvedCurrentUserPermissions) {
		var popupData = this;

		//#region << Init >>
		popupData.relation = fastCopy(resolvedTreeRelation);
		popupData.currentEntity = fastCopy(contentData.currentEntity);
		popupData.currentField = {};
		for (var i = 0; i < popupData.currentEntity.fields.length; i++) {
			if (popupData.currentEntity.fields[i].selectedTreeId == selectedItem.treeId) {
				popupData.currentField = popupData.currentEntity.fields[i];
			}
		}
		popupData.tree = fastCopy(resolvedTree);
		popupData.itemMeta = fastCopy(selectedItem);
		popupData.addButtonLoadingClass = {};
		popupData.attachHoverEffectClass = {};

		popupData.userPermissionsForTreeEntity = {};
		for (var i = 0; i < resolvedCurrentUserPermissions.length; i++) {
			if (resolvedCurrentUserPermissions[i].entityId == selectedItem.entityId) {
				popupData.userPermissionsForTreeEntity = fastCopy(resolvedCurrentUserPermissions[i]);
			}
		}

		//#endregion 

		popupData.cancel = function () {
			$modalInstance.dismiss('cancel');
		};


		//#region << Read only tree >>

		//#region << Node collapse >>
		popupData.collapsedTreeNodes = [];
		popupData.toggleNodeCollapse = function (node) {
			var nodeIndex = popupData.collapsedTreeNodes.indexOf(node.id);
			if (nodeIndex > -1) {
				popupData.collapsedTreeNodes.splice(nodeIndex, 1);
			}
			else {
				popupData.collapsedTreeNodes.push(node.id);
			}
		}

		popupData.nodesToBeCollapsed = [];

		function iterateCollapse(current, depth) {
			var children = current.nodes;
			if (children.length > 0) {
				popupData.collapsedTreeNodes.push(current.id);
			}
			for (var i = 0, len = children.length; i < len; i++) {
				iterateCollapse(children[i], depth + 1);
			}
		}

		popupData.collapseAll = function () {
			popupData.collapsedTreeNodes = [];
			for (var i = 0; i < popupData.tree.data.length; i++) {
				iterateCollapse(popupData.tree.data[i], 0);
			}
		}
		popupData.expandAll = function () {
			popupData.collapsedTreeNodes = [];
		}

		//#endregion

		//#region << Node selection >>

		popupData.selectedTreeRecords = [];

		popupData.selectableNodeIds = [];

		var selectedNodesByBranch = {};

		function iterateCanBeSelected(current, depth, rootNode, isInitial) {
			var children = current.nodes;
			var shouldBeSelectable = true;
			//isInitial is added in order to auto collapse nodes that are more than 3 children
			if (isInitial && children.length > 3) {
				popupData.collapsedTreeNodes.push(current.id);
			}
			//Case: selection type
			switch (popupData.currentField.selectionType) {
				case "single-select":
					if (popupData.selectedTreeRecords && popupData.selectedTreeRecords.length > 0 && popupData.selectedTreeRecords[0] != current.recordId) {
						shouldBeSelectable = false;
					}
					break;
				case "multi-select":
					break;
				case "single-branch-select":
					if (selectedNodesByBranch[rootNode.id] && selectedNodesByBranch[rootNode.id].length > 0 && selectedNodesByBranch[rootNode.id][0] != current.id) {
						shouldBeSelectable = false;
					}
					break;
			}

			switch (popupData.currentField.selectionTarget) {
				case "all":
					break;
				case "leaves":
					//Check if the node is not selected
					var leaveCheckIndex = popupData.selectedTreeRecords.indexOf(current.recordId);
					if (children.length > 0 && leaveCheckIndex == -1) {
						shouldBeSelectable = false;
					}
					break;
			}

			if (shouldBeSelectable) {
				popupData.selectableNodeIds.push(current.id);
			}

			for (var i = 0, len = children.length; i < len; i++) {
				iterateCanBeSelected(children[i], depth + 1, rootNode, isInitial);
			}
		}

		popupData.regenerateCanBeSelected = function (isInitial) {
			//isInitial is added in order to auto collapse nodes that are more than 3 children
			popupData.selectableNodeIds = [];
			for (var i = 0; i < popupData.tree.data.length; i++) {
				iterateCanBeSelected(popupData.tree.data[i], 0, popupData.tree.data[i], isInitial);
			}
		}



		popupData.toggleNodeSelection = function (node) {
			var nodeIndex = popupData.selectedTreeRecords.indexOf(node.recordId);
			var recordsToBeAttached = [];
			var recordsToBeDettached = [];			
			function createRelationChangeSuccessCallback(response) {
				popupData.selectedTreeRecords.push(node.recordId);
				//Add to the branch selected object
				var nodeRootBranchId = node.branch[0];
				if (selectedNodesByBranch[nodeRootBranchId]) {
					selectedNodesByBranch[node.branch[0]].push(node.id);
				}
				else {
					selectedNodesByBranch[node.branch[0]] = [];
					selectedNodesByBranch[node.branch[0]].push(node.id);
				}
				popupData.regenerateCanBeSelected(false);
			}
			function removeRelationChangeSuccessCallback(response) {
				popupData.selectedTreeRecords.splice(nodeIndex, 1);
				var nodeRootBranchId = node.branch[0];

				if (selectedNodesByBranch[nodeRootBranchId]) {
					var selectedIndex = selectedNodesByBranch[nodeRootBranchId].indexOf(node.id)
					selectedNodesByBranch[node.branch[0]].splice(selectedIndex, 1);
				}
				popupData.regenerateCanBeSelected(false);
			}
			function applyRelationChangeErrorCallback(response) { }
			//Node should be unselected. Relations should be severed
			if (nodeIndex > -1) {
				recordsToBeDettached.push($stateParams.recordId);
				webvellaAdminService.manageRecordsRelation(popupData.relation.name, node.recordId, recordsToBeAttached, recordsToBeDettached, removeRelationChangeSuccessCallback, applyRelationChangeErrorCallback);
			}
				//Node should be selected. Relations should be created
			else {
				recordsToBeAttached.push($stateParams.recordId);
				webvellaAdminService.manageRecordsRelation(popupData.relation.name, node.recordId, recordsToBeAttached, recordsToBeDettached, createRelationChangeSuccessCallback, applyRelationChangeErrorCallback);
			}
		}

		popupData.regenerateCanBeSelected(true);

		popupData.clearSelection = function () {
			popupData.selectedTreeRecords = [];
			popupData.regenerateCanBeSelected(false);
		}

		//#endregion

		//#region << Register toggle node events >>

		//This event is later used by the recursive directive that follows
		////READY hook listeners
		var toggleTreeNodeSelectedDestructor = $rootScope.$on("webvellaAdmin-toggleTreeNode-selected", function (event, data) {
			popupData.toggleNodeSelection(data);
		})

		var toggleTreeNodeCollapsedDestructor = $rootScope.$on("webvellaAdmin-toggleTreeNode-collapsed", function (event, data) {
			popupData.toggleNodeCollapse(data);
		})

		////DESCTRUCTOR - hook listeners remove on scope destroy. This avoids duplication, as rootScope is never destroyed and new controller load will duplicate the listener
		$scope.$on("$destroy", function () {
			toggleTreeNodeSelectedDestructor();
			toggleTreeNodeCollapsedDestructor();
		});



		//#endregion

		//#endregion 


	};
	//#endregion

	//#endregion


})();



