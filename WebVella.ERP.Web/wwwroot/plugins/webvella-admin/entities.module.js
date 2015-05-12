/* home.module.js */

/**
* @desc this module manages the application home desktop screen
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminEntitiesController', controller)
        .controller('CreateEntityModalController', createEntityController);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];

    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-admin-entities', {
            parent: 'webvella-admin-base',
            url: '/entities', //  /desktop/areas after the parent state is prepended
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
                    controller: 'WebVellaAdminEntitiesController',
                    templateUrl: '/plugins/webvella-admin/entities.view.html',
                    controllerAs: 'contentData'
                }
            },
            resolve: {
                resolvedEntityMetaList: resolveEntityMetaList
            },
            data: {

            }
        });
    };


    // Resolve Function /////////////////////////
    resolveEntityMetaList.$inject = ['$q', '$log', 'webvellaAdminService'];

    /* @ngInject */
    function resolveEntityMetaList($q, $log, webvellaAdminService) {
        $log.debug('webvellaAdmin>entities> BEGIN state.resolved');
        // Initialize
        var defer = $q.defer();

        // Process
        function successCallback(response) {
            defer.resolve(response.object);
        }

        function errorCallback(response) {
            defer.resolve(response.object);
        }

        webvellaAdminService.getMetaEntityList(successCallback, errorCallback);

        // Return
        $log.debug('webvellaAdmin>entities> END state.resolved');
        return defer.promise;
    }



    // Controller ///////////////////////////////
    controller.$inject = ['$log', '$rootScope', '$state', 'pageTitle', 'resolvedEntityMetaList', '$modal'];

    /* @ngInject */
    function controller($log, $rootScope, $state, pageTitle, resolvedEntityMetaList, $modal) {
        $log.debug('webvellaAdmin>entities> START controller.exec');
        /* jshint validthis:true */
        var contentData = this;
        //Update page title
        contentData.pageTitle = "Entities | " + pageTitle;
        $rootScope.$emit("application-pageTitle-update", contentData.pageTitle);
        contentData.entities = resolvedEntityMetaList.entities;

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
        $log.debug('webvellaAdmin>entities> END controller.exec');
        function activate() { }
    }


    //// Modal Controllers
    createEntityController.$inject = ['$modalInstance', '$log', 'webvellaAdminService', 'webvellaRootService', 'ngToast', '$timeout', '$state', '$location'];

    /* @ngInject */
    function createEntityController($modalInstance, $log, webvellaAdminService,webvellaRootService, ngToast, $timeout, $state, $location) {
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
