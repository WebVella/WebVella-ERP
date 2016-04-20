/* some-name.provider.js */

/**
* @desc just a sample directive code
*/


(function () {
    'use strict';

    angular
        .module('appCore')
        .provider('someNameProvider', provider);


    provider.$inject = ['$stateProvider'];

    
    function providerProvider() {
        var configValue = false;

        this.setConfigValue = function (value) {
            configValue = value;
        };

        this.$get = providerFactory;

        providerFactory.$inject = ['$http'];
        function providerFactory($http) {
            var service = {
                getData: getData
            };

            return service;
            

            function getData() {                
                return configValue;
            }
        }
    }
})();