/* dynamic-view.directive.js */

(function () {
	'use strict';

	angular
        .module('webvellaAreas')
        .directive('dynamicView', dynamicView);

	dynamicView.$inject = ['$compile', '$templateRequest','RecursionHelper'];

	/* @ngInject */
	function dynamicView($compile, $templateRequest, RecursionHelper) {
		//Text Binding (Prefix: @) - only strings
		//One-way Binding (Prefix: &) - $scope functions
		//Two-way Binding (Prefix: =) -$scope.properties
		var directive = {
			controller: DirectiveController,
			template: '<ng-include src="getTemplateUrl()"/>',
			restrict: 'E',
			scope: {
				templateData: '=?',
				templateMeta: '=?',
				templateUrl: '=?'
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
	}


	DirectiveController.$inject = ['$scope'];
	/* @ngInject */
	function DirectiveController($scope) {
		$scope.templateData = $scope.templateData;
		$scope.templateMeta = $scope.templateMeta;
		$scope.getTemplateUrl = function(){
			if($scope.templateUrl == null){
				return "/plugins/webvella-areas/assets/no-template.html"
			}
			else{
				return 	$scope.templateUrl;
			}
		}
	}

})();


/* dynamic-list.directive.js */

(function () {
	'use strict';

	angular
        .module('webvellaAreas')
        .directive('dynamicList', dynamicView);

	dynamicView.$inject = ['$compile', '$templateRequest','RecursionHelper'];

	/* @ngInject */
	function dynamicView($compile, $templateRequest, RecursionHelper) {
		//Text Binding (Prefix: @) - only strings
		//One-way Binding (Prefix: &) - $scope functions
		//Two-way Binding (Prefix: =) -$scope.properties
		var directive = {
			controller: DirectiveController,
			template: '<ng-include src="getTemplateUrl()"/>',
			restrict: 'E',
			scope: {
				templateData: '=?',
				templateMeta: '=?',
				templateUrl: '=?'
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
	}


	DirectiveController.$inject = ['$scope'];
	/* @ngInject */
	function DirectiveController($scope) {
		$scope.templateData = $scope.templateData;
		$scope.templateMeta = $scope.templateMeta;
		$scope.getTemplateUrl = function(){
			if($scope.templateUrl == null){
				return "/plugins/webvella-areas/assets/no-template.html"
			}
			else{
				return 	$scope.templateUrl;
			}
		}
	}

})();