var CkEditorImageBrowser = {};

CkEditorImageBrowser.folders = [];
CkEditorImageBrowser.images = {}; //folder => list of images
CkEditorImageBrowser.ckFunctionNum = null;

CkEditorImageBrowser.$folderSwitcher = null;
CkEditorImageBrowser.$imagesContainer = null;

CkEditorImageBrowser.init = function () {
	CkEditorImageBrowser.$folderSwitcher = $('#js-folder-switcher');
	CkEditorImageBrowser.$imagesContainer = $('#js-images-container');

	var baseHref = CkEditorImageBrowser.getQueryStringParam("baseHref");
	if (baseHref) {
		var h = (document.head || document.getElementsByTagName("head")[0]),
			el = h.getElementsByTagName("link")[0];
		el.href = location.href.replace(/\/[^\/]*$/,"/browser.css");
		(h.getElementsByTagName("base")[0]).href = baseHref;
	}

	CkEditorImageBrowser.ckFunctionNum = CkEditorImageBrowser.getQueryStringParam('CKEditorFuncNum');

	CkEditorImageBrowser.initEventHandlers();

	CkEditorImageBrowser.loadData(CkEditorImageBrowser.getQueryStringParam('listUrl'), function () {
		CkEditorImageBrowser.initFolderSwitcher();
	});
};

CkEditorImageBrowser.loadData = function (url, onLoaded) {
	CkEditorImageBrowser.folders = [];
	CkEditorImageBrowser.images = {};

	$.getJSON(url, function (list) {
		$.each(list, function (_idx, item) {
			if (typeof(item.folder) === 'undefined') {
				item.folder = 'Images';
			}

			if (typeof(item.thumb) === 'undefined') {
				item.thumb = item.image;
			}

			CkEditorImageBrowser.addImage(item.folder, item.image, item.thumb);
		});

		onLoaded();
	}).error(function(jqXHR, textStatus, errorThrown) {
		var errorMessage;
		if (jqXHR.status < 200 || jqXHR.status >= 400) {
			errorMessage = 'HTTP Status: ' + jqXHR.status + '/' + jqXHR.statusText + ': "<strong style="color: red;">' + url + '</strong>"';
		} else if (textStatus === 'parsererror') {
			errorMessage = textStatus + ': invalid JSON file: "<strong style="color: red;">' + url + '</strong>": ' + errorThrown.message;
		} else {
			errorMessage = textStatus + ' / ' + jqXHR.statusText + ' / ' + errorThrown.message;
		}
		CkEditorImageBrowser.$imagesContainer.html(errorMessage);
    });
};

CkEditorImageBrowser.addImage = function (folderName, imageUrl, thumbUrl) {
	if (typeof(CkEditorImageBrowser.images[folderName]) === 'undefined') {
		CkEditorImageBrowser.folders.push(folderName);
		CkEditorImageBrowser.images[folderName] = [];
	}

	CkEditorImageBrowser.images[folderName].push({
		"imageUrl": imageUrl,
		"thumbUrl": thumbUrl
	});
};

CkEditorImageBrowser.initFolderSwitcher = function () {
	var $switcher = CkEditorImageBrowser.$folderSwitcher;

	$switcher.find('li').remove();

	$.each(CkEditorImageBrowser.folders, function (idx, folderName) {
		var $option = $('<li></li>').data('idx', idx).text(folderName);
		$option.appendTo($switcher);
	});


	if (CkEditorImageBrowser.folders.length === 0) {
		$switcher.remove();
		CkEditorImageBrowser.$imagesContainer.text('No images.');
	} else {
		if (CkEditorImageBrowser.folders.length === 1) {
			$switcher.hide();
		}

		$switcher.find('li:first').click();
	}
};

CkEditorImageBrowser.renderImagesForFolder = function (folderName) {
	var images = CkEditorImageBrowser.images[folderName],
		templateHtml = $('#js-template-image').html();

	CkEditorImageBrowser.$imagesContainer.html('');

	$.each(images, function (_idx, imageData) {
		var html = templateHtml;
		html = html.replace('%imageUrl%', imageData.imageUrl);
		html = html.replace('%thumbUrl%', imageData.thumbUrl);

		var $item = $($.parseHTML(html));

		CkEditorImageBrowser.$imagesContainer.append($item);
	});
};

CkEditorImageBrowser.initEventHandlers = function () {
	$(document).on('click', '#js-folder-switcher li', function () {
		var idx = parseInt($(this).data('idx'), 10),
			folderName = CkEditorImageBrowser.folders[idx];

		$(this).siblings('li').removeClass('active');
		$(this).addClass('active');

		CkEditorImageBrowser.renderImagesForFolder(folderName);
	});

	$(document).on('click', '.js-image-link', function () {
		window.opener.CKEDITOR.tools.callFunction(CkEditorImageBrowser.ckFunctionNum, $(this).data('url'));
		window.close();
	});
};

CkEditorImageBrowser.getQueryStringParam = function (name) {
	var regex = new RegExp('[?&]' + name + '=([^&]*)'),
		result = window.location.search.match(regex);

	return (result && result.length > 1 ? decodeURIComponent(result[1]) : null);
};
