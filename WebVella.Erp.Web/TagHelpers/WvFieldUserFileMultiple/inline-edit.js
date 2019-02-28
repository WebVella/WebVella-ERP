
function FileInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config) {
	//Method for generating selector strings of some of the presentation elements
	var selectors = {};
	selectors.viewWrapper = "#view-" + fieldId;
	selectors.editWrapper = "#edit-" + fieldId;
	selectors.inputEl = "#input-" + fieldId;
	selectors.fileUploadEl = "#file-" + fieldId;
	selectors.fakeInputEl = "#fake-" + fieldId;
	selectors.fakeInputLinkEl = "#fake-" + fieldId + " a";
	selectors.fakeInputProgressEl = "#fake-" + fieldId + " .form-control-progress";
	selectors.removeValueEl = "#remove-" + fieldId;
	return selectors;
}

function FileInlineEditPreEnableCallback(fieldId, fieldName, entityName, recordId, config) {
	var selectors = FileInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
	//Remove value
	$(selectors.removeValueEl).first().on('click', function (e) {
		$(selectors.fakeInputLinkEl).first().attr("href", "").attr("title", "").text("").addClass("d-none");
		$(selectors.editWrapper + " .icon-addon").first().addClass("d-none");
		$(selectors.editWrapper + " .input-group").first().addClass("left-border");
		$(selectors.editWrapper + " .input-group-append .remove").first().addClass("d-none");
		$(selectors.fileUploadEl).first().val("");
		$(selectors.inputEl).attr("data-newfilepath", "").attr("data-newfilename", ""); //Input element 'value' and 'data-filename' are updated only on save
	});
	//Prefill the date as it could be removed from the above method without saving. inputEl should always have the correct data
	var filePath = $(selectors.inputEl).first().val();
	var fileName = $(selectors.inputEl).first().attr("data-filename");
	if (!isStringNullOrEmptyOrWhiteSpace(filePath)) {
		var typeIconClass = GetPathTypeIcon(filePath);
		$(selectors.fakeInputLinkEl).first().text(fileName).attr("href", "/fs" + filePath).attr("title", "/fs" + filePath).removeClass("d-none");;
		$(selectors.editWrapper + " .icon-addon").removeClass("d-none");
		$(selectors.editWrapper + " .type-icon").first().attr("class", "fa fa-fw type-icon " + typeIconClass);
		$(selectors.editWrapper + " .input-group-append .remove").removeClass("d-none");
	}
	else {
		$(selectors.fakeInputLinkEl).first().text("").attr("href", "").attr("title", "").addClass("d-none");
		$(selectors.editWrapper + " .icon-addon").addClass("d-none");
		$(selectors.editWrapper + " .input-group-append .remove").addClass("d-none");
		$(selectors.fileUploadEl).first().val("");
	}

	//Upload functionality
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
						$(selectors.editWrapper + " .type-icon").first().attr("class", "fa fa-fw type-icon " + typeIconClass);
						//Show the input-group-addon if needed
						$(selectors.editWrapper + " .input-group-prepend.icon-addon").removeClass("d-none");
						$(selectors.editWrapper + " .input-group").removeClass("left-border");
						$(selectors.editWrapper + " .input-group-prepend.addon-remove").removeClass("d-none");
						$(selectors.inputEl).attr("data-newfilepath", result.object.url).attr("data-newfilename", result.object.filename);  //Input element 'value' and 'data-filename' are updated only on save
						$(selectors.fakeInputLinkEl).first().removeClass("d-none");
					},
					error: function (xhr, status, p3, p4) {
						var err = "Error " + " " + status + " " + p3 + " " + p4;
						if (xhr.responseText && xhr.responseText[0] === "{")
							err = JSON.parse(xhr.responseText).Message;
						$(selectors.fakeInputEl).addClass("is-invalid");
						$(selectors.editWrapper + " .input-group").after("<div class='invalid-feedback'>" + response.message + "</div>");
						$(selectors.editWrapper + " .invalid-feedback").first().show();
						toastr.error("An error occurred", 'Error!', { closeButton: true, tapToDismiss: true });
						console.log(err);
					}
				});
			} else {
				alert("This browser doesn't support HTML5 file uploads!");
			}
		}
	});

	$(selectors.viewWrapper).hide();
	$(selectors.editWrapper).show();
}

function FileInlineEditPreDisableCallback(fieldId, fieldName, entityName, recordId, config) {
	var selectors = FileInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
	$(selectors.editWrapper + " .invalid-feedback").remove();
	$(selectors.editWrapper + " .form-control").removeClass("is-invalid");
	$(selectors.editWrapper + " .save .fa").addClass("fa-check").removeClass("fa-spin fa-spinner");
	$(selectors.editWrapper + " .save").attr("disabled", false);
	$(selectors.viewWrapper).show();
	$(selectors.editWrapper).hide();
}

