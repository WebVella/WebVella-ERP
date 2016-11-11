/* entity-fields.module.js */

/**
* @desc this module manages the entity record fields in the admin screen
*/

(function () {
    //'use strict';

    angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminEntityFieldsController', controller)
        .controller('CreateFieldModalController', CreateFieldModalController)
        .controller('ManageFieldModalController', ManageFieldModalController)
        .controller('DeleteFieldModalController', DeleteFieldModalController);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];

    function config($stateProvider) {
        $stateProvider.state('webvella-admin-entity-fields', {
            parent: 'webvella-admin-base',
            url: '/entities/:entityName/fields', //  /desktop/areas after the parent state is prepended
            views: {
                "topnavView": {
                    controller: 'WebVellaAdminTopnavController',
                    templateUrl: '/plugins/webvella-admin/topnav.view.html',
                    controllerAs: 'topnavData'
                },
                "sidebarView": {
                    controller: 'WebVellaAdminSidebarController',
                    templateUrl: '/plugins/webvella-admin/sidebar-avatar-only.view.html',
                    controllerAs: 'sidebarData'
                },
                "contentView": {
                    controller: 'WebVellaAdminEntityFieldsController',
                    templateUrl: '/plugins/webvella-admin/entity-fields.view.html',
                    controllerAs: 'ngCtrl'
                }
            },
            resolve: {
                resolvedEntityList: resolveEntityList,
                resolvedRolesList: resolveRolesList,
                resolvedRelationsList: resolveRelationsList,
                translatedFieldTypes: translateFieldTypes
            },
            data: {

            }
        });
    };


    //#region << Resolve Function /////////////////////////

    // Resolve Roles list /////////////////////////
    resolveRolesList.$inject = ['$q', '$log', 'webvellaCoreService'];

    function resolveRolesList($q, $log, webvellaCoreService) {
        // Initialize
        var defer = $q.defer();

        // Process
        function successCallback(response) {
            defer.resolve(response.object);
        }

        function errorCallback(response) {
            defer.reject(response.message);
        }

        webvellaCoreService.getRecordsWithoutList(null, null, null, "role", successCallback, errorCallback);

        return defer.promise;
    }

    resolveRelationsList.$inject = ['$q', '$log', 'webvellaCoreService', '$state', '$timeout'];

    function resolveRelationsList($q, $log, webvellaCoreService) {

        // Initialize
        var defer = $q.defer();

        // Process
        function successCallback(response) {
            defer.resolve(response.object);
        }

        function errorCallback(response) {
            defer.reject(response.message);
        }

        webvellaCoreService.getRelationsList(successCallback, errorCallback);

        return defer.promise;
    }

    // translateFieldTypes /////////////////////////
    translateFieldTypes.$inject = ['$q', '$log', 'webvellaCoreService'];

    function translateFieldTypes($q, $log, webvellaCoreService) {
        // Initialize
        var defer = $q.defer();

        function successCallback(response) {
            defer.resolve(response);
        }
        webvellaCoreService.getFieldTypes(successCallback);
        return defer.promise;
    }

    resolveEntityList.$inject = ['$q', '$log', 'webvellaCoreService', '$state', '$stateParams'];
    function resolveEntityList($q, $log, webvellaCoreService, $state, $stateParams) {
        var defer = $q.defer();
        function successCallback(response) {
            defer.resolve(response.object);
        }
        function errorCallback(response) {
            defer.reject(response.message);
        }
        webvellaCoreService.getEntityMetaList(successCallback, errorCallback);
        return defer.promise;
    }


    //#endregion

    // Controller ///////////////////////////////
    controller.$inject = ['$scope', '$log', '$rootScope', '$state', 'pageTitle', '$uibModal', 'resolvedRolesList', 'resolvedEntityList',
						'resolvedRelationsList', '$timeout', '$q', 'webvellaCoreService', '$translate', 'translatedFieldTypes', '$stateParams'];

    function controller($scope, $log, $rootScope, $state, pageTitle, $uibModal, resolvedRolesList, resolvedEntityList,
						resolvedRelationsList, $timeout, $q, webvellaCoreService, $translate, translatedFieldTypes, $stateParams) {

        var ngCtrl = this;

        //#region << Init >>
        ngCtrl.search = {};
        ngCtrl.entity = webvellaCoreService.getEntityMetaFromEntityList($stateParams.entityName, resolvedEntityList);
        ngCtrl.rolesList = resolvedRolesList;
        ngCtrl.entity.fields = ngCtrl.entity.fields.sort(function (a, b) {
            if (a.name < b.name) return -1;
            if (a.name > b.name) return 1;
            return 0;
        });

        //#region << Update page title>> 
        $translate(['ENTITY_FIELDS', 'ENTITIES']).then(function (translations) {
            ngCtrl.pageTitle = translations.ENTITY_FIELDS + " | " + pageTitle;
            $rootScope.$emit("application-pageTitle-update", ngCtrl.pageTitle);
            $rootScope.adminSectionName = translations.ENTITIES;
        });
        $rootScope.adminSubSectionName = ngCtrl.entity.label;
        //#endregion

        ngCtrl.fieldTypes = [];
        ngCtrl.fieldTypes = translatedFieldTypes;

        //Currency meta - used in Create and Manage fields
        ngCtrl.currencyMetas = webvellaCoreService.currencyMetas();

        ngCtrl.relationsList = resolvedRelationsList;
        //#endregion

        //Create new field modal
        ngCtrl.createFieldModal = function () {
            // ReSharper disable once UnusedLocals
            var modalInstance = $uibModal.open({
                animation: false,
                templateUrl: 'createFieldModal.html',
                controller: 'CreateFieldModalController',
                controllerAs: "popupCtrl",
                size: "lg",
                resolve: {
                    ngCtrl: function () { return ngCtrl; }
                }
            });
        }

        ngCtrl.findFieldType = function (fieldTypeId) {
            if (ngCtrl.fieldTypes) {
                for (var typeIndex = 0; typeIndex < ngCtrl.fieldTypes.length; typeIndex++) {
                    if (ngCtrl.fieldTypes[typeIndex].id === fieldTypeId) {
                        return ngCtrl.fieldTypes[typeIndex];
                    }
                }
            }
            return null;
        };

        //Manage field modal
        ngCtrl.manageFieldModal = function (fieldId) {
            var managedField = null;
            for (var i = 0; i < ngCtrl.entity.fields.length; i++) {
                if (ngCtrl.entity.fields[i].id === fieldId) {
                    managedField = ngCtrl.entity.fields[i];
                }
            }
            // ReSharper disable once UnusedLocals
            var modalInstance = $uibModal.open({
                animation: false,
                templateUrl: 'manageFieldModal.html',
                controller: 'ManageFieldModalController',
                controllerAs: "popupCtrl",
                size: "lg",
                resolve: {
                    ngCtrl: function () { return ngCtrl; },
                    resolvedField: function () {
                        return managedField;
                    },
                    resolvedRelatedEntity: resolveRelatedEntity()
                }
            });

            function resolveRelatedEntity() {
                // Initialize
                var defer = $q.defer();

                // Process
                function successCallback(response) {
                    if (response.object === null) {
                        $translate(['ERROR_IN_RESPONSE']).then(function (translations) {
                            alert(translations.ERROR_IN_RESPONSE);
                        });
                    }
                    else {
                        defer.resolve(response.object);
                    }
                }

                function errorCallback(response) {
                    if (response.object === null) {
                        $translate(['ERROR_IN_RESPONSE']).then(function (translations) {
                            alert(translations.ERROR_IN_RESPONSE);
                        });
                    }
                    else {
                        defer.reject(response.message);
                    }
                }

                if (managedField.fieldType === 21) {
                    webvellaCoreService.getEntityMetaById(managedField.relatedEntityId, successCallback, errorCallback);
                }
                else {
                    defer.resolve(null);
                }
                // Return
                return defer.promise;
            }
        }

    }



    //// Create Field Controllers
    CreateFieldModalController.$inject = ['ngCtrl', '$scope', '$uibModalInstance', '$log', '$sce', 'ngToast', '$timeout', '$state', 'webvellaCoreService', '$location', '$translate'];

    function CreateFieldModalController(ngCtrl, $scope, $uibModalInstance, $log, $sce, ngToast, $timeout, $state, webvellaCoreService, $location, $translate) {

        var popupCtrl = this;
        popupCtrl.validation = {};
        popupCtrl.ngCtrl = ngCtrl;
        popupCtrl.createFieldStep2Loading = false;
        popupCtrl.field = webvellaCoreService.initField();

        popupCtrl.isEmpty = function (object) {
            return isEmpty(object)
        }

        popupCtrl.fieldTypes = ngCtrl.fieldTypes;
        // Inject a searchable field
        for (var i = 0; i < popupCtrl.fieldTypes.length; i++) {
            popupCtrl.fieldTypes[i].searchBox = popupCtrl.fieldTypes[i].label + " " + popupCtrl.fieldTypes[i].description;
        }

        //Generate roles and checkboxes
        popupCtrl.fieldPermissions = [];
        for (var i = 0; i < popupCtrl.ngCtrl.rolesList.data.length; i++) {

            //Now create the new entity.roles array
            var role = {};
            role.id = popupCtrl.ngCtrl.rolesList.data[i].id;
            role.label = popupCtrl.ngCtrl.rolesList.data[i].name;
            role.canRead = false;
            role.canUpdate = false;
            popupCtrl.fieldPermissions.push(role);
        }

        popupCtrl.togglePermissionRole = function (permission, roleId) {
            //Get the current state

            var permissionArrayRoleIndex = -1;
            if (popupCtrl.field.permissions !== null) {
                for (var k = 0; k < popupCtrl.field.permissions[permission].length; k++) {
                    if (popupCtrl.field.permissions[permission][k] === roleId) {
                        permissionArrayRoleIndex = k;
                    }
                }
            }

            if (permissionArrayRoleIndex !== -1) {
                popupCtrl.field.permissions[permission].splice(permissionArrayRoleIndex, 1);
            }
            else {
                if (popupCtrl.field.permissions === null) {
                    popupCtrl.field.permissions = {};
                    popupCtrl.field.permissions.canRead = [];
                    popupCtrl.field.permissions.canUpdate = [];
                }
                popupCtrl.field.permissions[permission].push(roleId);
            }

        }

        //#region << Selection Tree field >>

        popupCtrl.relationsList = fastCopy(popupCtrl.ngCtrl.relationsList);

        //#region << Selection types >>
        popupCtrl.selectionTypes = [];
        $translate(['RECORD_TREE_SINGLE_SELECT_OPTION_VALUE', 'RECORD_TREE_MULTI_SELECT_OPTION_VALUE', 'RECORD_TREE_SINGLE_BRANCH_SELECT_OPTION_VALUE']).then(function (translations) {
            popupCtrl.singleSelectType = {
                key: "single-select",
                value: translations.RECORD_TREE_SINGLE_SELECT_OPTION_VALUE
            };
            popupCtrl.multiSelectType = {
                key: "multi-select",
                value: translations.RECORD_TREE_MULTI_SELECT_OPTION_VALUE
            };
            popupCtrl.singleBranchSelectType = {
                key: "single-branch-select",
                value: translations.RECORD_TREE_SINGLE_BRANCH_SELECT_OPTION_VALUE
            };
        });
        //#endregion

        //#region << Selection targets >>
        popupCtrl.selectionTargets = [];
        $translate(['RECORD_TREE_TARGET_ALL_NODES_OPTION_VALUE', 'RECORD_TREE_TARGET_ONLY_LEAVES_OPTION_VALUE']).then(function (translations) {
            popupCtrl.allNodesSelectTarget = {
                key: "all",
                value: translations.RECORD_TREE_TARGET_ALL_NODES_OPTION_VALUE
            };
            popupCtrl.multiSelectTarget = {
                key: "leaves",
                value: translations.RECORD_TREE_TARGET_ONLY_LEAVES_OPTION_VALUE
            };
            popupCtrl.selectionTargets.push(popupCtrl.allNodesSelectTarget);
            popupCtrl.selectionTargets.push(popupCtrl.multiSelectTarget);
        });
        //#endregion

        //#region << Tree selection >>
        // ReSharper disable once UnusedParameter
        popupCtrl.getRelationHtml = function (treeId) {


            return result;
        }

        $scope.$watch("popupCtrl.field.relatedEntityId", function (newValue) {
            if (newValue) {
                $translate(['UNKNOWN']).then(function (translations) {
                    popupCtrl.SelectedInTreeRelationHtml = translations.UNKNOWN;
                });
                var selectedEntity = findInArray(popupCtrl.eligibleEntitiesForTree, "id", newValue);
                var selectedTree = selectedEntity.recordTrees[0];
                var selectedRelation = selectedEntity.relations[0];

                //Reinit dropdowns
                popupCtrl.selectedEntityRelations = selectedEntity.relations;
                popupCtrl.field.relationId = selectedRelation.id;

                popupCtrl.selectedEntityTrees = selectedEntity.recordTrees;
                popupCtrl.field.selectedTreeId = selectedTree.id;
            }
        });

        $scope.$watch("popupCtrl.field.relationId", function (newValue) {
            if (newValue) {
                var selectedEntity = findInArray(popupCtrl.eligibleEntitiesForTree, "id", popupCtrl.field.relatedEntityId);
                var selectedRelation = findInArray(selectedEntity.relations, "id", newValue);

                popupCtrl.selectionTypes = [];
                popupCtrl.selectionTypes.push(popupCtrl.singleSelectType);
                if (selectedRelation) {
                    if (selectedRelation.relationType === 2) {
                        //Fix type selection if it was a multiselect option
                        popupCtrl.field.selectionType = "single-select";
                        popupCtrl.field.selectionTarget = "all";
                    }
                    else if (selectedRelation.relationType === 3) {
                        popupCtrl.selectionTypes.push(popupCtrl.multiSelectType);
                        popupCtrl.selectionTypes.push(popupCtrl.singleBranchSelectType);
                        popupCtrl.field.selectionType = "single-select";
                        popupCtrl.field.selectionTarget = "all";
                    }
                }
            }
        });

        //#endregion

        //#endregion

        //Wizard
        popupCtrl.wizard = {};
        popupCtrl.wizard.steps = [];
        //Initialize steps
        var step = fastCopy({ "active": false }, { "completed": false });
        popupCtrl.wizard.steps.push(step); // Dummy step
        step = fastCopy({ "active": false }, { "completed": false });
        popupCtrl.wizard.steps.push(step); // Step 1
        step = fastCopy({ "active": false }, { "completed": false });
        popupCtrl.wizard.steps.push(step); // Step 2
        // Set steps
        popupCtrl.wizard.steps[1].active = true;
        popupCtrl.wizard.selectedType = null;

        popupCtrl.selectType = function (typeId) {
            //find selected type
            for (var typeIndex = 0; typeIndex < popupCtrl.fieldTypes.length; typeIndex++) {
                if (popupCtrl.fieldTypes[typeIndex].id === typeId) {
                    popupCtrl.wizard.selectedType = popupCtrl.fieldTypes[typeIndex];
                    break;
                }
            }

            if (popupCtrl.wizard.selectedType === null) {
                console.log('The selected field type [' + typeId + '] is missing in collection of field types');
            }

            popupCtrl.field = webvellaCoreService.initField(popupCtrl.wizard.selectedType.id);
            popupCtrl.wizard.steps[1].active = false;
            popupCtrl.wizard.steps[1].completed = true;
            popupCtrl.wizard.steps[2].active = true;
            if (typeId === 17 || typeId === 11) //If dropdown || multiselect
            {
                popupCtrl.field.options = [];
            }
            else if (typeId === 4) {// If date or datetime
                popupCtrl.field.format = "yyyy-MMM-dd";
            }
            else if (typeId === 5) {// If date or datetime
                popupCtrl.field.format = "yyyy-MMM-dd HH:mm";
            }
            else if (typeId === 21) { //if tree field
                popupCtrl.createFieldStep2Loading = true;
                //Tree select
                //List of entities eligible to be selected.Conditions:
                //1.Have 1:N or N:N relation with the current entity as origin, and the target and origin entity are not the same(and equal to the current)
                //2.Have existing trees
                popupCtrl.eligibleEntitiesForTreeProcessQueue = null;
                popupCtrl.eligibleEntitiesForTree = [];
                for (var i = 0; i < popupCtrl.relationsList.length; i++) {
                    if (popupCtrl.relationsList[i].originEntityId !== popupCtrl.relationsList[i].targetEntityId && popupCtrl.relationsList[i].targetEntityId === popupCtrl.ngCtrl.entity.id) {

                        if (popupCtrl.eligibleEntitiesForTreeProcessQueue === null) {
                            popupCtrl.eligibleEntitiesForTreeProcessQueue = {};
                        }
                        if (popupCtrl.eligibleEntitiesForTreeProcessQueue[popupCtrl.relationsList[i].originEntityId]) {
                            //entity already added we need just to push the new relations
                            popupCtrl.eligibleEntitiesForTreeProcessQueue[popupCtrl.relationsList[i].originEntityId].relations.push(popupCtrl.relationsList[i]);
                        }
                        else {
                            //entity not yet registered
                            var processItem = {
                                entityId: popupCtrl.relationsList[i].originEntityId,
                                relations: [],
                                processed: false
                            }
                            processItem.relations.push(popupCtrl.relationsList[i]);
                            popupCtrl.eligibleEntitiesForTreeProcessQueue[popupCtrl.relationsList[i].originEntityId] = processItem;
                        }

                    }
                }

                popupCtrl.selectedEntityTrees = [];
                popupCtrl.selectedEntityRelations = [];

                function reInitializeTreeAndRelationListBySelectedEntity(entity) {
                    popupCtrl.selectedEntityTrees = entity.recordTrees;
                    popupCtrl.selectedEntityRelations = entity.relations;
                    popupCtrl.field.selectedTreeId = entity.recordTrees[0].id;
                    popupCtrl.field.relationId = entity.relations[0].id;
                }


                function relatedEntitiesWithTreeSuccessCallback(response) {
                    popupCtrl.eligibleEntitiesForTreeProcessQueue[response.object.id].processed = true;
                    var entityTreeMeta = {};
                    entityTreeMeta.id = response.object.id;
                    entityTreeMeta.name = response.object.name;
                    entityTreeMeta.label = response.object.label;
                    entityTreeMeta.recordTrees = response.object.recordTrees;
                    entityTreeMeta.relations = popupCtrl.eligibleEntitiesForTreeProcessQueue[response.object.id].relations;
                    popupCtrl.eligibleEntitiesForTree.push(entityTreeMeta);
                    var allProcessed = true;
                    var nextForProcess = {};
                    for (var entityProperty in popupCtrl.eligibleEntitiesForTreeProcessQueue) {
                        if (popupCtrl.eligibleEntitiesForTreeProcessQueue.hasOwnProperty(entityProperty)) {
                            var processedItem = popupCtrl.eligibleEntitiesForTreeProcessQueue[entityProperty];
                            if (!processedItem.processed) {
                                nextForProcess = processedItem;
                                allProcessed = false;
                            }
                        }
                    }
                    if (!allProcessed) {
                        webvellaCoreService.getEntityMetaById(nextForProcess.entityId, relatedEntitiesWithTreeSuccessCallback, relatedEntitiesWithTreeErrorCallback);
                    }
                    else {
                        //All entities meta received
                        //Add only entities that have trees defined
                        var tempDictionaryOfEligibleEntities = [];
                        for (var m = 0; m < popupCtrl.eligibleEntitiesForTree.length; m++) {
                            if (popupCtrl.eligibleEntitiesForTree[m].recordTrees.length > 0) {
                                tempDictionaryOfEligibleEntities.push(popupCtrl.eligibleEntitiesForTree[m]);
                            }
                        }
                        popupCtrl.eligibleEntitiesForTree = tempDictionaryOfEligibleEntities;
                        if (popupCtrl.eligibleEntitiesForTree.length > 0) {
                            popupCtrl.field.relatedEntityId = popupCtrl.eligibleEntitiesForTree[0].id;
                            reInitializeTreeAndRelationListBySelectedEntity(popupCtrl.eligibleEntitiesForTree[0]);

                            popupCtrl.createFieldStep2Loading = false;
                        }
                        else {
                            popupCtrl.createFieldStep2Error = true;
                            $translate(['VALIDATION_TREE_SELECT_FIELD_NO_TREES_DEFINED']).then(function (translations) {
                                popupCtrl.createFieldStep2ErrorMessage = translations.VALIDATION_TREE_SELECT_FIELD_NO_TREES_DEFINED;
                            });
                            popupCtrl.createFieldStep2Loading = false;
                        }

                    }
                }
                function relatedEntitiesWithTreeErrorCallback() {

                }

                if (popupCtrl.eligibleEntitiesForTreeProcessQueue !== null) {
                    webvellaCoreService.getEntityMetaById(popupCtrl.eligibleEntitiesForTreeProcessQueue[Object.keys(popupCtrl.eligibleEntitiesForTreeProcessQueue)[0]].entityId, relatedEntitiesWithTreeSuccessCallback, relatedEntitiesWithTreeErrorCallback);
                }
                else {
                    popupCtrl.createFieldStep2Error = true;
                    $translate(['VALIDATION_TREE_SELECT_FIELD_NO_1_N_RELATION']).then(function (translations) {
                        popupCtrl.createFieldStep2ErrorMessage = translations.VALIDATION_TREE_SELECT_FIELD_NO_1_N_RELATION;
                    });
                    popupCtrl.createFieldStep2Loading = false;
                }
            }
        }
        popupCtrl.setActiveStep = function (stepIndex) {
            popupCtrl.createFieldStep2Error = false;
            popupCtrl.createFieldStep2ErrorMessage = "";
            if (popupCtrl.wizard.steps[stepIndex].completed) {
                for (var i = 1; i < 3; i++) {
                    popupCtrl.wizard.steps[i].active = false;
                }
                popupCtrl.wizard.steps[stepIndex].active = true;
            }
        }

        // Logic
        popupCtrl.completeStep2 = function () {
            popupCtrl.wizard.steps[2].active = false;
            popupCtrl.wizard.steps[2].completed = true;
        }

        popupCtrl.calendars = {};
        popupCtrl.openCalendar = function (event, name) {
            popupCtrl.calendars[name] = true;
        }



        //Currency
        popupCtrl.selectedCurrencyMeta = ngCtrl.currencyMetas[0].code;

        //Identifier GUID field specific functions
        popupCtrl.uniqueGuidGenerateCheckboxEnabled = true;
        popupCtrl.defaultValueTextboxEnabled = true;
        $translate(['GUID_INPUT_PLACEHOLDER']).then(function (translations) {
            popupCtrl.defaultValueTextboxPlaceholder = translations.GUID_INPUT_PLACEHOLDER;
        });
        popupCtrl.defaultValueTextboxValue = null;
        popupCtrl.uniqueGuidGenerateToggle = function (newValue) {
            if (popupCtrl.field.fieldTypeId === 16) {
                if (newValue) { // if it is checked
                    popupCtrl.defaultValueTextboxEnabled = false;
                    $translate(['GUID_AUTOGENERATED_PLACEHOLDER']).then(function (translations) {
                        popupCtrl.defaultValueTextboxPlaceholder = translations.GUID_AUTOGENERATED_PLACEHOLDER;
                    });
                    popupCtrl.defaultValueTextboxValue = popupCtrl.field.defaultValue;
                    popupCtrl.field.defaultValue = null;
                }
                else {
                    popupCtrl.defaultValueTextboxEnabled = true;
                    $translate(['GUID_INPUT_PLACEHOLDER']).then(function (translations) {
                        popupCtrl.defaultValueTextboxPlaceholder = translations.GUID_INPUT_PLACEHOLDER;
                    });
                    popupCtrl.field.defaultValue = popupCtrl.defaultValueTextboxValue;
                    popupCtrl.defaultValueTextboxValue = null;
                }
            }
        }
        popupCtrl.uniqueGuidPropertyChecked = function (newValue) {
            if (popupCtrl.field.fieldTypeId === 16) {
                if (newValue) {
                    popupCtrl.field.generateNewId = true;
                    popupCtrl.uniqueGuidGenerateCheckboxEnabled = false;
                    popupCtrl.uniqueGuidGenerateToggle(true);
                }
                else {
                    popupCtrl.field.generateNewId = false;
                    popupCtrl.uniqueGuidGenerateCheckboxEnabled = true;
                    popupCtrl.uniqueGuidGenerateToggle(false);
                }
            }
        }



        //////
        popupCtrl.ok = function () {
            popupCtrl.validation = {};
            switch (popupCtrl.field.fieldType) {
				case 1:
					if(!popupCtrl.field.displayFormat || popupCtrl.field.displayFormat  == ''){
						popupCtrl.field.displayFormat = null;
					}
					break;
                case 3:
                    for (var i = 0; i < ngCtrl.currencyMetas.length; i++) {
                        if (ngCtrl.currencyMetas[i].code === popupCtrl.selectedCurrencyMeta) {
                            popupCtrl.field.currency.symbol = ngCtrl.currencyMetas[i].symbol;
                            popupCtrl.field.currency.symbolNative = ngCtrl.currencyMetas[i].symbol_native;
                            popupCtrl.field.currency.name = ngCtrl.currencyMetas[i].name;
                            popupCtrl.field.currency.namePlural = ngCtrl.currencyMetas[i].name_plural;
                            popupCtrl.field.currency.code = ngCtrl.currencyMetas[i].code;
                            popupCtrl.field.currency.decimalDigits = ngCtrl.currencyMetas[i].decimal_digits;
                            popupCtrl.field.currency.rounding = ngCtrl.currencyMetas[i].rounding;
                            popupCtrl.field.currency.symbolPlacement = 1;
                        }
                    }
                    break;
                case 4: //Date
                    if (popupCtrl.field.defaultValue !== null) {
                        popupCtrl.field.defaultValue = moment(popupCtrl.field.defaultValue).startOf('day').utc().toDate();
                    }
                    break;
                case 5: //Date & Time
                    if (popupCtrl.field.defaultValue !== null) {
                        popupCtrl.field.defaultValue = moment(popupCtrl.field.defaultValue).startOf('minute').utc().toDate();
                    }
                    break;
                case 11: //Multiselect
                    if (popupCtrl.field.required && (!popupCtrl.field.defaultValue || popupCtrl.field.defaultValue.length == 0)) {
                        $translate(['ERROR_MESSAGE_LABEL']).then(function (translations) {
                            ngToast.create({
                                className: 'error',
                                content: translations.ERROR_MESSAGE_LABEL + ' ' + translations.ONE_OPTION_IS_REQUIRED
                            });
                        });

                    }
                    break;
            }
            webvellaCoreService.createField(popupCtrl.field, popupCtrl.ngCtrl.entity.id, successCallback, errorCallback);
        };

        popupCtrl.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };

        //Dropdown or Multiselect
        popupCtrl.addDropdownValue = function () {
            var option = {
                key: null,
                value: null
            };
            popupCtrl.field.options.push(option);
        }

        popupCtrl.deleteDropdownValue = function (index) {
            var defaultKeyIndex = -1;
            if (popupCtrl.isKeyDefault(popupCtrl.field.options[index].key)) {
                for (var j = 0; j < popupCtrl.field.defaultValue.length; j++) {
                    if (popupCtrl.field.defaultValue[j] !== popupCtrl.field.options[index].key) {
                        defaultKeyIndex = j;
                        break;
                    }
                }
                //if multiselect - default is an array
                if (Object.prototype.toString.call(popupCtrl.field.defaultValue) === '[object Array]') {
                    popupCtrl.field.defaultValue.splice(defaultKeyIndex, 1); //Remove from default
                }
                else if (typeof popupCtrl.field.defaultValue === 'string') {
                    popupCtrl.field.defaultValue = null;
                }
            }
            popupCtrl.field.options.splice(index, 1); // remove from options
        }

        popupCtrl.toggleKeyAsDefault = function (key) {
            if (!key || key == "") {
                $translate(['ERROR_MESSAGE_LABEL', 'VALIDATION_MULTISELECT_OPTION_SET_DEFAULT']).then(function (translations) {
                    ngToast.create({
                        className: 'error',
                        content: translations.ERROR_MESSAGE_LABEL + ' ' + translations.VALIDATION_MULTISELECT_OPTION_SET_DEFAULT
                    });
                });
            }
            else {
                if (!popupCtrl.field.defaultValue) {
                    popupCtrl.field.defaultValue = [];
                }
                if (popupCtrl.field.defaultValue.indexOf(key) > -1) {
                    //There could be multiple keys with the same key
                    var cleanArray = [];
                    for (var j = 0; j < popupCtrl.field.defaultValue.length; j++) {
                        if (popupCtrl.field.defaultValue[j] !== key) {
                            cleanArray.push(popupCtrl.field.defaultValue[j]);
                        }
                    }
                    popupCtrl.field.defaultValue = fastCopy(cleanArray);
                } else {
                    popupCtrl.field.defaultValue.push(key);
                }
            }
        }

        popupCtrl.isKeyDefault = function (key) {

            //if multiselect - default is an array
            if (Object.prototype.toString.call(popupCtrl.field.defaultValue) === '[object Array]') {
                if (popupCtrl.field.defaultValue.length > 0 && popupCtrl.field.defaultValue.indexOf(key) > -1) {
                    return true;
                } else {
                    return false;
                }
            }
            else if (typeof popupCtrl.field.defaultValue === 'string') {
                if (popupCtrl.field.defaultValue != null && popupCtrl.field.defaultValue == key) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        /// Aux
        function successCallback(response) {
            $translate(['SUCCESS_MESSAGE_LABEL']).then(function (translations) {
                ngToast.create({
                    className: 'success',
                    content: translations.SUCCESS_MESSAGE_LABEL + ' ' + response.message
                });
            });
            $uibModalInstance.close('success');
            webvellaCoreService.GoToState($state.current.name, {});
        }

        function errorCallback(response) {
            var location = $location;
            //Process the response and generate the validation Messages
            webvellaCoreService.generateValidationMessages(response, popupCtrl, popupCtrl.field, location);
        }
    };

    //// Create Field Controllers
    ManageFieldModalController.$inject = ['ngCtrl', 'resolvedField', '$uibModal', '$uibModalInstance', '$log', 'ngToast', '$timeout', '$state',
						'webvellaCoreService', '$location', 'resolvedRelatedEntity', '$sce', '$scope', '$translate'];

    function ManageFieldModalController(ngCtrl, resolvedField, $uibModal, $uibModalInstance, $log, ngToast, $timeout, $state,
						webvellaCoreService, $location, resolvedRelatedEntity, $sce, $scope, $translate) {

        var popupCtrl = this;
        popupCtrl.isEmpty = function (object) {
            return isEmpty(object)
        }
        //#region << Init >>
        popupCtrl.modalInstance = $uibModalInstance;
        popupCtrl.ngCtrl = ngCtrl;

        popupCtrl.field = fastCopy(resolvedField);
        if (popupCtrl.field.permissions === null) {
            popupCtrl.field.permissions = {
                canRead: [],
                canUpdate: []
            }
        }


        popupCtrl.fieldTypes = ngCtrl.fieldTypes;
        popupCtrl.fieldType = null;

        for (var i = 0; i < popupCtrl.fieldTypes.length; i++) {
            if (popupCtrl.fieldTypes[i].id === popupCtrl.field.fieldType) {
                popupCtrl.fieldType = popupCtrl.fieldTypes[i];
            }
        }

        //#endregion

        //#region << Logic >>

        //Generate roles and checkboxes
        popupCtrl.fieldPermissions = [];
        for (var i = 0; i < popupCtrl.ngCtrl.rolesList.data.length; i++) {

            //Now create the new entity.roles array
            var role = {};
            role.id = popupCtrl.ngCtrl.rolesList.data[i].id;
            role.label = popupCtrl.ngCtrl.rolesList.data[i].name;
            role.canRead = false;
            if (popupCtrl.field.permissions !== null && popupCtrl.field.permissions.canRead.indexOf(popupCtrl.ngCtrl.rolesList.data[i].id) > -1) {
                role.canRead = true;
            }
            role.canUpdate = false;
            if (popupCtrl.field.permissions !== null && popupCtrl.field.permissions.canUpdate.indexOf(popupCtrl.ngCtrl.rolesList.data[i].id) > -1) {
                role.canUpdate = true;
            }
            popupCtrl.fieldPermissions.push(role);
        }

        popupCtrl.togglePermissionRole = function (permission, roleId) {
            //Get the current state

            var permissionArrayRoleIndex = -1;
            if (popupCtrl.field.permissions !== null) {
                for (var k = 0; k < popupCtrl.field.permissions[permission].length; k++) {
                    if (popupCtrl.field.permissions[permission][k] === roleId) {
                        permissionArrayRoleIndex = k;
                    }
                }
            }

            if (permissionArrayRoleIndex !== -1) {
                popupCtrl.field.permissions[permission].splice(permissionArrayRoleIndex, 1);
            }
            else {
                if (popupCtrl.field.permissions === null) {
                    popupCtrl.field.permissions = {};
                    popupCtrl.field.permissions.canRead = [];
                    popupCtrl.field.permissions.canUpdate = [];
                }
                popupCtrl.field.permissions[permission].push(roleId);
            }

        }

        //Currency
        if (popupCtrl.field.fieldType === 3) {
            popupCtrl.selectedCurrencyMeta = ngCtrl.currencyMetas[0].code;
            if (popupCtrl.field.currency !== null && popupCtrl.field.currency !== {} && popupCtrl.field.currency.code) {
                for (var i = 0; i < ngCtrl.currencyMetas.length; i++) {
                    if (popupCtrl.field.currency.code === ngCtrl.currencyMetas[i].code) {
                        popupCtrl.selectedCurrencyMeta = ngCtrl.currencyMetas[i].code;
                    }
                }
            }
        }

        //Identifier GUID field specific functions
        popupCtrl.uniqueGuidGenerateCheckboxEnabled = true;
        popupCtrl.defaultValueTextboxEnabled = true;
        $translate(['GUID_INPUT_PLACEHOLDER']).then(function (translations) {
            popupCtrl.defaultValueTextboxPlaceholder = translations.GUID_INPUT_PLACEHOLDER;
        });
        popupCtrl.defaultValueTextboxValue = null;
        popupCtrl.uniqueGuidGenerateToggle = function (newValue) {
            if (newValue) { // if it is checked
                popupCtrl.defaultValueTextboxEnabled = false;
                $translate(['GUID_AUTOGENERATED_PLACEHOLDER']).then(function (translations) {
                    popupCtrl.defaultValueTextboxPlaceholder = translations.GUID_AUTOGENERATED_PLACEHOLDER;
                });
                popupCtrl.defaultValueTextboxValue = popupCtrl.field.defaultValue;
                popupCtrl.field.defaultValue = null;
            }
            else {
                popupCtrl.defaultValueTextboxEnabled = true;
                $translate(['GUID_INPUT_PLACEHOLDER']).then(function (translations) {
                    popupCtrl.defaultValueTextboxPlaceholder = translations.GUID_INPUT_PLACEHOLDER;
                });
                popupCtrl.field.defaultValue = popupCtrl.defaultValueTextboxValue;
                popupCtrl.defaultValueTextboxValue = null;
            }
        }

        popupCtrl.uniqueGuidPropertyChecked = function (newValue) {
            if (popupCtrl.field.fieldType === 16) {
                if (newValue) {
                    popupCtrl.field.generateNewId = true;
                    popupCtrl.uniqueGuidGenerateCheckboxEnabled = false;
                    popupCtrl.uniqueGuidGenerateToggle(true);
                }
                else {
                    popupCtrl.field.generateNewId = false;
                    popupCtrl.uniqueGuidGenerateCheckboxEnabled = true;
                    popupCtrl.uniqueGuidGenerateToggle(false);
                }
            }
        }

        // Date and Date time
        popupCtrl.calendars = {};
        popupCtrl.openCalendar = function (event, name) {
            popupCtrl.calendars[name] = true;
        }
        if (popupCtrl.field.fieldType === 4 || popupCtrl.field.fieldType === 5) {
            popupCtrl.field.defaultValue = moment(popupCtrl.field.defaultValue).toDate();
        }


        //Tree select
        var relationsList = fastCopy(popupCtrl.ngCtrl.relationsList);
        var selectedRelation = {};
        for (var i = 0; i < relationsList.length; i++) {
            if (relationsList[i].id === popupCtrl.field.relationId) {
                selectedRelation = relationsList[i];
            }
        }
        popupCtrl.getTreeSelectRelatedEntityName = function () {
            var relatedEntity = resolvedRelatedEntity;
            return "<i class='fa fa-fw fa-" + relatedEntity.iconName + "'></i> " + relatedEntity.name;
        }
        popupCtrl.getTreeSelectRecordTreeName = function () {
            var relatedEntity = resolvedRelatedEntity;
            for (var i = 0; i < relatedEntity.recordTrees.length; i++) {
                if (relatedEntity.recordTrees[i].id === popupCtrl.field.selectedTreeId) {
                    return "<i class='fa fa-fw fa-sitemap'></i> " + relatedEntity.recordTrees[i].name;
                }
            }
            return "";
        }
        popupCtrl.getTreeSelectRelationHtml = function () {
            var result = "unknown";
            if (selectedRelation) {
                if (selectedRelation.relationType === 2) {
                    result = $sce.trustAsHtml(selectedRelation.name + " <span class=\"badge badge-primary badge-inverse\" title=\"One to Many\" style=\"margin-left:5px;\">1 : N</span>");
                }
                else if (selectedRelation.relationType === 3) {
                    result = $sce.trustAsHtml(selectedRelation.name + ' <span class="badge badge-primary badge-inverse" title="Many to Many" style="margin-left:5px;">N : N</span>');
                }
            }
            return result;
        }

        //#region << Selection types >>
        popupCtrl.selectionTypes = [];
        $translate(['RECORD_TREE_SINGLE_SELECT_OPTION_VALUE', 'RECORD_TREE_MULTI_SELECT_OPTION_VALUE', 'RECORD_TREE_SINGLE_BRANCH_SELECT_OPTION_VALUE']).then(function (translations) {
            popupCtrl.singleSelectType = {
                key: "single-select",
                value: translations.RECORD_TREE_SINGLE_SELECT_OPTION_VALUE
            };
            popupCtrl.multiSelectType = {
                key: "multi-select",
                value: translations.RECORD_TREE_MULTI_SELECT_OPTION_VALUE
            };
            popupCtrl.singleBranchSelectType = {
                key: "single-branch-select",
                value: translations.RECORD_TREE_SINGLE_BRANCH_SELECT_OPTION_VALUE
            };
        });

        //#endregion

        //#region << Selection targets >>
        popupCtrl.selectionTargets = [];
        $translate(['RECORD_TREE_TARGET_ALL_NODES_OPTION_VALUE', 'RECORD_TREE_TARGET_ONLY_LEAVES_OPTION_VALUE']).then(function (translations) {
            popupCtrl.allNodesSelectTarget = {
                key: "all",
                value: translations.RECORD_TREE_TARGET_ALL_NODES_OPTION_VALUE
            };
            popupCtrl.multiSelectTarget = {
                key: "leaves",
                value: translations.RECORD_TREE_TARGET_ONLY_LEAVES_OPTION_VALUE
            };
            popupCtrl.selectionTargets.push(popupCtrl.allNodesSelectTarget);
            popupCtrl.selectionTargets.push(popupCtrl.multiSelectTarget);
        });

        //#endregion

        popupCtrl.selectionTypes = [];
        popupCtrl.selectionTypes.push(popupCtrl.singleSelectType);
        if (selectedRelation && selectedRelation.relationType === 3) {
            popupCtrl.selectionTypes.push(popupCtrl.multiSelectType);
            popupCtrl.selectionTypes.push(popupCtrl.singleBranchSelectType);
        }
        //#endregion

        popupCtrl.ok = function () {
            switch (popupCtrl.field.fieldType) {
				case 1:
					if(!popupCtrl.field.displayFormat || popupCtrl.field.displayFormat  == ''){
						popupCtrl.field.displayFormat = null;
					}
					break;
                case 3:
                    for (var i = 0; i < ngCtrl.currencyMetas.length; i++) {
                        if (ngCtrl.currencyMetas[i].code === popupCtrl.selectedCurrencyMeta) {
                            popupCtrl.field.currency.symbol = ngCtrl.currencyMetas[i].symbol;
                            popupCtrl.field.currency.symbolNative = ngCtrl.currencyMetas[i].symbol_native;
                            popupCtrl.field.currency.name = ngCtrl.currencyMetas[i].name;
                            popupCtrl.field.currency.namePlural = ngCtrl.currencyMetas[i].name_plural;
                            popupCtrl.field.currency.code = ngCtrl.currencyMetas[i].code;
                            popupCtrl.field.currency.decimalDigits = ngCtrl.currencyMetas[i].decimal_digits;
                            popupCtrl.field.currency.rounding = ngCtrl.currencyMetas[i].rounding;
                            popupCtrl.field.currency.symbolPlacement = 1;
                        }
                    }
                    break;
                case 4: //Date
                    if (popupCtrl.field.defaultValue !== null) {
                        popupCtrl.field.defaultValue = moment(popupCtrl.field.defaultValue).startOf('day').utc().toDate();
                    }
                    break;
                case 5: //Date & Time
                    if (popupCtrl.field.defaultValue !== null) {
                        popupCtrl.field.defaultValue = moment(popupCtrl.field.defaultValue).startOf('minute').utc().toDate();
                    }
                    break;
            }
            webvellaCoreService.updateField(popupCtrl.field, popupCtrl.ngCtrl.entity.id, successCallback, errorCallback);
        };

        popupCtrl.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };

        //Delete field
        //Create new field modal
        popupCtrl.deleteFieldModal = function () {
            // ReSharper disable once UnusedLocals
            var modalInstance = $uibModal.open({
                animation: false,
                templateUrl: 'deleteFieldModal.html',
                controller: 'DeleteFieldModalController',
                controllerAs: "popupCtrl",
                size: "",
                resolve: {
                    parentpopupCtrl: function () { return popupCtrl; }
                }
            });
        }

        //Dropdown or Multiselect
        popupCtrl.addDropdownValue = function () {
            var option = {
                key: null,
                value: null
            };
            popupCtrl.field.options.push(option);
        }

        popupCtrl.deleteDropdownValue = function (index) {
            //if the removed field is with the default value or in the default values list(multiselect)
            if (popupCtrl.field.defaultValue && popupCtrl.field.fieldType == 11) { //Multiselect
                var defaultKeyIndex = -1;
                if (popupCtrl.isKeyDefault(popupCtrl.field.options[index].key)) {
                    for (var j = 0; j < popupCtrl.field.defaultValue.length; j++) {
                        if (popupCtrl.field.defaultValue[j] === popupCtrl.field.options[index].key) {
                            defaultKeyIndex = j;
                            break;
                        }
                    }
                }
                popupCtrl.field.defaultValue.splice(defaultKeyIndex, 1); //Remove from default
            }
            else if (popupCtrl.field.defaultValue && popupCtrl.field.fieldType == 17) {
                if (popupCtrl.field.options[index].key === popupCtrl.field.defaultValue) {
                    popupCtrl.field.defaultValue = ""; //Remove from default										
                }

            }

            popupCtrl.field.options.splice(index, 1); // remove from options
        }

        popupCtrl.toggleKeyAsDefault = function (key) {
            if (popupCtrl.field.defaultValue.indexOf(key) > -1) {
                //There could be multiple keys with the same key
                var cleanArray = [];
                for (var j = 0; j < popupCtrl.field.defaultValue.length; j++) {
                    if (popupCtrl.field.defaultValue[j] !== key) {
                        cleanArray.push(popupCtrl.field.defaultValue[j]);
                    }
                }
                popupCtrl.field.defaultValue = fastCopy(cleanArray);
            } else {
                popupCtrl.field.defaultValue.push(key);
            }
        }

        popupCtrl.isKeyDefault = function (key) {
            if (popupCtrl.field.defaultValue && popupCtrl.field.defaultValue.indexOf(key) > -1) {
                return true;
            } else {
                return false;
            }
        }

        /// Aux
        function successCallback(response) {
            $translate(['SUCCESS_MESSAGE_LABEL']).then(function (translations) {
                ngToast.create({
                    className: 'success',
                    content: translations.SUCCESS_MESSAGE_LABEL + ' ' + response.message
                });
            });
            $uibModalInstance.close('success');
            webvellaCoreService.GoToState($state.current.name, {});
        }

        function errorCallback(response) {
            var location = $location;
            //Process the response and generate the validation Messages
            webvellaCoreService.generateValidationMessages(response, popupCtrl, popupCtrl.field, location);
        }
    };


    //// Modal Controllers
    DeleteFieldModalController.$inject = ['parentpopupCtrl', '$uibModalInstance', '$log', 'webvellaCoreService', 'ngToast', '$timeout', '$state', '$translate'];


    function DeleteFieldModalController(parentpopupCtrl, $uibModalInstance, $log, webvellaCoreService, ngToast, $timeout, $state, $translate) {

        var popupCtrl = this;
        popupCtrl.parentData = parentpopupCtrl;

        popupCtrl.ok = function () {
            webvellaCoreService.deleteField(popupCtrl.parentData.field.id, popupCtrl.parentData.ngCtrl.entity.id, successCallback, errorCallback);
        };

        popupCtrl.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };

        /// Aux
        function successCallback(response) {
            $translate(['SUCCESS_MESSAGE_LABEL']).then(function (translations) {
                ngToast.create({
                    className: 'success',
                    content: translations.SUCCESS_MESSAGE_LABEL + ' ' + response.message
                });
            });
            $uibModalInstance.close('success');
            popupCtrl.parentData.modalInstance.close('success');
            $timeout(function () {
                $state.go("webvella-admin-entity-fields", { name: popupCtrl.parentData.ngCtrl.entity.name }, { reload: true });
            }, 0);
        }

        function errorCallback(response) {
            popupCtrl.hasError = true;
            popupCtrl.errorMessage = response.message;


        }
    };


})();
