function ImageFormGenerateSelectors(fieldId, fieldName, config) {
	//Method for generating selector strings of some of the presentation elements
	var selectors = {};
	selectors.viewWrapper = "#view-" + fieldId;
	selectors.editWrapper = "#edit-" + fieldId;
	selectors.inputEl = "#input-" + fieldId;
	selectors.fileUploadEl = "#file-" + fieldId;
	selectors.viewWrapperImage = selectors.viewWrapper + " .wrapper-image";
	selectors.editWrapperLink = selectors.editWrapper + " .wrapper-text a";
	selectors.editWrapperText = selectors.editWrapper + " .wrapper-text span";
	selectors.editValueEl = selectors.viewWrapper + " .edit.action";
	selectors.removeValueEl = selectors.viewWrapper + " .remove.action";
	return selectors;
}

function ImageFormInit(fieldId, fieldName, config) {
	config = ProcessConfig(config);
	var selectors = ImageFormGenerateSelectors(fieldId, fieldName, config);
	//Remove value
	$(selectors.removeValueEl).first().on('click', function (e) {
		$(selectors.inputEl).val("");
		$(selectors.fileUploadEl).val("");
		$(selectors.editWrapperLink).first().removeClass("d-none");
		$(selectors.editWrapperText).first().addClass("d-none");
		$(selectors.viewWrapper).addClass("d-none");
		$(selectors.editWrapper).removeClass("d-none");
	});

	//Edit value
	$(selectors.editValueEl).first().on('click', function (e) {
		$(selectors.fileUploadEl).click();
	});

	$(selectors.fileUploadEl).first().on('change', function (e) {
		$(selectors.editWrapperLink).first().addClass("d-none");
		$(selectors.editWrapperText).first().text("Sending to server ...").removeClass("d-none");
		var files = e.target.files;
		if (files.length > 0) {
			if (window.FormData !== undefined) {
				var data = new FormData();
				//support only single file upload
				data.append("file", files[0]);
				$.ajax({
					type: "POST",
					url: '/fs/upload',
					contentType: false,
					processData: false,
					data: data,
					xhr: function () {
						// get the native XmlHttpRequest object
						var xhr = $.ajaxSettings.xhr();
						// set the onprogress event handler
						xhr.upload.onprogress = function (evt) {
							var progressPercent = evt.loaded / evt.total * 100;
							$(selectors.editWrapperText).first().text("Sending to server ..." + " " + progressPercent + "%");
						};
						// set the onload event handler
						xhr.upload.onload = function () {
							$(selectors.editWrapperText).first().html("<i class='fa fa-spin fa-spinner go-blue'></i> Sent, processing ...");
						};
						// return the customized object
						return xhr;
					},
					success: function (result) {
						$(selectors.inputEl).val(result.object.url);
						var oldImageSrc = $(selectors.viewWrapperImage).first().attr("src");
						var oldImgQuery = URI(oldImageSrc).query();
						if (oldImgQuery && oldImgQuery !== "") {
							$(selectors.viewWrapperImage).first().attr("src", "/fs" + result.object.url + "?" + oldImgQuery).attr("title", result.object.filename);
						}
						else {
							$(selectors.viewWrapperImage).first().attr("src", "/fs" + result.object.url).attr("title", result.object.filename);
						}

						$(selectors.editWrapper).addClass("d-none");
						$(selectors.viewWrapper).removeClass("d-none");
					},
					error: function (xhr, status, p3, p4) {
						var err = "Error " + " " + status + " " + p3 + " " + p4;
						if (xhr.responseText && xhr.responseText[0] === "{")
							err = JSON.parse(xhr.responseText).Message;
						$(selectors.editWrapperText).first().html("<i class='fa fa-exclamation-circle go-red'></i> Error");
						$(selectors.editWrapper).after("<div class='invalid-feedback'>" + response.message + "</div>");
						$(selectors.editWrapper).closest(".erp-field").find(".invalid-feedback").first().show();
						toastr.error("An error occurred", 'Error!', { closeButton: true, tapToDismiss: true });
						console.log(err);
					}
				});
			} else {
				alert("This browser doesn't support HTML5 file uploads!");
			}
		}
	});
}
