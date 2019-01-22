//JS DateTime picker init method
var dateTimePickerDictionary = {};
var flatPickrServerDateTimeFormat = "Z";//"Y-m-dTH:i:S";
//From the server dates will be received yyyy-MM-ddTHH:mm:ss.fff
var flatPickrUiDateTimeFormat = "d M Y H:i";
function InitFlatPickrDateTime(selector) {
	var fp = document.querySelector(selector)._flatpickr;
	if (!fp) {
		var instance = flatpickr(selector, { time_24hr: true, dateFormat: flatPickrServerDateTimeFormat, locale: BulgarianDateTimeLocale, enableTime: true, minuteIncrement: 1, altInput: true, altFormat: flatPickrUiDateTimeFormat });
		return instance;
	}
	else {
		return fp;
	}
}