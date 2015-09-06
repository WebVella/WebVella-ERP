/* some-name.directive.js */

/**
* @desc record view - subview template
* @example <sub-view></sub-view>
*/

(function () {
	'use strict';

	angular
        .module('webvellaAreas')
        .directive('subView', directive);

	directive.$inject = ['webvellaAdminService'];

	/* @ngInject */
	function directive(webvellaAdminService) {
		//Text Binding (Prefix: @)
		//One-way Binding (Prefix: &)
		//Two-way Binding (Prefix: =)
		var directive = {
			controller: DirectiveController,
			templateUrl: '/plugins/webvella-areas/providers/subview.template.html',
			link: link,
			restrict: 'E',
			scope: {
				recordsData: '&',
				itemMeta: '&',
				relationsList: '&'
			}
		};
		return directive;



		function link(scope, element, attrs, controller) {
			//Your code here
			
		}

		DirectiveController.$inject = ['$filter','$log','$scope'];
		/* @ngInject */
		function DirectiveController($filter,$log, $scope) {
			//#region << Init >>
			//var directiveData = this;
			$scope.itemEntityId = $scope.itemMeta().entityId;
			$scope.itemMeta = $scope.itemMeta().meta;
			$scope.relationsList = $scope.relationsList();
			$scope.selectedRegion = null;
			for (var i = 0; i < $scope.itemMeta.regions.length; i++) {
				if ($scope.itemMeta.regions[i].name === "content") {
					$scope.selectedRegion = $scope.itemMeta.regions[i];
				}
			}
			if ($scope.selectedRegion == null) {
				$log.error("the subview: " + $scope.itemMeta.name + " does not have a content region");
			}
			$scope.recordsData = $scope.recordsData();

			//SubViews sections collapsed state - depends on sectionId and recordId as there could be multiple records presented with the same view (section id)
			$scope.sectionCollapsedData = [];
			//Init all sections collapsed state with section[id] and record[id] key

			//Find the records for this view
			var recordIdArray = [];
			for (var l = 0; l < $scope.recordsData.length; l++) {
				recordIdArray.push($scope.recordsData.id);
			}
			for (var m = 0; m < $scope.selectedRegion.sections.length; m++) {
				$scope.sectionCollapsedData[$scope.selectedRegion.sections[m].id] = {};
				for (var n = 0; n < recordIdArray.length; n++) {
					$scope.sectionCollapsedData[$scope.selectedRegion.sections[m].id][recordIdArray[n]] = $scope.selectedRegion.sections[m].collapsed;
				}
			}
			//#endregion

			//#region << Logic >>
			$scope.toggleSectionCollapse = function (sectionId, recordId) {
				$scope.sectionCollapsedData[sectionId][recordId] = !$scope.sectionCollapsedData[sectionId][recordId];
			}
			//#endregion

			//#region << Field Render logic >>
			//1. Auto increment
			$scope.getAutoIncrementString = function (item,record) {
				var fieldValue = record[item.dataName];
				if (!fieldValue) {
					return "empty";
				}
				else if (item.meta.displayFormat) {
					return item.meta.displayFormat.replace("{0}", fieldValue);
				}
				else {
					return fieldValue;
				}
			}

			//2. Checkbox
			$scope.getCheckboxString = function (item,record) {
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
					return "empty";
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

			//7. File upload
			$scope.getFileString = function (item, record) {
				var fieldValue = record[item.dataName];
				if (!fieldValue) {
					return "";
				}
				else {
					return '<a class="link-icon" href="'+ fieldValue +'" target="_blank">view file</a>';
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


			$scope.getPercentString = function (item,record) {
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
			$scope.getDropdownString = function (item,record) {
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

			//#region << Field Relation render >>

			$scope.getRelation = function (relationName) {
				for (var i = 0; i < $scope.relationsList.length; i++) {
					if ($scope.relationsList[i].name === relationName) {
						//set current entity role
						if ($scope.itemEntityId === $scope.relationsList[i].targetEntityId && $scope.itemEntityId === $scope.relationsList[i].originEntityId) {
							$scope.relationsList[i].currentEntityRole = 3; //both origin and target
						}
						else if ($scope.itemEntityId === $scope.relationsList[i].targetEntityId && $scope.itemEntityId !== $scope.relationsList[i].originEntityId) {
							$scope.relationsList[i].currentEntityRole = 2; //target
						}
						else if ($scope.itemEntityId !== $scope.relationsList[i].targetEntityId && $scope.itemEntityId === $scope.relationsList[i].originEntityId) {
							$scope.relationsList[i].currentEntityRole = 1; //origin
						}
						else if ($scope.itemEntityId !== $scope.relationsList[i].targetEntityId && $scope.itemEntityId !== $scope.relationsList[i].originEntityId) {
							$scope.relationsList[i].currentEntityRole = 0; //possible problem
						}
						return $scope.relationsList[i];
					}
				}
				return null;
			}

			$scope.getRelatedFieldSingleHtml = function(item, record) {
				var htmlString = "&nbsp;";
				switch (item.meta.fieldType) {
					case 1:
						htmlString = $scope.getAutoIncrementString(item, record);
						break;
					case 2:
						htmlString = $scope.getCheckboxString(item, record);
						break;
					case 3:
						htmlString = $scope.getCurrencyString(item, record);
						break;
					case 4:
						htmlString = $scope.getDateString(item, record);
						break;
					case 5:
						htmlString = $scope.getDateString(item, record);
						break;
					case 7:
						htmlString = $scope.getFileString(item, record);
						break;
					case 9:
						htmlString = $scope.getImageString(item, record);
						break;
					case 11:
						htmlString = $scope.getCheckboxlistString(item, record);
						break;
					case 14:
						htmlString = $scope.getPercentString(item, record);
						break;
					case 17:
						htmlString = $scope.getDropdownString(item, record);
						break;
					case 19:
						htmlString = $scope.getUrlString(item, record);
						break;
					default:
						if (record[item.dataName].length > 0) {
							htmlString = record[item.dataName][0];
						}
						break;
				}

				return htmlString;
			}

			$scope.getRelatedFieldMultiHtml = function (item, record) {
				var htmlString = "<ul>";
				switch (item.meta.fieldType) {
					default:
						for (var j = 0; j < record[item.dataName].length; j++) {
							htmlString += "<li>" + $scope.getRelatedFieldSingleHtml(item,record[item.dataName][j]) + "</li>";
						}
						break;
				}
				htmlString += "</ul>";
				return htmlString;
			}


			//#endregion
		}


	}

})();