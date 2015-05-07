/* some-name.directive.js */

/**
* @desc just a sample directive code
* @example <div sample></div> or <sample></sample>
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin')
        .directive('boz', directive);

    directive.$inject = [];

    /* @ngInject */
    function directive() {
        // Usage:
        //
        // Creates:
        //
        var directive = {
            bindToController: true,
            controller: DirectiveController,
            controllerAs: 'directiveData',
            link: link,
            restrict: 'EA',
            template: "<p>{{directiveData.title}}</p>",
            scope: {
                title:'@'
            }
        };
        return directive;

        function link(scope, element, attrs, controller) {
            //Your code here
            var toggle = true;
            element.bind('click', function () {
                console.log(scope);
                if (toggle) {
                    element.css('background-color', 'yellow');
                    toggle = !toggle;
                }
                else {
                    element.css('background-color', 'white');
                    toggle = !toggle;
                }
            });
        }

        DirectiveController.$inject = [];

        /* @ngInject */
        function DirectiveController() {
        }
    }

})();