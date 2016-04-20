/* some-name.directive.js */

/**
* @desc just a sample directive code
* @example <div sample></div> or <sample></sample>
*/

(function() {
    'use strict';

    angular
        .module('appCore')
        .directive('someNameDirective', directive);

    directive.$inject = ['dependencies'];
    
    
    function directive (dependencies) {
        // Usage:
        //
        // Creates:
        //
        var directive = {
            bindToController: true,
            controller:DirectiveController,
            controllerAs: 'vm',
            link: link,
            restrict: 'EA',
            scope: {}
        };
        return directive;

        function link(scope, element, attrs, controller) {
            //Your code here
        }
        
        DirectiveController.$inject = ['dependencies'];

        
        function DirectiveController(dependencies) {
        }
    }

})();