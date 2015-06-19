/* areas-lists.module.js */

/**
* @desc this module manages the entity record lists in the admin screen
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminAreasController', controller)
        .controller('ManageAreaModalController', manageAreaController)
        .controller('DeleteAreaModalController', DeleteAreaModalController);


    

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];

    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-admin-areas-lists', {
            parent: 'webvella-admin-base',
            url: '/areas/lists', 
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
                    controller: 'WebVellaAdminAreasController',
                    templateUrl: '/plugins/webvella-admin/areas.view.html',
                    controllerAs: 'contentData'
                }
            },
            resolve: {
                resolvedAreaRecordsList: resolveAreaRecordsList,
                resolvedRolesList: resolveRolesList,
                resolvedEntityMetaList: resolveEntityMetaList
            },
            data: {

            }
        });
    };


    //#region << Resolve Functions >>/////////////////////////

    resolveAreaRecordsList.$inject = ['$q', '$log', 'webvellaAdminService', '$stateParams', '$state', '$timeout'];
    /* @ngInject */
    function resolveAreaRecordsList($q, $log, webvellaAdminService, $stateParams, $state, $timeout) {
        $log.debug('webvellaAdmin>areas-list>resolveAreaRecordsList BEGIN state.resolved');
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

        webvellaAdminService.getRecordsByEntityName("area", successCallback, errorCallback);


        // Return
        $log.debug('webvellaAdmin>areas-list>resolveAreaRecordsList END state.resolved');
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

    //#endregion

    //#region << Controller >> ///////////////////////////////
    controller.$inject = ['$scope', '$log', '$rootScope', '$state', 'pageTitle', 'resolvedAreaRecordsList', 'resolvedRolesList', 'resolvedEntityMetaList', '$modal'];
    /* @ngInject */
    function controller($scope, $log, $rootScope, $state, pageTitle, resolvedAreaRecordsList,resolvedRolesList,resolvedEntityMetaList, $modal) {
        $log.debug('webvellaAdmin>areas-list> START controller.exec');
        /* jshint validthis:true */
        var contentData = this;
        contentData.search = {};
        //#region << Update page title >>
        contentData.pageTitle = "Areas List | " + pageTitle;
        $rootScope.$emit("application-pageTitle-update", contentData.pageTitle);
        //#endregion

        contentData.areas = resolvedAreaRecordsList.data.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });
        contentData.roles = resolvedRolesList.data.sort(function (a, b) {
            if (a.name < b.name) return -1;
            if (a.name > b.name) return 1;
            return 0;
        });
        contentData.entities = resolvedEntityMetaList.entities.sort(function (a, b) {
        	if (a.label < b.label) return -1;
        	if (a.label > b.label) return 1;
        	return 0;
        });

        //Create new entity modal
        contentData.openManageAreaModal = function (currentArea) {
            contentData.currentArea = currentArea;
            var modalInstance = $modal.open({
                animation: false,
                templateUrl: 'manageAreaModal.html',
                controller: 'ManageAreaModalController',
                controllerAs: "popupData",
                size: "lg",
                resolve: {
                    contentData: function () {
                        return contentData;
                    }
                }
            });

        }


        $log.debug('webvellaAdmin>areas-list> END controller.exec');
    }
    //#endregion


    //// Modal Controllers
    manageAreaController.$inject = ['$modalInstance', '$log', '$sce', '$modal', '$filter', 'webvellaAdminService', 'webvellaRootService', 'ngToast', '$timeout', '$state', '$location', 'contentData'];

    /* @ngInject */
    function manageAreaController($modalInstance, $log,$sce,$modal,$filter, webvellaAdminService, webvellaRootService, ngToast, $timeout, $state, $location, contentData) {
        $log.debug('webvellaAdmin>entities>createEntityModal> START controller.exec');
        /* jshint validthis:true */
        var popupData = this;
        popupData.modalInstance = $modalInstance;
        popupData.area = angular.copy(contentData.currentArea);
        popupData.roles = angular.copy(contentData.roles);
        popupData.entities = angular.copy(contentData.entities);
        popupData.isUpdate = true;
        if (popupData.area == null) {
            popupData.area = {};
            popupData.area.id = null;
            popupData.area.color = "red";
            popupData.area.icon_name = "database";
            popupData.area.weight = 10;
            popupData.area.roles = [];
            popupData.isUpdate = false;
            popupData.modalTitle = $sce.trustAsHtml("Create new area");
        }
        else {
            popupData.area.roles = JSON.parse(popupData.area.roles);
            popupData.modalTitle = $sce.trustAsHtml('Edit area <span class="go-green">' + popupData.area.label + '</span>');
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

		//Select entity typeahead

    	//Manage View section
        popupData.user = 2;

        popupData.statuses = [
		  { value: 1, text: 'status1' },
		  { value: 2, text: 'status2' },
		  { value: 3, text: 'status3' },
		  { value: 4, text: 'status4' }
        ];

        popupData.showStatus = function () {
        	var selected = $filter('filter')(popupData.statuses, { value: popupData.user });
        	return (popupData.user && selected.length) ? selected[0].text : 'Not set';
        };

        popupData.ok = function () {
            if (!popupData.isUpdate) {
                popupData.area.roles = JSON.stringify(popupData.area.roles);
                webvellaAdminService.createRecord("area", popupData.area, successCallback, errorCallback);
            }
            else {
                popupData.area.roles = JSON.stringify(popupData.area.roles);
                webvellaAdminService.updateRecord(popupData.area.id,"area", popupData.area, successCallback, errorCallback);
            } 
        };

        popupData.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

        /// Aux
        function successCallback(response) {
            ngToast.create({
                className: 'success',
                content: '<h4>Success</h4><p>The area was successfully saved</p>'
            });
            $modalInstance.close('success');
            webvellaRootService.reloadCurrentState($state);
        }

        function errorCallback(response) {
            var location = $location;
            //Process the response and generate the validation Messages
            webvellaRootService.generateValidationMessages(response, popupData, popupData.entity, location);
        }

        //Delete
        //Delete field
        //Create new field modal
        popupData.deleteAreaModal = function () {
            var modalInstance = $modal.open({
                animation: false,
                templateUrl: 'deleteAreaModal.html',
                controller: 'DeleteAreaModalController',
                controllerAs: "popupData",
                size: "",
                resolve: {
                    parentPopupData: function () { return popupData; }
                }
            });
        }

        $log.debug('webvellaAdmin>entities>createEntityModal> END controller.exec');
    };


    //// Modal Controllers
    DeleteAreaModalController.$inject = ['parentPopupData', '$modalInstance', '$log', 'webvellaAdminService', 'webvellaRootService', 'ngToast', '$timeout', '$state'];

    /* @ngInject */
    function DeleteAreaModalController(parentPopupData, $modalInstance, $log, webvellaAdminService, webvellaRootService, ngToast, $timeout, $state) {
        $log.debug('webvellaAdmin>entities>deleteFieldModal> START controller.exec');
        /* jshint validthis:true */
        var popupData = this;
        popupData.parentData = parentPopupData;

        popupData.ok = function () {

            webvellaAdminService.deleteArea(popupData.parentData.area.id, successCallback, errorCallback);

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
            webvellaRootService.reloadCurrentState($state);
        }

        function errorCallback(response) {
            popupData.hasError = true;
            popupData.errorMessage = response.message;


        }
        $log.debug('webvellaAdmin>entities>createEntityModal> END controller.exec');
    };


})();
