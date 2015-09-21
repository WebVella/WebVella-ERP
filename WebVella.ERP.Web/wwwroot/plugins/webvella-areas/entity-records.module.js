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
		.controller('SetFiltersModalController', SetFiltersModalController);



	// Configuration ///////////////////////////////////
	config.$inject = ['$stateProvider'];

	/* @ngInject */
	function config($stateProvider) {
		$stateProvider.state('webvella-entity-records', {
			parent: 'webvella-areas-base',
			url: '/:areaName/:entityName/:listName/:filter/:page?search',
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
		$log.debug('webvellaAreas>entities> BEGIN module.run ' + moment().format('HH:mm:ss SSSS'));

		$log.debug('webvellaAreas>entities> END module.run ' + moment().format('HH:mm:ss SSSS'));
	};

	//#endregion

	//#region << Resolve Function >>
	resolveListRecords.$inject = ['$q', '$log', 'webvellaAreasService', '$state', '$stateParams'];
	/* @ngInject */
	function resolveListRecords($q, $log, webvellaAreasService, $state, $stateParams) {
		$log.debug('webvellaDesktop>browse> BEGIN state.resolved ' + moment().format('HH:mm:ss SSSS'));
		// Initialize
		var defer = $q.defer();
		var listRecordsObject = {};
		listRecordsObject.filterRecords = {};

		// Process
		function filterSuccessCallback(response) {
			listRecordsObject.filterRecords = response.object;
			//TODO: Maintain and apply list filters
			//get and check filter records and list meta
			//if in the list there are removed fields or now unsearchable fields, which fields are part from the current filter, we need to update the current filter
			//and make new data request.

			defer.resolve(listRecordsObject);
		}


		// Process
		function successCallback(response) {
			listRecordsObject = response.object;
			webvellaAreasService.getListFilter($stateParams.filter, filterSuccessCallback, errorCallback);
		}

		function errorCallback(response) {
			defer.resolve(response.object);
		}

		if (!$stateParams.search) {
			$stateParams.search = null;
		}

		webvellaAreasService.getListRecords($stateParams.listName, $stateParams.entityName, $stateParams.filter, $stateParams.page, $stateParams.search, successCallback, errorCallback);

		// Return
		$log.debug('webvellaDesktop>browse> END state.resolved ' + moment().format('HH:mm:ss SSSS'));
		return defer.promise;
	}

	resolveCurrentEntityMeta.$inject = ['$q', '$log', 'webvellaAdminService', '$state', '$stateParams'];
	/* @ngInject */
	function resolveCurrentEntityMeta($q, $log, webvellaAdminService, $state, $stateParams) {
		$log.debug('webvellaDesktop>resolveCurrentEntityMeta> BEGIN state.resolved ' + moment().format('HH:mm:ss SSSS'));
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
		$log.debug('webvellaDesktop>resolveCurrentEntityMeta> END state.resolved ' + moment().format('HH:mm:ss SSSS'));
		return defer.promise;
	}


	resolveCurrentArea.$inject = ['$q', '$log', 'webvellaAdminService', '$state', '$stateParams'];
	/* @ngInject */
	function resolveCurrentArea($q, $log, webvellaAdminService, $state, $stateParams) {
		$log.debug('webvellaDesktop>resolveCurrentEntityMeta> BEGIN state.resolved ' + moment().format('HH:mm:ss SSSS'));
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
		$log.debug('webvellaDesktop>resolveCurrentEntityMeta> END state.resolved ' + moment().format('HH:mm:ss SSSS'));
		return defer.promise;
	}

	resolveEntityRelationsList.$inject = ['$q', '$log', 'webvellaAdminService', '$stateParams', '$state', '$timeout'];
	/* @ngInject */
	function resolveEntityRelationsList($q, $log, webvellaAdminService, $stateParams, $state, $timeout) {
		$log.debug('webvellaAdmin>entity-details> BEGIN state.resolved ' + moment().format('HH:mm:ss SSSS'));
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
		$log.debug('webvellaAdmin>entity-details> END state.resolved ' + moment().format('HH:mm:ss SSSS'));
		return defer.promise;
	}

	//#endregion


	// Controller ///////////////////////////////
	controller.$inject = ['$filter', '$log', '$modal', '$rootScope', '$state', '$stateParams', 'pageTitle', 'webvellaRootService',
        'resolvedSitemap', '$timeout', 'webvellaAreasService', 'resolvedListRecords', 'resolvedCurrentEntityMeta', 'resolvedCurrentArea',
		'resolvedEntityRelationsList', 'resolvedCurrentUser'];

	/* @ngInject */
	function controller($filter, $log, $modal, $rootScope, $state, $stateParams, pageTitle, webvellaRootService,
        resolvedSitemap, $timeout, webvellaAreasService, resolvedListRecords, resolvedCurrentEntityMeta, resolvedCurrentArea,
		resolvedEntityRelationsList, resolvedCurrentUser) {
		$log.debug('webvellaAreas>entities> BEGIN controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var contentData = this;
		contentData.records = angular.copy(resolvedListRecords.data);
		contentData.recordsMeta = angular.copy(resolvedListRecords.meta);
		contentData.relationsMeta = resolvedEntityRelationsList;
		//#region << Set Environment >>
		contentData.pageTitle = "Area Entities | " + pageTitle;
		webvellaRootService.setPageTitle(contentData.pageTitle);
		contentData.currentArea = webvellaAreasService.getCurrentAreaFromSitemap($stateParams.areaName, resolvedSitemap.data);
		contentData.stateParams = $stateParams;
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

		//#region << Init filters >>
		contentData.filterChangeRequested = false;
		contentData.filterRecords = angular.copy(resolvedListRecords.filterRecords);
		if (contentData.filterRecords == null || contentData.filterRecords.data == null) {
			contentData.filterRecords = {};
			contentData.filterRecords.data = [];
		}
		else {
			for (var i = 0; i < contentData.filterRecords.data.length; i++) {
				contentData.filterRecords.data[i].helper = angular.fromJson(contentData.filterRecords.data[i].helper);
				var helperDataAray = contentData.filterRecords.data[i].helper.data;
				for (var k = 0; k < helperDataAray.length; k++) {
					helperDataAray[k].value = decodeURIComponent(helperDataAray[k].value);
				}
			}
		}
		//#endregion

		//#region << Search >>
		contentData.defaultSearchField = null;
		for (var k = 0; k < contentData.currentListView.columns.length; k++) {
			if (contentData.currentListView.columns[k].type == "field") {
				contentData.defaultSearchField = contentData.currentListView.columns[k];
				break;
			}
		}
		if (contentData.defaultSearchField != null) {
			contentData.searchQueryPlaceholder = "" + contentData.defaultSearchField.meta.label;
		}


		contentData.searchQuery = null;
		if ($stateParams.search) {
			contentData.searchQuery = $stateParams.search;
		}
		contentData.checkForSearchEnter = function (e) {
			var code = (e.keyCode ? e.keyCode : e.which);
			if (code == 13) { //Enter keycode
				contentData.submitSearchQuery();
			}
		}
		contentData.submitSearchQuery = function () {
			$timeout(function () {
				$state.go("webvella-entity-records", { areaName: $stateParams.areaName, entityName: $stateParams.entityName, listName: $stateParams.listName, filter: $stateParams.filter, search: contentData.searchQuery }, { reload: true });
			}, 1);

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

		contentData.currentUser = angular.copy(resolvedCurrentUser).data[0];

		contentData.currentUserRoles = angular.copy(resolvedCurrentUser).data[0].$user_role;

		contentData.currentUserHasReadPermission = function (column) {
			var result = false;
			if (!column.meta.enableSecurity || column.meta.permissions == null) {
				return true;
			}
			for (var i = 0; i < contentData.currentUserRoles.length; i++) {
				for (var k = 0; k < column.meta.permissions.canRead.length; k++) {
					if (column.meta.permissions.canRead[k] == contentData.currentUserRoles[i].id) {
						result = true;
					}
				}
			}
			return result;
		}

		contentData.requestFilterChange = function () {
			contentData.filterChangeRequested = true;
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

		//filter modal
		contentData.openSetFiltersModal = function () {
			var modalInstance = $modal.open({
				animation: false,
				templateUrl: 'setFiltersModalContent.html',
				controller: 'SetFiltersModalController',
				controllerAs: "popupData",
				size: "lg",
				resolve: {
					contentData: function () {
						return contentData;
					},
					currentFilterRecords: function () {
						//TODO: apply getting the current filter records
						return null;
					}
				}
			});

		}
		//#endregion

		$log.debug('webvellaAreas>entities> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	}

	//// Modal Controllers
	SetFiltersModalController.$inject = ['$modalInstance', '$log', 'webvellaAreasService', 'ngToast', '$timeout', '$state', '$location', 'contentData'];
	/* @ngInject */
	function SetFiltersModalController($modalInstance, $log, webvellaAreasService, ngToast, $timeout, $state, $location, contentData) {
		$log.debug('webvellaAreas>records>SetFiltersModalController> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
		var popupData = this;
		popupData.contentData = angular.copy(contentData);
		popupData.currentUserRoles = angular.copy(contentData.currentUserRoles);
		popupData.currentUser = angular.copy(contentData.currentUser);
		//#region << generate searchable fields list
		//1. Get the list meta and find who are the searchable fields
		popupData.filterColumns = [];
		for (var m = 0; m < popupData.contentData.currentListView.columns.length; m++) {
			//is this field visible for the currentUser
			var userHasReadPermissionForField = false;
			for (var r = 0; r < popupData.currentUserRoles.length; r++) {
				for (var p = 0; p < popupData.contentData.currentListView.columns[m].meta.permissions.canRead.length; p++) {
					if (popupData.currentUserRoles[r].id == popupData.contentData.currentListView.columns[m].meta.permissions.canRead[p]) {
						userHasReadPermissionForField = true;
						break;
					}
				}
			}

			switch (popupData.contentData.currentListView.columns[m].type) {
				case "field":
					if (popupData.contentData.currentListView.columns[m].meta.searchable && (!popupData.contentData.currentListView.columns[m].enableSecurity || (popupData.contentData.currentListView.columns[m].enableSecurity && userHasReadPermissionForField))) {
						var filterObject = {};
						filterObject = popupData.contentData.currentListView.columns[m];
						filterObject.data = [];
						filterObject.loading = false;
						popupData.filterColumns.push(filterObject);
					}
					break;
				default:
					break;
			}
		}

		// Rules:
		// Simple fields -> depending on the field type
		// 1:1 (field is target) -> lookuplist
		// 1:1 (field is origin) -> textbox
		// 1:N (field is target) -> lookuplist
		// 1:N (field is origin) -> textbox
		// N:N for each of the relations we should generate a tab as it is a different field -> multiselect list

		//The helper object that helps show the filters to the user should include
		//Applied filter field name and field Id. For the selected values - record id(could be the selected option key) value
		//If applied filters are for related fields - related entity name, entity id, field name, field id, record id, value

		//If the showed on screen (not popup filters are changed) the pupup button should become an apply button

		popupData.tabLoading = false;
		popupData.tabSelected = function () {
			popupData.tabLoading = true;
			$timeout(function () {
				popupData.tabLoading = false;
			}, 500);
		}
		popupData.getTabExtraCssClass = function (column) {
			if (column.data.length > 0) {
				return "active-filter";
			}
			return "";
		}

		popupData.fieldTypes = [
					{
						"id": 1,
						"name": "AutoNumberField",
						"label": "Auto increment number",
						"description": "If you need a automatically incremented number with each new record, this is the field you need. You can customize the display format also."
					},
					{
						"id": 2,
						"name": "CheckboxField",
						"label": "Checkbox",
						"description": "The simple on and off switch. This field allows you to get a True (checked) or False (unchecked) value."
					},
					{
						"id": 3,
						"name": "CurrencyField",
						"label": "Currency",
						"description": "A currency amount can be entered and will be represented in a suitable formatted way"
					},
					{
						"id": 4,
						"name": "DateField",
						"label": "Date",
						"description": "A data pickup field, that can be later converting according to a provided pattern"
					},
					{
						"id": 5,
						"name": "DateTimeField",
						"label": "Date & Time",
						"description": "A date and time can be picked up and later presented according to a provided pattern"
					},
					{
						"id": 6,
						"name": "EmailField",
						"label": "Email",
						"description": "An email can be entered by the user, which will be validated and presented accordingly"
					},
					{
						"id": 7,
						"name": "FileField",
						"label": "File",
						"description": "File upload field. Files will be stored within the system."
					},
					{
						"id": 8,
						"name": "HtmlField",
						"label": "HTML",
						"description": "Provides the ability of entering and presenting an HTML code. Supports multiple input languages."
					},
					{
						"id": 9,
						"name": "ImageField",
						"label": "Image",
						"description": "Image upload field. Images will be stored within the system"
					},
					{
						"id": 10,
						"name": "MultiLineTextField",
						"label": "Textarea",
						"description": "A textarea for plain text input. Will be cleaned and stripped from any web unsafe content"
					},
					{
						"id": 11,
						"name": "MultiSelectField",
						"label": "Multiple select",
						"description": "Multiple values can be selected from a provided list"
					},
					{
						"id": 12,
						"name": "NumberField",
						"label": "Number",
						"description": "Only numbers are allowed. Leading zeros will be stripped."
					},
					{
						"id": 13,
						"name": "PasswordField",
						"label": "Password",
						"description": "This field is suitable for submitting passwords or other data that needs to stay encrypted in the database"
					},
					{
						"id": 14,
						"name": "PercentField",
						"label": "Percent",
						"description": "This will automatically present any number you enter as a percent value"
					},
					{
						"id": 15,
						"name": "PhoneField",
						"label": "Phone",
						"description": "Will allow only valid phone numbers to be submitted"
					},
					{
						"id": 16,
						"name": "GuidField",
						"label": "Identifier GUID",
						"description": "Very important field for any entity to entity relation and required by it"
					},
					{
						"id": 17,
						"name": "SelectField",
						"label": "Dropdown",
						"description": "One value can be selected from a provided list"
					},
					{
						"id": 18,
						"name": "TextField",
						"label": "Text",
						"description": "A simple text field. One of the most needed field nevertheless"
					},
					{
						"id": 19,
						"name": "UrlField",
						"label": "URL",
						"description": "This field will validate local and static URLs. Will present them accordingly"
					}
		];

		popupData.getFieldTypeName = function (column) {
			var fieldLabel = " ";
			for (var l = 0; l < popupData.fieldTypes.length; l++) {
				if (popupData.fieldTypes[l].id == column.meta.fieldType) {
					fieldLabel = popupData.fieldTypes[l].label;
					break;
				}
			}
			return fieldLabel;
		}

		popupData.clearFilter = function (column) {
			column.data = [];
		}

		//#endregion
		popupData.filterId = null;

		popupData.ok = function () {
			//Create the new filter (update currently is not developed as the filter should act as query params search - new options change the link
			//1. Generate the filter array
			var filterArray = [];
			popupData.filterId = moment().format('YYYYMMDDHHmmssSSS');
			for (var j = 0; j < popupData.filterColumns.length; j++) {
				if (popupData.filterColumns[j].data != null && popupData.filterColumns[j].data.length > 0) {
					var filterRecord = {};
					filterRecord.filter_id = popupData.filterId;
					filterRecord.field_name = popupData.filterColumns[j].meta.name;
					filterRecord.entity_name = $state.params.entityName;
					filterRecord.list_name = $state.params.listName;
					filterRecord.values = [];
					//Generate relation name
					switch (popupData.filterColumns[j].type) {
						default:
							//field
							filterRecord.relation_name = null;
							break;
					}

					//Generate helper 
					var helperObject = {};
					helperObject.label = popupData.filterColumns[j].meta.label;
					helperObject.name = popupData.filterColumns[j].meta.name;
					helperObject.type = popupData.filterColumns[j].type;
					helperObject.fieldType = popupData.filterColumns[j].meta.fieldType;
					helperObject.data = [];
					for (var m = 0; m < popupData.filterColumns[j].data.length; m++) {
						var valueRecord = {};
						switch (popupData.filterColumns[j].meta.fieldType) {
							default:
								//Single value options - AutoIncrement,
								valueRecord.value.push(encodeURIComponent(angular.copy(popupData.filterColumns[j].data[m])));
								valueRecord.label.push(valueRecord.value);
								break;
						}
						helperObject.data.push(valueRecord);
					}
					filterRecord.helper = angular.toJson(helperObject);

					//Generate values
					for (var k = 0; k < popupData.filterColumns[j].data.length; k++) {
						filterRecord.values.push(encodeURIComponent(angular.copy(popupData.filterColumns[j].data[k])))
					}
					filterRecord.id = null;
					filterRecord.created_by = popupData.currentUser.id;
					filterRecord.last_modified_by = popupData.currentUser.id;
					filterRecord.values = angular.toJson(filterRecord.values);
					filterArray.push(filterRecord);
				}
			}
			//2. Store the array 
			function successCallback(response) {
				$timeout(function () {
					$state.go("webvella-entity-records", { areaName: $state.params.areaName, entityName: $state.params.entityName, listName: $state.params.listName, filter: popupData.filterId, page: 1, search: $state.params.search}, { reload: true });
					$modalInstance.close('success');
					ngToast.create({
						className: 'success',
						content: '<span class="go-green">Success:</span> ' + 'New filters applied'
					});
				}, 0);
			}

			function errorCallback(response) {
				ngToast.create({
					className: 'error',
					content: '<span class="go-red">Error:</span> ' + response.message
				});
				$modalInstance.close('success');
			}
			webvellaAreasService.createListFilter(filterArray, $state.params.entityName, $state.params.listName, successCallback, errorCallback);

			//3. Redirect to the new link
		};

		popupData.cancel = function () {
			$modalInstance.dismiss('cancel');
		};

		$log.debug('webvellaAreas>records>SetFiltersModalController> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	}


})();
