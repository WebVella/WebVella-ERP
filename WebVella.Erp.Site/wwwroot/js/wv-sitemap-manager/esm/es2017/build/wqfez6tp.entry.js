import { h } from '../wv-sitemap-manager.core.js';

class WvSitemapAreaModal {
    constructor() {
        this.area = null;
        this.submitResponse = { message: "", errors: [] };
        this.modalArea = null;
    }
    componentWillLoad() {
        var backdropId = "wv-sitemap-manager-area-modal-backdrop";
        var backdropDomEl = document.getElementById(backdropId);
        if (!backdropDomEl) {
            var backdropEl = document.createElement('div');
            backdropEl.className = "modal-backdrop show";
            backdropEl.id = backdropId;
            document.body.appendChild(backdropEl);
            this.modalArea = Object.assign({}, this.area);
            delete this.modalArea["nodes"];
        }
    }
    componentDidUnload() {
        var backdropId = "wv-sitemap-manager-area-modal-backdrop";
        var backdropDomEl = document.getElementById(backdropId);
        if (backdropDomEl) {
            backdropDomEl.remove();
        }
    }
    closeModal() {
        this.wvSitemapManagerAreaModalCloseEvent.emit();
    }
    handleSubmit(e) {
        e.preventDefault();
        this.wvSitemapManagerAreaSubmittedEvent.emit(this.modalArea);
    }
    handleChange(event) {
        let propertyName = event.target.getAttribute('name');
        this.modalArea[propertyName] = event.target.value;
    }
    handleCheckboxChange(event) {
        let propertyName = event.target.getAttribute('name');
        let isChecked = event.target.checked;
        this.modalArea[propertyName] = isChecked;
    }
    render() {
        let modalTitle = "Manage area";
        if (!this.area) {
            modalTitle = "Create area";
        }
        return (h("div", { class: "modal d-block" },
            h("div", { class: "modal-dialog modal-xl" },
                h("div", { class: "modal-content" },
                    h("form", { onSubmit: (e) => this.handleSubmit(e) },
                        h("div", { class: "modal-header" },
                            h("h5", { class: "modal-title" }, modalTitle)),
                        h("div", { class: "modal-body" },
                            h("div", { class: "alert alert-danger " + (this.submitResponse["success"] ? "d-none" : "") }, this.submitResponse["message"]),
                            h("div", { class: "row" },
                                h("div", { class: "col col-sm-6" },
                                    h("div", { class: "form-group erp-field" },
                                        h("label", { class: "control-label" }, "Name"),
                                        h("input", { class: "form-control", name: "name", value: this.modalArea["name"], onInput: (event) => this.handleChange(event) }))),
                                h("div", { class: "col col-sm-6" },
                                    h("div", { class: "form-group erp-field" },
                                        h("label", { class: "control-label" }, "Label"),
                                        h("input", { class: "form-control", name: "label", value: this.modalArea["label"], onInput: (event) => this.handleChange(event) })))),
                            h("div", { class: "row" },
                                h("div", { class: "col col-sm-12" },
                                    h("div", { class: "form-group erp-field" },
                                        h("label", { class: "control-label" }, "Description"),
                                        h("textarea", { class: "form-control", style: { height: "60px" }, name: "description", onInput: (event) => this.handleChange(event) }, this.modalArea["description"])))),
                            h("div", { class: "row" },
                                h("div", { class: "col col-sm-6" },
                                    h("div", { class: "form-group erp-field" },
                                        h("label", { class: "control-label" }, "Weight"),
                                        h("input", { type: "number", step: 1, min: 1, class: "form-control", name: "weight", value: this.modalArea["weight"], onInput: (event) => this.handleChange(event) }))),
                                h("div", { class: "col col-sm-6" },
                                    h("div", { class: "form-group erp-field" },
                                        h("label", { class: "control-label" }, "Group names"),
                                        h("div", { class: "form-control-plaintext" },
                                            h("div", { class: "form-check" },
                                                h("label", { class: "form-check-label" },
                                                    h("input", { class: "form-check-input", type: "checkbox", name: "show_group_names", value: "true", checked: this.modalArea["show_group_names"], onChange: (event) => this.handleCheckboxChange(event) }),
                                                    " group names are visible")))))),
                            h("div", { class: "row" },
                                h("div", { class: "col col-sm-6" },
                                    h("div", { class: "form-group erp-field" },
                                        h("label", { class: "control-label" }, "Color"),
                                        h("input", { class: "form-control", name: "color", value: this.modalArea["color"], onInput: (event) => this.handleChange(event) }))),
                                h("div", { class: "col col-sm-6" },
                                    h("div", { class: "form-group erp-field" },
                                        h("label", { class: "control-label" }, "Icon Class"),
                                        h("input", { class: "form-control", name: "icon_class", value: this.modalArea["icon_class"], onInput: (event) => this.handleChange(event) })))),
                            h("div", { class: "alert alert-info" }, "Label and Description translations, and access are currently not managable")),
                        h("div", { class: "modal-footer" },
                            h("button", { type: "submit", class: "btn btn-green btn-sm " + (this.area == null ? "" : "d-none") },
                                h("span", { class: "fa fa-plus" }),
                                " Create area"),
                            h("button", { type: "submit", class: "btn btn-blue btn-sm " + (this.area != null ? "" : "d-none") },
                                h("span", { class: "far fa-save" }),
                                " Save area"),
                            h("button", { type: "button", class: "btn btn-white btn-sm ml-1", onClick: () => this.closeModal() }, "Close")))))));
    }
    static get is() { return "wv-sitemap-area-modal"; }
    static get properties() { return {
        "area": {
            "type": "Any",
            "attr": "area"
        },
        "modalArea": {
            "state": true
        },
        "submitResponse": {
            "type": "Any",
            "attr": "submit-response"
        }
    }; }
    static get events() { return [{
            "name": "wvSitemapManagerAreaModalCloseEvent",
            "method": "wvSitemapManagerAreaModalCloseEvent",
            "bubbles": true,
            "cancelable": true,
            "composed": true
        }, {
            "name": "wvSitemapManagerAreaSubmittedEvent",
            "method": "wvSitemapManagerAreaSubmittedEvent",
            "bubbles": true,
            "cancelable": true,
            "composed": true
        }]; }
}

var bind = function bind(fn, thisArg) {
  return function wrap() {
    var args = new Array(arguments.length);
    for (var i = 0; i < args.length; i++) {
      args[i] = arguments[i];
    }
    return fn.apply(thisArg, args);
  };
};

/*!
 * Determine if an object is a Buffer
 *
 * @author   Feross Aboukhadijeh <https://feross.org>
 * @license  MIT
 */

// The _isBuffer check is for Safari 5-7 support, because it's missing
// Object.prototype.constructor. Remove this eventually
var isBuffer_1 = function (obj) {
  return obj != null && (isBuffer(obj) || isSlowBuffer(obj) || !!obj._isBuffer)
};

function isBuffer (obj) {
  return !!obj.constructor && typeof obj.constructor.isBuffer === 'function' && obj.constructor.isBuffer(obj)
}

// For Node v0.10 support. Remove this eventually.
function isSlowBuffer (obj) {
  return typeof obj.readFloatLE === 'function' && typeof obj.slice === 'function' && isBuffer(obj.slice(0, 0))
}

/*global toString:true*/

// utils is a library of generic helper functions non-specific to axios

var toString = Object.prototype.toString;

/**
 * Determine if a value is an Array
 *
 * @param {Object} val The value to test
 * @returns {boolean} True if value is an Array, otherwise false
 */
function isArray(val) {
  return toString.call(val) === '[object Array]';
}

/**
 * Determine if a value is an ArrayBuffer
 *
 * @param {Object} val The value to test
 * @returns {boolean} True if value is an ArrayBuffer, otherwise false
 */
function isArrayBuffer(val) {
  return toString.call(val) === '[object ArrayBuffer]';
}

/**
 * Determine if a value is a FormData
 *
 * @param {Object} val The value to test
 * @returns {boolean} True if value is an FormData, otherwise false
 */
function isFormData(val) {
  return (typeof FormData !== 'undefined') && (val instanceof FormData);
}

/**
 * Determine if a value is a view on an ArrayBuffer
 *
 * @param {Object} val The value to test
 * @returns {boolean} True if value is a view on an ArrayBuffer, otherwise false
 */
function isArrayBufferView(val) {
  var result;
  if ((typeof ArrayBuffer !== 'undefined') && (ArrayBuffer.isView)) {
    result = ArrayBuffer.isView(val);
  } else {
    result = (val) && (val.buffer) && (val.buffer instanceof ArrayBuffer);
  }
  return result;
}

/**
 * Determine if a value is a String
 *
 * @param {Object} val The value to test
 * @returns {boolean} True if value is a String, otherwise false
 */
function isString(val) {
  return typeof val === 'string';
}

/**
 * Determine if a value is a Number
 *
 * @param {Object} val The value to test
 * @returns {boolean} True if value is a Number, otherwise false
 */
function isNumber(val) {
  return typeof val === 'number';
}

/**
 * Determine if a value is undefined
 *
 * @param {Object} val The value to test
 * @returns {boolean} True if the value is undefined, otherwise false
 */
function isUndefined(val) {
  return typeof val === 'undefined';
}

/**
 * Determine if a value is an Object
 *
 * @param {Object} val The value to test
 * @returns {boolean} True if value is an Object, otherwise false
 */
function isObject(val) {
  return val !== null && typeof val === 'object';
}

/**
 * Determine if a value is a Date
 *
 * @param {Object} val The value to test
 * @returns {boolean} True if value is a Date, otherwise false
 */
function isDate(val) {
  return toString.call(val) === '[object Date]';
}

/**
 * Determine if a value is a File
 *
 * @param {Object} val The value to test
 * @returns {boolean} True if value is a File, otherwise false
 */
function isFile(val) {
  return toString.call(val) === '[object File]';
}

