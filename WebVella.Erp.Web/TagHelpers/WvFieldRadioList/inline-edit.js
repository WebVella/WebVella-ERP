function RadioListInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config) {
	//Method for generating selector strings of some of the presentation elements
	var selectors = {};
	selectors.viewWrapper = "#view-" + fieldId;
	selectors.editWrapper = "#edit-" + fieldId;
	return selectors;
}

function RadioListInlineEditPreEnableCallback(fieldId, fieldName, entityName, recordId, config) {
	var selectors = RadioListInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
	$(selectors.viewWrapper).hide();
	$(selectors.editWrapper).show();
}

function RadioListInlineEditPreDisableCallback(fieldId, fieldName, entityName, recordId, config) {
	var selectors = RadioListInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
	$(selectors.editWrapper + " .invalid-feedback").remove();
	$(selectors.editWrapper + " .form-control").removeClass("is-invalid");
	$(selectors.editWrapper + " .save .fa").addClass("fa-check").removeClass("fa-spin fa-spinner");
	$(selectors.editWrapper + " .save").attr("disabled", false);
	$(selectors.viewWrapper).show();
	$(selectors.editWrapper).hide();
}

function RadioListInlineEditInit(fieldId, fieldName, entityName, recordId, config) {
	config = ProcessConfig(config);
	var selectors = RadioListInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
	//Init enable action click
	$(selectors.viewWrapper + " .action.btn").on("click", function (event) {
		event.stopPropagation();
		event.preventDefault();
		RadioListInlineEditPreEnableCallback(fieldId, fieldName, entityName, recordId, config);
	});
	//Init enable action dblclick
	$(selectors.viewWrapper + " .form-control").on("dblclick", function (event) {
		event.stopPropagation();
		event.preventDefault();
		RadioListInlineEditPreEnableCallback(fieldId, fieldName, entityName, recordId, config);

	});
	//Disable inline edit action
	$(selectors.editWrapper + " .cancel").on("click", function (event) {
		event.stopPropagation();
		event.preventDefault();
		RadioListInlineEditPreDisableCallback(fieldId, fieldName, entityName, recordId, config);
	});

	//Save inline changes
	$(selectors.editWrapper + " .save").on("click", function (event) {
		event.stopPropagation();
		event.preventDefault();
		var inputValueArray = [];
		_.forEach($(selectors.editWrapper + " .form-check-input"), function (element) {
			if ($(element).is(':checked')) {
				inputValueArray.push($(element).val());
			}
		});

		var submitObj = {};
		submitObj.id = recordId;
		submitObj[fieldName] = _.join(inputValueArray,',');
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
					RadioListInlineEditInitSuccessCallback(response, fieldId, fieldName, entityName, recordId, config);
				}
				else {
					RadioListInlineEditInitErrorCallback(response, fieldId, fieldName, entityName, recordId, config);
				}
			},
			error: function (jqXHR, textStatus, errorThrown) {
				var response = {};
				response.message = "";
				if (jqXHR && jqXHR.responseJSON) {
					response = jqXHR.responseJSON;
				}
				RadioListInlineEditInitErrorCallback(response, fieldId, fieldName, entityName, recordId, config);
			}
		});
	});
}

function RadioListInlineEditInitSuccessCallback(response, fieldId, fieldName, entityName, recordId, config) {
	var selectors = RadioListInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
	var newValue = ProcessNewValue(response, fieldName);
	if (newValue) {
		$(selectors.viewWrapper + " .input-group-prepend .fa").removeClass("fa-check fa-question fa-times").addClass("fa-check");
		$(selectors.viewWrapper + " .form-control").html(newValue);
		var valueArray = _.split(newValue,',');
		_.forEach($("#edit-" + fieldId + " .form-check-input"), function (element) {
			var elValue = $(element).val();
			var valueIndex = _.findIndex(valueArray, function (record) {return record === elValue;});
			if (valueIndex > -1) {
				$(element).prop('checked', true);
			}
			else {
				$(element).prop('checked', false);
			}
		});
	}
	else {
		$(selectors.viewWrapper + " .input-group-prepend .fa").removeClass("fa-check fa-question fa-times").addClass("fa-times");
		$(selectors.viewWrapper + " .form-control").html(config.checkbox_false_label);
		$(selectors.viewWrapper + " .form-control").html("");
		_.forEach($("#edit-" + fieldId + " .form-check-input"), function (element) {
			$(element).prop('checked', false);
		});
	}
	RadioListInlineEditPreDisableCallback(fieldId, fieldName, entityName, recordId, config);
	toastr.success("The new value is successfull saved", 'Success!', { closeButton: true, tapToDismiss: true });
}

function RadioListInlineEditInitErrorCallback(response, fieldId, fieldName, entityName, recordId, config) {
	var selectors = RadioListInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
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
