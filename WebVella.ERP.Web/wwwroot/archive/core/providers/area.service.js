/* area.service.js */

/**
* @desc all actions with site area
*/

(function () {
    'use strict';

    angular
        .module('appCore')
        .service('areaService', service);

    service.$inject = ['$http', '$rootScope', 'apiConstants','$state'];

    /* @ngInject */
    function service($http, $rootScope, apiConstants, $state) {
        var serviceInstance = this;

        serviceInstance.getAreaByName = getAreaByName;
        serviceInstance.navigateToArea = navigateToArea;

        //// Go to Area method - Navigation to the area's default location //////////////////////////////////////////
        function navigateToArea(areaName) {
            //Get the requested area from the site meta object
            var requestedArea = null;
            var firstEntityName = null;
            var firstEntitySectionName = null;


            for (var i = 0; i < $rootScope.siteMeta.areas.length; i++) {
                if ($rootScope.siteMeta.areas[i].name == areaName) {
                    requestedArea = $rootScope.siteMeta.areas[i];
                }
            }

            if (requestedArea == null) {
                alert("No area with this name is found");
                return;
            }

            //Navigate to the first entity - the site meta Object is already sorted in the service
            for (var i = 0; i < requestedArea.sections.length; i++) {
                if (requestedArea.sections[i].entities.length > 0) {
                    firstEntityName = requestedArea.sections[i].entities[0].name;
                    firstEntitySectionName = requestedArea.sections[i].name;
                    break;
                }
            }
            if (firstEntityName != null) {
                $state.go('entityList', { areaName: areaName, sectionName: firstEntitySectionName, entityName: firstEntityName });
            }
            else {
                //If no entities related raise error and cancel navigation
                alert("This area has no entities attached");
            }

        }


        //// Get area by name from the $rootScope meta object
        function getAreaByName(areaName) {
            var requestedArea = null;
            for (var i = 0; i < $rootScope.siteMeta.areas.length; i++) {
                if ($rootScope.siteMeta.areas[i].name == areaName) {
                    requestedArea = $rootScope.siteMeta.areas[i];
                }
            }

            return requestedArea;
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
                //Updating the application siteMetaValue
                $rootScope.siteMeta = data.object;
                successCallBack();
            }
        }

    }
})();