/* base.module.js */

/**
* @desc this the base module of the Desktop plugin. Its only tasks is to check the topNavFactory and redirect to the first menu item state
*/

(function () {
	'use strict';

	angular
        .module('webvellaDevelopers', ['ui.router'])
        .config(config)
        .run(run)
        .controller('WebVellaDevelopersBaseController', controller)
		.directive('treeView', treeView)
		.controller('treeViewDirectiveController', treeViewDirectiveController);

	// Configuration ///////////////////////////////////
	config.$inject = ['$stateProvider'];

	/* @ngInject */
	function config($stateProvider) {
		$stateProvider.state('webvella-developers-base', {
			//abstract: true,
			url: '/developers', //will be added to all children states
			views: {
				"rootView": {
					controller: 'WebVellaDevelopersBaseController',
					templateUrl: '/plugins/webvella-developers/base.view.html',
					controllerAs: 'pluginData'
				}
			},
			resolve: {
				//here you can resolve any plugin wide data you need. It will be available for all children states. Parent resolved objects can be injected in the functions too
				pageTitle: function () {
					return "Webvella ERP";
				}
			},
			data: {}
		});
	};


	// Run //////////////////////////////////////
	run.$inject = ['$log', '$rootScope', 'webvellaDesktopBrowsenavFactory', 'Upload', 'webvellaRootService'];

	/* @ngInject */
	function run($log, $rootScope, webvellaDesktopBrowsenavFactory, upload, webvellaRootService) {
		$log.debug('webvellaDevelopers>base> BEGIN module.run ' + moment().format('HH:mm:ss SSSS'));
		//Allow visible only to admins
		$rootScope.$on('webvellaDesktop-browsenav-ready', function (event) {
			var currentUser = webvellaRootService.getCurrentUser();
			if (currentUser.roles.indexOf("bdc56420-caf0-4030-8a0e-d264938e0cda") > -1) {
				var item = {
					"label": "Developers",
					"stateName": "webvella-developers-base",
					"stateParams": {},
					"parentName": "",
					"nodes": [],
					"weight": 101.0,
					"color": "purple",
					"iconName": "code"
				};
				//If item is admin
				webvellaDesktopBrowsenavFactory.addItem(item);
			}
		});
		$log.debug('webvellaDevelopers>base> END module.run ' + moment().format('HH:mm:ss SSSS'));
	};


	// Controller ///////////////////////////////
	controller.$inject = ['$log', '$scope', '$timeout', '$rootScope', 'webvellaDevelopersQueryService', 'Upload', 'webvellaAdminService'];

	/* @ngInject */
	function controller($log, $scope, $timeout,$rootScope, queryService, Upload, webvellaAdminService) {
		$log.debug('webvellaDevelopers>base> BEGIN controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var pluginData = this;
		pluginData.executeSampleQuery = executeSampleQuery;
		pluginData.createSampleQueryDataStructure = createSampleQueryDataStructure;
		pluginData.executeSampleRelationRecordUpdate = executeSampleRelationRecordUpdate;
		pluginData.result = "";

		$log.debug('webvellaDevelopers>base> END controller.exec ' + moment().format('HH:mm:ss SSSS'));

		$scope.$watch('pluginData.files', function () {
			pluginData.upload(pluginData.files);
		});

		pluginData.upload = function (files) {
			if (files && files.length) {
				for (var i = 0; i < files.length; i++) {
					var file = files[i];
					Upload.upload({
						url: 'http://localhost:2202/fs/upload/',
						file: file
					}).progress(function (evt) {
						var progressPercentage = parseInt(100.0 * evt.loaded / evt.total);
					}).success(function (data, status, headers, config) {
						$timeout(function () {
							queryService.moveFile({ 'source': data.object.url, 'target': "/test/test.pdf", overwrite: true },
                                function (response) {
                                	$log.debug('webvellaDevelopers>base> END controller.moveFile> SUCCESS');
                                	pluginData.result = response;

                                	queryService.deleteFile("/fs/test/test.pdf",
                                        function (response) {
                                        	$log.debug('webvellaDevelopers>base> END controller.deleteFile> SUCCESS');
                                        	$log.debug(response);
                                        	pluginData.result = response;
                                        },
                                        function (response) {
                                        	$log.debug('webvellaDevelopers>base> END controller.deleteFile> ERROR');
                                        	$log.debug(response);
                                        	pluginData.result = response;
                                        });


                                },
				                function (response) {
				                	$log.debug('webvellaDevelopers>base> END controller.moveFile> ERROR');
				                	$log.debug(response);
				                	pluginData.result = response;
				                });

						});
					});
				}
			}
		};

		function executeSampleRelationRecordUpdate() {
			$log.debug('webvellaDevelopers>base> BEGIN controller.executeSampleRelationRecordUpdate ' + moment().format('HH:mm:ss SSSS'));
			queryService.executeSampleRelationRecordUpdate({ relationName: "account_id_to_test_user_id", originFieldRecordId: '8e253615-d91c-4c60-91ca-c13d3c7c6053', targetFieldRecordIds: ['1af61222-986c-403c-a1e6-738c986312f0'], operation: "create" },
				function (response) {
					$log.debug('webvellaDevelopers>base> END controller.executeSampleRelationRecordUpdate> SUCCESS ' + moment().format('HH:mm:ss SSSS'));
					$log.debug(response);
					pluginData.result = response;
				},
				function (response) {
					$log.debug('webvellaDevelopers>base> END controller.executeSampleRelationRecordUpdate> ERROR ' + moment().format('HH:mm:ss SSSS'));
					$log.debug(response);
					pluginData.result = response;
				}
			);

		}

		function executeSampleQuery() {
			$log.debug('webvellaDevelopers>base> BEGIN controller.executeSampleQuery ' + moment().format('HH:mm:ss SSSS'));
			queryService.executeSampleQuery({},
				function (response) {
					$log.debug('webvellaDevelopers>base> END controller.executeSampleQuery> SUCCESS ' + moment().format('HH:mm:ss SSSS'));
					$log.debug(response);
					pluginData.result = response;
				},
				function (response) {
					$log.debug('webvellaDevelopers>base> END controller.executeSampleQuery> ERROR ' + moment().format('HH:mm:ss SSSS'));
					$log.debug(response);
					pluginData.result = response;
				}
			);

		}

		function createSampleQueryDataStructure() {
			$log.debug('webvellaDevelopers>base> BEGIN controller.createSampleQueryDataStructure ' + moment().format('HH:mm:ss SSSS'));
			queryService.createSampleQueryDataStructure({},
				function (response) {
					$log.debug('webvellaDevelopers>base> END controller.createSampleQueryDataStructure> SUCCESS ' + moment().format('HH:mm:ss SSSS'));
					$log.debug(response);
					pluginData.result = response;
				},
				function (response) {
					$log.debug('webvellaDevelopers>base> END controller.createSampleQueryDataStructure> ERROR ' + moment().format('HH:mm:ss SSSS'));
					$log.debug(response);
					pluginData.result = response;
				}
			);
		}


		//#region << Tree view init >>

		pluginData.treeMeta = {
			"id": "be607564-8424-4df2-b6ff-16a7a9529c30",
			"name": "categories",
			"label": "Categories",
			"default": false,
			"system": false,
			"cssClass": "some-css",
			"iconName": "sitemap",
			"relationId": "460e699c-6624-4238-bb17-12243cf5d56b", // Only relations in which both origin and target are the current entity
			"depthLimit": 5,
			"nodeParentIdFieldId": "16672229-1694-468e-a363-c80effffe5d1", //Inherited from the relation Target field
			"nodeIdFieldId": "5df6bba4-061b-41ce-bf39-8f6b50fd023d", //Inherited from the relation Origin field
			"nodeNameFieldId": "c80e1e20-71e2-4de1-8b3b-5a63c6740cea", //Only certain types should be allowed here - used for URL generation
			"nodeLabelFieldId": "664f2b4a-dd96-4e27-aabd-898e255d9c8e", //Only certain types should be allowed here - human readable node label
			"rootNodes": [
				{
					"id": "5548bbc7-eda3-45e7-b0ee-253f4eaf2785",
					"recordId": "5548bbc7-eda3-45e7-b0ee-253f4eaf2785",
					"name": "clothes",
					"label": "Clothes",
					"parentId": "b6add018-f9eb-4b60-a724-7d1e2597449c"
				}
			],
			"nodeObjectProperties": ["16672229-1694-468e-a363-c80effffe5d1", "5df6bba4-061b-41ce-bf39-8f6b50fd023d", "c80e1e20-71e2-4de1-8b3b-5a63c6740cea", "664f2b4a-dd96-4e27-aabd-898e255d9c8e"]
		}

		pluginData.itemMeta = { "id": null, "name": "test", "label": "test", "placeholderText": "", "description": "", "helpText": "", "required": false, "unique": false, "searchable": false, "auditable": false, "system": false, "fieldType": 20, "enableSecurity": false, "permissions": { "canRead": [], "canUpdate": [] }, "selectedTreeId": "be607564-8424-4df2-b6ff-16a7a9529c31", "selectionType": "single-branch-select", "selectionTarget": "leaves" };

		pluginData.treeBranches = [
		{
			"id": "0ccae7b9-890b-41e5-a144-50e98046f68b",
			"recordId": "da2207d2-3be2-4f8c-b2ae-cec727600e19",
			"name": "women",
			"label": "Women",
			"parentId": null,
			"branch":[],
			"nodes": [
					{
						"id": "6720e75c-3ecd-4144-899a-c1430ae13c21",
						"recordId": "ae1552e0-43d6-4999-a8e0-a2e1605aef66",
						"name": "dresses",
						"label": "Dresses",
						"parentId": null,
						"branch": ["0ccae7b9-890b-41e5-a144-50e98046f68b"],
						"nodes": []
					},
					{
						"id": "73728e22-4402-4c55-a726-8d7dac4d1459",
						"recordId": "135d4f88-b480-492a-b893-f1038d29dea7",
						"name": "shoes",
						"label": "Shoes",
						"parentId": null,
						"branch": ["0ccae7b9-890b-41e5-a144-50e98046f68b"],
						"nodes": [
							{
								"id": "89e38911-b75d-418a-89cb-382ac5819e68",
								"recordId": "f24841d8-98e6-4b47-b0d5-38887eb9b5e7",
								"name": "sport",
								"label": "Sport",
								"parentId": null,
								"branch": ["0ccae7b9-890b-41e5-a144-50e98046f68b", "73728e22-4402-4c55-a726-8d7dac4d1459"],
								"nodes": []
							},
							{
								"id": "eb3e87ce-6136-4f6d-9404-e91c38149edb",
								"recordId": "a691ccfe-cd4e-4cc2-a160-e5e095bebb50",
								"name": "official",
								"label": "Official",
								"parentId": null,
								"branch": ["0ccae7b9-890b-41e5-a144-50e98046f68b", "73728e22-4402-4c55-a726-8d7dac4d1459"],
								"nodes": []
							}

						]
					}
			]
		},
		{
			"id": "96efda97-11e8-49fa-b075-6e32ebef04f9",
			"recordId": "f934478a-a955-455a-b3e1-13d40379ace6",
			"name": "men",
			"label": "Men",
			"parentId": null,
			"branch":[],
			"nodes": [
							{
								"id": "3079f7e4-5eb7-4ef9-87fa-802abeb3b6a4",
								"recordId": "fa516ed5-7eb4-45d2-b6c3-d141e321c9f3",
								"name": "pants",
								"label": "Pants",
								"parentId": null,
								"branch": ["96efda97-11e8-49fa-b075-6e32ebef04f9"],
								"nodes": [],
							},
							{
								"id": "23cf9649-0f7e-4c7e-b5f3-f267dea0a789",
								"recordId": "968e7455-01fe-4b64-8f21-180b92351395",
								"name": "shirts",
								"label": "Shirts",
								"parentId": null,
								"branch": ["96efda97-11e8-49fa-b075-6e32ebef04f9"],
								"nodes": []
							}
			]
		}
		];

		//#endregion



		//#region << Node selection >>

		pluginData.selectedTreeRecords = [];

		pluginData.selectableNodeIds = [];

		var selectedNodesByBranch = {};

		function iterateCanBeSelected(current, depth, rootNode) {
			var children = current.nodes;
			var shouldBeSelectable = true;

			//Case: selection type
			switch (pluginData.itemMeta.selectionType) {
				case "single-select":
					if (pluginData.selectedTreeRecords && pluginData.selectedTreeRecords.length > 0 && pluginData.selectedTreeRecords[0] != current.recordId) {
						shouldBeSelectable = false;
					}
					break;
				case "multi-select":
					break;
				case "single-branch-select":
					if (selectedNodesByBranch[rootNode.id] && selectedNodesByBranch[rootNode.id].length > 0 && selectedNodesByBranch[rootNode.id][0] != current.id) {
						shouldBeSelectable = false;
					}
					break;
			}

			switch (pluginData.itemMeta.selectionTarget) {
				case "all":
					break;
				case "leaves":
					//Check if the node is not selected
					var leaveCheckIndex = pluginData.selectedTreeRecords.indexOf(current.recordId);
					if (children.length > 0 && leaveCheckIndex == -1) {
						shouldBeSelectable = false;
					}
					break;
			}

			if (shouldBeSelectable) {
				pluginData.selectableNodeIds.push(current.id);
			}
			
			for (var i = 0, len = children.length; i < len; i++) {
				iterateCanBeSelected(children[i], depth + 1, rootNode);
			}
		}

		pluginData.regenerateCanBeSelected = function () {
			pluginData.selectableNodeIds = [];
			for (var i = 0; i < pluginData.treeBranches.length; i++) {
				iterateCanBeSelected(pluginData.treeBranches[i], 0, pluginData.treeBranches[i]);
			}
		}

		pluginData.toggleNodeSelection = function (node) {
			var nodeIndex = pluginData.selectedTreeRecords.indexOf(node.recordId);
			//Node should be unselected
			if (nodeIndex > -1) {
				pluginData.selectedTreeRecords.splice(nodeIndex, 1);
				var nodeRootBranchId = node.branch[0];

				if (selectedNodesByBranch[nodeRootBranchId]) {
					var selectedIndex = selectedNodesByBranch[nodeRootBranchId].indexOf(node.id)
					selectedNodesByBranch[node.branch[0]].splice(selectedIndex,1);
				}
				pluginData.regenerateCanBeSelected();
			}
			//Node should be selected
			else {
				pluginData.selectedTreeRecords.push(node.recordId);
				//Add to the branch selected object
				var nodeRootBranchId = node.branch[0];
				if (selectedNodesByBranch[nodeRootBranchId]) {
					selectedNodesByBranch[node.branch[0]].push(node.id);
				}
				else {
					selectedNodesByBranch[node.branch[0]] = [];
					selectedNodesByBranch[node.branch[0]].push(node.id);
				}
				pluginData.regenerateCanBeSelected();
			}
		}

		pluginData.regenerateCanBeSelected();

		pluginData.clearSelection = function () {
			pluginData.selectedTreeRecords = [];
			pluginData.regenerateCanBeSelected();
		}

		//#endregion

		//#region << Node collapse >>
		pluginData.collapsedTreeNodes = [];
		pluginData.toggleNodeCollapse = function (node) {
			var nodeIndex = pluginData.collapsedTreeNodes.indexOf(node.id);
			if (nodeIndex > -1) {
				pluginData.collapsedTreeNodes.splice(nodeIndex, 1);
			}
			else {
				pluginData.collapsedTreeNodes.push(node.id);
			}
		}

		pluginData.nodesToBeCollapsed = [];

		function iterateCollapse(current, depth) {
			var children = current.nodes;
			if (children.length > 0) {
				pluginData.collapsedTreeNodes.push(current.id);
			}
			for (var i = 0, len = children.length; i < len; i++) {
				iterateCollapse(children[i], depth + 1);
			}
		}

		pluginData.collapseAll = function () {
			pluginData.collapsedTreeNodes = [];
			for (var i = 0; i < pluginData.treeBranches.length; i++) {
				iterateCollapse(pluginData.treeBranches[i], 0);
			}
		}
		pluginData.expandAll = function () {
			pluginData.collapsedTreeNodes = [];
		}

		//#endregion

		//#region << Register toggle node events >>

		//This event is later used by the recursive directive that follows
		////READY hook listeners
		var toggleTreeNodeSelectedDestructor = $rootScope.$on("webvellaAdmin-toggleTreeNode-selected", function (event, data) {
			pluginData.toggleNodeSelection(data);
		})

		var toggleTreeNodeCollapsedDestructor = $rootScope.$on("webvellaAdmin-toggleTreeNode-collapsed", function (event, data) {
			pluginData.toggleNodeCollapse(data);
		})

		////DESCTRUCTOR - hook listeners remove on scope destroy. This avoids duplication, as rootScope is never destroyed and new controller load will duplicate the listener
		$scope.$on("$destroy", function () {
			toggleTreeNodeSelectedDestructor();
			toggleTreeNodeCollapsedDestructor();
		});



		//#endregion


	}


	//#region << Query Directive >>
	treeView.$inject = ['$compile', '$templateRequest', 'RecursionHelper'];
	/* @ngInject */
	function treeView($compile, $templateRequest, RecursionHelper) {
		var directive = {
			controller: treeViewDirectiveController,
			templateUrl: "treeView.html",
			restrict: 'E',
			scope: {
				treeMeta: '=',
				treeBranches: '=', //The tree object root branches
				selectedTreeRecords: '=', //The ids of the records of the the tree that is selected /not the node ids but the record ids/,
				selectableNodeIds:'=', //Array of node Ids that can be selected, due to field tree selection options
				collapsedTreeNodes:'=',
				fieldMeta: "="
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

	treeViewDirectiveController.$inject = ['$filter', '$log', '$state', '$scope','$rootScope', '$q', '$uibModal', 'ngToast', 'webvellaAreasService', 'webvellaAdminService'];
	/* @ngInject */
	function treeViewDirectiveController($filter, $log, $state, $scope,$rootScope, $q, $uibModal, ngToast, webvellaAreasService, webvellaAdminService) {

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
		//$scope.forceSingleSelectLock = false;
		//$scope.forceSingleBranchSelectLock = []; //array of the root node id's of the locked branches

		$scope.toggleNodeSelected = function (node) {
			if ($scope.isNodeSelectable(node.id) || $scope.isRecordSelected(node.recordId)) {
				$rootScope.$emit("webvellaAdmin-toggleTreeNode-selected", node);
			}
		}

		$scope.toggleNodeCollapse = function (node) {
			$rootScope.$emit("webvellaAdmin-toggleTreeNode-collapsed", node);
		}


	}
	//#endregion

})();
