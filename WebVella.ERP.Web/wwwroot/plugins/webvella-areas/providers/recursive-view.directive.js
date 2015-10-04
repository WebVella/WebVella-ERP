/* recursive-view.directive.js */

/**
* @desc recursive record view
* @example <recursive-view item-meta="viewMeta" records-data="recordInstance" relations-list="contentData.relationsList"></recursive-view>
 * @item-meta => object, the view meta data
 * @records-data => Array, data of the records that need presentation
 * @relations-list => Array all relations list Meta
*/

(function () {
	'use strict';

	angular
        .module('webvellaAreas')
        .directive('recursiveView', directive);

	directive.$inject = ['$compile', '$templateRequest', 'RecursionHelper', 'webvellaAdminService'];

	/* @ngInject */
	function directive($compile, $templateRequest, RecursionHelper, webvellaAdminService) {
		//Text Binding (Prefix: @)
		//One-way Binding (Prefix: &)
		//Two-way Binding (Prefix: =)
		var directive = {
			controller: DirectiveController,
			templateUrl: '/plugins/webvella-areas/providers/recursive-view.template.html',
			restrict: 'E',
			scope: {
				viewData: '&',
				viewMeta: '&',
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


	DirectiveController.$inject = ['$filter', '$log', '$scope', 'webvellaAreasService'];
	/* @ngInject */
	function DirectiveController($filter, $log, $scope, webvellaAreasService) {
		//#region << Init >>
		//var directiveData = this;
		$scope.itemEntityId = $scope.viewMeta().entityId;
		$scope.viewMeta = $scope.viewMeta().meta;
		$scope.relationsList = $scope.relationsList();
		$scope.selectedRegion = null;
		for (var i = 0; i < $scope.viewMeta.regions.length; i++) {
			if ($scope.viewMeta.regions[i].name === "content") {
				$scope.selectedRegion = $scope.viewMeta.regions[i];
			}
		}
		if ($scope.selectedRegion == null) {
			$log.error("the subview: " + $scope.viewMeta.name + " does not have a content region");
		}
		$scope.viewData = $scope.viewData();

		//SubViews sections collapsed state - depends on sectionId and recordId as there could be multiple records presented with the same view (section id)
		$scope.sectionCollapsedData = [];
		//Init all sections collapsed state with section[id] and record[id] key

		//Find the records for this view
		var recordIdArray = [];
		for (var l = 0; l < $scope.viewData.length; l++) {
			recordIdArray.push($scope.viewData.id);
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

		$scope.getRelatedFieldSingleHtml = function (item, record) {
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
					htmlString = record[item.dataName];
					break;
			}

			return htmlString;
		}

		$scope.getRelatedFieldMultiHtml = function (item, record) {
			var htmlString = "<ul>";
			for (var j = 0; j < record[item.dataName].length; j++) {
				var tempRecord = {};
				tempRecord[item.dataName] = record[item.dataName][j];
				switch (item.meta.fieldType) {
					case 1:
						htmlString += "<li>" + $scope.getAutoIncrementString(item, tempRecord) + "</li>";
						break;
					case 2:
						htmlString += "<li>" + $scope.getCheckboxString(item, tempRecord) + "</li>";
						break;
					case 3:
						htmlString += "<li>" + $scope.getCurrencyString(item, tempRecord) + "</li>";
						break;
					case 4:
						htmlString += "<li>" + $scope.getDateString(item, tempRecord) + "</li>";
						break;
					case 5:
						htmlString += "<li>" + $scope.getDateString(item, tempRecord) + "</li>";
						break;
					case 7:
						htmlString += "<li>" + $scope.getFileString(item, tempRecord) + "</li>";
						break;
					case 9:
						htmlString += "<li>" + $scope.getImageString(item, tempRecord) + "</li>";
						break;
					case 11:
						htmlString += "<li>" + $scope.getCheckboxlistString(item, tempRecord) + "</li>";
						break;
					case 14:
						htmlString += "<li>" + $scope.getPercentString(item, tempRecord) + "</li>";
						break;
					case 17:
						htmlString += "<li>" + $scope.getDropdownString(item, tempRecord) + "</li>";
						break;
					case 19:
						htmlString += "<li>" + $scope.getUrlString(item, tempRecord) + "</li>";
						break;
					default:
						htmlString += "<li>" + $scope.getRelatedFieldSingleHtml(item, tempRecord) + "</li>";
						break;
				}
			}
			htmlString += "</ul>";
			return htmlString;
		}


		//#endregion
	}




})();