/**
 * Determine if a value is a Blob
 *
 * @param {Object} val The value to test
 * @returns {boolean} True if value is a Blob, otherwise false
 */
function isBlob(val) {
  return toString.call(val) === '[object Blob]';
}

/**
 * Determine if a value is a Function
 *
 * @param {Object} val The value to test
 * @returns {boolean} True if value is a Function, otherwise false
 */
function isFunction(val) {
  return toString.call(val) === '[object Function]';
}

/**
 * Determine if a value is a Stream
 *
 * @param {Object} val The value to test
 * @returns {boolean} True if value is a Stream, otherwise false
 */
function isStream(val) {
  return isObject(val) && isFunction(val.pipe);
}

/**
 * Determine if a value is a URLSearchParams object
 *
 * @param {Object} val The value to test
 * @returns {boolean} True if value is a URLSearchParams object, otherwise false
 */
function isURLSearchParams(val) {
  return typeof URLSearchParams !== 'undefined' && val instanceof URLSearchParams;
}

/**
 * Trim excess whitespace off the beginning and end of a string
 *
 * @param {String} str The String to trim
 * @returns {String} The String freed of excess whitespace
 */
function trim(str) {
  return str.replace(/^\s*/, '').replace(/\s*$/, '');
}

/**
 * Determine if we're running in a standard browser environment
 *
 * This allows axios to run in a web worker, and react-native.
 * Both environments support XMLHttpRequest, but not fully standard globals.
 *
 * web workers:
 *  typeof window -> undefined
 *  typeof document -> undefined
 *
 * react-native:
 *  navigator.product -> 'ReactNative'
 */
function isStandardBrowserEnv() {
  if (typeof navigator !== 'undefined' && navigator.product === 'ReactNative') {
    return false;
  }
  return (
    typeof window !== 'undefined' &&
    typeof document !== 'undefined'
  );
}

/**
 * Iterate over an Array or an Object invoking a function for each item.
 *
 * If `obj` is an Array callback will be called passing
 * the value, index, and complete array for each item.
 *
 * If 'obj' is an Object callback will be called passing
 * the value, key, and complete object for each property.
 *
 * @param {Object|Array} obj The object to iterate
 * @param {Function} fn The callback to invoke for each item
 */
function forEach(obj, fn) {
  // Don't bother if no value provided
  if (obj === null || typeof obj === 'undefined') {
    return;
  }

  // Force an array if not already something iterable
  if (typeof obj !== 'object') {
    /*eslint no-param-reassign:0*/
    obj = [obj];
  }

  if (isArray(obj)) {
    // Iterate over array values
    for (var i = 0, l = obj.length; i < l; i++) {
      fn.call(null, obj[i], i, obj);
    }
  } else {
    // Iterate over object keys
    for (var key in obj) {
      if (Object.prototype.hasOwnProperty.call(obj, key)) {
        fn.call(null, obj[key], key, obj);
      }
    }
  }
}

/**
 * Accepts varargs expecting each argument to be an object, then
 * immutably merges the properties of each object and returns result.
 *
 * When multiple objects contain the same key the later object in
 * the arguments list will take precedence.
 *
 * Example:
 *
 * ```js
 * var result = merge({foo: 123}, {foo: 456});
 * console.log(result.foo); // outputs 456
 * ```
 *
 * @param {Object} obj1 Object to merge
 * @returns {Object} Result of all merge properties
 */
function merge(/* obj1, obj2, obj3, ... */) {
  var result = {};
  function assignValue(val, key) {
    if (typeof result[key] === 'object' && typeof val === 'object') {
      result[key] = merge(result[key], val);
    } else {
      result[key] = val;
    }
  }

  for (var i = 0, l = arguments.length; i < l; i++) {
    forEach(arguments[i], assignValue);
  }
  return result;
}

/**
 * Extends object a by mutably adding to it the properties of object b.
 *
 * @param {Object} a The object to be extended
 * @param {Object} b The object to copy properties from
 * @param {Object} thisArg The object to bind function to
 * @return {Object} The resulting value of object a
 */
function extend(a, b, thisArg) {
  forEach(b, function assignValue(val, key) {
    if (thisArg && typeof val === 'function') {
      a[key] = bind(val, thisArg);
    } else {
      a[key] = val;
    }
  });
  return a;
}

var utils = {
  isArray: isArray,
  isArrayBuffer: isArrayBuffer,
  isBuffer: isBuffer_1,
  isFormData: isFormData,
  isArrayBufferView: isArrayBufferView,
  isString: isString,
  isNumber: isNumber,
  isObject: isObject,
  isUndefined: isUndefined,
  isDate: isDate,
  isFile: isFile,
  isBlob: isBlob,
  isFunction: isFunction,
  isStream: isStream,
  isURLSearchParams: isURLSearchParams,
  isStandardBrowserEnv: isStandardBrowserEnv,
  forEach: forEach,
  merge: merge,
  extend: extend,
  trim: trim
};

var global$1 = (typeof global !== "undefined" ? global :
            typeof self !== "undefined" ? self :
            typeof window !== "undefined" ? window : {});

// shim for using process in browser
// based off https://github.com/defunctzombie/node-process/blob/master/browser.js

function defaultSetTimout() {
    throw new Error('setTimeout has not been defined');
}
function defaultClearTimeout () {
    throw new Error('clearTimeout has not been defined');
}
var cachedSetTimeout = defaultSetTimout;
var cachedClearTimeout = defaultClearTimeout;
if (typeof global$1.setTimeout === 'function') {
    cachedSetTimeout = setTimeout;
}
if (typeof global$1.clearTimeout === 'function') {
    cachedClearTimeout = clearTimeout;
}

function runTimeout(fun) {
    if (cachedSetTimeout === setTimeout) {
        //normal enviroments in sane situations
        return setTimeout(fun, 0);
    }
    // if setTimeout wasn't available but was latter defined
    if ((cachedSetTimeout === defaultSetTimout || !cachedSetTimeout) && setTimeout) {
        cachedSetTimeout = setTimeout;
        return setTimeout(fun, 0);
    }
    try {
        // when when somebody has screwed with setTimeout but no I.E. maddness
        return cachedSetTimeout(fun, 0);
    } catch(e){
        try {
            // When we are in I.E. but the script has been evaled so I.E. doesn't trust the global object when called normally
            return cachedSetTimeout.call(null, fun, 0);
        } catch(e){
            // same as above but when it's a version of I.E. that must have the global object for 'this', hopfully our context correct otherwise it will throw a global error
            return cachedSetTimeout.call(this, fun, 0);
        }
    }


}
function runClearTimeout(marker) {
    if (cachedClearTimeout === clearTimeout) {
        //normal enviroments in sane situations
        return clearTimeout(marker);
    }
    // if clearTimeout wasn't available but was latter defined
    if ((cachedClearTimeout === defaultClearTimeout || !cachedClearTimeout) && clearTimeout) {
        cachedClearTimeout = clearTimeout;
        return clearTimeout(marker);
    }
    try {
        // when when somebody has screwed with setTimeout but no I.E. maddness
        return cachedClearTimeout(marker);
    } catch (e){
        try {
            // When we are in I.E. but the script has been evaled so I.E. doesn't  trust the global object when called normally
            return cachedClearTimeout.call(null, marker);
        } catch (e){
            // same as above but when it's a version of I.E. that must have the global object for 'this', hopfully our context correct otherwise it will throw a global error.
            // Some versions of I.E. have different rules for clearTimeout vs setTimeout
            return cachedClearTimeout.call(this, marker);
        }
    }



}
var queue = [];
var draining = false;
var currentQueue;
var queueIndex = -1;

function cleanUpNextTick() {
    if (!draining || !currentQueue) {
        return;
    }
    draining = false;
    if (currentQueue.length) {
        queue = currentQueue.concat(queue);
    } else {
        queueIndex = -1;
    }
    if (queue.length) {
        drainQueue();
    }
}

function drainQueue() {
    if (draining) {
        return;
    }
    var timeout = runTimeout(cleanUpNextTick);
    draining = true;

    var len = queue.length;
    while(len) {
        currentQueue = queue;
        queue = [];
        while (++queueIndex < len) {
            if (currentQueue) {
                currentQueue[queueIndex].run();
            }
        }
        queueIndex = -1;
        len = queue.length;
    }
    currentQueue = null;
    draining = false;
    runClearTimeout(timeout);
}
function nextTick(fun) {
    var args = new Array(arguments.length - 1);
    if (arguments.length > 1) {
        for (var i = 1; i < arguments.length; i++) {
            args[i - 1] = arguments[i];
        }
    }
    queue.push(new Item(fun, args));
    if (queue.length === 1 && !draining) {
        runTimeout(drainQueue);
    }
}
// v8 likes predictible objects
function Item(fun, array) {
    this.fun = fun;
    this.array = array;
}
Item.prototype.run = function () {
    this.fun.apply(null, this.array);
};
var title = 'browser';
var platform = 'browser';
var browser = true;
var env = {};
var argv = [];
var version = ''; // empty string to avoid regexp issues
var versions = {};
var release = {};
var config = {};

function noop() {}

var on = noop;
var addListener = noop;
var once = noop;
var off = noop;
var removeListener = noop;
var removeAllListeners = noop;
var emit = noop;

function binding(name) {
    throw new Error('process.binding is not supported');
}

function cwd () { return '/' }
function chdir (dir) {
    throw new Error('process.chdir is not supported');
}function umask() { return 0; }

// from https://github.com/kumavis/browser-process-hrtime/blob/master/index.js
var performance = global$1.performance || {};
var performanceNow =
  performance.now        ||
  performance.mozNow     ||
  performance.msNow      ||
  performance.oNow       ||
  performance.webkitNow  ||
  function(){ return (new Date()).getTime() };

