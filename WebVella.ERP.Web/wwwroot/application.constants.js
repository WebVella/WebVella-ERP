/* application.constants.js */

/**
* @desc the configuration constants for working with API methods
*/

(function () {
    'use strict';
    angular
        .module('wvApp')
        .constant('wvAppConstants', {
            "debugEnabled": true,
            "apiBaseUrl": "/api"
        });
})();

// Later can be used in controller as a dependency