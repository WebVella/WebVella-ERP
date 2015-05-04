/* some-name.service.js */

/**
* @desc just a sample service code
*/

(function () {
    'use strict';

    angular
        .module('app')
        .service('someNameService', service);

    service.$inject = ['$http'];
    
    /* @ngInject */
    function service($http) {
        this.getData = getData;
        
        /////////////
        function getData() { }
    }
})();