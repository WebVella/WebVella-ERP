/* base.module.js */

/**
* @desc this the base module of the Desktop plugin. Its only tasks is to check the topNavFactory and redirect to the first menu item state
*/

(function () {
    'use strict';

    angular
        .module('webvellaDevelopers', ['ui.router'])
        .config(config)
        .run(run)
        .controller('WebVellaDevelopersBaseController', controller);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];

    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-developers-base', {
            //abstract: true,
            parent: 'webvella-root',
            url: '/developers', //will be added to all children states
            views: {
                "pluginView": {
                    controller: 'WebVellaDevelopersBaseController',
                    templateUrl: '/plugins/webvella-developers/base.view.html',
                    controllerAs: 'pluginData'
                }
            },
            resolve: {
                //here you can resolve any plugin wide data you need. It will be available for all children states. Parent resolved objects can be injected in the functions too
                pageTitle: function (pageTitle) {
                    return "Developers | " + pageTitle;
                }
            },
            data: { }
        });
    };


    // Run //////////////////////////////////////
    run.$inject = ['$log', '$rootScope', 'webvellaDesktopBrowsenavFactory'];

    /* @ngInject */
    function run($log, $rootScope, webvellaDesktopBrowsenavFactory) {
        $log.debug('webvellaDevelopers>base> BEGIN module.run');
        $rootScope.$on('webvellaDesktop-browsenav-ready', function (event) {
            var item = {
                "label": "Developers",
                "stateName": "webvella-developers-base",
                "stateParams": {},
                "parentName": "",
                "nodes": [],
                "weight": 101.0,
                "color": "purple",
                "iconName": "code"
            };

            webvellaDesktopBrowsenavFactory.addItem(item);
        });
        $log.debug('webvellaDevelopers>base> END module.run');
    };


    // Controller ///////////////////////////////
    controller.$inject = ['$log', 'webvellaDevelopersQueryService'];

    /* @ngInject */
    function controller($log, queryService) {
        $log.debug('webvellaDevelopers>base> BEGIN controller.exec');
        /* jshint validthis:true */
        var pluginData = this;
        pluginData.executeSampleQuery = executeSampleQuery;
        pluginData.createSampleQueryDataStructure = createSampleQueryDataStructure;


        $log.debug('webvellaDevelopers>base> END controller.exec');

        function activate() {

        }

        function executeSampleQuery() {
        	$log.debug('webvellaDevelopers>base> BEGIN controller.executeSampleQuery');
        	queryService.executeSampleQuery({},
				function (response) {
					$log.debug('webvellaDevelopers>base> END controller.executeSampleQuery> SUCCESS');
					$log.debug(response);
				},
				function (response) {
					$log.debug('webvellaDevelopers>base> END controller.executeSampleQuery> ERROR');
					$log.debug(response);
				}
			);
        	
        }

        function createSampleQueryDataStructure() {
        	$log.debug('webvellaDevelopers>base> BEGIN controller.createSampleQueryDataStructure');
        	queryService.createSampleQueryDataStructure({}, 
				function (response) {
					$log.debug('webvellaDevelopers>base> END controller.createSampleQueryDataStructure> SUCCESS');
					$log.debug(response);
				},
				function (response) {
					$log.debug('webvellaDevelopers>base> END controller.createSampleQueryDataStructure> ERROR');
					$log.debug(response);
				}
			);
        }
    }

})();
