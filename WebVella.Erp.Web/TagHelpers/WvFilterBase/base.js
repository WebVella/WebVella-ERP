function BaseFilterGenerateSelectors(filterId) {
	//Method for generating selector strings of some of the presentation elements
	var selectors = {};
	selectors.filterWrapper = "#erp-filter-" + filterId;
	selectors.filterRuleSelect = selectors.filterWrapper + " .erp-filter-rule";
	selectors.filterDivider = selectors.filterWrapper + " .erp-filter-divider";
	selectors.filterFirstValue = selectors.filterWrapper + " .form-control.value";
	selectors.filterSecondValue = selectors.filterWrapper + " .form-control.value2";

	selectors.filterFirstFakeValue = selectors.filterWrapper + " .form-control.fake-value";
	selectors.filterSecondFakeValue = selectors.filterWrapper + " .form-control.fake-value2";

	selectors.clearFilterLink = selectors.filterWrapper + " .clear-filter";
	return selectors;
}

function BaseFilterShowSecondValue(filterId,fieldName) {
	var selectors = BaseFilterGenerateSelectors(filterId);
	//Add SecondValue name
	$(selectors.filterSecondValue).attr("name",fieldName);
	//Show SecondValue control (fake if present)
	var fakeEl = $(selectors.filterSecondFakeValue);
	if (fakeEl.length > 0) {
		$(selectors.filterSecondFakeValue).removeClass("d-none");
	}
	else {
		$(selectors.filterSecondValue).removeClass("d-none");
	}
	//Show Divider
	$(selectors.filterDivider).removeClass("d-none");
	//Remove rounded border class (fake if present)
	$(selectors.filterFirstValue).removeClass("rounded-right");
}

function BaseFilterHideSecondValue(filterId,fieldName) {
	var selectors = BaseFilterGenerateSelectors(filterId);
	//Remove SecondValue name
	$(selectors.filterSecondValue).removeAttr("name");
	//Hide SecondValue control (fake if present)
	var fakeEl = $(selectors.filterSecondFakeValue);
	if (fakeEl.length > 0) {
		$(selectors.filterSecondFakeValue).addClass("d-none");
	}
	else {
		$(selectors.filterSecondValue).addClass("d-none");
	}
	//Hide Divider
	$(selectors.filterDivider).addClass("d-none");
	//Add rounded border class (fake if present)
	$(selectors.filterFirstValue).addClass("rounded-right");
}

function clearFilter(filterId) {
	var selectors = BaseFilterGenerateSelectors(filterId);
	if ($(selectors.filterFirstFakeValue).length > 0) {
		$(selectors.filterFirstFakeValue).val("");
	}
	$(selectors.filterFirstValue).val("");

	if ($(selectors.filterSecondFakeValue).length > 0) {
		$(selectors.filterSecondFakeValue).val("");
	}
	$(selectors.filterSecondValue).val("");

}


function BaseFilterInit(filterId,fieldName) {
	var selectors = BaseFilterGenerateSelectors(filterId);
	$(selectors.filterRuleSelect).change(function () {
		$(this).find("option:selected").each(function () {
			//Should Be only one
			var optionValue = $(this).attr("value");
			if (optionValue === "BETWEEN" || optionValue === "NOTBETWEEN") {
				BaseFilterShowSecondValue(filterId,fieldName);
			}
			else {
				BaseFilterHideSecondValue(filterId,fieldName);
			}
		});
	});
	$(selectors.clearFilterLink).on("click", function () {
		var filterEl = $(this).closest(".erp-filter");
		if (filterEl.length > 0) {
			var filterId = filterEl.attr("data-filter-id");
			if (filterId) {
				clearFilter(filterId);
			}
		}
		$(this).addClass("d-none");
	});
}

$(function () {
	$(".clear-filter-all").on("click", function () {
		var filters = $(this).closest(".drawer").find(".erp-filter");
		if (filters.length > 0) {
			$.each(filters,function (index,filterEl) {
				var filterId = $(filterEl).attr("data-filter-id");
				if (filterId) {
					clearFilter(filterId);
				}
			});
		}
	});
});