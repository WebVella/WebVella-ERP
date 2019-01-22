"use strict";
var ApiBaseUrl = "/api/v3/en_US";

/*******************************************************************************
FIELDS GENRAL METHODS
*******************************************************************************/

function ProcessConfig(config) {
	if (config !== null && typeof config === 'object') {
		return config;
	}
	else if (config) {
		return JSON.parse(config);
	}
	else {
		return {};
	}
}

function ProcessNewValue(response, fieldName) {
	var newValue = null;
	if (response.object.data && Array.isArray(response.object.data)) {
		newValue = response.object.data[0][fieldName];
	}
	else if (response.object.data) {
		newValue = response.object.data[fieldName];
	}
	else if (response.object) {
		newValue = response.object[fieldName];
	}
	return newValue;
}

/*******************************************************************************
RELATED FIELD - FORM EDIT
*******************************************************************************/

//function RelatedFieldFormGenerateSelectors(fieldId, fieldName, config) {
//	//Method for generating selector strings of some of the presentation elements
//	var selectors = {};
//	if (!config.prefix || config === "") {
//		selectors.inputEl = "#input-" + fieldId;
//	}
//	else {
//		selectors.inputEl = "#input-" + config.prefix + "-" + fieldId;
//	}
//	selectors.modalEl = "#add-option-modal-" + fieldId;
//	selectors.primaryBtnEl = "#add-option-modal-" + fieldId + " .btn-primary";
//	selectors.modalFormEl = "#add-option-form-" + fieldId;
//	return selectors;
//}

//function RelatedFieldFormInit(fieldId, fieldName, entityName, config) {
//	config = ProcessConfig(config);
//	var selectors = RelatedFieldFormGenerateSelectors(fieldId, fieldName, config);
//	$(selectors.inputEl).select2({
//		ajax: {
//			url: '/api/v3.0/p/core/related-field-multiselect',
//			data: function (params) {
//				var query = {
//					search: params.term,
//					page: params.page || 1,
//					fieldName: fieldName,
//					entityName: entityName
//				}
//				return query;
//			}
//		},
//		language: "bg",
//		width: 'style',
//		closeOnSelect: true,
//		templateResult: function (state) {
//			var $state = $(
//				'<div class="erp-ta-result"><div class="icon-wrapper ' + ' go-bkg-' + state.color + '"><i class="fa fa-fw fa-' + state.iconName + '"/></div><div class="meta"><div class="title">' + state.text + '</div><div class="entity go-gray">' + state.entityName + '</div></div>'
//			);
//			return $state;
//		}
//	});

//	//Stops remove selection click opening the dropdown
//	$(selectors.inputEl).on("select2:unselect", function (evt) {
//		if (!evt.params.originalEvent) {
//			return;
//		}

//		evt.params.originalEvent.stopPropagation();
//	});
//	$(selectors.inputEl).on('select2:open', () => {
//		var appendLinkString = "<a href=\"javascript:void(0)\" onclick=\"addRelatedRecordModal('" + fieldId + "','" + fieldName + "','" + entityName + "','" + config.prefix + "')\" class=\"select2-add-option\"><i class=\"fa fa-plus-circle\"></i> create new record</a>";
//		if (config && config.can_add_values) {
//			$(".select2-results:not(:has(a))").append(appendLinkString);
//		}
//	});
//	$(selectors.modalEl).appendTo("body");
//	var $form = $('<form id="add-option-modal-' + fieldId + '" name="add-option-modal-' + fieldId + '"></form>');
//	$(selectors.modalEl + " .modal-dialog").append($form)
//	$(selectors.modalEl + " .modal-content").appendTo(selectors.modalEl + " form");
//	$(selectors.modalEl + " form").on("submit", (event) => {
//		event.preventDefault();
//		var $alertMessage = $(selectors.modalEl).find(".alert-danger");
//		$($alertMessage).addClass("d-none").html("");

