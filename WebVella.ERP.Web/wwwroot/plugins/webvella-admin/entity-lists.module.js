/* entity-lists.module.js */

/**
* @desc this module manages the entity record lists in the admin screen
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminEntityListsController', controller)
		.controller('createListModalController', createListModalController);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];

    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-admin-entity-lists', {
            parent: 'webvella-admin-base',
            url: '/entities/:entityName/lists', //  /desktop/areas after the parent state is prepended
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
                    controller: 'WebVellaAdminEntityListsController',
                    templateUrl: '/plugins/webvella-admin/entity-lists.view.html',
                    controllerAs: 'contentData'
                }
            },
            resolve: {
                resolvedCurrentEntityMeta: resolveCurrentEntityMeta,
                resolvedEntityRecordsList: resolveEntityRecordsList
            },
            data: {

            }
        });
    };


    //#region << Resolve Functions >>/////////////////////////
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


    resolveEntityRecordsList.$inject = ['$q', '$log', 'webvellaAdminService', '$stateParams', '$state', '$timeout'];
    /* @ngInject */
    function resolveEntityRecordsList($q, $log, webvellaAdminService, $stateParams, $state, $timeout) {
        $log.debug('webvellaAdmin>entity-records-list>resolveEntityRecordsList BEGIN state.resolved');
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

        webvellaAdminService.getEntityLists($stateParams.entityName, successCallback, errorCallback);

        // Return
        $log.debug('webvellaAdmin>entity-records-list>resolveEntityRecordsList END state.resolved');
        return defer.promise;
    }
    //#endregion

    //#region << Controller >> ///////////////////////////////
    controller.$inject = ['$scope', '$log', '$rootScope', '$state', 'pageTitle', 'resolvedCurrentEntityMeta', '$modal', 'resolvedEntityRecordsList'];
    /* @ngInject */
    function controller($scope, $log, $rootScope, $state, pageTitle, resolvedCurrentEntityMeta, $modal, resolvedEntityRecordsList) {
        $log.debug('webvellaAdmin>entity-records-list> START controller.exec');
        /* jshint validthis:true */
        var contentData = this;
        //#region << Initialize the current entity >>
        contentData.entity = angular.copy(resolvedCurrentEntityMeta);
        //#endregion

        //#region << Update page title & hide the side menu >>
        contentData.pageTitle = "Entity Details | " + pageTitle;
        $rootScope.$emit("application-pageTitle-update", contentData.pageTitle);
        //Hide Sidemenu
        $rootScope.$emit("application-body-sidebar-menu-isVisible-update", false);
        $log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted');
        $scope.$on("$destroy", function () {
            $rootScope.$emit("application-body-sidebar-menu-isVisible-update", true);
            $log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted');
        });
        //#endregion

        //#region << Initialize the lists >>
        contentData.lists = angular.copy(resolvedEntityRecordsList.recordLists);
        //#endregion

    	//Create new list modal
        contentData.createListModal = function () {
        	var modalInstance = $modal.open({
        		animation: false,
        		templateUrl: 'createListModal.html',
        		controller: 'createListModalController',
        		controllerAs: "popupData",
        		size: "lg",
        		resolve: {
        			contentData: function () { return contentData; }
        		}
        	});
        }

        $log.debug('webvellaAdmin>entity-records-list> END controller.exec');
    }
    //#endregion

	//// Modal Controllers
    createListModalController.$inject = ['$modalInstance', '$log', 'ngToast', '$timeout', '$state', '$location', 'contentData', 'webvellaAdminService', 'webvellaRootService'];

	/* @ngInject */
    function createListModalController($modalInstance, $log, ngToast, $timeout, $state, $location, contentData, webvellaAdminService, webvellaRootService) {
    	$log.debug('webvellaAdmin>entities>createViewModalController> START controller.exec');
    	/* jshint validthis:true */
    	var popupData = this;
    	popupData.modalInstance = $modalInstance;
    	popupData.contentData = angular.copy(contentData);
    	popupData.list = webvellaAdminService.initList();
    	//Check if there is an id column set, if not include it as it always should be there

    	var idFieldGuid = null;
    	for (var j = 0; j < popupData.contentData.entity.fields.length; j++) {
    		if (popupData.contentData.entity.fields[j].name == "id") {
    			idFieldGuid = popupData.contentData.entity.fields[j].id;
    		}
    	}
    	var column = {};
    	column.fieldId = idFieldGuid;
    	column.fieldName = "id";
    	column.type = "field";
    	popupData.list.columns.unshift(column);

    	popupData.ok = function () {
    		webvellaAdminService.createEntityList(popupData.list, popupData.contentData.entity.name, successCallback, errorCallback);
    	};

    	popupData.cancel = function () {
    		$modalInstance.dismiss('cancel');
    	};

    	/// Aux
    	function successCallback(response) {
    		ngToast.create({
    			className: 'success',
    			content: '<span class="go-green">Success:</span> ' + 'The view was successfully saved'
    		});
    		$modalInstance.close('success');
    		webvellaRootService.GoToState($state, $state.current.name, {});
    	}

    	function errorCallback(response) {
    		var location = $location;
    		//Process the response and generate the validation Messages
    		webvellaRootService.generateValidationMessages(response, popupData, popupData.entity, location);
    	}

    	$log.debug('webvellaAdmin>entities>createViewModalController> END controller.exec');
    };

})();
