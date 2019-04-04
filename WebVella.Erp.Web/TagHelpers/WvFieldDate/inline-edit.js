
//JS DateTime picker init method
var datePickerDictionary = {};
var flatPickrServerDateFormat = "Y-m-dTH:i:S";
var flatPickrUiDateFormat = "d M Y";
//From the server dates will be received yyyy-MM-ddTHH:mm:ss.fff
function InitFlatPickrDateInlineEdit(editWrapperSelector) {
    var defaultDate = $(editWrapperSelector).attr("data-default-date");
    if (defaultDate === "") {
        defaultDate = null;
    }
	var options = { 
		time_24hr: true,
		defaultDate: defaultDate,
        dateFormat: flatPickrServerDateFormat,
        "static": true,
		//locale: BulgarianDateTimeLocale,
		altInput: true,
		altFormat: flatPickrUiDateFormat };
	flatpickr(editWrapperSelector + " .form-control", options);
}

function DateInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config) {
	//Method for generating selector strings of some of the presentation elements
	var selectors = {};
	selectors.viewWrapper = "#view-" + fieldId;
	selectors.editWrapper = "#edit-" + fieldId;
	return selectors;
}

function DateInlineEditPreEnableCallback(fieldId, fieldName, entityName, recordId, config) {
	var selectors = DateInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
	//init pickr only when needed
	InitFlatPickrDateInlineEdit(selectors.editWrapper, "date");
	$(selectors.viewWrapper).hide();
	$(selectors.editWrapper).show();
	$(selectors.editWrapper + " .form-control").focus();
}

function DateInlineEditPreDisableCallback(fieldId, fieldName, entityName, recordId, config) {
	var selectors = DateInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
	$(selectors.editWrapper + " .invalid-feedback").remove();
	$(selectors.editWrapper + " .form-control").removeClass("is-invalid");
	$(selectors.editWrapper + " .save .fa").addClass("fa-check").removeClass("fa-spin fa-spinner");
	$(selectors.editWrapper + " .save").attr("disabled", false);
	$(selectors.viewWrapper).show();
	$(selectors.editWrapper).hide();
	var calendarInstance = document.querySelector(selectors.editWrapper + " .form-control")._flatpickr;
	calendarInstance.destroy();
}

function DateInlineEditInit(fieldId, fieldName, entityName, recordId, config) {
	config = ProcessConfig(config);
	var selectors = DateInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
	//Init enable action click
	$(selectors.viewWrapper + " .action.btn").on("click", function (event) {
		event.stopPropagation();
		event.preventDefault();
		DateInlineEditPreEnableCallback(fieldId, fieldName, entityName, recordId, config);
	});
	//Init enable action dblclick
	$(selectors.viewWrapper + " .form-control").on("dblclick", function (event) {
		event.stopPropagation();
		event.preventDefault();
		DateInlineEditPreEnableCallback(fieldId, fieldName, entityName, recordId, config);
		//clearSelection();//double click causes text to be selected.
		setTimeout(function () {
			$(selectors.editWrapper + " .form-control").get(0).focus();
		}, 200);
	});
	//Disable inline edit action
	$(selectors.editWrapper + " .cancel").on("click", function (event) {
		event.stopPropagation();
		event.preventDefault();
		DateInlineEditPreDisableCallback(fieldId, fieldName, entityName, recordId, config);
	});
	//Save inline changes
	$(selectors.editWrapper + " .save").on("click", function (event) {
		event.stopPropagation();
		event.preventDefault();
		var inputValue = $(selectors.editWrapper + " .form-control").val();
		if (!inputValue) {
			inputValue = null;
		}
		else if (!moment(inputValue).isValid()) {
			toastr.error("invalid date", 'Error!', { closeButton: true, tapToDismiss: true });
			return;
		}

        if (inputValue === "") {
            inputValue = null;
        }

		var submitObj = {};
		submitObj.id = recordId;
		submitObj[fieldName] = inputValue;
		$(selectors.editWrapper + " .save .fa").removeClass("fa-check").addClass("fa-spin fa-spinner");
		$(selectors.editWrapper + " .save").attr("disabled", true);
		$(selectors.editWrapper + " .invalid-feedback").remove();
		$(selectors.editWrapper + " .form-control").removeClass("is-invalid");
		var apiUrl = ApiBaseUrl + "/record/" + entityName + "/" + recordId;
		if (config.api_url) {
			apiUrl = config.api_url;
		}
		$.ajax({
			headers: {
				'Accept': 'application/json',
				'Content-Type': 'application/json'
			},
			url: apiUrl,
			type: 'PATCH',
			data: JSON.stringify(submitObj),
			success: function (response) {
				if (response.success) {
					DateInlineEditInitSuccessCallback(response, fieldId, fieldName, entityName, recordId, config);
				}
				else {
					DateInlineEditInitErrorCallback(response, fieldId, fieldName, entityName, recordId, config);
				}
			},
			error: function (jqXHR, textStatus, errorThrown) {
				var response = {};
				response.message = "";
				if (jqXHR && jqXHR.responseJSON) {
					response = jqXHR.responseJSON;
				}
				DateInlineEditInitErrorCallback(response, fieldId, fieldName, entityName, recordId, config);
			}
		});
	});
}

function DateInlineEditInitSuccessCallback(response, fieldId, fieldName, entityName, recordId, config) {
	var selectors = DateInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
	var newValue = ProcessNewValue(response, fieldName);
	if (newValue !== null) {
		var formatedDate = moment(newValue).format("DD MMM YYYY");
		$(selectors.viewWrapper + " .form-control").html(formatedDate);
	}
	else {
		$(selectors.viewWrapper + " .form-control").html(newValue);
	}
	$(selectors.viewWrapper + " .form-control").attr("title", newValue);
	$(selectors.editWrapper + " .form-control").val(newValue);
	$(selectors.editWrapper).attr("data-default-date",newValue);
	DateInlineEditPreDisableCallback(fieldId, fieldName, entityName, recordId, config);
	toastr.success("The new value is successfull saved", 'Success!', { closeButton: true, tapToDismiss: true });
}

function DateInlineEditInitErrorCallback(response, fieldId, fieldName, entityName, recordId, config) {
	var selectors = DateInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
	$(selectors.editWrapper + " .form-control").addClass("is-invalid");
	var errorMessage = response.message;
	if (!errorMessage && response.errors && response.errors.length > 0) {
		errorMessage = response.errors[0].message;
	}
		
	$(selectors.editWrapper + " .input-group").after("<div class='invalid-feedback'>" + errorMessage + "</div>");
	$(selectors.editWrapper + " .invalid-feedback").show();
	$(selectors.editWrapper + " .save .fa").addClass("fa-check").removeClass("fa-spin fa-spinner");
	$(selectors.editWrapper + " .save").attr("disabled", false);
	toastr.error("An error occurred", 'Error!', { closeButton: true, tapToDismiss: true });
	console.log("error", response);
}