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
   
    ///////////////////////////////////////////////////////
    /// Configuration
    ///////////////////////////////////////////////////////

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
            	resolvedAreaEntityRelationRecords:resolveAreaEntityRelationRecords,
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

        webvellaAdminService.getRecordsByEntityName("null","area","null","null", successCallback, errorCallback);


        // Return
        $log.debug('webvellaAdmin>areas-list>resolveAreaRecordsList END state.resolved');
        return defer.promise;
    }


    resolveAreaEntityRelationRecords.$inject = ['$q', '$log', 'webvellaAdminService', '$stateParams', '$state', '$timeout'];
	/* @ngInject */
    function resolveAreaEntityRelationRecords($q, $log, webvellaAdminService, $stateParams, $state, $timeout) {
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

    	webvellaAdminService.getRecordsByEntityName("null", "areas_entities", "null", "null", successCallback, errorCallback);


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

        webvellaRootService.getEntityRecordsByName("null","role","null","null", successCallback, errorCallback);

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
    controller.$inject = ['$scope', '$log', '$rootScope', '$state', 'pageTitle', 'resolvedAreaRecordsList',
							'resolvedAreaEntityRelationRecords', 'resolvedRolesList', 'resolvedEntityMetaList', '$modal',
                            'webvellaAdminService'];
    /* @ngInject */
    function controller($scope, $log, $rootScope, $state, pageTitle, resolvedAreaRecordsList,
						resolvedAreaEntityRelationRecords, resolvedRolesList, resolvedEntityMetaList, $modal,
                        webvellaAdminService) {
        $log.debug('webvellaAdmin>areas-list> START controller.exec');
        /* jshint validthis:true */
        var contentData = this;
        contentData.search = {};
        //#region << Update page title >>
        contentData.pageTitle = "Areas List | " + pageTitle;
        $rootScope.$emit("application-pageTitle-update", contentData.pageTitle);
    	//#endregion

        contentData.areas = angular.copy(resolvedAreaRecordsList.data);
        contentData.areas = contentData.areas.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });

        contentData.areaEntityRelations = angular.copy(resolvedAreaEntityRelationRecords.data);

        contentData.roles = angular.copy(resolvedRolesList.data);
        contentData.roles = contentData.roles.sort(function (a, b) {
            if (a.name < b.name) return -1;
            if (a.name > b.name) return 1;
            return 0;
        });

        contentData.entities = angular.copy(resolvedEntityMetaList.entities)
        contentData.entities = contentData.entities.sort(function (a, b) {
        	if (a.label < b.label) return -1;
        	if (a.label > b.label) return 1;
        	return 0;
        });

        //Create new entity modal
        contentData.openManageAreaModal = function (currentArea) {
            if (currentArea != null) {
                contentData.currentArea = currentArea;
            }
            else {
                contentData.currentArea = webvellaAdminService.initArea();
            }
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
        popupData.areaEntityRelations = angular.copy(contentData.areaEntityRelations);
        popupData.roles = angular.copy(contentData.roles);
        popupData.entities = angular.copy(contentData.entities);
        popupData.subscribedEntities = [];
        if (popupData.area.subscriptions != null && popupData.area.subscriptions.length > 0 ) {
            popupData.subscribedEntities = angular.fromJson(popupData.area.subscriptions);
        }

        popupData.cleanEntities = [];

        //Add only entities that have default view and list
        for (var i = 0; i < popupData.entities.length; i++) {
            var hasDefaultView = false;
            var hasDefaultList = false;
            //check if has default view
            for (var v = 0; v < popupData.entities[i].recordViews.length; v++) {
                if (popupData.entities[i].recordViews[v].default && popupData.entities[i].recordViews[v].type === "general") {
                    hasDefaultView = true;
                }
            }
            //check if has default list
            for (var l = 0; l < popupData.entities[i].recordLists.length; l++) {
                if (popupData.entities[i].recordLists[l].default && popupData.entities[i].recordLists[l].type === "general") {
                    hasDefaultList = true;
                }
            }

            if (hasDefaultView && hasDefaultList) {
                popupData.cleanEntities.push(popupData.entities[i]);
            }
        }
        //Soft alphabetically
        popupData.cleanEntities = popupData.cleanEntities.sort(function (a, b) {
            if (a.label < b.label) return -1;
            if (a.label > b.label) return 1;
            return 0;
        });


        popupData.isUpdate = true;
        if (popupData.area.id == null) {
            popupData.isUpdate = false;
            popupData.modalTitle = $sce.trustAsHtml("Create new area");
        }
        else {
            popupData.area.roles = angular.fromJson(popupData.area.roles);

            //Remove the already subscribed from the available for subscription list
        	popupData.tempEntitiesList = [];
        	for (var i = 0; i < popupData.cleanEntities.length; i++) {
        		var isSubscribed = false;
        		//check if subscribed
        		for (var j = 0; j < popupData.subscribedEntities.length; j++) {
        		    if (popupData.cleanEntities[i].name === popupData.subscribedEntities[j].name) {
        				isSubscribed = true;
        			}
        		}

        		if (!isSubscribed) {
        		    popupData.tempEntitiesList.push(popupData.cleanEntities[i]);
        		}
        	}
        	//Soft alphabetically
        	popupData.cleanEntities = popupData.tempEntitiesList.sort(function (a, b) {
        		if (a.name < b.name) return -1;
        		if (a.na,e > b.name) return 1;
        		return 0;
        	});

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

    	//Manage Inline edit
        popupData.getViews = function (entityName) {
            var views = [];

            for (var i = 0; i < popupData.entities.length; i++) {
                if (popupData.entities[i].name == entityName) {
                    views = popupData.entities[i].recordViews;
                    break;
                }
            }
            return views;
        }
        popupData.updateSubscriptionView = function (subscription) {
            for (var i = 0; i < popupData.entities.length; i++) {
                if (popupData.entities[i].name == subscription.name) {
                    for (var j = 0; j < popupData.entities[i].recordViews.length; j++) {
                        if (popupData.entities[i].recordViews[j].name == subscription.view.name) {
                            subscription.view.label = popupData.entities[i].recordViews[j].label;
                            break;
                        }
                    }

                    break;
                }
            }
        }

        popupData.getLists = function (entityName) {
            var lists = [];

            for (var i = 0; i < popupData.entities.length; i++) {
                if (popupData.entities[i].name == entityName) {
                    lists = popupData.entities[i].recordLists;
                    break;
                }
            }
            return lists;
        }
        popupData.updateSubscriptionList = function (subscription) {
            for (var i = 0; i < popupData.entities.length; i++) {
                if (popupData.entities[i].name == subscription.name) {
                    for (var j = 0; j < popupData.entities[i].recordLists.length; j++) {
                        if (popupData.entities[i].recordLists[j].name == subscription.list.name) {
                            subscription.list.label = popupData.entities[i].recordLists[j].label;
                            break;
                        }
                    }
                    break;
                }
            }
        }


        //Attach entity
        popupData.attachEntity = function(name) {
            //Find the entity
            var selectedEntity = {
                name: null,
                label: null,
                weight: null
            };
            selectedEntity.view = {
                name: null,
                label: null
            };

            selectedEntity.list = {
                name: null,
                label: null
            };

            for (var i = 0; i < popupData.cleanEntities.length; i++) {
                if (popupData.cleanEntities[i].name == name) {
                    selectedEntity.name = popupData.cleanEntities[i].name;
                    selectedEntity.label = popupData.cleanEntities[i].label;
                    selectedEntity.labelPlural = popupData.cleanEntities[i].labelPlural;
                    selectedEntity.iconName = popupData.cleanEntities[i].iconName;
                    selectedEntity.weight = popupData.cleanEntities[i].weight;
                    for (var j = 0; j < popupData.cleanEntities[i].recordViews.length; j++) {
                        if (popupData.cleanEntities[i].recordViews[j].default) {
                            selectedEntity.view.name = popupData.cleanEntities[i].recordViews[j].name;
                            selectedEntity.view.label = popupData.cleanEntities[i].recordViews[j].label;
                        }
                    }
                    for (var m  = 0; m < popupData.cleanEntities[i].recordLists.length; m++) {
                        if (popupData.cleanEntities[i].recordLists[m].default) {
                            selectedEntity.list.name = popupData.cleanEntities[i].recordLists[m].name;
                            selectedEntity.list.label = popupData.cleanEntities[i].recordLists[m].label;
                        }
                    }
                }
            }
            //Add to subscribed 
            popupData.subscribedEntities.push(selectedEntity);
            popupData.subscribedEntities = popupData.subscribedEntities.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });
            popupData.pendingEntity = null;
            //Remove from cleanEntities
            var attachedItemIndex = -1;
            for (var i = 0; i < popupData.cleanEntities.length; i++) {
                if (popupData.cleanEntities[i].name == selectedEntity.name) {
                    attachedItemIndex = i;
                    break;
                }
            }
            if (attachedItemIndex != -1) {
                popupData.cleanEntities.splice(attachedItemIndex, 1);
            }
        }


        //Delete subscribed entity
        popupData.deleteSubscription = function (subscription) {
            var unsubscribedEntity = {};
            for (var i = 0; i < popupData.entities.length; i++) {
                if (popupData.entities[i].name == subscription.name) {
                    unsubscribedEntity = popupData.entities[i];
                    break;
                }
            }
            popupData.cleanEntities.push(unsubscribedEntity);
            //Soft alphabetically
            popupData.cleanEntities = popupData.tempEntitiesList.sort(function (a, b) {
                if (a.name < b.name) return -1;
                if (a.na, e > b.name) return 1;
                return 0;
            });
            
            //Remove subscription
            var subscriptionIndex = -1;
            for (var i = 0; i < popupData.subscribedEntities.length; i++) {
                if (popupData.subscribedEntities[i].name == subscription.name) {
                    subscriptionIndex = i;
                    break;
                }
            }
            if (subscriptionIndex != -1) {
                popupData.subscribedEntities.splice(subscriptionIndex, 1);
            }

        }


        /// EXIT functions
        popupData.ok = function () {
            if (!popupData.isUpdate) {
                popupData.area.roles = angular.toJson(popupData.area.roles);
                popupData.area.subscriptions = angular.toJson(popupData.subscribedEntities);
                webvellaAdminService.createRecord("area", popupData.area, successCallback, errorCallback);
            }
            else {
                popupData.area.roles = angular.toJson(popupData.area.roles);
                popupData.area.subscriptions = angular.toJson(popupData.subscribedEntities);
                webvellaAdminService.updateRecord(popupData.area.id, "area", popupData.area, successCallback, errorCallback);
            } 
        };

        popupData.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

        /// Aux
        function successCallback(response) {
            ngToast.create({
                className: 'success',
                content: '<span class="go-green">Success:</span> ' + 'The area was successfully saved'
            });
            $modalInstance.close('success');
            webvellaRootService.GoToState($state,$state.current.name, {});
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
                content: '<span class="go-green">Success:</span> ' + response.message
            });
            $modalInstance.close('success');
            popupData.parentData.modalInstance.close('success');
            webvellaRootService.GoToState($state,$state.current.name, {});
        }

        function errorCallback(response) {
            popupData.hasError = true;
            popupData.errorMessage = response.message;


        }
        $log.debug('webvellaAdmin>entities>createEntityModal> END controller.exec');
    };


})();
