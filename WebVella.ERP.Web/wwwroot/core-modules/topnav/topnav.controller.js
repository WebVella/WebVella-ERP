/* topnav.controller.js */

/**
* @desc this controller manages the top navigation section of page
*/

(function () {
    'use strict';

    angular
        .module('topnavModule', [])
        .controller('TopnavController', controller);

    // Controller ///////////////////////////////
    controller.$inject = []; 

    /* @ngInject */
    function controller() {
        /* jshint validthis:true */
        var topnavData = this;


        activate();

        function activate() { }
    }
    
})();
