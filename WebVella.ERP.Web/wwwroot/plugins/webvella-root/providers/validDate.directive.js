/* valid-date.directive.js */

/**
* @desc A custom validation rule for ngMessages
* @example <input valid-date/> and than in the validation messages you can work with the $validators object
*/

(function () {
    'use strict';

    angular
        .module('webvellaRoot')
        .directive('validDate', directive);

    directive.$inject = [];

    /* @ngInject */
    function directive() {
        // Usage:
        //
        // Creates:
        //
        var directive = {
            restrict: 'A',
            require: "ngModel",
            link: link
        };
        return directive;

        function link(scope, element, attrs, ngModel) {
            ngModel.$validators.validDate = function (modelValue) {
                //moment().toISOString();
                if (modelValue === "") {
                    return true;
                } else {
                    if (moment(modelValue).isValid()) {
                        return true;
                    } else {
                        return false;
                    }
                }
            }
        }

    }

})();