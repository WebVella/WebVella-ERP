/* dynamic-html.directive.js */

(function () {
	'use strict';

	angular
        .module('webvellaAreas')
        .directive('dynamicHtml', dynamicHtml);

	dynamicHtml.$inject = ['$compile', '$templateRequest', 'RecursionHelper'];


	function dynamicHtml($compile, $templateRequest, RecursionHelper) {
		var directive = {
			restrict: 'A',
			compile: function (element) {

				return RecursionHelper.compile(element, function (scope, iElement, iAttrs, controller, transcludeFn) {
					scope.$watch(iAttrs.template, function (html) {
						iElement.html(html);
						$compile(iElement.contents())(scope);
					});
				});
			}
		};
		return directive;
	}

})();
