/* recursive-list.directive.js */

/**
* @desc Recursive records list
* @example <recursive-list></sub-view>
*/

(function () {
	'use strict';

	angular
        .module('webvellaAreas')
        .directive('recursiveList', directive);

	directive.$inject = ['$compile', '$templateRequest', 'RecursionHelper', 'webvellaAdminService'];

	/* @ngInject */
	function directive($compile, $templateRequest, RecursionHelper, webvellaAdminService) {
		//Text Binding (Prefix: @)
		//One-way Binding (Prefix: &)
		//Two-way Binding (Prefix: =)
		var directive = {
			controller: DirectiveController,
			templateUrl: '/plugins/webvella-areas/providers/recursive-list.template.html',
			restrict: 'E',
			scope: {
				recordsData: '&',
				itemMeta: '&',
				relationsList: '&'
			},
			compile: function (element) {
				return RecursionHelper.compile(element, function (scope, iElement, iAttrs, controller, transcludeFn) {
					// Define your normal link function here.
					// Alternative: instead of passing a function,
					// you can also pass an object with 
					// a 'pre'- and 'post'-link function.
				});
			}
		};
		return directive;



		DirectiveController.$inject = ['$filter', '$log', '$scope'];
		/* @ngInject */
		function DirectiveController($filter, $log, $scope) {

		}


	}

})();