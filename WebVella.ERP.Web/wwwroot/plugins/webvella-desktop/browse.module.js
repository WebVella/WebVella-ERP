/* entities.module.js */

/**
* @desc this module manages the application home desktop screen
*/

(function () {
	'use strict';

	angular
        .module('webvellaDesktop') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .run(run)
        .config(config)
        .controller('WebVellaDesktopBrowseController', controller);

	//#region << Configuration >>
	config.$inject = ['$stateProvider'];

	/* @ngInject */
	function config($stateProvider) {
		$stateProvider.state('webvella-desktop-browse', {
			parent: "webvella-desktop-base", // the state is defined in the webvella-desktop-plugin
			url: '/desktop/browse?folder', //  /desktop/browse after the parent state is prepended
			views: {
				"contentView": {
					controller: 'WebVellaDesktopBrowseController',
					templateUrl: '/plugins/webvella-desktop/browse.view.html',
					controllerAs: 'contentData'
				}
			},
			resolve: {
				resolvedSitemap: resolveSitemap,
				resolvedLastUsedArea: resolveLastUsedArea
			},
			data: {}
		});
	};
	//#endregion

	//#region << Run >>
	run.$inject = ['$log', '$rootScope', 'webvellaDesktopTopnavFactory', 'webvellaDesktopBrowsenavFactory'];

	/* @ngInject */
	function run($log, $rootScope, webvellaDesktopTopnavFactory, webvellaDesktopBrowsenavFactory) {
		$log.debug('webvellaDesktop>browse> BEGIN module.run ' + moment().format('HH:mm:ss SSSS'));

		//Initialize the pluggable object made with factories, always when state is changed. (it fixes the duplication problem with browser back and forward buttons)
		//$rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
		//    webvellaDesktopBrowsenavService.initBrowsenav();
		//})

		$log.debug('webvellaDesktop>browse> END module.run ' + moment().format('HH:mm:ss SSSS'));
	};
	//#endregion

	//#region << Resolve Function >>
	resolveSitemap.$inject = ['$q', '$log', 'webvellaRootService'];
	/* @ngInject */
	function resolveSitemap($q, $log, webvellaRootService) {
		$log.debug('webvellaDesktop>browse> BEGIN state.resolved ' + moment().format('HH:mm:ss SSSS'));
		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			defer.resolve(response.object);
		}

		function errorCallback(response) {
			defer.reject(response.message);
		}

		webvellaRootService.getSitemap(successCallback, errorCallback);

		// Return
		$log.debug('webvellaDesktop>browse> END state.resolved ' + moment().format('HH:mm:ss SSSS'));
		return defer.promise;
	}

	resolveLastUsedArea.$inject = ['$q', '$log', 'webvellaRootService', '$localStorage', '$stateParams', '$window'];
	/* @ngInject */
	function resolveLastUsedArea($q, $log, webvellaRootService, $localStorage, $stateParams, $window) {
		$log.debug('webvellaDesktop>browse> BEGIN resolveLastUsedArea ' + moment().format('HH:mm:ss SSSS'));
		// Initialize
		var defer = $q.defer();
		var currentFolderName = $stateParams.folder;
		var localStorage = $localStorage;
		if (!currentFolderName && localStorage.folder) {
			webvellaRootService.GoToState("webvella-desktop-browse", { folder: localStorage.folder });
			//$window.location = '#/desktop/browse?folder=' + localStorage.folder;
		}
		else if (!currentFolderName && !localStorage.folder) {
			webvellaRootService.GoToState("webvella-desktop-browse", { folder: "Default" });
			//$window.location = '#/desktop/browse?folder=Default';
		}
		else {
			//Do nothing
			defer.resolve();
		}

		// Return
		$log.debug('webvellaDesktop>browse> END resolveLastUsedArea ' + moment().format('HH:mm:ss SSSS'));
		return defer.promise;
	}
	//#endregion


	//#region << Controller >>
	controller.$inject = ['$log', '$rootScope', '$scope', '$state', 'pageTitle', '$localStorage', '$timeout',
					'webvellaDesktopTopnavFactory', 'webvellaRootService', 'resolvedSitemap', 'webvellaDesktopBrowsenavFactory', 'resolvedCurrentUser'];

	/* @ngInject */
	function controller($log, $rootScope, $scope, $state, pageTitle, $localStorage, $timeout,
					webvellaDesktopTopnavFactory, webvellaRootService, resolvedSitemap, webvellaDesktopBrowsenavFactory, resolvedCurrentUser) {
		$log.debug('webvellaDesktop>browse> BEGIN controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var contentData = this;
		contentData.browsenav = [];
		contentData.folder = fastCopy($state.params.folder);
		var localStorage = $localStorage;
		if (contentData.folder) {
			localStorage.folder = contentData.folder;
		}
		else {
			localStorage.folder = "Default";
		}
		contentData.topNav = [];
		contentData.topNavDict = [];
		webvellaDesktopTopnavFactory.initTopnav();

		//#region << Set Page title >>
		contentData.pageTitle = "Browse Desktop | " + pageTitle;
		webvellaRootService.setPageTitle(contentData.pageTitle);
		//#endregion
		//#region << Make the Browsenav pluggable & Initialize>>
		////1. CONSTRUCTOR - initialize the factory
		webvellaDesktopBrowsenavFactory.initBrowsenav();

		var sitemapAreas = fastCopy(resolvedSitemap.data);
		sitemapAreas.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });
		for (var i = 0; i < sitemapAreas.length; i++) {
			//Generate topnav
			var folderLabel = "Default";
			if (sitemapAreas[i].folder && sitemapAreas[i].folder != "") {
				folderLabel = sitemapAreas[i].folder;
			}
			if (contentData.topNavDict.indexOf(folderLabel) == -1) {
				var topNavItem = {
					"label": folderLabel,
					"stateName": "webvella-desktop-browse",
					"stateParams": { folder: folderLabel },
					"parentName": "",
					"nodes": [],
					"weight": sitemapAreas[i].weight
				}
				topNavItem.nodes.push(sitemapAreas[i]);
				contentData.topNav.push(topNavItem);
				contentData.topNavDict.push(folderLabel);
			}
			else {
				for (var j = 0; j < contentData.topNav.length; j++) {
					if (contentData.topNav[j].label == folderLabel) {
						var currentWeight = contentData.topNav[j].weight;
						var currentNodes = contentData.topNav[j].nodes.length;

						var newAvarageWeight = (currentWeight * currentNodes + sitemapAreas[i].weight) / (currentNodes + 1);
						newAvarageWeight = Math.round(newAvarageWeight);
						contentData.topNav[j].weight = newAvarageWeight;
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

		for (var i = 0; i < contentData.topNav.length; i++) {
			webvellaDesktopTopnavFactory.addItem(contentData.topNav[i]);
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
			contentData.browsenav = browseNav;
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
		contentData.loaded = false;
		$timeout(function () {
		  contentData.loaded = true;
		},250);

		$log.debug('webvellaDesktop>browse> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	}
	//#endregion
})();
