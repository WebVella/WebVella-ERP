/* view-tree-select.directive.js */

/**
* @desc helper for managing the tree select included in a view
* @example <view-tree-select></view-tree-select>
* @params:
- treeMeta -> tree as defined in the entity. Comes from the entity meta.
- treeData -> the records of the entity requested with the selected tree. Comes from the recordsByTree request
- selectedTreeRecords -> records selected through the relation for the current record
- fieldMeta -> the current's view item meta
*/

(function () {
	'use strict';

	angular
        .module('webvellaAreas')
        .directive('viewTreeSelect', viewTreeSelect)
		.controller('viewTreeSelectDirectiveController', viewTreeSelectDirectiveController);

	viewTreeSelect.$inject = ['$compile', '$templateRequest', 'RecursionHelper'];

	
	function viewTreeSelect($compile, $templateRequest, RecursionHelper) {
		var directive = {
			controller: viewTreeSelectDirectiveController,
			templateUrl: "/plugins/webvella-areas/providers/view-tree-select.template.html",
			restrict: 'E',
			scope: {
				treeMeta: '=',
				treeData: '=', //The tree object root branches
				selectedTreeRecords: '=', //The ids of the records of the the tree that is selected /not the node ids but the record ids/,
				fieldMeta: "=",
				collapsedTreeNodes: "=",
				selectableNodeIds: "="
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

	viewTreeSelectDirectiveController.$inject = ['$filter', '$log', '$state', '$scope', '$rootScope', '$q', '$uibModal', 'ngToast', 'webvellaCoreService','$timeout'];
	
	function viewTreeSelectDirectiveController($filter, $log, $state, $scope, $rootScope, $q, $uibModal, ngToast, webvellaCoreService,$timeout) {

		$scope.isRecordSelected = function (nodeRecordId) {
			return $scope.selectedTreeRecords.indexOf(nodeRecordId) > -1
		}

		$scope.isNodeSelectable = function (nodeId) {
			return $scope.selectableNodeIds.indexOf(nodeId) > -1
		}

		$scope.isNodeCollapsed = function (nodeId) {
			return $scope.collapsedTreeNodes.indexOf(nodeId) > -1
		}

		$scope.attachHoverEffectClass = {};

		$scope.toggleNodeSelected = function (node) {
			if ($scope.isNodeSelectable(node.id) || $scope.isRecordSelected(node.recordId)) {
				$timeout(function(){
					$rootScope.$emit("webvellaAdmin-toggleTreeNode-selected", node);
				},0);
			}
		}

		$scope.toggleNodeCollapse = function (node) {
			$timeout(function(){
				$rootScope.$emit("webvellaAdmin-toggleTreeNode-collapsed", node);
			},0);
		}


	}


})();