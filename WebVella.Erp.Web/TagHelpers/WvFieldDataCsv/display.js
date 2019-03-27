
function DataCsvDisplayInitGenerateSelectors(fieldId) {
	//Method for generating selector strings of some of the presentation elements
    var $selectors = {};
    $selectors.card = $("#doublescroll-" + fieldId);
    $selectors.doubleScrollWrapper1 = $selectors.card.find(".doublescroll-wrapper1");
    $selectors.doubleScrollWrapper2 = $selectors.card.find(".doublescroll-wrapper2");
    $selectors.doubleScrollInner1 = $selectors.card.find(".doublescroll-inner1");
    $selectors.doubleScrollInner2 = $selectors.card.find(".doublescroll-inner2");
    $selectors.previewWrapper = $selectors.card.find(".preview");
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
}

