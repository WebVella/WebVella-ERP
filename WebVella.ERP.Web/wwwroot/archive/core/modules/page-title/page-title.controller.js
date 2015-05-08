/* page-title.controller.js */

/**
* @desc this controller manages the actions in the page title panel
*/

(function () {
    'use strict';

    angular
        .module('pageTitleModule', [])
        .controller('PageTitleController', controller);

    // Controller ///////////////////////////////
    controller.$inject = []; 

    /* @ngInject */
    function controller() {
        /* jshint validthis:true */
        var areaNavigationData = this;


        activate();

        function activate() { }
    }
    
})();
