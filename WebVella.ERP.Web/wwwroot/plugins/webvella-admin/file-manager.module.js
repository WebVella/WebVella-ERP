/* items-add-item.module.js */

/**
* @desc used in the customer profile
*/

(function () {
	'use strict';
	var FileManagerPageSize = 30;
	angular
		.module('webvellaAdmin')
		.config(config)
		.controller('WebVellaUserFileManagerController', controller);

	///////////////////////////////////////////////////////
	/// Configuration
	///////////////////////////////////////////////////////

	config.$inject = ['$stateProvider'];

	
	function config($stateProvider) {
		$stateProvider.state('webvella-admin-file-manager', {
			parent: 'webvella-admin-base',
			url: '/file-manager', 
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
					controller: 'WebVellaUserFileManagerController',
					templateUrl: '/plugins/webvella-admin/file-manager.view.html',
					controllerAs: 'ngCtrl'
				}
			},
			resolve: {
				resolvedFilesList: resolveFilesList,
				resolvedUserFileEntityMeta: resolveUserFileEntityMeta
			},
			data: {

			}
		});
	};


	//#region << Resolve Functions >>/////////////////////////

	// Resolve Roles list /////////////////////////
	resolveFilesList.$inject = ['$q', '$log', 'webvellaCoreService','$location'];
	
	function resolveFilesList($q, $log, webvellaCoreService,$location) {
		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			defer.resolve(response.object);
		}

		function errorCallback(response) {
			defer.reject(response.message);
		}
		var ngCtrl = {};
		ngCtrl.type = null;
		ngCtrl.search = "";
		ngCtrl.page = 1;
		ngCtrl.pageSize = FileManagerPageSize;
		
		var queryObject = $location.search();
		for (var key in queryObject) {
			ngCtrl[key] = queryObject[key];
		}
		if(typeof ngCtrl.page === "string"){
			var response = checkInt(ngCtrl.page);
			if(response.success){
				ngCtrl.page = parseInt(ngCtrl.page);
			}
			else{
				ngCtrl.page = 1;
			}
		}

		webvellaCoreService.getUserFileList(ngCtrl.type,ngCtrl.search,ngCtrl.sort,ngCtrl.page,ngCtrl.pageSize,  successCallback, errorCallback);

		return defer.promise;
	}

	resolveUserFileEntityMeta.$inject = ['$q', '$log', 'webvellaCoreService'];
	
	function resolveUserFileEntityMeta($q, $log, webvellaCoreService) {
		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			defer.resolve(response.object);
		}

		function errorCallback(response) {
			defer.reject(response.message);
		}


		webvellaCoreService.getEntityMeta("user_file", successCallback, errorCallback);

		return defer.promise;
	}


	//#endregion


	//#region >> Controller >>
	controller.$inject = ['$scope', '$location', '$timeout', 'resolvedFilesList','resolvedUserFileEntityMeta','webvellaCoreService', '$uibModal','ngToast'];
	function controller($scope, $location, $timeout, resolvedFilesList,resolvedUserFileEntityMeta, webvellaCoreService, $uibModal, ngToast) {
		//#region >> Init >>
		var ngCtrl = this;
		var typeFieldMeta = _.find(resolvedUserFileEntityMeta.fields,function(record){ return record.name == "type";});
		ngCtrl.types = typeFieldMeta.options;
		ngCtrl.files = resolvedFilesList;
		ngCtrl.isLoading = false;
		ngCtrl.type = null;
		ngCtrl.search = "";
		ngCtrl.page = 1;
		ngCtrl.pageSize = FileManagerPageSize;
		
		var queryObject = $location.search();
		for (var key in queryObject) {
			ngCtrl[key] = queryObject[key];
		}
		if(typeof ngCtrl.page === "string"){
			var response = checkInt(ngCtrl.page);
			if(response.success){
				ngCtrl.page = parseInt(ngCtrl.page);
			}
			else{
				ngCtrl.page = 1;
			}
		}



		function successCallback(response){
			ngCtrl.files = response.object;
			ngCtrl.isLoading = false;
		}
		function errorCallback(response){
			alert(response.message);
			ngCtrl.isLoading = false;
		}

		function loadFiles(){
			ngCtrl.isLoading = true;
			ngCtrl.errorMessage = null;
			webvellaCoreService.getUserFileList(ngCtrl.type,ngCtrl.search,ngCtrl.sort,ngCtrl.page,ngCtrl.pageSize, successCallback, errorCallback);
		}


		ngCtrl.submitFilter = function () {
			if (!isStringNullOrEmptyOrWhiteSpace(ngCtrl.type)) {
				$location.search("type", ngCtrl.type);
			}
			else {
				$location.search("type", null);
			}
			if (!isStringNullOrEmptyOrWhiteSpace(ngCtrl.search)) {
				$location.search("search", ngCtrl.search);
			}
			else {
				$location.search("search", null);
			}
			ngCtrl.page = 1;
			$location.search("page", 1);
			loadFiles();
		};

		ngCtrl.selectPage = function (page) {
			$location.search("page", page);
			ngCtrl.page = page;
			loadFiles();
		};


		ngCtrl.createFile = function(){
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'createFileModal.html',
				controller: function ($scope, $sce, $filter,$state, $uibModalInstance,webvellaCoreService,parentCtrl,pathFieldMeta,ngToast,$timeout) {
					$scope.popupCtrl = {};
					$scope.popupCtrl.validation = webvellaCoreService.initValidationObject();
					$scope.popupCtrl.loading = false;
					$scope.popupCtrl.file = {};
					$scope.popupCtrl.file.alt = null;
					$scope.popupCtrl.file.caption = null;
					$scope.popupCtrl.file.path = null;
					$scope.popupCtrl.uploadedFile = null;
					$scope.popupCtrl.progress = 0;
					$scope.popupCtrl.uploadedFileName = "";
					$scope.upload = function (file) {
        				if (file != null) {
							$scope.popupCtrl.validation = webvellaCoreService.initValidationObject();
        					$scope.popupCtrl.uploadSuccessCallback = function (response) {
        						$scope.popupCtrl.file.path = response.object.url;
        						$scope.popupCtrl.uploadedFileName = response.object.filename;
        					}
        					$scope.popupCtrl.uploadErrorCallback = function (response) {
        						ngToast.create({
        							className: 'error',
        							content: '<span class="go-red">Error:</span> ' + response.message,
        							timeout: 7000
        						});
        					}
        					$scope.popupCtrl.uploadProgressCallback = function (response) {
        						$timeout(function () {
        							$scope.popupCtrl.progress= parseInt(100.0 * response.loaded / response.total);
        						}, 0);
        					}
        					webvellaCoreService.uploadFileToTemp(file, $scope.popupCtrl.uploadProgressCallback, $scope.popupCtrl.uploadSuccessCallback, $scope.popupCtrl.uploadErrorCallback);
        				}
					};
					$scope.popupCtrl.deleteFile = function () {
        				var filePath = $scope.popupCtrl.file.path;

        				function deleteSuccessCallback(response) {
        					$timeout(function () {
								$scope.popupCtrl.file.path = null;
								$scope.popupCtrl.uploadedFile = null;
								$scope.popupCtrl.progress = 0;
								$scope.popupCtrl.uploadedFileName = "";
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

					$scope.popupCtrl.renderFieldValue = function(){
						return webvellaCoreService.renderFieldValue($scope.popupCtrl.file.path,pathFieldMeta);
					}

					$scope.popupCtrl.save = function(){
        				function uploadSuccessCallback(response) {
        					$timeout(function () {
								$scope.popupCtrl.file.path = null;
								$scope.popupCtrl.uploadedFile = null;
								$scope.popupCtrl.progress = 0;
								$scope.popupCtrl.uploadedFileName = "";
								parentCtrl.files.unshift(response.object);
								$scope.popupCtrl.loading = false;
								$uibModalInstance.close('dismiss'); 
        					}, 0);
        					return true;
        				}

        				function uploadFailedCallback(response) {
        					ngToast.create({
        						className: 'error',
        						content: '<span class="go-red">Error:</span> ' + response.message,
        						timeout: 7000
        					});
							$scope.popupCtrl.loading = false;
        					return "validation error";
        				}

						$scope.popupCtrl.validation = webvellaCoreService.initValidationObject();
						if(isStringNullOrEmptyOrWhiteSpace($scope.popupCtrl.file.path)){
							$scope.popupCtrl.validation = webvellaCoreService.setValidationError($scope.popupCtrl.validation,"Validation error","path","* required");
						}
						if(!$scope.popupCtrl.validation.isInvalid){
							$scope.popupCtrl.loading = true;
							var submitObject = {};
							submitObject.path = $scope.popupCtrl.file.path;
							submitObject.caption = $scope.popupCtrl.file.caption;
							submitObject.alt = $scope.popupCtrl.file.alt;
							webvellaCoreService.uploadUserFile(submitObject, uploadSuccessCallback, uploadFailedCallback);
						}
					};

					$scope.popupCtrl.cancel = function () { $uibModalInstance.close('dismiss'); };
				},
				size: 'lg',
				resolve: {
					parentCtrl: function () {
						return ngCtrl;
					},
					pathFieldMeta: function () {
						return _.find(resolvedUserFileEntityMeta.fields,function(record){ return record.name == "path";});
					}
				}
			});						
		};


		ngCtrl.viewFile = function(file){
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'viewFileModal.html',
				controller: function ($scope, $sce, $filter,$state, $uibModalInstance, parentCtrl,selectedFile, webvellaCoreService) {
					$scope.popupCtrl = {};
					$scope.popupCtrl.selectedFile = fastCopy(selectedFile);
					$scope.popupCtrl.saving =false;
					$scope.popupCtrl.saved =false;
					$scope.popupCtrl.errored =false;
					$scope.popupCtrl.delete = function (){

						 function successCallback(response) {
							$timeout(function(){
								_.remove(parentCtrl.files,function(record){return record.id == selectedFile.id;});
							},0);
							ngToast.create({
								className: 'success',
								content: '<span class="go-green">Success:</span> ' + response.message
							}); 
							$scope.popupCtrl.cancel();
						 }

						function errorCallback(response) {
							ngToast.create({
								className: 'danger',
								content: '<span class="go-red">Error:</span> ' + response.message
							});
						}

						webvellaCoreService.deleteRecord(selectedFile.id, "user_file", successCallback, errorCallback);
					};

					$scope.popupCtrl.cancel = function () { $uibModalInstance.close('dismiss'); };
				},
				size: 'width-75p',
				resolve: {
					parentCtrl: function () {
						return ngCtrl;
					},
					selectedFile: function () {
						return file;
					}
				}
			});						
		};

	}
	//#endregion << Controller <<

})();

