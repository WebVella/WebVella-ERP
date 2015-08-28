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
	    .controller('ManageRelationFieldModalSingleSelectionController', ManageRelationFieldModalSingleSelectionController);

	// Configuration ///////////////////////////////////
	config.$inject = ['$stateProvider'];

	/* @ngInject */
	function config($stateProvider) {
		$stateProvider.state('webvella-areas-record-view', {
			parent: 'webvella-areas-base',
			url: '/:areaName/:entityName/:recordId/view/:viewName/section/:sectionName/:filter/:page',
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
				resolvedCurrentEntityMeta: resolveCurrentEntityMeta,
				resolvedEntityRelationsList: resolveEntityRelationsList,
			},
			data: {

			}
		});
	};


	//#region << Run >> //////////////////////////////////////
	run.$inject = ['$log'];
	/* @ngInject */
	function run($log) {
		$log.debug('webvellaAreas>entities> BEGIN module.run');

		$log.debug('webvellaAreas>entities> END module.run');
	};
    //#endregion

	//#region << Resolve Function >> /////////////////////////
	resolveCurrentView.$inject = ['$q', '$log', 'webvellaAreasService', '$stateParams', '$state', '$timeout'];
	/* @ngInject */
	function resolveCurrentView($q, $log, webvellaAreasService, $stateParams, $state, $timeout) {
		$log.debug('webvellaAdmin>entity-views>resolveCurrentView BEGIN state.resolved');
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

		webvellaAreasService.getViewRecord($stateParams.recordId, $stateParams.viewName, $stateParams.entityName, successCallback, errorCallback);

		// Return
		$log.debug('webvellaAdmin>entity-views>resolveCurrentView END state.resolved');
		return defer.promise;
	}

	resolveCurrentEntityMeta.$inject = ['$q', '$log', 'webvellaAdminService', '$stateParams', '$state', '$timeout'];
	/* @ngInject */
	function resolveCurrentEntityMeta($q, $log, webvellaAdminService, $stateParams, $state, $timeout) {
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

		webvellaAdminService.getEntityMeta($stateParams.entityName, successCallback, errorCallback);

		// Return
		$log.debug('webvellaAdmin>entity-details> END state.resolved');
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
	controller.$inject = ['$filter', '$modal', '$log', '$q', '$rootScope', '$state', '$stateParams', '$scope', 'pageTitle', 'webvellaRootService', 'webvellaAdminService', 'webvellaAreasService',
        'resolvedSitemap', '$timeout', 'resolvedCurrentView', 'ngToast', 'wvAppConstants', 'resolvedCurrentEntityMeta', 'resolvedEntityRelationsList'];

	/* @ngInject */
	function controller($filter,$modal, $log,$q, $rootScope, $state,$stateParams, $scope, pageTitle, webvellaRootService, webvellaAdminService,webvellaAreasService,
        resolvedSitemap, $timeout, resolvedCurrentView, ngToast, wvAppConstants, resolvedCurrentEntityMeta, resolvedEntityRelationsList) {
		$log.debug('webvellaAreas>entities> BEGIN controller.exec');
		/* jshint validthis:true */
		var contentData = this;
		contentData.isView = false;
		contentData.isList = false;
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
		contentData.recordView = angular.copy(resolvedCurrentView.meta);
		contentData.contentRegion = null;
		contentData.sidebarRegion = contentData.recordView.sidebar;
		for (var i = 0; i < contentData.recordView.regions.length; i++) {
			if (contentData.recordView.regions[i].name === "content") {
				contentData.contentRegion = contentData.recordView.regions[i];
			}
		}
		contentData.viewData = angular.copy(resolvedCurrentView.data[0]);
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

	    //#region << View Seciton >>
		contentData.viewSection = {};
	    //<< Initialize section label >>
		contentData.viewSection.label = "General";
		for (var i = 0; i < contentData.recordView.sidebar.items.length; i++) {
		    if ($stateParams.sectionName == contentData.recordView.sidebar.items[i].meta.name) {
		        contentData.viewSection.label = contentData.recordView.sidebar.items[i].meta.label;
		    }
		}


		if ($stateParams.sectionName == "$") {
		    //The default view is active
		    contentData.isView = true;
		}
		else {
		    //One of the sidebar view or lists is active
		}

	    //#endregion

		//#region << Logic >>

		contentData.toggleSectionCollapse = function (section) {
			section.collapsed = !section.collapsed;
		}

		contentData.htmlFieldUpdate = function (item) {
			contentData.fieldUpdate(item, contentData.viewData[item.dataName]);
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
						data = moment(data).toISOString();
						break;
					case 5: //Datetime
						if (!data && item.meta.required) {
							return "This is a required field";
						}
						data = moment(data).toISOString();
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
			    contentData.viewData = angular.copy(response.object.data[0]);
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
		contentData.getAutoIncrementString = function (item) {
			var fieldValue = contentData.viewData[item.dataName];
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
			var fieldValue = contentData.viewData[item.dataName];
			if (fieldValue) {
			    return "<i class='fa fa-fw fa-check go-green'></i> true";
			}
			else {
			    return "<i class='fa fa-fw fa-close go-red'></i> false";
			}
		}
		//Currency
		contentData.getCurrencyString = function (item) {
			var fieldValue = contentData.viewData[item.dataName];
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
			var fieldValue = contentData.viewData[item.dataName];
			if (!fieldValue) {
				return "";
			}
			else {
				return $filter('date')(fieldValue, "dd MMM yyyy");
			}
		}
		contentData.getTimeString = function (item) {
			var fieldValue = contentData.viewData[item.dataName];
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

		/////////Register variables
		for (var sectionIndex = 0; sectionIndex < contentData.contentRegion.sections.length; sectionIndex++) {
			for (var rowIndex = 0; rowIndex < contentData.contentRegion.sections[sectionIndex].rows.length; rowIndex++) {
				for (var columnIndex = 0; columnIndex < contentData.contentRegion.sections[sectionIndex].rows[rowIndex].columns.length; columnIndex++) {
					for (var itemIndex = 0; itemIndex < contentData.contentRegion.sections[sectionIndex].rows[rowIndex].columns[columnIndex].items.length; itemIndex++) {
						if (contentData.contentRegion.sections[sectionIndex].rows[rowIndex].columns[columnIndex].items[itemIndex].meta.fieldType === 7
							|| contentData.contentRegion.sections[sectionIndex].rows[rowIndex].columns[columnIndex].items[itemIndex].meta.fieldType === 9) {
							var item = contentData.contentRegion.sections[sectionIndex].rows[rowIndex].columns[columnIndex].items[itemIndex];
							var FieldName = item.dataName;
							contentData.progress[FieldName] = 0;
						}
					}
				}
			}
		}

		contentData.upload = function (files, item) {
			var fieldName = item.dataName;
			function moveSuccessCallback(response) {
				contentData.viewData[fieldName] = response.object.url;
				contentData.fieldUpdate(item, response.object.url);
			}

			function uploadSuccessCallback(response) {
				var tempPath = response.object.url;
				var fileName = response.object.filename;
				var targetPath = "/fs/" + item.fieldId + "/" + fileName;
				var overwrite = false;
				webvellaAdminService.moveFileFromTempToFS(tempPath, targetPath, overwrite, moveSuccessCallback, uploadErrorCallback);
			}
			function uploadErrorCallback(response) {
				alert(response.message);
			}
			function uploadProgressCallback(response) {
				contentData.progress[dataName] = parseInt(100.0 * response.loaded / response.total);
			}
			webvellaAdminService.uploadFileToTemp(files, item.meta.name, uploadProgressCallback, uploadSuccessCallback, uploadErrorCallback);
		};

		contentData.deleteFileUpload = function (item) {
			var fieldName = item.dataName;
			var filePath = contentData.viewData[fieldName];

			function deleteSuccessCallback(response) {
				$timeout(function () {
					contentData.viewData[fieldName] = null;
					contentData.progress[fieldName] = 0;
					contentData.fieldUpdate(item,null);
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

		//Checkbox list
		contentData.getCheckboxlistString = function (fieldData, array) {
			if (fieldData) {
				var selected = [];
				angular.forEach(array, function (s) {
					if (fieldData.indexOf(s.key) >= 0) {
						selected.push(s.value);
					}
				});
				return selected.length ? selected.join(', ') : 'empty';
			}
			else {
				return 'empty';
			}
		}

		//Password
		contentData.dummyPasswordModels = {};//as the password value is of know use being encrypted, we will assign dummy models
		//Dropdown
		contentData.getDropdownString = function (fieldData, array) {
			var selected = $filter('filter')(array, { key: fieldData });
			return (fieldData && selected.length) ? selected[0].value : 'empty';
		}

	    //Percent
		$scope.Math = window.Math;
		function multiplyDecimals(val1, val2, decimalPlaces) {
		    var helpNumber = 100;
		    for (var i = 0; i < decimalPlaces; i++) {
		        helpNumber = helpNumber * 10;
		    }
		    var temp1 = $scope.Math.round(val1 * helpNumber);
		    var temp2 = $scope.Math.round(val2 * helpNumber);
		    return (temp1 * temp2) / (helpNumber * helpNumber);
		}

		contentData.getPercentString = function (item) {
		    var fieldValue = contentData.viewData[item.dataName];

		    if (!fieldValue) {
		        return "empty";
		    }
		    else {
		        //JavaScript has a bug when multiplying decimals
		        //The way to correct this is to multiply the decimals before multiple their values,
		        var resultPercentage = 0.00;
		        resultPercentage = multiplyDecimals(fieldValue,100,3);
		        return resultPercentage + "%";
		        }

		}

	    //Test

		contentData.getItem = function (item) {
		    var i = 0;
		}


		//#endregion

	    //#region << Modals >>

	    ////Relation field

        ////////////////////
		contentData.openManageRelationFieldModal = function (item, relationType, dataKind) {
		    //relationType = 1 (one-to-one) , 2(one-to-many), 3(many-to-many)
		    //dataKind - target, origin, origin-target

            //Select ONE item modal
		    if (relationType == 1 || (relationType == 2 && dataKind == "target")) {
		        var modalInstance = $modal.open({
		            animation: false,
		            templateUrl: 'manageRelationFieldModalSingleSelection.html',
		            controller: 'ManageRelationFieldModalSingleSelectionController',
		            controllerAs: "popupData",
		            size: "lg",
		            resolve: {
		                contentData: function () {
		                    return contentData;
		                },
		                selectedItem: function () {
		                    return item;
		                },
		                resolvedLookupRecords: function () {
		                    var i = 0;
		                    return resolveLookupRecords(item);
		                }
		            }
		        });


                //On modal exit
		        modalInstance.result.then(function (selectedRecordId) {

		            //Get relation from the relations list
                    //Get the origin record data.



		            function patchRecordSuccessCallback(response) {
		                webvellaRootService.GoToState($state, $state.current.name, contentData.stateParams);
		            }

		            function patchRecordErrorCallback(response) {

		            }

		            function getRelationSuccessCallback(response) {
		                var currentRelation = response.object;

		                //#region < Relation type 1 - 1:1 > ///////////////////////////////////////
		                if (currentRelation.relationType == 1) {

		                    if (dataKind == "origin") {
                                //CURRENT entity == Origin entity
		                        //1. Look in the ORIGIN entity for the field ID in the relation and get its name

		                        //2. Get the value of the field by its name from the ORIGIN entity

		                        //3. Get the TARGET entity and find the target field name by its ID in the relation

		                        //4. Patch the TARGET entity field with the origin field value
		                    }
		                    else if (dataKind == "target") {
		                        //CURRENT entity == Target entity

		                    }
		                    else if (dataKind == "origin-target") {

		                    }

		                }
                        //#endregion

		                //Case 2 - 1:N or N:1
		                else if (currentRelation.relationType == 2) {
		                    //Find which is the needed field - target or origin

		                    //Check the relation meta for the targetFieldId and than find the field name from the entityMeta
		                    var targetRelationFieldId = currentRelation.targetFieldId;
		                    var targetFieldName = null;
		                    for (var i = 0; i < contentData.currentEntity.fields.length; i++) {
		                        if (contentData.currentEntity.fields[i].id == targetRelationFieldId) {
		                            targetFieldName = contentData.currentEntity.fields[i].name;
		                            break;
		                        }
		                    }
		                    //Update the record with the new Id, get the returned object and substitute the presented one.
		                    var patchObject = {};
		                    patchObject[targetFieldName] = selectedRecordId;
		                    webvellaAdminService.patchRecord(contentData.stateParams.recordId, contentData.currentEntity.name, patchObject, patchRecordSuccessCallback, patchRecordErrorCallback);
		                }
		                    //Case 3 - Many to many relation
		                else if (currentRelation.relationType == 3) {
		                    //What is the current entity in the relation - target or origin

		                    //Find the relation record
		                }
		            }

		            function getRelationErrorCallback(response) {

		            }

		            //find from the relation name which property of the current entity should set with the new Id
		            webvellaAdminService.getRelationByName(item.relationName, getRelationSuccessCallback, getRelationErrorCallback);
		        });
		    }
		}

	    //Resolve function lookup records
		var resolveLookupRecords = function (item) {
		    // Initialize
		    var defer = $q.defer();

		    // Process
		    function errorCallback(response) {
		        defer.resolve(response.object);
		    }
		    function getListRecordsSuccessCallback(response) {
		        defer.resolve(response.object);
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
		            defer.resolve({});
		        }
		        else {
		            webvellaAreasService.getListRecords(defaultLookupList.name, entityMeta.name, "all", 1, getListRecordsSuccessCallback, errorCallback);
		        }
		    }

		    webvellaAdminService.getEntityMeta(item.entityName, getEntityMetaSuccessCallback, errorCallback);

		    return defer.promise;
		}

        //#endregion


		$log.debug('webvellaAreas>entities> END controller.exec');
	}


    //#region < Modal Controllers >


    ////////////////////////
	ManageRelationFieldModalSingleSelectionController.$inject = ['contentData', '$modalInstance', '$log', '$q', 'resolvedLookupRecords', 'selectedItem', 'webvellaAdminService', 'webvellaAreasService', 'webvellaRootService', 'ngToast', '$timeout', '$state'];

    /* @ngInject */
	function ManageRelationFieldModalSingleSelectionController(contentData, $modalInstance, $log, $q, resolvedLookupRecords, selectedItem, webvellaAdminService, webvellaAreasService, webvellaRootService, ngToast, $timeout, $state) {
	    $log.debug('webvellaAdmin>entities>deleteFieldModal> START controller.exec');
	    /* jshint validthis:true */
	    var popupData = this;
	    popupData.currentPage = 1;
	    popupData.parentData = angular.copy(contentData);
	    popupData.selectedItem = angular.copy(selectedItem);
	    //Get the default lookup list for the entity
	    popupData.relationLookupList = angular.copy(resolvedLookupRecords);

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
        

	    //#region << Logic >>

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

	    popupData.selectInstitutionCategory = function (record) {
	        $modalInstance.close(record.id);
            //category_id
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





	    $log.debug('webvellaAdmin>entities>createEntityModal> END controller.exec');
	};

    //#endregion


})();


