/* recursive-list.directive.js */

/**
* @desc Recursive records list
* @example 	<recursive-list list-meta="contentData.recordsMeta" list-data="contentData.records" relations-list="contentData.relationsMeta"></recursive-list>
 * 
*/

(function () {
	'use strict';

	angular
        .module('webvellaAreas')
        .directive('recursiveList', directive)
		.controller('RLAddExistingModalController', RLAddExistingModalController)
		.controller('RLManageRelatedRecordModalController', RLManageRelatedRecordModalController);

	directive.$inject = ['$compile', '$templateRequest', 'RecursionHelper'];

	/* @ngInject */
	function directive($compile, $templateRequest, RecursionHelper) {
		//Text Binding (Prefix: @)
		//One-way Binding (Prefix: &)
		//Two-way Binding (Prefix: =)
		var directive = {
			controller: DirectiveController,
			templateUrl: '/plugins/webvella-areas/providers/recursive-list.template.html',
			restrict: 'E',
			scope: {
				listData: '&',
				listMeta: '&',
				relation: '&',
				parentId: '&',
				canAddExisting: '&',
				canCreate: '&',
				canRemove: '&',
				canEdit: '&'
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


	DirectiveController.$inject = ['$filter', '$log', '$state', '$scope', '$q', '$uibModal', 'ngToast', 'webvellaAreasService', 'webvellaAdminService'];
	/* @ngInject */
	function DirectiveController($filter, $log, $state, $scope, $q, $uibModal, ngToast, webvellaAreasService, webvellaAdminService) {

		//#region << Init >>
		$scope.relation = $scope.relation();
		$scope.listData = $scope.listData();
		$scope.listMeta = $scope.listMeta();
		$scope.entity = {};
		$scope.entity.id = $scope.listMeta.entityId;
		$scope.entity.name = $scope.listMeta.entityName;
		$scope.parentId = $scope.parentId();
		$scope.canAddExisting = $scope.canAddExisting();
		$scope.canCreate = $scope.canCreate();
		$scope.canRemove = $scope.canRemove();
		$scope.canEdit = $scope.canEdit();

		//Validation
		if (!$scope.parentId || $scope.parentId == "") {
			$scope.hasError = true;
			$scope.errorMessage = "Error: No list parent Id provided!";
		}
		else if (!$scope.listData || $scope.listData.length == 0) {
			$scope.hasError = true;
			$scope.errorMessage = "Error: No list data provided!";
		}
		else if (!$scope.listMeta || $scope.listMeta.length == 0) {
			$scope.hasError = true;
			$scope.errorMessage = "Error: No list meta provided!";
		}
		else if (!$scope.listMeta || $scope.listMeta.length == 0) {
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

		if (!$scope.canRemove) {
			$scope.canRemove = false;
		}
		if (!$scope.canAddExisting) {
			$scope.canAddExisting = false;
		}
		else if (!$scope.relation) {
			$scope.canAddExisting = false;
			$scope.canRemove = false;
		}

		if (!$scope.canCreate) {
			$scope.canCreate = false;
		}
		if (!$scope.canEdit) {
			$scope.canEdit = false;
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

		//#region << Columns render>> //////////////////////////////////////

		//1.Auto increment
		$scope.getAutoIncrementString = webvellaAreasService.getAutoIncrementString;
		//2.Checkbox
		$scope.getCheckboxString = webvellaAreasService.getCheckboxString;
		//3.Currency
		$scope.getCurrencyString = webvellaAreasService.getCurrencyString;
		//4.Date
		$scope.getDateString = webvellaAreasService.getDateString;
		//5.Datetime
		$scope.getDateTimeString = webvellaAreasService.getDateTimeString;
		//6.Email
		$scope.getEmailString = webvellaAreasService.getEmailString;
		//7.File
		$scope.getFileString = webvellaAreasService.getFileString;
		//8.Html
		$scope.getHtmlString = webvellaAreasService.getHtmlString;
		//9.Image
		$scope.getImageString = webvellaAreasService.getImageString;
		//10.Textarea
		$scope.getTextareaString = webvellaAreasService.getTextareaString;
		//11.Multiselect
		$scope.getMultiselectString = webvellaAreasService.getMultiselectString;
		//12.Number
		$scope.getNumberString = webvellaAreasService.getNumberString;
		//13.Password
		$scope.getPasswordString = webvellaAreasService.getPasswordString;
		//14.Percent
		$scope.getPercentString = webvellaAreasService.getPercentString;
		//15.Phone
		$scope.getPhoneString = webvellaAreasService.getPhoneString;
		//15.Guid
		$scope.getGuidString = webvellaAreasService.getGuidString;
		//17.Dropdown
		$scope.getDropdownString = webvellaAreasService.getDropdownString;
		//18. Text
		$scope.getTextString = webvellaAreasService.getTextString;
		//18.Url
		$scope.getUrlString = webvellaAreasService.getUrlString;
		//#endregion

		//#region << Logic >>
		$scope.instantDettachRecord = function (record) {
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
			var recordsToBeDettached = [];

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
					recordsToBeDettached.push(selectedRecordId);
				}
				webvellaAdminService.manageRecordsRelation(returnObject.relationName, $scope.parentId, recordsToBeAttached, recordsToBeDettached, successCallback, errorCallback);
			}
			else {
				//The list entity is origin
				//this means that the list Entity is the origin and relation should be managed through the selected record Id.
				if (returnObject.operation == "attach") {
					recordsToBeAttached.push($scope.parentId);
				}
				else if (returnObject.operation == "detach") {
					recordsToBeDettached.push($scope.parentId);
				}
				webvellaAdminService.manageRecordsRelation(returnObject.relationName, returnObject.selectedRecordId, recordsToBeAttached, recordsToBeDettached, successCallback, errorCallback);
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
				controllerAs: "popupData",
				size: "lg",
				resolve: {
					contentData: function () {
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
					webvellaAreasService.getListRecords(defaultLookupList.name, $scope.entity.name, "all", 1, null, getListRecordsSuccessCallback, errorCallback);
				}
			}

			webvellaAdminService.getEntityMeta($scope.entity.name, getEntityMetaSuccessCallback, errorCallback);

			return defer.promise;
		}

		$scope.manageRelatedRecordItem = function (record) {
			//relationType = 1 (one-to-one) , 2(one-to-many), 3(many-to-many) only 2 or 3 are viable here
			//dataKind - target, origin, origin-target
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'manageRelatedRecordModal.html',
				controller: 'RLManageRelatedRecordModalController',
				controllerAs: "popupData",
				size: "lg",
				resolve: {
					contentData: function () {
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

				//Find the default lookup field if none return null.
				for (var i = 0; i < entityMeta.recordViews.length; i++) {
					if (entityMeta.recordViews[i].default && entityMeta.recordViews[i].type == "quick_create") {
						defaultQuickCreateView = entityMeta.recordViews[i];
						break;
					}
				}

				if (defaultQuickCreateView == null) {
					response.message = "This entity does not have a default quick create view";
					response.success = false;
					errorCallback(response);
				}
				else {
					if (managedRecord == null) {
						webvellaAreasService.getViewByName(defaultQuickCreateView.name, $scope.entity.name, getViewMetaSuccessCallback, errorCallback);
					}
					else {
						webvellaAreasService.getViewRecord(managedRecord.id, defaultQuickCreateView.name, $scope.entity.name, getViewRecordSuccessCallback, errorCallback);
					}
				}
			}

			webvellaAdminService.getEntityMeta($scope.entity.name, getEntityMetaSuccessCallback, errorCallback);

			return defer.promise;
		}

		//#endregion


	}


	//#region < Modal Controllers >

	RLAddExistingModalController.$inject = ['contentData', '$modalInstance', '$log', '$q', '$stateParams', 'resolvedLookupRecords',
        'ngToast', '$timeout', '$state', 'webvellaAreasService', 'webvellaAdminService'];
	/* @ngInject */
	function RLAddExistingModalController(contentData, $modalInstance, $log, $q, $stateParams, resolvedLookupRecords,
        ngToast, $timeout, $state, webvellaAreasService, webvellaAdminService) {

		$log.debug('webvellaAdmin>recursive-list>RLAddExistingModalController> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
		//#region << Init >>
		/* jshint validthis:true */
		var popupData = this;
		popupData.currentPage = 1;
		popupData.relation = fastCopy(contentData.relation);
		popupData.hasWarning = false;
		popupData.warningMessage = "";
		popupData.parentEntity = fastCopy(contentData.entity);
		popupData.searchQuery = null;
		popupData.parentRecordId = fastCopy(contentData.parentId);
		popupData.listData = fastCopy(contentData.listData);
		popupData.dataKind = fastCopy(contentData.dataKind);
		//Get the default lookup list for the entity
		if (resolvedLookupRecords.success) {
			popupData.relationLookupList = fastCopy(resolvedLookupRecords.object);
		}
		else {
			popupData.hasWarning = true;
			popupData.warningMessage = resolvedLookupRecords.message;
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

			webvellaAreasService.getListRecords(popupData.relationLookupList.meta.name, popupData.parentEntity.name, "all", page, popupData.searchQuery, successCallback, errorCallback);
		}

		//#endregion

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

			function errorCallback(response) {

			}
			webvellaAreasService.getListRecords(popupData.relationLookupList.meta.name, popupData.parentEntity.name, "all", popupData.currentPage, popupData.searchQuery, successCallback, errorCallback);

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

		//#region << Logic >>
		popupData.instantAttachRecord = function (record) {
			var returnObject = {
				relationName: popupData.relation.name,
				dataKind: popupData.dataKind,
				selectedRecordId: record.id,
				operation: "attach"
			}
			popupData.processInstantSelection(returnObject);
			$modalInstance.close();
		};

		popupData.cancel = function () {
			$modalInstance.dismiss('cancel');
		};

		popupData.processInstantSelection = contentData.processInstantSelection;

		popupData.isSelected = function (record) {
			for (var j = 0; j < popupData.listData.length; j++) {
				if (popupData.listData[j].id == record.id) {
					return true;
				}
			}
			return false;
		}
		//#endregion

		$log.debug('webvellaAdmin>recursive-list>RLAddExistingModalController> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	};


	RLManageRelatedRecordModalController.$inject = ['contentData', '$modalInstance', '$log', '$q', '$stateParams', '$scope', '$location',
        'ngToast', '$timeout', '$state', 'webvellaAreasService', 'webvellaAdminService', 'resolvedManagedRecordQuickCreateView'];
	function RLManageRelatedRecordModalController(contentData, $modalInstance, $log, $q, $stateParams, $scope, $location,
        ngToast, $timeout, $state, webvellaAreasService, webvellaAdminService, resolvedManagedRecordQuickCreateView) {
		$log.debug('webvellaAdmin>recursive-list>RLManageRelatedRecordModalController> START controller.exec ' + moment().format('HH:mm:ss SSSS'));

		//#region << Init >>
		/* jshint validthis:true */
		var popupData = this;
		popupData.isFromRelation = true;
		if (!contentData.relation) {
			popupData.isFromRelation = false;
		}
		else {
			popupData.relation = fastCopy(contentData.relation);
			popupData.dataKind = fastCopy(contentData.dataKind);
		}
		popupData.entity = fastCopy(contentData.entity);
		if (resolvedManagedRecordQuickCreateView.data == null) {
			popupData.isEdit = false;
			popupData.recordData = {};
		}
		else {
			popupData.recordData = fastCopy(resolvedManagedRecordQuickCreateView.data)[0];
			popupData.isEdit = true;
		}

		popupData.viewMeta = fastCopy(resolvedManagedRecordQuickCreateView.meta);
		popupData.contentRegion = {};
		for (var j = 0; j < popupData.viewMeta.regions.length; j++) {
			if (popupData.viewMeta.regions[j].name === "content") {
				popupData.contentRegion = popupData.viewMeta.regions[j];
			}
		}

		//#endregion

		//#region << Fields init >>
		popupData.files = {}; // this is the data wrapper for the temporary upload objects that will be used in the html and for which we will generate watches below
		popupData.progress = {}; //Needed for file and image uploads
		var availableViewFields = [];
		//Init default values of fields
		if (popupData.contentRegion != null) {
			availableViewFields = webvellaAdminService.getItemsFromRegion(popupData.contentRegion);
			for (var j = 0; j < availableViewFields.length; j++) {
				if (!popupData.recordData[availableViewFields[j].meta.name] && availableViewFields[j].type === "field") {
					switch (availableViewFields[j].meta.fieldType) {

						case 2: //Checkbox
							popupData.recordData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							break;

						case 3: //Currency
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								popupData.recordData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 4: //Date
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								if (availableViewFields[j].meta.useCurrentTimeAsDefaultValue) {
									popupData.recordData[availableViewFields[j].meta.name] = moment().toISOString();
								}
								else if (availableViewFields[j].meta.defaultValue) {
									popupData.recordData[availableViewFields[j].meta.name] = moment(availableViewFields[j].meta.defaultValue).toISOString();
								}
							}
							break;
						case 5: //Date
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								if (availableViewFields[j].meta.useCurrentTimeAsDefaultValue) {
									popupData.recordData[availableViewFields[j].meta.name] = moment().toISOString();
								}
								else if (availableViewFields[j].meta.defaultValue) {
									popupData.recordData[availableViewFields[j].meta.name] = moment(availableViewFields[j].meta.defaultValue).toISOString();
								}
							}
							break;
						case 6: //Email
							break;
						case 7: //File
							popupData.progress[availableViewFields[j].meta.name] = 0;
							if (availableViewFields[j].meta.required) {
								popupData.recordData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 8: //HTML
							if (availableViewFields[j].meta.required) {
								popupData.recordData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 9: //Image
							popupData.progress[availableViewFields[j].meta.name] = 0;
							if (availableViewFields[j].meta.required) {
								popupData.recordData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 10: //TextArea
							if (availableViewFields[j].meta.required) {
								popupData.recordData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 11: //Multiselect
							if (availableViewFields[j].meta.required) {
								popupData.recordData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 12: //Number
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								popupData.recordData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 13: //Password
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								//Does not have default value
								//popupData.recordData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
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
								//popupData.recordData[availableViewFields[j].meta.name] = resultPercentage;
							}
							break;
						case 15: //Phone
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								popupData.recordData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 16: //Guid
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								popupData.recordData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 17: //Dropdown
							if (availableViewFields[j].meta.required && availableViewFields[j].meta.defaultValue) {
								popupData.recordData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 18: //Text
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								popupData.recordData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
						case 19: //URL
							if (availableViewFields[j].meta.required || (!availableViewFields[j].meta.required && !availableViewFields[j].meta.placeholderText)) {
								popupData.recordData[availableViewFields[j].meta.name] = fastCopy(availableViewFields[j].meta.defaultValue);
							}
							break;
					}


				}
			}
		}

		//#region << File >>
		popupData.uploadedFileName = "";
		popupData.upload = function (file, item) {
			if (file != null) {
				popupData.uploadedFileName = item.dataName;
				popupData.moveSuccessCallback = function (response) {
					$timeout(function () {
						popupData.recordData[popupData.uploadedFileName] = response.object.url;
					}, 1);
				}

				popupData.uploadSuccessCallback = function (response) {
					var tempPath = response.object.url;
					var fileName = response.object.filename;
					var targetPath = "/fs/" + item.fieldId + "/" + fileName;
					var overwrite = true;
					webvellaAdminService.moveFileFromTempToFS(tempPath, targetPath, overwrite, popupData.moveSuccessCallback, popupData.uploadErrorCallback);
				}
				popupData.uploadErrorCallback = function (response) {
					alert(response.message);
				}
				popupData.uploadProgressCallback = function (response) {
					$timeout(function () {
						popupData.progress[popupData.uploadedFileName] = parseInt(100.0 * response.loaded / response.total);
					}, 1);
				}
				webvellaAdminService.uploadFileToTemp(file, item.meta.name, popupData.uploadProgressCallback, popupData.uploadSuccessCallback, popupData.uploadErrorCallback);
			}
		};

		popupData.deleteFileUpload = function (item) {
			var fieldName = item.dataName;
			var filePath = popupData.recordData[fieldName];

			function deleteSuccessCallback(response) {
				$timeout(function () {
					popupData.recordData[fieldName] = null;
					popupData.progress[fieldName] = 0;
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
		//#endregion

		//#region << Html >>
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

		popupData.toggleSectionCollapse = function (section) {
			section.collapsed = !section.collapsed;
		}

		popupData.calendars = {};
		popupData.openCalendar = function (event, name) {
			popupData.calendars[name] = true;
		}

		//#endregion


		//#endregion

		//#region << Logic >>
		popupData.ok = function () {
			if (!popupData.isEdit) {
				webvellaAdminService.createRecord(popupData.entity.name, popupData.recordData, createSuccessCallback, manageErrorCallback);
			}
			else {
				webvellaAdminService.updateRecord(popupData.recordData.id, popupData.entity.name, popupData.recordData, successCallback, manageErrorCallback);
			}
		}

		/// Aux
		function createSuccessCallback(response) {
			if (!popupData.isFromRelation) {
				$state.reload();
				$modalInstance.close('success');
			}
			else {
				var returnObject = {
					relationName: popupData.relation.name,
					dataKind: popupData.dataKind,
					selectedRecordId: response.object.data[0].id,
					operation: "attach"
				}
				popupData.processInstantSelection = contentData.processInstantSelection;
				popupData.processInstantSelection(returnObject);
				$modalInstance.close('success');
			}
		}

		function successCallback(response) {
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success:</span> ' + 'The record was successfully updated'
			});
			$state.reload();
			$modalInstance.close('success');
		}

		function manageErrorCallback(response) {
			var location = $location;
			//Process the response and generate the validation Messages
			webvellaRootService.generateValidationMessages(response, popupData, popupData.recordData, location);
		}

		popupData.cancel = function () {
			$modalInstance.dismiss('cancel');
		};
		//#endregion

		$log.debug('webvellaAdmin>recursive-list>RLManageRelatedRecordModalController> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	};

	//#endregion


})();
