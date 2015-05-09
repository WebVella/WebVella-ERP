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

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];

    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-desktop-browse', {
            parent: "webvella-desktop-base", // the state is defined in the webvella-desktop-plugin
            url: '/browse', //  /desktop/browse after the parent state is prepended
            views: {
                "contentView": {
                    controller: 'WebVellaDesktopBrowseController',
                    templateUrl: '/plugins/webvella-desktop/browse.view.html',
                    controllerAs: 'contentData'
                }
            },
            resolve: {
                resolvedDesktopBrowseNavigation: resolveDesktopBrowseNavigation
            },
            data: { }
        });
    };


    // Run //////////////////////////////////////
    run.$inject = ['$rootScope', 'webvellaDesktopTopnavFactory'];

    /* @ngInject */
    function run($rootScope, webvellaDesktopTopnavFactory ) {

        // Push the Browse area menu and state to the desktop
        var item = {
            "label": "Browse",
            "stateName": "webvella-desktop-browse",
            "stateParams": {},
            "parentName": "",
            "nodes": [],
            "weight": 1.0
        };
        webvellaDesktopTopnavFactory.addItem(item);

    };

    // Resolve Function /////////////////////////
    resolveDesktopBrowseNavigation.$inject = ['$q', 'webvellaDesktopBrowsenavFactory','resolvedSiteMeta'];

    /* @ngInject */
    function resolveDesktopBrowseNavigation($q, webvellaDesktopBrowsenavFactory, resolvedSiteMeta) {
        var defer = $q.defer();
        var navigation = [];
        navigation = webvellaDesktopBrowsenavFactory.generateInitializeFromAreas(resolvedSiteMeta.areas);
        defer.resolve(navigation);
        // Return
        return defer.promise;
    }


    // Controller ///////////////////////////////
    controller.$inject = ['$rootScope', '$state', 'pageTitle', 'resolvedDesktopBrowseNavigation'];

    /* @ngInject */
    function controller($rootScope, $state, pageTitle, resolvedDesktopBrowseNavigation) {
        /* jshint validthis:true */
        var contentData = this;
        contentData.browsenav = resolvedDesktopBrowseNavigation;
        //listen for changes in the browsenav
        $rootScope.$on('webvellaDesktop-browsenav-updated', function (event, browsenav) {
            contentData.browsenav = browsenav;
        });
        contentData.pageTitle = "Browse Desktop | " + pageTitle;
        $rootScope.$emit('application-pageTitle', contentData.pageTitle);


        activate();

        function activate() { }
    }

})();
