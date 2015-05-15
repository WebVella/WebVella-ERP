/* application.constants.js */

/**
* @desc the configuration constants for working with API methods
*/

(function () {
    'use strict';
    angular
        .module('wvApp')
        .constant('wvAppConstants', {
            "debugEnabled": false,
            "apiBaseUrl": "/api/v1/en_US/",
            "apiSandboxBaseUrl": "/sandbox/api",
            "locale":"en_US"
        });
})();

// Later can be used in controller as a dependency