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


	DirectiveController.$inject = ['$filter', '$log', '$scope'];
	/* @ngInject */
	function DirectiveController($filter, $log, $scope) {
		$scope.relationsList = $scope.relationsList();
		$scope.listData = $scope.listData();
		$scope.listMeta = $scope.listMeta().meta;

		//#region << Fields Render >>
		//1. Auto increment
		$scope.getAutoIncrementString = function (item, record) {
			var fieldValue = record[item.dataName];
			if (!fieldValue) {
				return "";
			}
			else if (item.meta.displayFormat) {
				return item.meta.displayFormat.replace("{0}", fieldValue);
			}
			else {
				return fieldValue;
			}
		}

		//2. Checkbox
		$scope.getCheckboxString = function (item, record) {
			var fieldValue = record[item.dataName];
			if (fieldValue) {
				return "<i class='fa fa-fw fa-check go-green'></i> true";
			}
			else {
				return "<i class='fa fa-fw fa-close go-red'></i> false";
			}
		}

		//3. Currency
		$scope.getCurrencyString = function (item, record) {
			var fieldValue = record[item.dataName];
			if (!fieldValue) {
				return "";
			}
			else if (item.meta.currency != null && item.meta.currency !== {} && item.meta.currency.symbol) {
				if (item.meta.currency.symbolPlacement === 1) {
					return item.meta.currency.symbol + " " + fieldValue;
				}
				else {
					return fieldValue + " " + item.meta.currency.symbol;
				}
			}
			else {
				return fieldValue;
			}
		}

		//4 and 5. Date & DateTime 
		$scope.getDateString = function (item, record) {
			var fieldValue = record[item.dataName];
			var format = "dd MMM yyyy";
			if (item.meta.fieldType === 5) {
				format = "dd MMM yyyy HH:mm";
			}
			if (item.meta.format) {
				format = item.meta.format;
			}
			if (!fieldValue) {
				return "";
			}
			else {
				return $filter('date')(fieldValue, format);
			}
		}

		//6. Email
		$scope.getEmailString = function (item, record) {
			var fieldValue = record[item.dataName];
			if (!fieldValue) {
				return "";
			}
			else {
				//There is a problem in Angular when having in href -> the href is not rendered
				//return "<a href='mailto:" + fieldValue + "' data-rel='external'>" + fieldValue + "</a>";
				return fieldValue;
			}
		}

		//7. File upload
		$scope.getFileString = function (item, record) {
			var fieldValue = record[item.dataName];
			if (!fieldValue) {
				return "";
			}
			else {
				return '<a class="link-icon" href="' + fieldValue + '" target="_blank">view file</a>';
			}
		}

		//9. Image
		$scope.getImageString = function (item, record) {
			var fieldValue = record[item.dataName];
			if (!fieldValue) {
				return "";
			}
			else {
				return '<img class="img-thumbnail" src="' + fieldValue + '"  />';

			}
		}

		//11. Checkbox list
		$scope.getCheckboxlistString = function (item, record) {
			var fieldData = record[item.dataName];
			if (fieldData) {
				var selected = [];
				angular.forEach(item.meta.options, function (s) {
					if (fieldData.indexOf(s.key) >= 0) {
						selected.push(s.value);
					}
				});
				return selected.length ? selected.join(', ') : '';
			}
			else {
				return '';
			}
		}

		//14. Percent
		$scope.Math = window.Math;
		function multiplyDecimals(val1, val2, decimalPlaces) {
			var helpNumber = 100;
			for (var i = 0; i < decimalPlaces; i++) {
				helpNumber = helpNumber * 10;
			}
			var temp1 = $scope.Math.round(val1 * helpNumber);
			var temp2 = $scope.Math.round(val2 * helpNumber);
			return (temp1 * temp2) / (helpNumber * helpNumber);
		}


		$scope.getPercentString = function (item, record) {
			var fieldValue = record[item.dataName];

			if (!fieldValue) {
				return "empty";
			}
			else {
				//JavaScript has a bug when multiplying decimals
				//The way to correct this is to multiply the decimals before multiple their values,
				var resultPercentage = 0.00;
				resultPercentage = multiplyDecimals(fieldValue, 100, 3);
				return resultPercentage + "%";
			}

		}

		//17. Dropdown
		$scope.getDropdownString = function (item, record) {
			var selected = $filter('filter')(item.meta.options, { key: record[item.dataName] });
			return (record[item.dataName] && selected.length) ? selected[0].value : 'empty';
		}

		//19. URL
		$scope.getUrlString = function (item, record) {
			var fieldValue = record[item.dataName];
			if (!fieldValue) {
				return "";
			}
			else {
				return '<a class="link-icon" href="' + fieldValue + '" target="_blank">visit link</a>';
			}
		}


		//#endregion


	}

})();