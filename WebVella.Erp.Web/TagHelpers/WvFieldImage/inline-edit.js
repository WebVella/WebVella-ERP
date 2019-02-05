
function ImageInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config) {
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

function ImageInlineEditInit(fieldId, fieldName, entityName, recordId, config) {
	config = ProcessConfig(config);
	var selectors = ImageInlineEditGenerateSelectors(fieldId, fieldName, config);
	//Remove value
	$(selectors.removeValueEl).first().on('click', function (e) {
		var result = confirm("Confirm image deletion?");
		if (result) {
			var response = {};
			response.object = {};
			response.object.url = null;
			response.object.filename = "";
			ImageInlineEditUploadSuccessCallback(response, fieldId, fieldName, entityName, recordId, config)
		}
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
						ImageInlineEditUploadSuccessCallback(result, fieldId, fieldName, entityName, recordId, config)
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

function ImageInlineEditUploadSuccessCallback(response, fieldId, fieldName, entityName, recordId, config) {
	var selectors = ImageInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
	var newValue = response.object.url;
	var newFilename = response.object.filename;
	var oldValue = $(selectors.inputEl).attr("data-original-value");
	if (newValue !== oldValue) {
		var submitObj = {};
		submitObj.id = recordId;
		if (isStringNullOrEmptyOrWhiteSpace(newValue)) {
			submitObj[fieldName] = null;
		}
		else {
			submitObj[fieldName] = newValue;
		}
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
			success: function (result) {
				if (result.success) {
					result.filename = response.filename;
					ImageInlineEditInitSuccessCallback(result, fieldId, fieldName, entityName, recordId, config);
				}
				else {
					ImageInlineEditInitErrorCallback(result, fieldId, fieldName, entityName, recordId, config);
				}
			},
			error: function (jqXHR, textStatus, errorThrown) {
				var result = {};
				response.message = "";
				if (jqXHR && jqXHR.responseJSON) {
					response = jqXHR.responseJSON;
				}
				ImageInlineEditInitErrorCallback(result, fieldId, fieldName, entityName, recordId, config);
			}
		});
	}
}


function ImageInlineEditInitSuccessCallback(response, fieldId, fieldName, entityName, recordId, config) {
	var selectors = ImageInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
	var newValue = ProcessNewValue(response, fieldName);
	var newFilename = response.filename;//injected from previous method
	if (newValue && newValue !== "") {
		$(selectors.inputEl).val(newValue);
		var oldImageSrc = $(selectors.viewWrapperImage).first().attr("src");
		var oldImgQuery = URI(oldImageSrc).query();
		if (oldImgQuery && oldImgQuery !== "") {
			$(selectors.viewWrapperImage).first().attr("src", "/fs" + newValue + "?" + oldImgQuery);//.attr("title", result.object.filename); removed as it does not work on patch record
		}
		else {
			$(selectors.viewWrapperImage).first().attr("src", "/fs" + newValue);//.attr("title", result.object.filename); removed as it does not work on patch record
		}
		$(selectors.editWrapper).addClass("d-none");
		$(selectors.viewWrapper).removeClass("d-none");
	}
	else {
		$(selectors.inputEl).val("");
		$(selectors.fileUploadEl).val("");
		$(selectors.editWrapperLink).first().removeClass("d-none");
		$(selectors.editWrapperText).first().addClass("d-none");
		$(selectors.viewWrapper).addClass("d-none");
		$(selectors.editWrapper).removeClass("d-none");
	}
	toastr.success("The new value is successfull saved", 'Success!', { closeButton: true, tapToDismiss: true });
}

function ImageInlineEditInitErrorCallback(response, fieldId, fieldName, entityName, recordId, config) {
	var selectors = ImageInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
	$(selectors.editWrapperText).first().html("<i class='fa fa-exclamation-circle go-red'></i> Error");
	var errorMessage = response.message;
	if (!errorMessage && response.errors && response.errors.length > 0) {
		errorMessage = response.errors[0].message;
	}
		
	$(selectors.editWrapper + " .input-group").after("<div class='invalid-feedback'>" + errorMessage + "</div>");
	$(selectors.editWrapper).closest(".erp-field").find(".invalid-feedback").first().show();
	toastr.error("An error occurred", 'Error!', { closeButton: true, tapToDismiss: true });
	console.log("error", response);
}
