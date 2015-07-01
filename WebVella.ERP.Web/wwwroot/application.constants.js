/* application.constants.js */

/**
* @desc the configuration constants for working with API methods
*/

(function () {
    'use strict';
    angular
        .module('wvApp')
        .constant('wvAppConstants', {
            "debugEnabled": false,
            "apiBaseUrl": "/api/v1/en_US/",
            "apiSandboxBaseUrl": "/sandbox/api",
            "locale": "en_US"
        });
})();

function checkInt(data) {
	var response = {
		success: true,
		message: "It is integer"
	}

	if (!isNaN(parseFloat(data)) && !isFinite(data)) {
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
	if (data.toString().indexOf(",") > -1) {
		response.success = false;
		response.message = "Comma is not allowed. Use '.' for decimal separator";
		return response;
	}
	if (!isNaN(parseFloat(data)) && !isFinite(data)) {
		response.success = false;
		response.message = "Only decimal is accepted";
		return response;
	}

	return response;
}

// Later can be used in controller as a dependency