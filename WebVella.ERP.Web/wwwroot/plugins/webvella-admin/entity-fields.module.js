/* entity-fields.module.js */

/**
* @desc this module manages the entity record fields in the admin screen
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminEntityFieldsController', controller)
        .controller('CreateFieldModalController', CreateFieldModalController)
        .controller('ManageFieldModalController', ManageFieldModalController)
        .controller('DeleteFieldModalController', DeleteFieldModalController);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];
    /* @ngInject */
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
                    templateUrl: '/plugins/webvella-admin/sidebar.view.html',
                    controllerAs: 'sidebarData'
                },
                "contentView": {
                    controller: 'WebVellaAdminEntityFieldsController',
                    templateUrl: '/plugins/webvella-admin/entity-fields.view.html',
                    controllerAs: 'contentData'
                }
            },
            resolve: {
                resolvedCurrentEntityMeta: resolveCurrentEntityMeta
            },
            data: {

            }
        });
    };


    // Resolve Function /////////////////////////
    resolveCurrentEntityMeta.$inject = ['$q', '$log', 'webvellaAdminService', '$stateParams', '$state', '$timeout'];
    /* @ngInject */
    function resolveCurrentEntityMeta($q, $log, webvellaAdminService, $stateParams, $state, $timeout) {
        $log.debug('webvellaAdmin>entity-details> BEGIN state.resolved');
        // Initialize
        var defer = $q.defer();

        // Process
        function successCallback(response) {
            if (response.object == null) {
                $timeout(function () {
                    $state.go("webvella-root-not-found");
                }, 0);
            }
            else {
                defer.resolve(response.object);
            }
        }

        function errorCallback(response) {
            if (response.object == null) {
                $timeout(function () {
                    $state.go("webvella-root-not-found");
                }, 0);
            }
            else {
                defer.resolve(response.object);
            }
        }

        webvellaAdminService.getEntityMeta($stateParams.entityName, successCallback, errorCallback);

        // Return
        $log.debug('webvellaAdmin>entity-details> END state.resolved');
        return defer.promise;
    }

    // Controller ///////////////////////////////
    controller.$inject = ['$scope', '$log', '$rootScope', '$state', 'pageTitle', 'resolvedCurrentEntityMeta', '$modal'];
    /* @ngInject */
    function controller($scope, $log, $rootScope, $state, pageTitle, resolvedCurrentEntityMeta, $modal) {
        $log.debug('webvellaAdmin>entity-details> START controller.exec');
        /* jshint validthis:true */
        var contentData = this;
        contentData.search = {};
        contentData.entity = resolvedCurrentEntityMeta;

        contentData.entity.fields = contentData.entity.fields.sort(function (a, b) {
            if (a.name < b.name) return -1;
            if (a.name > b.name) return 1;
            return 0;
        });

        //Update page title
        contentData.pageTitle = "Entity Fields | " + pageTitle;
        $rootScope.$emit("application-pageTitle-update", contentData.pageTitle);
        //Hide Sidemenu
        $rootScope.$emit("application-body-sidebar-menu-isVisible-update", false);
        $log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted');
        $scope.$on("$destroy", function () {
            $rootScope.$emit("application-body-sidebar-menu-isVisible-update", true);
            $log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted');
        });

        contentData.fieldTypes = [
            {
                "id": 1,
                "name": "AutoNumberField",
                "label": "Auto increment number",
                "description": "If you need a automatically incremented number with each new record, this is the field you need. You can customize the display format also."
            },
            {
                "id": 2,
                "name": "CheckboxField",
                "label": "Checkbox",
                "description": "The simple on and off switch. This field allows you to get a True (checked) or False (unchecked) value."
            },
            {
                "id": 3,
                "name": "CurrencyField",
                "label": "Currency",
                "description": "A currency amount can be entered and will be represented in a suitable formatted way"
            },
            {
                "id": 4,
                "name": "DateField",
                "label": "Date",
                "description": "A data pickup field, that can be later converting according to a provided pattern"
            },
            {
                "id": 5,
                "name": "DateTimeField",
                "label": "Date & Time",
                "description": "A date and time can be picked up and later presented according to a provided pattern"
            },
            {
                "id": 6,
                "name": "EmailField",
                "label": "Email",
                "description": "An email can be entered by the user, which will be validated and presented accordingly"
            },
            {
                "id": 7,
                "name": "FileField",
                "label": "File",
                "description": "File upload field. Files will be stored within the system."
            },
            {
                "id": 8,
                "name": "HtmlField",
                "label": "HTML",
                "description": "Provides the ability of entering and presenting an HTML code. Supports multiple input languages."
            },
            {
                "id": 9,
                "name": "ImageField",
                "label": "Image",
                "description": "Image upload field. Images will be stored within the system"
            },
            {
                "id": 10,
                "name": "MultiLineTextField",
                "label": "Textarea",
                "description": "A textarea for plain text input. Will be cleaned and stripped from any web unsafe content"
            },
            {
                "id": 11,
                "name": "MultiSelectField",
                "label": "Multiple select",
                "description": "Multiple values can be selected from a provided list"
            },
            {
                "id": 12,
                "name": "NumberField",
                "label": "Number",
                "description": "Only numbers are allowed. Leading zeros will be stripped."
            },
            {
                "id": 13,
                "name": "PasswordField",
                "label": "Password",
                "description": "This field is suitable for submitting passwords or other data that needs to stay encrypted in the database"
            },
            {
                "id": 14,
                "name": "PercentField",
                "label": "Percent",
                "description": "This will automatically present any number you enter as a percent value"
            },
            {
                "id": 15,
                "name": "PhoneField",
                "label": "Phone",
                "description": "Will allow only valid phone numbers to be submitted"
            },
            {
                "id": 16,
                "name": "GuidField",
                "label": "Identifier GUID",
                "description": "Very important field for any entity to entity relation and required by it"
            },
            {
                "id": 17,
                "name": "SelectField",
                "label": "Dropdown",
                "description": "One value can be selected from a provided list"
            },
            {
                "id": 18,
                "name": "TextField",
                "label": "Text",
                "description": "A simple text field. One of the most needed field nevertheless"
            },
            {
                "id": 19,
                "name": "UrlField",
                "label": "URL",
                "description": "This field will validate local and static URLs. Will present them accordingly"
            }
        ];


        //Create new field modal
        contentData.createFieldModal = function () {
            var modalInstance = $modal.open({
                animation: false,
                templateUrl: 'createFieldModal.html',
                controller: 'CreateFieldModalController',
                controllerAs: "popupData",
                size: "lg",
                resolve: {
                    contentData: function () { return contentData; }
                }
            });
        }

        //Manage field modal
        contentData.manageFieldModal = function (fieldId) {
            var modalInstance = $modal.open({
                animation: false,
                templateUrl: 'manageFieldModal.html',
                controller: 'ManageFieldModalController',
                controllerAs: "popupData",
                size: "lg",
                resolve: {
                    contentData: function () { return contentData; },
                    resolvedField: function () {
                        var managedField = null;
                        for (var i = 0; i < contentData.entity.fields.length; i++) {
                            if (contentData.entity.fields[i].id === fieldId) {
                                managedField = contentData.entity.fields[i];
                            }
                        }
                        return managedField;
                    }
                }
            });
        }


        activate();
        $log.debug('webvellaAdmin>entity-details> END controller.exec');
        function activate() { }
    }

    //// Create Field Controllers
    CreateFieldModalController.$inject = ['contentData', '$modalInstance', '$log', 'webvellaAdminService', 'ngToast', '$timeout', '$state', 'webvellaRootService', '$location'];
    /* @ngInject */
    function CreateFieldModalController(contentData, $modalInstance, $log, webvellaAdminService, ngToast, $timeout, $state, webvellaRootService, $location) {
        $log.debug('webvellaAdmin>entities>CreateFieldModalController> START controller.exec');
        /* jshint validthis:true */
        var popupData = this;

        popupData.contentData = contentData;

        popupData.field = {};

        popupData.fieldTypes = contentData.fieldTypes;
        // Inject a searchable field
        for (var i = 0; i < popupData.fieldTypes.length; i++) {
            popupData.fieldTypes[i].searchBox = popupData.fieldTypes[i].label + " " + popupData.fieldTypes[i].description;
        }

        //Wizard
        popupData.wizard = {};
        popupData.wizard.steps = [];
        //Initialize steps
        var step = angular.copy({ "active": false }, { "completed": false });
        popupData.wizard.steps.push(step); // Dummy step
        step = angular.copy({ "active": false }, { "completed": false });
        popupData.wizard.steps.push(step); // Step 1
        step = angular.copy({ "active": false }, { "completed": false });
        popupData.wizard.steps.push(step); // Step 2
        // Set steps
        popupData.wizard.steps[1].active = true;
        popupData.wizard.selectedType = null;

        popupData.selectType = function (typeId) {
            var typeIndex = typeId - 1;
            popupData.wizard.selectedType = popupData.fieldTypes[typeIndex];
            popupData.field = webvellaAdminService.initField(popupData.wizard.selectedType.id);

            popupData.wizard.steps[1].active = false;
            popupData.wizard.steps[1].completed = true;
            popupData.wizard.steps[2].active = true;
            if (typeId == 17 || typeId == 11) //If dropdown || multiselect
            {
            	popupData.field.options = [];
            }
        }
        popupData.setActiveStep = function (stepIndex) {
            if (popupData.wizard.steps[stepIndex].completed) {
                for (var i = 1; i < 3; i++) {
                    popupData.wizard.steps[i].active = false;
                }
                popupData.wizard.steps[stepIndex].active = true;
            }
        }

        //////
        popupData.completeStep2 = function () {
            popupData.wizard.steps[2].active = false;
            popupData.wizard.steps[2].completed = true;
        }


        //Identifier GUID field specific functions
        popupData.uniqueGuidGenerateCheckboxEnabled = true;
        popupData.defaultValueTextboxEnabled = true;
        popupData.defaultValueTextboxPlaceholder = "fill in a GUID";
        popupData.defaultValueTextboxValue = null;
        popupData.uniqueGuidGenerateToggle = function (newValue) {
            if (newValue) { // if it is checked
                popupData.defaultValueTextboxEnabled = false;
                popupData.defaultValueTextboxPlaceholder = "will be auto-generated";
                popupData.defaultValueTextboxValue = popupData.field.defaultValue;
                popupData.field.defaultValue = null;
            }
            else {
                popupData.defaultValueTextboxEnabled = true;
                popupData.defaultValueTextboxPlaceholder = "fill in a GUID";
                popupData.field.defaultValue = popupData.defaultValueTextboxValue;
                popupData.defaultValueTextboxValue = null;
            }
        }

        popupData.uniqueGuidPropertyChecked = function (newValue) {
            if (newValue) {
                popupData.field.generateNewId = true;
                popupData.uniqueGuidGenerateCheckboxEnabled = false;
                popupData.uniqueGuidGenerateToggle(true);
            }
            else {
                popupData.field.generateNewId = false;
                popupData.uniqueGuidGenerateCheckboxEnabled = true;
                popupData.uniqueGuidGenerateToggle(false);
            }
        }



        //////
        popupData.ok = function () {
            webvellaAdminService.createField(popupData.field, popupData.contentData.entity.id, successCallback, errorCallback);
        };

        popupData.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

    	//Dropdown or Multiselect
        popupData.addDropdownValue = function () {
        	var option = {
        		key: null,
        		value: null
        	};
        	popupData.field.options.push(option);
        }

        popupData.deleteDropdownValue = function (index) {
        	popupData.field.options.splice(index, 1);
        }


        /// Aux
        function successCallback(response) {
            ngToast.create({
                className: 'success',
                content: '<h4>Success</h4><p>' + response.message + '</p>'
            });
            $modalInstance.close('success');
            webvellaRootService.reloadCurrentState($state, {});
        }

        function errorCallback(response) {
            var location = $location;
            //Process the response and generate the validation Messages
            webvellaRootService.generateValidationMessages(response, popupData, popupData.field, location);
        }
        $log.debug('webvellaAdmin>entities>CreateFieldModalController> END controller.exec');
    };

    //// Create Field Controllers
    ManageFieldModalController.$inject = ['contentData', 'resolvedField','$modal', '$modalInstance', '$log', 'webvellaAdminService', 'ngToast', '$timeout', '$state', 'webvellaRootService', '$location'];
    /* @ngInject */
    function ManageFieldModalController(contentData, resolvedField,$modal, $modalInstance, $log, webvellaAdminService, ngToast, $timeout, $state, webvellaRootService, $location) {
        $log.debug('webvellaAdmin>entities>ManageFieldModalController> START controller.exec');
        /* jshint validthis:true */
        var popupData = this;
        popupData.modalInstance = $modalInstance;
        popupData.contentData = contentData;

        popupData.field = angular.copy(resolvedField);

        popupData.fieldTypes = contentData.fieldTypes;
        popupData.fieldType = null;

        for (var i = 0; i < popupData.fieldTypes.length; i++) {
            if (popupData.fieldTypes[i].id === popupData.field.fieldType) {
                popupData.fieldType = popupData.fieldTypes[i];
            }
        }

        //Identifier GUID field specific functions
        popupData.uniqueGuidGenerateCheckboxEnabled = true;
        popupData.defaultValueTextboxEnabled = true;
        popupData.defaultValueTextboxPlaceholder = "fill in a GUID";
        popupData.defaultValueTextboxValue = null;
        popupData.uniqueGuidGenerateToggle = function (newValue) {
            if (newValue) { // if it is checked
                popupData.defaultValueTextboxEnabled = false;
                popupData.defaultValueTextboxPlaceholder = "will be auto-generated";
                popupData.defaultValueTextboxValue = popupData.field.defaultValue;
                popupData.field.defaultValue = null;
            }
            else {
                popupData.defaultValueTextboxEnabled = true;
                popupData.defaultValueTextboxPlaceholder = "fill in a GUID";
                popupData.field.defaultValue = popupData.defaultValueTextboxValue;
                popupData.defaultValueTextboxValue = null;
            }
        }

        popupData.uniqueGuidPropertyChecked = function (newValue) {
            if (newValue) {
                popupData.field.generateNewId = true;
                popupData.uniqueGuidGenerateCheckboxEnabled = false;
                popupData.uniqueGuidGenerateToggle(true);
            }
            else {
                popupData.field.generateNewId = false;
                popupData.uniqueGuidGenerateCheckboxEnabled = true;
                popupData.uniqueGuidGenerateToggle(false);
            }
        }




        /////
        popupData.ok = function () {
            webvellaAdminService.updateField(popupData.field, popupData.contentData.entity.id, successCallback, errorCallback);
        };

        popupData.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

        //Delete field
        //Create new field modal
        popupData.deleteFieldModal = function () {
            var modalInstance = $modal.open({
                animation: false,
                templateUrl: 'deleteFieldModal.html',
                controller: 'DeleteFieldModalController',
                controllerAs: "popupData",
                size: "",
                resolve: {
                    parentPopupData: function () { return popupData; }
                }
            });
        }

    	//Dropdown or Multiselect
        popupData.addDropdownValue = function () {
        	var option = {
        		key: null,
        		value: null
        	};
        	popupData.field.options.push(option);
        }

        popupData.deleteDropdownValue = function (index) {
        	popupData.field.options.splice(index,1);
        }

        /// Aux
        function successCallback(response) {
            ngToast.create({
                className: 'success',
                content: '<h4>Success</h4><p>' + response.message + '</p>'
            });
            $modalInstance.close('success');
            webvellaRootService.reloadCurrentState($state, {});
        }

        function errorCallback(response) {
            var location = $location;
            //Process the response and generate the validation Messages
            webvellaRootService.generateValidationMessages(response, popupData, popupData.field, location);
        }
        $log.debug('webvellaAdmin>entities>ManageFieldModalController> END controller.exec');
    };


    //// Modal Controllers
    DeleteFieldModalController.$inject = ['parentPopupData', '$modalInstance', '$log', 'webvellaAdminService', 'ngToast', '$timeout', '$state'];

    /* @ngInject */
    function DeleteFieldModalController(parentPopupData, $modalInstance, $log, webvellaAdminService, ngToast, $timeout, $state) {
        $log.debug('webvellaAdmin>entities>deleteFieldModal> START controller.exec');
        /* jshint validthis:true */
        var popupData = this;
        popupData.parentData = parentPopupData;

        popupData.ok = function () {
            webvellaAdminService.deleteField(popupData.parentData.field.id, popupData.parentData.contentData.entity.id, successCallback, errorCallback);
        };

        popupData.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

        /// Aux
        function successCallback(response) {
            ngToast.create({
                className: 'success',
                content: '<h4>Success</h4><p>' + response.message + '</p>'
            });
            $modalInstance.close('success');
            popupData.parentData.modalInstance.close('success');
            $timeout(function() {
                $state.go("webvella-admin-entity-fields", { name: popupData.parentData.contentData.entity.name }, { reload: true });
            }, 0);
        }

        function errorCallback(response) {
            popupData.hasError = true;
            popupData.errorMessage = response.message;


        }
        $log.debug('webvellaAdmin>entities>createEntityModal> END controller.exec');
    };


})();
