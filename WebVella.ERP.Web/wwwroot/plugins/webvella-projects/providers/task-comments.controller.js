// IMPORTANT: You must always have at least webvellaListActionService defined or the page will not load
// The methods inside it are optional 
// For usage in action items, the service is bound to the controller with ngCtrl.actionService. So if 
// what to use a test method from this service in an action you need to call like 'ng-click=""ngCtrl.actionService.test()""'
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Objects accessible through the ngCtrl:
// ngCtrl.list.data => the records' data array
// ngCtrl.list.meta => the records' list meta
// ngCtrl.entity    => the current entity's meta
// ngCtrl.entityRelations => list of all relations of the entity
// ngCtrl.areas		=> the current areas in the site and their meta, attached entities and etc.
// ngCtrl.currentUser => the current user
// ngCtrl.$sessionStorage => copy of the session local storage service
// ngCtrl.stateParams => all state parameters

// IMPORTANT: all data is two way bound, which means it will be watched by angular and any changes propagated. If you want to get a copy of one of the objects, without the binding
// use the var copyObject = fastCopy(originalObject); . This will break the binding.

(function () {
	'use strict';
	angular
    .module('webvellaAreas')
	.controller('ManageTaskCommentModalController', ManageTaskCommentModalController)
	.controller('TaskDetailsCommentListController', TaskDetailsCommentListController);

	TaskDetailsCommentListController.$inject = ['$log', '$http', 'wvAppConstants', '$timeout', 'ngToast', '$filter', 'webvellaCoreService', '$uibModal','$rootScope'];
	function TaskDetailsCommentListController($log, $http, wvAppConstants, $timeout, ngToast, $filter, webvellaCoreService, $uibModal,$rootScope) {

		var pluginCtrl = this;

		// CUSTOM
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		pluginCtrl.isCurrentUserAuthor = isCurrentUserAuthor;
		function isCurrentUserAuthor(authorId) {
			var currentUser = webvellaCoreService.getCurrentUser();
			if (authorId == currentUser.id) {
				return true;
			}
			else {
				return false;
			}
		}

		pluginCtrl.extractFileNameFromPath = function(fullPath){
			if(fullPath){
				return fullPath.replace(/^.*[\\\/]/, '')
			}
			return '';
		}

		pluginCtrl.manageComment = manageComment;
		function manageComment(record,ngCtrl) {
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'manageCommentModal.html',
				controller: 'ManageTaskCommentModalController',
				controllerAs: "popupCtrl",
				size: "lg",
				backdrop:"static",
				resolve: {
					ngCtrl: function () {
						return ngCtrl;
					},
					record: function () {
						return record;
					}
				}
			});
		}

	}

	ManageTaskCommentModalController.$inject = ['$uibModalInstance', '$log', '$sce', '$uibModal', '$filter', 'webvellaCoreService','webvellaProjectsService', 'ngToast', '$timeout',
									'$state', '$location', 'ngCtrl', '$translate','record','$scope'];
	function ManageTaskCommentModalController($uibModalInstance, $log, $sce, $uibModal, $filter, webvellaCoreService,webvellaProjectsService, ngToast, $timeout,
									$state, $location, ngCtrl, $translate,record,$scope) {
		var popupCtrl = this;
		popupCtrl.isUpdate = false;
		popupCtrl.recordStatus = ngCtrl.view.data[0].status;
		popupCtrl.task = fastCopy(ngCtrl.view.data[0]);

		if(record != null){
			popupCtrl.record = fastCopy(record);
			popupCtrl.isUpdate = true;
		}
		else {
			popupCtrl.record = {};
			popupCtrl.record.id = null;
			popupCtrl.record.content = null;
			popupCtrl.record.task_id = fastCopy(ngCtrl.stateParams.recordId);
		}

		popupCtrl.new_owner_id = "";
		popupCtrl.taskOwner = {};
		popupCtrl.taskOwner.id = popupCtrl.task.$field$user_1_n_task_owner$id[0];
		popupCtrl.taskOwner.username = popupCtrl.task.$field$user_1_n_task_owner$username[0];

		popupCtrl.taskWatchers = [];
		var watcherIndex = 0;
		_.forEach(popupCtrl.task.$field$user_n_n_task_watchers$id,function(watcherId){
			if(watcherId !== popupCtrl.taskOwner.id){
				var watcher = {};
				watcher.id = watcherId;
				watcher.username = popupCtrl.task.$field$user_n_n_task_watchers$username[watcherIndex];
				popupCtrl.taskWatchers.push(watcher);
			}
			watcherIndex ++;
		});


		$scope.editorOptions = {
			filebrowserImageBrowseUrl: '/ckeditor/image-finder',
			filebrowserImageUploadUrl: '/ckeditor/image-upload-url',
			uploadUrl :'/ckeditor/drop-upload-url',
			language: GlobalLanguage,
			skin: 'moono-lisa',
			height: '160',
			contentsCss: '/plugins/webvella-core/css/editor.css',
			extraPlugins: "sourcedialog,colorbutton,colordialog,panel,font,uploadimage",
			allowedContent: true,
			colorButton_colors: '333333,FFFFFF,F44336,E91E63,9C27B0,673AB7,3F51B5,2196F3,03A9F4,00BCD4,009688,4CAF50,8BC34A,CDDC39,FFEB3B,FFC107,FF9800,FF5722,795548,607D8B,999999',
			colorButton_enableAutomatic: false,
			colorButton_enableMore: false,
			toolbarLocation: 'top',
			toolbar: 'full',
			toolbar_full: [
				{ name: 'basicstyles', items: ['Save', 'Bold', 'Italic', 'Strike', 'Underline'] },
				{ name: 'colors', items: ['TextColor', 'BGColor'] },
				{ name: 'styles', items: ['FontSize', 'RemoveFormat'] },
				{ name: 'editing', items: ['Format'] },
				{ name: 'links', items: ['Link', 'Unlink'] },
				{ name: 'pasting', items: ['PasteText', 'PasteFromWord'] },
				{ name: 'paragraph', items: ['BulletedList', 'NumberedList', 'Blockquote'] },
				{ name: 'insert', items: ['Image', 'Table', 'SpecialChar'] },
				{ name: 'tools', items: ['Sourcedialog', 'Maximize'] }, '/'
			]
		};

		popupCtrl.ok = function () {
			var submitObject = {};
			submitObject.id = popupCtrl.record.id;
			submitObject.content = popupCtrl.record.content;
			submitObject.task_id = popupCtrl.task.id;
			submitObject.bug_id = "";
			submitObject.new_owner_id = "";
			if(popupCtrl.new_owner_id != popupCtrl.taskOwner.id){
				submitObject.new_owner_id = popupCtrl.new_owner_id;
			}
			webvellaProjectsService.upsertComment(submitObject, successCallback, errorCallback);
		};


		function successCallback(response) {
			$translate(['SUCCESS_MESSAGE_LABEL']).then(function (translations) {
				ngToast.create({
					className: 'success',
					content: translations.SUCCESS_MESSAGE_LABEL + " " + response.message
				});
			});
			$state.reload();
			$uibModalInstance.close('success');
		}

		function errorCallback(response) {
			$translate(['ERROR_MESSAGE_LABEL']).then(function (translations) {
				ngToast.create({
					className: 'error',
					content: translations.ERROR_MESSAGE_LABEL + ' ' + response.message,
					timeout: 7000
				});
			});
		}


		popupCtrl.cancel = function () {
			$uibModalInstance.close('dismiss');
		};

	};

})();

