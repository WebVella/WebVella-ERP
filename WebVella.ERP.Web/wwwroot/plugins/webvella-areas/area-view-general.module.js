/* entities.module.js */

/**
* @desc this module manages the application home desktop screen
*/

(function () {
	'use strict';

	angular
		.module('webvellaAreas') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
		.config(config)
		.controller('WebVellaAreaViewGeneralController', controller)
		.controller('ManageRelationFieldModalController', ManageRelationFieldModalController)
		.controller('SelectTreeNodesModalController', SelectTreeNodesModalController);

	// Configuration ///////////////////////////////////
	config.$inject = ['$stateProvider'];


	function config($stateProvider) {
		$stateProvider
		//general view in an area with view sidebar
		.state('webvella-areas-view-general', {
			parent: 'webvella-area-base',
			url: '/view-general/sb/:viewName/:recordId/:regionName?returnUrl',
			params: {
				regionName: { value: "header", squash: true }
			},
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
					controller: 'WebVellaAreaViewGeneralController',
					templateUrl: '/plugins/webvella-areas/area-view-general.view.html',
					controllerAs: 'ngCtrl'
				}
			},
			resolve: {
				loadDependency: loadDependency,
				resolvedParentViewData: function () { return null; },
				resolvedCurrentViewData: resolveCurrentViewData
			}
		})
		//general view in an area with the area sidebar
		.state('webvella-areas-view-general-no-sidebar', {
			parent: 'webvella-area-base',
			url: '/view-general/nsb/:viewName/:recordId/:regionName',
			params: {
				regionName: { value: "header", squash: true }
			},
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
					controller: 'WebVellaAreaViewGeneralController',
					templateUrl: '/plugins/webvella-areas/area-view-general.view.html',
					controllerAs: 'ngCtrl'
				}
			},
			resolve: {
				loadDependency: loadDependency,
				resolvedParentViewData: function () { return null; },
				resolvedCurrentViewData: resolveCurrentViewData
			}
		})
		.state('webvella-areas-view-general-in-view', {
			parent: 'webvella-area-base',
			url: '/view-general/sb/:parentViewName/:recordId/view-general/:viewName/:regionName',
			params: {
				regionName: { value: "header", squash: true }
			},
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
					controller: 'WebVellaAreaViewGeneralController',
					templateUrl: '/plugins/webvella-areas/area-view-general.view.html',
					controllerAs: 'ngCtrl'
				}
			},
			resolve: {
				loadDependency: loadDependency,
				resolvedParentViewData: resolveParentViewData,
				resolvedCurrentViewData: resolveCurrentViewData
			}
		});
	}

	//#region << Resolve Function >> /////////////////////////
	resolveParentViewData.$inject = ['$q', '$log', 'webvellaCoreService', '$stateParams', '$state', '$timeout', 'resolvedCurrentEntityMeta', 'resolvedEntityList'];
	function resolveParentViewData($q, $log, webvellaCoreService, $stateParams, $state, $timeout, resolvedCurrentEntityMeta, resolvedEntityList) {

		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			if (response.object === null) {
				alert("error in response! " + response.message);
			}
			else if (response.object.meta === null) {
				alert("The view with name: " + $stateParams.parentViewName + " does not exist");
			} else {
				defer.resolve(response.object);
			}
		}

		function errorCallback(response) {
			if (response.object === null) {
				alert("error in response! " + response.message);
			}
			else {
				defer.reject(response.message);
			}
		}

		var userHasReadEntityPermission = webvellaCoreService.userHasRecordPermissions(resolvedCurrentEntityMeta, "canRead");
		if (!userHasReadEntityPermission) {
			alert("you do not have permissions to view records from this entity!");
			defer.reject("you do not have permissions to view records from this entity");
		}

		var parentView = webvellaCoreService.getEntityRecordViewFromEntitiesMetaList($stateParams.parentViewName, $stateParams.entityName, resolvedEntityList);

		webvellaCoreService.getRecordByViewMeta($stateParams.recordId, parentView, $stateParams.entityName, null, successCallback, errorCallback);

		return defer.promise;
	}

	resolveCurrentViewData.$inject = ['$q', '$log', 'webvellaCoreService', '$stateParams', '$state', '$timeout', 'resolvedCurrentEntityMeta', 'resolvedParentViewData', 'resolvedEntityList', 'resolvedEntityRelationsList'];

	function resolveCurrentViewData($q, $log, webvellaCoreService, $stateParams, $state, $timeout, resolvedCurrentEntityMeta, resolvedParentViewData, resolvedEntityList, resolvedEntityRelationsList) {

		// Initialize
		var defer = $q.defer();
		var userHasReadEntityPermission = webvellaCoreService.userHasRecordPermissions(resolvedCurrentEntityMeta, "canRead");
		if (!userHasReadEntityPermission) {
			alert("you do not have permissions to view records from this entity!");
			defer.reject("you do not have permissions to view records from this entity");
		}

		var safeViewNameAndEntity = webvellaCoreService.getSafeViewNameAndEntityName($stateParams.viewName, $stateParams.entityName, resolvedEntityRelationsList);
		var getViewMeta = webvellaCoreService.getEntityRecordViewFromEntitiesMetaList(safeViewNameAndEntity.viewName, safeViewNameAndEntity.entityName, resolvedEntityList);
		if (getViewMeta.dataSourceUrl != null && getViewMeta.dataSourceUrl != "") {
			//This view has a dynamicSourceUrl defined
			webvellaCoreService.getRecordByViewMeta($stateParams.recordId, getViewMeta, safeViewNameAndEntity.entityName, $stateParams, successCallback, errorCallback);
		}
		else {
			//No dynamicSourceUrl defined
			var parentView = {};
			if (resolvedParentViewData === null) {

				webvellaCoreService.getRecordByViewMeta($stateParams.recordId, getViewMeta, $stateParams.entityName, null, successCallback, errorCallback);
			}
			else {
				parentView.data = fastCopy(resolvedParentViewData);
				parentView.meta = webvellaCoreService.getEntityRecordViewFromEntitiesMetaList($stateParams.parentViewName, $stateParams.entityName, resolvedEntityList);
				var currentViewData = {};
				if ($stateParams.viewName.startsWith("$view$")) {
					//View from the same entity
					parentView.meta.sidebar.items.forEach(function (sidebarItem) {
						if (sidebarItem.dataName === $stateParams.viewName) {
							currentViewData = parentView.data[0][$stateParams.viewName];
						}
					});
					defer.resolve(currentViewData);
				}
				else {

				}

			}
		}

		// Process
		function successCallback(response) {
			if (response.object === null) {
				alert("error in response! " + response.message);
			}
			else if (response.object.meta === null) {
				alert("The view with name: " + $stateParams.viewName + " does not exist");
			} else {
				defer.resolve(response.object);
			}
		}

		function errorCallback(response) {
			if (response.object === null) {
				alert("error in response! " + response.message);
			}
			else {
				defer.reject(response.message);
			}
		}

		return defer.promise;
	}

	loadDependency.$inject = ['$ocLazyLoad', '$q', '$http', '$state', '$timeout', '$stateParams', 'wvAppConstants', 'resolvedCurrentEntityMeta', 'resolvedParentViewData', 'resolvedEntityRelationsList'];
	function loadDependency($ocLazyLoad, $q, $http, $state, $timeout, $stateParams, wvAppConstants, resolvedCurrentEntityMeta, resolvedParentViewData, resolvedEntityRelationsList) {
		var lazyDeferred = $q.defer();
		var listServiceJavascriptPath = "";
		if (resolvedParentViewData === null) {
			//Parent view is reviewed
			listServiceJavascriptPath = wvAppConstants.apiBaseUrl + 'meta/entity/' + $stateParams.entityName + '/view/' + $stateParams.viewName + '/service.js?v=' + resolvedCurrentEntityMeta.hash;
		}
		else {
			//A view in another view is reviewed
			var dataNameArray = fastCopy($stateParams.viewName).split('$');
			if (dataNameArray.length < 3 || dataNameArray.length > 4) {
				lazyDeferred.reject("The view dataName is not correct");
			}
			else if (dataNameArray.length === 3) {
				//it is view from the current entity  e.g. $view$second
				var realViewName = dataNameArray[2];
				listServiceJavascriptPath = wvAppConstants.apiBaseUrl + 'meta/entity/' + $stateParams.entityName + '/view/' + realViewName + '/service.js?v=' + resolvedCurrentEntityMeta.hash;
			}
			else {
				//e.g. "$view$project_1_n_ticket$general"
				//extract the real view name
				var viewRealName = dataNameArray[3];
				//find the other entity in the relation so we can include it in the request
				var viewRealEntity = null;
				resolvedEntityRelationsList.forEach(function (relation) {
					if (relation.name === dataNameArray[2]) {
						if (relation.originEntityName == $stateParams.entityName) {
							viewRealEntity = relation.targetEntityName;
						}
						else if (relation.targetEntityName == $stateParams.entityName) {
							viewRealEntity = relation.originEntityName;
						}
					}
				});
				if (viewRealEntity == null) {
					lazyDeferred.reject("Cannot find the list real entity name");
				}
				else {
					listServiceJavascriptPath = wvAppConstants.apiBaseUrl + 'meta/entity/' + viewRealEntity + '/view/' + viewRealName + '/service.js?v=' + moment().toISOString();	//do not have the hash of the real entity here
				}
			}
		}



		var loadFilesArray = [];
		loadFilesArray.push(listServiceJavascriptPath);

		return $ocLazyLoad.load({
			cache: false,
			files: loadFilesArray
		}).then(function () {
			return lazyDeferred.resolve("ready");
		},
		function error(err) {
			$timeout(function () {
				$state.go('webvella-core-error', { 'code': '404', 'url': "some-url-error" });
			}, 0);
			lazyDeferred.reject(err);
			//return err;
		});

	}

	//#endregion


	// Controller ///////////////////////////////

	controller.$inject = ['$filter','$injector', '$uibModal', '$log', '$q', '$rootScope', '$state', '$stateParams', '$scope', '$window', 'pageTitle', 'webvellaCoreService',
		'resolvedAreas', '$timeout', 'resolvedCurrentViewData', 'ngToast', 'wvAppConstants', 'resolvedEntityList', 'resolvedCurrentEntityMeta', 'resolvedEntityRelationsList', 'resolvedCurrentUser',
		'resolvedCurrentUserEntityPermissions', '$sessionStorage', 'resolvedParentViewData'];


	function controller($filter, $injector, $uibModal, $log, $q, $rootScope, $state, $stateParams, $scope, $window, pageTitle, webvellaCoreService,
		resolvedAreas, $timeout, resolvedCurrentViewData, ngToast, wvAppConstants, resolvedEntityList, resolvedCurrentEntityMeta, resolvedEntityRelationsList, resolvedCurrentUser,
		resolvedCurrentUserEntityPermissions, $sessionStorage, resolvedParentViewData) {

		//#region << ngCtrl initialization >>
		var ngCtrl = this;
		//#endregion

		//#region <<Set Page meta>>
		ngCtrl.pageTitle = "Area Entities | " + pageTitle;
		webvellaCoreService.setPageTitle(ngCtrl.pageTitle);
		ngCtrl.currentArea = webvellaCoreService.getCurrentAreaFromAreaList($stateParams.areaName, resolvedAreas.data);
		webvellaCoreService.setBodyColorClass(ngCtrl.currentArea.color);
		ngCtrl.currentState = $state.current;
		//#endregion

		var safeViewNameAndEntity = webvellaCoreService.getSafeViewNameAndEntityName($stateParams.viewName, $stateParams.entityName, resolvedEntityRelationsList);
		//#region << Initialize main objects >>
		ngCtrl.view = {};
		ngCtrl.view.meta = webvellaCoreService.getEntityRecordViewFromEntitiesMetaList(safeViewNameAndEntity.viewName, safeViewNameAndEntity.entityName, resolvedEntityList);
		ngCtrl.view.data = resolvedCurrentViewData;
		if (resolvedParentViewData == null) {
			ngCtrl.parentView = null;
		}
		else {
			ngCtrl.parentView = {};
			ngCtrl.parentView.meta = webvellaCoreService.getEntityRecordViewFromEntitiesMetaList($stateParams.parentViewName, $stateParams.entityName, resolvedEntityList);
			ngCtrl.parentView.data = resolvedParentViewData[0];
		}
		ngCtrl.entityList = resolvedEntityList;
		ngCtrl.entity = resolvedCurrentEntityMeta;
		ngCtrl.entityRelations = resolvedEntityRelationsList;
		ngCtrl.areas = resolvedAreas.data;
		ngCtrl.currentUser = resolvedCurrentUser;
		ngCtrl.$sessionStorage = $sessionStorage;
		ngCtrl.stateParams = $stateParams;
		//#endregion

		//#region << Init aux objects >>
		ngCtrl.currentUserEntityPermissions = fastCopy(resolvedCurrentUserEntityPermissions);
		//#endregion

		//#region << Initialize general views alternatives >>
		ngCtrl.generalViews = [];
		if ($stateParams.parentViewName == null) {
			ngCtrl.entity.recordViews.forEach(function (view) {
				if (view.type == "general") {
					ngCtrl.generalViews.push(view);
				}
			});
		}
		ngCtrl.generalViews.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });
		//#endregion

		//#region << Entity relations functions >>
		ngCtrl.getRelation = function (relationName) {
			for (var i = 0; i < ngCtrl.entityRelations.length; i++) {
				if (ngCtrl.entityRelations[i].name == relationName) {
					//set current entity role
					if (safeViewNameAndEntity.entityName == ngCtrl.entityRelations[i].targetEntityName && safeViewNameAndEntity.entityName == ngCtrl.entityRelations[i].originEntityName) {
						ngCtrl.entityRelations[i].currentEntityRole = 3; //both origin and target
					}
					else if (safeViewNameAndEntity.entityName == ngCtrl.entityRelations[i].targetEntityName && safeViewNameAndEntity.entityName != ngCtrl.entityRelations[i].originEntityName) {
						ngCtrl.entityRelations[i].currentEntityRole = 2; //target
					}
					else if (safeViewNameAndEntity.entityName != ngCtrl.entityRelations[i].targetEntityName && safeViewNameAndEntity.entityName == ngCtrl.entityRelations[i].originEntityName) {
						ngCtrl.entityRelations[i].currentEntityRole = 1; //origin
					}
					else if (safeViewNameAndEntity.entityName != ngCtrl.entityRelations[i].targetEntityName && safeViewNameAndEntity.entityName != ngCtrl.entityRelations[i].originEntityName) {
						ngCtrl.entityRelations[i].currentEntityRole = 0; //possible problem
					}
					return ngCtrl.entityRelations[i];
				}
			}
			return null;
		}
		//#endregion

		//#region << Permission checks >>
		ngCtrl.currentUserHasReadPermission = function (item) {
			var result = false;
			if (!item.meta.enableSecurity || item.meta.permissions == null) {
				return true;
			}
			for (var i = 0; i < ngCtrl.currentUser.roles.length; i++) {
				for (var k = 0; k < item.meta.permissions.canRead.length; k++) {
					if (item.meta.permissions.canRead[k] == ngCtrl.currentUser.roles[i]) {
						result = true;
					}
				}
			}
			return result;
		}

		ngCtrl.currentUserHasUpdatePermission = function (item) {
			var result = false;
			//Check first if the entity allows it
			var viewEntity = webvellaCoreService.getEntityMetaFromEntityList(safeViewNameAndEntity.entityName, resolvedEntityList);
			var userHasUpdateEntityPermission = webvellaCoreService.userHasRecordPermissions(viewEntity, "canUpdate");

			if (!userHasUpdateEntityPermission) {
				return false;
			}
			if (!item.meta.enableSecurity) {
				return true;
			}
			for (var i = 0; i < ngCtrl.currentUser.roles.length; i++) {
				for (var k = 0; k < item.meta.permissions.canUpdate.length; k++) {
					if (item.meta.permissions.canUpdate[k] == ngCtrl.currentUser.roles[i]) {
						result = true;
					}
				}
			}
			return result;
		}

		ngCtrl.userHasRecordDeletePermission = function () {
			var viewEntity = webvellaCoreService.getEntityMetaFromEntityList(safeViewNameAndEntity.entityName, resolvedEntityList);
			return webvellaCoreService.userHasRecordPermissions(viewEntity, "canDelete");
		}

		ngCtrl.userHasRecordPermissions = function (permissionsCsv) {
			var viewEntity = webvellaCoreService.getEntityMetaFromEntityList(safeViewNameAndEntity.entityName, resolvedEntityList);
			return webvellaCoreService.userHasRecordPermissions(viewEntity, permissionsCsv);
		}



		//The goal for this method is to check the permissions that this user has to change either the current record or the related record in the N:N case
		//The reason is:	when relation type 1:N or 1:1 only the target's record data is changed
		//					when relation type N:N the target's and the origin's record data is changed
		//User needs to have update permissions to change data
		ngCtrl.currentUserHasUpdatePermissionRelation = function (item) {
			var userHasUpdatePermission = false;
			var relation = ngCtrl.getRelation(item.relationName);
			var currentEntityId = ngCtrl.entity.id;
			var currentEntityIsRelationStatus = 1; // 1 - origin, 2- target, 3 - both
			if (relation.originEntityId == relation.targetEntityId && relation.originEntityId == currentEntityId) {
				currentEntityIsRelationStatus = 3;
			}
			else if (relation.targetEntityId == currentEntityId) {
				currentEntityIsRelationStatus = 2;
			}

			//Case 0: the current entity is both item and origin. 
			//		  User should have permission to update both fields in the current entity
			if (currentEntityIsRelationStatus == 3) {
				var originFieldMeta = null;
				var userCanUpdateOrigin = false;
				var targetFieldMeta = null;
				var userCanUpdateTarget = false;
				for (var i = 0; i < ngCtrl.entity.fields.length; i++) {
					if (ngCtrl.entity.fields[i].id == relation.originFieldId) {
						originFieldMeta = ngCtrl.entity.fields[i];
					}
					else if (ngCtrl.entity.fields[i].id == relation.targetFieldId) {
						targetFieldMeta = ngCtrl.entity.fields[i];
					}
				}
				//Check basic security
				if (!originFieldMeta.enableSecurity) {
					userCanUpdateOrigin = true;
				}

				if (!targetFieldMeta.enableSecurity) {
					userCanUpdateTarget = true;
				}

				for (var i = 0; i < ngCtrl.currentUser.roles.length; i++) {
					//Check if origin has this role
					if (!userCanUpdateOrigin) {
						for (var k = 0; k < originFieldMeta.permissions.canUpdate.length; k++) {
							if (originFieldMeta.permissions.canUpdate[k] == ngCtrl.currentUser.roles[i]) {
								userCanUpdateOrigin = true;
								break;
							}
						}
					}
					//Check if target has this role
					if (!userCanUpdateTarget) {
						for (var k = 0; k < targetFieldMeta.permissions.canUpdate.length; k++) {
							if (targetFieldMeta.permissions.canUpdate[k] == ngCtrl.currentUser.roles[i]) {
								userCanUpdateTarget = true;
								break;
							}
						}
					}
				}
				if (userCanUpdateOrigin && userCanUpdateTarget) {
					userHasUpdatePermission = true;
				}
				else {
					//we need to find the corresponding field from the current entity
					if (relation.originFieldId == item.meta.id) {
						//the field from the current entity is than target
						checkedFieldId = relation.targetFieldId;
					}
				}
			}
				//Case 1: (1:1 or 1:N) and the current entity is target
				//						the user should have permission to change the current Entity's field
			else if (currentEntityIsRelationStatus == 2 && (relation.relationType == 1 || relation.relationType == 2)) {
				var currentEntityFieldMeta = null;
				for (var i = 0; i < ngCtrl.entity.fields.length; i++) {
					if (ngCtrl.entity.fields[i].id == relation.targetFieldId) {
						currentEntityFieldMeta = ngCtrl.entity.fields[i];
					}
				}
				if (currentEntityFieldMeta != null) {
					if (!currentEntityFieldMeta.enableSecurity) {
						userHasUpdatePermission = true;
					}
					else {
						for (var i = 0; i < ngCtrl.currentUser.roles.length; i++) {
							for (var k = 0; k < currentEntityFieldMeta.permissions.canUpdate.length; k++) {
								if (currentEntityFieldMeta.permissions.canUpdate[k] == ngCtrl.currentUser.roles[i]) {
									userHasUpdatePermission = true;
									break;
								}
							}
						}
					}
				}
			}
				//Case 2: (1:1 or 1:N) and the current entity is origin
			else if (currentEntityIsRelationStatus == 1 && (relation.relationType == 1 || relation.relationType == 2)) {
				if (!item.meta.enableSecurity) {
					userHasUpdatePermission = true;
				}
				else {
					for (var i = 0; i < ngCtrl.currentUser.roles.length; i++) {
						for (var k = 0; k < item.meta.permissions.canUpdate.length; k++) {
							if (item.meta.permissions.canUpdate[k] == ngCtrl.currentUser.roles[i]) {
								userHasUpdatePermission = true;
								break;
							}
						}
					}
				}
			}
				//Case 3: (N:N) 	no matter if the current entity is origin or target
				//					user should have permission to update both fields in both entities 
			else if (relation.relationType == 3) {
				var originFieldMeta = null;
				var userCanUpdateOrigin = false;
				var targetFieldMeta = null;
				var userCanUpdateTarget = false;
				//get origin field meta
				if (currentEntityIsRelationStatus == 1) {
					for (var i = 0; i < ngCtrl.entity.fields.length; i++) {
						if (ngCtrl.entity.fields[i].id == relation.originFieldId) {
							originFieldMeta = ngCtrl.entity.fields[i];
						}
					}
					targetFieldMeta = item.meta;
				}
				else {
					originFieldMeta = item.meta;
					for (var i = 0; i < ngCtrl.entity.fields.length; i++) {
						if (ngCtrl.entity.fields[i].id == relation.targetFieldId) {
							targetFieldMeta = ngCtrl.entity.fields[i];
						}
					}
				}

				//Check basic security
				if (!originFieldMeta.enableSecurity) {
					userCanUpdateOrigin = true;
				}

				if (!targetFieldMeta.enableSecurity) {
					userCanUpdateTarget = true;
				}

				for (var i = 0; i < ngCtrl.currentUser.roles.length; i++) {
					//Check if origin has this role
					if (!userCanUpdateOrigin) {
						for (var k = 0; k < originFieldMeta.permissions.canUpdate.length; k++) {
							if (originFieldMeta.permissions.canUpdate[k] == ngCtrl.currentUser.roles[i]) {
								userCanUpdateOrigin = true;
								break;
							}
						}
					}
					//Check if target has this role
					if (!userCanUpdateTarget) {
						for (var k = 0; k < targetFieldMeta.permissions.canUpdate.length; k++) {
							if (targetFieldMeta.permissions.canUpdate[k] == ngCtrl.currentUser.roles[i]) {
								userCanUpdateTarget = true;
								break;
							}
						}
					}
				}
				if (userCanUpdateOrigin && userCanUpdateTarget) {
					userHasUpdatePermission = true;
				}
				else {
					userHasUpdatePermission = false;
				}
			}

			return userHasUpdatePermission;
		}

		ngCtrl.recursiveObjectCanDo = function (permissionName, relatedEntityName) {
			var currentEntityPermissions = {};
			var relatedEntityPermissions = {};
			for (var i = 0; i < ngCtrl.currentUserEntityPermissions.length; i++) {
				if (ngCtrl.currentUserEntityPermissions[i].entityName == ngCtrl.entity.name) {
					currentEntityPermissions = ngCtrl.currentUserEntityPermissions[i];
				}
				if (ngCtrl.currentUserEntityPermissions[i].entityName == relatedEntityName) {
					relatedEntityPermissions = ngCtrl.currentUserEntityPermissions[i];
				}
			}
			switch (permissionName) {
				case "can-add-existing":
					if (currentEntityPermissions.canUpdate) {
						return true;
					}
					else {
						return false;
					}
				case "can-create":
					if (currentEntityPermissions.canUpdate && relatedEntityPermissions.canCreate) {
						return true;
					}
					else {
						return false;
					}
				case "can-edit":
					if (relatedEntityPermissions.canUpdate) {
						return true;
					}
					else {
						return false;
					}
				case "can-remove":
					if (currentEntityPermissions.canUpdate) {
						return true;
					}
					else {
						return false;
					}
			}
		}

		//#endregion

		//#region << Render >>
		ngCtrl.convertToJSDate = function(date){
			return moment(fastCopy(date)).toDate();
		}

		ngCtrl.initInlineEdit = function(fieldType,item,$this,viewData){
			switch(fieldType){
				case 4:
					if(viewData[item.dataName] == null){
						viewData[item.dataName] = moment().toDate();
					}
					ngCtrl['fieldForm_' + item.dataName] = $this['fieldForm_' + item.dataName];
					$this['fieldForm_' + item.dataName].$show(); 
					break;
				case 5:
					if(viewData[item.dataName] == null){
						viewData[item.dataName] = moment().toDate();
					}
					ngCtrl['fieldForm_' + item.dataName] = $this['fieldForm_' + item.dataName];
					$this['fieldForm_' + item.dataName].$show(); 
					break;
			}
		}

		ngCtrl.headerRegion = [];
		ngCtrl.activeRegion = [];
		ngCtrl.activeTabName = ngCtrl.stateParams.regionName;
		ngCtrl.view.meta.regions.sort(sort_by({ name: 'weight', primer: parseInt, reverse: false }));
		if (ngCtrl.activeTabName == "header") {
			//Set the first tab as active
			if (ngCtrl.view.meta.regions[0].name != "header") {
				ngCtrl.activeTabName = ngCtrl.view.meta.regions[0].name;
			}
			else if (ngCtrl.view.meta.regions.length > 1) {
				ngCtrl.activeTabName = ngCtrl.view.meta.regions[1].name;
			}
			else {
				ngCtrl.activeTabName = null;
			}
		}
		ngCtrl.renderTabBar = false;
		ngCtrl.view.meta.regions.forEach(function (region) {
			if (region.render && region.name != "header") {
				ngCtrl.renderTabBar = true;
			}
			if (region.render && region.name == "header") {
				ngCtrl.headerRegion.push(region);
			}
			else if (region.name == ngCtrl.activeTabName) {
				ngCtrl.activeRegion.push(region);
			}
		});


		ngCtrl.renderFieldValue = webvellaCoreService.renderFieldValue;
		ngCtrl.getRelationLabel = function (item) {
			if (item.fieldLabel) {
				return item.fieldLabel
			}
			else {
				var relationName = item.relationName;
				var relation = findInArray(ngCtrl.entityRelations, "name", relationName);
				if (relation) {
					return relation.label;
				}
				else {
					return "";
				}
			}
		}
		//Date & DateTime 
		ngCtrl.getTimeString = function (item, data) {
			if (item && item.dataName && data) {
				var fieldValue = data;
				if (!fieldValue) {
					return "";
				} else {
					return $filter('date')(fieldValue, "HH:mm");
				}
			}
		}

		ngCtrl.initDate = function(date){
			if(date != null){
				return ngCtrl.convertToJSDate(date)
			}
			return null;
		}

		ngCtrl.showPageTitleAuxLabelSecondary = false;

		ngCtrl.generateHighlightString = function () {
			if(ngCtrl.parentView && ngCtrl.parentView.data){
				ngCtrl.showPageTitleAuxLabelSecondary = true;
				return webvellaCoreService.generateHighlightString(ngCtrl.view.meta,ngCtrl.parentView.data,ngCtrl.stateParams,"title");
			}
			else {
				return webvellaCoreService.generateHighlightString( ngCtrl.view.meta,ngCtrl.view.data[0],ngCtrl.stateParams,"title");
			}
		}

		ngCtrl.generateAuxHighlightString = function(){
			if (ngCtrl.parentView && ngCtrl.parentView.data) {
				return webvellaCoreService.generateHighlightString(ngCtrl.parentView.meta, ngCtrl.parentView.data, ngCtrl.stateParams, "title");
			}
			else {
				return webvellaCoreService.generateHighlightString(ngCtrl.view.meta, ngCtrl.view.data[0], ngCtrl.stateParams, "title");
			}
		}

		ngCtrl.generateSublistHighlightString = function (viewMeta) {
			if(ngCtrl.parentView && ngCtrl.parentView.data){
				ngCtrl.showPageTitleAuxLabelSecondary = true;
				return webvellaCoreService.generateHighlightString(viewMeta,ngCtrl.parentView.data,ngCtrl.stateParams,"title");
			}
			else {
				return webvellaCoreService.generateHighlightString(viewMeta,ngCtrl.view.data[0],ngCtrl.stateParams,"title");
			}
		}

		//ngCtrl.generateHighlightString = function () {
		//	if (ngCtrl.parentView && ngCtrl.parentView.data) {
		//		ngCtrl.showPageTitleAuxLabelSecondary = true;
		//		return webvellaCoreService.generateHighlightString(ngCtrl.parentView.meta, ngCtrl.parentView.data, ngCtrl.stateParams, "title");
		//	}
		//	else {
		//		return webvellaCoreService.generateHighlightString(ngCtrl.view.meta, ngCtrl.view.data[0], ngCtrl.stateParams, "title");
		//	}
		//}

		//ngCtrl.generateAuxHighlightString = function(){
		//	if(ngCtrl.parentView && ngCtrl.parentView.data){
		//		return webvellaCoreService.generateHighlightString(ngCtrl.view.meta,ngCtrl.parentView.data,ngCtrl.stateParams,"label");
		//	}
		//	else {
		//		return webvellaCoreService.generateHighlightString( ngCtrl.view.meta,null,ngCtrl.stateParams,"label");
		//	}
		//}
		//#endregion

		//#region << Logic >>

		//#region << Section collapse>>
		ngCtrl.toggleSectionCollapse = function (section) {
			section.collapsed = !section.collapsed;
		}
		//#endregion

		ngCtrl.goToRegion = function (regionName) {
			webvellaCoreService.GoToState(ngCtrl.currentState.name, { areaName: ngCtrl.stateParams.areaName, entityName: ngCtrl.stateParams.entityName, viewName: ngCtrl.stateParams.viewName, recordId: ngCtrl.stateParams.recordId, regionName: regionName })
		}


		//#region << Html box helpers >>

		//Init
		ngCtrl.htmlFieldEdit = function(item,viewData){
				var modalInstance = $uibModal.open({
					animation: false,
					templateUrl: 'manageHtmlFieldModal.html',
					controller: function($scope, $uibModalInstance,selectedItem,selectedViewData){
						$scope.popupCtrl.selectedValue = fastCopy(selectedViewData[selectedItem.dataName]);
						$scope.popupCtrl.selectedFieldName = fastCopy(selectedItem.fieldName);

		$scope.editorOptions = {
			filebrowserImageBrowseUrl: '/ckeditor/image-finder',
			filebrowserImageUploadUrl: '/ckeditor/image-upload-url',
			uploadUrl :'/ckeditor/drop-upload-url',
			language: GlobalLanguage,
			skin: 'moono-lisa',
			height: '160',
			contentsCss: '/plugins/webvella-core/css/editor.css',
			extraPlugins: "sourcedialog,colorbutton,colordialog,panel,font,uploadimage",
			allowedContent: true,
			colorButton_colors: '333333,FFFFFF,F44336,E91E63,9C27B0,673AB7,3F51B5,2196F3,03A9F4,00BCD4,009688,4CAF50,8BC34A,CDDC39,FFEB3B,FFC107,FF9800,FF5722,795548,607D8B,999999',
			colorButton_enableAutomatic: false,
			colorButton_enableMore: false,
			toolbarLocation: 'top',
			toolbar: 'full',
			toolbar_full: [
				{ name: 'basicstyles', items: ['Save', 'Bold', 'Italic', 'Strike', 'Underline'] },
				{ name: 'colors', items: ['TextColor', 'BGColor'] },
				{ name: 'styles', items: ['FontSize', 'RemoveFormat'] },
				{ name: 'editing', items: ['Format'] },
				{ name: 'links', items: ['Link', 'Unlink'] },
				{ name: 'pasting', items: ['PasteText', 'PasteFromWord'] },
				{ name: 'paragraph', items: ['BulletedList', 'NumberedList', 'Blockquote'] },
				{ name: 'insert', items: ['Image', 'Table', 'SpecialChar'] },
				{ name: 'tools', items: ['Sourcedialog', 'Maximize'] }, '/'
			]
		};

						//We need to get editor so later to unfocus and destroy it before closing the modal in order not to receive errors
						$scope.editor = {};
						CKEDITOR.on( 'instanceCreated', function( event ) {	$scope.editor = event.editor;});
						$scope.popupCtrl.isReady = false;
						$scope.$on("ckeditor.ready", function( event ) {$scope.popupCtrl.isReady = true;});

						function destroyEditor(){
								 try {
									CKEDITOR.instances[$scope.editor.name].focusManager.blur( true );
									 CKEDITOR.instances[$scope.editor.name].destroy(false);
								 } catch (e) {
									console.log(e);
								 }
							}
						
						$scope.popupCtrl.ok = function () {
							var submitObject = {};
							submitObject.dataName = selectedItem.dataName;
							submitObject.value = $scope.popupCtrl.selectedValue;
							destroyEditor();
							$uibModalInstance.close(submitObject);
						
						};						

						$scope.popupCtrl.close = function () { 
							destroyEditor();
							$uibModalInstance.close(null);
						}
					},
					controllerAs: "popupCtrl",
					backdrop:'static',
					size: "lg",
					resolve: {
						selectedItem: function () {
							return item;
						},
						selectedViewData: function () {
							return viewData;
						}
					}
				});
				//On modal exit
				modalInstance.result.then(function (returnObject) {
					if(returnObject != null){
						viewData[item.dataName] = returnObject.value;
						ngCtrl.fieldUpdate(item, returnObject.value, viewData['id']);
						
					}

				});
		
		}


		//#endregion

		//#region << Date time helpers >>
		ngCtrl.opened = {};
		ngCtrl.open = function (dataName, isOpen) {
			ngCtrl.opened[dataName] = isOpen;
		}
		//#endregion

		//#region << File upload >>
		ngCtrl.files = {}; // this is the data wrapper for the temporary upload objects that will be used in the html and for which we will generate watches below
		ngCtrl.progress = {}; //data wrapper for the progress percentage for each upload

		ngCtrl.getProgressStyle = function (name) {
			return "width: " + ngCtrl.progress[name] + "%;";
		}

		ngCtrl.uploadedFileName = "";
		ngCtrl.upload = function (file, item, recordId) {
			if (file != null) {
				ngCtrl.uploadedFileName = item.dataName + "-" + recordId;

				ngCtrl.moveSuccessCallback = function (response) {
					for (var i = 0; i < ngCtrl.view.data.length; i++) {
						if (ngCtrl.view.data[i].id == recordId) {
							ngCtrl.fieldUpdate(item, response.object.url, recordId);
							ngCtrl.view.data[i][item.dataName] = response.object.url;
							ngCtrl.progress[item.dataName][recordId] = 0;
						}
					}
				}

				ngCtrl.uploadSuccessCallback = function (response) {
					var tempPath = response.object.url;
					var fileName = response.object.filename;
					var targetPath = "/" + ngCtrl.entity.name + "/" + newGuid() + "/" + fileName;
					var overwrite = false;
					webvellaCoreService.moveFileFromTempToFS(tempPath, targetPath, overwrite, ngCtrl.moveSuccessCallback, ngCtrl.uploadErrorCallback);
				}
				ngCtrl.uploadErrorCallback = function (response) {
					alert(response.message);
				}
				ngCtrl.uploadProgressCallback = function (response) {
					ngCtrl.progress[item.dataName][recordId] = parseInt(100.0 * response.loaded / response.total);
				}

				webvellaCoreService.uploadFileToTemp(file, ngCtrl.uploadProgressCallback, ngCtrl.uploadSuccessCallback, ngCtrl.uploadErrorCallback);
			}
		};

		ngCtrl.cacheBreakers = {};
		ngCtrl.updateFileUpload = function (file, item, recordId) {
			if (file != null) {
				ngCtrl.uploadedFileName = item.dataName + "-" + recordId;

				ngCtrl.moveSuccessCallback = function (response) {
					for (var i = 0; i < ngCtrl.view.data.length; i++) {
						if (ngCtrl.view.data[i].id == recordId) {
							ngCtrl.view.data[i][item.dataName] = response.object.url;
							ngCtrl.fieldUpdate(item, response.object.url, recordId);
							ngCtrl.cacheBreakers[item.dataName][recordId] = "?v=" + moment().toISOString();
							ngCtrl.progress[item.dataName][recordId] = 0;
						}
					}
				}

				ngCtrl.uploadSuccessCallback = function (response) {
					var tempPath = response.object.url;
					var fileName = response.object.filename;
					var targetPath = "/" + ngCtrl.entity.name + "/" + newGuid() + "/" + fileName;
					var overwrite = true;
					webvellaCoreService.moveFileFromTempToFS(tempPath, targetPath, overwrite, ngCtrl.moveSuccessCallback, ngCtrl.uploadErrorCallback);
				}
				ngCtrl.uploadErrorCallback = function (response) {
					alert(response.message);
				}
				ngCtrl.uploadProgressCallback = function (response) {
					ngCtrl.progress[item.dataName][recordId] = parseInt(100.0 * response.loaded / response.total);
				}
				webvellaCoreService.uploadFileToTemp(file, ngCtrl.uploadProgressCallback, ngCtrl.uploadSuccessCallback, ngCtrl.uploadErrorCallback);
			}
		};

		ngCtrl.deleteFileUpload = function (item, recordId) {

			function deleteSuccessCallback(response) {
				for (var i = 0; i < ngCtrl.view.data.length; i++) {
					if (ngCtrl.view.data[i].id == recordId) {
						ngCtrl.view.data[i][item.dataName] = null;
						ngCtrl.progress[item.dataName][recordId] = 0;
						ngCtrl.fieldUpdate(item, null, recordId);
						return true;
					}
				}
			}

			function deleteFailedCallback(response) {
				ngToast.create({
					className: 'error',
					content: '<span class="go-red">Error:</span> ' + response.message,
					timeout: 7000
				});
				return "validation error";
			}

			for (var i = 0; i < ngCtrl.view.data.length; i++) {
				if (ngCtrl.view.data[i].id == recordId) {
					var filePath = ngCtrl.view.data[i][item.dataName];
					webvellaCoreService.deleteFileFromFS(filePath, deleteSuccessCallback, deleteFailedCallback);
				}
			}



		}
		//#endregion


		// Update field
		ngCtrl.fieldUpdate = function (item, data, recordId) {
			//alert("need to pass the recordId too so we can manage and viewFromRelation");
			return webvellaCoreService.viewAction_fieldUpdate(item, data, recordId, ngCtrl);
		}

		//#region << Remove relation >>
		ngCtrl.removeRelation = function (item) {
			var relation = ngCtrl.getRelation(item.relationName);
			var presentedFieldId = item.meta.id;
			var currentEntityId = ngCtrl.entity.id;
			//Currently it is implemented only for 1:N relation type and the current entity should be target and field is required
			if (relation.relationType == 2 && relation.targetEntityId == currentEntityId) {
				var itemObject = {};
				itemObject.meta = null;
				itemObject.entityName = ngCtrl.entity.name;
				for (var i = 0; i < ngCtrl.entity.fields.length; i++) {
					if (ngCtrl.entity.fields[i].id == relation.targetFieldId) {
						itemObject.meta = ngCtrl.entity.fields[i];
					}
				}
				if (itemObject.meta != null && !itemObject.meta.required) {
					ngCtrl.view.data[0][item.dataName] = [];
					ngCtrl.fieldUpdate(itemObject, null, ngCtrl.view.data[0]["id"]);
				}
			}
		}
		//#endregion

		//#endregion

		//#region << Modals >>

		//#region << Relation field >>

		////////////////////
		// Single selection modal used in 1:1 relation and in 1:N when the currently viewed entity is a target in this relation
		ngCtrl.openManageRelationFieldModal = function (item, relationType, dataKind, viewData) {
			//relationType = 1 (one-to-one) , 2(one-to-many), 3(many-to-many)
			//dataKind - target, origin, origin-target

			//Select ONE item modal
			if (relationType == 1 || (relationType == 2 && dataKind == "target")) {
				var modalInstance = $uibModal.open({
					animation: false,
					templateUrl: 'manageRelationFieldModal.html',
					controller: 'ManageRelationFieldModalController',
					controllerAs: "popupCtrl",
					size: "lg",
					resolve: {
						ngCtrl: function () {
							return ngCtrl;
						},
						viewData: function () {
							return viewData;
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
					if (ngCtrl.view.data["$field$" + returnObject.relationName + "$id"]) {
						oldRelationRecordId = ngCtrl.view.data["$field$" + returnObject.relationName + "$id"][0];
					}

					function successCallback(response) {
						ngToast.create({
							className: 'success',
							content: '<span class="go-green">Success:</span> Change applied'
						});
						webvellaCoreService.GoToState($state.current.name, ngCtrl.stateParams);
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
					var recordsToBeDetached = [];
					if (returnObject.dataKind == "origin") {
						recordsToBeAttached.push(returnObject.selectedRecordId);
						if (oldRelationRecordId != null) {
							recordsToBeDetached.push(oldRelationRecordId);
						}
						webvellaCoreService.updateRecordRelation(returnObject.relationName, displayedRecordId, recordsToBeAttached, recordsToBeDetached, successCallback, errorCallback);
					}
					else if (returnObject.dataKind == "target") {
						recordsToBeAttached.push(displayedRecordId);
						webvellaCoreService.updateRecordRelation(returnObject.relationName, returnObject.selectedRecordId, recordsToBeAttached, recordsToBeDetached, successCallback, errorCallback);
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
					controllerAs: "popupCtrl",
					size: "lg",
					resolve: {
						ngCtrl: function () {
							return ngCtrl;
						},
						viewData: function () {
							return viewData;
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
						webvellaCoreService.GoToState($state.current.name, ngCtrl.stateParams);
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
						webvellaCoreService.updateRecordRelation(returnObject.relationName, displayedRecordId, returnObject.attachDelta, returnObject.detachDelta, successCallback, errorCallback);
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
					controllerAs: "popupCtrl",
					size: "lg",
					resolve: {
						ngCtrl: function () {
							return ngCtrl;
						},
						viewData: function () {
							return viewData;
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
		ngCtrl.modalSelectedItem = {};
		ngCtrl.modalRelationType = -1;
		ngCtrl.modalDataKind = "";

		//Resolve function lookup records
		var resolveLookupRecords = function (item, relationType, dataKind) {
			// Initialize
			var defer = $q.defer();
			var defaultLookupList = null;
			ngCtrl.modalSelectedItem = fastCopy(item);
			ngCtrl.modalRelationType = fastCopy(relationType);
			ngCtrl.modalDataKind = fastCopy(dataKind);
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
				var responseObj = {};
				responseObj.success = response.success;
				responseObj.message = response.message;
				responseObj.timestamp = response.timestamp;
				responseObj.object = {};
				responseObj.object.data = response.object;
				responseObj.object.meta = defaultLookupList;
				defer.resolve(responseObj); //Submitting the whole response to manage the error states
			}

			function getEntityMetaSuccessCallback(response) {
				var entityMeta = response.object;
				var selectedLookupListName = ngCtrl.modalSelectedItem.fieldLookupList;
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

					//var gg = ngCtrl.modalSelectedItem;
					//ngCtrl.modalRelationType;
					//ngCtrl.modalDataKind;
					if (selectedLookupList != null) {
						defaultLookupList = selectedLookupList;
					}

					//Current record is Origin
					if (ngCtrl.modalDataKind == "origin") {
						//Find if the target field is required
						var targetRequiredField = false;
						var modalCurrrentRelation = ngCtrl.getRelation(ngCtrl.modalSelectedItem.relationName);
						for (var m = 0; m < entityMeta.fields.length; m++) {
							if (entityMeta.fields[m].id == modalCurrrentRelation.targetFieldId) {
								targetRequiredField = entityMeta.fields[m].required;
							}
						}
						if (targetRequiredField && ngCtrl.modalRelationType == 1) {
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
							webvellaCoreService.getRecordsByListMeta(defaultLookupList, entityMeta.name, 1, null, null, getListRecordsSuccessCallback, errorCallback);
						}
					}
					else if (ngCtrl.modalDataKind == "target") {
						//Current records is Target
						webvellaCoreService.getRecordsByListMeta(defaultLookupList, entityMeta.name, 1, null, null, getListRecordsSuccessCallback, errorCallback);
					}
				}
			}

			webvellaCoreService.getEntityMeta(item.entityName, getEntityMetaSuccessCallback, errorCallback);

			return defer.promise;
		}

		//#endregion

		//#region << Tree select field >>
		ngCtrl.openSelectTreeNodesModal = function (item, viewData) {
			var treeSelectModalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'selectTreeNodesModal.html',
				controller: 'SelectTreeNodesModalController',
				controllerAs: "popupCtrl",
				size: "width-100p",
				backdrop: "static",
				resolve: {
					ngCtrl: function () {
						return ngCtrl;
					},
					viewData: function () {
						return viewData;
					},
					selectedItem: function () {
						return item;
					},
					selectedItemData: function () {
						return viewData[item.dataName];
					},
					resolvedTree: resolveTree(item),
					resolvedTreeRelation: resolveTreeRelation(item),
					resolvedCurrentUserPermissions: function () {
						return resolvedCurrentUserEntityPermissions;
					}
				}
			});
			//On modal exit
			treeSelectModalInstance.result.then(function () {
				$state.reload();
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

			webvellaCoreService.getRecordsByTreeName(item.treeName, item.entityName, successCallback, errorCallback);

			return defer.promise;
		}

		var resolveTreeRelation = function (item) {
			// Initialize
			var response = null;

			for (var i = 0; i < ngCtrl.entityRelations.length; i++) {
				if (ngCtrl.entityRelations[i].id == item.relationId) {
					response = ngCtrl.entityRelations[i];
					break;
				}
			}

			return response;

		}
		
		var treeLabelFieldNameDict = {};

		ngCtrl.generateTreeRowData = function(treeMeta,rowData){
			//Get the label field name
			var treeLabelFieldName = "id";
			if(treeLabelFieldNameDict[treeMeta.entityName]){
				treeLabelFieldName = treeLabelFieldNameDict[treeMeta.entityName];
			}
			else {
				var treeEntityMeta = webvellaCoreService.getEntityMetaFromEntityList(treeMeta.entityName,resolvedEntityList);
				for (var i = 0; i < treeEntityMeta.fields.length; i++) {
					 var fieldMeta =  treeEntityMeta.fields[i];
					 if(fieldMeta.id == treeMeta.meta.nodeLabelFieldId){
						 treeLabelFieldName = fieldMeta.name;
						 treeLabelFieldNameDict[treeMeta.entityName] = fieldMeta.name;
						 break;
					 }
				}
			}

			return 	rowData[treeLabelFieldName];
		}
		//#endregion

		//#endregion

		//#region << List actions  bind >>
		var serviceName = safeViewNameAndEntity.entityName + "_" + safeViewNameAndEntity.viewName + "_view_service";
		try {
			ngCtrl.actionService = $injector.get(serviceName);
		}
		catch (err) {
			//console.log(err);
			ngCtrl.actionService = {};
		}
		ngCtrl.pageTitleActions = [];
		ngCtrl.pageTitleDropdownActions = [];
		ngCtrl.createBottomActions = [];
		ngCtrl.pageBottomActions = [];
		ngCtrl.view.meta.actionItems.sort(sort_by('menu', { name: 'weight', primer: parseInt, reverse: false }));
		ngCtrl.view.meta.actionItems.forEach(function (actionItem) {
			switch (actionItem.menu) {
				case "page-title":
					ngCtrl.pageTitleActions.push(actionItem);
					break;
				case "page-title-dropdown":
					ngCtrl.pageTitleDropdownActions.push(actionItem);
					break;
				case "create-bottom":
					ngCtrl.createBottomActions.push(actionItem);
					break;
				case "page-bottom":
					ngCtrl.pageBottomActions.push(actionItem);
					break;
			}
		});
		ngCtrl.deleteRecord = function () {
			webvellaCoreService.viewAction_deleteRecord(ngCtrl);
		}

		//Manage list
		ngCtrl.getViewManageUrl = function(){
			return "/#/admin/entities/"+ $stateParams.entityName +"/views/" + $stateParams.viewName;
		}

		ngCtrl.userIsAdmin = function(){
			return webvellaCoreService.userIsInRole("bdc56420-caf0-4030-8a0e-d264938e0cda");
		}

		//#endregion
	}

	//#region << Modal Controllers >>

	//Test to unify all modals - Single select, multiple select, click to select
	ManageRelationFieldModalController.$inject = ['ngCtrl', 'viewData', '$uibModalInstance', '$log', '$q', '$stateParams', 'modalMode', 'resolvedLookupRecords',
		'selectedDataKind', 'selectedItem', 'selectedRelationType', 'webvellaCoreService', 'ngToast', '$timeout', '$state', '$translate'];

	function ManageRelationFieldModalController(ngCtrl, viewData, $uibModalInstance, $log, $q, $stateParams, modalMode, resolvedLookupRecords,
		selectedDataKind, selectedItem, selectedRelationType, webvellaCoreService, ngToast, $timeout, $state, $translate) {

		var popupCtrl = this;
		popupCtrl.currentPage = 1;
		popupCtrl.parentData = fastCopy(ngCtrl);
		popupCtrl.parentData.view.data = viewData;
		popupCtrl.selectedItem = fastCopy(selectedItem);
		popupCtrl.modalMode = fastCopy(modalMode);
		popupCtrl.hasWarning = false;
		popupCtrl.warningMessage = "";

		//Init
		popupCtrl.currentlyAttachedIds = fastCopy(popupCtrl.parentData.view.data["$field$" + popupCtrl.selectedItem.relationName + "$id"]); // temporary object in order to highlight
		popupCtrl.dbAttachedIds = fastCopy(popupCtrl.currentlyAttachedIds);
		popupCtrl.getRelationLabel = ngCtrl.getRelationLabel;
		popupCtrl.attachedRecordIdsDelta = [];
		popupCtrl.detachedRecordIdsDelta = [];


		//Get the default lookup list for the entity
		if (resolvedLookupRecords.success) {
			popupCtrl.relationLookupList = fastCopy(resolvedLookupRecords.object);
		}
		else {
			popupCtrl.hasWarning = true;
			popupCtrl.warningMessage = resolvedLookupRecords.message;
		}

		//#region << Column widths from CSV >>
		popupCtrl.columnWidths = [];
		var columnWidthsArray = [];
		if (popupCtrl.relationLookupList.meta.columnWidthsCSV) {
			columnWidthsArray = popupCtrl.relationLookupList.meta.columnWidthsCSV.split(',');
		}
		var visibleColumns = popupCtrl.relationLookupList.meta.visibleColumnsCount;
		if (columnWidthsArray.length > 0) {
			for (var i = 0; i < visibleColumns; i++) {
				if (columnWidthsArray.length >= i + 1) {
					popupCtrl.columnWidths.push(columnWidthsArray[i]);
				}
				else {
					popupCtrl.columnWidths.push("auto");
				}
			}
		}
		else {
			//set all to auto
			for (var i = 0; i < visibleColumns; i++) {
				popupCtrl.columnWidths.push("auto");
			}
		}

		//#endregion

		//#region << List filter row >>
		popupCtrl.filterQuery = {};
		popupCtrl.listIsFiltered = false;
		popupCtrl.columnDictionary = {};
		popupCtrl.columnDataNamesArray = [];
		popupCtrl.queryParametersArray = [];
		//Extract the available columns
		popupCtrl.relationLookupList.meta.columns.forEach(function (column) {
			if (popupCtrl.columnDataNamesArray.indexOf(column.dataName) == -1) {
				popupCtrl.columnDataNamesArray.push(column.dataName);
			}
			popupCtrl.columnDictionary[column.dataName] = column;
		});
		popupCtrl.filterLoading = false;
		popupCtrl.columnDataNamesArray.forEach(function (dataName) {
			if (popupCtrl.queryParametersArray.indexOf(dataName) > -1) {
				popupCtrl.listIsFiltered = true;
				var columnObj = popupCtrl.columnDictionary[dataName];
				//some data validations and conversions	
				switch (columnObj.meta.fieldType) {
					//TODO if percent convert to > 1 %
					case 14:
						if (checkDecimal(queryObject[dataName])) {
							popupCtrl.filterQuery[dataName] = queryObject[dataName] * 100;
						}
						break;
					default:
						popupCtrl.filterQuery[dataName] = queryObject[dataName];
						break;

				}
			}
		});

		popupCtrl.applyQueryFilter = function () {
			var searchParams = {};
			popupCtrl.filterLoading = true;
			for (var filter in popupCtrl.filterQuery) {
				//Check if the field is percent or date
				if (popupCtrl.filterQuery[filter]) {
					for (var i = 0; i < popupCtrl.relationLookupList.meta.columns.length; i++) {
						if (popupCtrl.relationLookupList.meta.columns[i].meta.name == filter) {
							var selectedField = popupCtrl.relationLookupList.meta.columns[i].meta;
							switch (selectedField.fieldType) {
								case 4: //Date
									searchParams[filter] = moment(popupCtrl.filterQuery[filter], 'D MMM YYYY').toISOString();
									break;
								case 5: //Datetime
									searchParams[filter] = moment(popupCtrl.filterQuery[filter], 'D MMM YYYY HH:mm').toISOString();
									break;
								case 14: //Percent
									searchParams[filter] = popupCtrl.filterQuery[filter] / 100;
									break;
								default:
									searchParams[filter] = popupCtrl.filterQuery[filter];
									break;
							}
						}
					}
				}
				else {
					delete searchParams[filter];
				}
			}
			//Find the entity of the list. It could not be the current one as it could be listFromRelation case
			var listEntityName = popupCtrl.selectedItem.entityName;

			webvellaCoreService.getRecordsByListMeta(popupCtrl.relationLookupList.meta, listEntityName, 1, $stateParams, searchParams, popupCtrl.ReloadRecordsSuccessCallback, popupCtrl.ReloadRecordsErrorCallback);
		}

		popupCtrl.ReloadRecordsSuccessCallback = function (response) {
			popupCtrl.relationLookupList.data = response.object;
			//Just a little wait
			$timeout(function () {
				popupCtrl.filterLoading = false;
			}, 300);
		}

		popupCtrl.ReloadRecordsErrorCallback = function (response) {
			//Just a little wait
			$timeout(function () {
				popupCtrl.filterLoading = false;
			}, 300);
			alert(response.message);
		}


		popupCtrl.getAutoIncrementString = function (column) {
			var returnObject = {};
			returnObject.prefix = null;
			returnObject.suffix = null;
			var keyIndex = column.meta.displayFormat.indexOf('{0}');
			if (keyIndex == 0) {
				return null;
			}
			else {
				returnObject.prefix = column.meta.displayFormat.slice(0, keyIndex);
				if (keyIndex + 3 < column.meta.displayFormat.length) {
					returnObject.suffix = column.meta.displayFormat.slice(keyIndex + 3, column.meta.displayFormat.length);
				}
				return returnObject;
			}
		}

		//#endregion

		//#region << Extract fields that are supported in the query to be filters>>
		popupCtrl.fieldsInQueryArray = webvellaCoreService.extractSupportedFilterFields(popupCtrl.relationLookupList);
		popupCtrl.checkIfFieldSetInQuery = function (dataName) {
			if (popupCtrl.fieldsInQueryArray.fieldNames.indexOf(dataName) != -1) {
				return true;
			}
			else {
				return false;
			}
		}

		popupCtrl.allQueryComparisonList = [];
		//#region << Query Dictionary >>
		$translate(['QUERY_RULE_EQ_LABEL', 'QUERY_RULE_NOT_LABEL', 'QUERY_RULE_LT_LABEL', 'QUERY_RULE_LTE_LABEL',
					'QUERY_RULE_GT_LABEL', 'QUERY_RULE_GTE_LABEL', 'QUERY_RULE_CONTAINS_LABEL', 'QUERY_RULE_NOT_CONTAINS_LABEL',
					'QUERY_RULE_STARTSWITH_LABEL', 'QUERY_RULE_NOT_STARTSWITH_LABEL','QUERY_RULE_FTS_LABEL']).then(function (translations) {
						popupCtrl.allQueryComparisonList = [
							{
								key: "EQ",
								value: translations.QUERY_RULE_EQ_LABEL
							},
							{
								key: "NOT",
								value: translations.QUERY_RULE_NOT_LABEL
							},
							{
								key: "LT",
								value: translations.QUERY_RULE_LT_LABEL
							},
							{
								key: "LTE",
								value: translations.QUERY_RULE_LTE_LABEL
							},
							{
								key: "GT",
								value: translations.QUERY_RULE_GT_LABEL
							},
							{
								key: "GTE",
								value: translations.QUERY_RULE_GTE_LABEL
							},
							{
								key: "CONTAINS",
								value: translations.QUERY_RULE_CONTAINS_LABEL
							},
							{
								key: "NOTCONTAINS",
								value: translations.QUERY_RULE_NOT_CONTAINS_LABEL
							},
							{
								key: "STARTSWITH",
								value: translations.QUERY_RULE_STARTSWITH_LABEL
							},
							{
								key: "NOTSTARTSWITH",
								value: translations.QUERY_RULE_NOT_STARTSWITH_LABEL
							},
							{
								key: "FTS",
								value: translations.QUERY_RULE_FTS_LABEL
							}
						];

					});
		//#endregion
		popupCtrl.getFilterInputPlaceholder = function (dataName) {
			var fieldIndex = popupCtrl.fieldsInQueryArray.fieldNames.indexOf(dataName);
			if (fieldIndex == -1) {
				return "";
			}
			else {
				var fieldQueryType = popupCtrl.fieldsInQueryArray.queryTypes[fieldIndex];
				for (var i = 0; i < popupCtrl.allQueryComparisonList.length; i++) {
					if (popupCtrl.allQueryComparisonList[i].key == fieldQueryType) {
						return popupCtrl.allQueryComparisonList[i].value;
					}
				}
				return "";
			}
		}
		//#endregion





		//#region << Paging >>
		popupCtrl.selectPage = function (page) {
			// Process
			function successCallback(response) {
				popupCtrl.relationLookupList.data = fastCopy(response.object);
				popupCtrl.currentPage = page;
			}

			function errorCallback(response) {

			}

			webvellaCoreService.getRecordsByListMeta(popupCtrl.relationLookupList.meta, popupCtrl.selectedItem.entityName, page, null, null, successCallback, errorCallback);
		}

		//#endregion

		//#region << Logic >>

		//Render field values
		popupCtrl.renderFieldValue = webvellaCoreService.renderFieldValue;

		popupCtrl.isSelectedRecord = function (recordId) {
			if (popupCtrl.currentlyAttachedIds) {
				return popupCtrl.currentlyAttachedIds.indexOf(recordId) > -1
			}
			else {
				return false;
			}
		}

		//Single record before save
		popupCtrl.selectSingleRecord = function (record) {
			var returnObject = {
				relationName: popupCtrl.selectedItem.relationName,
				dataKind: selectedDataKind,
				selectedRecordId: record.id
			};
			$uibModalInstance.close(returnObject);
		};

		// Multiple records before save
		popupCtrl.attachRecord = function (record) {
			//Add record to delta  if it is NOT part of the original list
			if (popupCtrl.dbAttachedIds.indexOf(record.id) == -1) {
				popupCtrl.attachedRecordIdsDelta.push(record.id);
			}

			//Check and remove from the detachDelta if it is there
			var elementIndex = popupCtrl.detachedRecordIdsDelta.indexOf(record.id);
			if (elementIndex > -1) {
				popupCtrl.detachedRecordIdsDelta.splice(elementIndex, 1);
			}
			//Update the currentlyAttachedIds for highlight
			elementIndex = popupCtrl.currentlyAttachedIds.indexOf(record.id);
			if (elementIndex == -1) {
				//this is the normal case
				popupCtrl.currentlyAttachedIds.push(record.id);
			}
			else {
				//if it is already in the highligted list there is probably some miscalculation from previous operation, but for now we will do nothing
			}

		}
		popupCtrl.detachRecord = function (record) {
			//Add record to detachDelta if it is part of the original list
			if (popupCtrl.dbAttachedIds.indexOf(record.id) > -1) {
				popupCtrl.detachedRecordIdsDelta.push(record.id);
			}
			//Check and remove from attachDelta if it is there
			var elementIndex = popupCtrl.attachedRecordIdsDelta.indexOf(record.id);
			if (elementIndex > -1) {
				popupCtrl.attachedRecordIdsDelta.splice(elementIndex, 1);
			}
			//Update the currentlyAttachedIds for highlight
			elementIndex = popupCtrl.currentlyAttachedIds.indexOf(record.id);
			if (elementIndex > -1) {
				//this is the normal case
				popupCtrl.currentlyAttachedIds.splice(elementIndex, 1);
			}
			else {
				//if it is already not in the highligted list there is probably some miscalculation from previous operation, but for now we will do nothing
			}
		}
		popupCtrl.saveRelationChanges = function () {
			var returnObject = {
				relationName: popupCtrl.selectedItem.relationName,
				dataKind: selectedDataKind,
				attachDelta: popupCtrl.attachedRecordIdsDelta,
				detachDelta: popupCtrl.detachedRecordIdsDelta
			};
			$uibModalInstance.close(returnObject);
			//category_id
		};

		//Instant save on selection, keep popup open
		popupCtrl.processingRecordId = "";
		popupCtrl.processOperation = "";
		popupCtrl.processInstantSelection = function (returnObject) {

			// Initialize
			popupCtrl.processingRecordId = returnObject.selectedRecordId;
			popupCtrl.processOperation = returnObject.operation;
			var displayedRecordId = $stateParams.recordId;
			var recordsToBeAttached = [];
			var recordsToBeDetached = [];
			if (returnObject.operation == "attach") {
				recordsToBeAttached.push(displayedRecordId);
			}
			else if (returnObject.operation == "detach") {
				recordsToBeDetached.push(displayedRecordId);
			}

			function successCallback(response) {
				var currentRecordId = fastCopy(popupCtrl.processingRecordId);
				var elementIndex = popupCtrl.currentlyAttachedIds.indexOf(currentRecordId);
				if (popupCtrl.processOperation == "attach" && elementIndex == -1) {
					popupCtrl.currentlyAttachedIds.push(currentRecordId);
					popupCtrl.processingRecordId = "";
				}
				else if (popupCtrl.processOperation == "detach" && elementIndex > -1) {
					popupCtrl.currentlyAttachedIds.splice(elementIndex, 1);
					popupCtrl.processingRecordId = "";
				}
				webvellaCoreService.GoToState($state.current.name, popupCtrl.parentData.stateParams);
				ngToast.create({
					className: 'success',
					content: '<span class="go-green">Success:</span> Change applied'
				});
			}

			function errorCallback(response) {
				popupCtrl.processingRecordId = "";
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
				webvellaCoreService.updateRecordRelation(returnObject.relationName, returnObject.selectedRecordId, recordsToBeAttached, recordsToBeDetached, successCallback, errorCallback);
			}
			else {
				alert("the <<origin-target>> dataKind is still not implemented. Contact the system administrator");
			}
		}
		popupCtrl.instantAttachRecord = function (record) {
			var returnObject = {
				relationName: popupCtrl.selectedItem.relationName,
				dataKind: selectedDataKind,
				selectedRecordId: record.id,
				operation: "attach"
			};
			if (!popupCtrl.processingRecordId) {
				popupCtrl.processInstantSelection(returnObject);
			}
		};
		popupCtrl.instantDetachRecord = function (record) {
			var returnObject = {
				relationName: popupCtrl.selectedItem.relationName,
				dataKind: selectedDataKind,
				selectedRecordId: record.id,
				operation: "detach"

			};
			if (!popupCtrl.processingRecordId) {
				popupCtrl.processInstantSelection(returnObject);
			}
		};

		//#endregion


		popupCtrl.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};

		/// Aux
		//function successCallback(response) {
		//	ngToast.create({
		//		className: 'success',
		//		content: '<span class="go-green">Success:</span> ' + response.message
		//	});
		//	$uibModalInstance.close('success');
		//	popupCtrl.parentData.modalInstance.close('success');
		//	//webvellaCoreService.GoToState($state.current.name, {});
		//}

		//function errorCallback(response) {
		//	popupCtrl.hasError = true;
		//	popupCtrl.errorMessage = response.message;


		//}


		//#endregion
	}

	SelectTreeNodesModalController.$inject = ['ngCtrl', 'viewData', '$uibModalInstance', '$rootScope', '$scope', '$log', '$q', '$stateParams', 'resolvedTree',
		'selectedItem', 'resolvedTreeRelation', 'selectedItemData', 'webvellaCoreService', 'ngToast', '$timeout', '$state', '$uibModal',
		'resolvedCurrentUserPermissions'];
	function SelectTreeNodesModalController(ngCtrl, viewData, $uibModalInstance, $rootScope, $scope, $log, $q, $stateParams, resolvedTree,
			selectedItem, resolvedTreeRelation, selectedItemData, webvellaCoreService, ngToast, $timeout, $state, $uibModal,
			resolvedCurrentUserPermissions) {
		var popupCtrl = this;

		//#region << Init >>
		popupCtrl.relation = fastCopy(resolvedTreeRelation);
		popupCtrl.currentEntity = fastCopy(ngCtrl.entity);
		popupCtrl.currentField = {};
		for (var i = 0; i < popupCtrl.currentEntity.fields.length; i++) {
			if (popupCtrl.currentEntity.fields[i].selectedTreeId == selectedItem.treeId) {
				popupCtrl.currentField = popupCtrl.currentEntity.fields[i];
			}
		}
		popupCtrl.tree = fastCopy(resolvedTree);
		popupCtrl.itemMeta = fastCopy(selectedItem);
		popupCtrl.addButtonLoadingClass = {};
		popupCtrl.attachHoverEffectClass = {};

		popupCtrl.userPermissionsForTreeEntity = {};
		for (var i = 0; i < resolvedCurrentUserPermissions.length; i++) {
			if (resolvedCurrentUserPermissions[i].entityId == selectedItem.entityId) {
				popupCtrl.userPermissionsForTreeEntity = fastCopy(resolvedCurrentUserPermissions[i]);
			}
		}

		//#region << Select the already selected nodes >>
		popupCtrl.selectedTreeRecords = [];
		for (var i = 0; i < selectedItemData.length; i++) {
			popupCtrl.selectedTreeRecords.push(selectedItemData[i].id);
		}


		//#region


		//#endregion 

		popupCtrl.close = function () {
			$uibModalInstance.close();
		};


		//#region << Read only tree >>

		//#region << Node collapse >>
		popupCtrl.collapsedTreeNodes = [];
		popupCtrl.toggleNodeCollapse = function (node) {
			var nodeIndex = popupCtrl.collapsedTreeNodes.indexOf(node.id);
			if (nodeIndex > -1) {
				popupCtrl.collapsedTreeNodes.splice(nodeIndex, 1);
			}
			else {
				popupCtrl.collapsedTreeNodes.push(node.id);
			}
		}

		popupCtrl.nodesToBeCollapsed = [];

		function iterateCollapse(current, depth) {
			var children = current.nodes;
			if (children.length > 0) {
				popupCtrl.collapsedTreeNodes.push(current.id);
			}
			for (var i = 0, len = children.length; i < len; i++) {
				iterateCollapse(children[i], depth + 1);
			}
		}

		popupCtrl.collapseAll = function () {
			popupCtrl.collapsedTreeNodes = [];
			for (var i = 0; i < popupCtrl.tree.data.length; i++) {
				iterateCollapse(popupCtrl.tree.data[i], 0);
			}
		}
		//Initially collapse all
		$timeout(function () {
			popupCtrl.collapseAll();
		}, 0);

		popupCtrl.expandAll = function () {
			popupCtrl.collapsedTreeNodes = [];
		}

		//#endregion

		//#region << Node selection >>

		popupCtrl.selectableNodeIds = [];

		var selectedNodesByBranch = {};

		function iterateCanBeSelected(current, depth, rootNode, isInitial) {
			var children = current.nodes;
			var shouldBeSelectable = true;
			//isInitial is added in order to auto collapse nodes that are more than 3 children
			if (isInitial && children.length > 3) {
				popupCtrl.collapsedTreeNodes.push(current.id);
			}
			//Case: selection type
			switch (popupCtrl.currentField.selectionType) {
				case "single-select":
					if (popupCtrl.selectedTreeRecords && popupCtrl.selectedTreeRecords.length > 0 && popupCtrl.selectedTreeRecords[0] != current.recordId) {
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

			switch (popupCtrl.currentField.selectionTarget) {
				case "all":
					break;
				case "leaves":
					//Check if the node is not selected
					var leaveCheckIndex = popupCtrl.selectedTreeRecords.indexOf(current.recordId);
					if (children.length > 0 && leaveCheckIndex == -1) {
						shouldBeSelectable = false;
					}
					break;
			}

			if (shouldBeSelectable) {
				popupCtrl.selectableNodeIds.push(current.id);
			}

			for (var i = 0, len = children.length; i < len; i++) {
				iterateCanBeSelected(children[i], depth + 1, rootNode, isInitial);
			}
		}

		popupCtrl.regenerateCanBeSelected = function (isInitial) {
			//isInitial is added in order to auto collapse nodes that are more than 3 children
			popupCtrl.selectableNodeIds = [];
			for (var i = 0; i < popupCtrl.tree.data.length; i++) {
				iterateCanBeSelected(popupCtrl.tree.data[i], 0, popupCtrl.tree.data[i], isInitial);
			}
		}

		popupCtrl.toggleNodeSelection = function (node) {
			var nodeIndex = popupCtrl.selectedTreeRecords.indexOf(node.recordId);
			var recordsToBeAttached = [];
			var recordsToBeDetached = [];
			function createRelationChangeSuccessCallback(response) {
				popupCtrl.selectedTreeRecords.push(node.recordId);
				//Add to the branch selected object
				var nodeRootBranchId = node.nodes[0];
				if (selectedNodesByBranch[nodeRootBranchId]) {
					selectedNodesByBranch[node.nodes[0]].push(node.id);
				}
				else {
					selectedNodesByBranch[node.nodes[0]] = [];
					selectedNodesByBranch[node.nodes[0]].push(node.id);
				}
				popupCtrl.regenerateCanBeSelected(false);
			}
			function removeRelationChangeSuccessCallback(response) {
				popupCtrl.selectedTreeRecords.splice(nodeIndex, 1);
				var nodeRootBranchId = node.nodes[0];

				if (selectedNodesByBranch[nodeRootBranchId]) {
					var selectedIndex = selectedNodesByBranch[nodeRootBranchId].indexOf(node.id)
					selectedNodesByBranch[node.nodes[0]].splice(selectedIndex, 1);
				}
				popupCtrl.regenerateCanBeSelected(false);
			}
			function applyRelationChangeErrorCallback(response) { }
			//Node should be unselected. Relations should be severed
			if (nodeIndex > -1) {
				recordsToBeDetached.push($stateParams.recordId);
				webvellaCoreService.updateRecordRelation(popupCtrl.relation.name, node.recordId, recordsToBeAttached, recordsToBeDetached, removeRelationChangeSuccessCallback, applyRelationChangeErrorCallback);
			}
				//Node should be selected. Relations should be created
			else {
				recordsToBeAttached.push($stateParams.recordId);
				webvellaCoreService.updateRecordRelation(popupCtrl.relation.name, node.recordId, recordsToBeAttached, recordsToBeDetached, createRelationChangeSuccessCallback, applyRelationChangeErrorCallback);
			}
		}

		popupCtrl.regenerateCanBeSelected(true);

		//#endregion

		//#region << Register toggle node events >>

		//This event is later used by the recursive directive that follows
		////READY hook listeners
		var toggleTreeNodeSelectedDestructor = $rootScope.$on("webvellaAdmin-toggleTreeNode-selected", function (event, data) {
			popupCtrl.toggleNodeSelection(data);
		})

		var toggleTreeNodeCollapsedDestructor = $rootScope.$on("webvellaAdmin-toggleTreeNode-collapsed", function (event, data) {
			popupCtrl.toggleNodeCollapse(data);
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


})();