/* areas-lists.module.js */

/**
* @desc this module manages the entity record lists in the admin screen
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminRolesController', controller)
        .controller('ManageRoleModalController', manageRoleController);
   
    ///////////////////////////////////////////////////////
    /// Configuration
    ///////////////////////////////////////////////////////

    config.$inject = ['$stateProvider'];

    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-admin-roles', {
            parent: 'webvella-admin-base',
            url: '/roles', 
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
                	controller: 'WebVellaAdminRolesController',
                	templateUrl: '/plugins/webvella-admin/roles.view.html',
                    controllerAs: 'ngCtrl'
                }
            },
            resolve: {
            	checkedAccessPermission: checkAccessPermission,
                resolvedRolesList: resolveRolesList
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
    			content: messageContent,
    			timeout: 7000
    		});
    		defer.reject("No access");
    	}

    	$log.debug('webvellaAreas>entities> BEGIN check access permission ' + moment().format('HH:mm:ss SSSS'));
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
        	defer.reject(response.message);
        }

        webvellaAdminService.getRecordsByEntityName("null", "role", "null", successCallback, errorCallback);

        // Return
        $log.debug('webvellaAdmin>entities> END state.resolved ' + moment().format('HH:mm:ss SSSS'));
        return defer.promise;
    }

    //#endregion

    //#region << Controller >> ///////////////////////////////
    controller.$inject = ['$scope', '$log', '$rootScope', '$state', 'pageTitle','$timeout', 
							'resolvedRolesList', '$uibModal', 'webvellaAdminService'];
    /* @ngInject */
    function controller($scope, $log, $rootScope, $state, pageTitle,$timeout,
						resolvedRolesList, $uibModal, webvellaAdminService) {
    	$log.debug('webvellaAdmin>roles> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
        /* jshint validthis:true */
        var ngCtrl = this;
        ngCtrl.search = {};

        //#region << Update page title >>
        ngCtrl.pageTitle = "User List | " + pageTitle;
		$timeout(function(){
			$rootScope.$emit("application-pageTitle-update", ngCtrl.pageTitle);
		},0);

    	//#endregion

        ngCtrl.roles = fastCopy(resolvedRolesList.data);
        ngCtrl.roles = ngCtrl.roles.sort(function (a, b) {
            if (a.name < b.name) return -1;
            if (a.name > b.name) return 1;
            return 0;
        });


        //Create new entity modal
        ngCtrl.openManageRoleModal = function (role) {
        	if (role != null) {
        		ngCtrl.currentRole = role;
            }
            else {
        		ngCtrl.currentRole = {};
        		ngCtrl.currentRole.id = null;
        		ngCtrl.currentRole.name = "";
            }
        	var modalInstance = $uibModal.open({
                animation: false,
                templateUrl: 'manageRoleModal.html',
                controller: 'ManageRoleModalController',
                controllerAs: "popupCtrl",
                //size: "lg",
                resolve: {
                    ngCtrl: function () {
                        return ngCtrl;
                    }
                }
            });

        }



        $log.debug('webvellaAdmin>roles> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
    }
    //#endregion


    //// Modal Controllers
    manageRoleController.$inject = ['$uibModalInstance', '$log', '$sce', '$uibModal', '$filter', 'webvellaAdminService', 'webvellaRootService', 'ngToast', '$timeout', '$state', '$location', 'ngCtrl'];
    /* @ngInject */
    function manageRoleController($uibModalInstance, $log, $sce, $uibModal, $filter, webvellaAdminService, webvellaRootService, ngToast, $timeout, $state, $location, ngCtrl) {
    	$log.debug('webvellaAdmin>entities>createEntityModal> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
        /* jshint validthis:true */
        var popupCtrl = this;
        popupCtrl.modalInstance = $uibModalInstance;
        popupCtrl.role = fastCopy(ngCtrl.currentRole);
        popupCtrl.isUpdate = true;
        if (popupCtrl.role.id == null) {
        	popupCtrl.isUpdate = false;
            popupCtrl.modalTitle = "Create new role";
        }
        else {
            popupCtrl.modalTitle ='Edit role <span class="go-green">' + popupCtrl.role.name + '</span>';
        }

        popupCtrl.deleteRoleModal = function () {
        	//Should block the ability to delete admin, guest, regular
			//On delete should update -> areas, entities, entity fields, users and other relevant items which has roles relations.
        	alert("Not implemented yet");
		}

        /// EXIT functions
        popupCtrl.ok = function () {
        	popupCtrl.validation = {};
        	if (!popupCtrl.isUpdate) {
        		webvellaAdminService.createRecord("role",popupCtrl.role, successCallback, errorCallback);
            }
            else {
        		webvellaAdminService.updateRecord(popupCtrl.role.id, "role", popupCtrl.role, successCallback, errorCallback);
            } 
        };

        popupCtrl.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };

        /// Aux
        function successCallback(response) {
            ngToast.create({
                className: 'success',
                content: '<span class="go-green">Success:</span> ' + 'The role was successfully saved'
            });
            $uibModalInstance.close('success');
            webvellaRootService.GoToState($state.current.name, {});
        }

        function errorCallback(response) {
            var location = $location;
            //Process the response and generate the validation Messages
            webvellaRootService.generateValidationMessages(response, popupCtrl, popupCtrl.user, location);
        }


        $log.debug('webvellaAdmin>entities>createEntityModal> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
    };



})();
