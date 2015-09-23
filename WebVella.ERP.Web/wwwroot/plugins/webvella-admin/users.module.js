/* areas-lists.module.js */

/**
* @desc this module manages the entity record lists in the admin screen
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminUsersController', controller)
        .controller('ManageUserModalController', manageUserController);
   
    ///////////////////////////////////////////////////////
    /// Configuration
    ///////////////////////////////////////////////////////

    config.$inject = ['$stateProvider'];

    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-admin-users', {
            parent: 'webvella-admin-base',
            url: '/users/lists', 
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
                	controller: 'WebVellaAdminUsersController',
                    templateUrl: '/plugins/webvella-admin/users.view.html',
                    controllerAs: 'contentData'
                }
            },
            resolve: {
            	resolvedUserRecordsList: resolveUserRecordsList,
                resolvedRolesList: resolveRolesList
            },
            data: {

            }
        });
    };


    //#region << Resolve Functions >>/////////////////////////

    resolveUserRecordsList.$inject = ['$q', '$log', 'webvellaAdminService', '$stateParams', '$state', '$timeout'];
    /* @ngInject */
    function resolveUserRecordsList($q, $log, webvellaAdminService, $stateParams, $state, $timeout) {
    	$log.debug('webvellaAdmin>areas-list>resolveAreaRecordsList BEGIN state.resolved ' + moment().format('HH:mm:ss SSSS'));
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

        webvellaAdminService.getAllUsers(successCallback, errorCallback);


        // Return
        $log.debug('webvellaAdmin>areas-list>resolveAreaRecordsList END state.resolved ' + moment().format('HH:mm:ss SSSS'));
        return defer.promise;
    }

    // Resolve Roles list /////////////////////////
    resolveRolesList.$inject = ['$q', '$log', 'webvellaAdminService'];
    /* @ngInject */
    function resolveRolesList($q, $log, webvellaAdminService) {
    	$log.debug('webvellaAdmin>entities> BEGIN state.resolved ' + moment().format('HH:mm:ss SSSS'));
        // Initialize
        var defer = $q.defer();

        // Process
        function successCallback(response) {
            defer.resolve(response.object);
        }

        function errorCallback(response) {
            defer.resolve(response.object);
        }

        webvellaAdminService.getRecordsByEntityName("null", "role", "null", "null", successCallback, errorCallback);

        // Return
        $log.debug('webvellaAdmin>entities> END state.resolved ' + moment().format('HH:mm:ss SSSS'));
        return defer.promise;
    }

    //#endregion

    //#region << Controller >> ///////////////////////////////
    controller.$inject = ['$scope', '$log', '$rootScope', '$state', 'pageTitle', 'resolvedUserRecordsList',
							'resolvedRolesList', '$modal','webvellaAdminService'];
    /* @ngInject */
    function controller($scope, $log, $rootScope, $state, pageTitle, resolvedUserRecordsList,
						resolvedRolesList, $modal, webvellaAdminService) {
    	$log.debug('webvellaAdmin>user-list> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
        /* jshint validthis:true */
        var contentData = this;
        contentData.search = {};

        //#region << Update page title >>
        contentData.pageTitle = "User List | " + pageTitle;
        $rootScope.$emit("application-pageTitle-update", contentData.pageTitle);
    	//#endregion

        contentData.users = angular.copy(resolvedUserRecordsList.data);
        contentData.users = contentData.users.sort(function (a, b) { return parseFloat(a.email) - parseFloat(b.email) });

        contentData.roles = angular.copy(resolvedRolesList.data);
        contentData.roles = contentData.roles.sort(function (a, b) {
            if (a.name < b.name) return -1;
            if (a.name > b.name) return 1;
            return 0;
        });


        //Create new entity modal
        contentData.openManageUserModal = function (user) {
            if (user != null) {
            	contentData.currentUser = user;
            }
            else {
            	contentData.currentUser = webvellaAdminService.initUser();
            }
            var modalInstance = $modal.open({
                animation: false,
                templateUrl: 'manageUserModal.html',
                controller: 'ManageUserModalController',
                controllerAs: "popupData",
                size: "lg",
                resolve: {
                    contentData: function () {
                        return contentData;
                    }
                }
            });

        }


        $log.debug('webvellaAdmin>areas-list> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
    }
    //#endregion


    //// Modal Controllers
    manageUserController.$inject = ['$modalInstance', '$log', '$sce', '$modal', '$filter', 'webvellaAdminService', 'webvellaRootService', 'ngToast', '$timeout', '$state', '$location', 'contentData'];
    /* @ngInject */
    function manageUserController($modalInstance, $log, $sce, $modal, $filter, webvellaAdminService, webvellaRootService, ngToast, $timeout, $state, $location, contentData) {
    	$log.debug('webvellaAdmin>entities>createEntityModal> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
        /* jshint validthis:true */
        var popupData = this;
        popupData.modalInstance = $modalInstance;
        popupData.user = angular.copy(contentData.currentUser);
        popupData.roles = angular.copy(contentData.roles);
        popupData.password = null;

        popupData.isUpdate = true;
        if (popupData.user.id == null) {
        	popupData.isUpdate = false;
        	popupData.user.$user_role = [];
            popupData.modalTitle = "Create new area";
            popupData.user.id = guid();
        }
        else {
            popupData.modalTitle ='Edit user <span class="go-green">' + popupData.user.email + '</span>';
        }

		//Image        
        popupData.progress = {};
        popupData.progress.image = 0;
        popupData.files = {}
        popupData.files.image = {}
        popupData.uploadedFileName = "";
        popupData.upload = function (file) {

        	if (file != null) {

        		popupData.moveSuccessCallback = function (response) {
        			$timeout(function () {
        				popupData.user.image = response.object.url;
        			}, 1);
        		}

        		popupData.uploadSuccessCallback = function (response) {
        			var tempPath = response.object.url;
        			var fileName = response.object.filename;
        			var targetPath = "/fs/" + popupData.user.id + "/" + fileName;
        			var overwrite = true;
        			webvellaAdminService.moveFileFromTempToFS(tempPath, targetPath, overwrite, popupData.moveSuccessCallback, popupData.uploadErrorCallback);
        		}
        		popupData.uploadErrorCallback = function (response) {
        			alert(response.message);
        		}
        		popupData.uploadProgressCallback = function (response) {
        			$timeout(function () {
        				popupData.progress.image= parseInt(100.0 * response.loaded / response.total);
        			}, 0);
        		}
        		webvellaAdminService.uploadFileToTemp(file, "image", popupData.uploadProgressCallback, popupData.uploadSuccessCallback, popupData.uploadErrorCallback);
        	}
        };
        popupData.deleteImage = function () {
        	var filePath = popupData.user.image;

        	function deleteSuccessCallback(response) {
        		$timeout(function () {
        			popupData.progress.image = 0;
        			popupData.user.image = "";
        			popupData.files.image = null;
        		}, 0);
        		return true;
        	}

        	function deleteFailedCallback(response) {
        		ngToast.create({
        			className: 'error',
        			content: '<span class="go-red">Error:</span> ' + response.message
        		});
        		return "validation error";
        	}

        	webvellaAdminService.deleteFileFromFS(filePath, deleteSuccessCallback, deleteFailedCallback);

        }

    	//Init user roles
        popupData.userRoles = [];
        for (var i = 0; i < popupData.user.$user_role.length; i++) {
        	popupData.userRoles.push(popupData.user.$user_role[i].id);
        }


        /// EXIT functions
        popupData.ok = function () {
        	if (!popupData.isUpdate) {
        		popupData.user.roles = popupData.userRoles;
            	webvellaAdminService.createUser(popupData.user, successCallback, errorCallback);
            }
            else {
                popupData.area.roles = angular.toJson(popupData.area.roles);
                popupData.area.subscriptions = angular.toJson(popupData.subscribedEntities);
                webvellaAdminService.updateRecord(popupData.area.id, "area", popupData.area, successCallback, errorCallback);
            } 
        };

        popupData.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

        /// Aux
        function successCallback(response) {
            ngToast.create({
                className: 'success',
                content: '<span class="go-green">Success:</span> ' + 'The area was successfully saved'
            });
            $modalInstance.close('success');
            webvellaRootService.GoToState($state,$state.current.name, {});
        }

        function errorCallback(response) {
            var location = $location;
            //Process the response and generate the validation Messages
            webvellaRootService.generateValidationMessages(response, popupData, popupData.entity, location);
        }

        //Delete
        //Delete field
        //Create new field modal
        popupData.deleteAreaModal = function () {
            var modalInstance = $modal.open({
                animation: false,
                templateUrl: 'deleteAreaModal.html',
                controller: 'DeleteAreaModalController',
                controllerAs: "popupData",
                size: "",
                resolve: {
                    parentPopupData: function () { return popupData; }
                }
            });
        }

        $log.debug('webvellaAdmin>entities>createEntityModal> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
    };



})();
