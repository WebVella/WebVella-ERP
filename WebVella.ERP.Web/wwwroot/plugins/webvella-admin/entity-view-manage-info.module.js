/* entity-view-manage.module.js */

/**
* @desc this module manages a single entity view in the admin screen
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminEntityViewManageInfoController', controller)
		.controller('DeleteViewModalController', deleteViewModalController);

    //#region << Configuration >> /////////////////////////
    config.$inject = ['$stateProvider'];
    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-admin-entity-view-manage-info', {
            parent: 'webvella-admin-base',
            url: '/entities/:entityName/views/:viewName/info',
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
                    controller: 'WebVellaAdminEntityViewManageInfoController',
                    templateUrl: '/plugins/webvella-admin/entity-view-manage-info.view.html',
                    controllerAs: 'contentData'
                }
            },
            resolve: {
                resolvedCurrentEntityMeta: resolveCurrentEntityMeta,
                resolvedCurrentView: resolveCurrentView
            },
            data: {

            }
        });
    };
    //#endregion

    //#region << Resolve >> ///////////////////////////////
    resolveCurrentEntityMeta.$inject = ['$q', '$log', 'webvellaAdminService', '$stateParams', '$state', '$timeout'];
    /* @ngInject */
    function resolveCurrentEntityMeta($q, $log, webvellaAdminService, $stateParams, $state, $timeout) {
        $log.debug('webvellaAdmin>entity-details> BEGIN resolveCurrentEntityMeta state.resolved');
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
        $log.debug('webvellaAdmin>entity-details> END resolveCurrentEntityMeta state.resolved');
        return defer.promise;
    }

    resolveCurrentView.$inject = ['$q', '$log', 'webvellaAdminService', '$stateParams', '$state', '$timeout'];
    /* @ngInject */
    function resolveCurrentView($q, $log, webvellaAdminService, $stateParams, $state, $timeout) {
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
            	var enableObjectConversion = true;
            	if (enableObjectConversion) {
            		//Convert old object to new object
            		var co = {};
            		co.meta = response.object;
            		var itemText = {};
            		itemText.type = "field";
            		itemText.dataName = "$field$text";
            		itemText.entityName = "test";
            		itemText.entityLabel = "Test";
            		itemText.entityLabelPlural = "Tests";
            		itemText.relationName = null;
            		itemText.meta = {
            			"auditable": false,
            			"defaultValue": null,
            			"description": "",
            			"fieldType": 18,
            			"helpText": "",
            			"id": "fc61bd8e-67bb-4eac-bb85-c285884f4c5f",
            			"label": "Text",
            			"maxLength": null,
            			"name": "text",
            			"placeholderText": "",
            			"required": false,
            			"searchable": false,
            			"system": false,
            			"unique": false
            		};
            		co.meta.regions[0].sections[0].rows[0].columns[0].items[0] = itemText;

            		co.data = [];
            		var dataRecord = {
            			"$field$text": "boz"
            		};
            		co.data.push(dataRecord);
            		defer.resolve(co);
            	}
            	else {
            		defer.resolve(response.object);
            	}
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

        webvellaAdminService.getEntityView($stateParams.viewName, $stateParams.entityName, successCallback, errorCallback);

        // Return
        $log.debug('webvellaAdmin>entity-views>resolveCurrentView END state.resolved');
        return defer.promise;
    }

    //#endregion

    //#region << Controller >> ////////////////////////////
    controller.$inject = ['$filter', '$scope', '$log', '$rootScope', '$state', '$stateParams', 'pageTitle', '$modal',
                            'resolvedCurrentEntityMeta', 'resolvedCurrentView', 'webvellaAdminService', 'ngToast'];
    /* @ngInject */
    function controller($filter,$scope, $log, $rootScope, $state,$stateParams, pageTitle, $modal,
                        resolvedCurrentEntityMeta, resolvedCurrentView, webvellaAdminService, ngToast) {
        $log.debug('webvellaAdmin>entity-view-manage-info> START controller.exec');

        /* jshint validthis:true */
        var contentData = this;
        //#region << Initialize Current Entity >>
        contentData.entity = angular.copy(resolvedCurrentEntityMeta);
        //#endregion

        //#region << Update page title & Hide side menu>>
        contentData.pageTitle = "Entity Views | " + pageTitle;
        $rootScope.$emit("application-pageTitle-update", contentData.pageTitle);
        //Hide side menu
        $rootScope.$emit("application-body-sidebar-menu-isVisible-update", false);
        $log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted');
        $scope.$on("$destroy", function () {
            $rootScope.$emit("application-body-sidebar-menu-isVisible-update", true);
            $log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted');
        });
        //#endregion

        //#region << Initialize View and Content Region >>
        contentData.view = angular.copy(resolvedCurrentView.meta);
        //#endregion

        contentData.fieldUpdate = function (key, data) {
        	contentData.patchObject = {};
        	contentData.patchObject[key] = data;
        	webvellaAdminService.patchEntityView(contentData.patchObject, contentData.view.name, $stateParams.entityName, patchSuccessCallback, patchFailedCallback);
        }

        function patchSuccessCallback(response) {
        	ngToast.create({
        		className: 'success',
        		content: '<span class="go-green">Success:</span> ' + response.message
        	});
        	return true;
        }
        function patchFailedCallback(response) {
        	ngToast.create({
        		className: 'error',
        		content: '<span class="go-red">Error:</span> ' + response.message
        	});
        	return false;
        }

    	//Delete view
        contentData.deleteViewModal = function () {
        	var modalInstance = $modal.open({
        		animation: false,
        		templateUrl: 'deleteViewModal.html',
        		controller: 'DeleteViewModalController',
        		controllerAs: "popupData",
        		size: "",
        		resolve: {
        			parentData: function () { return contentData; }
        		}
        	});
        }

        contentData.viewTypes = [
		{
			name: "general",
			label: "General"
		},
		{
			name: "quickview",
			label: "Quick view"
		},
		{
			name: "quickcreate",
			label: "Quick create"
		}
        ];

        contentData.showViewType = function () {
        	var selected = $filter('filter')(contentData.viewTypes, { name: contentData.view.type });
        	return (contentData.view.type && selected.length) ? selected[0].label : 'empty';
        };

        $log.debug('webvellaAdmin>entity-view-manage-info> END controller.exec');

    }
    //#endregion
	//// Modal Controllers
    deleteViewModalController.$inject = ['parentData', '$modalInstance', '$log', 'webvellaAdminService', 'webvellaRootService', 'ngToast', '$timeout', '$state'];

	/* @ngInject */
    function deleteViewModalController(parentData, $modalInstance, $log, webvellaAdminService, webvellaRootService, ngToast, $timeout, $state) {
    	$log.debug('webvellaAdmin>entities>deleteFieldModal> START controller.exec');
    	/* jshint validthis:true */
    	var popupData = this;
    	popupData.parentData = parentData;

    	popupData.ok = function () {

    		webvellaAdminService.deleteEntityView(popupData.parentData.view.name, popupData.parentData.entity.name, successCallback, errorCallback);

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
    			$state.go("webvella-admin-entity-views", { entityName: popupData.parentData.entity.name }, { reload: true });
    		}, 0);
    	}

    	function errorCallback(response) {
    		popupData.hasError = true;
    		popupData.errorMessage = response.message;


    	}
    	$log.debug('webvellaAdmin>entities>createEntityModal> END controller.exec');
    };


})();