//		$(selectors.modalEl).find(".btn-primary").attr("disabled", "disabled").find(".fa").removeClass("fa-plus-circle").addClass("fa-spin fa-spinner");
//		$.ajax({
//			type: "POST",
//			url: '/items/manage?handler=EntityNameViewName',
//			data: $(event.target).serialize(),
//			success: function (response) {
//				if (response.success) {
//					addRelatedRecordSuccessCallback(response, fieldId, fieldName, entityName, $(this).serialize(), inputValue, config.prefix);
//				}
//				else {
//					addRelatedRecordErrorCallback(response, fieldId, fieldName, entityName, $(this).serialize(), inputValue, config.prefix);
//				}
//			},
//			error: function (jqXHR, textStatus, errorThrown) {
//				console.log(jqXHR);
//				console.log(textStatus);
//				console.log(errorThrown);
//			}
//		});
//	});
//}

//function addRelatedRecordSuccessCallback(response, fieldId, fieldName, entityName, inputValue, prefix) {
//	var selectorInputEl = "";

//	if (!prefix || prefix === "") {
//		selectorInputEl = "#input-" + fieldId;
//	}
//	else {
//		selectorInputEl = "#input-" + prefix + "-" + fieldId;
//	}
//	var selectorModalEl = "#add-option-modal-" + fieldId;
//	var newOption = new Option(inputValue, inputValue, false, false);
//	$(selectorInputEl).append(newOption);
//	$(selectorInputEl).select2().val(inputValue).trigger('change');
//	$(selectorModalEl).modal('hide');
//}

//function addRelatedRecordErrorCallback(response, fieldId, fieldName, entityName, inputValue, prefix) {
//	var selectorInputEl = "";

//	if (!prefix || prefix === "") {
//		selectorInputEl = "#input-" + fieldId;
//	}
//	else {
//		selectorInputEl = "#input-" + prefix + "-" + fieldId;
//	}
//	var selectorModalEl = "#add-option-modal-" + fieldId;
//	var $alertMessage = $(selectorModalEl).find(".alert-danger");
//	$(selectorModalEl).find(".btn-primary").removeAttr("disabled", "disabled").find(".fa").addClass("fa-plus-circle").removeClass("fa-spin fa-spinner");
//	$($alertMessage).html(response.message).removeClass("d-none");
//}

//function addRelatedRecordModal(fieldId, fieldName, entityName, prefix) {
//	var selectorInputEl = "";

//	if (!prefix || prefix === "") {
//		selectorInputEl = "#input-" + fieldId;
//	}
//	else {
//		selectorInputEl = "#input-" + prefix + "-" + fieldId;
//	}
//	var selectorModalEl = "#add-option-modal-" + fieldId;
//	var $alertMessage = $(selectorModalEl).find(".alert-danger");
//	$($alertMessage).addClass("d-none").html("");

//	$(selectorModalEl).on('shown.bs.modal', function () {
//		$(selectorModalEl).find(".btn-primary").removeAttr("disabled", "disabled").find(".fa").addClass("fa-plus-circle").removeClass("fa-spin fa-spinner");
//		$("#input-" + fieldId).select2("close");
//	});

//	$(selectorModalEl).modal();
//};




/*******************************************************************************
VIEW
*******************************************************************************/

//function ViewGenerateSelectors(viewId, viewName, entityName, recordId, config) {
//	//Method for generating selector strings of some of the presentation elements
//	var selectors = {};
//	selectors.viewWrapper = "#view-" + viewId;
//	selectors.tabNavWrapper = "#tabnav-" + viewId;
//	selectors.tabContentWrapper = "#tabcontent-" + viewId;
//	return selectors;
//}

//function ViewInit(viewId, viewName, entityName, recordId, config) {
//	config = ProcessConfig(config);
//	var selectors = ViewGenerateSelectors(viewId, viewName, entityName, recordId, config);
//	$(selectors.tabNavWrapper + " .nav-link").on("click", function (e) {
//		e.preventDefault();
//		$(this).tab('show');
//	});
//	$(selectors.viewWrapper + " .card-header").on("click", function (e) {
//		e.preventDefault();
//		var sectionBodyId = $(this).attr("data-target");
//		var isSectionBodyShowed = $(sectionBodyId).hasClass("show");
//		if (isSectionBodyShowed) {
//			$(this).find(".fa-caret-down").addClass("d-none");
//			$(this).find(".fa-caret-right").removeClass("d-none");
//			$(sectionBodyId).removeClass("show");
//		}
//		else {
//			$(this).find(".fa-caret-down").removeClass("d-none");
//			$(this).find(".fa-caret-right").addClass("d-none");
//			$(sectionBodyId).addClass("show");
//		}
//	});
//}

