/**
 *
 */
(function () {
    /**
     * The **moment.js** library.
     *
     * @property moment
     * @type {Object}
     */
    var moment,
        /**
         * The internal **Angular** date formats cache.
         *
         * @property angularDateFormats
         * @type {Object}
         */
        angularDateFormats = {},

        /**
         * The internal **moment.js** date formats cache.
         *
         * @property momentDateFormats
         * @type {Object}
         */
        momentDateFormats = {},

        /**
         * The format pattern mapping from Angular format to momentjs.
         *
         * @property angularFormatMapping
         * @type {Object}
         */
        angularFormatMapping = {
            d: 'D',
            dd: 'DD',
            y: 'YYYY',
            yy: 'YY',
            yyyy: 'YYYY',
            a: 'a',
            M: 'M',
            MM: 'MM',
            MMM: 'MMM',
            MMMM: 'MMMM',
            h: 'h',
            hh: 'hh',
            H: 'H',
            HH: 'HH',
            m: 'm',
            mm: 'mm',
            s: 's',
            ss: 'ss',
            sss: 'SSS',
            EEE: 'ddd',
            EEEE: 'dddd',
            w: 'W',
            ww: 'WW',
            Z: 'ZZ'
        },

        /**
         * The format pattern mapping from Angular format to momentjs.
         *
         * @property momentFormatMapping
         * @type {Object}
         */
        momentFormatMapping = {
            D: 'd',
            DD: 'dd',
            YY: 'yy',
            YYYY: 'yyyy',
            a: 'a',
            A: 'a',
            M: 'M',
            MM: 'MM',
            MMM: 'MMM',
            MMMM: 'MMMM',
            h: 'h',
            hh: 'hh',
            H: 'H',
            HH: 'HH',
            m: 'm',
            mm: 'mm',
            s: 's',
            ss: 'ss',
            S: 'sss',
            SS: 'sss',
            SSS: 'sss',
            ddd: 'EEE',
            dddd: 'EEEE',
            W: 'w',
            WW: 'ww',
            ZZ: 'Z'
        };

    if (typeof require !== 'undefined' && require !== null) {
        moment = require('moment');
    } else {
        moment = this.moment;
    }


    /**
     * Translates the angular date format String to a momentjs format String.
     *
     * @function translateFormat
     * @param {String}  formatString    The unmodified format string
     * @param {Object}  mapping         The date format mapping object
     * @returns {String}
     */
    var translateFormat = function (formatString, mapping) {
        var len = formatString.length,
            i = 0,
            beginIndex = -1,
            lastChar = null,
            currentChar = "",
            resultString = "";

        for (; i < len; i++) {
            currentChar = formatString.charAt(i);

            if (lastChar === null || lastChar !== currentChar) {
                // change detected
                resultString = _appendMappedString(formatString, mapping, beginIndex, i, resultString);

                beginIndex = i;
            }

            lastChar = currentChar;
        }

        return _appendMappedString(formatString, mapping, beginIndex, i, resultString);
    };

    /**
     * Checks if the substring is a mapped date format pattern and adds it to the result format String.
     *
     * @function _appendMappedString
     * @param {String}  formatString    The unmodified format String.
     * @param {Object}  mapping         The date format mapping Object.
     * @param {Number}  beginIndex      The begin index of the continuous format characters.
     * @param {Number}  currentIndex    The last index of the continuous format characters.
     * @param {String}  resultString    The result format String.
     * @returns {String}
     * @private
     */
    var _appendMappedString = function (formatString, mapping, beginIndex, currentIndex, resultString) {
        var tempString;

        if (beginIndex !== -1) {
            tempString = formatString.substring(beginIndex, currentIndex);
            // check if the temporary string has a known mapping
            if (mapping[tempString]) {
                tempString = mapping[tempString];
            }
            resultString = resultString.concat(tempString);
        }
        return resultString;
    };

    // register as private function (good for testing purposes)
    moment.fn.__translateAngularFormat = translateFormat;

    /**
     * Translates the momentjs format String to a angular date format String.
     *
     * @function toMomentFormatString
     * @param {String}  formatString    The format String to be translated.
     * @returns {String}
     */
    moment.fn.toMomentFormatString = function (formatString) {
        if (!angularDateFormats[formatString]) {
            angularDateFormats[formatString] = translateFormat(formatString, angularFormatMapping);
        }
        return angularDateFormats[formatString];
    };

    /**
     * Format the moment with the given angular date format String.
     *
     * @function formatWithADF
     * @param {String}  formatString    The format String to be translated.
     * @returns {String}
     */
    moment.fn.formatWithADF = function (formatString) {
        return this.format(this.toMomentFormatString(formatString));
    };

    /**
     * Translates the momentjs format string to a angular date format string
     *
     * @function toADFString
     * @param {String}  formatString    The format String to be translated.
     * @returns {String}
     */
    moment.fn.toADFString = function (formatString) {
        if (!momentDateFormats[formatString]) {
            momentDateFormats[formatString] = translateFormat(formatString, momentFormatMapping);
        }
        return momentDateFormats[formatString];
    };


    if (typeof module !== 'undefined' && module !== null) {
        module.exports = moment;
    } else {
        this.moment = moment;
    }

}).call(this);
