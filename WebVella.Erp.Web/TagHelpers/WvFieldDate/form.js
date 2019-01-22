//JS DateTime picker init method
var datePickerDictionary = {};
var flatPickrServerDateFormat = "Y-m-dTH:i:S";
var flatPickrUiDateFormat = "d M Y";
//From the server dates will be received yyyy-MM-ddTHH:mm:ss.fff
function InitFlatPickrDate(selector) {
	if (document.querySelector(selector)) {
		var fp = document.querySelector(selector)._flatpickr;
		if (!fp) {
			var instance = flatpickr(selector, { time_24hr: true, dateFormat: flatPickrServerDateFormat, locale: BulgarianDateTimeLocale, altInput: true, altFormat: flatPickrUiDateFormat });
			return instance;
		}
		else {
			return fp;
		}
	}
}