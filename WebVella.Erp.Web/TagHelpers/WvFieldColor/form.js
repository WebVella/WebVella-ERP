function ColorFormGenerateSelectors(fieldId) {
	//Method for generating selector strings of some of the presentation elements
	var selectors = {};
	selectors.inputControl = "#input-" + fieldId;
	return selectors;
}

function ColorFormInit(fieldId) {
	var selectors = ColorFormGenerateSelectors(fieldId);

	$(selectors.inputControl).spectrum({
		showPaletteOnly: true,
		showPalette:true,
		allowEmpty: true,
		preferredFormat: "hex",
		palette: [
			['#B71C1C', '#F44336', '#FFEBEE', '#880E4F','#E91E63','#FCE4EC','#4A148C','#9C27B0','#F3E5F5'],
			['#311B92', '#673AB7', '#EDE7F6', '#1A237E','#3F51B5','#E8EAF6','#0D47A1','#2196F3','#E3F2FD'],
			['#01579B', '#03A9F4', '#E1F5FE', '#006064','#00BCD4','#E0F7FA','#004D40','#009688','#E0F2F1'],
			['#1B5E20', '#4CAF50', '#E8F5E9', '#33691E','#8BC34A','#F1F8E9','#827717','#CDDC39','#F9FBE7'],
			['#F57F17', '#FFEB3B', '#FFFDE7', '#FF6F00','#FFC107','#FFF8E1','#E65100','#FF9800','#FFF3E0'],
			['#BF360C', '#FF5722', '#FBE9E7', '#3E2723','#795548','#EFEBE9','#212121','#9E9E9E','#FAFAFA'],
			['#CCCCCC', '#263238', '#607D8B', '#ECEFF1','#FFFFFF','#000000']
		]
	});
}