// generate timestamp or delta
// see http://nodejs.org/api/process.html#process_process_hrtime
function hrtime(previousTimestamp){
  var clocktime = performanceNow.call(performance)*1e-3;
  var seconds = Math.floor(clocktime);
  var nanoseconds = Math.floor((clocktime%1)*1e9);
  if (previousTimestamp) {
    seconds = seconds - previousTimestamp[0];
    nanoseconds = nanoseconds - previousTimestamp[1];
    if (nanoseconds<0) {
      seconds--;
      nanoseconds += 1e9;
    }
  }
  return [seconds,nanoseconds]
}

var startTime = new Date();
function uptime() {
  var currentTime = new Date();
  var dif = currentTime - startTime;
  return dif / 1000;
}

var process = {
  nextTick: nextTick,
  title: title,
  browser: browser,
  env: env,
  argv: argv,
  version: version,
  versions: versions,
  on: on,
  addListener: addListener,
  once: once,
  off: off,
  removeListener: removeListener,
  removeAllListeners: removeAllListeners,
  emit: emit,
  binding: binding,
  cwd: cwd,
  chdir: chdir,
  umask: umask,
  hrtime: hrtime,
  platform: platform,
  release: release,
  config: config,
  uptime: uptime
};

var normalizeHeaderName = function normalizeHeaderName(headers, normalizedName) {
  utils.forEach(headers, function processHeader(value, name) {
    if (name !== normalizedName && name.toUpperCase() === normalizedName.toUpperCase()) {
      headers[normalizedName] = value;
      delete headers[name];
    }
  });
};

/**
 * Update an Error with the specified config, error code, and response.
 *
 * @param {Error} error The error to update.
 * @param {Object} config The config.
 * @param {string} [code] The error code (for example, 'ECONNABORTED').
 * @param {Object} [request] The request.
 * @param {Object} [response] The response.
 * @returns {Error} The error.
 */
var enhanceError = function enhanceError(error, config, code, request, response) {
  error.config = config;
  if (code) {
    error.code = code;
  }
  error.request = request;
  error.response = response;
  return error;
};

/**
 * Create an Error with the specified message, config, error code, request and response.
 *
 * @param {string} message The error message.
 * @param {Object} config The config.
 * @param {string} [code] The error code (for example, 'ECONNABORTED').
 * @param {Object} [request] The request.
 * @param {Object} [response] The response.
 * @returns {Error} The created error.
 */
var createError = function createError(message, config, code, request, response) {
  var error = new Error(message);
  return enhanceError(error, config, code, request, response);
};

/**
 * Resolve or reject a Promise based on response status.
 *
 * @param {Function} resolve A function that resolves the promise.
 * @param {Function} reject A function that rejects the promise.
 * @param {object} response The response.
 */
var settle = function settle(resolve, reject, response) {
  var validateStatus = response.config.validateStatus;
  // Note: status is not exposed by XDomainRequest
  if (!response.status || !validateStatus || validateStatus(response.status)) {
    resolve(response);
  } else {
    reject(createError(
      'Request failed with status code ' + response.status,
      response.config,
      null,
      response.request,
      response
    ));
  }
};

function encode(val) {
  return encodeURIComponent(val).
    replace(/%40/gi, '@').
    replace(/%3A/gi, ':').
    replace(/%24/g, '$').
    replace(/%2C/gi, ',').
    replace(/%20/g, '+').
    replace(/%5B/gi, '[').
    replace(/%5D/gi, ']');
}

/**
 * Build a URL by appending params to the end
 *
 * @param {string} url The base of the url (e.g., http://www.google.com)
 * @param {object} [params] The params to be appended
 * @returns {string} The formatted url
 */
var buildURL = function buildURL(url, params, paramsSerializer) {
  /*eslint no-param-reassign:0*/
  if (!params) {
    return url;
  }

  var serializedParams;
  if (paramsSerializer) {
    serializedParams = paramsSerializer(params);
  } else if (utils.isURLSearchParams(params)) {
    serializedParams = params.toString();
  } else {
    var parts = [];

    utils.forEach(params, function serialize(val, key) {
      if (val === null || typeof val === 'undefined') {
        return;
      }

      if (utils.isArray(val)) {
        key = key + '[]';
      } else {
        val = [val];
      }

      utils.forEach(val, function parseValue(v) {
        if (utils.isDate(v)) {
          v = v.toISOString();
        } else if (utils.isObject(v)) {
          v = JSON.stringify(v);
        }
        parts.push(encode(key) + '=' + encode(v));
      });
    });

    serializedParams = parts.join('&');
  }

  if (serializedParams) {
    url += (url.indexOf('?') === -1 ? '?' : '&') + serializedParams;
  }

  return url;
};

// Headers whose duplicates are ignored by node
// c.f. https://nodejs.org/api/http.html#http_message_headers
var ignoreDuplicateOf = [
  'age', 'authorization', 'content-length', 'content-type', 'etag',
  'expires', 'from', 'host', 'if-modified-since', 'if-unmodified-since',
  'last-modified', 'location', 'max-forwards', 'proxy-authorization',
  'referer', 'retry-after', 'user-agent'
];

/**
 * Parse headers into an object
 *
 * ```
 * Date: Wed, 27 Aug 2014 08:58:49 GMT
 * Content-Type: application/json
 * Connection: keep-alive
 * Transfer-Encoding: chunked
 * ```
 *
 * @param {String} headers Headers needing to be parsed
 * @returns {Object} Headers parsed into an object
 */
var parseHeaders = function parseHeaders(headers) {
  var parsed = {};
  var key;
  var val;
  var i;

  if (!headers) { return parsed; }

  utils.forEach(headers.split('\n'), function parser(line) {
    i = line.indexOf(':');
    key = utils.trim(line.substr(0, i)).toLowerCase();
    val = utils.trim(line.substr(i + 1));

    if (key) {
      if (parsed[key] && ignoreDuplicateOf.indexOf(key) >= 0) {
        return;
      }
      if (key === 'set-cookie') {
        parsed[key] = (parsed[key] ? parsed[key] : []).concat([val]);
      } else {
        parsed[key] = parsed[key] ? parsed[key] + ', ' + val : val;
      }
    }
  });

  return parsed;
};

