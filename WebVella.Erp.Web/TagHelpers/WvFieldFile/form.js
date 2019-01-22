function FileFormGenerateSelectors(fieldId, fieldName, config) {
	//Method for generating selector strings of some of the presentation elements
	var selectors = {};
	selectors.inputEl = "#input-" + fieldId;
	selectors.fileUploadEl = "#file-" + fieldId;
	selectors.fakeInputEl = "#fake-" + fieldId;
	selectors.fakeInputLinkEl = "#fake-" + fieldId + " a";
	selectors.fakeInputProgressEl = "#fake-" + fieldId + " .form-control-progress";
	selectors.removeValueEl = "#remove-" + fieldId;
	return selectors;
}

function FileFormInit(fieldId, fieldName, config) {
	config = ProcessConfig(config);
	var selectors = FileFormGenerateSelectors(fieldId, fieldName, config);
	//Remove value
	$(selectors.removeValueEl).first().on('click', function (e) {
		$(selectors.fakeInputLinkEl).attr("href", "").attr("title", "").text();
		$(selectors.fakeInputLinkEl).hide();
		$(selectors.fakeInputEl).closest(".input-group").find(".icon-addon").addClass("d-none");
		$(selectors.fakeInputEl).closest(".input-group").addClass("left-border");
		$(selectors.fakeInputEl).closest(".input-group").find(".input-group-append .remove").addClass("d-none");
		$(selectors.inputEl).first().val("");
		$(selectors.fileUploadEl).first().val("");
	});

	$(selectors.fileUploadEl).first().on('change', function (e) {
		$(selectors.fakeInputLinkEl).hide();
		$(selectors.fakeInputProgressEl).first().text("Sending to server ...");
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
							$(selectors.fakeInputProgressEl).first().attr("style", "display:block;width:" + progressPercent + "%")
						};
						// set the onload event handler
						xhr.upload.onload = function () {
							$(selectors.fakeInputProgressEl).first().html("<i class='fa fa-spin fa-spinner go-blue'></i> Sent, processing ...");
						};
						// return the customized object
						return xhr;
					},
					success: function (result) {
						$(selectors.fakeInputLinkEl).first().text(result.object.filename).attr("href", "/fs" + result.object.url).attr("title", "/fs" + result.object.url);
						$(selectors.fakeInputProgressEl).first().attr("style", "display:none;width:0%")
						$(selectors.fakeInputLinkEl).show();
						var typeIconClass = GetPathTypeIcon(result.object.filename);
						$(selectors.fakeInputEl).closest(".input-group").find(".type-icon").first().attr("class", "fa fa-fw type-icon " + typeIconClass);
						//Show the input-group-addon if needed
						$(selectors.fakeInputEl).closest(".input-group").find(".icon-addon").removeClass("d-none");
						$(selectors.fakeInputEl).closest(".input-group").removeClass("left-border");
						$(selectors.fakeInputEl).closest(".input-group").find(".input-group-append .remove").removeClass("d-none");
						$(selectors.inputEl).val(result.object.url);
					},
					error: function (xhr, status, p3, p4) {
						var err = "Error " + " " + status + " " + p3 + " " + p4;
						if (xhr.responseText && xhr.responseText[0] === "{")
							err = JSON.parse(xhr.responseText).Message;
						$(selectors.fakeInputEl).addClass("is-invalid");
						$(selectors.fakeInputEl).closest(".input-group").after("<div class='invalid-feedback'>" + response.message + "</div>");
						$(selectors.fakeInputEl).closest(".erp-field").find(".invalid-feedback").first().show();
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
