function PercentFilterGenerateSelectors(filterId) {
	//Method for generating selector strings of some of the presentation elements
	var selectors = {};
	selectors.inputValueEl = "#erp-filter-input-value-" + filterId;
	selectors.fakeValueEl = "#erp-filter-fake-value-" + filterId;
	selectors.inputValue2El = "#erp-filter-input-value2-" + filterId;
	selectors.fakeValue2El = "#erp-filter-fake-value2-" + filterId;
	return selectors;
}

function PercentFilterSetPercent(filterId,fakeName,inputName) {
	var selectors = PercentFilterGenerateSelectors(filterId);
	var fakeEl = selectors[fakeName];
	var inputEl = selectors[inputName];

	var value = $(fakeEl).val();
	if (!value || value === null) {
		$(inputEl).val(null);
	}
	else {
		var valDec = new Decimal(value);
		var hundDec = new Decimal(100);
		var percentDec = valDec.dividedBy(hundDec);
		$(inputEl).val(percentDec.toString());
	}
}

function PercentFilterInit(filterId) {
	var selectors = PercentFilterGenerateSelectors(filterId);
	$(selectors.fakeValueEl).on("change paste keyup", function () {
		PercentFilterSetPercent(filterId,"fakeValueEl","inputValueEl");
	});
	$(selectors.fakeValue2El).on("change paste keyup", function () {
		PercentFilterSetPercent(filterId,"fakeValue2El","inputValue2El");
	});
}