var isURLSameOrigin = (
  utils.isStandardBrowserEnv() ?

  // Standard browser envs have full support of the APIs needed to test
  // whether the request URL is of the same origin as current location.
  (function standardBrowserEnv() {
    var msie = /(msie|trident)/i.test(navigator.userAgent);
    var urlParsingNode = document.createElement('a');
    var originURL;

    /**
    * Parse a URL to discover it's components
    *
    * @param {String} url The URL to be parsed
    * @returns {Object}
    */
    function resolveURL(url) {
      var href = url;

      if (msie) {
        // IE needs attribute set twice to normalize properties
        urlParsingNode.setAttribute('href', href);
        href = urlParsingNode.href;
      }

      urlParsingNode.setAttribute('href', href);

      // urlParsingNode provides the UrlUtils interface - http://url.spec.whatwg.org/#urlutils
      return {
        href: urlParsingNode.href,
        protocol: urlParsingNode.protocol ? urlParsingNode.protocol.replace(/:$/, '') : '',
        host: urlParsingNode.host,
        search: urlParsingNode.search ? urlParsingNode.search.replace(/^\?/, '') : '',
        hash: urlParsingNode.hash ? urlParsingNode.hash.replace(/^#/, '') : '',
        hostname: urlParsingNode.hostname,
        port: urlParsingNode.port,
        pathname: (urlParsingNode.pathname.charAt(0) === '/') ?
                  urlParsingNode.pathname :
                  '/' + urlParsingNode.pathname
      };
    }

    originURL = resolveURL(window.location.href);

    /**
    * Determine if a URL shares the same origin as the current location
    *
    * @param {String} requestURL The URL to test
    * @returns {boolean} True if URL shares the same origin, otherwise false
    */
    return function isURLSameOrigin(requestURL) {
      var parsed = (utils.isString(requestURL)) ? resolveURL(requestURL) : requestURL;
      return (parsed.protocol === originURL.protocol &&
            parsed.host === originURL.host);
    };
  })() :

  // Non standard browser envs (web workers, react-native) lack needed support.
  (function nonStandardBrowserEnv() {
    return function isURLSameOrigin() {
      return true;
    };
  })()
);

// btoa polyfill for IE<10 courtesy https://github.com/davidchambers/Base64.js

var chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=';

function E() {
  this.message = 'String contains an invalid character';
}
E.prototype = new Error;
E.prototype.code = 5;
E.prototype.name = 'InvalidCharacterError';

function btoa(input) {
  var str = String(input);
  var output = '';
  for (
    // initialize result and counter
    var block, charCode, idx = 0, map = chars;
    // if the next str index does not exist:
    //   change the mapping table to "="
    //   check if d has no fractional digits
    str.charAt(idx | 0) || (map = '=', idx % 1);
    // "8 - idx % 1 * 8" generates the sequence 2, 4, 6, 8
    output += map.charAt(63 & block >> 8 - idx % 1 * 8)
  ) {
    charCode = str.charCodeAt(idx += 3 / 4);
    if (charCode > 0xFF) {
      throw new E();
    }
    block = block << 8 | charCode;
  }
  return output;
}

var btoa_1 = btoa;

var cookies = (
  utils.isStandardBrowserEnv() ?

  // Standard browser envs support document.cookie
  (function standardBrowserEnv() {
    return {
      write: function write(name, value, expires, path, domain, secure) {
        var cookie = [];
        cookie.push(name + '=' + encodeURIComponent(value));

        if (utils.isNumber(expires)) {
          cookie.push('expires=' + new Date(expires).toGMTString());
        }

        if (utils.isString(path)) {
          cookie.push('path=' + path);
        }

        if (utils.isString(domain)) {
          cookie.push('domain=' + domain);
        }

        if (secure === true) {
          cookie.push('secure');
        }

        document.cookie = cookie.join('; ');
      },

      read: function read(name) {
        var match = document.cookie.match(new RegExp('(^|;\\s*)(' + name + ')=([^;]*)'));
        return (match ? decodeURIComponent(match[3]) : null);
      },

      remove: function remove(name) {
        this.write(name, '', Date.now() - 86400000);
      }
    };
  })() :

  // Non standard browser env (web workers, react-native) lack needed support.
  (function nonStandardBrowserEnv() {
    return {
      write: function write() {},
      read: function read() { return null; },
      remove: function remove() {}
    };
  })()
);

var btoa$1 = (typeof window !== 'undefined' && window.btoa && window.btoa.bind(window)) || btoa_1;

var xhr = function xhrAdapter(config) {
  return new Promise(function dispatchXhrRequest(resolve, reject) {
    var requestData = config.data;
    var requestHeaders = config.headers;

    if (utils.isFormData(requestData)) {
      delete requestHeaders['Content-Type']; // Let the browser set it
    }

    var request = new XMLHttpRequest();
    var loadEvent = 'onreadystatechange';
    var xDomain = false;

    // For IE 8/9 CORS support
    // Only supports POST and GET calls and doesn't returns the response headers.
    // DON'T do this for testing b/c XMLHttpRequest is mocked, not XDomainRequest.
    if (typeof window !== 'undefined' &&
        window.XDomainRequest && !('withCredentials' in request) &&
        !isURLSameOrigin(config.url)) {
      request = new window.XDomainRequest();
      loadEvent = 'onload';
      xDomain = true;
      request.onprogress = function handleProgress() {};
      request.ontimeout = function handleTimeout() {};
    }

    // HTTP basic authentication
    if (config.auth) {
      var username = config.auth.username || '';
      var password = config.auth.password || '';
      requestHeaders.Authorization = 'Basic ' + btoa$1(username + ':' + password);
    }

    request.open(config.method.toUpperCase(), buildURL(config.url, config.params, config.paramsSerializer), true);

    // Set the request timeout in MS
    request.timeout = config.timeout;

    // Listen for ready state
    request[loadEvent] = function handleLoad() {
      if (!request || (request.readyState !== 4 && !xDomain)) {
        return;
      }

      // The request errored out and we didn't get a response, this will be
      // handled by onerror instead
      // With one exception: request that using file: protocol, most browsers
      // will return status as 0 even though it's a successful request
      if (request.status === 0 && !(request.responseURL && request.responseURL.indexOf('file:') === 0)) {
        return;
      }

      // Prepare the response
      var responseHeaders = 'getAllResponseHeaders' in request ? parseHeaders(request.getAllResponseHeaders()) : null;
      var responseData = !config.responseType || config.responseType === 'text' ? request.responseText : request.response;
      var response = {
        data: responseData,
        // IE sends 1223 instead of 204 (https://github.com/axios/axios/issues/201)
        status: request.status === 1223 ? 204 : request.status,
        statusText: request.status === 1223 ? 'No Content' : request.statusText,
        headers: responseHeaders,
        config: config,
        request: request
      };

      settle(resolve, reject, response);

      // Clean up request
      request = null;
    };

    // Handle low level network errors
    request.onerror = function handleError() {
      // Real errors are hidden from us by the browser
      // onerror should only fire if it's a network error
      reject(createError('Network Error', config, null, request));

      // Clean up request
      request = null;
    };

    // Handle timeout
    request.ontimeout = function handleTimeout() {
      reject(createError('timeout of ' + config.timeout + 'ms exceeded', config, 'ECONNABORTED',
        request));

      // Clean up request
      request = null;
    };

    // Add xsrf header
    // This is only done if running in a standard browser environment.
    // Specifically not if we're in a web worker, or react-native.
    if (utils.isStandardBrowserEnv()) {
      var cookies$$1 = cookies;

      // Add xsrf header
      var xsrfValue = (config.withCredentials || isURLSameOrigin(config.url)) && config.xsrfCookieName ?
          cookies$$1.read(config.xsrfCookieName) :
          undefined;

      if (xsrfValue) {
        requestHeaders[config.xsrfHeaderName] = xsrfValue;
      }
    }

    // Add headers to the request
    if ('setRequestHeader' in request) {
      utils.forEach(requestHeaders, function setRequestHeader(val, key) {
        if (typeof requestData === 'undefined' && key.toLowerCase() === 'content-type') {
          // Remove Content-Type if data is undefined
          delete requestHeaders[key];
        } else {
          // Otherwise add header to the request
          request.setRequestHeader(key, val);
        }
      });
    }

    // Add withCredentials to request if needed
    if (config.withCredentials) {
      request.withCredentials = true;
    }

    // Add responseType to request if needed
    if (config.responseType) {
      try {
        request.responseType = config.responseType;
      } catch (e) {
        // Expected DOMException thrown by browsers not compatible XMLHttpRequest Level 2.
        // But, this can be suppressed for 'json' type as it can be parsed by default 'transformResponse' function.
        if (config.responseType !== 'json') {
          throw e;
        }
      }
    }

    // Handle progress if needed
    if (typeof config.onDownloadProgress === 'function') {
      request.addEventListener('progress', config.onDownloadProgress);
    }

    // Not all browsers support upload events
    if (typeof config.onUploadProgress === 'function' && request.upload) {
      request.upload.addEventListener('progress', config.onUploadProgress);
    }

    if (config.cancelToken) {
      // Handle cancellation
      config.cancelToken.promise.then(function onCanceled(cancel) {
        if (!request) {
          return;
        }

        request.abort();
        reject(cancel);
        // Clean up request
        request = null;
      });
    }

    if (requestData === undefined) {
      requestData = null;
    }

    // Send the request
    request.send(requestData);
  });
};

var DEFAULT_CONTENT_TYPE = {
  'Content-Type': 'application/x-www-form-urlencoded'
};

function setContentTypeIfUnset(headers, value) {
  if (!utils.isUndefined(headers) && utils.isUndefined(headers['Content-Type'])) {
    headers['Content-Type'] = value;
  }
}

function getDefaultAdapter() {
  var adapter;
  if (typeof XMLHttpRequest !== 'undefined') {
    // For browsers use XHR adapter
    adapter = xhr;
  } else if (typeof process !== 'undefined') {
    // For node use HTTP adapter
    adapter = xhr;
  }
  return adapter;
}

var defaults = {
  adapter: getDefaultAdapter(),

  transformRequest: [function transformRequest(data, headers) {
    normalizeHeaderName(headers, 'Content-Type');
    if (utils.isFormData(data) ||
      utils.isArrayBuffer(data) ||
      utils.isBuffer(data) ||
      utils.isStream(data) ||
      utils.isFile(data) ||
      utils.isBlob(data)
    ) {
      return data;
    }
    if (utils.isArrayBufferView(data)) {
      return data.buffer;
    }
    if (utils.isURLSearchParams(data)) {
      setContentTypeIfUnset(headers, 'application/x-www-form-urlencoded;charset=utf-8');
      return data.toString();
    }
    if (utils.isObject(data)) {
      setContentTypeIfUnset(headers, 'application/json;charset=utf-8');
      return JSON.stringify(data);
    }
    return data;
  }],

  transformResponse: [function transformResponse(data) {
    /*eslint no-param-reassign:0*/
    if (typeof data === 'string') {
      try {
        data = JSON.parse(data);
      } catch (e) { /* Ignore */ }
    }
    return data;
  }],

  /**
   * A timeout in milliseconds to abort a request. If set to 0 (default) a
   * timeout is not created.
   */
  timeout: 0,

  xsrfCookieName: 'XSRF-TOKEN',
  xsrfHeaderName: 'X-XSRF-TOKEN',

  maxContentLength: -1,

  validateStatus: function validateStatus(status) {
    return status >= 200 && status < 300;
  }
};

defaults.headers = {
  common: {
    'Accept': 'application/json, text/plain, */*'
  }
};

utils.forEach(['delete', 'get', 'head'], function forEachMethodNoData(method) {
  defaults.headers[method] = {};
});

utils.forEach(['post', 'put', 'patch'], function forEachMethodWithData(method) {
  defaults.headers[method] = utils.merge(DEFAULT_CONTENT_TYPE);
});

var defaults_1 = defaults;

function InterceptorManager() {
  this.handlers = [];
}

/**
 * Add a new interceptor to the stack
 *
 * @param {Function} fulfilled The function to handle `then` for a `Promise`
 * @param {Function} rejected The function to handle `reject` for a `Promise`
 *
 * @return {Number} An ID used to remove interceptor later
 */
InterceptorManager.prototype.use = function use(fulfilled, rejected) {
  this.handlers.push({
    fulfilled: fulfilled,
    rejected: rejected
  });
  return this.handlers.length - 1;
};

/**
 * Remove an interceptor from the stack
 *
 * @param {Number} id The ID that was returned by `use`
 */
InterceptorManager.prototype.eject = function eject(id) {
  if (this.handlers[id]) {
    this.handlers[id] = null;
  }
};

/**
 * Iterate over all the registered interceptors
 *
 * This method is particularly useful for skipping over any
 * interceptors that may have become `null` calling `eject`.
 *
 * @param {Function} fn The function to call for each interceptor
 */
InterceptorManager.prototype.forEach = function forEach(fn) {
  utils.forEach(this.handlers, function forEachHandler(h) {
    if (h !== null) {
      fn(h);
    }
  });
};

var InterceptorManager_1 = InterceptorManager;

/**
 * Transform the data for a request or a response
 *
 * @param {Object|String} data The data to be transformed
 * @param {Array} headers The headers for the request or response
 * @param {Array|Function} fns A single function or Array of functions
 * @returns {*} The resulting transformed data
 */
var transformData = function transformData(data, headers, fns) {
  /*eslint no-param-reassign:0*/
  utils.forEach(fns, function transform(fn) {
    data = fn(data, headers);
  });

  return data;
};

var isCancel = function isCancel(value) {
  return !!(value && value.__CANCEL__);
};

/**
 * Determines whether the specified URL is absolute
 *
 * @param {string} url The URL to test
 * @returns {boolean} True if the specified URL is absolute, otherwise false
 */
var isAbsoluteURL = function isAbsoluteURL(url) {
  // A URL is considered absolute if it begins with "<scheme>://" or "//" (protocol-relative URL).
  // RFC 3986 defines scheme name as a sequence of characters beginning with a letter and followed
  // by any combination of letters, digits, plus, period, or hyphen.
  return /^([a-z][a-z\d\+\-\.]*:)?\/\//i.test(url);
};

/**
 * Creates a new URL by combining the specified URLs
 *
 * @param {string} baseURL The base URL
 * @param {string} relativeURL The relative URL
 * @returns {string} The combined URL
 */
var combineURLs = function combineURLs(baseURL, relativeURL) {
  return relativeURL
    ? baseURL.replace(/\/+$/, '') + '/' + relativeURL.replace(/^\/+/, '')
    : baseURL;
};

/**
 * Throws a `Cancel` if cancellation has been requested.
 */
function throwIfCancellationRequested(config) {
  if (config.cancelToken) {
    config.cancelToken.throwIfRequested();
  }
}

/**
 * Dispatch a request to the server using the configured adapter.
 *
 * @param {object} config The config that is to be used for the request
 * @returns {Promise} The Promise to be fulfilled
 */
var dispatchRequest = function dispatchRequest(config) {
  throwIfCancellationRequested(config);

  // Support baseURL config
  if (config.baseURL && !isAbsoluteURL(config.url)) {
    config.url = combineURLs(config.baseURL, config.url);
  }

  // Ensure headers exist
  config.headers = config.headers || {};

  // Transform request data
  config.data = transformData(
    config.data,
    config.headers,
    config.transformRequest
  );

  // Flatten headers
  config.headers = utils.merge(
    config.headers.common || {},
    config.headers[config.method] || {},
    config.headers || {}
  );

  utils.forEach(
    ['delete', 'get', 'head', 'post', 'put', 'patch', 'common'],
    function cleanHeaderConfig(method) {
      delete config.headers[method];
    }
  );

  var adapter = config.adapter || defaults_1.adapter;

  return adapter(config).then(function onAdapterResolution(response) {
    throwIfCancellationRequested(config);

    // Transform response data
    response.data = transformData(
      response.data,
      response.headers,
      config.transformResponse
    );

    return response;
  }, function onAdapterRejection(reason) {
    if (!isCancel(reason)) {
      throwIfCancellationRequested(config);

      // Transform response data
      if (reason && reason.response) {
        reason.response.data = transformData(
          reason.response.data,
          reason.response.headers,
          config.transformResponse
        );
      }
    }

    return Promise.reject(reason);
  });
};

/**
 * Create a new instance of Axios
 *
 * @param {Object} instanceConfig The default config for the instance
 */
function Axios(instanceConfig) {
  this.defaults = instanceConfig;
  this.interceptors = {
    request: new InterceptorManager_1(),
    response: new InterceptorManager_1()
  };
}

/**
 * Dispatch a request
 *
 * @param {Object} config The config specific for this request (merged with this.defaults)
 */
Axios.prototype.request = function request(config) {
  /*eslint no-param-reassign:0*/
  // Allow for axios('example/url'[, config]) a la fetch API
  if (typeof config === 'string') {
    config = utils.merge({
      url: arguments[0]
    }, arguments[1]);
  }

  config = utils.merge(defaults_1, {method: 'get'}, this.defaults, config);
  config.method = config.method.toLowerCase();

  // Hook up interceptors middleware
  var chain = [dispatchRequest, undefined];
  var promise = Promise.resolve(config);

  this.interceptors.request.forEach(function unshiftRequestInterceptors(interceptor) {
    chain.unshift(interceptor.fulfilled, interceptor.rejected);
  });

  this.interceptors.response.forEach(function pushResponseInterceptors(interceptor) {
    chain.push(interceptor.fulfilled, interceptor.rejected);
  });

  while (chain.length) {
    promise = promise.then(chain.shift(), chain.shift());
  }

  return promise;
};

// Provide aliases for supported request methods
utils.forEach(['delete', 'get', 'head', 'options'], function forEachMethodNoData(method) {
  /*eslint func-names:0*/
  Axios.prototype[method] = function(url, config) {
    return this.request(utils.merge(config || {}, {
      method: method,
      url: url
    }));
  };
});

utils.forEach(['post', 'put', 'patch'], function forEachMethodWithData(method) {
  /*eslint func-names:0*/
  Axios.prototype[method] = function(url, data, config) {
    return this.request(utils.merge(config || {}, {
      method: method,
      url: url,
      data: data
    }));
  };
});

var Axios_1 = Axios;

/**
 * A `Cancel` is an object that is thrown when an operation is canceled.
 *
 * @class
 * @param {string=} message The message.
 */
function Cancel(message) {
  this.message = message;
}

Cancel.prototype.toString = function toString() {
  return 'Cancel' + (this.message ? ': ' + this.message : '');
};

Cancel.prototype.__CANCEL__ = true;

var Cancel_1 = Cancel;

/**
 * A `CancelToken` is an object that can be used to request cancellation of an operation.
 *
 * @class
 * @param {Function} executor The executor function.
 */
function CancelToken(executor) {
  if (typeof executor !== 'function') {
    throw new TypeError('executor must be a function.');
  }

  var resolvePromise;
  this.promise = new Promise(function promiseExecutor(resolve) {
    resolvePromise = resolve;
  });

  var token = this;
  executor(function cancel(message) {
    if (token.reason) {
      // Cancellation has already been requested
      return;
    }

    token.reason = new Cancel_1(message);
    resolvePromise(token.reason);
  });
}

/**
 * Throws a `Cancel` if cancellation has been requested.
 */
CancelToken.prototype.throwIfRequested = function throwIfRequested() {
  if (this.reason) {
    throw this.reason;
  }
};

/**
 * Returns an object that contains a new `CancelToken` and a function that, when called,
 * cancels the `CancelToken`.
 */
CancelToken.source = function source() {
  var cancel;
  var token = new CancelToken(function executor(c) {
    cancel = c;
  });
  return {
    token: token,
    cancel: cancel
  };
};

var CancelToken_1 = CancelToken;

/**
 * Syntactic sugar for invoking a function and expanding an array for arguments.
 *
 * Common use case would be to use `Function.prototype.apply`.
 *
 *  ```js
 *  function f(x, y, z) {}
 *  var args = [1, 2, 3];
 *  f.apply(null, args);
 *  ```
 *
 * With `spread` this example can be re-written.
 *
 *  ```js
 *  spread(function(x, y, z) {})([1, 2, 3]);
 *  ```
 *
 * @param {Function} callback
 * @returns {Function}
 */
var spread = function spread(callback) {
  return function wrap(arr) {
    return callback.apply(null, arr);
  };
};

/**
 * Create an instance of Axios
 *
 * @param {Object} defaultConfig The default config for the instance
 * @return {Axios} A new instance of Axios
 */
function createInstance(defaultConfig) {
  var context = new Axios_1(defaultConfig);
  var instance = bind(Axios_1.prototype.request, context);

  // Copy axios.prototype to instance
  utils.extend(instance, Axios_1.prototype, context);

  // Copy context to instance
  utils.extend(instance, context);

  return instance;
}

// Create the default instance to be exported
var axios = createInstance(defaults_1);

// Expose Axios class to allow class inheritance
axios.Axios = Axios_1;

// Factory for creating new instances
axios.create = function create(instanceConfig) {
  return createInstance(utils.merge(defaults_1, instanceConfig));
};

// Expose Cancel & CancelToken
axios.Cancel = Cancel_1;
axios.CancelToken = CancelToken_1;
axios.isCancel = isCancel;

// Expose all/spread
axios.all = function all(promises) {
  return Promise.all(promises);
};
axios.spread = spread;

var axios_1 = axios;

// Allow use of default import syntax in TypeScript
var default_1 = axios;
axios_1.default = default_1;

var axios$1 = axios_1;

class WvSitemapManager {
    constructor() {
        this.sitemapObj = null;
        this.nodePageDict = null;
        this.isAreaModalVisible = false;
        this.managedArea = null;
        this.isNodeModalVisible = false;
        this.managedNodeObj = { areaId: null, node: null };
        this.apiResponse = { message: "", errors: [], success: true };
        this.nodeAuxData = null;
    }
    componentWillLoad() {
        if (this.initData) {
            var initDataObj = JSON.parse(this.initData);
            this.sitemapObj = initDataObj["sitemap"];
            this.nodePageDict = initDataObj["node_page_dict"];
        }
    }
    createArea() {
        this.isAreaModalVisible = true;
        this.managedArea = null;
    }
    areaManageEventHandler(event) {
        this.isAreaModalVisible = true;
        this.managedArea = Object.assign({}, event.detail);
    }
    areaModalClose() {
        this.isAreaModalVisible = false;
        this.managedArea = null;
        this.apiResponse = { message: "", errors: [], success: true };
    }
    areaSubmittedEventHandler(event) {
        this.apiResponse = { message: "", errors: [], success: true };
        let submittedArea = event.detail;
        let apiUrl = this.apiRoot + "sitemap/area";
        if (submittedArea != null && submittedArea["id"]) {
            apiUrl += "/" + submittedArea["id"];
        }
        apiUrl += "?appId=" + this.appId;
        let requestConfig = {
            headers: {
                'Content-Type': 'application/json;charset=UTF-8',
                'Accept': 'application/json'
            }
        };
        let thisEl = this;
        let requestBody = JSON.stringify(submittedArea);
        axios$1.post(apiUrl, requestBody, requestConfig)
            .then(function (response) {
            let responseData = response.data;
            if (response.status !== 200 || responseData == null || !responseData["success"]) {
                thisEl.apiResponse = Object.assign({}, responseData);
                thisEl.managedArea = Object.assign({}, submittedArea);
                return;
            }
            thisEl.sitemapObj = Object.assign({}, responseData.object["sitemap"]);
            thisEl.nodePageDict = Object.assign({}, responseData.object["node_page_dict"]);
            thisEl.areaModalClose();
        })
            .catch(function (err) {
            var responseError = {
                success: false,
                message: err
            };
            thisEl.apiResponse = Object.assign({}, responseError);
            thisEl.managedArea = Object.assign({}, submittedArea);
        });
    }
    areaDeleteEventHandler(event) {
        this.apiResponse = { message: "", errors: [], success: true };
        let areaId = event.detail;
        let apiUrl = this.apiRoot + "sitemap/area/" + areaId + "/delete" + "?appId=" + this.appId;
        let thisEl = this;
        let requestConfig = {
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json',
            }
        };
        axios$1.post(apiUrl, null, requestConfig)
            .then(function (response) {
            let responseData = response.data;
            if (response.status !== 200 || responseData == null || !responseData["success"]) {
                alert(responseData.message);
                return;
            }
            thisEl.sitemapObj = Object.assign({}, responseData.object["sitemap"]);
            thisEl.nodePageDict = Object.assign({}, responseData.object["node_page_dict"]);
            thisEl.areaModalClose();
        })
            .catch(function (err) {
            alert(err.message);
        });
    }
    nodeManageEventHandler(event) {
        this.isNodeModalVisible = true;
        this.managedNodeObj = Object.assign({}, event.detail);
    }
    nodeModalCloseEventHandler() {
        this.isNodeModalVisible = false;
        this.managedNodeObj = { areaId: null, node: null };
        this.apiResponse = { message: "", errors: [], success: true };
    }
    nodeSubmittedEventHandler(event) {
        this.apiResponse = { message: "", errors: [], success: true };
        let submittedNode = event.detail.node;
        let areaId = event.detail.areaId;
        let apiUrl = this.apiRoot + "sitemap/node";
        if (submittedNode != null && submittedNode["id"] != null) {
            apiUrl += "/" + submittedNode["id"];
        }
        apiUrl += "?appId=" + this.appId + "&areaId=" + areaId;
        let requestConfig = {
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json',
            }
        };
        let thisEl = this;
        axios$1.post(apiUrl, JSON.stringify(submittedNode), requestConfig)
            .then(function (response) {
            let responseData = response.data;
            if (response.status !== 200 || responseData == null || !responseData["success"]) {
                thisEl.apiResponse = Object.assign({}, responseData);
                thisEl.managedNodeObj = Object.assign({}, event.detail);
                return;
            }
            thisEl.sitemapObj = Object.assign({}, responseData.object["sitemap"]);
            thisEl.nodePageDict = Object.assign({}, responseData.object["node_page_dict"]);
            thisEl.nodeModalCloseEventHandler();
            thisEl.nodeAuxDataUpdateEventHandler(null);
        })
            .catch(function (err) {
            var responseError = {
                success: false,
                message: err
            };
            thisEl.apiResponse = Object.assign({}, responseError);
            thisEl.managedNodeObj = Object.assign({}, event.detail);
        });
    }
    nodeDeleteEventHandler(event) {
        this.apiResponse = { message: "", errors: [], success: true };
        let nodeId = event.detail;
        let apiUrl = this.apiRoot + "sitemap/node/" + nodeId + "/delete" + "?appId=" + this.appId;
        let requestConfig = {
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json',
            }
        };
        let thisEl = this;
        axios$1.post(apiUrl, null, requestConfig)
            .then(function (response) {
            let responseData = response.data;
            if (response.status !== 200 || responseData == null || !responseData["success"]) {
                alert(responseData.message);
                return;
            }
            thisEl.sitemapObj = Object.assign({}, responseData.object["sitemap"]);
            thisEl.nodePageDict = Object.assign({}, responseData.object["node_page_dict"]);
            thisEl.nodeModalCloseEventHandler();
        })
            .catch(function (err) {
            alert(err.message);
        });
    }
    nodeAuxDataUpdateEventHandler(event) {
        if (event != null) {
            var newNodeAuxData = {
                allEntities: event.detail.allEntities,
                nodeTypes: event.detail.nodeTypes,
                appPages: event.detail.appPages,
                allEntityPages: event.detail.allEntityPages,
                nodePageDict: event.detail.nodePageDict
            };
            this.nodeAuxData = Object.assign({}, newNodeAuxData);
            this.managedNodeObj = Object.assign({}, event.detail["selectedNodeObj"]);
        }
        else {
            this.nodeAuxData = null;
            this.managedNodeObj = null;
        }
    }
    render() {
        return (h("div", { id: "sitemap-manager" },
            h("div", { class: "btn-group btn-group-sm mb-2" },
                h("button", { type: "button", class: "btn btn-white", onClick: () => this.createArea() },
                    h("span", { class: "fa fa-plus go-green" }),
                    " add area")),
            this.sitemapObj["areas"].map((area) => (h("wv-sitemap-manager-area", { area: area }))),
            this.isAreaModalVisible ? (h("wv-sitemap-area-modal", { submitResponse: this.apiResponse, area: this.managedArea })) : null,
            this.isNodeModalVisible ? (h("wv-sitemap-node-modal", { nodePageDict: this.nodePageDict, nodeAuxData: this.nodeAuxData, appId: this.appId, submitResponse: this.apiResponse, nodeObj: this.managedNodeObj, apiRoot: this.apiRoot })) : null));
    }
    static get is() { return "wv-sitemap-manager"; }
    static get properties() { return {
        "apiResponse": {
            "state": true
        },
        "apiRoot": {
            "type": String,
            "attr": "api-root"
        },
        "appId": {
            "type": String,
            "attr": "app-id"
        },
        "initData": {
            "type": String,
            "attr": "init-data"
        },
        "isAreaModalVisible": {
            "state": true
        },
        "isNodeModalVisible": {
            "state": true
        },
        "managedArea": {
            "state": true
        },
        "managedNodeObj": {
            "state": true
        },
        "nodeAuxData": {
            "state": true
        },
        "nodePageDict": {
            "state": true
        },
        "sitemapObj": {
            "state": true
        }
    }; }
    static get listeners() { return [{
            "name": "wvSitemapManagerAreaManageEvent",
            "method": "areaManageEventHandler"
        }, {
            "name": "wvSitemapManagerAreaModalCloseEvent",
            "method": "areaModalClose"
        }, {
            "name": "wvSitemapManagerAreaSubmittedEvent",
            "method": "areaSubmittedEventHandler"
        }, {
            "name": "wvSitemapManagerAreaDeleteEvent",
            "method": "areaDeleteEventHandler"
        }, {
            "name": "wvSitemapManagerNodeManageEvent",
            "method": "nodeManageEventHandler"
        }, {
            "name": "wvSitemapManagerNodeModalCloseEvent",
            "method": "nodeModalCloseEventHandler"
        }, {
            "name": "wvSitemapManagerNodeSubmittedEvent",
            "method": "nodeSubmittedEventHandler"
        }, {
            "name": "wvSitemapManagerNodeDeleteEvent",
            "method": "nodeDeleteEventHandler"
        }, {
            "name": "wvSitemapManagerNodeAuxDataUpdateEvent",
            "method": "nodeAuxDataUpdateEventHandler"
        }]; }
}

