function CheckboxFormGenerateSelectors(fieldId) {
	//Method for generating selector strings of some of the presentation elements
	var selectors = {};
	selectors.inputEl = "#input-" + fieldId;
	selectors.submittedInput = "[data-source-id=input-" + fieldId + "]";
	return selectors;
}

function CheckboxFormInit(fieldId) {
	var selectors = CheckboxFormGenerateSelectors(fieldId);
	//Remove value
	$(selectors.inputEl).first().on('change', function (e) {
		if ($(this).prop('checked')) {
			$(selectors.submittedInput).val("true");
			$(selectors.submittedInput).trigger("change");
		}
		else {
			$(selectors.submittedInput).val("false");
			$(selectors.submittedInput).trigger("change");
		}
	});

}