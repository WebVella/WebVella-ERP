/* recursive-list.directive.js */

/**
* @desc Recursive records list
* @example 	<recursive-list list-meta="contentData.recordsMeta" list-data="contentData.records" relations-list="contentData.relationsMeta"></recursive-list>
 * 
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
				listData: '&',
				listMeta: '&',
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
	}


	DirectiveController.$inject = ['$filter', '$log', '$scope','webvellaAreasService'];
	/* @ngInject */
	function DirectiveController($filter, $log, $scope, webvellaAreasService) {
		$scope.relationsList = $scope.relationsList();
		$scope.listData = $scope.listData();
		$scope.listMeta = $scope.listMeta().meta;

		//#region << Columns render>> //////////////////////////////////////
		//1.Auto increment
		$scope.getAutoIncrementString = webvellaAreasService.getAutoIncrementString;
		//2.Checkbox
		$scope.getCheckboxString = webvellaAreasService.getCheckboxString;
		//3.Currency
		$scope.getCurrencyString = webvellaAreasService.getCurrencyString;
		//4.Date
		$scope.getDateString = webvellaAreasService.getDateString;
		//5.Datetime
		$scope.getDateTimeString = webvellaAreasService.getDateTimeString;
		//6.Email
		$scope.getEmailString = webvellaAreasService.getEmailString;
		//7.File
		$scope.getFileString = webvellaAreasService.getFileString;
		//8.Html
		$scope.getHtmlString = webvellaAreasService.getHtmlString;
		//9.Image
		$scope.getImageString = webvellaAreasService.getImageString;
		//10.Textarea
		$scope.getTextareaString = webvellaAreasService.getTextareaString;
		//11.Multiselect
		$scope.getMultiselectString = webvellaAreasService.getMultiselectString;
		//12.Number
		$scope.getNumberString = webvellaAreasService.getNumberString;
		//13.Password
		$scope.getPasswordString = webvellaAreasService.getPasswordString;
		//14.Percent
		$scope.getPercentString = webvellaAreasService.getPercentString;
		//15.Phone
		$scope.getPhoneString = webvellaAreasService.getPhoneString;
		//15.Guid
		$scope.getGuidString = webvellaAreasService.getGuidString;
		//17.Dropdown
		$scope.getDropdownString = webvellaAreasService.getDropdownString;
		//18. Text
		$scope.getTextString = webvellaAreasService.getTextString;
		//18.Url
		$scope.getUrlString = webvellaAreasService.getUrlString;
		//#endregion

	}

})();