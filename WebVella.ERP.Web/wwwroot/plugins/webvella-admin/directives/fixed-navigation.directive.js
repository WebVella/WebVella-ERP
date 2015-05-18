/* some-name.directive.js */

/**
* @desc just a sample directive code
* @example <div sample></div> or <sample></sample>
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin')
        .directive('fixedNavigation', directive);

    directive.$inject = ['$window'];

    /* @ngInject */
    function directive($window) {
        // Usage:
        //
        // Creates:
        //
        var directive = {
            link: link,
            restrict: 'A',
            scope: {
                
            }
        };
        return directive;

        function link(scope, element, attrs) {

            var rectInitial = element[0].getBoundingClientRect();


            function onResize(el) {
                var rect = el[0].getBoundingClientRect();
                var clw = ($window.innerWidth || document.documentElement.clientWidth);
                var clh = ($window.innerHeight || document.documentElement.clientHeight);
                var offsetY = $window.pageYOffset;
                
                var bottomScrollHeight = 50;

                var tabsElement = document.getElementById('tabs-header');
                var tabsElementHeight = tabsElement.getBoundingClientRect().height;
                var sideDropZoneHeight = clh - rectInitial.top - bottomScrollHeight - tabsElementHeight;
                var tabsPane = document.getElementById('tabs-pane');
                tabsPane.style.height = sideDropZoneHeight + "px";


                var styles = {
                    "position": "fixed",
                    "right": '14px',
                    "background": "white",
                    "display": 'block',
                    "top": '14px',
                    "width":rect.width + "px"
                };
                var reverseStyles = {
                    "position": "static",
                    "right": 'auto',
                    "background": "white",
                    "display": 'block',
                    "top": 'auto',
                    "width": "auto"
                };
                if (offsetY > rectInitial.top) {
                    el.css(styles);
                }
                else {
                    el.css(reverseStyles);
                }
                
            }
            
            onResize(element);

            angular.element($window).bind('resize scroll', function () {
                onResize(element);
            });
        }

    }

})();