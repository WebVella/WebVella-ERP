
function DataCsvFormInitGenerateSelectors(fieldId, name, delimiterName, hasHeaderName, lang) {
	//Method for generating selector strings of some of the presentation elements
    var $selectors = {};
    $selectors.card = $("#card-" + fieldId);
    $selectors.tabLink = $selectors.card.find(".nav-link");
    $selectors.tabPane = $selectors.card.find(".tab-pane");
    $selectors.doubleScrollWrapper1 = $selectors.card.find(".doublescroll-wrapper1");
    $selectors.doubleScrollWrapper2 = $selectors.card.find(".doublescroll-wrapper2");
    $selectors.doubleScrollInner1 = $selectors.card.find(".doublescroll-inner1");
    $selectors.doubleScrollInner2 = $selectors.card.find(".doublescroll-inner2");
    $selectors.previewWrapper = $selectors.card.find(".preview");
    $selectors.inputHasHeaderEl = $selectors.card.find("input[name='" + hasHeaderName + "']");
    $selectors.inputHasHeaderFakeEl = $selectors.card.find("#input-hasheader-fake-" + fieldId);
    $selectors.inputEl = $selectors.card.find("textarea[name='" + name + "']");
    $selectors.inputDelimiterEl = $selectors.card.find("input[name='" + delimiterName + "']");

	return $selectors;
}


function DataCsvFormInit(fieldId,name,delimiterName,hasHeaderName,lang) {
    var $selectors = DataCsvFormInitGenerateSelectors(fieldId, name, delimiterName, hasHeaderName, lang);

    $selectors.doubleScrollWrapper1.on('scroll', function (e) {
        $selectors.doubleScrollWrapper2.scrollLeft($selectors.doubleScrollWrapper1.scrollLeft());
    });
    $selectors.doubleScrollWrapper2.on('scroll', function (e) {
        $selectors.doubleScrollWrapper1.scrollLeft($selectors.doubleScrollWrapper2.scrollLeft());
    });

    $selectors.tabLink.click(function (event) {
        event.preventDefault();
        var tabId = $(this).attr("data-tab-id");
        $selectors.tabLink.removeClass("active");
        $selectors.tabPane.removeClass("active");
        $selectors.card.find("#" + fieldId + "-tab-" + tabId).addClass("active");
        $(this).addClass("active");

        var hasHeader = true;
        if ($selectors.inputHasHeaderEl.val() === "false") {
            hasHeader = false;
        }

        var delimiter = $selectors.card.find("input[name='" + delimiterName + "']:checked").val();

        if (tabId === "preview") {
            var payload = {
                csv: $selectors.inputEl.val(),
                hasHeader: hasHeader,
                delimiter: delimiter
            };

            $.ajax({
                type: "POST",
                url: "/api/v3.0/" + lang +"/p/core/ui/field-table-data/generate/preview",
                // The key needs to match your method's input parameter (case-sensitive).
                data: JSON.stringify(payload),
                contentType: "application/json; charset=utf-8",
                dataType: "html",
                success: function (data) {
                    $selectors.doubleScrollInner1.css("width", '');
                    $selectors.doubleScrollInner1.css("width", '');
                    $selectors.previewWrapper.html(data);
                    $selectors.doubleScrollInner1.width($selectors.doubleScrollInner2.find('.table').width());
                    $selectors.doubleScrollInner1.width($selectors.doubleScrollInner2.find('.table').width());
                },
                failure: function (errMsg) {
                    toastr.error("System Error", 'Error!', { tapToDismiss: true });
                    console.error(errMsg);
                }
            }); 

        }
        else {
            $selectors.previewWrapper.html("<div class='loading-pane'><i class='fa fa-spin fa-spinner'></i></div>");
        }

    });

    $selectors.inputHasHeaderFakeEl.on('change',function () {
        if ($(this).prop('checked')) {
            $($selectors.inputHasHeaderEl).val("true");
            $($selectors.inputHasHeaderEl).trigger("change");
        }
        else {
            $($selectors.inputHasHeaderEl).val("false");
            $($selectors.inputHasHeaderEl).trigger("change");
        }
    });
	
}

