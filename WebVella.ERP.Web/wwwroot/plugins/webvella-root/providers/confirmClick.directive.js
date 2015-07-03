/* valid-date.directive.js */

/**
* @desc Requires confirmation before executing function
* @example <button confirmed-click="sayHi()" ng-confirm-click="Would you like to say hi?">Say hi to {{ name }}</button>
*/

(function () {
    'use strict';

    angular
        .module('webvellaRoot')
        .directive('ngConfirmClick', directive);

    directive.$inject = [];

    /* @ngInject */
    function directive() {
    	return {
    		link: function (scope, element, attr) {
    			var msg = attr.ngConfirmClick || "Are you sure?";
    			var clickAction = attr.confirmedClick;
    			element.bind('click', function (event) {
    				if (window.confirm(msg)) {
    					scope.$eval(clickAction)
    				}
    			});
    		}
    	};
    }

})();