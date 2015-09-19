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
            url: '/desktop/browse', //  /desktop/browse after the parent state is prepended
            views: {
                "contentView": {
                    controller: 'WebVellaDesktopBrowseController',
                    templateUrl: '/plugins/webvella-desktop/browse.view.html',
                    controllerAs: 'contentData'
                }
            },
            resolve: {
                resolvedSitemap: resolveSitemap
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
            defer.resolve(response.object);
        }

        webvellaRootService.getSitemap(successCallback, errorCallback);

        // Return
        $log.debug('webvellaDesktop>browse> END state.resolved ' + moment().format('HH:mm:ss SSSS'));
        return defer.promise;
    }
    //#endregion


    //#region << Controller >>
    controller.$inject = ['$log', '$rootScope', '$scope', '$state', 'pageTitle', 'webvellaRootService', 'resolvedSitemap', 'webvellaDesktopBrowsenavFactory'];

    /* @ngInject */
    function controller($log, $rootScope, $scope, $state, pageTitle, webvellaRootService, resolvedSitemap, webvellaDesktopBrowsenavFactory) {
    	$log.debug('webvellaDesktop>browse> BEGIN controller.exec ' + moment().format('HH:mm:ss SSSS'));
        /* jshint validthis:true */
        var contentData = this;
        contentData.browsenav = [];

        //#region << Set Page title >>
        contentData.pageTitle = "Browse Desktop | " + pageTitle;
        webvellaRootService.setPageTitle(contentData.pageTitle);
        //#endregion

        //#region << Make the Browsenav pluggable & Initialize>>
        ////1. CONSTRUCTOR - initialize the factory
        webvellaDesktopBrowsenavFactory.initBrowsenav();
        ////2. READY hook listener
        var readyBrowsenavDestructor = $rootScope.$on("webvellaDesktop-browsenav-ready", function (event, data) {
        	var sitemapAreas = angular.copy(resolvedSitemap.data);
        	sitemapAreas.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });
			for (var i = 0; i < sitemapAreas.length; i++) {
        		var menuItem = webvellaDesktopBrowsenavFactory.generateMenuItemFromArea(resolvedSitemap.data[i]);
                if (menuItem != null) {
                	webvellaDesktopBrowsenavFactory.addItem(menuItem);
                }
            };
        })
        ////3. UPDATED hook listener
        var updateBrowsenavDestructor = $rootScope.$on("webvellaDesktop-browsenav-updated", function (event, data) {
            contentData.browsenav = data;
        });
        ////4. DESCTRUCTOR - hook listeners remove on scope destroy. This avoids duplication, as rootScope is never destroyed and new controller load will duplicate the listener
        $scope.$on("$destroy", function () {
            readyBrowsenavDestructor();
            updateBrowsenavDestructor();
        });
        ////5. Bootstrap the pluggable Browsenav
        $rootScope.$emit("webvellaDesktop-browsenav-ready");
        //#endregion



        activate();
        $log.debug('webvellaDesktop>browse> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
        function activate() { }
    }
    //#endregion
})();