/*******************************************************************************
LIST
*******************************************************************************/


////List filter
//function removeSubfilter(listId, filterGroupId) {
//	var $filterEl = $("#filter-group-" + filterGroupId);
//	var $filterTogglerEl = $("#filter-toggle-" + filterGroupId);
//	var filterGroupQueryNameList = JSON.parse($("#filter-group-" + filterGroupId).attr("data-query-name-list"));

//	$filterEl.addClass("d-none").removeClass("show");
//	$filterEl.find(".dropdown-toggle").attr("aria-expanded", "false");
//	$filterEl.find(".dropdown-menu").removeClass("show");
//	//Modify filters
//	$filterTogglerEl.find("i").removeClass("fa-check-square").addClass("fa-square");

//	//Check if all filters are hidden so you can hide the subfilter
//	var allSubFiltersHidden = true;
//	$("#list-filter-" + listId + " .subfilters .list-inline-item").each(function (i, filterObj) {
//		if (!$(filterObj).hasClass("d-none") && !$(filterObj).hasClass("filter-label")) {
//			allSubFiltersHidden = false;
//		}
//	});
//	if (allSubFiltersHidden) {
//		$("#list-filter-" + listId + " .subfilters").addClass("d-none");
//	}

//	//Clean cookie
//	var listConfigCookie = ProcessConfig(Cookies.get('wv_list_' + listId));
//	//Set cookie
//	const idx = _.indexOf(listConfigCookie["filter_show_fields"], filterGroupId);
//	if (idx !== -1) {
//		listConfigCookie["filter_show_fields"].splice(idx, 1);
//		Cookies.set('wv_list_' + listId, listConfigCookie, { expires: 30 });
//		//Remove query in url and redirect
//		var currentUrl = window.location.href;
//		var uri = new URI(currentUrl);
//		var oneQueryNameFound = false;
//		_.forEach(filterGroupQueryNameList, function (queryName) {
//			if (uri.hasQuery(queryName)) {
//				uri.removeQuery(queryName);
//				oneQueryNameFound = true;
//			}
//		});
//		if (oneQueryNameFound) {
//			window.location = uri;
//		}
//	}
//}

//function ErpListClearSubfilter(e, listId, filterGroupId) {
//	var filterGroupQueryNameList = JSON.parse($("#filter-group-" + filterGroupId).attr("data-query-name-list"));
//	var currentUrl = window.location.href;
//	var uri = new URI(currentUrl);
//	var oneQueryNameFound = false;
//	_.forEach(filterGroupQueryNameList, function (queryName) {
//		if (uri.hasQuery(queryName)) {
//			uri.removeQuery(queryName);
//			oneQueryNameFound = true;
//		}
//	});
//	if (oneQueryNameFound) {
//		window.location = uri;
//	}
//}

//function ErpListFilterChange(e, listId, element) {
//	//e.preventDefault();
//	//e.stopPropagation();
//	console.log("changed");
//}


//function ErpListApplyFilterChange(e, listId) {
//	//e.preventDefault();
//	//e.stopPropagation();
//	$("#list-filter-" + listId).trigger("submit");
//}

//function ErpListFilterInit(listId) {
//	//Init
//	var generalSearchQueryName = $("#list-filter-" + listId).attr("data-general-filter-query-name");
//	var generalSearchGroupId = $("#list-filter-" + listId).attr("data-general-filter-group-id");

//	//Show Hide filters
//	$("#list-filter-" + listId + " .filter-toggle").click(function (e) {
//		e.preventDefault();
//		//e.stopPropagation(); //Stops the filter dropdown from closing on click

//		var filterGroupId = $(this).attr("data-filter-group-id");
//		var filterGroupQueryNameList = JSON.parse($("#filter-group-" + filterGroupId).attr("data-query-name-list"));
//		var showNow = false;
//		var listConfigCookie = ProcessConfig(Cookies.get('wv_list_' + listId));

