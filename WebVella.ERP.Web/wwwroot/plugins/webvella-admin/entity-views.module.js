/* entity-views.module.js */

/**
* @desc this module manages the entity views in the admin screen
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminEntityViewsController', controller);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];

    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-admin-entity-views', {
            parent: 'webvella-admin-base',
            url: '/entities/:entityName/views', //  /desktop/areas after the parent state is prepended
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
                    controller: 'WebVellaAdminEntityViewsController',
                    templateUrl: '/plugins/webvella-admin/entity-views.view.html',
                    controllerAs: 'contentData'
                }
            },
            resolve: {
                resolvedCurrentEntityMeta: resolveCurrentEntityMeta,
                resolvedCurrentEntityViews: resolveCurrentEntityViews // TODO this should be removed once the views are implemented in the entity Meta
            },
            data: {

            }
        });
    };


    // Resolve Function /////////////////////////
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


    // TODO this should be removed once the views are implemented in the entity Meta
    resolveCurrentEntityViews.$inject = ['$q', '$log', 'webvellaAdminService', '$stateParams', '$state', '$timeout'];
    /* @ngInject */
    function resolveCurrentEntityViews($q, $log, webvellaAdminService, $stateParams, $state, $timeout) {
        $log.debug('webvellaAdmin>entity-views> BEGIN state.resolved');
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

        webvellaAdminService.getEntityViewList($stateParams.entityName, successCallback, errorCallback);

        // Return
        $log.debug('webvellaAdmin>entity-views> END state.resolved');
        return defer.promise;
    }


    // Controller ///////////////////////////////
    controller.$inject = ['$scope', '$log', '$rootScope', '$state', 'pageTitle', 'resolvedCurrentEntityMeta', '$modal', 'resolvedCurrentEntityViews'];

    /* @ngInject */
    function controller($scope, $log, $rootScope, $state, pageTitle, resolvedCurrentEntityMeta, $modal, resolvedCurrentEntityViews) {
        $log.debug('webvellaAdmin>entity-details> START controller.exec');
        /* jshint validthis:true */
        var contentData = this;
        contentData.entity = resolvedCurrentEntityMeta;
        contentData.views = resolvedCurrentEntityViews.views; // TODO this should be removed once the views are implemented in the entity Meta

        //Update page title
        contentData.pageTitle = "Entity Views | " + pageTitle;
        $rootScope.$emit("application-pageTitle-update", contentData.pageTitle);
        //Hide Sidemenu
        $rootScope.$emit("application-body-sidebar-menu-isVisible-update", false);
        $log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted');
        $scope.$on("$destroy", function () {
            $rootScope.$emit("application-body-sidebar-menu-isVisible-update", true);
            $log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted');
        });

        //Create new entity modal
        contentData.openDeleteEntityModal = function () {
            var modalInstance = $modal.open({
                animation: false,
                templateUrl: 'deleteEntityModal.html',
                controller: 'DeleteEntityModalController',
                controllerAs: "popupData",
                size: "",
                resolve: {
                    parentData: function () { return contentData; }
                }
            });

        }

        activate();
        $log.debug('webvellaAdmin>entity-details> END controller.exec');
        function activate() { }
    }


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
                content: '<h4>Success</h4><p>' + response.message + '</p>'
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
