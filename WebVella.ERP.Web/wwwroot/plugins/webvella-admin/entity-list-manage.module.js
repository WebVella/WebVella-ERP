/* entity-lists.module.js */

/**
* @desc this module manages the entity record lists in the admin screen
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminEntityListManageController', controller)
        .controller('DeleteListModalController', deleteListModalController);

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
            	checkedAccessPermission: checkAccessPermission,
                resolvedCurrentEntityMeta: resolveCurrentEntityMeta,
                resolvedViewLibrary: resolveViewLibrary,
                resolvedCurrentEntityList: resolveCurrentEntityList,
                resolvedEntityRelationsList: resolveEntityRelationsList,
            },
            data: {

            }
        });
    };


	//#region << Resolve Functions >>/////////////////////////
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

        webvellaAdminService.getEntityMeta($stateParams.entityName, successCallback, errorCallback);

        // Return
        $log.debug('webvellaAdmin>entity-details> END state.resolved ' + moment().format('HH:mm:ss SSSS'));
        return defer.promise;
    }

    resolveCurrentEntityList.$inject = ['$q', '$log', 'webvellaAdminService', '$stateParams', '$state', '$timeout'];
    /* @ngInject */
    function resolveCurrentEntityList($q, $log, webvellaAdminService, $stateParams, $state, $timeout) {
    	$log.debug('webvellaAdmin>entity-records-list>resolveEntityRecordsList BEGIN state.resolved ' + moment().format('HH:mm:ss SSSS'));
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

        webvellaAdminService.getEntityList($stateParams.listName, $stateParams.entityName, successCallback, errorCallback);

        // Return
        $log.debug('webvellaAdmin>entity-records-list>resolveEntityRecordsList END state.resolved ' + moment().format('HH:mm:ss SSSS'));
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

    //#region << Controller >> ///////////////////////////////
    controller.$inject = ['$scope', '$log', '$rootScope', '$state', 'ngToast', 'pageTitle', 'resolvedCurrentEntityMeta', '$modal', 'resolvedCurrentEntityList',
						'resolvedViewLibrary', 'webvellaAdminService', 'resolvedEntityRelationsList'];
    /* @ngInject */
    function controller($scope, $log, $rootScope, $state, ngToast, pageTitle, resolvedCurrentEntityMeta, $modal, resolvedCurrentEntityList,
						resolvedViewLibrary, webvellaAdminService, resolvedEntityRelationsList) {
    	$log.debug('webvellaAdmin>entity-records-list> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
        /* jshint validthis:true */
        var contentData = this;
        //#region << Initialize the current entity >>
        contentData.entity = resolvedCurrentEntityMeta;
    	//#endregion

    	//Awesome font icon names array 
        contentData.icons = getFontAwesomeIconNames();

        //#region << Update page title & hide the side menu >>
        contentData.pageTitle = "Entity Details | " + pageTitle;
        $rootScope.$emit("application-pageTitle-update", contentData.pageTitle);
        //Hide Sidemenu
        $rootScope.$emit("application-body-sidebar-menu-isVisible-update", false);
        $log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted ' + moment().format('HH:mm:ss SSSS'));
        $scope.$on("$destroy", function () {
            $rootScope.$emit("application-body-sidebar-menu-isVisible-update", true);
            $log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted ' + moment().format('HH:mm:ss SSSS'));
        });
        //#endregion

        //#region << List types >>
        contentData.listTypeOptions = [
            {
            	key: "general",
                value: "general"
            },
            {
            	key: "lookup",
                value: "lookup"
            }
        ];
        //#endregion

        //#region << Initialize the list >>
        contentData.list = fastCopy(resolvedCurrentEntityList);
        contentData.relationsList = fastCopy(resolvedEntityRelationsList);

        contentData.defaultFieldName = null;
        function calculateDefaultSearchFieldName() {
        	var name = null;
        	for (var k = 0; k < contentData.list.columns.length; k++) {
        		if (contentData.list.columns[k].type === "field") {
        			name = contentData.list.columns[k].meta.name;
        			break;
        		}
        	}
        	contentData.defaultFieldName = name;
        }
        calculateDefaultSearchFieldName();

        function patchFieldSuccessCallback(response) {
        	ngToast.create({
        		className: 'success',
        		content: '<span class="go-green">Success:</span> ' + response.message
        	});
        	webvellaAdminService.regenerateAllAreaSubscriptions();
        }

        function patchSuccessCallback(response) {
        	ngToast.create({
        		className: 'success',
        		content: '<span class="go-green">Success:</span> ' + response.message
        	});
        }
        function patchErrorCallback(response) {
        	ngToast.create({
        		className: 'error',
        		content: '<span class="go-red">Error:</span> ' + response.message
        	});
        }

        contentData.fieldUpdate = function (fieldName, data) {
        	var postObj = {};
        	postObj[fieldName] = data;
        	webvellaAdminService.patchEntityList(postObj, contentData.list.name, contentData.entity.name, patchFieldSuccessCallback, patchErrorCallback)
        }

        contentData.updateColumns = function () {
        	var postObj = {};
        	postObj.columns = contentData.list.columns;
        	calculateDefaultSearchFieldName();
        	webvellaAdminService.patchEntityList(postObj, contentData.list.name, contentData.entity.name, patchSuccessCallback, patchErrorCallback)
        }

        contentData.updateQuery = function () {
        	var postObj = {};
        	postObj.query = contentData.list.query;
        	webvellaAdminService.patchEntityList(postObj, contentData.list.name, contentData.entity.name, patchSuccessCallback, patchErrorCallback)
        }

        contentData.updateSorts = function () {
        	var postObj = {};
        	postObj.sorts = contentData.list.sorts;
        	webvellaAdminService.patchEntityList(postObj, contentData.list.name, contentData.entity.name, patchSuccessCallback, patchErrorCallback)
        }

        //#endregion

        //#region << Initialize the library >>
        contentData.tempLibrary = {};
        contentData.tempLibrary.items = fastCopy(resolvedViewLibrary);
        contentData.library = {};
        contentData.library.relations = [];
        contentData.library.items = [];
        contentData.tempLibrary.items.forEach(function (item) {
        	//Initially remove all items that are from relation or relationOptions
        	if (item.type != "relationOptions" && !item.relationName && item.type != "html") {
        		contentData.library.items.push(item);
        	}
        	else if (item.type == "relationOptions") {
        		item.addedToLibrary = false;
        		item.sameOriginTargetEntity = false;
        		for (var r = 0; r < contentData.relationsList.length; r++) {
        			if (item.relationName == contentData.relationsList[r].name && contentData.relationsList[r].originEntityId == contentData.relationsList[r].targetEntityId) {
        				item.sameOriginTargetEntity = true;
        			}
        		}
        		contentData.library.relations.push(item);
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

    	//Extract the direction change information from the list if present
        for (var k = 0; k < contentData.list.relationOptions.length; k++) {
        	for (var m = 0; m < contentData.library.relations.length; m++) {
        		if (contentData.list.relationOptions[k].relationName == contentData.library.relations[m].relationName) {
        			contentData.library.relations[m].direction = contentData.list.relationOptions[k].direction;
        		}

        	}

        }

        contentData.library.relations = contentData.library.relations.sort(function (a, b) {
        	if (a.relationName < b.relationName) return -1;
        	if (a.relationName > b.relationName) return 1;
        	return 0;
        });


        //#endregion

        //#region << Logic >>
        contentData.moveToColumns = function (item,index) {
            //Add Item at the end of the columns list
        	contentData.list.columns.push(item);
            //Remove from library
        	contentData.library.items.splice(index, 1);
        	contentData.updateColumns();
        }
        contentData.moveToLibrary = function (item, index) {
            //Add Item at the end of the columns list
            contentData.library.items.push(item);
            //Remove from library
            contentData.list.columns.splice(index, 1);

            contentData.library.items = contentData.library.items.sort(function (a, b) {
            	if (a.type < b.type) return -1;
            	if (a.type > b.type) return 1;
            	return 0;
            });
            contentData.updateColumns();
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
        		//executeDragViewChange(eventObj);
        		contentData.updateColumns();
        	},
        	orderChanged: function (eventObj) {
        		//Item is moved within the same column
        		//executeDragViewChange(eventObj);
        		contentData.updateColumns();
        	}
        };
  
    	//#endregion

    	//#region << Query >>
    	//Attach guid to each area and rule so we can find it when managed
        function findInTreeById(startElement, matchingId) {
        	if (startElement.id == matchingId) {
        		return startElement;
        	} else if (startElement.subQueries != null) {
        		var result = null;
        		for (i = 0; result == null && i < startElement.subQueries.length; i++) {
        			result = searchTree(startElement.subQueries[i], matchingId);
        		}
        		return result;
        	}
        	return null;
        }
        function deleteInTreeById(startElement, matchingId) {
        	if (startElement.id == matchingId) {
        		return startElement;
        	} else if (startElement.subQueries != null) {
        		var result = null;
        		for (i = 0; result == null && i < startElement.subQueries.length; i++) {
        			result = searchTree(startElement.subQueries[i], matchingId);
        		}
        		return result;
        	}
        	return null;
        }
        contentData.getIncludeFile = function (query) {
        switch(query.queryType) {
        	case "EQ":
        		return 'queryRule.html';
        	case "NOT":
        		return 'queryRule.html';
        	case "LT":
        		return 'queryRule.html';
        	case "LTE":
        		return 'queryRule.html';
        	case "GT":
        		return 'queryRule.html';
        	case "GTE":
        		return 'queryRule.html';
        	case "CONTAINS":
        		return 'queryRule.html';
        	case "STARTSWITH":
        		return 'queryRule.html';
        	case "AND":
        		return 'querySection.html';
        	case "OR":
        		return 'querySection.html';
	        }
        }
        contentData.AddRule = function (query) {
        	var subquery = {
        		"queryType": "EQ",
        		"fieldName": "id",
        		"fieldValue": "",
        		"subQueries": []
        	};
        	query.subQueries.push(subquery);
        	contentData.updateQuery();
        }
        contentData.AddSection = function (query) {
        	var subquery = {
        		"queryType": "AND",
        		"fieldName": null,
        		"fieldValue": null,
        		"subQueries": [
					{
						"queryType": "EQ",
						"fieldName": "id",
						"fieldValue": "",
						"subQueries": []
					}
        		]
        	};
        	if (query != null) {
        		query.subQueries.push(subquery);
        	}
        	else {
        		contentData.list.query = subquery;
        	}
        	contentData.updateQuery();
        }
        contentData.DeleteItem = function (parent, index) {
        	if (parent != null) {
        		parent.subQueries.splice(index, 1);
        	}
        	else {
        		contentData.list.query = {};
        		contentData.list.query = null;
        	}
        	contentData.updateQuery();
        }
        contentData.DeleteSortRule = function (index) {
        	contentData.list.sorts.splice(index, 1);
        	if(contentData.list.sorts.length == 0) {
        		contentData.list.sorts = null;
        	}
        	contentData.updateSorts();
        }
        contentData.AddSortRule = function () {
        	if (contentData.list.sorts == null) {
        		contentData.list.sorts = [];
        	}
        	var subrule = {
        		"fieldName": "id",
        		"sortType": "ascending"
        	};
        	contentData.list.sorts.push(subrule);
        	contentData.updateSorts();
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
        	contentData.list.relationOptions = [];

        	for (var i = 0; i < contentData.library.relations.length; i++) {
        		var relation = fastCopy(contentData.library.relations[i]);
        		delete relation.addedToLibrary;
        		delete relation.sameOriginTargetEntity;
        		contentData.list.relationOptions.push(relation);
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

        	webvellaAdminService.updateEntityList(contentData.list, contentData.entity.name, successCallback, errorCallback);
        }

        contentData.toggleRelationToLibrary = function (relation) {
        	if (!relation.addedToLibrary) {
        		contentData.tempLibrary.items.forEach(function (item) {
        			if (item.relationName && item.relationName == relation.relationName) {
        				contentData.library.items.push(item);
        			}
        		});
        		relation.addedToLibrary = true;
        	}
        	else {
        		var tempRelationChangeLibrary = [];
        		contentData.library.items.forEach(function (item) {
        			if (!item.relationName || item.relationName != relation.relationName) {
        				tempRelationChangeLibrary.push(item);
        			}
        		});
        		contentData.library.items = tempRelationChangeLibrary;
        		relation.addedToLibrary = false;
        	}
        	sortLibrary();
        }


    	//#endregion

        //Delete list
        contentData.deleteListModal = function () {
            var modalInstance = $modal.open({
                animation: false,
                templateUrl: 'deleteListModal.html',
                controller: 'DeleteListModalController',
                controllerAs: "popupData",
                size: "",
                resolve: {
                    parentData: function () { return contentData; }
                }
            });
        }


        $log.debug('webvellaAdmin>entity-records-list> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
    }
    //#endregion

    //#region << Modal Controllers >>
    deleteListModalController.$inject = ['parentData', '$modalInstance', '$log', 'webvellaAdminService', 'webvellaRootService', 'ngToast', '$timeout', '$state'];

    /* @ngInject */
    function deleteListModalController(parentData, $modalInstance, $log, webvellaAdminService, webvellaRootService, ngToast, $timeout, $state) {
    	$log.debug('webvellaAdmin>entities>deleteListModal> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
        /* jshint validthis:true */
        var popupData = this;
        popupData.parentData = parentData;

        popupData.ok = function () {

            webvellaAdminService.deleteEntityList(popupData.parentData.list.name, popupData.parentData.entity.name, successCallback, errorCallback);

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
                $state.go("webvella-admin-entity-lists", { entityName: popupData.parentData.entity.name }, { reload: true });
            }, 0);
        }

        function errorCallback(response) {
            popupData.hasError = true;
            popupData.errorMessage = response.message;


        }
        $log.debug('webvellaAdmin>entities>deleteListModal> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
    };

    //#endregion
})();
