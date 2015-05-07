/* site-meta.service.js */

/**
* @desc this service manages all aspects of the siteMetaValue object, that is later available from all processes and provides the core application structure
*/

(function () {
    'use strict';

    angular
        .module('wvApp')
        .service('siteMetaService', service);

    service.$inject = ['$http','$rootScope','apiConstants'];

    /* @ngInject */
    function service($http, $rootScope, apiConstants) {
        var serviceInstance = this;

        serviceInstance.getUpdateSiteMeta = getUpdateSiteMeta;

        //// Get Site Meta method //////////////////////////////////////////
        // via $rootScope.siteMetaObject;


        //// Update Site Meta method //////////////////////////////////////////
        function getUpdateSiteMeta(successCallback, errorCallback) {
            $http({ method: 'GET', url: apiConstants.baseUrl + 'site/meta' }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }


        //// Aux methods //////////////////////////////////////////////////////

        // Global functions for result handling for all methods of this service
        function handleErrorResult(data, status, errorCallBack) {
            switch (status) {
                case 400:
                    if (errorCallBack === undefined || typeof (errorCallBack) != "function") {
                        alert("The errorCallback argument is not a function or missing");
                        return;
                    }
                    data.success = false;
                    errorCallBack();
                    break;
                default:
                    alert("An API call finished with error: " + status);
                    break;
            }
        }

        function handleSuccessResult(data, status, successCallBack, errorCallBack) {
            if (successCallBack === undefined || typeof (successCallBack) != "function") {
                alert("The successCallback argument is not a function or missing");
                return;
            }

            if (!data.success) {
                //when the validation errors occurred
                if (errorCallBack === undefined || typeof (errorCallBack) != "function") {
                    alert("The errorCallBack argument in handleSuccessResult is not a function or missing");
                    return;
                }
                status = 400;//Bad request
                errorCallBack();
            }
            else {
                //Updating the application siteMetaValue but first sorting 
                var siteMeta = data.object;
                
                //Sort areas
                siteMeta.areas.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });
                
                //Sections sort
                for (var i = 0; i < siteMeta.areas.length; i++) {
                    siteMeta.areas[i].sections.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });

                    //Sort entities
                    for (var j = 0; j < siteMeta.areas[i].sections.length; j++) {
                        siteMeta.areas[i].sections[j].entities.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });
                    }
                }

                $rootScope.siteMeta = siteMeta;
                successCallBack(data);
            }
        }

    }
})();