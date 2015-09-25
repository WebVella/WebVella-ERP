/* home.module.js */

/**
* @desc handles the "/" url. Manages a login form and redirects the user to "webvella-desktop-areas" state if authenticated successfully. The only module that should be accessed by not logged user
*/

(function () {
    'use strict';

    angular
        .module('webvellaRoot')  //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .run(run)
        .controller('WebVellaRootLoginController', controller);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];
    /* @ngInject */
    function config($stateProvider) {
    	$stateProvider.state('webvella-root-login', {
        	url: '/login',
            views: {
                "rootView": {
                    controller: 'WebVellaRootLoginController',
                    templateUrl: '/plugins/webvella-root/login.view.html',
                    controllerAs: 'loginData'
                }
            },
            resolve: {
            	pageTitle: function () {
            		return "Webvella ERP";
            	}
            }
        });
    };

    // Run //////////////////////////////////////
    run.$inject = ['$log'];
    /* @ngInject */
    function run($log) {
    	$log.debug('webvellaRoot>login> BEGIN module.run ' + moment().format('HH:mm:ss SSSS'));

    	$log.debug('webvellaRoot>login> END module.run ' + moment().format('HH:mm:ss SSSS'));
    };


    // Controller ///////////////////////////////
    controller.$inject = ['$state', '$log', 'webvellaRootService', '$timeout', 'pageTitle'];

    /* @ngInject */
    function controller($state, $log, webvellaRootService, $timeout, pageTitle) {
    	$log.debug('webvellaRoot>login> BEGIN controller.exec ' + moment().format('HH:mm:ss SSSS'));
        /* jshint validthis:true */
    	var loginData = this;
    	var currentUser = webvellaRootService.getCurrentUser();
        if (currentUser != null) {
        	$timeout(function () {
        		$state.go("webvella-desktop-browse");
        	}, 0);
        }

        loginData.email = "erp@webvella.com";
        loginData.password = "ttg";
        loginData.rememberMe = false;
        loginData.pageTitle = "Login | " + pageTitle;
        webvellaRootService.setPageTitle(loginData.pageTitle);

        loginData.ValidationErrors = false;

        webvellaRootService.setPageTitle(loginData.pageTitle);
        activate();
        $log.debug('webvellaRoot>login> END controller.exec ' + moment().format('HH:mm:ss SSSS'));

        loginData.doLogin = function(){
            webvellaRootService.login( loginData,
                                      function (response ) {
                                          $timeout(function () {
                                              $state.go('webvella-desktop-browse');
                                          }, 0);
                                      },
                                      function (response) {
                                         //show validation
                                      });
        }

        function activate() {

        }
    }

})();
