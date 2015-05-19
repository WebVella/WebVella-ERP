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
                resolvedAreasList: resolveAreasList
            },
            data: {}
        });
    };
    //#endregion

    //#region << Run >>
    run.$inject = ['$log', '$rootScope', 'webvellaDesktopTopnavFactory', 'webvellaDesktopBrowsenavFactory'];

    /* @ngInject */
    function run($log, $rootScope, webvellaDesktopTopnavFactory, webvellaDesktopBrowsenavFactory) {
        $log.debug('webvellaDesktop>browse> BEGIN module.run');

        //Initialize the pluggable object made with factories, always when state is changed. (it fixes the duplication problem with browser back and forward buttons)
        //$rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
        //    webvellaDesktopBrowsenavService.initBrowsenav();
        //})

        $log.debug('webvellaDesktop>browse> END module.run');
    };
    //#endregion

    //#region << Resolve Function >>
    resolveAreasList.$inject = ['$q', '$log', 'webvellaRootService'];

    /* @ngInject */
    function resolveAreasList($q, $log, webvellaRootService) {
        $log.debug('webvellaDesktop>browse> BEGIN state.resolved');
        // Initialize
        var defer = $q.defer();

        // Process
        function successCallback(response) {
            defer.resolve(response.object);
        }

        function errorCallback(response) {
            defer.resolve(response.object);
        }

        webvellaRootService.getEntityRecordsByName("area", successCallback, errorCallback);

        // Return
        $log.debug('webvellaDesktop>browse> END state.resolved');
        return defer.promise;
    }
    //#endregion


    //#region << Controller >>
    controller.$inject = ['$log', '$rootScope', '$scope', '$state', 'pageTitle', 'webvellaRootService', 'resolvedAreasList', 'webvellaDesktopBrowsenavFactory'];

    /* @ngInject */
    function controller($log, $rootScope, $scope, $state, pageTitle, webvellaRootService, resolvedAreasList, webvellaDesktopBrowsenavFactory) {
        $log.debug('webvellaDesktop>browse> BEGIN controller.exec');
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
            for (var i = 0; i < resolvedAreasList.entities.length; i++) {
                var menuItem = webvellaDesktopBrowsenavFactory.generateMenuItemFromArea(resolvedAreasList.entities[i]);
                webvellaDesktopBrowsenavFactory.addItem(menuItem);
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
        $log.debug('webvellaDesktop>browse> END controller.exec');
        function activate() { }
    }
    //#endregion
})();