class WvSitemapManagerArea {
    manageArea() {
        this.wvSitemapManagerAreaManageEvent.emit(this.area);
    }
    deleteArea(event) {
        if (confirm("Are you sure?")) {
            this.wvSitemapManagerAreaDeleteEvent.emit(this.area["id"]);
        }
        else {
            event.preventDefault();
        }
    }
    createNode() {
        var submitObj = {
            node: null,
            areaId: this.area["id"]
        };
        this.wvSitemapManagerNodeManageEvent.emit(submitObj);
    }
    manageNode(node) {
        var submitObj = {
            node: node,
            areaId: this.area["id"]
        };
        this.wvSitemapManagerNodeManageEvent.emit(submitObj);
    }
    deleteNode(event, node) {
        if (confirm("Are you sure?")) {
            this.wvSitemapManagerNodeDeleteEvent.emit(node["id"]);
        }
        else {
            event.preventDefault();
        }
    }
    render() {
        var areaCmpt = this;
        var areaColor = "#999";
        if (this.area["color"]) {
            areaColor = this.area["color"];
        }
        var areaIconClass = "far fa-question-circle";
        if (this.area["icon_class"]) {
            areaIconClass = this.area["icon_class"];
        }
        return (h("div", { class: "sitemap-area mb-3" },
            h("div", { class: "area-header" },
                h("span", { class: "icon " + areaIconClass, style: { backgroundColor: areaColor } }),
                h("div", { class: "label" },
                    "(",
                    this.area["weight"],
                    ") ",
                    this.area["label"]),
                h("div", { class: "btn-group btn-group-sm action" },
                    h("button", { type: "button", class: "btn btn-link", onClick: (e) => this.deleteArea(e) },
                        h("span", { class: "fa fa-trash-alt go-red" }),
                        " delete"),
                    h("button", { type: "button", class: "btn btn-link", onClick: () => this.manageArea() },
                        h("span", { class: "fa fa-cog" }),
                        " config"))),
            h("div", { class: "area-body " + (this.area["nodes"].length > 0 ? "" : "d-none") },
                h("button", { type: "button", class: "btn btn-white btn-sm", onClick: () => this.createNode() },
                    h("span", { class: "fa fa-plus" }),
                    " add area node"),
                h("table", { class: "table table-bordered table-sm mb-0 sitemap-nodes mt-3" },
                    h("thead", null,
                        h("tr", null,
                            h("th", { style: { width: "40px" } }, "w."),
                            h("th", { style: { width: "40px" } }, "icon"),
                            h("th", { style: { width: "200px" } }, "name"),
                            h("th", { style: { width: "auto" } }, "label"),
                            h("th", { style: { width: "200px" } }, "group"),
                            h("th", { style: { width: "100px" } }, "type"),
                            h("th", { style: { width: "160px" } }, "action"))),
                    h("tbody", null, this.area["nodes"].map(function (node) {
                        var typeString = "";
                        switch (node["type"]) {
                            case 1:
                                typeString = "entity list";
                                break;
                            case 2:
                                typeString = "application";
                                break;
                            case 3:
                                typeString = "url";
                                break;
                            case 4:
                                typeString = "site";
                                break;
                            default:
                                break;
                        }
                        return (h("tr", null,
                            h("td", null, node["weight"]),
                            h("td", null,
                                h("span", { class: "icon " + node["icon_class"] })),
                            h("td", null, node["name"]),
                            h("td", null, node["label"]),
                            h("td", null, node["group_name"]),
                            h("td", null, typeString),
                            h("td", null,
                                h("div", { class: "btn-group btn-group-sm action" },
                                    h("button", { type: "button", class: "btn btn-link", onClick: (e) => areaCmpt.deleteNode(e, node) },
                                        h("span", { class: "fa fa-trash-alt go-red" }),
                                        " delete"),
                                    h("button", { type: "button", class: "btn btn-link", onClick: () => areaCmpt.manageNode(node) },
                                        h("span", { class: "fa fa-cog" }),
                                        " config")))));
                    })))),
            h("div", { class: "area-body " + (this.area["nodes"].length > 0 ? "d-none" : "") },
                h("button", { type: "button", class: "btn btn-white btn-sm", onClick: () => this.createNode() },
                    h("span", { class: "fa fa-plus" }),
                    " add area node"),
                h("div", { class: "alert alert-info mt-3" }, "No nodes in this area."))));
    }
    static get is() { return "wv-sitemap-manager-area"; }
    static get properties() { return {
        "area": {
            "type": "Any",
            "attr": "area"
        }
    }; }
    static get events() { return [{
            "name": "wvSitemapManagerAreaManageEvent",
            "method": "wvSitemapManagerAreaManageEvent",
            "bubbles": true,
            "cancelable": true,
            "composed": true
        }, {
            "name": "wvSitemapManagerAreaDeleteEvent",
            "method": "wvSitemapManagerAreaDeleteEvent",
            "bubbles": true,
            "cancelable": true,
            "composed": true
        }, {
            "name": "wvSitemapManagerNodeManageEvent",
            "method": "wvSitemapManagerNodeManageEvent",
            "bubbles": true,
            "cancelable": true,
            "composed": true
        }, {
            "name": "wvSitemapManagerNodeDeleteEvent",
            "method": "wvSitemapManagerNodeDeleteEvent",
            "bubbles": true,
            "cancelable": true,
            "composed": true
        }]; }
}

