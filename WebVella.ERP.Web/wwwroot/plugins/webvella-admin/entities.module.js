/* home.module.js */

/**
* @desc this module manages the application home desktop screen
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminEntitiesController', controller)
        .controller('CreateEntityModalController', createEntityController);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];

    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-admin-entities', {
            parent: 'webvella-admin-base',
            url: '/entities', //  /desktop/areas after the parent state is prepended
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
                    controller: 'WebVellaAdminEntitiesController',
                    templateUrl: '/plugins/webvella-admin/entities.view.html',
                    controllerAs: 'contentData'
                }
            },
            resolve: {
                resolvedEntityMetaList: resolveEntityMetaList,
                resolvedRolesList:resolveRolesList
            },
            data: {

            }
        });
    };


    // Resolve EntityMetaList /////////////////////////
    resolveEntityMetaList.$inject = ['$q', '$log', 'webvellaAdminService'];

    /* @ngInject */
    function resolveEntityMetaList($q, $log, webvellaAdminService) {
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

        webvellaAdminService.getMetaEntityList(successCallback, errorCallback);

        // Return
        $log.debug('webvellaAdmin>entities> END state.resolved');
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

        webvellaRootService.getEntityRecordsByName("role", successCallback, errorCallback);

        // Return
        $log.debug('webvellaAdmin>entities> END state.resolved');
        return defer.promise;
    }


    // Controller ///////////////////////////////
    controller.$inject = ['$log', '$rootScope', '$state', 'pageTitle', 'resolvedEntityMetaList', '$modal', 'resolvedRolesList'];

    /* @ngInject */
    function controller($log, $rootScope, $state, pageTitle, resolvedEntityMetaList, $modal, resolvedRolesList) {
        $log.debug('webvellaAdmin>entities> START controller.exec');
        /* jshint validthis:true */
        var contentData = this;
        //Update page title
        contentData.pageTitle = "Entities | " + pageTitle;
        $rootScope.$emit("application-pageTitle-update", contentData.pageTitle);
        contentData.entities = resolvedEntityMetaList.entities;
        contentData.roles = resolvedRolesList.entities;

        //Create new entity modal
        contentData.openAddEntityModal = function () {
            var modalInstance = $modal.open({
                animation: false,
                templateUrl: 'createEntityModal.html',
                controller: 'CreateEntityModalController',
                controllerAs: "popupData",
                size: "lg",
                resolve: {
                    contentData: function () {
                        return contentData;
                    }
                }
            });

        }

        activate();
        $log.debug('webvellaAdmin>entities> END controller.exec');
        function activate() { }
    }


    //// Modal Controllers
    createEntityController.$inject = ['$modalInstance', '$log', 'webvellaAdminService', 'webvellaRootService', 'ngToast', '$timeout', '$state', '$location', 'contentData'];

    /* @ngInject */
    function createEntityController($modalInstance, $log, webvellaAdminService, webvellaRootService, ngToast, $timeout, $state, $location, contentData) {
        $log.debug('webvellaAdmin>entities>createEntityModal> START controller.exec');
        /* jshint validthis:true */
        var popupData = this;
        popupData.entity = webvellaAdminService.initEntity();
        popupData.roles = contentData.roles;
        
        //Processing the roles for generation the checkbox values
        popupData.entity.roles = [];

        for (var i = 0; i < popupData.roles.length; i++) {
            //Enable all checkboxes for administrators
            if (popupData.roles[i].name == "administrator") {
                popupData.entity.recordPermissions.canRead.push(popupData.roles[i].id);
                popupData.entity.recordPermissions.canCreate.push(popupData.roles[i].id);
                popupData.entity.recordPermissions.canUpdate.push(popupData.roles[i].id);
                popupData.entity.recordPermissions.canDelete.push(popupData.roles[i].id);
            }

            //Now create the new entity.roles array
            var role = {};
            role.id = popupData.roles[i].id;
            role.label = popupData.roles[i].label;
            role.canRead = false;
            if (popupData.entity.recordPermissions.canRead.indexOf(popupData.roles[i].id) > -1) {
                role.canRead = true;
            }
            role.canCreate = false;
            if (popupData.entity.recordPermissions.canCreate.indexOf(popupData.roles[i].id) > -1) {
                role.canCreate = true;
            }
            role.canUpdate = false;
            if (popupData.entity.recordPermissions.canUpdate.indexOf(popupData.roles[i].id) > -1) {
                role.canUpdate = true;
            }
            role.canDelete = false;
            if (popupData.entity.recordPermissions.canDelete.indexOf(popupData.roles[i].id) > -1) {
                role.canDelete = true;
            }
            popupData.entity.roles.push(role);
        }
        
        function removeValueFromArray(array, value) {
            for (var i = array.length - 1; i >= 0; i--) {
                if (array[i] === value) {
                    array.splice(i, 1);
                    // break;       //<-- Uncomment  if only the first term has to be removed
                }
            }
        }

        popupData.toggleCanRead = function (roleId) {
            if (popupData.entity.recordPermissions.canRead.indexOf(roleId) > -1) {
                //Found - should be removed
                removeValueFromArray(popupData.entity.recordPermissions.canRead, roleId);
            }
            else {
                //Not Found - should be added
                popupData.entity.recordPermissions.canRead.push(roleId);
            }
        }

        popupData.toggleCanCreate = function (roleId) {
            if (popupData.entity.recordPermissions.canCreate.indexOf(roleId) > -1) {
                //Found - should be removed
                removeValueFromArray(popupData.entity.recordPermissions.canCreate, roleId);
            }
            else {
                //Not Found - should be added
                popupData.entity.recordPermissions.canCreate.push(roleId);
            }
        }

        popupData.toggleCanUpdate = function (roleId) {
            if (popupData.entity.recordPermissions.canUpdate.indexOf(roleId) > -1) {
                //Found - should be removed
                removeValueFromArray(popupData.entity.recordPermissions.canUpdate, roleId);
            }
            else {
                //Not Found - should be added
                popupData.entity.recordPermissions.canUpdate.push(roleId);
            }
        }

        popupData.toggleCanDelete = function (roleId) {
            if (popupData.entity.recordPermissions.canDelete.indexOf(roleId) > -1) {
                //Found - should be removed
                removeValueFromArray(popupData.entity.recordPermissions.canDelete, roleId);
            }
            else {
                //Not Found - should be added
                popupData.entity.recordPermissions.canDelete.push(roleId);
            }
        }

        //Awesome font icon names array 
        popupData.icons = [
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

  


        popupData.ok = function () {
            webvellaAdminService.createEntity(popupData.entity, successCallback, errorCallback)
        };

        popupData.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

        /// Aux
        function successCallback(response) {
            ngToast.create({
                className: 'success',
                content: '<h4>Success</h4><p>The entity was successfully created</p>'
            });
            $modalInstance.close('success');
            webvellaRootService.reloadCurrentState($state);
        }

        function errorCallback(response) {
            var location = $location;
            //Process the response and generate the validation Messages
            webvellaRootService.generateValidationMessages(response, popupData,popupData.entity, location);
        }
        $log.debug('webvellaAdmin>entities>createEntityModal> END controller.exec');
    };

})();
