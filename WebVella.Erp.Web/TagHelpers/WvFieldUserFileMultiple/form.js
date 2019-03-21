function FieldUserMultiFileFormGenerateSelectors(fieldId, config) {
	//Method for generating selector strings of some of the presentation elements
	var selectors = {};
	selectors.inputEl = "#input-" + fieldId;
	selectors.fileUploadEl = "#file-" + fieldId;
	selectors.fakeInputEl = "#fake-" + fieldId;
	selectors.fakeInputProgressEl = selectors.fakeInputEl + " .form-control-progress";
	selectors.fileListEl = "#fake-list-" + fieldId;
	selectors.removeFileLink = selectors.fileListEl + " .filerow .action .link";
	return selectors;
}

function FieldUserMultiFileRemoveFileRow(e) {
	e.preventDefault();
    e.stopPropagation();
    var clickedBtn = event.target;
    var fileId = $(clickedBtn).closest(".filerow").attr("data-file-id");

    if (confirm("Are you sure?")) {

        $.ajax({
            type: "DELETE",
            url: '/api/v3/en_US/record/user_file/' + fileId,
            success: function () {
                var fileRow = $(clickedBtn).closest(".filerow");
                var fieldId = $(clickedBtn).closest(".erp-file-multiple-list").attr("id").replace("fake-list-", "");
                var selectors = FieldUserMultiFileFormGenerateSelectors(fieldId, {});
                var inputValue = $(selectors.inputEl).val();
                if (inputValue && inputValue.indexOf(fileId) > -1) {
                    var idArray = [];
                    if (inputValue) {
                        idArray = inputValue.toLowerCase().split(',');
                    }
                    var filteredArray = _.filter(idArray, function (recordId) { return recordId !== fileId });
                    $(selectors.inputEl).val(filteredArray.join(','));
                    $(fileRow).remove();
                    if (filteredArray.length === 0) {
                        $(selectors.fileListEl).addClass("d-none");
                    }
                }
                else {
                    console.error("File Id: " + fileId + " not found in the hidden input value");
                }
            },
            error: function (xhr, status, p3, p4) {
                var err = "Error " + " " + status;
                if (p3) {
                    err += " " + p3;
                }
                if (p4) {
                    err += " " + p4;
                }
                if (xhr.responseText && xhr.responseText.startsWith("{")) {
                    err = JSON.parse(xhr.responseText).message;
                }

                toastr.error("An error occurred", 'Error!', { closeButton: true, tapToDismiss: true });
                console.log(err);
            }
        });


	}
}

function FieldUserMultiFileFormInit(fieldId, config) {
	config = ProcessConfig(config);
    var selectors = FieldUserMultiFileFormGenerateSelectors(fieldId, config);
	//Remove value
	$(selectors.removeFileLink).on('click', FieldUserMultiFileRemoveFileRow);

    $(selectors.fileUploadEl).first().on('change', function (e) {
        var files = e.target.files;
		if (files.length > 0) {
			if (window.FormData !== undefined) {
				var inputEl = $(selectors.inputEl);
				$(selectors.fakeInputEl).html("<div class='form-control-progress'></div>");
				$(selectors.fakeInputEl).removeClass("is-invalid");

				var data = new FormData();
				//support only single file upload
				for (var i = 0; i < files.length; ++i) {
					data.append('files', files[i]);
				}
				//data.append("files", files);

				$.ajax({
					type: "POST",
					url: '/fs/upload-user-file-multiple',
					contentType: false,
					processData: false,
					data: data,
					xhr: function () {
						// get the native XmlHttpRequest object
						var xhr = $.ajaxSettings.xhr();
						// set the onprogress event handler
						xhr.upload.onprogress = function (evt,e) {
							var progressPercent = parseInt(evt.loaded / evt.total * 100);
							$(selectors.fakeInputProgressEl).first().attr("style", "display:block;width:" + progressPercent + "%");
							$(selectors.fakeInputProgressEl).first().html(progressPercent + "%");
						};
						// set the onload event handler
						xhr.upload.onload = function (e) {
							$(selectors.fakeInputProgressEl).first().html("<i class='fa fa-spin fa-spinner go-blue'></i> Processing ...");
						};
						// return the customized object
						return xhr;
					},
					success: function (result) {
						$(selectors.fakeInputProgressEl).first().attr("style", "display:none;width:0%")

						if (result.success) {
							if (result.object && result.object.length > 0) {
								_.forEach(result.object, function(file) {

									//Add file id to hidden input
									var inputValue = $(selectors.inputEl).val();
									var idArray = [];
									if (inputValue) {
										idArray = inputValue.toLowerCase().split(',');
									}
									idArray.push(file.id);
									$(selectors.inputEl).val(idArray.join(','));

									//Add filerow above
									var iconClass = GetPathTypeIcon(file.name);
									var sizeString = "";
									var sizeKBInt = parseInt(file.size);
									if (sizeKBInt < 1024) {
										sizeString = sizeKBInt + " KB";
									}
									else if (sizeKBInt >= 1024 && sizeKBInt < 1024 * 1024) {
										sizeString = Math.round(sizeKBInt / 1024) + " MB";
									}
									else {
										sizeString = Math.round(sizeKBInt / (1024*1024)) + " GB";
									}
									var fileRowEl = document.createElement("div");
									fileRowEl.className = "filerow";
									fileRowEl.dataset["fileId"] = file.id;
									fileRowEl.innerHTML = '<div class="icon"><i class="fa ' + iconClass +'"></i></div><div class="meta"><a class="link" href="/fs' + file.path +'" target="_blank" title="/fs'+ file.path + '">'+ file.name +'<em></em></a><div class="size">' + sizeString + '</div></div>';
									
									var fileRowAction = document.createElement("div");
									fileRowAction.className = "action remove";
									
									var fileRowActionLink = document.createElement("a");
									fileRowActionLink.className = "link";
									fileRowActionLink.href = "#";
									fileRowActionLink.innerHTML = '<i class="fa fa-times-circle"></i>';
									fileRowActionLink.onclick = FieldUserMultiFileRemoveFileRow;
									fileRowAction.appendChild(fileRowActionLink);
									fileRowEl.appendChild(fileRowAction);
									
									$(selectors.fileListEl).prepend(fileRowEl);
								});			
								
								$(selectors.fileListEl).removeClass("d-none");
							}
						}
						else {
							$(selectors.fakeInputEl).addClass("is-invalid");
							$(selectors.fakeInputProgressEl).first().attr("style", "display:none;width:'0px'");
							$(selectors.fakeInputEl).html("<span class='go-red'><i class='fa fa-exclamation-circle'></i> " + result.message +"</span>");

							toastr.error("An error occurred", 'Error!', { closeButton: true, tapToDismiss: true });
							console.log(result.message);							
						}

					},
					error: function (xhr, status, p3, p4) {
						var err = "Error " + " " + status;
						if (p3) {
							err += " " + p3
						}
						if (p4) {
							err += " " + p4
						}
						if (xhr.responseText && xhr.responseText.startsWith("{")) {
							err = JSON.parse(xhr.responseText).message;
						}

						$(selectors.fakeInputEl).addClass("is-invalid");
						$(selectors.fakeInputProgressEl).first().attr("style", "display:none;width:'0px'");
						$(selectors.fakeInputEl).html("<span class='go-red'><i class='fa fa-exclamation-circle'></i> " + err +"</span>");

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
