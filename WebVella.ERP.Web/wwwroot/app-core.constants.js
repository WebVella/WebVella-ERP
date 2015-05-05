/* app-core.constants.js */

/**
* @desc the configuration constants of the application
*/

(function () {
    'use strict';

    angular
        .module('appCore')
        .constant('apiConstants', {
            "baseUrl": "/api/"
        });

})();

// Later can be used in controller as a dependency