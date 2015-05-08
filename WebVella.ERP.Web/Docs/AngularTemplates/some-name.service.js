/* some-name.service.js */

/**
* @desc just a sample service code
*/

(function () {
    'use strict';

    angular
        .module('appCore')
        .service('someNameService', service);

    service.$inject = ['$http'];
    
    /* @ngInject */
    function service($http) {
        var self = this;

        self.getData = getData;
        
        /////////////
        function getData() { }
    }
})();