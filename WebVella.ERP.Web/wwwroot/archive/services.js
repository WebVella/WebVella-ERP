'use strict';

///////////////////////////////////////////////////////////////////////////////
ErpApp.service('$ApiService', function ($config, $http) {
	var self = this;

	// Global services for result handling for all methods of this service
	function handleErrorResult(data, status, errorCallBack) {
		switch (status) {
			case 400:
				if (errorCallBack === undefined || typeof (errorCallBack) != "function") {
					alert("The errorCallBack argument is not a function or missing");
					return;
				}
				data.success = false;
				errorCallBack(data);
				break;
			default:
				alert("An API call finished with error: " + status);
				break;
		}
	}

	function handleSuccessResult(data, status, successCallBack, errorCallBack) {
		if (successCallBack === undefined || typeof (successCallBack) != "function") {
			alert("The successCallBack argument is not a function or missing");
			return;
		}

		if (!data.success) {
			//when the validation errors occured
			if (errorCallBack === undefined || typeof (errorCallBack) != "function") {
				alert("The errorCallBack argument in handleSuccessResult is not a function or missing");
				return;
			}
			status = 400;//Bad request
			errorCallBack(data);
		}
		else {
			data.success = true;
			successCallBack(data);
		}
	}

	//Site Meta - returns the general site object with the sitemap and meta info
	this.getSiteMeta = function (successCallBack, errorCallBack) {
	    $http({ method: 'GET', url: $config.apiBaseUrl + 'site/meta' }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallBack, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallBack); });
	};

});

///////////////////////////////////////////////////////////////////////////////
ErpApp.service('$SiteMetaCache', function ($rootScope) {
		//This service purpose is to broadcast the siteMeta to all views, through the rootScope
		var siteMetaCache = {};

		var update = function (newObj) {
			$rootScope.$broadcast('siteMeta-update', newObj);
		}
		return {
			update: update
		};
});