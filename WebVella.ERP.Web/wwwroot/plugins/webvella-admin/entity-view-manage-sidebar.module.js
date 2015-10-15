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
            	resolvedEntityRelationsList: resolveEntityRelationsList,
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
    			content: messageContent,
    			timeout: 7000
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

    resolveEntityRelationsList.$inject = ['$q', '$log', 'webvellaAdminService', '$stateParams', '$state', '$timeout'];
	/* @ngInject */
    function resolveEntityRelationsList($q, $log, webvellaAdminService, $stateParams, $state, $timeout) {
    	$log.debug('webvellaAdmin>entity-details> BEGIN state.resolved ' + moment().format('HH:mm:ss SSSS'));
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

    	webvellaAdminService.getRelationsList(successCallback, errorCallback);

    	// Return
    	$log.debug('webvellaAdmin>entity-details> END state.resolved ' + moment().format('HH:mm:ss SSSS'));
    	return defer.promise;
    }


    //#endregion

    //#region << Controller >> ////////////////////////////
    controller.$inject = ['$scope', '$log', '$rootScope', '$state', '$stateParams', 'pageTitle', '$uibModal',
                            'resolvedCurrentEntityMeta', 'webvellaAdminService', 'ngToast', 'resolvedViewLibrary','resolvedEntityRelationsList'];
    /* @ngInject */
    function controller($scope, $log, $rootScope, $state, $stateParams, pageTitle, $uibModal,
                        resolvedCurrentEntityMeta, webvellaAdminService, ngToast, resolvedViewLibrary, resolvedEntityRelationsList) {
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

		contentData.relationsList = fastCopy(resolvedEntityRelationsList);
        contentData.tempLibrary = {};
        contentData.tempLibrary.items = fastCopy(resolvedViewLibrary);
        contentData.tempLibrary.items = contentData.tempLibrary.items.sort(function (a, b) {
        	if (a.type < b.type) return -1;
        	if (a.type > b.type) return 1;
        	return 0;
        });
        contentData.library = {};
        contentData.library.relations = [];
        contentData.library.items = [];
        contentData.tempLibrary.items.forEach(function (item) {
        	//Initially remove all items that are from relation or relationOptions
        	switch (item.type) {
        		//case "field":
        		//	contentData.library.items.push(item);
        		//	break;
        		case "view":
        			if (item.viewId != contentData.view.id) {
        				contentData.library.items.push(item);
        			}
        			break;
        		case "list":
        			contentData.library.items.push(item);
        			break;
        		case "relationOptions":
        			item.addedToLibrary = false;
        			item.sameOriginTargetEntity = false;
        			for (var r = 0; r < contentData.relationsList.length; r++) {
        				if (item.relationName == contentData.relationsList[r].name && contentData.relationsList[r].originEntityId == contentData.relationsList[r].targetEntityId) {
        					item.sameOriginTargetEntity = true;
        				}
        			}
        			contentData.library.relations.push(item);
        			break;
        	}
        });
        function sortLibrary() {
        	contentData.library.items = contentData.library.items.sort(function (a, b) {
        		if (a.dataName < b.dataName) return -1;
        		if (a.dataName > b.dataName) return 1;
        		return 0;
        	});
        }
        sortLibrary();
        contentData.originalLibrary = fastCopy(contentData.library.items);


    	//Extract the direction change information from the view if present
        for (var k = 0; k < contentData.view.relationOptions.length; k++) {
        	for (var m = 0; m < contentData.library.relations.length; m++) {
        		if (contentData.view.relationOptions[k].relationName == contentData.library.relations[m].relationName) {
        			contentData.library.relations[m].direction = contentData.view.relationOptions[k].direction;
        		}

        	}

        }

        contentData.library.relations = contentData.library.relations.sort(function (a, b) {
        	if (a.relationName < b.relationName) return -1;
        	if (a.relationName > b.relationName) return 1;
        	return 0;
        });


        //#endregion

        //#region << Drag & Drop Management >>

        function executeDragViewChange(eventObj) {
            //#region << 1.Define functions >>
            var moveSuccess, moveFailure, successCallback, errorCallback;

            function successCallback(response) {
                if (response.success) {
                    ngToast.create({
                        className: 'success',
                        content: '<span class="go-green">Success:</span> ' + response.message
                    });
                    contentData.library.items = fastCopy(contentData.originalLibrary);
                    contentData.view.sidebar.items = response.object.sidebar.items;
                }
                else {
                	errorCallback(response);
                	moveFailure();
                }
            }

            function errorCallback(response) {
                ngToast.create({
                    className: 'error',
                    content: '<span class="go-red">Error:</span> ' + response.message,
					timeout:7000
                });
            }
            //#endregion

        	//1. Clean contentData.view from system properties like $$hashKey
            contentData.view.sidebar.items = fastCopy(contentData.view.sidebar.items);
        	//contentData.view = angular.fromJson(angular.toJson(contentData.view));
        	////2. Call the service
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

        contentData.libraryDragControlListeners = {
        	accept: function (sourceItemHandleScope, destSortableScope) {
        		if (sourceItemHandleScope.itemScope.element[0].id != "library" && destSortableScope.element[0].id == "library") {
        			return false;
        		}
        		return true;
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

        contentData.dragItemRemove = function (index) {

        	function successCallback(response) {
        		ngToast.create({
        			className: 'success',
        			content: '<span class="go-green">Success:</span> ' + response.message
        		});
        	}

        	function errorCallback(response) {
        		ngToast.create({
        			className: 'error',
        			content: '<span class="go-red">Error:</span> ' + response.message,
        			timeout: 7000
        		});
        		$state.reload();
        	}

        	contentData.view.sidebar.items.splice(index, 1);
        	webvellaAdminService.updateEntityView(contentData.view, contentData.entity.name, successCallback, errorCallback);
        }



    	//#endregion

    	//#region << Relations >>

        contentData.changeRelationDirection = function (relation) {
        	if (relation.direction == "origin-target") {
        		relation.direction = "target-origin";
        	}
        	else {
        		relation.direction = "origin-target";
        	}
        	contentData.view.relationOptions = [];

        	for (var i = 0; i < contentData.library.relations.length; i++) {
        		var relation = fastCopy(contentData.library.relations[i]);
        		delete relation.addedToLibrary;
        		delete relation.sameOriginTargetEntity;
        		contentData.view.relationOptions.push(relation);
        	}

        	function successCallback(response) {
        		ngToast.create({
        			className: 'success',
        			content: '<span class="go-green">Success:</span> ' + response.message
        		});
        	}

        	function errorCallback(response) {
        		ngToast.create({
        			className: 'error',
        			content: '<span class="go-red">Error:</span> ' + response.message,
        			timeout: 7000
        		});
        		//Undo change
        		for (var j = 0; j < contentData.library.relations.length; j++) {
        			if (contentData.library.relations[j].relationName == relation.relationName) {
        				if (contentData.library.relations[j].direction == "origin-target") {
        					contentData.library.relations[j].direction = "target-origin";
        				}
        				else {
        					contentData.library.relations[j].direction = "origin-target";
        				}
        			}
        		}
        	}
        	webvellaAdminService.updateEntityView(contentData.view, contentData.entity.name, successCallback, errorCallback);
        }

        contentData.toggleRelationToLibrary = function (relation) {
        	if (!relation.addedToLibrary) {
        		contentData.tempLibrary.items.forEach(function (item) {
        			if (item.relationName && item.relationName == relation.relationName) {
        				switch (item.type) {
        					//case "fieldFromRelation":
        					//	contentData.library.items.push(item);
        					//	break;
        					case "viewFromRelation":
        						if (item.viewId != contentData.view.id) {
        							contentData.library.items.push(item);
        						}
        						break;
        					case "listFromRelation":
        						contentData.library.items.push(item);
        						break;
        				}
        			}
        		});
        		relation.addedToLibrary = true;
        	}
        	else {
        		var tempRelationChangeLibrary = [];
        		contentData.library.items.forEach(function (item) {
        			if (!item.relationName) {
        				tempRelationChangeLibrary.push(item);
        			}
        			else if (item.relationName != relation.relationName) {
        				tempRelationChangeLibrary.push(item);
        			}
        		});
        		contentData.library.items = tempRelationChangeLibrary;
        		relation.addedToLibrary = false;
        	}
        	sortLibrary();
        }

        contentData.getRelationType = function (relationId) {
        	for (var i = 0; i < contentData.relationsList.length; i++) {
        		if (contentData.relationsList[i].id == relationId) {
        			return contentData.relationsList[i].relationType;
        		}
        	}
        	return 0;
        }

    	//#endregion

        $log.debug('webvellaAdmin>entity-details> END controller.exec ' + moment().format('HH:mm:ss SSSS'));

    }
    //#endregion


})();
 