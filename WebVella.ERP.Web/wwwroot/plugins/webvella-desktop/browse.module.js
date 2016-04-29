
(function () {
	'use strict';

	angular
        .module('webvellaDesktop')
        .config(config)
        .controller('WebVellaDesktopBrowseController', controller);

	//#region << Configuration >>
	config.$inject = ['$stateProvider'];
	function config($stateProvider) {
		$stateProvider.state('webvella-desktop-browse', {
			parent: "webvella-desktop-base",
			url: '/desktop/browse?folder',
			views: {
				"contentView": {
					controller: 'WebVellaDesktopBrowseController',
					templateUrl: '/plugins/webvella-desktop/browse.view.html',
					controllerAs: 'ngCtrl'
				}
			},
			resolve: {
				resolvedAreas: resolveAreas,
				resolvedLastUsedArea: resolveLastUsedArea
			},
			data: {}
		});
	};
	//#endregion

	//#region << Resolve Function >>
	resolveAreas.$inject = ['$q', 'webvellaCoreService'];
	function resolveAreas($q, webvellaCoreService) {
		var defer = $q.defer();
		function successCallback(response) {
			defer.resolve(response.object);
		}
		function errorCallback(response) {
			defer.reject(response.message);
		}
		webvellaCoreService.getRecordsWithoutList(null,null,null,"area", successCallback, errorCallback);
		return defer.promise;
	}

	resolveLastUsedArea.$inject = ['$q', 'webvellaCoreService', '$localStorage', '$stateParams'];
	function resolveLastUsedArea($q, webvellaCoreService, $localStorage, $stateParams) {
		var defer = $q.defer();
		var currentFolderName = $stateParams.folder;
		var localStorage = $localStorage;
		if (!currentFolderName && localStorage.folder) {
			webvellaCoreService.GoToState("webvella-desktop-browse", { folder: localStorage.folder });
		}
		else if (!currentFolderName && !localStorage.folder) {
			webvellaCoreService.GoToState("webvella-desktop-browse", { folder: "Default" });
		}
		else {
			defer.resolve();
		}
		return defer.promise;
	}
	//#endregion
 
	//#region << Controller >>
	controller.$inject = ['$rootScope', '$scope', '$state', 'pageTitle', '$localStorage', '$timeout',
					'webvellaDesktopTopnavFactory', 'webvellaCoreService', 'resolvedAreas', 'webvellaDesktopBrowsenavFactory', 'resolvedCurrentUser'];
	function controller($rootScope, $scope, $state, pageTitle, $localStorage, $timeout,
					webvellaDesktopTopnavFactory, webvellaCoreService, resolvedAreas, webvellaDesktopBrowsenavFactory, resolvedCurrentUser) {
		var ngCtrl = this;
		ngCtrl.browsenav = [];
		ngCtrl.folder = $state.params.folder;
		var localStorage = $localStorage;
		if (ngCtrl.folder) {
			localStorage.folder = ngCtrl.folder;
		}
		else {
			localStorage.folder = "Default";
		}
		ngCtrl.topNav = [];
		ngCtrl.topNavDict = [];
		webvellaDesktopTopnavFactory.initTopnav();

		//#region << Set Page title >>
		ngCtrl.pageTitle = "Browse Desktop | " + pageTitle;
		webvellaCoreService.setPageTitle(ngCtrl.pageTitle);
		//#endregion

		//#region << Make the Browsenav pluggable & Initialize>>
		////1. CONSTRUCTOR - initialize the factory
		webvellaDesktopBrowsenavFactory.initBrowsenav();

		var sitemapAreas = fastCopy(resolvedAreas.data);
		sitemapAreas.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });
		for (var i = 0; i < sitemapAreas.length; i++) {
			//Generate topnav
			var folderLabel = "Default";
			if (sitemapAreas[i].folder && sitemapAreas[i].folder != "") {
				folderLabel = sitemapAreas[i].folder;
			}
			if (ngCtrl.topNavDict.indexOf(folderLabel) == -1) {
				var topNavItem = {
					"label": folderLabel,
					"stateName": "webvella-desktop-browse",
					"stateParams": { folder: folderLabel },
					"parentName": "",
					"nodes": [],
					"weight": sitemapAreas[i].weight
				}
				topNavItem.nodes.push(sitemapAreas[i]);
				ngCtrl.topNav.push(topNavItem);
				ngCtrl.topNavDict.push(folderLabel);
			}
			else {
				for (var j = 0; j < ngCtrl.topNav.length; j++) {
					if (ngCtrl.topNav[j].label == folderLabel) {
						var currentWeight = ngCtrl.topNav[j].weight;
						var currentNodes = ngCtrl.topNav[j].nodes.length;

						var newAvarageWeight = (currentWeight * currentNodes + sitemapAreas[i].weight) / (currentNodes + 1);
						newAvarageWeight = Math.round(newAvarageWeight);
						ngCtrl.topNav[j].weight = newAvarageWeight;
					}
				}
			}



			var menuItem = webvellaDesktopBrowsenavFactory.generateMenuItemFromArea(sitemapAreas[i]);
			if (menuItem != null) {
				var userCanUseArrea = false;
				for (var k = 0; k < resolvedCurrentUser.roles.length; k++) {
					for (var p = 0; p < menuItem.roles.length; p++) {
						if (menuItem.roles[p] == resolvedCurrentUser.roles[k]) {
							userCanUseArrea = true;
							break;
						}
					}
				}
				if (userCanUseArrea) {
					webvellaDesktopBrowsenavFactory.addItem(menuItem);
				}
			}
		};

		for (var i = 0; i < ngCtrl.topNav.length; i++) {
			webvellaDesktopTopnavFactory.addItem(ngCtrl.topNav[i]);
		}

		////3. UPDATED hook listener
		var updateBrowsenavDestructor = $rootScope.$on("webvellaDesktop-browsenav-updated", function (event, data) {
			var browseNav = [];
			for (var i = 0; i < data.length; i++) {
				var menuItem = data[i];
				if (!menuItem.folder || menuItem.folder == "") {
					menuItem.folder = "Default";
				}
				if (menuItem.folder == $state.params.folder) {
					browseNav.push(menuItem);
				}

			}
			ngCtrl.browsenav = browseNav;
		});
		////4. DESCTRUCTOR - hook listeners remove on scope destroy. This avoids duplication, as rootScope is never destroyed and new controller load will duplicate the listener
		$scope.$on("$destroy", function () {
			updateBrowsenavDestructor();
		});
		////5. Bootstrap the pluggable Browsenav
		$timeout(function () {
			$rootScope.$emit("webvellaDesktop-browsenav-ready");
		}, 0);
		//#endregion


		//Wait for the load and show non items messages if needed
		ngCtrl.loaded = false;
		$timeout(function () {
		  ngCtrl.loaded = true;
		},250);
	}
	//#endregion
})();
