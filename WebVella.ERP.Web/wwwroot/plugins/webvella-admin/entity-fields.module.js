/* home.module.js */

/**
* @desc this module manages the application home desktop screen
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminEntityFieldsController', controller)
        .controller('CreateFieldModalController', CreateFieldModalController);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];

    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-admin-entity-fields', {
            parent: 'webvella-admin-base',
            url: '/entities/:name/fields', //  /desktop/areas after the parent state is prepended
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

        webvellaAdminService.getEntityMeta($stateParams.name, successCallback, errorCallback);

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
        contentData.entity = resolvedCurrentEntityMeta;
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

        //Create new entity modal
        contentData.createFieldModal = function () {
            var modalInstance = $modal.open({
                animation: false,
                templateUrl: 'createFieldModal.html',
                controller: 'CreateFieldModalController',
                controllerAs: "modalData",
                size: "lg",
                resolve: {
                    contentData: function () { return contentData; }
                }
            });

        }

        
        activate();
        $log.debug('webvellaAdmin>entity-details> END controller.exec');
        function activate() { }
    }


    //// Modal Controllers
    CreateFieldModalController.$inject = ['contentData', '$modalInstance', '$log', 'webvellaAdminService', 'ngToast', '$timeout', '$state'];

    /* @ngInject */
    function CreateFieldModalController(contentData,$modalInstance, $log, webvellaAdminService, ngToast, $timeout, $state) {
        $log.debug('webvellaAdmin>entities>createEntityModal> START controller.exec');
        /* jshint validthis:true */
        var modalData = this;
        modalData.contentData = contentData;

        modalData.fieldTypes = [
            {
                "id": 1,
                "name": "AutoNumberField",
                "label": "Auto increment",
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
                "name": "LookupRelationField",
                "label": "One to one relation",
                "description": "Lookup field that creates a relation between this object and another one. The other object's records will be represented with a field of your choice."
            },
            {
                "id": 11,
                "name": "MasterDetailsRelationshipField",
                "label": "One to many relation",
                "description": "With this field you can create a relation between one record and many others e.g. account and contacts "
            },
            {
                "id": 12,
                "name": "MultiLineTextField",
                "label": "Textarea",
                "description": "A textarea for plain text input. Will be cleaned and stripped from any web unsafe content"
            },
            {
                "id": 13,
                "name": "MultiSelectField",
                "label": "Multiple select",
                "description": "Multiple values can be selected from a provided list"
            },
            {
                "id": 14,
                "name": "NumberField",
                "label": "Number",
                "description": "Only numbers are allowed. Leading zeros will be stripped."
            },
            {
                "id": 15,
                "name": "PasswordField",
                "label": "Password",
                "description": "This field is suitable for submitting passwords or other data that needs to stay encrypted in the database"
            },
            {
                "id": 16,
                "name": "PercentField",
                "label": "Percent",
                "description": "This will automatically present any number you enter as a percent value"
            },
            {
                "id": 17,
                "name": "PhoneField",
                "label": "Phone",
                "description": "Will allow only valid phone numbers to be submitted"
            },
            {
                "id": 18,
                "name": "PrimaryKeyField",
                "label": "Primary Key",
                "description": "Very important field for any entity to entity relation. It will generate an unique key for each entity record, which can serve as identification."
            },
            {
                "id": 19,
                "name": "SelectField",
                "label": "Dropdown",
                "description": "One value can be selected from a provided list"
            },
            {
                "id": 20,
                "name": "TextField",
                "label": "Text",
                "description": "A simple text field. One of the most needed field nevertheless"
            },
            {
                "id": 21,
                "name": "UrlField",
                "label": "URL",
                "description": "This field will validate local and static URLs. Will present them accordingly"
            }    
        ];

        for (var i = 0; i < modalData.fieldTypes.length; i++) {
            modalData.fieldTypes[i].searchBox = modalData.fieldTypes[i].label + " " + modalData.fieldTypes[i].description;
        }



        modalData.ok = function () {
            webvellaAdminService.deleteEntity(modalData.entity.id, successCallback, errorCallback)
        };

        modalData.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

        /// Aux
        function successCallback(response) {
            ngToast.create({
                className: 'success',
                content: '<h4>Success</h4><p>' + response.message + '</p>'
            });
            $modalInstance.close('success');
            $timeout(function () {
                $state.go("webvella-admin-entities");
            }, 0)
        }

        function errorCallback(response) {
            modalData.hasError = true;
            modalData.errorMessage = response.message;


        }
        $log.debug('webvellaAdmin>entities>createEntityModal> END controller.exec');
    };

})();
