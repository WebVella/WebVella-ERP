
function CurrencyInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config) {
	//Method for generating selector strings of some of the presentation elements
	var selectors = {};
	selectors.viewWrapper = "#view-" + fieldId;
	selectors.editWrapper = "#edit-" + fieldId;
	return selectors;
}

function CurrencyInlineEditPreEnableCallback(fieldId, fieldName, entityName, recordId, config) {
	var selectors = CurrencyInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
	$(selectors.viewWrapper).hide();
	$(selectors.editWrapper).show();
	$(selectors.editWrapper + " .form-control").focus();
}

function CurrencyInlineEditPreDisableCallback(fieldId, fieldName, entityName, recordId, config) {
	var selectors = CurrencyInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
	$(selectors.editWrapper + " .invalid-feedback").remove();
	$(selectors.editWrapper + " .form-control").removeClass("is-invalid");
	$(selectors.editWrapper + " .save .fa").addClass("fa-check").removeClass("fa-spin fa-spinner");
	$(selectors.editWrapper + " .save").attr("disabled", false);
	$(selectors.viewWrapper).show();
	$(selectors.editWrapper).hide();
}

function CurrencyInlineEditInit(fieldId, fieldName, entityName, recordId, config) {
	config = ProcessConfig(config);
	var selectors = CurrencyInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
	//Init enable action click
	$(selectors.viewWrapper + " .action.btn").on("click", function (event) {
		event.stopPropagation();
		event.preventDefault();
		CurrencyInlineEditPreEnableCallback(fieldId, fieldName, entityName, recordId, config);
	});
	//Init enable action dblclick
	$(selectors.viewWrapper + " .form-control").on("dblclick", function (event) {
		event.stopPropagation();
		event.preventDefault();
		CurrencyInlineEditPreEnableCallback(fieldId, fieldName, entityName, recordId, config);
		//clearSelection();//double click causes text to be selected.
		setTimeout(function () {
			$(selectors.editWrapper + " .form-control").get(0).focus();
		}, 200);
	});
	$(selectors.editWrapper + " .form-control").keypress(function (e) {
		if (e.which === 13) {
			$(selectors.editWrapper + " .save").click();
		}
	});
	//Disable inline edit action
	$(selectors.editWrapper + " .cancel").on("click", function (event) {
		event.stopPropagation();
		event.preventDefault();
		CurrencyInlineEditPreDisableCallback(fieldId, fieldName, entityName, recordId, config);
	});
	//Save inline changes
	$(selectors.editWrapper + " .save").on("click", function (event) {
		event.stopPropagation();
		event.preventDefault();
		var inputValue = $(selectors.editWrapper + " .form-control").val();
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
					CurrencyInlineEditInitSuccessCallback(response, fieldId, fieldName, entityName, recordId, config);
				}
				else {
					CurrencyInlineEditInitErrorCallback(response, fieldId, fieldName, entityName, recordId, config);
				}
			},
			error: function (jqXHR, textStatus, errorThrown) {
				var response = {};
				response.message = "";
				if (jqXHR && jqXHR.responseJSON) {
					response = jqXHR.responseJSON;
				}
				CurrencyInlineEditInitErrorCallback(response, fieldId, fieldName, entityName, recordId, config);
			}
		});
	});
}

function CurrencyInlineEditInitSuccessCallback(response, fieldId, fieldName, entityName, recordId, config) {
	var selectors = CurrencyInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
	var newValue = ProcessNewValue(response, fieldName);
	$(selectors.viewWrapper + " .form-control").html(newValue);
	$(selectors.editWrapper + " .form-control").val(newValue);
	CurrencyInlineEditPreDisableCallback(fieldId, fieldName, entityName, recordId, config);
	toastr.success("The new value is successfull saved", 'Success!', { closeButton: true, tapToDismiss: true });
}

function CurrencyInlineEditInitErrorCallback(response, fieldId, fieldName, entityName, recordId, config) {
	var selectors = CurrencyInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
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
