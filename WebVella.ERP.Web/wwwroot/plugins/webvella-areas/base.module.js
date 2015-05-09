/* base.module.js */

/**
* @desc this the base module of the Desktop plugin
*/

(function () {
    'use strict';

    angular
        .module('webvellaAreas', ['ui.router'])
        .config(config)
        .run(run)
        .controller('WebVellaAreasBaseController', controller);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];

    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-areas-base', {
           abstract: true,
           parent: 'webvella-root',
            url: '/areas', //will be added to all children states
            views: {
                "pluginView": {
                    controller: 'WebVellaAreasBaseController',
                    templateUrl: '/plugins/webvella-areas/base.view.html',
                    controllerAs: 'pluginData'
                }
            },
            resolve: {
                //here you can resolve any plugin wide data you need. It will be available for all children states. Parent resolved objects can be injected in the functions too
                pageTitle: function (pageTitle) {
                    return "Areas | " + pageTitle;
                }
            },
            data: {
                //Custom data is inherited by the parent state 'webvella-root', but it can be overwritten if necessary. Available for all child states in this plugin
            }
        });
    };


    // Run //////////////////////////////////////
    run.$inject = [];

    /* @ngInject */
    function run() { };


    // Resolve Function /////////////////////////
    resolvingApplicationAreas.$inject = ['$q'];

    /* @ngInject */
    function resolvingApplicationAreas($q) {
        // Initialize
        var defer = $q.defer();

        //call the API and get the list of all areas
        webvellaAreasService.getAllAreas(successCallback, errorCallback);

        //On success, push the areas object to the factory, so it is accessible by other states
        function successCallback(data) {
            // Process
            defer.resolve(data);
        }

        function errorCallback(data) {
            defer.resolve(data);
        }



        // Return
        return defer.promise;
    }


    // Controller ///////////////////////////////
    controller.$inject = [];

    /* @ngInject */
    function controller() {
        /* jshint validthis:true */
        var pluginData = this;

        activate();

        function activate() { }
    }

})();
