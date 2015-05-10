/* home.module.js */

/**
* @desc this module manages the application home desktop screen
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminEntityDetailsController', controller)
        .controller('DeleteEntityModalController', deleteEntityController);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];

    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-admin-entity-details', {
            parent: 'webvella-admin-base',
            url: '/admin/entities/:name', //  /desktop/areas after the parent state is prepended
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
                    controller: 'WebVellaAdminEntityDetailsController',
                    templateUrl: '/plugins/webvella-admin/entity-details.view.html',
                    controllerAs: 'contentData'
                }
            },
            resolve: {
                resolvedCurrentEntityMeta: resolveCurrentEntityMeta
            },
            data: {

            }
        });
    };


    // Resolve Function /////////////////////////
    resolveCurrentEntityMeta.$inject = ['$q', '$log', 'webvellaAdminService','$stateParams'];

    /* @ngInject */
    function resolveCurrentEntityMeta($q, $log, webvellaAdminService, $stateParams) {
        $log.debug('webvellaAdmin>entity-details> BEGIN state.resolved');
        // Initialize
        var defer = $q.defer();

        // Process
        function successCallback(response) {
            defer.resolve(response.object);
        }

        function errorCallback(response) {
            defer.resolve(response.object);
        }

        webvellaAdminService.getEntityMeta($stateParams.name, successCallback, errorCallback);

        // Return
        $log.debug('webvellaAdmin>entity-details> END state.resolved');
        return defer.promise;
    }



    // Controller ///////////////////////////////
    controller.$inject = ['$scope', '$log', '$rootScope', '$state', 'pageTitle', 'resolvedCurrentEntityMeta', '$modal'];

    /* @ngInject */
    function controller($scope, $log, $rootScope, $state, pageTitle, resolvedCurrentEntityMeta, $modal) {
        $log.debug('webvellaAdmin>entity-details> START controller.exec');
        /* jshint validthis:true */
        var contentData = this;
        contentData.entity = resolvedCurrentEntityMeta;
        //Update page title
        contentData.pageTitle = "Entity Details | " + pageTitle;
        $rootScope.$emit("application-pageTitle-update", contentData.pageTitle);
        //Hide Sidemenu
        $rootScope.$emit("application-body-sidebar-menu-isVisible-update", false);
        $log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted');
        $scope.$on("$destroy", function () {
            $rootScope.$emit("application-body-sidebar-menu-isVisible-update", true);
            $log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted');
        });



        //Create new entity modal
        contentData.openAddEntityModal = function () {
            var modalInstance = $modal.open({
                animation: false,
                templateUrl: 'createEntityModal.html',
                controller: 'CreateEntityModalController',
                controllerAs: "modalData",
                size: "",
                resolve: {}
            });

        }

        activate();
        $log.debug('webvellaAdmin>entity-details> END controller.exec');
        function activate() { }
    }


    //// Modal Controllers
    deleteEntityController.$inject = ['$modalInstance', '$log', 'webvellaAdminService', 'webvellaRootService', 'ngToast', '$timeout', '$state', '$location'];

    /* @ngInject */
    function deleteEntityController($modalInstance, $log, webvellaAdminService, webvellaRootService, ngToast, $timeout, $state, $location) {
        $log.debug('webvellaAdmin>entities>createEntityModal> START controller.exec');
        /* jshint validthis:true */
        var modalData = this;
        modalData.entity = {
            id: null,
            name: "",
            label: "",
            pluralLabel: "",
            system: false
        };

        modalData.ok = function () {
            webvellaAdminService.createEntity(modalData.entity, successCallback, errorCallback)
            //$modalInstance.close(modalData.entity);
        };

        modalData.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

        /// Aux
        function successCallback(response) {
            ngToast.create({
                className: 'success',
                content: '<h4>Success</h4><p>The entity was successfully created</p>'
            });
            $modalInstance.close('success');
            webvellaRootService.reloadCurrentState($state);
        }

        function errorCallback(response) {
            var location = $location;
            //Process the response and generate the validation Messages
            webvellaRootService.generateValidationMessages(response, modalData,modalData.entity, location);
        }
        $log.debug('webvellaAdmin>entities>createEntityModal> END controller.exec');
    };

})();
