/* recursive-view.directive.js */

/**
* @desc recursive record view
* @example <recursive-view item-meta="viewMeta" records-data="recordInstance" relations-list="ngCtrl.relationsList"></recursive-view>
 * @item-meta => object, the view meta data
 * @records-data => Array, data of the records that need presentation
 * @relations-list => Array all relations list Meta
*/

(function () {
	'use strict';

	angular
        .module('webvellaAreas')
        .directive('recursiveView', recursiveView)
		.controller('RVAddExistingModalController', RVAddExistingModalController)
		.controller('RVManageRelatedRecordModalController', RVManageRelatedRecordModalController);

	recursiveView.$inject = ['$compile', '$templateRequest', 'RecursionHelper', 'webvellaCoreService'];

	
	function recursiveView($compile, $templateRequest, RecursionHelper, webvellaCoreService) {
		//Text Binding (Prefix: @) - only strings
		//One-way Binding (Prefix: &) - $scope functions
		//Two-way Binding (Prefix: =) -$scope.properties
		var directive = {
			controller: DirectiveController,
			templateUrl: '/plugins/webvella-areas/providers/recursive-view.template.html',
			restrict: 'E',
			scope: {
				viewData: '=?',
				viewMeta: '=?',
				relationsList: '=?',
				relation: '=?',
				parentId: '=?',
				canAddExisting: '=?',
				canCreate: '=?',
				canRemove: '=?',
				canUpdate: '=?'
			},
			compile: function (element) {
				return RecursionHelper.compile(element, function (scope, iElement, iAttrs, controller, transcludeFn) {
					// Define your normal link function here.
					// Alternative: instead of passing a function,
					// you can also pass an object with 
					// a 'pre'- and 'post'-link function.
				});
			}
		};
		return directive;
	}


	DirectiveController.$inject = ['$filter', '$log', '$state', '$scope', '$q', '$uibModal', 'ngToast', 'webvellaCoreService'];
	
	function DirectiveController($filter, $log, $state, $scope, $q, $uibModal, ngToast, webvellaCoreService) {
		//#region << Init >>
		//$scope.relation = $scope.relation();
		//$scope.viewMeta = $scope.viewMeta();
		//$scope.viewData = $scope.viewData();
		$scope.entity = {};
		$scope.entity.id = $scope.viewMeta.entityId;
		$scope.entity.name = $scope.viewMeta.entityName;
		//$scope.parentId = $scope.parentId();
		//$scope.canAddExisting = $scope.canAddExisting();
		//$scope.canCreate = $scope.canCreate();
		//$scope.canRemove = $scope.canRemove();
		//$scope.canUpdate = $scope.canUpdate();

		if (!$scope.viewMeta || $scope.viewMeta.length == 0) {
			$scope.hasError = true;
			$scope.errorMessage = "Error: No view meta provided!";
		}
		else if (!$scope.viewMeta.entityId || $scope.viewMeta.entityId == "") {
			$scope.hasError = true;
			$scope.errorMessage = "Error: No entityId property in the view meta found!";
		}
		else if (!$scope.viewMeta.entityName || $scope.viewMeta.entityName == "") {
			$scope.hasError = true;
			$scope.errorMessage = "Error: No entityName property in the view meta found!";
		}


		if (!$scope.parentId || $scope.parentId == "") {
			$scope.canAddExisting = false;
			$scope.canCreate = false;
			$scope.canRemove = false;
			$scope.canUpdate = false;
		}

		if (!$scope.relation) {
			$scope.canAddExisting = false;
			$scope.canRemove = false;
		}


		if (!$scope.canRemove) {
			$scope.canRemove = false;
		}
		if (!$scope.canAddExisting) {
			$scope.canAddExisting = false;
		}
		if (!$scope.canCreate) {
			$scope.canCreate = false;
		}
		if (!$scope.canUpdate) {
			$scope.canUpdate = false;
		}



		$scope.selectedRegion = null;
		for (var i = 0; i < $scope.viewMeta.meta.regions.length; i++) {
			if ($scope.viewMeta.meta.regions[i].name === "default") {
				$scope.selectedRegion = $scope.viewMeta.meta.regions[i];
			}
		}
		if ($scope.selectedRegion == null) {
			$log.error("the subview: " + $scope.viewMeta.name + " does not have a content region");
		}

		//Calculate entity stance in the relation
		$scope.dataKind = "target";
		if ($scope.relation && $scope.entity.id === $scope.relation.originEntityId) {
			$scope.dataKind = "origin";
			if ($scope.entity.id === $scope.relation.targetEntityId) {
				$scope.dataKind = "origin-target";
			}
		}

		//SubViews sections collapsed state - depends on sectionId and recordId as there could be multiple records presented with the same view (section id)
		$scope.sectionCollapsedData = [];
		//Init all sections collapsed state with section[id] and record[id] key

		//Find the records for this view
		var recordIdArray = [];
		for (var l = 0; l < $scope.viewData.length; l++) {
			recordIdArray.push($scope.viewData.id);
		}
		for (var m = 0; m < $scope.selectedRegion.sections.length; m++) {
			$scope.sectionCollapsedData[$scope.selectedRegion.sections[m].id] = {};
			for (var n = 0; n < recordIdArray.length; n++) {
				$scope.sectionCollapsedData[$scope.selectedRegion.sections[m].id][recordIdArray[n]] = $scope.selectedRegion.sections[m].collapsed;
			}
		}
		//#endregion

		//#region << Logic >>
		$scope.toggleSectionCollapse = function (sectionId, recordId) {
			$scope.sectionCollapsedData[sectionId][recordId] = !$scope.sectionCollapsedData[sectionId][recordId];
		}
		//#endregion

		//Columns render 
		$scope.renderFieldValue = webvellaCoreService.renderFieldValue;

		//#region << Logic >>
		$scope.instantDetachRecord = function (record) {
			var returnObject = {
				relationName: $scope.relation.name,
				dataKind: $scope.dataKind,
				selectedRecordId: record.id,
				operation: "detach"
			}
			$scope.processInstantSelection(returnObject);
		};
		$scope.processInstantSelection = function (returnObject) {

			// Initialize

			var recordsToBeAttached = [];
			var recordsToBeDetached = [];

			function successCallback(response) {
				ngToast.create({
					className: 'success',
					content: '<span class="go-green">Success:</span> Change applied'
				});
				$state.reload();
			}

			function errorCallback(response) {
				ngToast.create({
					className: 'error',
					content: '<span class="go-red">Error:</span> Operation failed due to ' + response.message,
					timeout: 7000
				});

			}

			// ** Post relation change between the two records
			if (returnObject.dataKind == "target") {
				//The list entity is target
				//this means that the parent Entity is the origin and relation should be managed through the parent record Id.
				if (returnObject.operation == "attach") {
					recordsToBeAttached.push(selectedRecordId);
				}
				else if (returnObject.operation == "detach") {
					recordsToBeDetached.push(selectedRecordId);
				}
				webvellaCoreService.updateRecordRelation(returnObject.relationName, $scope.parentId, recordsToBeAttached, recordsToBeDetached, successCallback, errorCallback);
			}
			else {
				//The list entity is origin
				//this means that the list Entity is the origin and relation should be managed through the selected record Id.
				if (returnObject.operation == "attach") {
					recordsToBeAttached.push($scope.parentId);
				}
				else if (returnObject.operation == "detach") {
					recordsToBeDetached.push($scope.parentId);
				}
				webvellaCoreService.updateRecordRelation(returnObject.relationName, returnObject.selectedRecordId, recordsToBeAttached, recordsToBeDetached, successCallback, errorCallback);
			}
		}
		//#endregion

		//#region << Modals >>
		$scope.addExistingItem = function () {
			//relationType = 1 (one-to-one) , 2(one-to-many), 3(many-to-many) only 2 or 3 are viable here
			//dataKind - target, origin, origin-target
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'addExistingModal.html',
				controller: 'RVAddExistingModalController',
				controllerAs: "popupCtrl",
				size: "lg",
				resolve: {
					ngCtrl: function () {
						return $scope;
					},
					resolvedLookupRecords: function () {
						return resolveLookupRecords();
					}
				}
			});
			//On modal exit
			modalInstance.result.then(function () { });

		}
		//Resolve function lookup records
		var resolveLookupRecords = function () {
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
			function getListRecordsSuccessCallback(response) {
				defer.resolve(response); //Submitting the whole response to manage the error states
			}

			function getEntityMetaSuccessCallback(response) {
				var entityMeta = response.object;
				var defaultLookupList = null;
				var selectedLookupListName = $scope.viewMeta.fieldLookupList;
				var selectedLookupList = null;
				//Find the default lookup field if none return null.
				for (var i = 0; i < entityMeta.recordLists.length; i++) {
					//Check if the selected lookupListExists
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
					if (selectedLookupList != null) {
						defaultLookupList = selectedLookupList;
					}
					webvellaCoreService.getRecordsByListMeta(defaultLookupList, $scope.entity.name, 1, null, getListRecordsSuccessCallback, errorCallback);
				}
			}

			webvellaCoreService.getEntityMeta($scope.entity.name, getEntityMetaSuccessCallback, errorCallback);

			return defer.promise;
		}

		$scope.manageRelatedRecordItem = function (record) {
			//relationType = 1 (one-to-one) , 2(one-to-many), 3(many-to-many) only 2 or 3 are viable here
			//dataKind - target, origin, origin-target
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'manageRelatedRecordModal.html',
				controller: 'RVManageRelatedRecordModalController',
				controllerAs: "popupCtrl",
				size: "lg",
				resolve: {
					ngCtrl: function () {
						return $scope;
					},
					resolvedManagedRecordQuickCreateView: resolveManagedRecordQuickCreateView(record)
				}
			});
			//On modal exit
			modalInstance.result.then(function () { });
		}

		//Resolve function lookup records
		var resolveManagedRecordQuickCreateView = function (managedRecord) {
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
			function getViewRecordSuccessCallback(response) {
				defer.resolve(response.object); //Submitting the whole response to manage the error states
			}

			function getViewMetaSuccessCallback(response) {
				var responseObject = {};
				responseObject.meta = response.object;
				responseObject.data = null;
				defer.resolve(responseObject); //Submitting the whole response to manage the error states
			}

			function getEntityMetaSuccessCallback(response) {
				var entityMeta = response.object;
				var defaultQuickCreateView = null;
				var selectedQuickCreateViewName = $scope.viewMeta.fieldManageView;
				var selectedQuickCreateView = null;
				//Find the default lookup field if none return null.
				for (var i = 0; i < entityMeta.recordViews.length; i++) {
					//Check if the selected quick create Exists
					if (entityMeta.recordViews[i].name == selectedQuickCreateViewName) {
						selectedQuickCreateView = entityMeta.recordViews[i];
					}
					if (entityMeta.recordViews[i].default && entityMeta.recordViews[i].type == "quick_create") {
						defaultQuickCreateView = entityMeta.recordViews[i];
					}
				}

				if (selectedQuickCreateView == null && defaultQuickCreateView == null) {
					response.message = "This entity does not have selected or default quick create view";
					response.success = false;
					errorCallback(response);
				}
				else {
					if (selectedQuickCreateView != null) {
						defaultQuickCreateView = selectedQuickCreateView;
					}

					if (managedRecord == null) {
						webvellaCoreService.getEntityView(defaultQuickCreateView.name, $scope.entity.name, getViewMetaSuccessCallback, errorCallback);
					}
					else {
						webvellaCoreService.getRecordByViewMeta(managedRecord.id, defaultQuickCreateView, $scope.entity.name, getViewRecordSuccessCallback, errorCallback);
					}
				}
			}

			webvellaCoreService.getEntityMeta($scope.entity.name, getEntityMetaSuccessCallback, errorCallback);

			return defer.promise;
		}

		//#endregion

	}

	//#region < Modal Controllers >

	RVAddExistingModalController.$inject = ['ngCtrl', '$uibModalInstance', '$log', '$q', '$stateParams', 'resolvedLookupRecords',
        'ngToast', '$timeout', '$state', 'webvellaCoreService'];
	
	function RVAddExistingModalController(ngCtrl, $uibModalInstance, $log, $q, $stateParams, resolvedLookupRecords,
        ngToast, $timeout, $state, webvellaCoreService) {

		//#region << Init >>
		
		var popupCtrl = this;
		popupCtrl.currentPage = 1;
		popupCtrl.relation = fastCopy(ngCtrl.relation);
		popupCtrl.hasWarning = false;
		popupCtrl.warningMessage = "";
		popupCtrl.parentEntity = fastCopy(ngCtrl.entity);
		popupCtrl.searchQuery = null;
		popupCtrl.parentRecordId = fastCopy(ngCtrl.parentId);
		popupCtrl.viewData = fastCopy(ngCtrl.viewData);
		popupCtrl.dataKind = fastCopy(ngCtrl.dataKind);
		//Get the default lookup list for the entity
		if (resolvedLookupRecords.success) {
			popupCtrl.relationLookupList = fastCopy(resolvedLookupRecords.object);
		}
		else {
			popupCtrl.hasWarning = true;
			popupCtrl.warningMessage = resolvedLookupRecords.message;
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

			webvellaCoreService.getRecordsByListMeta(popupCtrl.relationLookupList.meta, popupCtrl.parentEntity.name, page, null, successCallback, errorCallback);
		}

		//#endregion

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

			function errorCallback(response) {
				ngToast.create({
					className: 'error',
					content: '<span class="go-green">Error:</span> ' + response.message,
					timeout: 7000
				});
			}
			webvellaCoreService.getRecordsByListMeta(popupCtrl.relationLookupList.meta, popupCtrl.parentEntity.name, popupCtrl.currentPage, null, successCallback, errorCallback);

		}
		//#endregion

		//Render
		popupCtrl.renderFieldValue = webvellaCoreService.renderFieldValue;

		//#region << Logic >>
		popupCtrl.instantAttachRecord = function (record) {
			var returnObject = {
				relationName: popupCtrl.relation.name,
				dataKind: popupCtrl.dataKind,
				selectedRecordId: record.id,
				operation: "attach"
			}
			popupCtrl.processInstantSelection(returnObject);
			$uibModalInstance.close();
		};

		popupCtrl.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};

		popupCtrl.processInstantSelection = ngCtrl.processInstantSelection;

		popupCtrl.isSelected = function (record) {
			for (var j = 0; j < popupCtrl.viewData.length; j++) {
				if (popupCtrl.viewData[j].id == record.id) {
					return true;
				}
			}
			return false;
		}
		//#endregion

	};


	RVManageRelatedRecordModalController.$inject = ['ngCtrl', '$uibModalInstance', '$log', '$q', '$stateParams', '$scope', '$location',
        'ngToast', '$timeout', '$state', 'webvellaCoreService', 'resolvedManagedRecordQuickCreateView'];
	function RVManageRelatedRecordModalController(ngCtrl, $uibModalInstance, $log, $q, $stateParams, $scope, $location,
        ngToast, $timeout, $state, webvellaCoreService, resolvedManagedRecordQuickCreateView) {

		//#region << Init >>
		
		var popupCtrl = this;
		popupCtrl.isFromRelation = true;
		if (!ngCtrl.relation) {
			popupCtrl.isFromRelation = false;
		}
		else {
			popupCtrl.relation = fastCopy(ngCtrl.relation);
			popupCtrl.dataKind = fastCopy(ngCtrl.dataKind);
		}
		popupCtrl.entity = fastCopy(ngCtrl.entity);
		if (resolvedManagedRecordQuickCreateView.data == null) {
			popupCtrl.isEdit = false;
			popupCtrl.recordData = {};
		}
		else {
			popupCtrl.recordData = fastCopy(resolvedManagedRecordQuickCreateView.data)[0];
			popupCtrl.isEdit = true;
		}

		popupCtrl.viewMeta = fastCopy(resolvedManagedRecordQuickCreateView.meta);
		popupCtrl.contentRegion = {};
		for (var j = 0; j < popupCtrl.viewMeta.regions.length; j++) {
			if (popupCtrl.viewMeta.regions[j].name === "default") {
				popupCtrl.contentRegion = popupCtrl.viewMeta.regions[j];
			}
		}

		//#endregion

		//#region << Fields init >>
		popupCtrl.files = {}; // this is the data wrapper for the temporary upload objects that will be used in the html and for which we will generate watches below
		popupCtrl.progress = {}; //Needed for file and image uploads
		var availableViewFields = [];
		//Init default values of fields
		if (popupCtrl.contentRegion != null) {
			availableViewFields = webvellaCoreService.getItemsFromRegion(popupCtrl.contentRegion);
			for (var j = 0; j < availableViewFields.length; j++) {
				if (!popupCtrl.recordData[availableViewFields[j].meta.name] && availableViewFields[j].type === "field") {
					switch (availableViewFields[j].meta.fieldType) {

						case 2: //Checkbox
							popupCtrl.recordData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							break;

						case 3: //Currency
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								popupCtrl.recordData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 4: //Date
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								if (availableViewFields[j].meta.useCurrentTimeAsDefaultValue) {
									popupCtrl.recordData[availableViewFields[j].meta.name] = moment().toISOString();
								}
								else if (availableViewFields[j].meta.defaultValue) {
									popupCtrl.recordData[availableViewFields[j].meta.name] = moment(availableViewFields[j].meta.defaultValue).toISOString();
								}
							}
							break;
						case 5: //Date
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								if (availableViewFields[j].meta.useCurrentTimeAsDefaultValue) {
									popupCtrl.recordData[availableViewFields[j].meta.name] = moment().toISOString();
								}
								else if (availableViewFields[j].meta.defaultValue) {
									popupCtrl.recordData[availableViewFields[j].meta.name] = moment(availableViewFields[j].meta.defaultValue).toISOString();
								}
							}
							break;
						case 6: //Email
							break;
						case 7: //File
							popupCtrl.progress[availableViewFields[j].meta.name] = 0;
							if (availableViewFields[j].meta.required) {
								popupCtrl.recordData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 8: //HTML
							if (availableViewFields[j].meta.required) {
								popupCtrl.recordData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 9: //Image
							popupCtrl.progress[availableViewFields[j].meta.name] = 0;
							if (availableViewFields[j].meta.required) {
								popupCtrl.recordData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 10: //TextArea
							if (availableViewFields[j].meta.required) {
								popupCtrl.recordData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 11: //Multiselect
							if (availableViewFields[j].meta.required) {
								popupCtrl.recordData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 12: //Number
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								popupCtrl.recordData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 13: //Password
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								//Does not have default value
								//popupCtrl.recordData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
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
								//popupCtrl.recordData[availableViewFields[j].meta.name] = resultPercentage;
							}
							break;
						case 15: //Phone
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								popupCtrl.recordData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 16: //Guid
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								popupCtrl.recordData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 17: //Dropdown
							if (availableViewFields[j].meta.required && availableViewFields[j].meta.defaultValue) {
								popupCtrl.recordData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 18: //Text
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								popupCtrl.recordData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 19: //URL
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								popupCtrl.recordData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
					}


				}
			}
		}

		//#region << File >>
		popupCtrl.uploadedFileName = "";
		popupCtrl.upload = function (file, item) {
			if (file != null) {
				popupCtrl.uploadedFileName = item.dataName;
				popupCtrl.moveSuccessCallback = function (response) {
					$timeout(function () {
						popupCtrl.recordData[popupCtrl.uploadedFileName] = response.object.url;
					}, 1);
				}

				popupCtrl.uploadSuccessCallback = function (response) {
					var tempPath = response.object.url;
					var fileName = response.object.filename;
					var targetPath = "/fs/" + item.fieldId + "/" + fileName;
					var overwrite = true;
					webvellaCoreService.moveFileFromTempToFS(tempPath, targetPath, overwrite, popupCtrl.moveSuccessCallback, popupCtrl.uploadErrorCallback);
				}
				popupCtrl.uploadErrorCallback = function (response) {
					alert(response.message);
				}
				popupCtrl.uploadProgressCallback = function (response) {
					$timeout(function () {
						popupCtrl.progress[popupCtrl.uploadedFileName] = parseInt(100.0 * response.loaded / response.total);
					}, 1);
				}
				webvellaCoreService.uploadFileToTemp(file, item.meta.name, popupCtrl.uploadProgressCallback, popupCtrl.uploadSuccessCallback, popupCtrl.uploadErrorCallback);
			}
		};

		popupCtrl.deleteFileUpload = function (item) {
			var fieldName = item.dataName;
			var filePath = popupCtrl.recordData[fieldName];

			function deleteSuccessCallback(response) {
				$timeout(function () {
					popupCtrl.recordData[fieldName] = null;
					popupCtrl.progress[fieldName] = 0;
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

			webvellaCoreService.deleteFileFromFS(filePath, deleteSuccessCallback, deleteFailedCallback);

		}
		//#endregion

		//#region << Html >>
		//Should use scope as it is not working with ngCtrl
		$scope.editorOptions = {
			language: GlobalLanguage,
			skin: 'moono',
			height: '160',
			contentsCss: '/plugins/webvella-core/css/editor.css',
			extraPlugins: "sourcedialog",
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
				{ name: 'insert', items: ['Image', 'Table', 'SpecialChar','Sourcedialog'] }, '/',
			]
		};	

		popupCtrl.toggleSectionCollapse = function (section) {
			section.collapsed = !section.collapsed;
		}

		popupCtrl.calendars = {};
		popupCtrl.openCalendar = function (event, name) {
			popupCtrl.calendars[name] = true;
		}

		//#endregion


		//#endregion

		//#region << Logic >>
		popupCtrl.ok = function () {
			if (!popupCtrl.isEdit) {
				webvellaCoreService.createRecord(popupCtrl.entity.name, popupCtrl.recordData, createSuccessCallback, manageErrorCallback);
			}
			else {
				webvellaCoreService.updateRecord(popupCtrl.recordData.id, popupCtrl.entity.name, popupCtrl.recordData, successCallback, manageErrorCallback);
			}
		}

		/// Aux
		function createSuccessCallback(response) {
			if (!popupCtrl.isFromRelation) {
				$state.reload();
				$uibModalInstance.close('success');
			}
			else {
				var returnObject = {
					relationName: popupCtrl.relation.name,
					dataKind: popupCtrl.dataKind,
					selectedRecordId: response.object.data[0].id,
					operation: "attach"
				}
				popupCtrl.processInstantSelection = ngCtrl.processInstantSelection;
				popupCtrl.processInstantSelection(returnObject);
				$uibModalInstance.close('success');
			}
		}

		function successCallback(response) {
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success:</span> ' + 'The record was successfully updated'
			});
			$state.reload();
			$uibModalInstance.close('success');
		}

		function manageErrorCallback(response) {
			var location = $location;
			//Process the response and generate the validation Messages
			webvellaCoreService.generateValidationMessages(response, popupCtrl, popupCtrl.recordData, location);
		}

		popupCtrl.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};
		//#endregion

	};

	//#endregion


})();