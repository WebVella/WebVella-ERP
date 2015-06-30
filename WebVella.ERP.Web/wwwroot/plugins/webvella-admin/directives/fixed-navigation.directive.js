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


            function onResize(e) {
                var rect = element[0].getBoundingClientRect();
                var clw = ($window.innerWidth || document.documentElement.clientWidth);
                var clh = ($window.innerHeight || document.documentElement.clientHeight);
                var offsetY = $window.pageYOffset;
                
                var bottomScrollHeight = 50;

            	//var tabsElement = document.getElementByClass('tabs-header');
                var tabsElement = document.getElementsByClassName("nav-tabs");
                var tabsElementHeight = tabsElement[0].getBoundingClientRect().height;
                var sideDropZoneHeight = clh - rectInitial.top - bottomScrollHeight;


                var styles = {
                    "position": "fixed",
                    "right": '14px',
                    "background": "white",
					"overflow-y":"scroll",
                    "display": 'block',
                    "top": '14px',
                    "width": rect.width + "px",
                    "height":sideDropZoneHeight + "px"
                };
                var reverseStyles = {
                    "position": "static",
                    "right": 'auto',
                    "background": "white",
                    "overflow-y": "scroll",
                    "display": 'block',
                    "top": 'auto',
                    "width": "auto",
                    "height": sideDropZoneHeight + "px"
                };
                if (offsetY > rectInitial.top) {
                    element.css(styles);
                }
                else {
                    element.css(reverseStyles);
                }
                
            }
            
            onResize();

            //bind to the event
            angular.element($window).bind('resize scroll', onResize);

            //unbind the event listener
            scope.$on('$destroy', function () {
                angular.element($window).unbind('resize scroll', onResize);
            });
        }

    }

})();