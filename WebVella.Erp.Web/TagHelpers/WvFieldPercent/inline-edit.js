function PercentInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config) {
	//Method for generating selector strings of some of the presentation elements
	var selectors = {};
	selectors.viewWrapper = "#view-" + fieldId;
	selectors.editWrapper = "#edit-" + fieldId;
	selectors.viewInputEl = selectors.viewWrapper + " .form-control";
	selectors.fakeInput = selectors.editWrapper + " #fake-" + fieldId;
	selectors.hiddenInput = selectors.editWrapper + " #input-" + fieldId;
	selectors.viewConvertedValueEl = selectors.viewWrapper + " .input-converted";
	selectors.editConvertedValueEl = selectors.editWrapper + " .input-converted";
	return selectors;
}

function SetHiddenPercent(fieldId, fieldName, entityName, recordId, config) {
	var selectors = PercentInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
	var decimalDigits = 4;
	if (config.decimal_digits) {
		decimalDigits = config.decimal_digits + 2;
	}
	var value = $(selectors.fakeInput).val();
	if (!value || value === null) {
		$(selectors.hiddenInput).val(null);
	}
	else {
		var valDec = new Decimal(value);
		var hundDec = new Decimal(100);
		var percentDec = valDec.dividedBy(hundDec);
		var roundedDec = percentDec.toDecimalPlaces(decimalDigits);
		$(selectors.hiddenInput).val(roundedDec.toString());
	}
}

function PercentInlineEditPreEnableCallback(fieldId, fieldName, entityName, recordId, config) {
	var selectors = PercentInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
	$(selectors.fakeInput).on("change paste keyup", function () {
		SetHiddenPercent(fieldId, fieldName, entityName, recordId, config);
	});
	$(selectors.viewWrapper).hide();
	$(selectors.editWrapper).show();
	$(selectors.editWrapper + " .form-control").focus();
}

function PercentInlineEditPreDisableCallback(fieldId, fieldName, entityName, recordId, config) {
	var selectors = PercentInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
	$(selectors.editWrapper + " .invalid-feedback").remove();
	$(selectors.editWrapper + " .form-control").removeClass("is-invalid");
	$(selectors.editWrapper + " .save .fa").addClass("fa-check").removeClass("fa-spin fa-spinner");
	$(selectors.editWrapper + " .save").attr("disabled", false);
	var originalValue = $(selectors.hiddenInput).attr("data-original-value");
	var newValueDec = new Decimal(originalValue);
	var decimalDigits = 2;
	if (config.decimal_digits) {
		decimalDigits = config.decimal_digits;
	}
	//Set View Wrapper
	var newViewValue = newValueDec.times(new Decimal(100)).toDecimalPlaces(decimalDigits);
	$(selectors.fakeInput).val(newViewValue.toNumber());
	$(selectors.fakeInput).off('change paste keyup');
	$(selectors.viewWrapper).show();
	$(selectors.editWrapper).hide();
}

function PercentInlineEditInit(fieldId, fieldName, entityName, recordId, config) {
	config = ProcessConfig(config);
	var selectors = PercentInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
	//Init enable action click
	$(selectors.viewWrapper + " .action.btn").on("click", function (event) {
		event.stopPropagation();
		event.preventDefault();
		PercentInlineEditPreEnableCallback(fieldId, fieldName, entityName, recordId, config);
	});
	//Init enable action dblclick
	$(selectors.viewWrapper + " .form-control").on("dblclick", function (event) {
		event.stopPropagation();
		event.preventDefault();
		PercentInlineEditPreEnableCallback(fieldId, fieldName, entityName, recordId, config);
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
		PercentInlineEditPreDisableCallback(fieldId, fieldName, entityName, recordId, config);
	});
	//Save inline changes
	$(selectors.editWrapper + " .save").on("click", function (event) {
		event.stopPropagation();
		event.preventDefault();
		var inputValue = $(selectors.hiddenInput).val();
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
					PercentInlineEditInitSuccessCallback(response, fieldId, fieldName, entityName, recordId, config);
				}
				else {
					PercentInlineEditInitErrorCallback(response, fieldId, fieldName, entityName, recordId, config);
				}
			},
			error: function (jqXHR, textStatus, errorThrown) {
				var response = {};
				response.message = "";
				if (jqXHR && jqXHR.responseJSON) {
					response = jqXHR.responseJSON;
				}
				PercentInlineEditInitErrorCallback(response, fieldId, fieldName, entityName, recordId, config);
			}
		});
	});
}

function PercentInlineEditInitSuccessCallback(response, fieldId, fieldName, entityName, recordId, config) {
	var selectors = PercentInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
	var newValue = ProcessNewValue(response, fieldName);
	var newValueDec = new Decimal(newValue);
	
	var decimalDigits = 2;
	if (config.decimal_digits) {
		decimalDigits = config.decimal_digits;
	}

	//Set View Wrapper
	var newViewValue = newValueDec.times(new Decimal(100)).toDecimalPlaces(decimalDigits);
	$(selectors.viewInputEl).html(newViewValue.toString());

	//Set Edit Wrapper
	//Set Fake
	//Fake will be set in PercentInlineEditPreDisableCallback

	//Set Hidden Input attributes
	$(selectors.hiddenInput).val(newValue);	
	$(selectors.hiddenInput).attr("data-original-value", newValue);

	PercentInlineEditPreDisableCallback(fieldId, fieldName, entityName, recordId, config);
	toastr.success("The new value is successfull saved", 'Success!', { closeButton: true, tapToDismiss: true });
}

function PercentInlineEditInitErrorCallback(response, fieldId, fieldName, entityName, recordId, config) {
	var selectors = PercentInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
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

