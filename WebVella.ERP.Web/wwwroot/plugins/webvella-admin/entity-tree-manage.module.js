/* entity-relations.module.js */

/**
* @desc this module manages the entity relations screen in the administration
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminManageEntityTreeController', controller)
		.controller('DeleteTreeModalController', deleteTreeModalController);
    
    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];

    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-admin-entity-tree-manage', {
            parent: 'webvella-admin-base',
            url: '/entities/:entityName/trees/:treeName', //  /desktop/areas after the parent state is prepended
            views: {
                "topnavView": {
                    controller: 'WebVellaAdminTopnavController',
                    templateUrl: '/plugins/webvella-admin/topnav.view.html',
                    controllerAs: 'topnavData'
                },
                "sidebarView": {
                    controller: 'WebVellaAdminSidebarController',
                    templateUrl: '/plugins/webvella-admin/sidebar.view.html',
                    controllerAs: 'sidebarData'
                },
                "contentView": {
                	controller: 'WebVellaAdminManageEntityTreeController',
                    templateUrl: '/plugins/webvella-admin/entity-tree-manage.view.html',
                    controllerAs: 'contentData'
                }
            },
            resolve: {
            	checkedAccessPermission: checkAccessPermission,
                resolvedRelationsList: resolveRelationsList,
                resolvedCurrentEntityMeta: resolveCurrentEntityMeta,
                resolvedCurrentEntityRecordTree: resolveCurrentEntityRecordTree
            },
            data: {

            }
        });
    };


	//#region << Resolve Function >>/////////////////////////
    checkAccessPermission.$inject = ['$q', '$log', 'resolvedCurrentUser', 'ngToast'];
	/* @ngInject */
    function checkAccessPermission($q, $log, resolvedCurrentUser, ngToast) {
    	$log.debug('webvellaAreas>entities> BEGIN check access permission ' + moment().format('HH:mm:ss SSSS'));
    	var defer = $q.defer();
    	var messageContent = '<span class="go-red">No access:</span> You do not have access to the <span class="go-red">Admin</span> area';
    	var accessPermission = false;
    	for (var i = 0; i < resolvedCurrentUser.roles.length; i++) {
    		if (resolvedCurrentUser.roles[i] == "bdc56420-caf0-4030-8a0e-d264938e0cda") {
    			accessPermission = true;
    		}
    	}

    	if (accessPermission) {
    		defer.resolve();
    	}
    	else {

    		ngToast.create({
    			className: 'error',
    			content: messageContent,
    			timeout: 7000
    		});
    		defer.reject("No access");
    	}

    	$log.debug('webvellaAreas>entities> BEGIN check access permission ' + moment().format('HH:mm:ss SSSS'));
    	return defer.promise;
    }

    resolveRelationsList.$inject = ['$q', '$log', 'webvellaAdminService', '$state', '$timeout'];
    /* @ngInject */
    function resolveRelationsList($q, $log, webvellaAdminService, $state, $timeout) {
    	$log.debug('webvellaAdmin>entity-relations> BEGIN resolveRelationsList state.resolved ' + moment().format('HH:mm:ss SSSS'));
        // Initialize
        var defer = $q.defer();

        // Process
        function successCallback(response) {
            defer.resolve(response.object);
        }

        function errorCallback(response) {
        	defer.reject(response.message);
        }

        webvellaAdminService.getRelationsList(successCallback, errorCallback);

        // Return
        $log.debug('webvellaAdmin>entity-relations> END resolveRelationsList state.resolved ' + moment().format('HH:mm:ss SSSS'));
        return defer.promise;
    }

    resolveCurrentEntityMeta.$inject = ['$q', '$log', 'webvellaAdminService', '$stateParams', '$state', '$timeout'];
    /* @ngInject */
    function resolveCurrentEntityMeta($q, $log, webvellaAdminService, $stateParams, $state, $timeout) {
    	$log.debug('webvellaAdmin>entity-details> BEGIN resolveCurrentEntityMeta state.resolved ' + moment().format('HH:mm:ss SSSS'));
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

        webvellaAdminService.getEntityMeta($stateParams.entityName, successCallback, errorCallback);

        // Return
        $log.debug('webvellaAdmin>entity-details> END resolveCurrentEntityMeta state.resolved ' + moment().format('HH:mm:ss SSSS'));
        return defer.promise;
    }

    resolveCurrentEntityRecordTree.$inject = ['$q', '$log', 'webvellaAdminService', '$state', '$timeout', '$stateParams'];
	/* @ngInject */
    function resolveCurrentEntityRecordTree($q, $log, webvellaAdminService, $state, $timeout, $stateParams) {
    	$log.debug('webvellaAdmin>entity-relations> BEGIN resolveRelationsList state.resolved ' + moment().format('HH:mm:ss SSSS'));
    	// Initialize
    	var defer = $q.defer();

    	// Process
    	function successCallback(response) {
    		defer.resolve(response.object);
    	}

    	function errorCallback(response) {
    		defer.reject(response.message);
    	}

    	webvellaAdminService.getEntityTree($stateParams.treeName,$stateParams.entityName, successCallback, errorCallback);

    	// Return
    	$log.debug('webvellaAdmin>entity-relations> END resolveRelationsList state.resolved ' + moment().format('HH:mm:ss SSSS'));
    	return defer.promise;
    }

	//#endregion

    // Controller ///////////////////////////////
    controller.$inject = ['$scope','$sce', '$log', '$rootScope', '$state','$timeout', 'pageTitle', 'resolvedRelationsList', 'resolvedCurrentEntityMeta',
					'$uibModal', 'resolvedCurrentEntityRecordTree', 'webvellaAdminService','ngToast'];
    /* @ngInject */
    function controller($scope,$sce, $log, $rootScope, $state,$timeout, pageTitle, resolvedRelationsList, resolvedCurrentEntityMeta,
					$uibModal, resolvedCurrentEntityRecordTree, webvellaAdminService, ngToast) {
    	$log.debug('webvellaAdmin>entity-relations> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
        /* jshint validthis:true */
    	var contentData = this;

		//#region << Init >>
        contentData.search = {};
        contentData.allRelations = fastCopy(resolvedRelationsList);
        contentData.currentEntityRelation = [];
        contentData.entity = fastCopy(resolvedCurrentEntityMeta);
        contentData.tree = fastCopy(resolvedCurrentEntityRecordTree);
    	//Awesome font icon names array 
        contentData.icons = getFontAwesomeIconNames();
        //Update page title
        contentData.pageTitle = "Entity Trees | " + pageTitle;
        $rootScope.$emit("application-pageTitle-update", contentData.pageTitle);
        //Hide Sidemenu
        $rootScope.$emit("application-body-sidebar-menu-isVisible-update", false);
        $log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted ' + moment().format('HH:mm:ss SSSS'));
        $scope.$on("$destroy", function () {
            $rootScope.$emit("application-body-sidebar-menu-isVisible-update", true);
            $log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted ' + moment().format('HH:mm:ss SSSS'));
        });

		//#region << Init selected relation >>
        contentData.selectedRelation = {};
        for (var i = 0; i < contentData.allRelations.length; i++) {
        	if (contentData.allRelations[i].id == contentData.tree.relationId) {
        		contentData.selectedRelation = contentData.allRelations[i];
        	}
        }
		//#endregion

    	//#region << Node options >>
        contentData.nodeIdField = {};
        contentData.nodeParentIdField = {};
        contentData.nodeNameEligibleFields = [];
        contentData.nodeLabelEligibleFields = [];

        for (var i = 0; i < contentData.entity.fields.length; i++) {
        	if (contentData.entity.fields[i].id == contentData.selectedRelation.originFieldId) {
        		contentData.nodeIdField = contentData.entity.fields[i];
        	}

        	if (contentData.entity.fields[i].id == contentData.selectedRelation.targetFieldId) {
        		contentData.nodeParentIdField = contentData.entity.fields[i];
        	}
			//Fill dictionaries
        	switch (contentData.entity.fields[i].fieldType) {
        		case 1: //Auto-increment
        			if (contentData.entity.fields[i].required) {
        				contentData.nodeLabelEligibleFields.push(contentData.entity.fields[i]);
        				if (contentData.entity.fields[i].unique) {
        					contentData.nodeNameEligibleFields.push(contentData.entity.fields[i]);
        				}
        			}
        			break;
        		case 16: //Guid
        			if (contentData.entity.fields[i].required) {
        				contentData.nodeLabelEligibleFields.push(contentData.entity.fields[i]);
        				if (contentData.entity.fields[i].unique) {
        					contentData.nodeNameEligibleFields.push(contentData.entity.fields[i]);
        				}
        			}
        			break;
        		case 18: // Text
        			if (contentData.entity.fields[i].required) {
        				contentData.nodeLabelEligibleFields.push(contentData.entity.fields[i]);
        				if (contentData.entity.fields[i].unique) {
        					contentData.nodeNameEligibleFields.push(contentData.entity.fields[i]);
        				}
        			}
        			break;
        	}
        }

        contentData.nodeNameEligibleFields = contentData.nodeNameEligibleFields.sort(function (a, b) {
        	if (a.name < b.name) return -1;
        	if (a.name > b.name) return 1;
        	return 0;
        });

        contentData.nodeLabelEligibleFields = contentData.nodeLabelEligibleFields.sort(function (a, b) {
        	if (a.name < b.name) return -1;
        	if (a.name > b.name) return 1;
        	return 0;
        });
    	//#endregion

		//#region << nodeNameField >> - auxiliary object
        contentData.nodeNameField = null;
        if (!contentData.tree.nodeNameFieldId) {
        	contentData.nodeNameField = contentData.nodeNameEligibleFields[0];
        }
        else {
        	for (var i = 0; i < contentData.nodeNameEligibleFields.length; i++) {
        		if (contentData.nodeNameEligibleFields[i].id == contentData.tree.nodeNameFieldId) {
        			contentData.nodeNameField = contentData.nodeNameEligibleFields[i];
        		}
        	}
        	//if old field id not found in the dictionary, it is probably changed or deleted. 
        	if (contentData.nodeNameField == null) {
        		contentData.tree.nodeNameFieldId = null;
        		contentData.nodeNameField = contentData.nodeNameEligibleFields[0];
        	}
        }
    	//#endregion

    	//#region << nodeLabelField >> - auxiliary object
        contentData.nodeLabelField = null;
        if (!contentData.tree.nodeLabelFieldId) {
        	contentData.nodeLabelField = contentData.nodeLabelEligibleFields[0];
        }
        else {
        	for (var i = 0; i < contentData.nodeLabelEligibleFields.length; i++) {
        		if (contentData.nodeLabelEligibleFields[i].id == contentData.tree.nodeLabelFieldId) {
        			contentData.nodeLabelField = contentData.nodeLabelEligibleFields[i];
        		}
        	}
        	//if old field id not found in the dictionary, it is probably changed or deleted. 
        	if (contentData.nodeLabelField == null) {
        		contentData.tree.nodeLabelFieldId = null;
        		contentData.nodeLabelField = contentData.nodeLabelEligibleFields[0];
        	}
        }
    	//#endregion



		//#endregion

    	//#region << Logic >>
        contentData.getRelationHtml = function (tree) {
        	var result = "unknown";
        	if (contentData.selectedRelation) {
        		if(contentData.selectedRelation.relationType == 2){
        			result =$sce.trustAsHtml(contentData.selectedRelation.name + " <span class=\"badge badge-primary badge-inverse\" title=\"One to Many\" style=\"margin-left:5px;\">1 : N</span>");
        		}
				else if(contentData.selectedRelation.relationType == 3) {
        			result = $sce.trustAsHtml(contentData.selectedRelation.name + ' <span class="badge badge-primary badge-inverse" title="Many to Many" style="margin-left:5px;">N : N</span>');
        		}
          	}
        	return result;
        }

        function patchSuccessCallback(response) {
        	ngToast.create({
        		className: 'success',
        		content: '<span class="go-green">Success:</span> ' + response.message
        	});
        	$timeout(function () { 
        		contentData.tree = response.object;
        		contentData.addRecordId = null;
        	}, 0);
        }
        function patchErrorCallback(response) {
        	ngToast.create({
        		className: 'error',
        		content: '<span class="go-red">Error:</span> ' + response.message,
        		timeout: 7000
        	});
        }

        contentData.fieldUpdate = function (fieldName, data) {
        	var postObj = {};
        	postObj[fieldName] = data;
        	webvellaAdminService.patchEntityTree(postObj, contentData.tree.name, contentData.entity.name, patchSuccessCallback, patchErrorCallback)
        }

        contentData.nodeNameUpdate = function (fieldObject) {
        	contentData.fieldUpdate('nodeNameFieldId', fieldObject.id);
        }

        contentData.nodeLabelUpdate = function (fieldObject) {
        	contentData.fieldUpdate('nodeLabelFieldId', fieldObject.id);
        }

        contentData.checkForAddEnter = function (e) {
        	var code = (e.keyCode ? e.keyCode : e.which);
        	if (code == 13) { //Enter keycode
        		contentData.addNewRootNodeById();
        	}
        }
        contentData.addNewRootNodeById = function () {
        	var rootNodeObject = {
				"recordId": null,
        		"id": null,
        		"name": null,
        		"label": null,
        		"parentId": null
        	}
        	rootNodeObject.recordId = contentData.addRecordId;
        	var rootNodes = fastCopy(contentData.tree.rootNodes);
        	rootNodes.push(rootNodeObject);
        	contentData.fieldUpdate('rootNodes', rootNodes);
        }

        contentData.removeRootNode = function (record, $index) {
        	var rootNodes = fastCopy(contentData.tree.rootNodes);
        	rootNodes.splice($index, 1);
        	contentData.fieldUpdate('rootNodes', rootNodes);
        }

        contentData.fieldSelectedBy = function (field) {
        	//Check if field id is in array
        	var index = contentData.tree.nodeObjectProperties.indexOf(field.id);
        	if (index > -1) {
        		//Check who selected it
        		if (field.id == contentData.tree.nodeIdFieldId || field.id == contentData.tree.nodeParentIdFieldId ||
				field.id == contentData.tree.nodeNameFieldId || field.id == contentData.tree.nodeLabelFieldId) {
        			return "system";
        		}
        		else {
        			return "user";
        		}
        	}
        	else {
        		return "none";
        	}
       	
        }

        contentData.getFieldType = function (field) {
        	var fieldTypes = getFieldTypes();
        	for (var i = 0; i < fieldTypes.length; i++) {
        		if (fieldTypes[i].id == field.fieldType) {
        			return fieldTypes[i].label;
        		}
        	}
        	return "";
        }

        contentData.toggleFieldSelection = function (field) {
        	//Check if field selected and by who
        	switch (contentData.fieldSelectedBy(field)) {
        		case "user":
        			//We need to remove it from array
        			var index = contentData.tree.nodeObjectProperties.indexOf(field.id);
        			contentData.tree.nodeObjectProperties.splice(index, 1);
        			break;
        		case "system":
        			break;
        		case "none":
        			//we need to add it to the array
        			contentData.tree.nodeObjectProperties.push(field.id);
        			break;
        	}
        	contentData.fieldUpdate('nodeObjectProperties', contentData.tree.nodeObjectProperties);
        }

		//#endregion

		//#region << Modals >>
        contentData.deleteTreeModal = function () {
        	var modalInstance = $uibModal.open({
        		animation: false,
        		templateUrl: 'deleteTreeModal.html',
        		controller: 'DeleteTreeModalController',
        		controllerAs: "popupData",
        		size: "",
        		resolve: {
        			parentData: function () { return contentData; }
        		}
        	});
        }

		//#endregion

        $log.debug('webvellaAdmin>entity-relations> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
    }

	//#region << Modal Controllers >>
    deleteTreeModalController.$inject = ['parentData', '$modalInstance', '$log', 'webvellaAdminService', 'webvellaRootService', 'ngToast', '$timeout', '$state'];

	/* @ngInject */
    function deleteTreeModalController(parentData, $modalInstance, $log, webvellaAdminService, webvellaRootService, ngToast, $timeout, $state) {
    	$log.debug('webvellaAdmin>entities>deleteListModal> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
    	/* jshint validthis:true */
    	var popupData = this;
    	popupData.parentData = parentData;

    	popupData.ok = function () {

    		webvellaAdminService.deleteEntityTree(popupData.parentData.tree.name, popupData.parentData.entity.name, successCallback, errorCallback);

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
    		$timeout(function () {
    			$state.go("webvella-admin-entity-trees", { entityName: popupData.parentData.entity.name }, { reload: true });
    		}, 0);
    	}

    	function errorCallback(response) {
    		popupData.hasError = true;
    		popupData.errorMessage = response.message;


    	}
    	$log.debug('webvellaAdmin>entities>deleteListModal> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
    };

	//#endregion
})();


