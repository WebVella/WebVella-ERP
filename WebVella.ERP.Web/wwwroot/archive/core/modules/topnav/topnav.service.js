/* topnav.service.js */

/**
* @desc just a sample service code
*/

(function () {
    'use strict';

    angular
        .module('appCore')
        .service('topnavService', service);

    service.$inject = ['$http'];

    /* @ngInject */
    function service($http) {
        this.getData = getData;

        /////////////
        function getData() { }
    }
})();