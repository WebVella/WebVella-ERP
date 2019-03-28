
function DataCsvInlineEditGenerateSelectors(fieldId, fieldName, delimiterFieldName, hasHeaderFieldName, entityName, recordId, config) {
	//Method for generating selector strings of some of the presentation elements
    var $selectors = {};
    $selectors.card = $("#card-" + fieldId);
    $selectors.cardDs = $("#doublescroll-" + fieldId);
    $selectors.tabLink = $selectors.card.find(".nav-link");
    $selectors.tabPane = $selectors.card.find(".tab-pane");
    $selectors.doubleScrollWrapper1 = $selectors.cardDs.find(".doublescroll-wrapper1");
    $selectors.doubleScrollWrapper2 = $selectors.cardDs.find(".doublescroll-wrapper2");
    $selectors.doubleScrollInner1 = $selectors.cardDs.find(".doublescroll-inner1");
    $selectors.doubleScrollInner2 = $selectors.cardDs.find(".doublescroll-inner2");
    $selectors.previewWrapper = $selectors.cardDs.find(".preview");
    $selectors.modal = $("#modal-" + fieldId);
    $selectors.modalInputHasHeaderEl = $selectors.modal.find("input[name='" + hasHeaderFieldName + "']");
    $selectors.modalInputHasHeaderFakeEl = $selectors.modal.find("#input-hasheader-fake-" + fieldId);
    $selectors.modalInputEl = $selectors.modal.find("#modal-textarea-" + fieldId);
    $selectors.modalInputDelimiterEl = $selectors.modal.find("input[name='" + delimiterFieldName + "']");
    $selectors.modalSubmitBtn = $selectors.modal.find(".submit");
    return $selectors;
}


function DataCsvInlineEditInit(fieldId, fieldName, delimiterFieldName, hasHeaderFieldName, entityName, recordId, config) {
	config = ProcessConfig(config);
    var $selectors = DataCsvInlineEditGenerateSelectors(fieldId, fieldName,delimiterFieldName, hasHeaderFieldName, entityName, recordId, config);

    $selectors.doubleScrollWrapper1.on('scroll', function (e) {
        $selectors.doubleScrollWrapper2.scrollLeft($selectors.doubleScrollWrapper1.scrollLeft());
    });
    $selectors.doubleScrollWrapper2.on('scroll', function (e) {
        $selectors.doubleScrollWrapper1.scrollLeft($selectors.doubleScrollWrapper2.scrollLeft());
    });

    $selectors.doubleScrollInner1.width($selectors.doubleScrollInner2.find('.table').width());
    $selectors.doubleScrollInner1.width($selectors.doubleScrollInner2.find('.table').width());


    $selectors.tabLink.click(function (event) {
        event.preventDefault();
        if ($(this).hasClass("edit")) {
            $selectors.modal.modal("show");
        }
        else {
            var tabId = $(this).attr("data-tab-id");
            $selectors.tabLink.removeClass("active");
            $selectors.tabPane.removeClass("active");
            $selectors.card.find("#" + fieldId + "-tab-" + tabId).addClass("active");
            $(this).addClass("active");
        }
    });


    $selectors.modalInputHasHeaderFakeEl.on('change', function () {
        if ($(this).prop('checked')) {
            $($selectors.modalInputHasHeaderEl).val("true");
            $($selectors.modalInputHasHeaderEl).trigger("change");
        }
        else {
            $($selectors.modalInputHasHeaderEl).val("false");
            $($selectors.modalInputHasHeaderEl).trigger("change");
        }
    });

    $selectors.modalSubmitBtn.click(function (event) {
        event.preventDefault();
        var hasHeader = true;
        if ($selectors.modalInputHasHeaderEl.val() === "false") {
            hasHeader = false;
        }
        var delimiter = $selectors.modal.find("input[name='" + delimiterFieldName + "']:checked").val();

        var payload = {};
        payload[fieldName] = $selectors.modalInputEl.val();
        payload[hasHeaderFieldName] = hasHeader;
        payload[delimiterFieldName] = delimiter;

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
            data: JSON.stringify(payload),
            success: function (response) {
                if (response.success) {
                    DataCsvInlineEditInitSuccessCallback();
                }
                else {
                    DataCsvInlineEditInitErrorCallback(response);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                var response = {};
                response.message = "";
                if (jqXHR && jqXHR.responseJSON) {
                    response = jqXHR.responseJSON;
                }
                DataCsvInlineEditInitErrorCallback(response);
            }
        });
    });

    function DataCsvInlineEditInitSuccessCallback() {
        toastr.success("The new value is successfull saved", 'Success!', { closeButton: true, tapToDismiss: true });
        location.reload();
    }

    function DataCsvInlineEditInitErrorCallback(response) {

        toastr.error("An error occurred", 'Error!', { closeButton: true, tapToDismiss: true });
        console.log("error", response);
        location.reload();
    }
}

