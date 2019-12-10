function DataSourceFormEditGenerateSelectors(fieldId) {
	//Method for generating selector strings of some of the presentation elements
	var selectors = {};
	selectors.dataSourceInput = "#input-" + fieldId + "-datasource";
	selectors.formControlInput = "#input-" + fieldId;
	selectors.linkNewBtn = "#input-" + fieldId + "-create-ds-link";
	selectors.removeBtn = "#input-" + fieldId + "-remove-ds-link";
	selectors.editBtn = "#input-" + fieldId + "-edit-ds-link";
	selectors.modal = "#modal-" + fieldId + "-datasource";
	selectors.modalTypeRadio = selectors.modal + " .ds-type-radio";
	selectors.closeBtn = selectors.modal + " .close";
	selectors.cancelBtn = selectors.modal + " .cancel";
	selectors.testBtn = selectors.modal + " .test";

	selectors.submitBtn = selectors.modal + " .submit";
	selectors.defaultFormGroup = "#modal-" + fieldId + "-default-group";
	selectors.dataSourceFormGroup = "#modal-" + fieldId + "-datasource-group";
	selectors.codeFormGroup = "#modal-" + fieldId + "-code-group";
	selectors.htmlFormGroup = "#modal-" + fieldId + "-html-group";
	selectors.snippetFormGroup = "#modal-" + fieldId + "-snippet-group";
	selectors.defaultFormGroup = "#modal-" + fieldId + "-default-group";
	selectors.codeValueInput = "#modal-" + fieldId + "-code-input";
	selectors.codeValueEditor = "#modal-" + fieldId + "-code-editor";
	selectors.htmlValueInput = "#modal-" + fieldId + "-html-input";
	selectors.htmlValueEditor = "#modal-" + fieldId + "-html-editor";
	selectors.snippetValueInput = "#modal-" + fieldId + "-snippet-input";
	selectors.snippetValueEditor = "#modal-" + fieldId + "-snippet-editor";

	selectors.pasteSimpleCodeLink = selectors.modal + " .simple-code";
	selectors.pasteDataSourceCodeLink = selectors.modal + " .datasource-code";
	selectors.pasteDataSourceTransformationCodeLink = selectors.modal + " .datasource-selectoptions-code";

	return selectors;
}

function initEditor(fieldId) {
	var selectors = DataSourceFormEditGenerateSelectors(fieldId);
	//Init Code editor
	var editor = ace.edit(selectors.codeValueEditor.replace("#", ""));
	editor.setTheme("ace/theme/cobalt");
	editor.session.setMode("ace/mode/csharp");
	editor.renderer.setOptions({
		showPrintMargin: false,
		maxLines: 30
	});
	editor.session.setValue($(selectors.codeValueInput).val());
	editor.session.on('change', function () {
		var value = editor.getValue();
		$(selectors.codeValueInput).val(value);
	});
	//Init Html Editor
	var editorHtml = ace.edit(selectors.htmlValueEditor.replace("#", ""));
	editorHtml.setTheme("ace/theme/cobalt");
	editorHtml.session.setMode("ace/mode/html");
	editorHtml.renderer.setOptions({
		showPrintMargin: false,
		maxLines: 30
	});
	editorHtml.session.setValue($(selectors.htmlValueInput).val());
	editorHtml.session.on('change', function () {
		var value = editorHtml.getValue();
		$(selectors.htmlValueInput).val(value);
	});
	//Init Snippet Editor
	var editorSnippet = ace.edit(selectors.snippetValueEditor.replace("#", ""));
	editorSnippet.setTheme("ace/theme/cobalt");
	editorSnippet.setReadOnly(true);
	var resourceName = $(selectors.snippetValueInput).val();
	if(resourceName && resourceName.endsWith(".cs")){
		editorSnippet.session.setMode("ace/mode/csharp");
	}
	else{
		editorSnippet.session.setMode("ace/mode/html");
	}
	editorSnippet.renderer.setOptions({
		showPrintMargin: false,
		maxLines: 30
	});
	editorSnippet.session.setValue($(selectors.snippetValueInput).val());
	editorSnippet.session.on('change', function () {
		var value = editorSnippet.getValue();
		$(selectors.snippetValueInput).val(value);
	});
}

function destroyEditor(fieldId) {
	var selectors = DataSourceFormEditGenerateSelectors(fieldId);
	//Destroy code editor
	var editor = ace.edit(selectors.codeValueEditor.replace("#", ""));
	editor.destroy();
	//Destroy html editor
	var editorHtml = ace.edit(selectors.htmlValueEditor.replace("#", ""));
	editorHtml.destroy();
}

