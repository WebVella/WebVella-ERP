/* entity-lists.module.js */

/**
* @desc this module manages the entity record lists in the admin screen
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminEntityListsController', controller);

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

        webvellaAdminService.getEntityListsList($stateParams.entityName, successCallback, errorCallback);

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
        contentData.entity = resolvedCurrentEntityMeta;
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
        contentData.lists = resolvedEntityRecordsList;
        //#endregion

        $log.debug('webvellaAdmin>entity-records-list> END controller.exec');
    }
    //#endregion

    //// Modal Controllers
    deleteEntityController.$inject = ['parentData', '$modalInstance', '$log', 'webvellaAdminService', 'ngToast', '$timeout', '$state'];

    /* @ngInject */
    function deleteEntityController(parentData, $modalInstance, $log, webvellaAdminService, ngToast, $timeout, $state) {
        $log.debug('webvellaAdmin>entities>createEntityModal> START controller.exec');
        /* jshint validthis:true */
        var popupData = this;
        popupData.entity = parentData.entity;

        popupData.ok = function () {
            webvellaAdminService.deleteEntity(popupData.entity.id, successCallback, errorCallback)
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
                $state.go("webvella-admin-entities");
            }, 0)
        }

        function errorCallback(response) {
            popupData.hasError = true;
            popupData.errorMessage = response.message;


        }
        $log.debug('webvellaAdmin>entities>createEntityModal> END controller.exec');
    };

})();
