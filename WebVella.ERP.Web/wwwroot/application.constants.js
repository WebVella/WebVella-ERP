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
	if (!data) {
		response.message = "Empty value is OK";
		return response;
	}
	if (!isNaN(parseFloat(data)) && !isFinite(data)) {
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
	if (isNaN(parseFloat(data)) && !isFinite(data)) {
		response.success = false;
		response.message = "Only decimal is accepted";
		return response;
	}

	return response;
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
// Later can be used in controller as a dependency