class WvSitemapNodeModal {
    constructor() {
        this.nodeObj = { areaId: null, node: null };
        this.nodePageDict = null;
        this.submitResponse = { message: "", errors: [], success: true };
        this.modalNodeObj = { areaId: null, node: {}, node_pages: [] };
    }
    componentWillLoad() {
        var backdropId = "wv-sitemap-manager-area-modal-backdrop";
        var backdropDomEl = document.getElementById(backdropId);
        if (!backdropDomEl) {
            var backdropEl = document.createElement('div');
            backdropEl.className = "modal-backdrop show";
            backdropEl.id = backdropId;
            document.body.appendChild(backdropEl);
        }
        if (this.nodeAuxData == null) {
            this.LoadData();
        }
        if (this.nodeObj["node"]) {
            this.modalNodeObj["node"] = Object.assign({}, this.nodeObj["node"]);
            if (!this.modalNodeObj["node"]["pages"]) {
                this.modalNodeObj["node"]["pages"] = [];
            }
        }
        else {
            this.modalNodeObj["node"] = { pages: [] };
        }
        this.modalNodeObj["areaId"] = this.nodeObj["areaId"];
        if (this.nodeObj["node"] && this.nodePageDict && this.nodePageDict[this.nodeObj["node"]["id"]]) {
            this.modalNodeObj["node_pages"] = this.nodePageDict[this.nodeObj["node"]["id"]];
            this.modalNodeObj["node_pages"].forEach(element => {
                this.modalNodeObj["node"]["pages"].push(element["value"]);
            });
        }
    }
    componentDidUnload() {
        var backdropId = "wv-sitemap-manager-area-modal-backdrop";
        var backdropDomEl = document.getElementById(backdropId);
        if (backdropDomEl) {
            backdropDomEl.remove();
        }
    }
    LoadData() {
        let apiUrl = this.apiRoot + "sitemap/node/get-aux-info" + "?appId=" + this.appId;
        let thisEl = this;
        axios$1.get(apiUrl, {
            method: 'GET',
            headers: new Headers({
                'Content-Type': 'application/json',
                'Accept-Encoding': 'gzip',
                'Accept': 'application/json',
            })
        })
            .then(function (response) {
            let responseData = response.data;
            if (response.status !== 200 || responseData == null || !responseData["success"]) {
                if (responseData != null) {
                    alert(responseData["message"]);
                }
                else {
                    alert("Error: " + response.status + " - " + response.statusText);
                }
                return;
            }
            var dataAuxObj = {};
            dataAuxObj["allEntities"] = responseData["object"]["all_entities"];
            dataAuxObj["nodeTypes"] = responseData["object"]["node_types"];
            dataAuxObj["appPages"] = responseData["object"]["app_pages"];
            dataAuxObj["allEntityPages"] = responseData["object"]["all_entity_pages"];
            dataAuxObj["nodePageDict"] = responseData["object"]["node_page_dict"];
            dataAuxObj["selectedNodeObj"] = thisEl.nodeObj;
            thisEl.wvSitemapManagerNodeAuxDataUpdateEvent.emit(dataAuxObj);
        })
            .catch(function (err) {
            alert(err.message);
        });
    }
    closeModal() {
        this.wvSitemapManagerNodeModalCloseEvent.emit();
    }
    handleSubmit(e) {
        e.preventDefault();
        this.wvSitemapManagerNodeSubmittedEvent.emit(this.modalNodeObj);
    }
    handleChange(event) {
        let propertyName = event.target.getAttribute('name');
        this.modalNodeObj["node"][propertyName] = event.target.value;
    }
    handleCheckboxChange(event) {
        let propertyName = event.target.getAttribute('name');
        let isChecked = event.target.checked;
        this.modalNodeObj["node"][propertyName] = isChecked;
    }
    handleSelectChange(event) {
        let propertyName = event.target.getAttribute('name');
        let newObj = Object.assign({}, this.modalNodeObj);
        newObj["node"][propertyName] = [];
        for (var i = 0; i < event.target.options.length; i++) {
            var option = event.target.options[i];
            if (option.selected) {
                newObj["node"][propertyName].push(String(option.value));
            }
        }
        if (!newObj["node"][propertyName] || newObj["node"][propertyName].length === 0) {
            newObj["node"][propertyName] = null;
        }
        else if (newObj["node"][propertyName].length == 1 && propertyName != "pages" && propertyName != "entity_list_pages" && propertyName != "entity_create_pages"
            && propertyName != "entity_details_pages" && propertyName != "entity_manage_pages") {
            newObj["node"][propertyName] = newObj["node"][propertyName][0];
        }
        this.modalNodeObj = newObj;
    }
    render() {
        let modalTitle = "Manage node";
        if (!this.nodeObj["node"]) {
            modalTitle = "Create node";
        }
        if (this.nodeAuxData == null) {
            return (h("div", { class: "modal d-block" },
                h("div", { class: "modal-dialog modal-xl" },
                    h("div", { class: "modal-content" },
                        h("div", { class: "modal-header" },
                            h("h5", { class: "modal-title" }, modalTitle)),
                        h("div", { class: "modal-body", style: { minHeight: "300px" } },
                            h("i", { class: "fas fa-spinner fa-spin go-blue" }),
                            " Loading data ...")))));
        }
        if (!this.modalNodeObj["node"]["type"]) {
            this.modalNodeObj["node"]["type"] = String(this.nodeAuxData["nodeTypes"][0]["value"]);
        }
        if (!this.modalNodeObj["node"]["entity_id"]) {
            this.modalNodeObj["node"]["entity_id"] = String(this.nodeAuxData["allEntities"][0]["value"]);
        }
        let appPagesPlusNode = [];
        let addedPages = [];
        let entityListPages = [];
        let entityCreatePages = [];
        let entityDetailsPages = [];
        let entityManagePages = [];
        this.modalNodeObj["node_pages"].forEach(element => {
            appPagesPlusNode.push(element);
            addedPages.push(element["value"]);
        });
        this.nodeAuxData["appPages"].forEach(element => {
            if (addedPages.length == 0 || (addedPages.length > 0 && addedPages.indexOf(element["page_id"]) === -1)) {
                if (!element["node_id"] || element["node_id"] === this.modalNodeObj["node"]["id"]) {
                    var selectOption = {
                        value: element["page_id"],
                        label: element["page_name"]
                    };
                    appPagesPlusNode.push(selectOption);
                }
            }
        });
        this.nodeAuxData["allEntityPages"].forEach(element => {
            if (String(this.modalNodeObj["node"]["type"]) === "1" && this.modalNodeObj["node"]["entity_id"]) {
                if (element["entity_id"] === this.modalNodeObj["node"]["entity_id"]) {
                    switch (element["type"]) {
                        case "3":
                            entityListPages.push(element);
                            break;
                        case "4":
                            entityCreatePages.push(element);
                            break;
                        case "5":
                            entityDetailsPages.push(element);
                            break;
                        case "6":
                            entityManagePages.push(element);
                            break;
                        default:
                            break;
                    }
                }
            }
        });
        return (h("div", { class: "modal d-block" },
            h("div", { class: "modal-dialog modal-xl" },
                h("div", { class: "modal-content" },
                    h("form", { onSubmit: (e) => this.handleSubmit(e) },
                        h("div", { class: "modal-header" },
                            h("h5", { class: "modal-title" }, modalTitle)),
                        h("div", { class: "modal-body" },
                            h("div", { class: "alert alert-danger " + (this.submitResponse["success"] ? "d-none" : "") }, this.submitResponse["message"]),
                            h("div", { class: "row" },
                                h("div", { class: "col col-sm-6" },
                                    h("div", { class: "form-group erp-field" },
                                        h("label", { class: "control-label" }, "Name"),
                                        h("input", { class: "form-control", name: "name", value: this.modalNodeObj["node"]["name"], onInput: (event) => this.handleChange(event) }))),
                                h("div", { class: "col col-sm-6" },
                                    h("div", { class: "form-group erp-field" },
                                        h("label", { class: "control-label" }, "Label"),
                                        h("input", { class: "form-control", name: "label", value: this.modalNodeObj["node"]["label"], onInput: (event) => this.handleChange(event) })))),
                            h("div", { class: "row" },
                                h("div", { class: "col col-sm-6" },
                                    h("div", { class: "form-group erp-field" },
                                        h("label", { class: "control-label" }, "Icon Class"),
                                        h("input", { class: "form-control", name: "icon_class", value: this.modalNodeObj["node"]["icon_class"], onInput: (event) => this.handleChange(event) }))),
                                h("div", { class: "col col-sm-6" },
                                    h("div", { class: "form-group erp-field" },
                                        h("label", { class: "control-label" }, "Weight"),
                                        h("input", { type: "number", step: 1, min: 1, class: "form-control", name: "weight", value: this.modalNodeObj["node"]["weight"], onInput: (event) => this.handleChange(event) })))),
                            h("div", { class: "row" },
                                h("div", { class: "col col-sm-6" },
                                    h("div", { class: "form-group erp-field" },
                                        h("label", { class: "control-label" }, "Type"),
                                        h("select", { class: "form-control", name: "type", onChange: (event) => this.handleSelectChange(event) }, this.nodeAuxData["nodeTypes"].map(function (type) {
                                            return (h("option", { value: type["value"], selected: type.value === String(this.modalNodeObj["node"]["type"]) }, type["label"]));
                                        }.bind(this))))),
                                String(this.modalNodeObj["node"]["type"]) === "1"
                                    ? (h("div", { class: "col col-sm-6" },
                                        h("div", { class: "form-group erp-field" },
                                            h("label", { class: "control-label" }, "Entity"),
                                            h("select", { class: "form-control", name: "entity_id", onChange: (event) => this.handleSelectChange(event) }, this.nodeAuxData["allEntities"].map(function (type) {
                                                return (h("option", { value: type["value"], selected: type.value === String(this.modalNodeObj["node"]["entity_id"]) }, type["label"]));
                                            }.bind(this))))))
                                    : null,
                                String(this.modalNodeObj["node"]["type"]) === "2"
                                    ? (h("div", { class: "col col-sm-6" },
                                        h("div", { class: "form-group erp-field" },
                                            h("label", { class: "control-label" }, "App Pages without nodes"),
                                            h("select", { class: "form-control", multiple: true, name: "pages", onChange: (event) => this.handleSelectChange(event) }, appPagesPlusNode.map(function (type) {
                                                let nodeSelected = false;
                                                if (this.modalNodeObj["node"]["pages"] && this.modalNodeObj["node"]["pages"].length > 0 && this.modalNodeObj["node"]["pages"].indexOf(type.value) > -1) {
                                                    nodeSelected = true;
                                                }
                                                return (h("option", { value: type["value"], selected: nodeSelected }, type["label"]));
                                            }.bind(this))))))
                                    : null,
                                String(this.modalNodeObj["node"]["type"]) === "3"
                                    ? (h("div", { class: "col col-sm-6" },
                                        h("div", { class: "form-group erp-field" },
                                            h("label", { class: "control-label" }, "Url"),
                                            h("input", { class: "form-control", name: "url", value: this.modalNodeObj["node"]["url"], onInput: (event) => this.handleChange(event) }))))
                                    : null),
                            String(this.modalNodeObj["node"]["type"]) === "1" && this.modalNodeObj["node"]["entity_id"]
                                ? (h("div", null,
                                    h("div", { class: "row" },
                                        h("div", { class: "col-3" },
                                            h("div", { class: "form-group erp-field" },
                                                h("label", { class: "control-label" }, "list pages"),
                                                h("select", { class: "form-control", multiple: true, name: "entity_list_pages", onChange: (event) => this.handleSelectChange(event) }, entityListPages.map(function (type) {
                                                    let nodeSelected = false;
                                                    if (this.modalNodeObj["node"]["entity_list_pages"] && this.modalNodeObj["node"]["entity_list_pages"].length > 0 && this.modalNodeObj["node"]["entity_list_pages"].indexOf(type.page_id) > -1) {
                                                        nodeSelected = true;
                                                    }
                                                    return (h("option", { value: type["page_id"], selected: nodeSelected }, type["page_name"]));
                                                }.bind(this))))),
                                        h("div", { class: "col-3" },
                                            h("div", { class: "form-group erp-field" },
                                                h("label", { class: "control-label" }, "create pages"),
                                                h("select", { class: "form-control", multiple: true, name: "entity_create_pages", onChange: (event) => this.handleSelectChange(event) }, entityCreatePages.map(function (type) {
                                                    let nodeSelected = false;
                                                    if (this.modalNodeObj["node"]["entity_create_pages"] && this.modalNodeObj["node"]["entity_create_pages"].length > 0 && this.modalNodeObj["node"]["entity_create_pages"].indexOf(type.page_id) > -1) {
                                                        nodeSelected = true;
                                                    }
                                                    return (h("option", { value: type["page_id"], selected: nodeSelected }, type["page_name"]));
                                                }.bind(this))))),
                                        h("div", { class: "col-3" },
                                            h("div", { class: "form-group erp-field" },
                                                h("label", { class: "control-label" }, "details pages"),
                                                h("select", { class: "form-control", multiple: true, name: "entity_details_pages", onChange: (event) => this.handleSelectChange(event) }, entityDetailsPages.map(function (type) {
                                                    let nodeSelected = false;
                                                    if (this.modalNodeObj["node"]["entity_details_pages"] && this.modalNodeObj["node"]["entity_details_pages"].length > 0 && this.modalNodeObj["node"]["entity_details_pages"].indexOf(type.page_id) > -1) {
                                                        nodeSelected = true;
                                                    }
                                                    return (h("option", { value: type["page_id"], selected: nodeSelected }, type["page_name"]));
                                                }.bind(this))))),
                                        h("div", { class: "col-3" },
                                            h("div", { class: "form-group erp-field" },
                                                h("label", { class: "control-label" }, "manage pages"),
                                                h("select", { class: "form-control", multiple: true, name: "entity_manage_pages", onChange: (event) => this.handleSelectChange(event) }, entityManagePages.map(function (type) {
                                                    let nodeSelected = false;
                                                    if (this.modalNodeObj["node"]["entity_manage_pages"] && this.modalNodeObj["node"]["entity_manage_pages"].length > 0 && this.modalNodeObj["node"]["entity_manage_pages"].indexOf(type.page_id) > -1) {
                                                        nodeSelected = true;
                                                    }
                                                    return (h("option", { value: type["page_id"], selected: nodeSelected }, type["page_name"]));
                                                }.bind(this)))))),
                                    h("div", { class: "go-gray" },
                                        h("i", { class: "fa fa-info-circle go-blue" }),
                                        " If no page is selected in certain type, all will be used")))
                                : null,
                            h("div", { class: "alert alert-info d-none" }, "Label and Description translations, and access are currently not managable")),
                        this.nodeAuxData == null
                            ? (null)
                            : (h("div", { class: "modal-footer" },
                                h("div", null,
                                    h("button", { type: "submit", class: "btn btn-green btn-sm " + (this.modalNodeObj["node"] == null ? "" : "d-none") },
                                        h("span", { class: "fa fa-plus" }),
                                        " Create node"),
                                    h("button", { type: "submit", class: "btn btn-blue btn-sm " + (this.modalNodeObj["node"] != null ? "" : "d-none") },
                                        h("span", { class: "far fa-disk-alt" }),
                                        " Save node"),
                                    h("button", { type: "button", class: "btn btn-white btn-sm ml-1", onClick: () => this.closeModal() }, "Close")))))))));
    }
    static get is() { return "wv-sitemap-node-modal"; }
    static get properties() { return {
        "apiRoot": {
            "type": String,
            "attr": "api-root"
        },
        "appId": {
            "type": String,
            "attr": "app-id"
        },
        "modalNodeObj": {
            "state": true
        },
        "nodeAuxData": {
            "type": "Any",
            "attr": "node-aux-data"
        },
        "nodeObj": {
            "type": "Any",
            "attr": "node-obj"
        },
        "nodePageDict": {
            "type": "Any",
            "attr": "node-page-dict"
        },
        "submitResponse": {
            "type": "Any",
            "attr": "submit-response"
        }
    }; }
    static get events() { return [{
            "name": "wvSitemapManagerNodeModalCloseEvent",
            "method": "wvSitemapManagerNodeModalCloseEvent",
            "bubbles": true,
            "cancelable": true,
            "composed": true
        }, {
            "name": "wvSitemapManagerNodeSubmittedEvent",
            "method": "wvSitemapManagerNodeSubmittedEvent",
            "bubbles": true,
            "cancelable": true,
            "composed": true
        }, {
            "name": "wvSitemapManagerNodeAuxDataUpdateEvent",
            "method": "wvSitemapManagerNodeAuxDataUpdateEvent",
            "bubbles": true,
            "cancelable": true,
            "composed": true
        }]; }
}

export { WvSitemapAreaModal, WvSitemapManager, WvSitemapManagerArea, WvSitemapNodeModal };
