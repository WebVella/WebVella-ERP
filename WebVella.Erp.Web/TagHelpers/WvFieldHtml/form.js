var htmlFieldModalOptions = {
	backdrop: "static"
};


function InitHtmlFieldCKEditor(fieldId, fieldConfig) { //modes are -> none, one-repository,user-repository
	fieldConfig = ProcessConfig(fieldConfig);
	var ckInstance = CKEDITOR.instances['input-' + fieldId];
	if (ckInstance) {
		try {
			ckInstance.removeAllListeners();
			CKEDITOR.remove(ckInstance);
		}
		catch (e) {
			var l = e;
		}
	}

	var config = {};
	config.language = SiteLang;
	config.skin = 'moono-lisa';
	//config.contentsCss = '/css/editor.css';
	config.autoGrow_minHeight = 160;
	config.autoGrow_maxHeight = 600;
	config.autoGrow_bottomSpace = 10;
	config.autoGrow_onStartup = true;
	config.allowedContent = true;
	config.autoParagraph = false;
	config.toolbarLocation = 'top';
	var extraPluginsArray = ['divarea'];
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


	var editor = CKEDITOR.replace('input-' + fieldId, config);
	editor.on('change', function () {
		editor.updateElement();
		var customEvent = new Event('WvFieldHtml_Change');
		var inputElement = document.getElementById('input-' + fieldId);
		customEvent.payload = {
			value: editor.getData(),
			fieldId: fieldId,
			fieldName:inputElement.name
		};
		document.dispatchEvent(customEvent);
	});
	editor.on('blur', function () {
		editor.updateElement();
		var customEvent = new Event('WvFieldHtml_Blur');
		var inputElement = document.getElementById('input-' + fieldId);
		customEvent.payload = {
			value: editor.getData(),
			fieldId: fieldId,
			fieldName:inputElement.name
		};
		document.dispatchEvent(customEvent);

	});
}
