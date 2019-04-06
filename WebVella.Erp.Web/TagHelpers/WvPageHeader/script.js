//ErpEvent.DISPATCH('WebVella.Erp.Web.Components.PcForm',{htmlId:null,action:'submit',payload:null})

function WebVellaErpWebComponentsPcPageHeader_Init(elementId) {
    var elementHtmlId = '#wv-' + elementId;
    var fakeElementId = "wv-fake-" + elementId;
    var elementPosition = $(elementHtmlId).offset();
    var elementHeight = $(elementHtmlId).outerHeight();

    //$("#body-inner-wrapper-2").scroll(function () {
    //    if ($("#body-inner-wrapper-2").scrollTop() > elementPosition.top) {
    //        //Append fake element to preserve height
    //        if (!$(elementHtmlId).hasClass("fixed")) {
    //            $("<div/>").css("height", elementHeight).attr("id", fakeElementId).insertAfter(elementHtmlId);
    //            $(elementHtmlId).addClass('fixed');
    //        }
    //    } else {
    //        if ($(elementHtmlId).hasClass("fixed")) {
    //            $("#" + fakeElementId).remove();
    //            $(elementHtmlId).removeClass('fixed');
    //        }
    //    }
    //});    	
    $(window).scroll(function () {
        if ($(window).scrollTop() > elementPosition.top) {
            //Append fake element to preserve height
            if (!$(elementHtmlId).hasClass("fixed")) {
                $("<div/>").css("height", elementHeight).attr("id", fakeElementId).insertAfter(elementHtmlId);
                $(elementHtmlId).addClass('fixed');
            }
        } else {
            if ($(elementHtmlId).hasClass("fixed")) {
                $("#" + fakeElementId).remove();
                $(elementHtmlId).removeClass('fixed');
            }
        }
    });  
}

