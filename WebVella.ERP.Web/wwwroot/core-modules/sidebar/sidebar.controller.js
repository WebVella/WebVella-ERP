/* sidebar.controller.js */

/**
* @desc this controller manages the sidebar
*/

(function () {
    'use strict';

    angular
        .module('sidebarModule', [])
        .controller('SidebarController', controller);

    // Controller ///////////////////////////////
    controller.$inject = []; 

    /* @ngInject */
    function controller() {
        /* jshint validthis:true */
        var sidebarData = this;

        sidebarData.UserName = "Wolfgang Mozart";

        activate();

        function activate() { }
    }
    
})();
