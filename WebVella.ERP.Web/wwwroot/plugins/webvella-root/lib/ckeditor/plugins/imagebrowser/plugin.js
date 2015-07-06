CKEDITOR.plugins.add('imagebrowser', {
	"init": function (editor) {
		if (typeof(editor.config.imageBrowser_listUrl) === 'undefined' || editor.config.imageBrowser_listUrl === null) {
			return;
		}

		var url = editor.plugins.imagebrowser.path + "browser/browser.html?listUrl=" + encodeURIComponent(editor.config.imageBrowser_listUrl);
		if (editor.config.baseHref) {
			url += "&baseHref=" + encodeURIComponent(editor.config.baseHref);
		}

		editor.config.filebrowserImageBrowseUrl = url;
	}
});
