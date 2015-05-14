/* entity-relations.module.js */

/**
* @desc this module manages the entity relations screen in the administration
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminEntityRelationsController', controller)
        .controller('CreateRelationModalController', CreateRelationModalController);
    
    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];

    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-admin-entity-relations', {
            parent: 'webvella-admin-base',
            url: '/entities/:name/relations', //  /desktop/areas after the parent state is prepended
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
                    controller: 'WebVellaAdminEntityRelationsController',
                    templateUrl: '/plugins/webvella-admin/entity-relations.view.html',
                    controllerAs: 'contentData'
                }
            },
            resolve: {
                resolvedRelationsList: resolveRelationsList,
                resolvedCurrentEntityMeta: resolveCurrentEntityMeta,
                resolvedEntityList:resolveEntityList
            },
            data: {

            }
        });
    };


    // Resolve Function /////////////////////////
    resolveRelationsList.$inject = ['$q', '$log', 'webvellaAdminService', '$state', '$timeout'];
    /* @ngInject */
    function resolveRelationsList($q, $log, webvellaAdminService, $state, $timeout) {
        $log.debug('webvellaAdmin>entity-relations> BEGIN resolveRelationsList state.resolved');
        // Initialize
        var defer = $q.defer();

        // Process
        function successCallback(response) {
             defer.resolve(response.object);
        }

        function errorCallback(response) {
             defer.resolve(response.object);
        }

        webvellaAdminService.getRelationsList(successCallback, errorCallback);

        // Return
        $log.debug('webvellaAdmin>entity-relations> END resolveRelationsList state.resolved');
        return defer.promise;
    }

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

        webvellaAdminService.getEntityMeta($stateParams.name, successCallback, errorCallback);

        // Return
        $log.debug('webvellaAdmin>entity-details> END resolveCurrentEntityMeta state.resolved');
        return defer.promise;
    }

    resolveEntityList.$inject = ['$q', '$log', 'webvellaAdminService', '$state', '$timeout'];
    /* @ngInject */
    function resolveEntityList($q, $log, webvellaAdminService, $state, $timeout) {
        $log.debug('webvellaAdmin>entity-relations> BEGIN resolveEntityList state.resolved');
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
        $log.debug('webvellaAdmin>entity-relations> END resolveEntityList state.resolved');
        return defer.promise;
    }


    // Controller ///////////////////////////////
    controller.$inject = ['$scope', '$log', '$rootScope', '$state', 'pageTitle', 'resolvedRelationsList', 'resolvedCurrentEntityMeta', 'resolvedEntityList', '$modal'];

    /* @ngInject */
    function controller($scope, $log, $rootScope, $state, pageTitle, resolvedRelationsList,resolvedCurrentEntityMeta,resolvedEntityList, $modal) {
        $log.debug('webvellaAdmin>entity-relations> START controller.exec');
        /* jshint validthis:true */
        var contentData = this;
        contentData.allRelations = resolvedRelationsList;
        contentData.currentEntityRelation = [];
        contentData.entity = resolvedCurrentEntityMeta;
        contentData.entityList = resolvedEntityList.entities;

        //Initialize relations in the scope of this entity
        for (var i = 0; i < contentData.allRelations.length; i++) {
            if (contentData.allRelations[i].originEntityId == contentData.entity.id || contentData.allRelations[i].targetEntityId == contentData.entity.id) {
                contentData.currentEntityRelation.push(contentData.allRelations[i]);
            }
        }
        for (var j = 0; j < contentData.currentEntityRelation.length; j++) {
            for (var k = 0; k < contentData.entityList.length; k++) {
                if (contentData.currentEntityRelation[j].originEntityId == contentData.entityList[k].id) {
                    //add origin Name 
                    contentData.currentEntityRelation[j].originEntityName = contentData.entityList[k].name;
                    //add origin label 
                    contentData.currentEntityRelation[j].originEntityLabel = contentData.entityList[k].label;

                    for (var m = 0; m < contentData.entityList[k].fields.length; m++) {
                        if (contentData.entityList[k].fields[m].id == contentData.currentEntityRelation[j].originFieldId) {
                            //add target Name 
                            contentData.currentEntityRelation[j].originFieldName = contentData.entityList[k].fields[m].name;
                            //add target Label 
                            contentData.currentEntityRelation[j].originFieldLabel = contentData.entityList[k].fields[m].label;
                        }
                    }
                }
                if (contentData.currentEntityRelation[j].targetEntityId == contentData.entityList[k].id) {
                    //add target Name 
                    contentData.currentEntityRelation[j].targetEntityName = contentData.entityList[k].name;
                    //add target Label 
                    contentData.currentEntityRelation[j].targetEntityLabel = contentData.entityList[k].label

                    for (var m = 0; m < contentData.entityList[k].fields.length; m++) {
                        if (contentData.entityList[k].fields[m].id == contentData.currentEntityRelation[j].targetFieldId) {
                            //add target Name 
                            contentData.currentEntityRelation[j].targetFieldName = contentData.entityList[k].fields[m].name;
                            //add target Label 
                            contentData.currentEntityRelation[j].targetFieldLabel = contentData.entityList[k].fields[m].label;
                        }
                    }
                }
            }
        }

        //Update page title
        contentData.pageTitle = "Entity Relations | " + pageTitle;
        $rootScope.$emit("application-pageTitle-update", contentData.pageTitle);
        //Hide Sidemenu
        $rootScope.$emit("application-body-sidebar-menu-isVisible-update", false);
        $log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted');
        $scope.$on("$destroy", function () {
            $rootScope.$emit("application-body-sidebar-menu-isVisible-update", true);
            $log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted');
        });


        contentData.createRelationModal = function () {
            var modalInstance = $modal.open({
                animation: false,
                templateUrl: 'createRelationModal.html',
                controller: 'CreateRelationModalController',
                controllerAs: "modalData",
                size: "lg",
                resolve: {
                    contentData: function () {
                        return contentData;
                    }
                }
            });
        }
    

        activate();
        $log.debug('webvellaAdmin>entity-relations> END controller.exec');
        function activate() { }
    }


    //// Modal Controllers
    CreateRelationModalController.$inject = ['$modalInstance', '$log', 'webvellaAdminService', 'webvellaRootService', 'ngToast', '$timeout', '$state', '$location', 'contentData'];

    /* @ngInject */
    function CreateRelationModalController($modalInstance, $log, webvellaAdminService, webvellaRootService, ngToast, $timeout, $state, $location, contentData) {
        $log.debug('webvellaAdmin>entities>CreateRelationModalController> START controller.exec');
        /* jshint validthis:true */
        var modalData = this;


        modalData.ok = function () {
            webvellaAdminService.createEntity(modalData.entity, successCallback, errorCallback)
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
            webvellaRootService.generateValidationMessages(response, modalData, modalData.entity, location);
        }
        $log.debug('webvellaAdmin>entities>CreateRelationModalController> END controller.exec');
    };


})();
