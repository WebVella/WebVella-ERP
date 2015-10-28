/* entity-relations.module.js */

/**
* @desc this module manages the entity relations screen in the administration
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminManageEntityTreeNodesController', controller);
    
    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];

    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-admin-entity-tree-nodes-manage', {
            parent: 'webvella-admin-base',
            url: '/entities/:entityName/trees/:treeName/nodes', //  /desktop/areas after the parent state is prepended
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
                	controller: 'WebVellaAdminManageEntityTreeNodesController',
                    templateUrl: '/plugins/webvella-admin/entity-tree-nodes-manage.view.html',
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

    	webvellaAdminService.getEntityTreeMeta($stateParams.treeName,$stateParams.entityName, successCallback, errorCallback);

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
		//#endregion


    	//#region << Manage tree >>

        contentData.addButtonLoadingClass = {};

        contentData.attachHoverEffectClass = {};

        contentData.addNodeModal = function (node) {
        	contentData.addButtonLoadingClass[node.id] = true;
        }

        contentData.removeNodeModal = function (node) {
        	contentData.addButtonLoadingClass[node.id] = false;
        }

    	//#endregion

        $log.debug('webvellaAdmin>entity-relations> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
    }

})();


