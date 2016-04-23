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
                resolvedRolesList: resolveRolesList
            },
            data: {

            }
        });
    };


    //#region << Resolve Functions >>/////////////////////////

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

        webvellaCoreService.getRecordsByListName("null","role", "null", null, successCallback, errorCallback);

        return defer.promise;
    }

    //#endregion

    //#region << Controller >> ///////////////////////////////
    controller.$inject = ['$scope', '$log', '$rootScope', '$state', 'pageTitle','$timeout', 
							'resolvedRolesList', '$uibModal', 'webvellaCoreService'];
    
    function controller($scope, $log, $rootScope, $state, pageTitle,$timeout,
						resolvedRolesList, $uibModal, webvellaCoreService) {

        
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
    }
    //#endregion


    //// Modal Controllers
    manageRoleController.$inject = ['$uibModalInstance', '$log', '$sce', '$uibModal', '$filter', 'webvellaCoreService', 'ngToast', '$timeout', '$state', '$location', 'ngCtrl'];
    
    function manageRoleController($uibModalInstance, $log, $sce, $uibModal, $filter, webvellaCoreService, ngToast, $timeout, $state, $location, ngCtrl) {
        
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
        		webvellaCoreService.createRecord("role",popupCtrl.role, successCallback, errorCallback);
            }
            else {
        		webvellaCoreService.updateRecord(popupCtrl.role.id, "role", popupCtrl.role, successCallback, errorCallback);
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
            webvellaCoreService.GoToState($state.current.name, {});
        }

        function errorCallback(response) {
            var location = $location;
            //Process the response and generate the validation Messages
            webvellaCoreService.generateValidationMessages(response, popupCtrl, popupCtrl.user, location);
        }
    };



})();
