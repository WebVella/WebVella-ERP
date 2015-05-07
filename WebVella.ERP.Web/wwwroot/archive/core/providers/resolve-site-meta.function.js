/* resolve-site-meta.controller.js */

/**
* @desc called in each state.config.resolve. Prepares the environment before the state load - > siteMeta
*/


//!!! GLOBAL FUNCTION !!!//

'use strict';

var ResolveSiteMeta = function ($q, $state, siteMetaService) {
    // Initialize
    var defer = $q.defer();

    // Process
    function successCallBack(response) {
        defer.resolve(response);
    }

    function errorCallBack(response) {
        defer.resolve(response);
    }

    siteMetaService.getUpdateSiteMeta(successCallBack, errorCallBack);

    // Return
    return defer.promise;
};