function GetSnippetTaResults(query, process) {
	return $.get("/api/v3/en_US/snippets", { search: query }, function (data) {
		return process(data);
	});
}


function GetSnippetTaTemplate(item){
	return item;
}

function GetSnippetTaSelected(item,fieldId){
	var selectors = DataSourceFormEditGenerateSelectors(fieldId);
	$.get("/api/v3/en_US/snippet?name=" + item, function (data) {
		var editorSnippet = ace.edit(selectors.snippetValueEditor.replace("#", ""));
		$(selectors.snippetValueInput).val(data.object);
		if(item.endsWith(".cs")){
			editorSnippet.session.setMode("ace/mode/csharp");
		}
		else{
			editorSnippet.session.setMode("ace/mode/html");
		}
		editorSnippet.session.setValue(data.object);
	});
	
}

function DataSourceFormEditInit(fieldId, propertyNameLibrary) {
	var selectors = DataSourceFormEditGenerateSelectors(fieldId);

	var intTypeVal = $(selectors.modalTypeRadio + ":checked").val();
	if (intTypeVal === "1" || intTypeVal === 1) {
		$(selectors.testBtn).removeClass("d-none");
		//initEditor(fieldId);
	}

	if (intTypeVal === "3" || intTypeVal === 3) {
		$(selectors.defaultFormGroup).addClass("d-none");
		//initEditor(fieldId);
	}


	var typeaheadEl = $(selectors.dataSourceFormGroup + " input.form-control");
	if (typeaheadEl) {
		typeaheadEl.typeahead({ source: propertyNameLibrary, items: 'all', fitToElement: true });
	}

	var snippetTaOptions = {
		minLength:1,
		source:GetSnippetTaResults,
		selectOnBlur:false,
		autoSelect:false,
		displayText:GetSnippetTaTemplate,
		fitToElement:true,
		afterSelect:function(item){GetSnippetTaSelected(item,fieldId);}
	};

	var typeaheadSnippetEl = $(selectors.snippetFormGroup + " input.form-control");

	if (typeaheadSnippetEl) {
		typeaheadSnippetEl.typeahead(snippetTaOptions);
	}


	$(selectors.submitBtn).click(function () {
		//If code is selected submit first to check it, if not, submit;
		var typeVal = $(selectors.modalTypeRadio + ":checked").val();
		if (typeVal === 0 || typeVal === "0" || typeVal === 2 || typeVal === "2" || typeVal === 3 || typeVal === "3") {
			submitOption(fieldId);
		}
		else {
			var editor = ace.edit(selectors.codeValueEditor.replace("#", ""));
			var submitObject = {};
			submitObject.csCode = editor.getValue();
			$.ajax({
				headers: {
					'Accept': 'application/json',
					'Content-Type': 'application/json'
				},
				url: "/api/v3.0/datasource/code-compile",
				type: 'POST',
				data: JSON.stringify(submitObject),
				success: function (response) {
					if (response.success) {
						submitOption(fieldId);
					}
					else {
						toastr.error(response.message, { timeOut: 10000 });
					}
				},
				error: function (jqXHR, textStatus, errorThrown) {
					var response = {};
					response.message = "";
					if (jqXHR && jqXHR.responseJSON) {
						response = jqXHR.responseJSON;
					}
					toastr.error(response.message);
				}
			});
		}
	});


	$(selectors.removeBtn).click(function () {
		$(selectors.dataSourceInput).closest(".input-group").addClass("d-none");
		$(selectors.formControlInput).val(null);
		$(selectors.formControlInput).closest(".input-group").removeClass("d-none");
	});

	$(selectors.cancelBtn).click(function () {
		$(selectors.modal).appendTo($(selectors.formControlInput).closest(".form-group"));
		$(selectors.modal).modal("hide");
	});

	$(selectors.closeBtn).click(function () {
		$(selectors.modal).appendTo($(selectors.formControlInput).closest(".form-group"));
		$(selectors.modal).modal("hide");
	});

	$(selectors.editBtn).click(function () {
		$(selectors.modal).appendTo("body");
		$(selectors.modal).modal("show");
		$(selectors.modalTypeRadio).attr("name", "ds-type-radio");
	});

	$(selectors.linkNewBtn).click(function () {
		$(selectors.modal).appendTo("body");
		//initEditor(fieldId);
		$(selectors.modal).modal({ backdrop: "static", keyboard: false });
		$(selectors.modal).modal("show");
		$(selectors.modalTypeRadio).attr("name", "ds-type-radio");
	});
	$(selectors.modal).on('shown.bs.modal', function () {
		initEditor(fieldId);
	});
	$(selectors.modal).on('hidden.bs.modal', function () {
		$(selectors.modalTypeRadio).removeAttr("name");
		destroyEditor(fieldId);
		FixModalInModalClose();
	});

	$(selectors.modalTypeRadio).change(function () {
		var typeVal = $(selectors.modalTypeRadio + ":checked").val();
		if (typeVal === 0 || typeVal === "0") {
			$(selectors.codeFormGroup).addClass("d-none");
			$(selectors.htmlFormGroup).addClass("d-none");
			$(selectors.dataSourceFormGroup).removeClass("d-none");
			$(selectors.defaultFormGroup).removeClass("d-none");
			$(selectors.testBtn).addClass("d-none");
			$(selectors.snippetFormGroup).addClass("d-none");
			//destroyEditor(fieldId);
		}
		else if (typeVal === 1 || typeVal === "1") {
			$(selectors.dataSourceFormGroup).addClass("d-none");
			$(selectors.htmlFormGroup).addClass("d-none");
			$(selectors.codeFormGroup).removeClass("d-none");
			$(selectors.defaultFormGroup).removeClass("d-none");
			$(selectors.testBtn).removeClass("d-none");
			$(selectors.snippetFormGroup).addClass("d-none");
			//initEditor(fieldId);
		}
		else if (typeVal === 3 || typeVal === "3") {
			$(selectors.dataSourceFormGroup).addClass("d-none");
			$(selectors.htmlFormGroup).addClass("d-none");
			$(selectors.codeFormGroup).addClass("d-none");
			$(selectors.defaultFormGroup).addClass("d-none");
			$(selectors.testBtn).addClass("d-none");
			$(selectors.snippetFormGroup).removeClass("d-none");
			//initEditor(fieldId);
		}
		else {
			$(selectors.dataSourceFormGroup).addClass("d-none");
			$(selectors.codeFormGroup).addClass("d-none");
			$(selectors.defaultFormGroup).addClass("d-none");
			$(selectors.htmlFormGroup).removeClass("d-none");
			$(selectors.testBtn).addClass("d-none");
			$(selectors.snippetFormGroup).addClass("d-none");
			//initEditor(fieldId);
		}
	});

	$(selectors.testBtn).click(function () {
		var editor = ace.edit(selectors.codeValueEditor.replace("#", ""));
		var submitObject = {};
		submitObject.csCode = editor.getValue();
		$.ajax({
			headers: {
				'Accept': 'application/json',
				'Content-Type': 'application/json'
			},
			url: "/api/v3.0/datasource/code-compile",
			type: 'POST',
			data: JSON.stringify(submitObject),
			success: function (response) {
				if (response.success) {
					toastr.success("Code successfully compiled!");
				}
				else {
					toastr.error(response.message, { timeOut: 10000 });
				}
			},
			error: function (jqXHR, textStatus, errorThrown) {
				var response = {};
				response.message = "";
				if (jqXHR && jqXHR.responseJSON) {
					response = jqXHR.responseJSON;
				}
				toastr.error(response.message);
			}
		});
	});

	//#region << Templates >>
	var DataSourceFormSimpleCode = "using System;\n";
	DataSourceFormSimpleCode += "using System.Collections.Generic;\n";
	DataSourceFormSimpleCode += "using WebVella.Erp.Web.Models;\n";
	DataSourceFormSimpleCode += "using WebVella.Erp.Api.Models;\n";
    DataSourceFormSimpleCode += "using Newtonsoft.Json;\n";
	DataSourceFormSimpleCode += "\n";
	DataSourceFormSimpleCode += "public class SampleCodeVariable : ICodeVariable\n";
	DataSourceFormSimpleCode += "{\n";
	DataSourceFormSimpleCode += "\tpublic object Evaluate(BaseErpPageModel pageModel)\n";
	DataSourceFormSimpleCode += "\t{\n";
	DataSourceFormSimpleCode += "\t\ttry{\n";
	DataSourceFormSimpleCode += "\t\t\treturn DateTime.Now;\n";
	DataSourceFormSimpleCode += "\t\t}\n";
	DataSourceFormSimpleCode += "\t\tcatch(Exception ex){\n";
	DataSourceFormSimpleCode += "\t\t\treturn \"Error: \" +  ex.Message;\n";
	DataSourceFormSimpleCode += "\t\t}\n";
	DataSourceFormSimpleCode += "\t}\n";
	DataSourceFormSimpleCode += "}\n";


	var DataSourceFormGetDatasourceCode = "using System;\n";
	DataSourceFormGetDatasourceCode += "using System.Collections.Generic;\n";
	DataSourceFormGetDatasourceCode += "using WebVella.Erp.Web.Models;\n";
	DataSourceFormGetDatasourceCode += "using WebVella.Erp.Api.Models;\n";
    DataSourceFormGetDatasourceCode += "using Newtonsoft.Json;\n";
	DataSourceFormGetDatasourceCode += "\n";
	DataSourceFormGetDatasourceCode += "public class GetDatasourceValueCodeVariable : ICodeVariable\n";
	DataSourceFormGetDatasourceCode += "{\n";
	DataSourceFormGetDatasourceCode += "\tpublic object Evaluate(BaseErpPageModel pageModel)\n";
	DataSourceFormGetDatasourceCode += "\t{\n";
	DataSourceFormGetDatasourceCode += "\t\ttry{\n";
	DataSourceFormGetDatasourceCode += "\t\t\t//replace constants with your values\n";
	DataSourceFormGetDatasourceCode += "\t\t\tconst string DATASOURCE_NAME = \"DATASOURCE_NAME_HERE\";\n";
	DataSourceFormGetDatasourceCode += "\t\n";
	DataSourceFormGetDatasourceCode += "\t\t\t//if pageModel is not provided, returns empty List<EntityRecordList>()\n";
	DataSourceFormGetDatasourceCode += "\t\t\tif (pageModel == null)\n";
	DataSourceFormGetDatasourceCode += "\t\t\t\treturn null;\n";
	DataSourceFormGetDatasourceCode += "\t\n";
	DataSourceFormGetDatasourceCode += "\t\t\t//try read data source by name and get result as specified type object\n";
	DataSourceFormGetDatasourceCode += "\t\t\tvar dataSource = pageModel.TryGetDataSourceProperty<EntityRecordList>(DATASOURCE_NAME);\n";
	DataSourceFormGetDatasourceCode += "\t\n";
	DataSourceFormGetDatasourceCode += "\t\t\t//if data source not found or different type, return empty List<EntityRecordList>()\n";
	DataSourceFormGetDatasourceCode += "\t\t\tif (dataSource == null)\n";
	DataSourceFormGetDatasourceCode += "\t\t\t\treturn null;\n";
	DataSourceFormGetDatasourceCode += "\t\n";
	DataSourceFormGetDatasourceCode += "\t\t\treturn dataSource;\n";
	DataSourceFormGetDatasourceCode += "\t\t}\n";
	DataSourceFormGetDatasourceCode += "\t\tcatch(Exception ex){\n";
	DataSourceFormGetDatasourceCode += "\t\t\treturn \"Error: \" + ex.Message;\n";
	DataSourceFormGetDatasourceCode += "\t\t}\n";
	DataSourceFormGetDatasourceCode += "\t}\n";
	DataSourceFormGetDatasourceCode += "}\n";

	var DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode = "using System;\n";
	DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "using System.Collections.Generic;\n";
	DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "using WebVella.Erp.Web.Models;\n";
	DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "using WebVella.Erp.Api.Models;\n";
    DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "using Newtonsoft.Json;\n";
	DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "\n";
	DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "public class SelectOptionsConvertCodeVariable : ICodeVariable\n";
	DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "{\n";
	DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "\tpublic object Evaluate(BaseErpPageModel pageModel)\n";
	DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "\t{\n";
	DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "\t\ttry{\n";
	DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "\t\t\t//replace constants with your values\n";
	DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "\t\t\tconst string DATASOURCE_NAME = \"DATASOURCE_NAME_HERE\";\n";
	DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "\t\t\tconst string KEY_FIELD_NAME = \"KEY_FIELD_NAME_HERE\";\n";
	DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "\t\t\tconst string VALUE_FIELD_NAME = \"VALUE_FIELD_NAME_HERE\";\n";
	DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "\t\n";
	DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "\t\t\tvar result = new List<SelectOption>();\n";
	DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "\t\n";
	DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "\t\t\t//if pageModel is not provided, returns empty List<EntityRecordList>()\n";
	DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "\t\t\tif (pageModel == null)\n";
	DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "\t\t\t\treturn result;\n";
	DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "\t\n";
	DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "\t\t\t//try read data source by name and get result as specified type object\n";
	DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "\t\t\tvar dataSource = pageModel.TryGetDataSourceProperty<EntityRecordList>(DATASOURCE_NAME);\n";
	DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "\t\n";
    DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "\t\t\t//if data source not found or different type, return empty List<EntityRecordList>()\n";
	DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "\t\t\tif (dataSource == null)\n";
	DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "\t\t\t\treturn result;\n";
	DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "\t\n";
	DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "\t\t\t//initialize SelectOptions by reading key and value from appropriate entity record fields\n";
	DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "\t\t\tforeach (var record in dataSource)\n";
	DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "\t\t\t\tresult.Add(new SelectOption(((Guid)record[KEY_FIELD_NAME]).ToString(), (string)record[VALUE_FIELD_NAME]));\n";
	DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "\t\t\treturn result;\n";
	DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "\t\t}\n";
	DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "\t\tcatch(Exception ex){\n";
	DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "\t\t\treturn  \"Error: \" + ex.Message;\n";
	DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "\t\t}\n";
	DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "\t}\n";
	DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode += "}\n";
	//#endregion

	$(selectors.pasteSimpleCodeLink).click(function () {
		var editor = ace.edit(selectors.codeValueEditor.replace("#", ""));
		editor.session.setValue(DataSourceFormSimpleCode);
	});

	$(selectors.pasteDataSourceCodeLink).click(function () {
		var editor = ace.edit(selectors.codeValueEditor.replace("#", ""));
		editor.session.setValue(DataSourceFormGetDatasourceCode);
	});

	$(selectors.pasteDataSourceTransformationCodeLink).click(function () {
		var editor = ace.edit(selectors.codeValueEditor.replace("#", ""));
		editor.session.setValue(DataSourceFormGetDatasourceEntityRecordToSelectOptionsCode);
	});
}