//		var idx = _.indexOf(listConfigCookie["filter_show_fields"], filterGroupId);

//		if (idx === -1) {
//			showNow = true;
//		}

//		//Apply show changes
//		if (showNow) {
//			listConfigCookie["filter_show_fields"].push(filterGroupId);
//			Cookies.set('wv_list_' + listId, listConfigCookie, { expires: 30 });
//			//check checkbox
//			$(this).find("i").removeClass("fa-square").addClass("fa-check-square");

//			//Hide any other filter dropdown
//			$("#list-filter-" + listId + " .subfilters .list-inline-item").each(function (i, filterObj) {
//				$(filterObj).removeClass("show");
//				$(filterObj).find(".dropdown-toggle").attr("aria-expanded", "false");
//				$(filterObj).find(".dropdown-menu").removeClass("show");
//			});

//			var $filterEl = $("#filter-group-" + filterGroupId);
//			setTimeout(function () {
//				$("#list-filter-" + listId + " .subfilters").removeClass("d-none");
//				$filterEl.removeClass("d-none").addClass("show");
//				$filterEl.find(".dropdown-toggle").trigger("click");
//			}, 100);
//		}
//		//Apply hide changes
//		else {
//			//uncheck checkbox
//			removeSubfilter(listId, filterGroupId)

//		}
//	});

//	$("#list-filter-" + listId + " .subfilters .dropdown-menu").on("click.bs.dropdown", function (e) { e.stopPropagation(); });

//	//Apply query to filters
//	$("#list-filter-" + listId + " .subfilters .list-filter-field").each(function (i, typeObj) {
//		var currentUrl = window.location.href;
//		var uri = new URI(currentUrl);
//		var currentQueryObj = uri.search(true);
//		var filterGroupId = $(typeObj).closest(".list-inline-item").attr("data-filter-group-id");
//		$("#filter-group-menu-" + filterGroupId + " .apply-filter").on("click", function (event) { ErpListApplyFilterChange(event, listId) });
//		$("#filter-group-menu-" + filterGroupId + " .clear-filter").on("click", function (event) { ErpListClearSubfilter(event, listId, filterGroupId) });
//		//Apply all fields except fts
//		$(typeObj).find(".form-group").each(function (g, obj) {
//			var fieldType = $(typeObj).attr("data-field-type");
//			var queryName = $(obj).attr("data-query-name");

//			//set input value
//			switch (fieldType) {
//				case "CheckboxField":
//					$(obj).find("input[name='" + queryName + "']").each(function (i, obj) {
//						var objValue = $(obj).val();
//						if (currentQueryObj[queryName] === $(obj).val()) {
//							$(obj).prop("checked", true);
//						}
//					});
//					$(obj).find("input[name='" + queryName + "']").on("change", function (event) { ErpListApplyFilterChange(event, listId, this) });
//					break;
//				case "DateField":
//					$(obj).find("input[name='" + queryName + "']").val(currentQueryObj[queryName]);
//					ErpListFilterInitFlatPickrDate("#filter-" + listId + "-" + queryName, listId, this);
//					$(obj).find("input[name='" + queryName + "']").on("change", function (event) { ErpListFilterChange(event, listId, this) });
//					break;
//				case "DateTimeField":
//					$(obj).find("input[name='" + queryName + "']").val(currentQueryObj[queryName]);
//					ErpListFilterInitFlatPickrDateTime("#filter-" + listId + "-" + queryName, listId, this);
//					$(obj).find("input[name='" + queryName + "']").on("change", function (event) { ErpListFilterChange(event, listId, this) });
//					break;
//				case "MultiSelectField":
//					$(obj).find("input[name='" + queryName + "']").each(function (i, obj) {
//						var objValue = $(obj).val();
//						var queryValuesArray = currentQueryObj[queryName];
//						_.forEach(queryValuesArray, function (queryValue) {
//							if (queryValue === $(obj).val()) {
//								$(obj).prop("checked", true);
//							}
//						});
//					});
//					$(obj).find("input[name='" + queryName + "']").on("change", function (event) { ErpListFilterChange(event, listId, this) });
//					break;
//				case "SelectField":
//					$(obj).find("input[name='" + queryName + "']").each(function (i, obj) {
//						var objValue = $(obj).val();
//						if (currentQueryObj[queryName] === $(obj).val()) {
//							$(obj).prop("checked", true);
//						}
//					});
//					$(obj).find("input[name='" + queryName + "']").on("change", function (event) { ErpListApplyFilterChange(event, listId, this) });
//					break;
//				case "PercentField":
//					$(obj).find("input[name='" + queryName + "']").val(currentQueryObj[queryName]);
//					ErpListPercentFilterInit(listId, queryName);
//					$(obj).find("input[name='" + queryName + "']").on("keyup mouseup", function (event) { ErpListFilterChange(event, listId, this) });
//					break;
//				default:
//					$(obj).find("input[name='" + queryName + "']").val(currentQueryObj[queryName]);
//					$(obj).find("input[name='" + queryName + "']").on("keyup mouseup", function (event) { ErpListFilterChange(event, listId, this) });
//					break;
//			}
//		});