function FileInlineEditInit(fieldId, fieldName, entityName, recordId, config) {
	config = ProcessConfig(config);
	var selectors = FileInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
	//Init enable action click
	$(selectors.viewWrapper + " .action.btn").on("click", function (event) {
		event.stopPropagation();
		event.preventDefault();
		FileInlineEditPreEnableCallback(fieldId, fieldName, entityName, recordId, config);
	});
	//Init enable action dblclick
	$(selectors.viewWrapper + " .form-control").on("dblclick", function (event) {
		event.stopPropagation();
		event.preventDefault();
		FileInlineEditPreEnableCallback(fieldId, fieldName, entityName, recordId, config);
		//clearSelection();//double click causes text to be selected.
		setTimeout(function () {
			$(selectors.editWrapper + " .form-control").get(0).focus();
		}, 200);
	});
	//Disable inline edit action
	$(selectors.editWrapper + " .cancel").on("click", function (event) {
		event.stopPropagation();
		event.preventDefault();
		FileInlineEditPreDisableCallback(fieldId, fieldName, entityName, recordId, config);
	});
	//Save inline changes
	$(selectors.editWrapper + " .save").on("click", function (event) {
		event.stopPropagation();
		event.preventDefault();
		$(selectors.editWrapper + " .save .fa").removeClass("fa-check").addClass("fa-spin fa-spinner");
		$(selectors.editWrapper + " .save").attr("disabled", true);
		$(selectors.editWrapper + " .invalid-feedback").remove();
		$(selectors.editWrapper + " .form-control").removeClass("is-invalid");
		var newValue = $(selectors.inputEl).attr("data-newfilepath");
		var newFileName = $(selectors.inputEl).attr("data-newfilename");
		var oldValue = $(selectors.inputEl).val();
		if (newValue === oldValue) {
			//Update is not needed just close
			FileInlineEditPreDisableCallback(fieldId, fieldName, entityName, recordId, config);
		}
		else {
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
				success: function (response) {
					if (response.success) {
						FileInlineEditInitSuccessCallback(response, fieldId, fieldName, entityName, recordId, config);
					}
					else {
						FileInlineEditInitErrorCallback(response, fieldId, fieldName, entityName, recordId, config);
					}
				},
				error: function (jqXHR, textStatus, errorThrown) {
					var response = {};
				response.message = "";
				if (jqXHR && jqXHR.responseJSON) {
					response = jqXHR.responseJSON;
				}
					FileInlineEditInitErrorCallback(response, fieldId, fieldName, entityName, recordId, config);
				}
			});
		}
	});
}

function FileInlineEditInitSuccessCallback(response, fieldId, fieldName, entityName, recordId, config) {
	var selectors = FileInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
	var newValue = ProcessNewValue(response, fieldName);
	var fileName = GetFilenameFromUrl(newValue);
	$(selectors.inputEl).val(newValue).attr("data-filename", fileName);
	if (!isStringNullOrEmptyOrWhiteSpace(newValue)) {
		var typeIconClass = GetPathTypeIcon(newValue);
		$(selectors.viewWrapper + " .form-control a").first().text(fileName).attr("href", "/fs" + newValue).attr("title", "/fs" + newValue).removeClass("d-none");;
		$(selectors.viewWrapper + " .type-icon").first().attr("class", "fa fa-fw type-icon " + typeIconClass);
		$(selectors.viewWrapper + " .input-group-prepend.icon-addon").removeClass("d-none");
	}
	else {
		$(selectors.viewWrapper + " .form-control a").first().text(fileName).attr("href", "").attr("title", "").addClass("d-none");
		$(selectors.viewWrapper + " .type-icon").first().addClass("d-none");
		$(selectors.viewWrapper + " .input-group-prepend.icon-addon").addClass("d-none");
	}
	if (newValue) {
		$(selectors.editWrapper + " .form-control").val(newValue);
	}
	else {
		$(selectors.editWrapper + " .form-control").val("");
	}
	FileInlineEditPreDisableCallback(fieldId, fieldName, entityName, recordId, config);
	toastr.success("The new value is successfull saved", 'Success!', { closeButton: true, tapToDismiss: true });
}

function FileInlineEditInitErrorCallback(response, fieldId, fieldName, entityName, recordId, config) {
	var selectors = FileInlineEditGenerateSelectors(fieldId, fieldName, entityName, recordId, config);
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