function submitOption(fieldId) {

	var selectors = DataSourceFormEditGenerateSelectors(fieldId);
	var optionObj = { "type": 0, "string": "", "default": "" };

	//Type
	var typeVal = $(selectors.modalTypeRadio + ":checked").val();
	optionObj.type = typeVal;
	//Default Value
	var defaultVal = $(selectors.defaultFormGroup).find("input.form-control").val();
	optionObj.default = defaultVal;
	//Value
	if (typeVal === 0 || typeVal === "0") {
		var inputVal = $(selectors.dataSourceFormGroup).find("input.form-control").val();
		optionObj.string = inputVal;
		$(selectors.dataSourceInput).removeClass("code").removeClass("html").addClass("datasource").removeClass("snippet");
		$(selectors.dataSourceInput).find(".select2-selection__choice").attr("title", inputVal);
		$(selectors.dataSourceInput).find(".select2-selection__choice").find("span").html(inputVal);
		$(selectors.dataSourceInput).find(".select2-selection__choice").find(".fa").removeClass("fa-code").addClass("fa-link").removeClass("fa-cog");
	}
	else if (typeVal === 1 || typeVal === "1") {
		var editor = ace.edit(selectors.codeValueEditor.replace("#", ""));
		optionObj.string = editor.getValue();
		//optionObj.string = $(selectors.codeValueInput).val();
		$(selectors.dataSourceInput).removeClass("datasource").removeClass("html").addClass("code").removeClass("snippet");
		$(selectors.dataSourceInput).find(".select2-selection__choice").find("span").html("c# code");
		$(selectors.dataSourceInput).find(".select2-selection__choice").find(".fa").removeClass("fa-link").addClass("fa-code").removeClass("fa-cog");
	}
	else if (typeVal === 3 || typeVal === "3") {
		optionObj.string = $(selectors.snippetFormGroup).find("input.form-control").val();
		$(selectors.dataSourceInput).removeClass("datasource").removeClass("html").removeClass("code").addClass("snippet");
		$(selectors.dataSourceInput).find(".select2-selection__choice").find("span").html(optionObj.string);
		$(selectors.dataSourceInput).find(".select2-selection__choice").find(".fa").removeClass("fa-link").removeClass("fa-code").addClass("fa-cog");
	}
	else {
		var editorHtml = ace.edit(selectors.htmlValueEditor.replace("#", ""));
		optionObj.string = editorHtml.getValue();
		//optionObj.string = $(selectors.codeValueInput).val();
		$(selectors.dataSourceInput).removeClass("datasource").removeClass("code").addClass("html").removeClass("snippet");
		$(selectors.dataSourceInput).find(".select2-selection__choice").find("span").html("html");
		$(selectors.dataSourceInput).find(".select2-selection__choice").find(".fa").removeClass("fa-link").addClass("fa-code").removeClass("fa-cog");
	}
	$(selectors.formControlInput).val(JSON.stringify(optionObj));
	$(selectors.formControlInput).closest(".input-group").addClass("d-none");
	$(selectors.dataSourceInput).closest(".input-group").removeClass("d-none");
	$(selectors.modal).appendTo($(selectors.formControlInput).closest(".form-group"));
	$(selectors.modal).modal("hide");

}
