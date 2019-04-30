function MultiSelectFormGenerateSelectors(fieldId, fieldName, config) {
	//Method for generating selector strings of some of the presentation elements
	var selectors = {};
	if (!config.prefix || config === "") {
		selectors.inputEl = "#input-" + fieldId;
	}
	else {
		selectors.inputEl = "#input-" + config.prefix + "-" + fieldId;
    }
    if (!config.prefix || config === "") {
        selectors.dummyInputEl = "#dummy-" + fieldId;
    }
    else {
        selectors.dummyInputEl = "#dummy-" + config.prefix + "-" + fieldId;
    }
	selectors.modalEl = "#add-option-modal-" + fieldId;
	selectors.primaryBtnEl = "#add-option-modal-" + fieldId + " .btn-primary";
	selectors.modalFormEl = "#add-option-form-" + fieldId;
	return selectors;
}

function MultiSelectFormInit(fieldId, fieldName, entityName, config) {
	config = ProcessConfig(config);
	var selectors = MultiSelectFormGenerateSelectors(fieldId, fieldName, config);

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
	$(selectors.inputEl).on('select2:open', function(){
		var appendLinkString = "<a href=\"javascript:void(0)\" onclick=\"addMultiSelectOptionModal('" + fieldId + "','" + fieldName + "','" + entityName + "')\" class=\"select2-add-option\"><i class=\"fa fa-plus-circle\"></i> create new record</a>";
		if (config && config.can_add_values) {
			$(".select2-results:not(:has(a))").append(appendLinkString);
		}
	});

	$(selectors.inputEl).on('change', function(event) {
        var customEvent = new Event('WvFieldSelect_Change');
		var inputElement = document.getElementById('input-' + fieldId);
		var selectedJson = $(selectors.inputEl).select2('data');
		var selectedKeys = [];
		for (var i= 0; i < selectedJson.length; i++) {
			selectedKeys.push(selectedJson[i].id); //this is a single select
        }
        var fieldname = $(selectors.inputEl).attr("data-field-name");
        if (selectedKeys.length === 0) {
            var attrDummy = $(selectors.dummyInputEl).attr('name');
            $(selectors.inputEl).removeAttr("name");
            if (typeof attrDummy !== typeof undefined && attrDummy !== false) {
                // Element has this attribute
            }
            else {
                $(selectors.dummyInputEl).attr('name', fieldname);
            }
        }
        else {
            var attr = $(selectors.inputEl).attr('name');
            $(selectors.dummyInputEl).removeAttr("name");
            if (typeof attr !== typeof undefined && attr !== false) {
                // Element has this attribute
            }
            else {
                $(selectors.inputEl).attr('name', fieldname);
            }
        }

		customEvent.payload = {
			value: selectedKeys,
			fieldId: fieldId,
			fieldName:inputElement.name
		};
		document.dispatchEvent(customEvent);		
	});


	$(selectors.modalEl).appendTo("body");
	var $form = $('<form id="add-option-modal-' + fieldId + '" name="add-option-modal-' + fieldId + '"></form>');
	$(selectors.modalEl + " .modal-dialog").append($form);
	$(selectors.modalEl + " .modal-content").appendTo(selectors.modalEl + " form");
	$(selectors.modalEl + " form").on("submit", function(event) {
		event.preventDefault();
		var $alertMessage = $(selectors.modalEl).find(".alert-danger");
		$($alertMessage).addClass("d-none").html("");
		var inputValue = $(selectors.modalEl).find(".add-option-input").val();
		if (!isStringNullOrEmptyOrWhiteSpace(inputValue)) {
			var submitObj = {};
			submitObj.value = inputValue;
			submitObj.entityName = entityName;
			submitObj.fieldName = fieldName;
			$(selectors.modalEl).find(".btn-primary").attr("disabled", "disabled").find(".fa").removeClass("fa-plus-circle").addClass("fa-spin fa-spinner");
			$.ajax({
				headers: {
					'Accept': 'application/json',
					'Content-Type': 'application/json'
				},
				type: "PUT",
				url: '/api/v3.0/p/core/select-field-add-option',
				data: JSON.stringify(submitObj),
				success: function (response) {
					if (response.success) {
						addMultiSelectOptionSuccessCallback(response, fieldId, fieldName, entityName, inputValue);
					}
					else {
						addMultiSelectOptionErrorCallback(response, fieldId, fieldName, entityName, inputValue);
					}
				},
				error: function (jqXHR, textStatus, errorThrown) {
					var response = {};
				response.message = "";
				if (jqXHR && jqXHR.responseJSON) {
					response = jqXHR.responseJSON;
				}
					addMultiSelectOptionErrorCallback(response, fieldId, fieldName, entityName, inputValue);
				}
			});
		}
		else {
			$($alertMessage).html("Required field").removeClass("d-none");
		}
	});
}


function addMultiSelectOptionSuccessCallback(response, fieldId, fieldName, entityName, inputValue) {
	var selectorInputEl = "#input-" + fieldId;
	var selectorModalEl = "#add-option-modal-" + fieldId;
	var newOption = new Option(inputValue, inputValue, false, false);
	$(selectorInputEl).append(newOption);
	var selectedValues = $(selectorInputEl).select2().val();
	selectedValues.push(inputValue);
	$(selectorInputEl).select2().val(selectedValues).trigger('change');
	$(selectorModalEl).modal('hide');
}

function addMultiSelectOptionErrorCallback(response, fieldId, fieldName, entityName, inputValue) {
	var selectorInputEl = "#input-" + fieldId;
	var selectorModalEl = "#add-option-modal-" + fieldId;
	var $alertMessage = $(selectorModalEl).find(".alert-danger");
	$(selectorModalEl).find(".btn-primary").removeAttr("disabled", "disabled").find(".fa").addClass("fa-plus-circle").removeClass("fa-spin fa-spinner");
	$($alertMessage).html(response.message).removeClass("d-none");
}

function addMultiSelectOptionModal(fieldId, fieldName, entityName) {
	var selectorInputEl = "#input-" + fieldId;
	var selectorModalEl = "#add-option-modal-" + fieldId;
	var $alertMessage = $(selectorModalEl).find(".alert-danger");
	$($alertMessage).addClass("d-none").html("");

	$(selectorModalEl).on('shown.bs.modal', function () {
		$(selectorModalEl).find(".add-option-input").val("");
		$(selectorModalEl).find(".btn-primary").removeAttr("disabled", "disabled").find(".fa").addClass("fa-plus-circle").removeClass("fa-spin fa-spinner");
		$(selectorInputEl).select2("close");
		$('.add-option-input').trigger('focus');
	});
	$(selectorModalEl).modal();
}

