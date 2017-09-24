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

    
    function config($stateProvider) {
        $stateProvider.state('webvella-admin-users', {
            parent: 'webvella-admin-base',
            url: '/users', 
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
                    controllerAs: 'ngCtrl'
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

    resolveUserRecordsList.$inject = ['$q', '$log', 'webvellaCoreService', '$stateParams', '$state', '$timeout','$translate'];
    
    function resolveUserRecordsList($q, $log, webvellaCoreService, $stateParams, $state, $timeout,$translate) {

        // Initialize
        var defer = $q.defer();
        
        // Process
        function successCallback(response) {
            if (response.object == null) {
				$translate(['ERROR_IN_RESPONSE']).then(function (translations) {
					alert(translations.ERROR_IN_RESPONSE);
				});
            }
            else {
                defer.resolve(response.object);
            }
        }

        function errorCallback(response) {
            if (response.object == null) {
				$translate(['ERROR_IN_RESPONSE']).then(function (translations) {
					alert(translations.ERROR_IN_RESPONSE);
				});
            }
            else {
            	defer.reject(response.message);
            }
        }

        webvellaCoreService.getRecordsWithoutList("","$user_role.id,$user_role.name,id,email,first_name,username,last_name,enabled,verified,image",null,"user",successCallback, errorCallback);

        return defer.promise;
    }

    // Resolve Roles list /////////////////////////
    resolveRolesList.$inject = ['$q', '$log', 'webvellaCoreService'];
    
    function resolveRolesList($q, $log, webvellaCoreService) {
        // Initialize
        var defer = $q.defer();

        // Process
        function successCallback(response) {
            defer.resolve(response.object);
        }

        function errorCallback(response) {
        	defer.reject(response.message);
        }

        webvellaCoreService.getRecordsWithoutList(null,null,null,"role", successCallback, errorCallback);

        return defer.promise;
    }

    //#endregion

    //#region << Controller >> ///////////////////////////////
    controller.$inject = ['$scope', '$log', '$rootScope', '$state', 'pageTitle', 'resolvedUserRecordsList',
							'resolvedRolesList', '$uibModal', 'webvellaCoreService','$timeout','$translate'];
    
    function controller($scope, $log, $rootScope, $state, pageTitle, resolvedUserRecordsList,
						resolvedRolesList, $uibModal, webvellaCoreService,$timeout,$translate) {
        
        var ngCtrl = this;
        ngCtrl.search = {};

		//#region << Update page title & hide the side menu >>
		$translate(['USERS_PAGE_TITLE', 'USERS']).then(function (translations) {
			ngCtrl.pageTitle = translations.USERS_PAGE_TITLE + " | " + pageTitle;
			$rootScope.$emit("application-pageTitle-update", ngCtrl.pageTitle);
			$rootScope.adminSectionName = translations.USERS;
		});
    	//#endregion

        ngCtrl.users = resolvedUserRecordsList.data;
        ngCtrl.users = ngCtrl.users.sort(function (a, b) { return parseFloat(a.email) - parseFloat(b.email) });

        ngCtrl.roles = resolvedRolesList.data;
        ngCtrl.roles = ngCtrl.roles.sort(function (a, b) {
            if (a.name < b.name) return -1;
            if (a.name > b.name) return 1;
            return 0;
        });


        //Create new entity modal
        ngCtrl.openManageUserModal = function (user) {
            if (user != null) {
            	ngCtrl.currentUser = user;
            }
            else {
            	ngCtrl.currentUser = webvellaCoreService.initUser();
            }
            var modalInstance = $uibModal.open({
                animation: false,
                templateUrl: 'manageUserModal.html',
                controller: 'ManageUserModalController',
                controllerAs: "popupCtrl",
                size: "lg",
                resolve: {
                    ngCtrl: function () {
                        return ngCtrl;
                    }
                }
            });

        }
    }
    //#endregion


    //// Modal Controllers
    manageUserController.$inject = ['$uibModalInstance', '$log', '$sce', '$uibModal', '$filter', 'webvellaCoreService', 'ngToast', '$timeout', '$state', '$location', 'ngCtrl','$translate'];
    
    function manageUserController($uibModalInstance, $log, $sce, $uibModal, $filter, webvellaCoreService, ngToast, $timeout, $state, $location, ngCtrl,$translate) {
        
        var popupCtrl = this;
        popupCtrl.modalInstance = $uibModalInstance;
        popupCtrl.user = fastCopy(ngCtrl.currentUser);
        popupCtrl.roles = fastCopy(ngCtrl.roles);
        popupCtrl.password = null;
    	//Init user roles
        popupCtrl.userRoles = [];

        popupCtrl.isUpdate = true;
        if (popupCtrl.user.id == null) {
        	popupCtrl.isUpdate = false;
        	//popupCtrl.user.$user_role = [];
			$translate(['USER_CREATE_MODAL_TITLE']).then(function (translations) {
				popupCtrl.modalTitle = translations.USER_CREATE_MODAL_TITLE;
			});
            popupCtrl.user.id = null;
            popupCtrl.userRoles.push("f16ec6db-626d-4c27-8de0-3e7ce542c55f"); //Push regular role by default
        	//Guest role = 987148b1-afa8-4b33-8616-55861e5fd065
        }
        else {
        	for (var i = 0; i < popupCtrl.user.$user_role.length; i++) {
        		popupCtrl.userRoles.push(popupCtrl.user.$user_role[i].id);
        	}
			$translate('USER_MANAGE_MODAL_TITLE', { email: popupCtrl.user.email }).then(function (modalTitle) {
				popupCtrl.modalTitle =modalTitle;
			} );
        }

		//Image        
        popupCtrl.progress = {};
        popupCtrl.progress.image = 0;
        popupCtrl.files = {}
        popupCtrl.files.image = {}
        popupCtrl.uploadedFileName = "";
        popupCtrl.upload = function (file) {

        	if (file != null) {

        		popupCtrl.moveSuccessCallback = function (response) {
        			$timeout(function () {
        				popupCtrl.user.image = response.object.url;
        			}, 1);
        		}

        		popupCtrl.uploadSuccessCallback = function (response) {
        			var tempPath = response.object.url;
        			var fileName = response.object.filename;
        			var targetPath = "/" + popupCtrl.user.id + "/" + fileName;
        			var overwrite = true;
        			webvellaCoreService.moveFileFromTempToFS(tempPath, targetPath, overwrite, popupCtrl.moveSuccessCallback, popupCtrl.uploadErrorCallback);
        		}
        		popupCtrl.uploadErrorCallback = function (response) {
        			alert(response.message);
        		}
        		popupCtrl.uploadProgressCallback = function (response) {
        			$timeout(function () {
        				popupCtrl.progress.image= parseInt(100.0 * response.loaded / response.total);
        			}, 0);
        		}
        		webvellaCoreService.uploadFileToTemp(file, popupCtrl.uploadProgressCallback, popupCtrl.uploadSuccessCallback, popupCtrl.uploadErrorCallback);
        	}
        };
        popupCtrl.deleteImage = function () {
        	var filePath = popupCtrl.user.image;

        	function deleteSuccessCallback(response) {
        		$timeout(function () {
        			popupCtrl.progress.image = 0;
        			popupCtrl.user.image = "";
        			popupCtrl.files.image = null;
        		}, 0);
        		return true;
        	}

        	function deleteFailedCallback(response) {
        		ngToast.create({
        			className: 'error',
        			content: '<span class="go-red">Error:</span> ' + response.message,
        			timeout: 7000
        		});
        		return "validation error";
        	}

        	webvellaCoreService.deleteFileFromFS(filePath, deleteSuccessCallback, deleteFailedCallback);

        }


        /// EXIT functions
        popupCtrl.ok = function () {
        	popupCtrl.validation = {};
        	if (!popupCtrl.isUpdate) {
        		popupCtrl.user.password = popupCtrl.password;
				popupCtrl.user["$user_role.id"] = [];
        		popupCtrl.userRoles.forEach(function(role){
					 popupCtrl.user["$user_role.id"].push(role);
				});
				webvellaCoreService.createRecord("user",popupCtrl.user, successCallback, errorCallback);
            }
            else {
        		popupCtrl.user["$user_role.id"] = [];
        		popupCtrl.userRoles.forEach(function(role){
					 popupCtrl.user["$user_role.id"].push(role);
				});
				delete popupCtrl.user["$user_role"];
        		if (popupCtrl.password) {
        			popupCtrl.user.password = popupCtrl.password;
        		}
        		webvellaCoreService.updateRecord(popupCtrl.user.id,"user",popupCtrl.user, successCallback, errorCallback);
            } 
        };

        popupCtrl.cancel = function () {
            $uibModalInstance.close('dismiss');
        };

        /// Aux
        function successCallback(response) {
            ngToast.create({
                className: 'success',
                content: '<span class="go-green">Success:</span> ' + 'The user was successfully saved'
            });
            $uibModalInstance.close('success');
            webvellaCoreService.GoToState($state.current.name, {});
        }

        function errorCallback(response) {
            var location = $location;
            //Process the response and generate the validation Messages
            webvellaCoreService.generateValidationMessages(response, popupCtrl, popupCtrl.user, location);
        }
    };



})();
