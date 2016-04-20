(function () {
    'use strict';

    angular
        .module('webvellaCore')
        .config(config)
        .controller('WebVellaCoreErrorController', controller);

    //#region << Configuration >> ///////////////////////////////////
    config.$inject = ['$stateProvider'];
    
    function config($stateProvider) {
        $stateProvider.state('webvella-core-error', {
            url: '/errors/:code?url',
            views: {
                "rootView": {
                    controller: 'WebVellaCoreErrorController',
                    templateUrl: '/plugins/webvella-core/error.view.html',
                    controllerAs: 'errorData'
                }
            },
            resolve: {}
        });
    };
	//#endregion

    //#region << Controller >> ///////////////////////////////
    controller.$inject = ['$state', 'webvellaCoreService', '$stateParams', '$timeout'];
    function controller($state, webvellaCoreService, $stateParams, $timeout) {
        var errorData = this;
        errorData.code = $stateParams.code;
        errorData.url = $stateParams.url;
        switch (errorData.code) {
            case '401':
                errorData.title = "Unauthorized";
                errorData.text = "The requested page or resource require you to authenticate.";
                break;
            case '403':
                errorData.title = "Forbidden";
                errorData.text = "The access to requested page or resource is forbidden.";
                break;
            case '404':

                errorData.title = "Not found";
                errorData.text = "The requested page or resource is not found.";
                break;
            case '500':
                errorData.title = "Internal Server Error";
                errorData.text = "An unexpected error occurred. The system administrators will be notified about it.";
                //TODO we may ask for more details about it in the future
                break;
            default:
                $timeout(function () {
                    $state.go('webvella-core-error', { 'code': '404', 'url': errorData.url });
                }, 0);
        }
        webvellaCoreService.setPageTitle("Error | " + errorData.title);
    }

	//#endregion
})();
