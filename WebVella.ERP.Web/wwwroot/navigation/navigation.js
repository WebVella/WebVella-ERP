'use strict'

/* Navigation Module Definition
 * The main purpose of the module is to present and manage
 * the ERP system navigation views as follows:
 * Area Nav - the list of all available areas for this user (presented on the top, right after the use navigation)
 * Area Local Nav - the list of all menu items, sections and entities that can be browsed within this area (presented on the left of the screen)
 * Actions Nav - the list of all global actions available to the user. (presented on the top right corner of the screen)
 * Actions Local - a sliding screen on the right that may be triggered by some of the actions
 * Admin Local Nav - a side menu typical for the administration area of the ERP
 */
angular.module('navigation', [])


/**
 * Area Local Nav
 */
.controller('AreaLocalNavViewModuleController', function ($scope, $rootScope, $state, $log) {
	//Initialize
	$scope.siteMeta = {};
	$scope.selectedArea = {};
	$scope.$on("siteMeta-update", function (event, args) {
		$scope.siteMeta = args;
		//Set the selected area
		for (var i = 0; i < $scope.siteMeta.areas.length; i++) {
			if ($scope.siteMeta.areas[i].name == $state.params.areaName) {
				$scope.selectedArea = $scope.siteMeta.areas[i];
				break;
			}
		}
	});

	$scope.SetAreaLocalActive = function (sectionName, entityName) {
		if (sectionName == $state.params.sectionName && entityName == $state.params.entityName) {
			return true;
		}
		else {
			return false;
		}
	}

	$scope.GoToEntity = function (sectionName, entityName) {

		var requestedArea = $scope.selectedArea; // This is a local navigation within the area
		var sectionNameIsCorrect = false;
		var entityNameIsCorrect = false;
		for (var i = 0; i < requestedArea.sections.length; i++) {
			//Check if the requested section exist
			if (requestedArea.sections[i].name == sectionName) {
				sectionNameIsCorrect = true;
			}
			for (var j = 0; j < requestedArea.sections[i].entities.length; j++) {
				//Check if the requested entity exist
				if (requestedArea.sections[i].entities[j].name == entityName) {
					entityNameIsCorrect = true;
				}
			}
		}

		if (sectionNameIsCorrect && entityNameIsCorrect) {
			$state.go('areaEntity', { areaName: requestedArea.name, sectionName: sectionName, entityName: entityName });
		}
		else {
			//If section OR entity requested does not exist
			alert("The requested section or entity is not correct");
		}


	}

	$scope.GoToDashboard = function () {
		if ($scope.selectedArea.hasDashboard) {
			$state.go('areaEntity', { areaName: $scope.selectedArea.name, sectionName: 'null', entityName: 'dashboard' });
		}
		else {
			alert("This area does not have dashboard enabled");
		}
	}
})
/**
 * Area Nav
 */
.controller('AreasNavViewModuleController', function ($scope, $rootScope, $state, $log, $timeout) {
	//Initialize
	$scope.siteMeta = {};
	$scope.$on("siteMeta-update", function (event, args) {
		//Event is raised from within the AppController
		$scope.siteMeta = args;
	});

	//Process
	$scope.SetActiveArea = function (areaName) {
		if (areaName == $state.params.areaName) {
			return true;
		}
		else {
			return false;
		}
	}

	$scope.GoToArea = function (areaName) {
		//Get the requested area from the sitemap
		var requestedArea = null;
		for (var i = 0; i < $scope.siteMeta.areas.length; i++) {
			if ($scope.siteMeta.areas[i].name == areaName) {
				requestedArea = $scope.siteMeta.areas[i];
			}
		}

		if (requestedArea == null) {
			alert("No area with this name is found");
			return;
		}

		//If Dashboard active - navigate to it
		if (requestedArea.hasDashboard) {
			$state.go('areaEntity', { areaName: areaName, sectionName: "null", entityName: "dashboard" });
		}
		else {
			var firstEntityName = null;
			var firstEntitySectionName = null;
			//Else Navigate to the first entity related
			for (var i = 0; i < requestedArea.sections.length; i++) {
				if (requestedArea.sections[i].entities.length > 0) {
					firstEntityName = requestedArea.sections[i].entities[0].name;
					firstEntitySectionName = requestedArea.sections[i].name;
					break;
				}
			}
			if (firstEntityName != null) {
				$state.go('areaEntity', { areaName: areaName, sectionName: firstEntitySectionName, entityName: firstEntityName });
			}
			else {
				//If no entities related raise error and cancel navigation

			}
		}


	}
})
/**
 * Actions Nav
 */
.controller('ActionsNavViewModuleController', function ($scope, $rootScope, $state) {

	$scope.SetAdminActive = function () {
		//Set active if the state name begins with admin
		if ($state.current.name.substring(0, 5) == "admin") {
			return true;
		}
		else {
			return false;
		}
	}

	$scope.GoToAdmin = function () {
		$state.go("adminEntities");
	}

})
/**
 * Actions Local Nav
 */
.controller('ActionsLocalNavViewModuleController', function ($scope, $rootScope, $state) {
})
/**
 * Admin Local nav
 */
.controller('AdminLocalNavViewModuleController', function ($scope, $rootScope, $state) {

	$scope.SetAdminLocalActive = function (stateName) {
		//Set active if the state name begins with admin
		if ($state.current.name == stateName) {
			return true;
		}
		else {
			return false;
		}
	}

	$scope.GoToState = function (stateName) {
		$state.go(stateName);
	}

});