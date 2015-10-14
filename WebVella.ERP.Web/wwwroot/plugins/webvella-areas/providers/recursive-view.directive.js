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
				relation: '&',
				parentId: '&',
				canAddExisting: '&',
				canCreate: '&',
				canRemove: '&',
				canEdit: '&'
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
		$scope.relation = $scope.relation();
		$scope.viewMeta = $scope.viewMeta();
		$scope.viewData = $scope.viewData();
		$scope.viewEntity = {};
		$scope.viewEntity.id = $scope.viewMeta.entityId;
		$scope.viewEntity.name = $scope.viewMeta.entityName;
		$scope.parentId = $scope.parentId();
		$scope.canAddExisting = $scope.canAddExisting();
		$scope.canCreate = $scope.canCreate();
		$scope.canRemove = $scope.canRemove();
		$scope.canEdit = $scope.canEdit();

		$scope.selectedRegion = null;
		for (var i = 0; i < $scope.viewMeta.meta.regions.length; i++) {
			if ($scope.viewMeta.meta.regions[i].name === "content") {
				$scope.selectedRegion = $scope.viewMeta.meta.regions[i];
			}
		}
		if ($scope.selectedRegion == null) {
			$log.error("the subview: " + $scope.viewMeta.name + " does not have a content region");
		}

		//Calculate listEntity stance in the relation
		$scope.dataKind = "target";
		if ($scope.relation && $scope.viewEntity.id === $scope.relation.originEntityId) {
			$scope.dataKind = "origin";
			if ($scope.viewEntity.id === $scope.relation.targetEntityId) {
				$scope.dataKind = "origin-target";
			}
		}

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

	}




})();