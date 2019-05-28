
function IconFieldInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config) {
	//Method for generating selector strings of some of the presentation elements
	var selectors = {};
	selectors.viewWrapper = "#view-" + fieldId;
	selectors.editWrapper = "#edit-" + fieldId;
	selectors.inputEl = "#input-" + fieldId;
	selectors.viewOptionsListUl = selectors.viewWrapper + " .select2-selection__rendered";
	selectors.editWrapper = "#edit-" + fieldId;
	return selectors;
}

function IconFieldInlineEditFormat(icon) {
	var originalOption = icon.element;
	var iconClass = $(originalOption).data('icon');
	var color = $(originalOption).data('color');
	if (!color) {
		color = "#999";
	}
	if (!iconClass) {
		return icon.text;
	}
	return '<i class="fa ' + iconClass + '" style="color:' + color + '"></i> ' + icon.text;
}

function IconFieldInlineEditPreEnableCallback(fieldId, fieldName, entityName, recordId, config) {
	config = ProcessConfig(config);
	var selectors = IconFieldInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);

	var selectInitObject = {
		ajax: {
			url: '/api/v3.0/p/core/select/font-awesome-icons',
			data: function (params) {
				var query = {
					search: params.term,
					page: params.page || 1
				};
				return query;
			},
			dataType: 'json',
			processResults: function (data) {
				// Tranforms the top-level key of the response object from 'items' to 'results'
				var results = [];
				if(data.object.results){
					_.forEach(data.object.results,function(rec){
						results.push({id:rec.class,text:rec.class,name:rec.name});
					});
				}

				data.object.results = results;
				return data.object;
			}
		},
		//language: "bg",
		placeholder: 'not-selected',
		allowClear: !$(selectors.inputControl).prop('required'),
		closeOnSelect: true,
		width: 'element',
		escapeMarkup: function (markup) {
			return markup;
		},
		templateResult: function (state) {
			var $state = $(
				'<div class="erp-ta-icon-result"><div class="icon-wrapper"><i class="icon fa-fw ' + state.id + '"/></div><div class="meta"><div class="title">' + state.id + '</div><div class="entity go-gray">' + state.name + '</div></div>'
			);
			return $state;
		}
	};

	$(selectors.inputEl).select2(selectInitObject);
	if (config.is_invalid) {
		$(selectors.inputEl).closest(".input-group").find(".select2-selection").addClass("is-invalid");
	}
	$(selectors.viewWrapper).hide();
	$(selectors.editWrapper).show();
	$(selectors.inputEl).focus();
}

function IconFieldInlineEditPreDisableCallback(fieldId, fieldName, entityName, recordId, config) {
	var selectors = IconFieldInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
	$(selectors.editWrapper + " .invalid-feedback").remove();
	$(selectors.editWrapper + " .form-control").removeClass("is-invalid");
	$(selectors.editWrapper + " .save .fa").addClass("fa-check").removeClass("fa-spin fa-spinner");
	$(selectors.editWrapper + " .save").attr("disabled", false);
	var originalValue = $(selectors.inputEl).attr("data-original-value");
	originalValue = ProcessConfig(originalValue);
	$(selectors.inputEl).val(originalValue);
	$(selectors.inputEl).select2('destroy');
	$(selectors.viewWrapper).show();
	$(selectors.editWrapper).hide();
}

function IconFieldInlineEditInit(fieldId, fieldName, entityName, recordId, config) {
	config = ProcessConfig(config);
	var selectors = IconFieldInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
	//Init enable action click
	$(selectors.viewWrapper + " .action.btn").on("click", function (event) {
		event.stopPropagation();
		event.preventDefault();
		IconFieldInlineEditPreEnableCallback(fieldId, fieldName, entityName, recordId, config);
	});
	//Init enable action dblclick
	$(selectors.viewWrapper + " .form-control").on("dblclick", function (event) {
		event.stopPropagation();
		event.preventDefault();
		IconFieldInlineEditPreEnableCallback(fieldId, fieldName, entityName, recordId, config);
		//clearSelection();//double click causes text to be selected.
		setTimeout(function () {
			$(selectors.editWrapper + " .form-control").get(0).focus();
		}, 200);
	});
	//Disable inline edit action
	$(selectors.editWrapper + " .cancel").on("click", function (event) {
		event.stopPropagation();
		event.preventDefault();
		IconFieldInlineEditPreDisableCallback(fieldId, fieldName, entityName, recordId, config);
	});
	//Save inline changes
	$(selectors.editWrapper + " .save").on("click", function (event) {
		event.stopPropagation();
		event.preventDefault();
		var inputValue = $(selectors.inputEl).val();
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
					IconFieldInlineEditInitSuccessCallback(response, fieldId, fieldName, entityName, recordId,inputValue, config);
				}
				else {
					IconFieldInlineEditInitErrorCallback(response, fieldId, fieldName, entityName, recordId, config);
				}
			},
			error: function (jqXHR, textStatus, errorThrown) {
				var response = {};
				response.message = "";
				if (jqXHR && jqXHR.responseJSON) {
					response = jqXHR.responseJSON;
				}
				IconFieldInlineEditInitErrorCallback(response, fieldId, fieldName, entityName, recordId, config);
			}
		});
	});
}

function IconFieldInlineEditInitSuccessCallback(response, fieldId, fieldName, entityName, recordId,inputValue, config) {
	var selectors = IconFieldInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
	var newValue = inputValue;

	if (!fieldName.startsWith("$")) {
		newValue = ProcessNewValue(response, fieldName);
	}

	var selectOptions = $(selectors.inputEl + ' option');
	var matchedOption = _.find(selectOptions, function (record) {
		if (!newValue && !record.attributes["value"].value) {
			return true;
		}
		else {
			return newValue === record.attributes["value"].value;
		}
	});
	var optionLabel = matchedOption.text;

	var iconClass = $(matchedOption).data('icon');
	var color = $(matchedOption).data('color');
	if (!color) {
		color = "#999";
	}
	if (!iconClass) {
		$(selectors.viewWrapper + " .form-control").html(optionLabel);
	}
	else {
		$(selectors.viewWrapper + " .form-control").html('<i class="fa ' + iconClass + '" style="color:' + color + '"></i>  ' + optionLabel);
	}

	$(selectors.inputEl).val(newValue).attr("data-original-value", JSON.stringify(newValue));
	IconFieldInlineEditPreDisableCallback(fieldId, fieldName, entityName, recordId, config);
	toastr.success("The new value is successful saved", 'Success!', { closeButton: true, tapToDismiss: true });
}

function IconFieldInlineEditInitErrorCallback(response, fieldId, fieldName, entityName, recordId, config) {
	var selectors = IconFieldInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
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


