/* application.constants.js */

/**
* @desc the configuration constants for working with API methods
*/

(function () {
	'use strict';
	angular
		.module('wvApp')
		.constant('wvAppConstants', {
			"debugEnabled": true,
			"apiBaseUrl": "/api/v1/en_US/",
			"apiSandboxBaseUrl": "/sandbox/api",
			"locale": "en_US",
			"authTokenKey": "erp-auth"
		});
})();

function checkInt(data) {
	var response = {
		success: true,
		message: "It is integer"
	}
	if (!data) {
		response.message = "Empty value is OK";
		return response;
	}
	if (!isNumeric(data)) {
		response.success = false;
		response.message = "Only integer is accepted";
		return response;
	}

	if (data.toString().indexOf(",") > -1 || data.toString().indexOf(".") > -1) {
		response.success = false;
		response.message = "Only integer is accepted";
		return response;
	}

	if (data == parseInt(data, 10)) {
		return response;
	}
	else {
		response.success = false;
		response.message = "Only integer is accepted";
		return response;
	}

}


function checkDecimal(data) {
	var response = {
		success: true,
		message: "It is decimal"
	}
	if (!data) {
		response.message = "Empty value is OK";
		return response;
	}
	if (data.toString().indexOf(",") > -1) {
		response.success = false;
		response.message = "Comma is not allowed. Use '.' for decimal separator";
		return response;
	}

	if (!isNumeric(data)) {
		response.success = false;
		response.message = "Only decimal is accepted";
		return response;
	}

	return response;
}

function isNumeric(n) {
	return !isNaN(parseFloat(n)) && isFinite(n);
}

function decimalPlaces(num) {
	var match = ('' + num).match(/(?:\.(\d+))?(?:[eE]([+-]?\d+))?$/);
	if (!match) { return 0; }
	return Math.max(
		 0,
		 // Number of digits right of decimal point.
		 (match[1] ? match[1].length : 0)
		 // Adjust for scientific notation.
		 - (match[2] ? +match[2] : 0));
}

//Only insite a scope

//$scope.Math = window.Math;
//function multiplyDecimals(val1, val2, decimalPlaces) {
//    var helpNumber = 100;
//    for (var i = 0; i < decimalPlaces; i++) {
//        helpNumber = helpNumber * 10;
//    }
//    var temp1 = $scope.Math.round(val1 * helpNumber);
//    var temp2 = $scope.Math.round(val2 * helpNumber);
//    return (temp1 * temp2) / (helpNumber*helpNumber);
//}


function checkPercent(data) {
	var response = {
		success: true,
		message: "It is decimal"
	}
	if (!data) {
		response.message = "Empty value is OK";
		return response;
	}
	if (data.toString().indexOf(",") > -1) {
		response.success = false;
		response.message = "Comma is not allowed. Use '.' for decimal separator";
		return response;
	}
	if (!isNumeric(data)) {
		response.success = false;
		response.message = "Only decimal is accepted";
		return response;
	}

	if (data > 1) {
		response.success = false;
		response.message = "Only decimal values between 0 and 1 are accepted";
		return response;
	}

	return response;
}

function checkPhone(data) {
	var response = {
		success: true,
		message: "It is decimal"
	}
	if (!phoneUtils.isValidNumber(data)) {
		response.success = false,
		response.message = "Not a valid phone. Should start with + followed by the country code digits";
		return response;
	}


	return response;
}


