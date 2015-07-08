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

        webvellaAdminService.getEntityList($stateParams.listName, $stateParams.entityName, successCallback, errorCallback);

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

        webvellaAdminService.getEntityViewLibrary($stateParams.entityName, successCallback, errorCallback);

        // Return
        $log.debug('webvellaAdmin>entity-views>resolveViewAvailableItems END state.resolved');
        return defer.promise;
    }
    //#endregion

    //#region << Controller >> ///////////////////////////////
    controller.$inject = ['$scope', '$log', '$rootScope', '$state', 'ngToast', 'pageTitle', 'resolvedCurrentEntityMeta', '$modal', 'resolvedCurrentEntityList',
						'resolvedViewLibrary', 'webvellaAdminService'];
    /* @ngInject */
    function controller($scope, $log, $rootScope, $state, ngToast, pageTitle, resolvedCurrentEntityMeta, $modal, resolvedCurrentEntityList,
						resolvedViewLibrary, webvellaAdminService) {
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
        contentData.list = angular.copy(resolvedCurrentEntityList);

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
        	webvellaAdminService.patchEntityList(postObj, contentData.list.name, contentData.entity.name, patchSuccessCallback, patchErrorCallback)
        }

        contentData.updateColumns = function () {
        	var postObj = {};
        	postObj.columns = contentData.list.columns;
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
        contentData.fieldsLibrary = {};
        contentData.fieldsLibrary.items = [];

        contentData.onlyFieldsLibrary = {};
        contentData.onlyFieldsLibrary.items = [];

        contentData.tempFieldsLibrary = {};
        contentData.tempFieldsLibrary.items = [];

        for (var i = 0; i < resolvedViewLibrary.length; i++) {
        	if (resolvedViewLibrary[i].type === "field" || resolvedViewLibrary[i].type === "fieldFromRelation" || resolvedViewLibrary[i].type === "view"
			 || resolvedViewLibrary[i].type === "html") {
        		contentData.tempFieldsLibrary.items.push(resolvedViewLibrary[i]);
        		if (resolvedViewLibrary[i].type === "field") {
        			contentData.onlyFieldsLibrary.items.push(resolvedViewLibrary[i]);
        		}
            }
        }

        contentData.tempFieldsLibrary.items = contentData.tempFieldsLibrary.items.sort(function (a, b) {
        	if (a.type < b.type) return -1;
        	if (a.type > b.type) return 1;
        	return 0;
        });

    	//Get fields already used in the view so they need to be removed from the library
        var usedItemsArray = [];
        for (var j = 0; j < contentData.list.columns.length; j++) {
        	usedItemsArray.push(contentData.list.columns[j]);
        }
        contentData.tempFieldsLibrary.items.forEach(function (item) {
        	var notUsed = true;
        	for (var k = 0; k < usedItemsArray.length; k++) {
        		if (item.type === "field" && usedItemsArray[k].type === "field"
						&& item.fieldId == usedItemsArray[k].fieldId) {
        			notUsed = false;
        		}
        		else if (item.type === "fieldFromRelation" && usedItemsArray[k].type === "fieldFromRelation"
						&& item.fieldId == usedItemsArray[k].fieldId && item.relationId == usedItemsArray[k].relationId) {
        			notUsed = false;
        		}
        		else if (item.type === "view" && usedItemsArray[k].type === "view" && item.viewId == usedItemsArray[k].viewId) {
        			notUsed = false;
        		}
        	}

        	if (notUsed) {
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
        	contentData.fieldsLibrary.items.push(item);
		}
        });
        //#endregion

        //#region << Logic >>
        contentData.moveToColumns = function (item,index) {
            //Add Item at the end of the columns list
        	contentData.list.columns.push(item);
            //Remove from library
        	contentData.fieldsLibrary.items.splice(index, 1);
        	contentData.updateColumns();
        }
        contentData.moveToLibrary = function (item, index) {
            //Add Item at the end of the columns list
            contentData.fieldsLibrary.items.push(item);
            //Remove from library
            contentData.list.columns.splice(index, 1);

            contentData.fieldsLibrary.items = contentData.fieldsLibrary.items.sort(function (a, b) {
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
        		"fieldName": "",
        		"fieldValue": "",
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
