//ErpEvent.DISPATCH('WebVella.Erp.Web.Components.PcForm',{htmlId:null,action:'submit',payload:null})

function WebVellaErpWebComponentsPcPageHeader_Init(elementId) {
    var elementHtmlId = '#wv-' + elementId;
    var elementPosition = $(elementHtmlId).offset();

    $("#body-inner-wrapper-2").scroll(function () {
        if ($("#body-inner-wrapper-2").scrollTop() > elementPosition.top) {
            $(elementHtmlId).addClass('fixed');
        } else {
            $(elementHtmlId).removeClass('fixed');
        }
    });    	
}

