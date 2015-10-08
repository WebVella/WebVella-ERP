/* entity-view-manage.module.js */

/**
* @desc this module manages a single entity view in the admin screen
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminEntityViewManageSidebarController', controller);

    //#region << Configuration >> /////////////////////////
    config.$inject = ['$stateProvider'];
    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-admin-entity-view-manage-sidebar', {
            parent: 'webvella-admin-base',
            url: '/entities/:entityName/views/:viewName/sidebar',
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
                	controller: 'WebVellaAdminEntityViewManageSidebarController',
                    templateUrl: '/plugins/webvella-admin/entity-view-manage-sidebar.view.html',
                    controllerAs: 'contentData'
                }
            },
            resolve: {
            	checkedAccessPermission: checkAccessPermission,
                resolvedCurrentEntityMeta: resolveCurrentEntityMeta,
                resolvedViewLibrary: resolveViewLibrary
            },
            data: {

            }
        });
    };
    //#endregion

	//#region << Resolve >> ///////////////////////////////
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
    			content: messageContent
    		});
    		defer.reject("No access");
    	}

    	$log.debug('webvellaAreas>entities> BEGIN check access permission ' + moment().format('HH:mm:ss SSSS'));
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
            	defer.reject(response.message);
            }
        }

        webvellaAdminService.getEntityMeta($stateParams.entityName, successCallback, errorCallback);

        // Return
        $log.debug('webvellaAdmin>entity-details> END resolveCurrentEntityMeta state.resolved ' + moment().format('HH:mm:ss SSSS'));
        return defer.promise;
    }

    resolveViewLibrary.$inject = ['$q', '$log', 'webvellaAdminService', '$stateParams', '$state', '$timeout'];
    /* @ngInject */
    function resolveViewLibrary($q, $log, webvellaAdminService, $stateParams, $state, $timeout) {
    	$log.debug('webvellaAdmin>entity-views>resolveViewAvailableItems BEGIN state.resolved ' + moment().format('HH:mm:ss SSSS'));
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
            	//Remove the current view from the list to avoid loop
            	var libraryWithoutTheCurrentView = [];
            	for (var i = 0; i < response.object.length; i++) {
            		if (response.object[i].type != "view" && response.object[i].type != "field"
						&& response.object[i].type != "fieldFromRelation" && response.object[i].type != "html") {
            			libraryWithoutTheCurrentView.push(response.object[i]);
            		}
            		else if (response.object[i].type == "view" && response.object[i].viewName != $stateParams.viewName) {
            			libraryWithoutTheCurrentView.push(response.object[i]);
            		}
            	}
				

            	defer.resolve(libraryWithoutTheCurrentView);
            }
        }

        function errorCallback(response) {
            if (response.object == null) {
                $timeout(function () {
                    $state.go("webvella-root-not-found");
                }, 0);
            }
            else {
            	defer.reject(response.message);
            }
        }

        webvellaAdminService.getEntityViewLibrary($stateParams.entityName, successCallback, errorCallback);

        // Return
        $log.debug('webvellaAdmin>entity-views>resolveViewAvailableItems END state.resolved ' + moment().format('HH:mm:ss SSSS'));
        return defer.promise;
    }

    //#endregion

    //#region << Controller >> ////////////////////////////
    controller.$inject = ['$scope', '$log', '$rootScope', '$state','$stateParams', 'pageTitle', '$modal',
                            'resolvedCurrentEntityMeta', 'webvellaAdminService', 'ngToast', 'resolvedViewLibrary'];
    /* @ngInject */
    function controller($scope, $log, $rootScope, $state,$stateParams, pageTitle, $modal,
                        resolvedCurrentEntityMeta, webvellaAdminService, ngToast, resolvedViewLibrary) {
    	$log.debug('webvellaAdmin>entity-details> START controller.exec ' + moment().format('HH:mm:ss SSSS'));

        /* jshint validthis:true */
        var contentData = this;
        //#region << Initialize Current Entity >>
        contentData.entity = fastCopy(resolvedCurrentEntityMeta);
        //#endregion

        //#region << Update page title & Hide side menu>>
        contentData.pageTitle = "Entity Views | " + pageTitle;
        $rootScope.$emit("application-pageTitle-update", contentData.pageTitle);
        //Hide side menu
        $rootScope.$emit("application-body-sidebar-menu-isVisible-update", false);
        $log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted ' + moment().format('HH:mm:ss SSSS'));
        $scope.$on("$destroy", function () {
            $rootScope.$emit("application-body-sidebar-menu-isVisible-update", true);
            $log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted ' + moment().format('HH:mm:ss SSSS'));
        });
        //#endregion

    	//#region << Initialize View and Content Region >>
        contentData.view = {};
        for (var i = 0; i < contentData.entity.recordViews.length; i++) {
        	if (contentData.entity.recordViews[i].name == $stateParams.viewName) {
        		contentData.view = fastCopy(contentData.entity.recordViews[i]);
        	}
        }

    	//Get fields already used in the view so they need to be removed from the library
        var usedItemsArray = contentData.view.sidebar.items;

        contentData.tempLibrary = {};
        contentData.tempLibrary.items = fastCopy(resolvedViewLibrary);
        contentData.tempLibrary.items = contentData.tempLibrary.items.sort(function (a, b) {
        	if (a.type < b.type) return -1;
        	if (a.type > b.type) return 1;
        	return 0;
        });
        contentData.library = {};
        contentData.library.items = [];
        contentData.tempLibrary.items.forEach(function (item) {
        	var notUsed = true;
        	for (var k = 0; k < usedItemsArray.length; k++) {
				if (item.type === "view" && usedItemsArray[k].type === "view" && item.viewId == usedItemsArray[k].viewId) {
        			notUsed = false;
        		}
        		else if (item.type === "viewFromRelation" && usedItemsArray[k].type === "viewFromRelation" && item.viewId == usedItemsArray[k].viewId) {
        			notUsed = false;
        		}
        		else if (item.type === "list" && usedItemsArray[k].type === "list" && item.listId == usedItemsArray[k].listId) {
        			notUsed = false;
        		}
        		else if (item.type === "listFromRelation" && usedItemsArray[k].type === "listFromRelation" && item.listId == usedItemsArray[k].listId) {
        			notUsed = false;
        		}
        	}
        	if (notUsed) {
        		//var search = "";
        		//if (item.type != null) {
        		//	search += item.type + " ";
        		//}
        		//if (item.tag != null) {
        		//	search += item.tag + " ";
        		//}
        		//if (item.fieldName != null) {
        		//	search += item.fieldName + " ";
        		//}
        		//if (item.fieldLabel != null) {
        		//	search += item.fieldLabel + " ";
        		//}
        		//if (item.entityName != null) {
        		//	search += item.entityName + " ";
        		//}
        		//if (item.entityLabel != null) {
        		//	search += item.entityLabel + " ";
        		//}
        		//if (item.viewName != null) {
        		//	search += item.viewName + " ";
        		//}
        		//if (item.viewLabel != null) {
        		//	search += item.viewLabel + " ";
        		//}
        		//if (item.listName != null) {
        		//	search += item.listName + " ";
        		//}
        		//if (item.listLabel != null) {
        		//	search += item.listLabel + " ";
        		//}
        		//if (item.entityLabelPlural != null) {
        		//	search += item.entityLabelPlural + " ";
        		//}
        		//item.search = search;
        		contentData.library.items.push(item);
        	}
        });



        //#endregion

        //#region << Drag & Drop Management >>

        function executeDragViewChange(eventObj) {
            //#region << 1.Define functions >>
            var moveSuccess, moveFailure, successCallback, errorCallback;

            moveSuccess = function () {};
            moveFailure = function () {
                eventObj.dest.sortableScope.removeItem(eventObj.dest.index);
                eventObj.source.itemScope.sortableScope.insertItem(eventObj.source.index, eventObj.source.itemScope.item);
            };

            function successCallback(response) {
                if (response.success) {
                    ngToast.create({
                        className: 'success',
                        content: '<span class="go-green">Success:</span> ' + response.message
                    });

                    moveSuccess();
                }
                else {
                	errorCallback(response);
                	moveFailure();
                }
            }

            function errorCallback(response) {
                ngToast.create({
                    className: 'error',
                    content: '<span class="go-red">Error:</span> ' + response.message
                });
                moveFailure();
            }
            //#endregion

            //2. Call the service
            webvellaAdminService.updateEntityView(contentData.view, contentData.entity.name, successCallback, errorCallback);
        }

        contentData.dragControlListeners = {
            accept: function (sourceItemHandleScope, destSortableScope) {
                //for (var i = 0; i < destSortableScope.modelValue.length; i++) {
                //    if (destSortableScope.modelValue[i].id == sourceItemHandleScope.item.id) {
                //        return false;
                //        break;
                //    }
                //}

                return true
            },
            itemMoved: function (eventObj) {
                //Item is moved from one column to another
                executeDragViewChange(eventObj);
            },
            orderChanged: function (eventObj) {
                //Item is moved within the same column
                executeDragViewChange(eventObj);
            }
        };

        //#endregion
        $log.debug('webvellaAdmin>entity-details> END controller.exec ' + moment().format('HH:mm:ss SSSS'));

    }
    //#endregion


})();
