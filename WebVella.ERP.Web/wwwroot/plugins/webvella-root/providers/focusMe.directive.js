/* valid-date.directive.js */

/**
* @desc A custom validation rule for ngMessages
* @example <input valid-date/> and than in the validation messages you can work with the $validators object
*/

(function () {
    'use strict';

    angular
        .module('webvellaRoot')
        .directive('focusMe', directive);

    directive.$inject = ['$timeout'];

    /* @ngInject */
    function directive($timeout) {
        // Usage:
        //
        // Creates:
        //
        var directive = {
            restrict: 'A',
            link: link
        };
        return directive;

        function link(scope, element, attrs) {
			 scope.$watch(attrs.focusMe, function(value) {
				if(value === true) { 
					$timeout(function(){
						element[0].focus();
					},0);
				}
			});
        }

    }

})();