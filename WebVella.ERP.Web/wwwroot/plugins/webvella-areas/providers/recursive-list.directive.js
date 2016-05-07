/* recursive-list.directive.js */

/**
* @desc Recursive records list
* @example 	<recursive-list list-meta="ngCtrl.recordsMeta" list-data="ngCtrl.records" relations-list="ngCtrl.relationsMeta"></recursive-list>
 * 
*/

(function () {
	'use strict';

	angular
        .module('webvellaAreas')
        .directive('recursiveList', recursiveList)										   
		.controller('RLAddExistingModalController', RLAddExistingModalController)
		.controller('ViewRelatedRecordModalController', ViewRelatedRecordModalController)
		.controller('RLManageRelatedRecordModalController', RLManageRelatedRecordModalController);

	recursiveList.$inject = ['$compile', '$templateRequest', 'RecursionHelper'];

	
	function recursiveList($compile, $templateRequest, RecursionHelper) {
		//Text Binding (Prefix: @) - only strings
		//One-way Binding (Prefix: &) - $scope functions
		//Two-way Binding (Prefix: =) -$scope.properties
		var directive = {
			controller: DirectiveController,
			templateUrl: '/plugins/webvella-areas/providers/recursive-list.template.html',
			restrict: 'E',
			scope: {
				listData: '=?',
				listMeta: '=?',
				relation: '=?',
				relationsList: '=?',
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
		//$scope.listData = $scope.listData();
		//$scope.listMeta = $scope.listMeta();
		$scope.entity = {};
		$scope.entity.id = $scope.listMeta.entityId;
		$scope.entity.name = $scope.listMeta.entityName;
		//$scope.parentId = $scope.parentId();
		//$scope.canAddExisting = $scope.canAddExisting();
		//$scope.canCreate = $scope.canCreate();
		//$scope.canRemove = $scope.canRemove();
		//$scope.canUpdate = $scope.canUpdate();

		//Validation
		if (!$scope.listMeta || $scope.listMeta.length == 0) {
			$scope.hasError = true;
			$scope.errorMessage = "Error: No list meta provided!";
		}
		else if (!$scope.listMeta.entityId || $scope.listMeta.entityId == "") {
			$scope.hasError = true;
			$scope.errorMessage = "Error: No entityId property in the list meta found!";
		}
		else if (!$scope.listMeta.entityName || $scope.listMeta.entityName == "") {
			$scope.hasError = true;
			$scope.errorMessage = "Error: No entityName property in the list meta found!";
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


		if ($scope.canRemove == undefined) {
			$scope.canRemove = false;
		}
		if ($scope.canAddExisting == undefined) {
			$scope.canAddExisting = false;
		}
		if ($scope.canCreate == undefined) {
			$scope.canCreate = false;
		}
		if ($scope.canUpdate == undefined) {
			$scope.canUpdate = false;
		}



		//Calculate entity stance in the relation
		$scope.dataKind = "target";
		if ($scope.relation && $scope.entity.id === $scope.relation.originEntityId) {
			$scope.dataKind = "origin";
			if ($scope.entity.id === $scope.relation.targetEntityId) {
				$scope.dataKind = "origin-target";
			}
		}


		//#endregion

		//#region << Render >>
		$scope.renderFieldValue = webvellaCoreService.renderFieldValue;
		
		$scope.getRelationLabel = function (item) {
			if (item.fieldLabel) {
				return item.fieldLabel
			}
			else {
				var relationName = item.relationName;
				var relation = findInArray($scope.relationsList, "name", relationName);
				if (relation) {
					return relation.label;
				}
				else {
					return "";
				}
			}
		}

		//#endregion

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
					recordsToBeAttached.push(returnObject.selectedRecordId);
				}
				else if (returnObject.operation == "detach") {
					recordsToBeDetached.push(returnObject.selectedRecordId);
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
				controller: 'RLAddExistingModalController',
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
				var selectedLookupListName = $scope.listMeta.fieldLookupList;
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
					webvellaCoreService.getRecordsByListMeta(defaultLookupList, $scope.entity.name, 1, null, null, getListRecordsSuccessCallback, errorCallback);
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
				controller: 'RLManageRelatedRecordModalController',
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
			var defaultQuickCreateView = null;
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
				var responseObj = {};
				responseObj.meta = defaultQuickCreateView;
				responseObj.data = response.object;
				defer.resolve(responseObj); //Submitting the whole response to manage the error states
			}

			function getViewMetaSuccessCallback(response) {
				var responseObject = {};
				responseObject.meta = response.object;
				responseObject.data = null;
				defer.resolve(responseObject); //Submitting the whole response to manage the error states
			}

			function getEntityMetaSuccessCallback(response) {
				var entityMeta = response.object;
				var selectedQuickCreateViewName = $scope.listMeta.fieldManageView;
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
						webvellaCoreService.getRecordByViewMeta(managedRecord.id, defaultQuickCreateView, $scope.entity.name,null, getViewRecordSuccessCallback, errorCallback);
					}
				}
			}

			webvellaCoreService.getEntityMeta($scope.entity.name, getEntityMetaSuccessCallback, errorCallback);

			return defer.promise;
		}


		$scope.viewRelatedRecordItem = function (record) {
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'viewRelatedRecordModal.html',
				controller: 'ViewRelatedRecordModalController',
				controllerAs: "popupCtrl",
				size: "lg",
				resolve: {
					ngCtrl: function () {
						return $scope;
					},
					resolvedQuickViewRecord: function () {
						return resolveQuickViewRecord(record);
					}
				}
			});
			//On modal exit
			modalInstance.result.then(function () { });

		}

		var resolveQuickViewRecord = function (record) {
			// Initialize
			var defer = $q.defer();
			var defaultQuickViewView = null;
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
				var responseObj = {};
				responseObj.meta = defaultQuickViewView;
				responseObj.data =response.object; 
				defer.resolve(responseObj); //Submitting the whole response to manage the error states
			}

			function getViewMetaSuccessCallback(response) {
				var responseObject = {};
				responseObject.meta = response.object;
				responseObject.data = null;
				defer.resolve(responseObject); //Submitting the whole response to manage the error states
			}

			function getEntityMetaSuccessCallback(response) {
				var entityMeta = response.object;
				//Find the default lookup field if none return null.
				for (var i = 0; i < entityMeta.recordViews.length; i++) {
					if (entityMeta.recordViews[i].default && entityMeta.recordViews[i].type == "quick_view") {
						defaultQuickViewView = entityMeta.recordViews[i];
					}
				}

				if (defaultQuickViewView == null) {
					response.message = "This entity does not have default quick_view view";
					response.success = false;
					errorCallback(response);
				}
				else {
					webvellaCoreService.getRecordByViewMeta(record.id, defaultQuickViewView, $scope.entity.name,null, getViewRecordSuccessCallback, errorCallback);
				}
			}

			webvellaCoreService.getEntityMeta($scope.entity.name, getEntityMetaSuccessCallback, errorCallback);

			return defer.promise;
		}


		//#endregion

	}


	//#region < Modal Controllers >

	RLAddExistingModalController.$inject = ['ngCtrl', '$uibModalInstance', '$log', '$q', '$stateParams', 'resolvedLookupRecords',
        'ngToast', '$timeout', '$state', 'webvellaCoreService'];
	
	function RLAddExistingModalController(ngCtrl, $uibModalInstance, $log, $q, $stateParams, resolvedLookupRecords,
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
		popupCtrl.listData = fastCopy(ngCtrl.listData);
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

			webvellaCoreService.getRecordsByListMeta(popupCtrl.relationLookupList.meta, popupCtrl.parentEntity.name, page, null, null, successCallback, errorCallback);
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
			webvellaCoreService.getRecordsByListMeta(popupCtrl.relationLookupList.meta, popupCtrl.parentEntity.name, popupCtrl.currentPage, null,null, successCallback, errorCallback);

		}
		//#endregion

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
			for (var j = 0; j < popupCtrl.listData.length; j++) {
				if (popupCtrl.listData[j].id == record.id) {
					return true;
				}
			}
			return false;
		}
		//#endregion

	};


	RLManageRelatedRecordModalController.$inject = ['ngCtrl', '$uibModalInstance', '$log', '$q', '$stateParams', '$scope', '$location',
        'ngToast', '$timeout', '$state', 'webvellaCoreService', 'resolvedManagedRecordQuickCreateView'];
	function RLManageRelatedRecordModalController(ngCtrl, $uibModalInstance, $log, $q, $stateParams, $scope, $location,
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
		popupCtrl.relationsList = fastCopy(ngCtrl.relationsList);
		popupCtrl.getRelationLabel = ngCtrl.getRelationLabel;

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
			if (popupCtrl.viewMeta.regions[j].name === "header") {
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


		//#region << Render >>
		popupCtrl.renderFieldValue = webvellaCoreService.renderFieldValue;

		//#endregion

		//#region << Logic >>
		popupCtrl.ok = function () {
			//We need to remove the unsupported field from relations that we display but do not want to submit
			for(var property in popupCtrl.recordData){
				if(property.startsWith("$field") || property.startsWith("$fieldFromRelation") || property.startsWith("$view") || property.startsWith("$viewFromRelation") || property.startsWith("$list") || property.startsWith("$listFromRelation")){
					delete popupCtrl.recordData[property];
				}
			}
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


	ViewRelatedRecordModalController.$inject = ['ngCtrl', '$uibModalInstance', '$log', '$q', '$stateParams', '$scope', '$location',
        'ngToast', '$timeout', '$state', 'webvellaCoreService', 'resolvedQuickViewRecord'];
	function ViewRelatedRecordModalController(ngCtrl, $uibModalInstance, $log, $q, $stateParams, $scope, $location,
        ngToast, $timeout, $state, webvellaCoreService, resolvedQuickViewRecord) {

		//#region << Init >>
		
		var popupCtrl = this;
		popupCtrl.entity = fastCopy(ngCtrl.entity);
		popupCtrl.recordData = fastCopy(resolvedQuickViewRecord.data)[0];
 		popupCtrl.viewMeta = fastCopy(resolvedQuickViewRecord.meta);
		
		popupCtrl.contentRegion = {};
		for (var j = 0; j < popupCtrl.viewMeta.regions.length; j++) {
			if (popupCtrl.viewMeta.regions[j].name === "header") {
				popupCtrl.contentRegion = popupCtrl.viewMeta.regions[j];
			}
		}

		//#endregion

		popupCtrl.renderFieldValue = webvellaCoreService.renderFieldValue;
		popupCtrl.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};
		//#endregion

	};

	//#endregion


})();
