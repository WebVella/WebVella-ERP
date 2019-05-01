
function SelectInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config) {
	//Method for generating selector strings of some of the presentation elements
	var selectors = {};
	selectors.viewWrapper = "#view-" + fieldId;
	selectors.editWrapper = "#edit-" + fieldId;
	selectors.inputEl = "#input-" + fieldId;
	selectors.viewOptionsListUl = selectors.viewWrapper + " .select2-selection__rendered";
	selectors.editWrapper = "#edit-" + fieldId;
	return selectors;
}

function SelectInlineEditFormat(icon) {
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

function SelectInlineEditPreEnableCallback(fieldId, fieldName, entityName, recordId, config) {
	config = ProcessConfig(config);
	var selectors = SelectInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);

	var selectInitObject = {
		closeOnSelect: true,
		language: "en",
		minimumResultsForSearch: 10,
		placeholder: 'not selected',
		allowClear: !$(selectors.inputEl).prop('required'),
		width: 'element',
		escapeMarkup: function (markup) {
			return markup;
		},
		templateResult: SelectInlineEditFormat,
		templateSelection: SelectInlineEditFormat
	};

	if (config.ajax_datasource) {
		var currentPage = 1;
		selectInitObject.ajax = {
			type: 'POST',
			headers: {
				'Accept': 'application/json',
				'Content-Type': 'application/json'
			},
			url: '/api/v3/en_US/eql-ds',
			data: function (params) {
				var query = {
					name: config.ajax_datasource.ds,
					parameters: [
						{
							name: "term",
							value: params.term || ""
						},
						{
							name: "page",
							value: params.page || 1
						}
					]
				};
				currentPage = params.page;
				return JSON.stringify(query);
			},
			processResults: function (data) {
				var results = [];
				var hasMore = false;
				var totalRecords = data.object.total_count;
				var displayedCount = data.object.list.length + currentPage * config.ajax_datasource.page_size;
				if (displayedCount < totalRecords) {
					hasMore = true;
				}
				_.forEach(data.object.list, function (record) {
					results.push({
						id: record[config.ajax_datasource.value],
						text: record[config.ajax_datasource.label]
					});
				});
				return {
					results: results, //id,text
					pagination: {
						more: hasMore
					}
				};
			}
		};
	}

	$(selectors.inputEl).select2(selectInitObject);
	if (config.is_invalid) {
		$(selectors.inputEl).closest(".input-group").find(".select2-selection").addClass("is-invalid");
	}
	$(selectors.viewWrapper).hide();
	$(selectors.editWrapper).show();
	$(selectors.inputEl).focus();
}

function SelectInlineEditPreDisableCallback(fieldId, fieldName, entityName, recordId, config) {
	var selectors = SelectInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
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

function SelectInlineEditInit(fieldId, fieldName, entityName, recordId, config) {
	config = ProcessConfig(config);
	var selectors = SelectInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
	//Init enable action click
	$(selectors.viewWrapper + " .action.btn").on("click", function (event) {
		event.stopPropagation();
		event.preventDefault();
		SelectInlineEditPreEnableCallback(fieldId, fieldName, entityName, recordId, config);
	});
	//Init enable action dblclick
	$(selectors.viewWrapper + " .form-control").on("dblclick", function (event) {
		event.stopPropagation();
		event.preventDefault();
		SelectInlineEditPreEnableCallback(fieldId, fieldName, entityName, recordId, config);
		//clearSelection();//double click causes text to be selected.
		setTimeout(function () {
			$(selectors.editWrapper + " .form-control").get(0).focus();
		}, 200);
	});
	//Disable inline edit action
	$(selectors.editWrapper + " .cancel").on("click", function (event) {
		event.stopPropagation();
		event.preventDefault();
		SelectInlineEditPreDisableCallback(fieldId, fieldName, entityName, recordId, config);
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
					SelectInlineEditInitSuccessCallback(response, fieldId, fieldName, entityName, recordId,inputValue, config);
				}
				else {
					SelectInlineEditInitErrorCallback(response, fieldId, fieldName, entityName, recordId, config);
				}
			},
			error: function (jqXHR, textStatus, errorThrown) {
				var response = {};
				response.message = "";
				if (jqXHR && jqXHR.responseJSON) {
					response = jqXHR.responseJSON;
				}
				SelectInlineEditInitErrorCallback(response, fieldId, fieldName, entityName, recordId, config);
			}
		});
	});
}

function SelectInlineEditInitSuccessCallback(response, fieldId, fieldName, entityName, recordId,inputValue, config) {
	var selectors = SelectInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
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
	SelectInlineEditPreDisableCallback(fieldId, fieldName, entityName, recordId, config);
	toastr.success("The new value is successful saved", 'Success!', { closeButton: true, tapToDismiss: true });
}

function SelectInlineEditInitErrorCallback(response, fieldId, fieldName, entityName, recordId, config) {
	var selectors = SelectInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
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


