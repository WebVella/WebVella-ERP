//JS DateTime picker init method
var timePickerDictionary = {};
var flatPickrServerTimeFormat = "Z";//"Y-m-dTH:i:S";
//From the server dates will be received yyyy-MM-ddTHH:mm:ss.fff
var flatPickrUiTimeFormat = "H:i";
function InitFlatPickrTime(selector) {
	var fp = document.querySelector(selector)._flatpickr;
	if (!fp) {
		var instance = flatpickr(selector, { time_24hr: true, dateFormat: flatPickrServerTimeFormat, locale: BulgarianDateTimeLocale,noCalendar: true, enableTime: true, minuteIncrement: 1, altInput: true, altFormat: flatPickrUiTimeFormat });
		return instance;
	}
	else {
		return fp;
	}
}