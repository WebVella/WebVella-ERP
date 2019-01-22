function CodeFormGenerateSelectors(fieldId, fieldName, config) {
	//Method for generating selector strings of some of the presentation elements
	var selectors = {};
	selectors.editorEl = "#ace-" + fieldId;
	selectors.editorElNoHash = "ace-" + fieldId;
	selectors.hiddenInputEl = "#input-" + fieldId;
	return selectors;
}

function CodeFormInit(fieldId, fieldName, entityName, recordId, config) {
	config = ProcessConfig(config);
	var selectors = CodeFormGenerateSelectors(fieldId, fieldName, config);


	var editor = ace.edit(selectors.editorElNoHash);
	editor.setTheme("ace/theme/" + config.theme);
	editor.session.setMode("ace/mode/" + config.language);
	editor.renderer.setOptions({
		showPrintMargin:false,
		maxLines:30,
		minLines:10
	});
	editor.setOptions({
		readOnly:config.read_only
	});
	editor.session.on('change', function(delta) {
		var value = editor.getValue();
		$(selectors.hiddenInputEl).val(value);
	});
	editor.session.setValue($(selectors.hiddenInputEl).val());
}

