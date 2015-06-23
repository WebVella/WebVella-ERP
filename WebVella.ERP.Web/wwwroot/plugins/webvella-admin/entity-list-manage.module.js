/* entity-lists.module.js */

/**
* @desc this module manages the entity record lists in the admin screen
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminEntityListManageController', controller);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];

    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-admin-entity-list-manage', {
            parent: 'webvella-admin-base',
            url: '/entities/:entityName/lists/:listName', //  /desktop/areas after the parent state is prepended
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
                    controller: 'WebVellaAdminEntityListManageController',
                    templateUrl: '/plugins/webvella-admin/entity-list-manage.view.html',
                    controllerAs: 'contentData'
                }
            },
            resolve: {
                resolvedCurrentEntityMeta: resolveCurrentEntityMeta,
                resolvedViewLibrary: resolveViewLibrary,
                resolvedCurrentEntityList: resolveCurrentEntityList
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

    resolveCurrentEntityList.$inject = ['$q', '$log', 'webvellaAdminService', '$stateParams', '$state', '$timeout'];
    /* @ngInject */
    function resolveCurrentEntityList($q, $log, webvellaAdminService, $stateParams, $state, $timeout) {
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

        webvellaAdminService.getEntityRecordsList($stateParams.listName, $stateParams.entityName, successCallback, errorCallback);

        // Return
        $log.debug('webvellaAdmin>entity-records-list>resolveEntityRecordsList END state.resolved');
        return defer.promise;
    }

    resolveViewLibrary.$inject = ['$q', '$log', 'webvellaAdminService', '$stateParams', '$state', '$timeout'];
    /* @ngInject */
    function resolveViewLibrary($q, $log, webvellaAdminService, $stateParams, $state, $timeout) {
        $log.debug('webvellaAdmin>entity-views>resolveViewAvailableItems BEGIN state.resolved');
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

        webvellaAdminService.getEntityViewLibrary($stateParams.viewName, $stateParams.entityName, successCallback, errorCallback);

        // Return
        $log.debug('webvellaAdmin>entity-views>resolveViewAvailableItems END state.resolved');
        return defer.promise;
    }
    //#endregion

    //#region << Controller >> ///////////////////////////////
    controller.$inject = ['$scope', '$log', '$rootScope', '$state', 'pageTitle', 'resolvedCurrentEntityMeta', '$modal', 'resolvedCurrentEntityList', 'resolvedViewLibrary'];
    /* @ngInject */
    function controller($scope, $log, $rootScope, $state, pageTitle, resolvedCurrentEntityMeta, $modal, resolvedCurrentEntityList, resolvedViewLibrary) {
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

        //#region << Initialize the list >>
        contentData.list = resolvedCurrentEntityList;
        //#endregion

        //#region << Initialize the library >>
        contentData.fieldsLibrary = {};
        contentData.fieldsLibrary.items = [];
        for (var i = 0; i < resolvedViewLibrary.items.length; i++) {
        	if (resolvedViewLibrary.items[i].type === "field" || resolvedViewLibrary.items[i].type === "fieldFromRelation" || resolvedViewLibrary.items[i].type === "view") {
                contentData.fieldsLibrary.items.push(resolvedViewLibrary.items[i])
            }
        }

        contentData.fieldsLibrary.items = contentData.fieldsLibrary.items.sort(function (a, b) {
        	if (a.type < b.type) return -1;
        	if (a.type > b.type) return 1;
        	return 0;
        });
        contentData.fieldsLibrary.items.forEach(function (item) {
        	var search = "";
        	if (item.type != null) {
        		search += item.type + " ";
        	}
        	if (item.tag != null) {
        		search += item.tag + " ";
        	}
        	if (item.fieldName != null) {
        		search += item.fieldName + " ";
        	}
        	if (item.fieldLabel != null) {
        		search += item.fieldLabel + " ";
        	}
        	if (item.entityName != null) {
        		search += item.entityName + " ";
        	}
        	if (item.entityLabel != null) {
        		search += item.entityLabel + " ";
        	}
        	if (item.viewName != null) {
        		search += item.viewName + " ";
        	}
        	if (item.viewLabel != null) {
        		search += item.viewLabel + " ";
        	}
        	if (item.listName != null) {
        		search += item.listName + " ";
        	}
        	if (item.listLabel != null) {
        		search += item.listLabel + " ";
        	}
        	if (item.entityLabelPlural != null) {
        		search += item.entityLabelPlural + " ";
        	}
        	item.search = search;
        });


        //#endregion

        //#region << Logic >>
        contentData.moveToColumns = function (item,index) {
            //Add Item at the end of the columns list
            contentData.list.items.push(item);
            //Remove from library
            contentData.fieldsLibrary.items.splice(index, 1);
        }

        contentData.moveToLibrary = function (item, index) {
            //Add Item at the end of the columns list
            contentData.fieldsLibrary.items.push(item);
            //Remove from library
            contentData.list.items.splice(index, 1);
        }

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
