/* area.factory.js */

/**
* @desc just a sample factory code
*/

(function () {
    'use strict';
    angular
        .module('webvellaAreas')
        .factory('webvellaAreasFactory', factory);

    factory.$inject = ['$rootScope'];

    /* @ngInject */
    function factory($rootScope) {
        var areas = [];
        var exports = {
            addNewArea: addNewArea,
            getAreas: getAreas,
            updateAreas: updateAreas
        };
        //Some code

        return exports;

        ////////////////

        function addNewArea(object) {
            areas.push(object);
            areas.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight); });
            $rootScope.$emit('webvellaAreas-areas-updated', areas);
        }

        function updateAreas(object) {
            areas = object;
            areas.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight); });
            $rootScope.$emit('webvellaAreas-areas-updated', areas);
        }


        function getAreas() {
            return areas;
        }
    }
})();