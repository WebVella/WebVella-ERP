
function DataCsvDisplayInitGenerateSelectors(fieldId) {
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
	return $selectors;
}


function DataCsvDisplayInit(fieldId) {
    var $selectors = DataCsvDisplayInitGenerateSelectors(fieldId);

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
        var tabId = $(this).attr("data-tab-id");
        $selectors.tabLink.removeClass("active");
        $selectors.tabPane.removeClass("active");
        $selectors.card.find("#" + fieldId + "-tab-" + tabId).addClass("active");
        $(this).addClass("active");

    });
}

