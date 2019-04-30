
function MultiSelectInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config) {
	//Method for generating selector strings of some of the presentation elements
	var selectors = {};
	selectors.viewWrapper = "#view-" + fieldId;
	selectors.editWrapper = "#edit-" + fieldId;
	selectors.inputEl = "#input-" + fieldId;
	selectors.viewOptionsListUl = selectors.viewWrapper + " .select2-selection__rendered";
	selectors.editWrapper = "#edit-" + fieldId;
	return selectors;
}

function MultiSelectInlineEditPreEnableCallback(fieldId, fieldName, entityName, recordId, config) {
	var selectors = MultiSelectInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);

	var selectInitObject = {
		closeOnSelect: true,
		language: "en",
		minimumResultsForSearch: 10,
		width: 'element'
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
	//Stops remove selection click opening the dropdown
	$(selectors.inputEl).on("select2:unselect", function (evt) {
		if (!evt.params.originalEvent) {
			return;
		}

		evt.params.originalEvent.stopPropagation();
	});
	$(selectors.viewWrapper).hide();
	$(selectors.editWrapper).show();
	$(selectors.inputEl).focus();
}

function MultiSelectInlineEditPreDisableCallback(fieldId, fieldName, entityName, recordId, config) {
	var selectors = MultiSelectInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
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

function MultiSelectInlineEditInit(fieldId, fieldName, entityName, recordId, config) {
	config = ProcessConfig(config);
	var selectors = MultiSelectInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
	//Init enable action click
	$(selectors.viewWrapper + " .action.btn").on("click", function (event) {
		event.stopPropagation();
		event.preventDefault();
		MultiSelectInlineEditPreEnableCallback(fieldId, fieldName, entityName, recordId, config);
	});
	//Init enable action dblclick
	$(selectors.viewWrapper + " .form-control").on("dblclick", function (event) {
		event.stopPropagation();
		event.preventDefault();
		MultiSelectInlineEditPreEnableCallback(fieldId, fieldName, entityName, recordId, config);
		//clearSelection();//double click causes text to be selected.
		setTimeout(function () {
			$(selectors.editWrapper + " .form-control").get(0).focus();
		}, 200);
	});
	//Disable inline edit action
	$(selectors.editWrapper + " .cancel").on("click", function (event) {
		event.stopPropagation();
		event.preventDefault();
		MultiSelectInlineEditPreDisableCallback(fieldId, fieldName, entityName, recordId, config);
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
					MultiSelectInlineEditInitSuccessCallback(response, fieldId, fieldName, entityName, recordId, config);
				}
				else {
					MultiSelectInlineEditInitErrorCallback(response, fieldId, fieldName, entityName, recordId, config);
				}
			},
			error: function (jqXHR, textStatus, errorThrown) {
				var response = {};
				response.message = "";
				if (jqXHR && jqXHR.responseJSON) {
					response = jqXHR.responseJSON;
				}
				MultiSelectInlineEditInitErrorCallback(response, fieldId, fieldName, entityName, recordId, config);
			}
		});
	});
}

function MultiSelectInlineEditInitSuccessCallback(response, fieldId, fieldName, entityName, recordId, config) {
	var selectors = MultiSelectInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
	var newValue = ProcessNewValue(response, fieldName);

	$(selectors.viewOptionsListUl).html("");
	var selectOptions = $(selectors.inputEl + ' option');
	_.forEach(newValue, function (optionKey) {
		var matchedOption = _.find(selectOptions, function (record) {
			if (!optionKey && !record.attributes["value"].value) {
				return true;
			}
			else {
				return optionKey === record.attributes["value"].value;
			}
		});
		var optionLabel = matchedOption.text;
		$(selectors.viewOptionsListUl).append('<li class="select2-selection__choice" title="' + optionLabel + '" data-key="' + optionKey + '">' + optionLabel + '</li>');
	});


	$(selectors.inputEl).val(newValue).attr("data-original-value", JSON.stringify(newValue));
	MultiSelectInlineEditPreDisableCallback(fieldId, fieldName, entityName, recordId, config);
	toastr.success("The new value is successfull saved", 'Success!', { closeButton: true, tapToDismiss: true });
}

function MultiSelectInlineEditInitErrorCallback(response, fieldId, fieldName, entityName, recordId, config) {
	var selectors = MultiSelectInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
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
