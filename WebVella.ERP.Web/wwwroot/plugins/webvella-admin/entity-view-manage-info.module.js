/* entity-view-manage.module.js */

/**
* @desc this module manages a single entity view in the admin screen
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminEntityViewManageInfoController', controller);

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
                resolvedCurrentView: resolveCurrentView // TODO this should be removed once the views are implemented in the entity Meta
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

        webvellaAdminService.getEntityView($stateParams.viewName, $stateParams.entityName, successCallback, errorCallback);

        // Return
        $log.debug('webvellaAdmin>entity-views>resolveCurrentView END state.resolved');
        return defer.promise;
    }

    //#endregion

    //#region << Controller >> ////////////////////////////
    controller.$inject = ['$scope', '$log', '$rootScope', '$state', 'pageTitle', '$modal',
                            'resolvedCurrentEntityMeta', 'resolvedCurrentView', 'webvellaAdminService', 'ngToast'];
    /* @ngInject */
    function controller($scope, $log, $rootScope, $state, pageTitle, $modal,
                        resolvedCurrentEntityMeta, resolvedCurrentView, webvellaAdminService, ngToast) {
        $log.debug('webvellaAdmin>entity-view-manage-info> START controller.exec');

        /* jshint validthis:true */
        var contentData = this;
        //#region << Initialize Current Entity >>
        contentData.entity = resolvedCurrentEntityMeta;
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
        contentData.view = angular.copy(resolvedCurrentView);
        //#endregion

        contentData.fieldUpdate = function (key, data) {
        	contentData.patchObject = {};
        	contentData.patchObject[key] = data;
        	webvellaAdminService.patchEntityView(contentData.patchObject, contentData.view.name, contentData.entity.name, patchSuccessCallback, patchFailedCallback);
        }

        function patchSuccessCallback(response) {
        	ngToast.create({
        		className: 'success',
        		content: '<h4>Success</h4><p>' + response.message + '</p>'
        	});
        	return true;
        }
        function patchFailedCallback(response) {
        	ngToast.create({
        		className: 'error',
        		content: '<h4>Error</h4><p>' + response.message + '</p>'
        	});
        	return false;
        }

        $log.debug('webvellaAdmin>entity-view-manage-info> END controller.exec');

    }
    //#endregion


})();