//		//Apply fts
//		$("#list-filter-" + listId + " .filters").find("input[name='" + generalSearchQueryName + "']").val(currentQueryObj[generalSearchQueryName]);
//	});

//	//Submit
//	$("#list-filter-" + listId).on("submit", function (e) {
//		e.preventDefault();
//		var currentUrl = window.location.href;
//		var uri = new URI(currentUrl);
//		var currentQueryObj = uri.search(true);
//		var listCookie = ProcessConfig(Cookies.get('wv_list_' + listId));
//		$("#list-filter-" + listId + " .subfilters .list-filter-field").each(function (i, typeObj) {
//			$(typeObj).find(".form-group").each(function (g, obj) {
//				var fieldType = $(typeObj).attr("data-field-type");
//				var queryName = $(obj).attr("data-query-name");
//				var filterGroupId = $(typeObj).closest(".list-inline-item").attr("data-filter-group-id");
//				var filterGroupQueryNameList = JSON.parse($("#filter-group-" + filterGroupId).attr("data-query-name-list"));
//				var filterValue = "";
//				switch (fieldType) {
//					case "CheckboxField":
//						filterValue = $(obj).find("input:radio[name='" + queryName + "']:checked").val();
//						var idx = _.indexOf(listCookie["filter_show_fields"], filterGroupId);
//						if (idx === -1 || isStringNullOrEmptyOrWhiteSpace(filterValue)) {
//							if (uri.hasQuery(queryName)) {
//								uri.removeQuery(queryName);
//							}
//						}
//						else {
//							uri.setQuery(queryName, filterValue);
//						}
//						break;
//					case "MultiSelectField":
//						var selectedOptionsArray = [];
//						$(obj).find("input:checkbox[name='" + queryName + "']:checked").each(function (f, msOption) {
//							selectedOptionsArray.push($(msOption).val());
//						});
//						var idx4 = _.indexOf(listCookie["filter_show_fields"], filterGroupId);
//						if (idx4 === -1 || selectedOptionsArray.length === 0) {
//							if (uri.hasQuery(queryName)) {
//								uri.removeQuery(queryName);
//							}
//						}
//						else {
//							uri.setQuery(queryName, selectedOptionsArray);
//						}
//						break;
//					case "SelectField":
//						filterValue = $(obj).find("input:radio[name='" + queryName + "']:checked").val();
//						var idx2 = _.indexOf(listCookie["filter_show_fields"], filterGroupId);
//						if (idx2 === -1 || isStringNullOrEmptyOrWhiteSpace(filterValue)) {
//							if (uri.hasQuery(queryName)) {
//								uri.removeQuery(queryName);
//							}
//						}
//						else {
//							uri.setQuery(queryName, filterValue);
//						}
//						break;
//					default:
//						filterValue = $(obj).find("input[name='" + queryName + "']").val();
//						var idx3 = _.indexOf(listCookie["filter_show_fields"], filterGroupId);
//						if (idx3 === -1 || isStringNullOrEmptyOrWhiteSpace(filterValue)) {
//							if (uri.hasQuery(queryName)) {
//								uri.removeQuery(queryName);
//							}
//						}
//						else {
//							uri.setQuery(queryName, filterValue);
//						}
//						break;
//				}
//			});
//		});

