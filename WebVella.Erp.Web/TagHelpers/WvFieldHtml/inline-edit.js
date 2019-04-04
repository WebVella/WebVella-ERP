var htmlFieldModalOptions = {
	backdrop: "static"
};

function InitHtmlFieldCKEditor(fieldId, fieldConfig) { //modes are -> none, one-repository,user-repository
	fieldConfig = ProcessConfig(fieldConfig);
	var editor = CKEDITOR.instances['input-' + fieldId];
	if (editor) {
		editor.destroy(true);
	}
	var config = {};
	config.language = SiteLang;
	config.skin = 'moono-lisa';
	//config.height = '160';
	config.contentsCss = '/css/editor.css';
	config.autoGrow_minHeight = 160;
	config.autoGrow_maxHeight = 600;
	config.autoGrow_bottomSpace = 10;
	config.autoGrow_onStartup = true;
	config.allowedContent = true;
	config.autoParagraph = false;
	config.toolbarLocation = 'top';
	config.keystrokes = [[ CKEDITOR.CTRL + 13, 'save' ]];
	var extraPluginsArray = [];
	var removePluginsArray = [];
	switch (fieldConfig.toolbar_mode) {
		default: //Basic
			extraPluginsArray.push("panel");
			extraPluginsArray.push("autogrow");
			config.toolbar = 'full';
			config.toolbar_full = [
				{ name: 'basicstyles', items: ['Bold', 'Italic'] },
				{ name: 'paragraph', items: ['NumberedList', 'BulletedList'] },
				{ name: 'indent', items: ['Indent', 'Outdent'] },
				{ name: 'links', items: ['Link', 'Unlink'] },
				{ name: 'pasting', items: ['PasteText', 'PasteFromWord'] },
			]
			break;
		case 2: //Standard
			extraPluginsArray.push("colorbutton");
			extraPluginsArray.push("colordialog");
			extraPluginsArray.push("panel");
			extraPluginsArray.push("font");
			extraPluginsArray.push("autogrow");
			config.colorButton_colors = '333333,FFFFFF,F44336,E91E63,9C27B0,673AB7,3F51B5,2196F3,03A9F4,00BCD4,009688,4CAF50,8BC34A,CDDC39,FFEB3B,FFC107,FF9800,FF5722,795548,607D8B,999999';
			config.colorButton_enableAutomatic = false;
			config.colorButton_enableMore = false;
			config.toolbar = 'full';
			config.toolbar_full = [
				{ name: 'basicstyles', items: ['Bold', 'Italic', 'Strike', 'Underline'] },
				{ name: 'colors', items: ['TextColor', 'BGColor'] },
				{ name: 'styles', items: ['FontSize', 'RemoveFormat'] },
				{ name: 'editing', items: ['Format'] },
				{ name: 'links', items: ['Link', 'Unlink'] },
				{ name: 'pasting', items: ['PasteText', 'PasteFromWord'] },
				{ name: 'paragraph', items: ['BulletedList', 'NumberedList', 'Blockquote'] },
				{ name: 'insert', items: ['Image', 'Table'] },
			]
			break;
		case 3: //Full
			extraPluginsArray.push("sourcedialog");
			extraPluginsArray.push("colorbutton");
			extraPluginsArray.push("colordialog");
			extraPluginsArray.push("panel");
			extraPluginsArray.push("font");
			extraPluginsArray.push("autogrow");
			config.colorButton_colors = '333333,FFFFFF,F44336,E91E63,9C27B0,673AB7,3F51B5,2196F3,03A9F4,00BCD4,009688,4CAF50,8BC34A,CDDC39,FFEB3B,FFC107,FF9800,FF5722,795548,607D8B,999999';
			config.colorButton_enableAutomatic = false;
			config.colorButton_enableMore = false;
			config.toolbar = 'full';
			config.toolbar_full = [
				{ name: 'basicstyles', items: ['Bold', 'Italic', 'Strike', 'Underline'] },
				{ name: 'colors', items: ['TextColor', 'BGColor'] },
				{ name: 'styles', items: ['FontSize', 'RemoveFormat'] },
				{ name: 'editing', items: ['Format'] },
				{ name: 'links', items: ['Link', 'Unlink'] },
				{ name: 'pasting', items: ['PasteText', 'PasteFromWord'] },
				{ name: 'paragraph', items: ['BulletedList', 'NumberedList', 'Blockquote'] },
				{ name: 'insert', items: ['Image', 'Table', 'SpecialChar'] },
				{ name: 'tools', items: ['Sourcedialog', 'Maximize'] },
			]
			break;
	}
	switch (fieldConfig.upload_mode) {
		default: //None
			removePluginsArray.push("uploadimage");
			removePluginsArray.push("uploadfile");
			break;
		case 2: //SiteRepository
			config.filebrowserImageBrowseUrl = '/ckeditor/ImageFinder';
			config.filebrowserImageUploadUrl = '/ckeditor/image-upload-url';
			config.uploadUrl = '/ckeditor/drop-upload-url';
			extraPluginsArray.push("uploadimage");
			break;
	}

	if (extraPluginsArray.length > 0) {
		config.extraPlugins = _.join(extraPluginsArray, ",");
	}

	if (removePluginsArray.length > 0) {
		config.removePlugins = _.join(removePluginsArray, ",");
	}

	editor = CKEDITOR.replace('input-' + fieldId, config);
	editor.on('change', function () {
		editor.updateElement();
	});
}


function HtmlInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config) {
	//Method for generating selector strings of some of the presentation elements
	var selectors = {};
	selectors.viewWrapper = "#view-" + fieldId;
	selectors.editWrapper = "#edit-" + fieldId;
	return selectors;
}

function HtmlInlineEditPreEnableCallback(fieldId, fieldName, entityName, recordId, config) {
	var selectors = HtmlInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
	//$(selectors.viewWrapper).hide();
	$(selectors.editWrapper).on('shown.bs.modal', function () {
		setTimeout(function () {
			InitHtmlFieldCKEditor(fieldId, config);
		}, 1);
	});
	//Enable default close functions to disable the inline edit properly;
	$(selectors.editWrapper).on('hidden.bs.modal', function () {
		HtmlInlineEditPreDisableCallback(fieldId, fieldName, entityName, recordId, config);
		FixModalInModalClose();
	});
	$(selectors.editWrapper).modal(htmlFieldModalOptions);
}

function HtmlInlineEditPreDisableCallback(fieldId, fieldName, entityName, recordId, config) {
	var selectors = HtmlInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
	$(selectors.editWrapper + " .invalid-feedback").remove();
	$(selectors.editWrapper + " .form-control").removeClass("is-invalid");
	$(selectors.editWrapper + " .save .fa").addClass("fa-check").removeClass("fa-spin fa-spinner");
	$(selectors.editWrapper + " .save").attr("disabled", false);
	//$(selectors.viewWrapper).show();
	$(selectors.editWrapper).modal('hide');
	//Destroy ckeditor
	var editor = CKEDITOR.instances['input-' + fieldId];
	if (editor) {
		editor.destroy(true);
	}
}

function HtmlInlineEditInit(fieldId, fieldName, entityName, recordId, config) {
	config = ProcessConfig(config);
	var selectors = HtmlInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
	//Init enable action click
	$(selectors.viewWrapper + " .action.btn").on("click", function (event) {
		event.stopPropagation();
		event.preventDefault();
		HtmlInlineEditPreEnableCallback(fieldId, fieldName, entityName, recordId, config);
	});
	//Init enable action dblclick
	$(selectors.viewWrapper + " .form-control").on("dblclick", function (event) {
		event.stopPropagation();
		event.preventDefault();
		HtmlInlineEditPreEnableCallback(fieldId, fieldName, entityName, recordId, config);
		//clearSelection();//double click causes text to be selected.
		setTimeout(function () {
			$(selectors.editWrapper + " .form-control").get(0).focus();
		}, 200);
	});
	//Disable inline edit action
	$(selectors.editWrapper + " .cancel").on("click", function (event) {
		event.stopPropagation();
		event.preventDefault();
		HtmlInlineEditPreDisableCallback(fieldId, fieldName, entityName, recordId, config);
	});
	//Save inline changes
	$(selectors.editWrapper + " .save").on("click", function (event) {
		event.stopPropagation();
		event.preventDefault();
		var inputValue = null;
		var editor = CKEDITOR.instances[fieldName + '-' + fieldId];
		if (editor) {
			inputValue = editor.getData();
		}
		else {
			inputValue = $("#edit-" + fieldId + " .form-control").val();
		}
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
					HtmlInlineEditInitSuccessCallback(response, fieldId, fieldName, entityName, recordId, config);
				}
				else {
					HtmlInlineEditInitErrorCallback(response, fieldId, fieldName, entityName, recordId, config);
				}
			},
			error: function (jqXHR, textStatus, errorThrown) {
				var response = {};
				response.message = "";
				if (jqXHR && jqXHR.responseJSON) {
					response = jqXHR.responseJSON;
				}
				HtmlInlineEditInitErrorCallback(response, fieldId, fieldName, entityName, recordId, config);
			}
		});
	});
}

function HtmlInlineEditInitSuccessCallback(response, fieldId, fieldName, entityName, recordId, config) {
	var selectors = HtmlInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
	var newValue = ProcessNewValue(response, fieldName);
	$(selectors.viewWrapper + " .form-control").html(newValue);
	$(selectors.editWrapper + " .form-control").val(newValue);
	HtmlInlineEditPreDisableCallback(fieldId, fieldName, entityName, recordId, config);
	toastr.success("The new value is successfull saved", 'Success!', { closeButton: true, tapToDismiss: true });
}

function HtmlInlineEditInitErrorCallback(response, fieldId, fieldName, entityName, recordId, config) {
	var selectors = HtmlInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
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