function checkEmail(data) {
	var response = {
		success: true,
		message: "It is email"
	}
	if (!data) {
		response.message = "Empty value is OK";
		return response;
	}
	var regex = new RegExp("[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
	if (!regex.test(data.toString())) {
		response.success = false;
		response.message = "Invalid email format";
		return response;
	}


	return response;
}

function getFontAwesomeIconNames() {
	//Extracting the font-awesome icon names from rawJSON
	//1. Get the raw json from the amazing work here https://github.com/Smartik89/SMK-Font-Awesome-PHP-JSON/blob/master/font-awesome/json/font-awesome-data.json

	//2. Paste the raw json in the object below
	//pluginData.faRaw = {}

	//3. Execute the following script
	//var iconNames = [];
	//for (var name in pluginData.faRaw) {
	//	iconNames.push(name);
	//}
	//$log.info(iconNames);

	//4. Copy the json from the console following this steps
	////// 1 - Right-click the object and select "Store as global variable"
	////// 2 - The console will print the new variable's name, for example: temp1
	////// 3 - Type: copy(temp1)  
	////// The object is now available in your clipboard.
	////// Tested in Chrome

	//5. Remove the "" to get the icon name


	var iconNames = [
	  "500px",
	  "adjust",
	  "adn",
	  "align-center",
	  "align-justify",
	  "align-left",
	  "align-right",
	  "amazon",
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
	  "balance-scale",
	  "ban",
	  "bar-chart",
	  "barcode",
	  "bars",
	  "battery-empty",
	  "battery-full",
	  "battery-half",
	  "battery-quarter",
	  "battery-three-quarters",
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
	  "black-tie",
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
	  "calendar-check-o",
	  "calendar-minus-o",
	  "calendar-o",
	  "calendar-plus-o",
	  "calendar-times-o",
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
	  "cc-diners-club",
	  "cc-discover",
	  "cc-jcb",
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
	  "chrome",
	  "circle",
	  "circle-o",
	  "circle-o-notch",
	  "circle-thin",
	  "clipboard",
	  "clock-o",
	  "clone",
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
	  "commenting",
	  "commenting-o",
	  "comments",
	  "comments-o",
	  "compass",
	  "compress",
	  "connectdevelop",
	  "contao",
	  "copyright",
	  "creative-commons",
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
	  "expeditedssl",
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
	  "firefox",
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
	  "fonticons",
	  "forumbee",
	  "forward",
	  "foursquare",
	  "frown-o",
	  "futbol-o",
	  "gamepad",
	  "gavel",
	  "gbp",
	  "genderless",
	  "get-pocket",
	  "gg",
	  "gg-circle",
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
	  "hand-lizard-o",
	  "hand-o-down",
	  "hand-o-left",
	  "hand-o-right",
	  "hand-o-up",
	  "hand-paper-o",
	  "hand-peace-o",
	  "hand-pointer-o",
	  "hand-rock-o",
	  "hand-scissors-o",
	  "hand-spock-o",
	  "hdd-o",
	  "header",
	  "headphones",
	  "heart",
	  "heart-o",
	  "heartbeat",
	  "history",
	  "home",
	  "hospital-o",
	  "hourglass",
	  "hourglass-end",
	  "hourglass-half",
	  "hourglass-o",
	  "hourglass-start",
	  "houzz",
	  "html5",
	  "i-cursor",
	  "ils",
	  "inbox",
	  "indent",
	  "industry",
	  "info",
	  "info-circle",
	  "inr",
	  "instagram",
	  "internet-explorer",
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
	  "map",
	  "map-marker",
	  "map-o",
	  "map-pin",
	  "map-signs",
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
	  "mouse-pointer",
	  "music",
	  "neuter",
	  "newspaper-o",
	  "object-group",
	  "object-ungroup",
	  "odnoklassniki",
	  "odnoklassniki-square",
	  "opencart",
	  "openid",
	  "opera",
	  "optin-monster",
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
	  "registered",
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
	  "safari",
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
	  "sticky-note",
	  "sticky-note-o",
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
	  "television",
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
	  "trademark",
	  "train",
	  "transgender",
	  "transgender-alt",
	  "trash",
	  "trash-o",
	  "tree",
	  "trello",
	  "tripadvisor",
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
	  "vimeo",
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
	  "wikipedia-w",
	  "windows",
	  "wordpress",
	  "wrench",
	  "xing",
	  "xing-square",
	  "y-combinator",
	  "yahoo",
	  "yelp",
	  "youtube",
	  "youtube-play",
	  "youtube-square"
	]
	return iconNames;
}

// Later can be used in controller as a dependency