//		//Add FTS field
//		var ftsValue = $("#list-filter-" + listId + " .filters").find("input[name='" + generalSearchQueryName + "']").val();
//		if (ftsValue) {
//			uri.setQuery(generalSearchQueryName, ftsValue);
//		}
//		else {
//			uri.removeQuery(generalSearchQueryName);
//		}

//		uri.removeQuery("page");
//		window.location = uri;
//	});

//	//Remove all filters
//	$("#list-filter-" + listId + " .remove-filters").on("click", function (e) {
//		e.preventDefault();
//		var listConfigCookie = ProcessConfig(Cookies.get('wv_list_' + listId));
//		var currentUrl = window.location.href;
//		var uri = new URI(currentUrl);
//		var currentQueryObj = uri.search(true);

//		_.forEach(listConfigCookie["filter_show_fields"], function (filterGroupId) {
//			if (generalSearchGroupId !== filterGroupId) {
//				var filterGroupQueryNameList = JSON.parse($("#filter-group-" + filterGroupId).attr("data-query-name-list"));
//				_.forEach(filterGroupQueryNameList, function (queryName) {
//					if (queryName !== generalSearchQueryName) {
//						//listConfigCookie["filter_show_fields"].splice(idx, 1);
//						//Clear URL
//						uri.removeQuery(queryName);
//					}
//				});
//			}
//		});
//		//Set new cookie
//		var idx = _.indexOf(listConfigCookie["filter_show_fields"], generalSearchGroupId);
//		if (idx > -1) {
//			Cookies.set('wv_list_' + listId, [generalSearchGroupId], { expires: 30 });
//		}
//		else {
//			Cookies.set('wv_list_' + listId, [], { expires: 30 });
//		}

//		//Redirect
//		window.location = uri;
//	});
//}


////JS DateTime picker init method
//var ErpListFilterDatePickerQueryFormat = "Y-m-d";
//var ErpListFilterDatePickerUserFormat = "d M Y";
////From the server dates will be received yyyy-MM-ddTHH:mm:ss.fff
//function ErpListFilterInitFlatPickrDate(selector, listId, element) {
//	const fp = document.querySelector(selector)._flatpickr;
//	if (!fp) {
//		var instance = flatpickr(selector, { time_24hr: true, dateFormat: ErpListFilterDatePickerQueryFormat, locale: BulgarianDateTimeLocale, altInput: true, altFormat: ErpListFilterDatePickerUserFormat });
//		return instance;
//	}
//	else {
//		return fp;
//	}
//}

//var ErpListFilterDateTimePickerQueryFormat = "Z";//"Y-m-dTH:i:S";
//var ErpListFilterDateTimePickerUserFormat = "d M Y H:i";
////From the server dates will be received yyyy-MM-ddTHH:mm:ss.fff
//function ErpListFilterInitFlatPickrDateTime(selector, listId, element) {
//	const fp2 = document.querySelector(selector)._flatpickr;
//	if (!fp2) {
//		var instance = flatpickr(selector, { time_24hr: true, dateFormat: ErpListFilterDateTimePickerQueryFormat, locale: BulgarianDateTimeLocale, enableTime: true, minuteIncrement: 1, altInput: true, altFormat: ErpListFilterDateTimePickerUserFormat });
//		return instance;
//	}
//	else {
//		return fp2;
//	}
//}

//function ErpListSetPercentFilter(listId, queryName) {
//	var value = $("input[name='" + queryName + "']").val();
//	if (!value || value === null) {
//		$("#filter-" + listId + "-" + queryName).closest(".input-group-append").first().addClass("d-none");
//	}
//	else {
//		var percent = _.multiply(value * 100);
//		var rounded = _.round(percent, 2);
//		$("#filter-" + listId + "-" + queryName).text(rounded);
//		$("#filter-" + listId + "-" + queryName).closest(".input-group-append").first().removeClass("d-none");
//	}
//}

//function ErpListPercentFilterInit(listId, queryName) {
//	ErpListSetPercentFilter(listId, queryName);
//	$("input[name='" + queryName + "']").on("keyup mouseup", function () {
//		ErpListSetPercentFilter(listId, queryName);
//	});
//}