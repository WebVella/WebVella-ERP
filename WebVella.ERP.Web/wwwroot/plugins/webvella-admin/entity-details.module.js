/* entity-details.module.js */

/**
* @desc this module manages the entity record details in the admin screen
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminEntityDetailsController', controller)
        .controller('DeleteEntityModalController', deleteEntityController);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];

    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-admin-entity-details', {
            parent: 'webvella-admin-base',
            url: '/entities/:entityName', //  /desktop/areas after the parent state is prepended
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
                    controller: 'WebVellaAdminEntityDetailsController',
                    templateUrl: '/plugins/webvella-admin/entity-details.view.html',
                    controllerAs: 'contentData'
                }
            },
            resolve: {
                resolvedCurrentEntityMeta: resolveCurrentEntityMeta,
                resolvedRolesList: resolveRolesList,
                resolvedAreasList: resolveAreasList,
                resolvedRelatedAreasList: resolveRelatedAreasList
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

    // Resolve Roles list /////////////////////////
    resolveRolesList.$inject = ['$q', '$log', 'webvellaRootService'];
    /* @ngInject */
    function resolveRolesList($q, $log, webvellaRootService) {
        $log.debug('webvellaAdmin>entities> BEGIN state.resolved');
        // Initialize
        var defer = $q.defer();

        // Process
        function successCallback(response) {
            defer.resolve(response.object);
        }

        function errorCallback(response) {
            defer.resolve(response.object);
        }

        webvellaRootService.getEntityRecordsByName("null", "role", "null", "null", successCallback, errorCallback);

        // Return
        $log.debug('webvellaAdmin>entities> END state.resolved');
        return defer.promise;
    }

    // Resolve Roles list /////////////////////////
    resolveAreasList.$inject = ['$q', '$log', 'webvellaRootService'];
    /* @ngInject */
    function resolveAreasList($q, $log, webvellaRootService) {
        $log.debug('webvellaAdmin>entities> BEGIN state.resolved');
        // Initialize
        var defer = $q.defer();

        // Process
        function successCallback(response) {
            defer.resolve(response.object);
        }

        function errorCallback(response) {
            defer.resolve(response.object);
        }

        webvellaRootService.getEntityRecordsByName("null", "area", "null", "null", successCallback, errorCallback);

        // Return
        $log.debug('webvellaAdmin>entities> END state.resolved');
        return defer.promise;
    }


    // Entity Related Areas list /////////////////////////
    resolveRelatedAreasList.$inject = ['$q', '$log', 'webvellaAdminService', '$stateParams'];

    /* @ngInject */
    function resolveRelatedAreasList($q, $log, webvellaAdminService, $stateParams) {
        $log.debug('webvellaAdmin>entities> BEGIN state.resolved');
        // Initialize
        var defer = $q.defer();
        var entity = {};
        // Process Area
        function successCallback(response) {
            defer.resolve(response.object);
        }

        function errorCallback(response) {
            defer.resolve(response.object);
        }

        // Process Entity
        function successEntityCallback(response) {
            webvellaAdminService.getAreaRelationByEntityId(response.object.id, successCallback, errorCallback);
        }


        webvellaAdminService.getEntityMeta($stateParams.entityName, successEntityCallback, errorCallback);


        // Return
        $log.debug('webvellaAdmin>entities> END state.resolved');
        return defer.promise;
    }


    // Controller ///////////////////////////////
    controller.$inject = ['$scope', '$log', '$rootScope', '$state', 'pageTitle', 'ngToast', 'resolvedCurrentEntityMeta', '$modal',
        'resolvedRolesList', 'webvellaAdminService', 'resolvedAreasList', 'resolvedRelatedAreasList', '$timeout'];

    /* @ngInject */
    function controller($scope, $log, $rootScope, $state, pageTitle, ngToast, resolvedCurrentEntityMeta, $modal,
        resolvedRolesList, webvellaAdminService, resolvedAreasList, resolvedRelatedAreasList, $timeout) {
        $log.debug('webvellaAdmin>entity-details> START controller.exec');
        /* jshint validthis:true */
        var contentData = this;
        contentData.entity = resolvedCurrentEntityMeta;
        //Update page title
        contentData.pageTitle = "Entity Details | " + pageTitle;
        $rootScope.$emit("application-pageTitle-update", contentData.pageTitle);
        //Hide Sidemenu
        $rootScope.$emit("application-body-sidebar-menu-isVisible-update", false);
        $log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted');
        $scope.$on("$destroy", function () {
            $rootScope.$emit("application-body-sidebar-menu-isVisible-update", true);
            $log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted');
        });

        //Create new entity modal
        contentData.openDeleteEntityModal = function () {
            var modalInstance = $modal.open({
                animation: false,
                templateUrl: 'deleteEntityModal.html',
                controller: 'DeleteEntityModalController',
                controllerAs: "popupData",
                size: "",
                resolve: {
                    parentData: function () { return contentData; }
                }
            });

        }

        //Awesome font icon names array 
        contentData.icons = [
  "adjust",
  "adn",
  "align-center",
  "align-justify",
  "align-left",
  "align-right",
  "ambulance",
  "anchor",
  "android",
  "angellist",
  "angle-double-down",
  "angle-double-left",
  "angle-double-right",
  "angle-double-up",
  "angle-down",
  "angle-left",
  "angle-right",
  "angle-up",
  "apple",
  "archive",
  "area-chart",
  "arrow-circle-down",
  "arrow-circle-left",
  "arrow-circle-o-down",
  "arrow-circle-o-left",
  "arrow-circle-o-right",
  "arrow-circle-o-up",
  "arrow-circle-right",
  "arrow-circle-up",
  "arrow-down",
  "arrow-left",
  "arrow-right",
  "arrow-up",
  "arrows",
  "arrows-alt",
  "arrows-h",
  "arrows-v",
  "asterisk",
  "at",
  "backward",
  "ban",
  "bar-chart",
  "barcode",
  "bars",
  "bed",
  "beer",
  "behance",
  "behance-square",
  "bell",
  "bell-o",
  "bell-slash",
  "bell-slash-o",
  "bicycle",
  "binoculars",
  "birthday-cake",
  "bitbucket",
  "bitbucket-square",
  "bold",
  "bolt",
  "bomb",
  "book",
  "bookmark",
  "bookmark-o",
  "briefcase",
  "btc",
  "bug",
  "building",
  "building-o",
  "bullhorn",
  "bullseye",
  "bus",
  "buysellads",
  "calculator",
  "calendar",
  "calendar-o",
  "camera",
  "camera-retro",
  "car",
  "caret-down",
  "caret-left",
  "caret-right",
  "caret-square-o-down",
  "caret-square-o-left",
  "caret-square-o-right",
  "caret-square-o-up",
  "caret-up",
  "cart-arrow-down",
  "cart-plus",
  "cc",
  "cc-amex",
  "cc-discover",
  "cc-mastercard",
  "cc-paypal",
  "cc-stripe",
  "cc-visa",
  "certificate",
  "chain-broken",
  "check",
  "check-circle",
  "check-circle-o",
  "check-square",
  "check-square-o",
  "chevron-circle-down",
  "chevron-circle-left",
  "chevron-circle-right",
  "chevron-circle-up",
  "chevron-down",
  "chevron-left",
  "chevron-right",
  "chevron-up",
  "child",
  "circle",
  "circle-o",
  "circle-o-notch",
  "circle-thin",
  "clipboard",
  "clock-o",
  "cloud",
  "cloud-download",
  "cloud-upload",
  "code",
  "code-fork",
  "codepen",
  "coffee",
  "cog",
  "cogs",
  "columns",
  "comment",
  "comment-o",
  "comments",
  "comments-o",
  "compass",
  "compress",
  "connectdevelop",
  "copyright",
  "credit-card",
  "crop",
  "crosshairs",
  "css3",
  "cube",
  "cubes",
  "cutlery",
  "dashcube",
  "database",
  "delicious",
  "desktop",
  "deviantart",
  "diamond",
  "digg",
  "dot-circle-o",
  "download",
  "dribbble",
  "dropbox",
  "drupal",
  "eject",
  "ellipsis-h",
  "ellipsis-v",
  "empire",
  "envelope",
  "envelope-o",
  "envelope-square",
  "eraser",
  "eur",
  "exchange",
  "exclamation",
  "exclamation-circle",
  "exclamation-triangle",
  "expand",
  "external-link",
  "external-link-square",
  "eye",
  "eye-slash",
  "eyedropper",
  "facebook",
  "facebook-official",
  "facebook-square",
  "fast-backward",
  "fast-forward",
  "fax",
  "female",
  "fighter-jet",
  "file",
  "file-archive-o",
  "file-audio-o",
  "file-code-o",
  "file-excel-o",
  "file-image-o",
  "file-o",
  "file-pdf-o",
  "file-powerpoint-o",
  "file-text",
  "file-text-o",
  "file-video-o",
  "file-word-o",
  "files-o",
  "film",
  "filter",
  "fire",
  "fire-extinguisher",
  "flag",
  "flag-checkered",
  "flag-o",
  "flask",
  "flickr",
  "floppy-o",
  "folder",
  "folder-o",
  "folder-open",
  "folder-open-o",
  "font",
  "forumbee",
  "forward",
  "foursquare",
  "frown-o",
  "futbol-o",
  "gamepad",
  "gavel",
  "gbp",
  "gift",
  "git",
  "git-square",
  "github",
  "github-alt",
  "github-square",
  "glass",
  "globe",
  "google",
  "google-plus",
  "google-plus-square",
  "google-wallet",
  "graduation-cap",
  "gratipay",
  "h-square",
  "hacker-news",
  "hand-o-down",
  "hand-o-left",
  "hand-o-right",
  "hand-o-up",
  "hdd-o",
  "header",
  "headphones",
  "heart",
  "heart-o",
  "heartbeat",
  "history",
  "home",
  "hospital-o",
  "html5",
  "ils",
  "inbox",
  "indent",
  "info",
  "info-circle",
  "inr",
  "instagram",
  "ioxhost",
  "italic",
  "joomla",
  "jpy",
  "jsfiddle",
  "key",
  "keyboard-o",
  "krw",
  "language",
  "laptop",
  "lastfm",
  "lastfm-square",
  "leaf",
  "leanpub",
  "lemon-o",
  "level-down",
  "level-up",
  "life-ring",
  "lightbulb-o",
  "line-chart",
  "link",
  "linkedin",
  "linkedin-square",
  "linux",
  "list",
  "list-alt",
  "list-ol",
  "list-ul",
  "location-arrow",
  "lock",
  "long-arrow-down",
  "long-arrow-left",
  "long-arrow-right",
  "long-arrow-up",
  "magic",
  "magnet",
  "male",
  "map-marker",
  "mars",
  "mars-double",
  "mars-stroke",
  "mars-stroke-h",
  "mars-stroke-v",
  "maxcdn",
  "meanpath",
  "medium",
  "medkit",
  "meh-o",
  "mercury",
  "microphone",
  "microphone-slash",
  "minus",
  "minus-circle",
  "minus-square",
  "minus-square-o",
  "mobile",
  "money",
  "moon-o",
  "motorcycle",
  "music",
  "neuter",
  "newspaper-o",
  "openid",
  "outdent",
  "pagelines",
  "paint-brush",
  "paper-plane",
  "paper-plane-o",
  "paperclip",
  "paragraph",
  "pause",
  "paw",
  "paypal",
  "pencil",
  "pencil-square",
  "pencil-square-o",
  "phone",
  "phone-square",
  "picture-o",
  "pie-chart",
  "pied-piper",
  "pied-piper-alt",
  "pinterest",
  "pinterest-p",
  "pinterest-square",
  "plane",
  "play",
  "play-circle",
  "play-circle-o",
  "plug",
  "plus",
  "plus-circle",
  "plus-square",
  "plus-square-o",
  "power-off",
  "print",
  "puzzle-piece",
  "qq",
  "qrcode",
  "question",
  "question-circle",
  "quote-left",
  "quote-right",
  "random",
  "rebel",
  "recycle",
  "reddit",
  "reddit-square",
  "refresh",
  "renren",
  "repeat",
  "reply",
  "reply-all",
  "retweet",
  "road",
  "rocket",
  "rss",
  "rss-square",
  "rub",
  "scissors",
  "search",
  "search-minus",
  "search-plus",
  "sellsy",
  "server",
  "share",
  "share-alt",
  "share-alt-square",
  "share-square",
  "share-square-o",
  "shield",
  "ship",
  "shirtsinbulk",
  "shopping-cart",
  "sign-in",
  "sign-out",
  "signal",
  "simplybuilt",
  "sitemap",
  "skyatlas",
  "skype",
  "slack",
  "sliders",
  "slideshare",
  "smile-o",
  "sort",
  "sort-alpha-asc",
  "sort-alpha-desc",
  "sort-amount-asc",
  "sort-amount-desc",
  "sort-asc",
  "sort-desc",
  "sort-numeric-asc",
  "sort-numeric-desc",
  "soundcloud",
  "space-shuttle",
  "spinner",
  "spoon",
  "spotify",
  "square",
  "square-o",
  "stack-exchange",
  "stack-overflow",
  "star",
  "star-half",
  "star-half-o",
  "star-o",
  "steam",
  "steam-square",
  "step-backward",
  "step-forward",
  "stethoscope",
  "stop",
  "street-view",
  "strikethrough",
  "stumbleupon",
  "stumbleupon-circle",
  "subscript",
  "subway",
  "suitcase",
  "sun-o",
  "superscript",
  "table",
  "tablet",
  "tachometer",
  "tag",
  "tags",
  "tasks",
  "taxi",
  "tencent-weibo",
  "terminal",
  "text-height",
  "text-width",
  "th",
  "th-large",
  "th-list",
  "thumb-tack",
  "thumbs-down",
  "thumbs-o-down",
  "thumbs-o-up",
  "thumbs-up",
  "ticket",
  "times",
  "times-circle",
  "times-circle-o",
  "tint",
  "toggle-off",
  "toggle-on",
  "train",
  "transgender",
  "transgender-alt",
  "trash",
  "trash-o",
  "tree",
  "trello",
  "trophy",
  "truck",
  "try",
  "tty",
  "tumblr",
  "tumblr-square",
  "twitch",
  "twitter",
  "twitter-square",
  "umbrella",
  "underline",
  "undo",
  "university",
  "unlock",
  "unlock-alt",
  "upload",
  "usd",
  "user",
  "user-md",
  "user-plus",
  "user-secret",
  "user-times",
  "users",
  "venus",
  "venus-double",
  "venus-mars",
  "viacoin",
  "video-camera",
  "vimeo-square",
  "vine",
  "vk",
  "volume-down",
  "volume-off",
  "volume-up",
  "weibo",
  "weixin",
  "whatsapp",
  "wheelchair",
  "wifi",
  "windows",
  "wordpress",
  "wrench",
  "xing",
  "xing-square",
  "yahoo",
  "yelp",
  "youtube",
  "youtube-play",
  "youtube-square"
        ];

        //Get Areas list and selected areas for the entity
        contentData.areas = angular.copy(resolvedAreasList.data);
        contentData.areas = contentData.areas.sort(function (a, b) {
            if (a.label < b.label) return -1;
            if (a.label > b.label) return 1;
            return 0;
        });
        contentData.selectedAreasObjectsList = angular.copy(resolvedRelatedAreasList.data);
        contentData.selectedAreasList = [];
        for (var i = 0; i < contentData.selectedAreasObjectsList.length; i++) {
            contentData.selectedAreasList.push(contentData.selectedAreasObjectsList[i].area_id)
        }

        //Generate roles and checkboxes
        contentData.entity.roles = [];
        for (var i = 0; i < resolvedRolesList.data.length; i++) {

            //Now create the new entity.roles array
            var role = {};
            role.id = resolvedRolesList.data[i].id;
            role.label = resolvedRolesList.data[i].name;
            role.canRead = false;
            if (contentData.entity.recordPermissions.canRead.indexOf(resolvedRolesList.data[i].id) > -1) {
                role.canRead = true;
            }
            role.canCreate = false;
            if (contentData.entity.recordPermissions.canCreate.indexOf(resolvedRolesList.data[i].id) > -1) {
                role.canCreate = true;
            }
            role.canUpdate = false;
            if (contentData.entity.recordPermissions.canUpdate.indexOf(resolvedRolesList.data[i].id) > -1) {
                role.canUpdate = true;
            }
            role.canDelete = false;
            if (contentData.entity.recordPermissions.canDelete.indexOf(resolvedRolesList.data[i].id) > -1) {
                role.canDelete = true;
            }
            contentData.entity.roles.push(role);
        }

        contentData.fieldUpdate = function (key, data) {
            contentData.patchObject = {};
            contentData.patchObject[key] = data;
            webvellaAdminService.patchEntity(contentData.entity.id, contentData.patchObject, patchSuccessCallback, patchFailedCallback);
        }

        // Helper function
        function removeValueFromArray(array, value) {
            for (var i = array.length - 1; i >= 0; i--) {
                if (array[i] === value) {
                    array.splice(i, 1);
                    // break;       //<-- Uncomment  if only the first term has to be removed
                }
            }
        }

        contentData.permissionPatch = function (roleId, key, isEnabled) {
            contentData.patchObject = {};
            contentData.patchObject.recordPermissions = {};
            contentData.patchObject.recordPermissions = contentData.entity.recordPermissions;
            if (isEnabled) {
                contentData.entity.recordPermissions[key].push(roleId);
            }
            else {
                removeValueFromArray(contentData.entity.recordPermissions[key], roleId);
            }
            contentData.patchObject.recordPermissions[key] = contentData.entity.recordPermissions[key];
            webvellaAdminService.patchEntity(contentData.entity.id, contentData.patchObject, patchSuccessCallback, patchFailedCallback);
        }


        contentData.areaChange = function (areaId) {
            //console.log(areaId);
            //console.log(contentData.entity.id);
            $timeout(function () {
                //console.log(contentData.selectedAreasList);
                if (contentData.selectedAreasList.indexOf(areaId) > -1) {
                    //Added
                    //console.log("added");
                    webvellaAdminService.createAreaEntityRelation(areaId, contentData.entity.id,patchAreaSuccessCallback, patchFailedCallback);
                }
                else {
                    //Removed
                	//console.log("removed");
                	webvellaAdminService.removeAreaEntityRelation(areaId, contentData.entity.id, patchAreaSuccessCallback, patchFailedCallback);
                }
            },0);
        }

        function patchAreaSuccessCallback(response) {
            ngToast.create({
                className: 'success',
                content: '<span class="go-green">Success:</span> ' + 'Areas list saved'
            });
            return true;
        }

        function patchSuccessCallback(response) {
            ngToast.create({
                className: 'success',
                content: '<span class="go-green">Success:</span> ' + response.message
            });
            return true;
        }
        function patchFailedCallback(response) {
            ngToast.create({
                className: 'error',
                content: '<span class="go-red">Error:</span> ' + response.message
            });
            return false;
        }

        $log.debug('webvellaAdmin>entity-details> END controller.exec');
    }


    //// Modal Controllers
    deleteEntityController.$inject = ['parentData', '$modalInstance', '$log', 'webvellaAdminService', 'ngToast', '$timeout', '$state'];

    /* @ngInject */
    function deleteEntityController(parentData, $modalInstance, $log, webvellaAdminService, ngToast, $timeout, $state) {
        $log.debug('webvellaAdmin>entities>createEntityModal> START controller.exec');
        /* jshint validthis:true */
        var popupData = this;
        popupData.entity = parentData.entity;

        popupData.ok = function () {
            webvellaAdminService.deleteEntity(popupData.entity.id, successCallback, errorCallback)
        };

        popupData.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

        /// Aux
        function successCallback(response) {
            ngToast.create({
                className: 'success',
                content: '<span class="go-green">Success:</span> ' + response.message
            });
            $modalInstance.close('success');
            $timeout(function() {
                $state.go("webvella-admin-entities");
            }, 0);
        }

        function errorCallback(response) {
            popupData.hasError = true;
            popupData.errorMessage = response.message;


        }
        $log.debug('webvellaAdmin>entities>createEntityModal> END controller.exec');
    };

})();
