import { h } from '../wv-pb-manager.core.js';

import { a as setNodeCreation, b as addNode, c as addReloadNodeIds, d as _, e as setHelpModalState, f as setOptionsModalState, g as updateNodeOptions, h as removeNode, i as SET_DRAKE, j as ADD_DRAKE_CONTAINER_ID, k as SET_ACTIVE_NODE, l as HOVER_NODE, m as HOVER_CONTAINER, n as SET_NODE_CREATION, o as ADD_NODE, p as REMOVE_NODE, q as UPDATE_NODE_OPTIONS, r as UPDATE_PAGE_NODES, s as SET_OPTIONS_MODAL_STATE, t as SET_HELP_MODAL_STATE, u as ADD_RELOAD_NODE_IDS, v as REMOVE_RELOAD_NODE_IDS, w as commonjsGlobal, x as setDrake, y as updatePageNodes, z as removeReloadNodeIds, A as addDrakeContainerId, B as hoverContainer, C as hoverNode, D as setActiveNode } from './chunk-3437f53a.js';

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

class WvPbEventPayload {
    constructor() {
        this.node = null;
        this.component_name = null;
        this.original_event = null;
    }
}

function AddNewComponent(scope, component) {
    let requestBody = scope.store.getState().createdNode;
    requestBody["component_name"] = component["name"];
    requestBody["options"] = JSON.stringify(requestBody["options"]);
    let requestConfig = {
        headers: {
            'Content-Type': 'application/json;charset=UTF-8',
            "Access-Control-Allow-Origin": "*",
        }
    };
    let siteRoot = scope.store.getState().siteRootUrl;
    let requestUrl = siteRoot + "/api/v3.0/page/" + requestBody["page_id"] + "/node/create";
    let recordId = scope.store.getState().recordId;
    if (recordId) {
        requestUrl += "?recId=" + recordId;
    }
    axios$1.post(requestUrl, requestBody, requestConfig)
        .then(function (response) {
        scope.addNode(response.data);
        window.setTimeout(function () {
            scope.addReloadNodeIds(response.data["id"]);
            var customEvent = new Event("WvPbManager_Node_Added");
            var payload = new WvPbEventPayload();
            payload.original_event = event;
            payload.node = requestBody;
            payload.component_name = requestBody["component_name"];
            customEvent["payload"] = payload;
            document.dispatchEvent(customEvent);
        }, 10);
    })
        .catch(function (error) {
        if (error.response) {
            if (error.response.data) {
                alert(error.response.data);
            }
            else {
                alert(error.response.statusText);
            }
        }
        else if (error.message) {
            alert(error.message);
        }
        else {
            alert(error);
        }
    });
}
function RecalculateComponentList(scope) {
    let library = scope.store.getState().library;
    let filteredLibrary = [];
    if (scope.filterString) {
        filteredLibrary = _.filter(library, function (x) {
            let state = x["label"].toLowerCase().includes(scope.filterString.toLowerCase());
            if (!state) {
                state = x["library"].toLowerCase().includes(scope.filterString.toLowerCase());
            }
            if (!state) {
                state = x["description"].toLowerCase().includes(scope.filterString.toLowerCase());
            }
            return state;
        });
    }
    else {
        filteredLibrary = library;
    }
    if (!filteredLibrary) {
        filteredLibrary = [];
    }
    switch (scope.sort) {
        case "usage":
            filteredLibrary = _.orderBy(filteredLibrary, ['usage_counter'], ['desc']);
            break;
        case "alpha":
            filteredLibrary = _.orderBy(filteredLibrary, ['label'], ['asc']);
            break;
        default:
            filteredLibrary = _.orderBy(filteredLibrary, ['last_used_on'], ['desc']);
            break;
    }
    let startIndex = (scope.page - 1) * scope.pageSize;
    let endIndex = startIndex + scope.pageSize;
    let filterTotal = filteredLibrary.length;
    if (endIndex > filterTotal) {
        endIndex = filterTotal;
    }
    scope.total = filterTotal;
    scope.pageCount = scope.total / scope.pageSize;
    scope.componentList = _.slice(filteredLibrary, startIndex, endIndex);
}
class WvCreateModal {
    constructor() {
        this.isCreateModalVisible = false;
        this.page = 1;
        this.pageSize = 24;
        this.sort = "recent";
        this.componentList = new Array();
        this.total = 0;
        this.pageCount = 0;
        this.focused = false;
    }
    componentWillLoad() {
        let scope = this;
        scope.store.mapStateToProps(scope, (state) => {
            return {
                isCreateModalVisible: state.isCreateModalVisible,
            };
        });
        scope.store.mapDispatchToProps(scope, {
            setNodeCreation: setNodeCreation,
            addNode: addNode,
            addReloadNodeIds: addReloadNodeIds
        });
        document.body.className = document.body.className.replace(" modal-open", "").replace("modal-open", "");
        document.body.style.paddingRight = null;
        let backdrop = document.getElementById("wv-pb-backdrop");
        if (backdrop) {
            backdrop.parentNode.removeChild(backdrop);
        }
        RecalculateComponentList(scope);
    }
    cancelNodeCreateHandler(event) {
        event.preventDefault();
        event.stopPropagation();
        this.filterString = "";
        RecalculateComponentList(this);
        this.setNodeCreation(null);
    }
    filterChangeHandler(event) {
        event.preventDefault();
        event.stopPropagation();
        this.filterString = event.target.value;
        this.page = 1;
        RecalculateComponentList(this);
    }
    selectComponent(event, component) {
        event.preventDefault();
        event.stopPropagation();
        AddNewComponent(this, component);
        this.filterString = "";
        RecalculateComponentList(this);
    }
    changeSort(ev, sort) {
        ev.preventDefault();
        this.sort = sort;
        this.page = 1;
        RecalculateComponentList(this);
    }
    changePage(ev, page) {
        ev.preventDefault();
        if (page > 0) {
            this.page = page;
        }
        RecalculateComponentList(this);
    }
    render() {
        let scope = this;
        let showModal = scope.isCreateModalVisible;
        if (!showModal) {
            scope.focused = false;
            scope.filterString = "";
            this.page = 1;
            this.sort = "recent";
            return null;
        }
        if (!scope.focused) {
            window.setTimeout(function () {
                let inputEl = document.getElementById("wv-pb-select-component-input");
                if (inputEl) {
                    inputEl.focus();
                    scope.focused = true;
                }
            }, 10);
        }
        let rowArray = [0, 1, 2, 3];
        let pageArray = [];
        for (let i = 1; i < scope.pageCount + 1; i++) {
            pageArray.push(i);
        }
        return (h("div", { class: "modal show d-block", style: { paddingRight: "17px" } },
            h("div", { class: "modal-dialog modal-full" },
                h("div", { class: "modal-content" },
                    h("div", { class: "modal-header d-none" },
                        h("h5", { class: "modal-title" }, "Select component"),
                        h("button", { type: "button", class: "close", onClick: (e) => scope.cancelNodeCreateHandler(e) },
                            h("span", { "aria-hidden": "true" }, "\u00D7"))),
                    h("div", { class: "modal-body" },
                        h("nav", { class: "navbar navbar-expand-lg navbar-light mb-3 go-bkg-blue-gray-light" },
                            h("div", { class: "flex-grow-1" },
                                h("ul", { class: "nav nav-pills" },
                                    h("li", { class: "nav-item" },
                                        h("a", { class: "nav-link " + (scope.sort === "recent" ? "active" : ""), href: "#", onClick: (e) => scope.changeSort(e, "recent") }, "Recent")),
                                    h("li", { class: "nav-item" },
                                        h("a", { class: "nav-link " + (scope.sort === "usage" ? "active" : ""), href: "#", onClick: (e) => scope.changeSort(e, "usage") }, "Most Used")),
                                    h("li", { class: "nav-item" },
                                        h("a", { class: "nav-link " + (scope.sort === "alpha" ? "active" : ""), href: "#", onClick: (e) => scope.changeSort(e, "alpha") }, "Alphabetical")))),
                            h("div", { class: "form-inline" },
                                h("input", { class: "form-control form-control-sm", placeholder: "component name", onInput: (e) => scope.filterChangeHandler(e), id: "wv-pb-select-component-input" }))),
                        scope.componentList.map(function (record, index) {
                            record = record;
                            return (h("div", { class: "row" }, rowArray.map(function (subIndex) {
                                let compIndex = index + subIndex;
                                if (compIndex < scope.componentList.length && index % 4 === 0) {
                                    let component = scope.componentList[compIndex];
                                    let iconClass = "ti-file";
                                    if (component["icon_class"]) {
                                        iconClass = component["icon_class"];
                                    }
                                    return (h("div", { class: "col-6 col-lg-3", key: component["name"] },
                                        h("div", { class: "shadow-sm mb-4 card icon-card clickable", onClick: (e) => scope.selectComponent(e, component) },
                                            h("div", { class: "card-body p-1" },
                                                h("div", { class: "icon-card-body" },
                                                    h("i", { class: "icon " + iconClass }),
                                                    h("div", { class: "meta" },
                                                        h("div", { class: "title" }, component["label"]),
                                                        h("div", { class: "description" }, component["description"]),
                                                        h("div", { class: "library" }, component["library"])))))));
                                }
                            })));
                        }),
                        h("nav", { "aria-label": "Page navigation example" },
                            h("ul", { class: "pagination justify-content-center" },
                                h("li", { class: "page-item " + (scope.page > 1 ? "" : "disabled") },
                                    h("a", { class: "page-link", href: "#", onClick: (e) => scope.changePage(e, scope.page - 1) }, "Previous")),
                                pageArray.map(function (pageNum) {
                                    return (h("li", { class: "page-item " + (scope.page === pageNum ? "active" : "") },
                                        h("a", { class: "page-link", href: "#", onClick: (e) => scope.changePage(e, pageNum) }, pageNum)));
                                }),
                                h("li", { class: "page-item " + (scope.page >= scope.pageCount ? "disabled" : "") },
                                    h("a", { class: "page-link", href: "#", onClick: (e) => scope.changePage(e, scope.page + 1) }, "Next"))))),
                    h("div", { class: "modal-footer" },
                        h("button", { type: "button", class: "btn btn-white btn-sm", onClick: (e) => scope.cancelNodeCreateHandler(e) }, "Cancel"))))));
    }
    static get is() { return "wv-create-modal"; }
    static get properties() { return {
        "componentList": {
            "state": true
        },
        "filterString": {
            "state": true
        },
        "isCreateModalVisible": {
            "state": true
        },
        "page": {
            "state": true
        },
        "pageCount": {
            "state": true
        },
        "pageSize": {
            "state": true
        },
        "sort": {
            "state": true
        },
        "store": {
            "context": "store"
        },
        "total": {
            "state": true
        }
    }; }
}

class WvCreateModal$1 {
    constructor() {
        this.isHelpModalVisible = false;
    }
    componentWillLoad() {
        this.store.mapStateToProps(this, (state) => {
            return {
                isHelpModalVisible: state.isHelpModalVisible,
            };
        });
        this.store.mapDispatchToProps(this, {
            setHelpModalState: setHelpModalState
        });
    }
    cancelHelpModalHandler(event) {
        event.preventDefault();
        event.stopPropagation();
        this.setHelpModalState(false);
    }
    render() {
        let scope = this;
        let showModal = scope.isHelpModalVisible;
        if (!showModal) {
            return null;
        }
        return (h("div", { class: "modal show d-block", style: { paddingRight: "17px" }, id: "modal-component-help" },
            h("div", { class: "modal-dialog modal-xl" },
                h("div", { class: "modal-content" },
                    h("div", { class: "modal-header" },
                        h("span", { class: "title" }, "Component help"),
                        h("span", { class: "aside" },
                            "wv-",
                            scope.store.getState().activeNodeId)),
                    h("div", { class: "modal-body", id: "modal-component-help-body" },
                        h("wv-show-help", { nodeId: scope.store.getState().activeNodeId })),
                    h("div", { class: "modal-footer" },
                        h("button", { type: "button", class: "btn btn-white btn-sm", onClick: (e) => scope.cancelHelpModalHandler(e) }, "Close"))))));
    }
    static get is() { return "wv-help-modal"; }
    static get properties() { return {
        "isHelpModalVisible": {
            "state": true
        },
        "store": {
            "context": "store"
        }
    }; }
}

class WvLoadingPane {
    render() {
        return (h("div", { class: "p-2" },
            h("i", { class: "fa fa-spin fa-spinner go-blue" }),
            " Loading..."));
    }
    static get is() { return "wv-loading-pane"; }
}

function seq(arr, callback, index) {
    if (typeof index === 'undefined') {
        index = 0;
    }
    if (!arr[index]) {
        return;
    }
    arr[index](function () {
        index++;
        if (index === arr.length) {
            callback();
        }
        else {
            seq(arr, callback, index);
        }
    });
}
function scriptsDone() {
    var DOMContentLoadedEvent = document.createEvent('Event');
    DOMContentLoadedEvent.initEvent('DOMContentLoaded', true, true);
    document.dispatchEvent(DOMContentLoadedEvent);
}
function insertScript($script, callback) {
    var s = document.createElement('script');
    s.type = 'text/javascript';
    if ($script.src) {
        s.onload = callback;
        s.onerror = callback;
        s.src = $script.src;
    }
    else {
        s.textContent = $script.innerText;
    }
    if (s.src.includes("http://localhost:3333/")) {
        s.src = s.src.replace("http://localhost:3333/", "http://localhost:2202/");
    }
    document.head.appendChild(s);
    $script.parentNode.removeChild($script);
    if (!$script.src) {
        callback();
    }
}
var runScriptTypes = [
    'application/javascript',
    'application/ecmascript',
    'application/x-ecmascript',
    'application/x-javascript',
    'text/ecmascript',
    'text/javascript',
    'text/javascript1.0',
    'text/javascript1.1',
    'text/javascript1.2',
    'text/javascript1.3',
    'text/javascript1.4',
    'text/javascript1.5',
    'text/jscript',
    'text/livescript',
    'text/x-ecmascript',
    'text/x-javascript'
];
function runScripts($container) {
    var $scripts = $container.querySelectorAll('script');
    var runList = [];
    var typeAttr;
    [].forEach.call($scripts, function ($script) {
        typeAttr = $script.getAttribute('type');
        if (!typeAttr || runScriptTypes.indexOf(typeAttr) !== -1) {
            runList.push(function (callback) {
                insertScript($script, callback);
            });
        }
    });
    seq(runList, scriptsDone, 0);
}

function LoadNodeDesignHtmlFromServerToStack(newNode, oldMeta, scope) {
    if (typeof newNode["options"] !== 'object') {
        if (!newNode["options"]) {
            newNode["options"] = {};
        }
        else {
            newNode["options"] = JSON.parse(newNode["options"]);
        }
    }
    let requestConfig = {
        headers: {
            'Content-Type': 'application/json;charset=UTF-8',
            "Access-Control-Allow-Origin": "*",
        }
    };
    let siteRootUrl = scope.store.getState().siteRootUrl;
    let requestUrl = siteRootUrl + oldMeta.meta["design_view_url"] + "&nid=" + newNode["id"] + "&pid=" + newNode["page_id"];
    let recordId = scope.store.getState().recordId;
    if (recordId) {
        requestUrl += "&recId=" + recordId;
    }
    let requestBody = newNode["options"];
    axios$1.post(requestUrl, requestBody, requestConfig)
        .then(function (response) {
        let updatedNodeHtmlEl = document.getElementById("node-design-" + newNode["id"]);
        let nodeIds = new Array();
        var childNodes = updatedNodeHtmlEl.querySelectorAll("[data-node-id]");
        _.forEach(childNodes, function (childNode) {
            nodeIds.push(childNode.getAttribute("data-node-id"));
        });
        _.forEach(nodeIds, function (nodeId) {
            NodeUtils.MoveNodeToStack(nodeId);
        });
        updatedNodeHtmlEl.remove();
        let nodeDiv = document.createElement("div");
        nodeDiv.id = "node-design-" + newNode["id"];
        nodeDiv.classList.add("wv-pb-node");
        nodeDiv.innerHTML = response.data;
        let nodeDesignStack = document.getElementById("wv-node-design-stack");
        nodeDesignStack.appendChild(nodeDiv);
        runScripts(nodeDiv);
        scope.updateNodeOptions(newNode);
        window.setTimeout(function () {
            nodeIds.push(newNode["id"]);
            scope.addReloadNodeIds(nodeIds);
            var customEvent = new Event("WvPbManager_Design_Loaded");
            var payload = new WvPbEventPayload();
            payload.node = newNode;
            payload.component_name = oldMeta.node["component_name"];
            customEvent["payload"] = payload;
            document.dispatchEvent(customEvent);
        }, 10);
    })
        .catch(function (error) {
        if (error.response) {
            if (error.response.data) {
                console.log(error.response.data);
            }
            else {
                console.log(error.response.statusText);
            }
        }
        else if (error.message) {
            console.log(error.message);
        }
        else {
            console.log(error);
        }
    });
}
class NodeUtils {
    static MoveNodeToStack(nodeId) {
        if (!nodeId) {
            console.error("node id not defined MoveNodeToStack " + nodeId);
            return false;
        }
        let nodeDesignTemplate = document.getElementById("node-design-" + nodeId);
        let nodeDesignStack = document.getElementById("wv-node-design-stack");
        if (nodeDesignTemplate && nodeDesignTemplate.parentElement.id === "wv-node-" + nodeId && nodeDesignStack) {
            nodeDesignStack.appendChild(nodeDesignTemplate);
            return true;
        }
        if (!nodeDesignStack) {
            console.error("stack is missing ");
        }
        else {
            console.error("error to-stack " + nodeId);
        }
        return false;
    }
    static GetNodeFromStack(nodeId) {
        if (!nodeId) {
            console.error("node id not defined GetNodeFromStack " + nodeId);
            return false;
        }
        let nodeDesignTemplate = document.getElementById("node-design-" + nodeId);
        let nodeContainerPlaceholder = document.getElementById("wv-node-" + nodeId);
        if (nodeDesignTemplate && nodeDesignTemplate.parentElement.id === "wv-node-design-stack" && nodeContainerPlaceholder) {
            nodeContainerPlaceholder.appendChild(nodeDesignTemplate);
            return true;
        }
        if (nodeDesignTemplate && nodeDesignTemplate.parentElement.id === "wv-node-" + nodeId) {
            return true;
        }
        if (!nodeContainerPlaceholder) {
            return true;
        }
        return false;
    }
    static GetNodeFromServerToStack(node, oldMeta, scope) {
        LoadNodeDesignHtmlFromServerToStack(node, oldMeta, scope);
    }
    static GetNodeAndMeta(scope, nodeId) {
        let returnObj = {
            node: null,
            meta: null
        };
        let pageNodes = scope.store.getState().pageNodes;
        let library = scope.store.getState().library;
        if (nodeId) {
            let activeNodeIndex = _.findIndex(pageNodes, function (x) { return x["id"] == nodeId; });
            if (activeNodeIndex == -1) {
                console.error("Node with id " + nodeId + " not found in pageNodes");
                return;
            }
            else {
                returnObj.node = Object.assign({}, pageNodes[activeNodeIndex]);
                let libObjIndex = _.findIndex(library, function (x) { return x["name"] == returnObj.node["component_name"]; });
                if (libObjIndex == -1) {
                    console.error("Component name " + returnObj.node["component_name"] + " not found in library");
                    return;
                }
                else {
                    returnObj.meta = library[libObjIndex];
                }
            }
        }
        return returnObj;
    }
    static GetActiveNodeAndMeta(scope) {
        let returnObj = {
            node: null,
            meta: null
        };
        let pageNodes = scope.store.getState().pageNodes;
        let library = scope.store.getState().library;
        let activeNodeId = scope.store.getState().activeNodeId;
        if (activeNodeId) {
            let activeNodeIndex = _.findIndex(pageNodes, function (x) { return x["id"] == activeNodeId; });
            if (activeNodeIndex == -1) {
                console.error("Node with id " + activeNodeId + " not found in pageNodes");
                return;
            }
            else {
                returnObj.node = Object.assign({}, pageNodes[activeNodeIndex]);
                let libObjIndex = _.findIndex(library, function (x) { return x["name"] == returnObj.node["component_name"]; });
                if (libObjIndex == -1) {
                    console.error("Component name " + returnObj.node["component_name"] + " not found in library");
                    return;
                }
                else {
                    returnObj.meta = library[libObjIndex];
                }
            }
        }
        return returnObj;
    }
    static GetChildNodes(parentId = null, containerId = null, allNodes = new Array(), result = new Array(), applyContainerId = true) {
        let childNodes = _.filter(allNodes, function (record) {
            return record["parent_id"] === parentId && (!applyContainerId || record["container_id"] === containerId);
        });
        _.forEach(childNodes, function (childNode) {
            result.push(childNode["id"]);
            NodeUtils.GetChildNodes(childNode["id"], "", allNodes, result, false);
        });
    }
}

function getSelectValues(select) {
    var result = [];
    var options = select && select.options;
    var opt;
    for (var i = 0, iLen = options.length; i < iLen; i++) {
        opt = options[i];
        if (opt.selected) {
            result.push(opt.value || opt.text);
        }
    }
    return result;
}
class WvCreateModal$2 {
    constructor() {
        this.isOptionsModalVisible = false;
        this.isSaveLoading = false;
    }
    componentWillLoad() {
        this.store.mapStateToProps(this, (state) => {
            return {
                isOptionsModalVisible: state.isOptionsModalVisible,
            };
        });
        this.store.mapDispatchToProps(this, {
            setOptionsModalState: setOptionsModalState,
            updateNodeOptions: updateNodeOptions,
            addReloadNodeIds: addReloadNodeIds
        });
    }
    cancelOptionsModalHandler(event) {
        event.preventDefault();
        event.stopPropagation();
        this.setOptionsModalState(false);
    }
    saveOptionsModalHandler(event) {
        let scope = this;
        scope.isSaveLoading = true;
        event.preventDefault();
        let options = {};
        let inputElements = document.querySelectorAll('#modal-component-options .modal-body input, #modal-component-options .modal-body textarea, #modal-component-options .modal-body select');
        if (inputElements && inputElements.length > 0) {
            _.forEach(inputElements, function (inputElement) {
                let inputName = inputElement.name;
                if (!inputElement.type || inputElement.type === "text" || inputElement.type === "number"
                    || inputElement.type === "email" || inputElement.type === "color" || inputElement.type === "textarea"
                    || inputElement.type === "hidden") {
                    if (!inputName) {
                        return true;
                    }
                    switch (inputElement.type) {
                        case "number":
                            var fieldValue = Number(inputElement.value);
                            if (isNaN(fieldValue)) {
                                fieldValue = null;
                            }
                            options[inputName] = fieldValue;
                            break;
                        default:
                            options[inputName] = inputElement.value;
                            break;
                    }
                }
                else if (inputElement.type === "checkbox") {
                    let isErpCheckbox = false;
                    if (inputElement.classList.contains("form-check-input")) {
                        isErpCheckbox = true;
                    }
                    if (!inputName && !isErpCheckbox) {
                        return true;
                    }
                    let value = false;
                    if (!isErpCheckbox) {
                        if (inputElement.checked) {
                            value = true;
                        }
                        options[inputName] = value;
                    }
                    else {
                        let checkboxId = inputElement.id;
                        let dummyHiddenInput = document.querySelector('#modal-component-options .modal-body input[data-source-id="' + checkboxId + '"]');
                        let customDummyElement = dummyHiddenInput;
                        value = customDummyElement.value;
                        let fieldName = inputElement.getAttribute("data-field-name");
                        options[fieldName] = value;
                    }
                }
                else if (inputElement.tagName.toLowerCase() === "select") {
                    if (inputElement.multiple) {
                        options[inputName] = getSelectValues(inputElement);
                    }
                    else {
                        options[inputName] = inputElement.value;
                    }
                }
            });
        }
        let componentObject = NodeUtils.GetActiveNodeAndMeta(scope);
        let requestConfig = {
            headers: {
                'Content-Type': 'application/json;charset=UTF-8',
                "Access-Control-Allow-Origin": "*",
            }
        };
        let siteUrl = scope.store.getState().siteRootUrl;
        let requestUrl = siteUrl + "/api/v3.0/page/" + componentObject.node["page_id"] + "/node/" + componentObject.node["id"] + "/options/update";
        let recordId = scope.store.getState().recordId;
        if (recordId) {
            requestUrl += "?recId=" + recordId;
        }
        let requestBody = options;
        axios$1.post(requestUrl, requestBody, requestConfig)
            .then(function (response) {
            var customEvent = new Event("WvPbManager_Options_Changed");
            var payload = new WvPbEventPayload();
            payload.original_event = event;
            payload.node = componentObject.node;
            payload.component_name = componentObject.node["component_name"];
            customEvent["payload"] = payload;
            document.dispatchEvent(customEvent);
            var updatedNodeIndex = _.findIndex(response.data, function (record) { return record.id === componentObject.node["id"]; });
            if (updatedNodeIndex > -1) {
                NodeUtils.GetNodeFromServerToStack(response.data[updatedNodeIndex], componentObject, scope);
            }
            else {
                alert("Error: Node not found in returned results");
            }
            scope.isSaveLoading = false;
            scope.setOptionsModalState(false);
        })
            .catch(function (error) {
            scope.isSaveLoading = false;
            if (error.response) {
                if (error.response.data) {
                    alert(error.response.data);
                }
                else {
                    alert(error.response.statusText);
                }
            }
            else if (error.message) {
                alert(error.message);
            }
            else {
                alert(error);
            }
        });
    }
    render() {
        let scope = this;
        let showModal = scope.isOptionsModalVisible;
        let activeNodeId = scope.store.getState().activeNodeId;
        if (!showModal) {
            return null;
        }
        var nodeMeta = NodeUtils.GetNodeAndMeta(scope, activeNodeId);
        var nameArray = nodeMeta.node["component_name"].split(".");
        var componentName = nameArray[nameArray.length - 1];
        return (h("div", { class: "modal show d-block", style: { paddingRight: "17px" }, id: "modal-component-options" },
            h("div", { class: "modal-dialog modal-xl" },
                h("div", { class: "modal-content" },
                    h("div", { class: "modal-header" },
                        h("span", { class: "title" },
                            h("span", { class: "go-teal" }, componentName),
                            " options"),
                        h("span", { class: "aside" },
                            "wv-",
                            activeNodeId)),
                    h("div", { class: "modal-body", id: "modal-component-options-body" },
                        h("wv-show-options", { nodeId: activeNodeId })),
                    h("div", { class: "modal-footer" },
                        h("button", { type: "button", class: "btn btn-primary btn-sm", onClick: (e) => scope.saveOptionsModalHandler(e), disabled: scope.isSaveLoading },
                            h("i", { class: scope.isSaveLoading ? "fa fa-spin fa-spinner" : "fa fa-save" }),
                            " Save Options"),
                        h("button", { type: "button", class: "btn btn-white btn-sm", onClick: (e) => scope.cancelOptionsModalHandler(e) }, "Close"))))));
    }
    static get is() { return "wv-options-modal"; }
    static get properties() { return {
        "isOptionsModalVisible": {
            "state": true
        },
        "isSaveLoading": {
            "state": true
        },
        "store": {
            "context": "store"
        }
    }; }
}

function RenderComponentCard(props) {
    let scope = props.scope;
    if (!scope.activeNodeId) {
        return (h("div", null, "Select a component to review its options"));
    }
    let pageNodes = scope.store.getState().pageNodes;
    let activeNodeIndex = _.findIndex(pageNodes, function (x) { return x["id"] === scope.activeNodeId; });
    if (activeNodeIndex === -1) {
        return null;
    }
    let activeNode = pageNodes[activeNodeIndex];
    let library = scope.store.getState().library;
    let metaIndex = _.findIndex(library, function (x) { return x["name"] === activeNode["component_name"]; });
    if (metaIndex === -1) {
        return null;
    }
    let meta = library[metaIndex];
    let style = {};
    if (meta["color"]) {
        style = {
            color: meta["color"]
        };
    }
    return ([
        h("div", { class: "icon-card-body" },
            h("span", { class: "icon " + meta["icon_class"], style: style }),
            h("div", { class: "meta" },
                h("div", { class: "title" }, meta["label"]),
                h("div", { class: "description" }, meta["description"]),
                h("div", { class: "library" }, meta["library"]))),
        h("hr", { class: "divider", style: { margin: "5px 0" } }),
        h("div", { class: "row no-gutters" },
            h("div", { class: "col-6 pr-1" },
                h("button", { id: "wv-pb-inspector-options-btn", type: "button", class: "btn btn-white btn-sm btn-block", onClick: (e) => scope.showOptionsModalHandler(e) },
                    h("i", { class: scope.isOptionsLoading ? "fa fa-spin fa-spinner" : "fa fa-cog" }),
                    " options")),
            h("div", { class: "col-6 pl-1" },
                h("button", { type: "button", class: "btn btn-white btn-sm btn-block", onClick: (e) => scope.showHelpModalHandler(e) },
                    h("i", { class: scope.isHelpLoading ? "fa fa-spin fa-spinner" : "far fa-question-circle" }),
                    " help")))
    ]);
}
function RenderAction(props) {
    let scope = props.scope;
    if (scope.activeNodeId) {
        return (h("a", { class: "go-red", href: "#", onClick: (e) => { if (window.confirm('Are you sure you wish to delete this component?'))
                scope.deleteNodeHandler(e); } },
            h("i", { class: "ti-trash" }),
            " delete node"));
    }
}
function RemoveNode(scope) {
    let requestConfig = {
        headers: {
            'Content-Type': 'application/json;charset=UTF-8',
            "Access-Control-Allow-Origin": "*",
        }
    };
    let componentObject = NodeUtils.GetActiveNodeAndMeta(scope);
    let siteUrl = scope.store.getState().siteRootUrl;
    let requestUrl = siteUrl + "/api/v3.0/page/" + componentObject.node["page_id"] + "/node/" + componentObject.node["id"] + "/delete";
    let recordId = scope.store.getState().recordId;
    if (recordId) {
        requestUrl += "?recId=" + recordId;
    }
    axios$1.post(requestUrl, null, requestConfig)
        .then(function () {
        var customEvent = new Event("WvPbManager_Node_Removed");
        var payload = new WvPbEventPayload();
        payload.original_event = event;
        payload.node = componentObject.node;
        payload.component_name = componentObject.node["component_name"];
        customEvent["payload"] = payload;
        document.dispatchEvent(customEvent);
        scope.removeNode(scope.activeNodeId);
    })
        .catch(function (error) {
        if (error.response) {
            if (error.response.data) {
                alert(error.response.data);
            }
            else {
                alert(error.response.statusText);
            }
        }
        else if (error.message) {
            alert(error.message);
        }
        else {
            alert(error);
        }
    });
}
function LoadHelpTemplate(scope) {
    scope.isHelpLoading = true;
    let responseObject = NodeUtils.GetNodeAndMeta(scope, scope.activeNodeId);
    let errorMessage = null;
    if (responseObject) {
        let requestConfig = {
            headers: {
                'Content-Type': 'application/json;charset=UTF-8',
                "Access-Control-Allow-Origin": "*",
            }
        };
        let siteRootUrl = scope.store.getState().siteRootUrl;
        let requestUrl = siteRootUrl + responseObject.meta["help_view_url"] + "&nid=" + responseObject.node["id"] + "&pid=" + responseObject.node["page_id"];
        let recordId = scope.store.getState().recordId;
        if (recordId) {
            requestUrl += "&recId=" + recordId;
        }
        let requestBody = responseObject.node["options"];
        axios$1.post(requestUrl, requestBody, requestConfig)
            .then(function (response) {
            let nodeHelpStack = document.getElementById("wv-node-help-stack");
            let nodeDiv = document.createElement("div");
            nodeDiv.id = "node-help-" + scope.activeNodeId;
            nodeDiv.innerHTML = response.data;
            nodeHelpStack.appendChild(nodeDiv);
            runScripts(nodeDiv);
            var customEvent = new Event("WvPbManager_Help_Loaded");
            var payload = new WvPbEventPayload();
            payload.node = responseObject.node;
            payload.component_name = responseObject.node["component_name"];
            customEvent["payload"] = payload;
            document.dispatchEvent(customEvent);
            scope.setHelpModalState(true);
            scope.isHelpLoading = false;
        })
            .catch(function (error) {
            if (error.response) {
                if (error.response.data) {
                    errorMessage = error.response.data;
                }
                else {
                    errorMessage = error.response.statusText;
                }
            }
            else if (error.message) {
                errorMessage = error.message;
            }
            else {
                errorMessage = error;
            }
            if (errorMessage) {
                let nodeContainerPlaceholder = document.getElementById("wv-node-" + scope.activeNodeId);
                let errorDiv = document.createElement("div");
                errorDiv.classList.add("alert");
                errorDiv.classList.add("alert-danger");
                errorDiv.classList.add("m-1");
                errorDiv.classList.add("p-1");
                errorDiv.innerHTML = errorMessage;
                nodeContainerPlaceholder.appendChild(errorDiv);
                scope.isOptionsLoading = false;
            }
        });
    }
}
function LoadOptionsTemplate(scope) {
    scope.isOptionsLoading = true;
    let responseObject = NodeUtils.GetNodeAndMeta(scope, scope.activeNodeId);
    let errorMessage = null;
    if (responseObject) {
        let requestConfig = {
            headers: {
                'Content-Type': 'application/json;charset=UTF-8',
                "Access-Control-Allow-Origin": "*",
            }
        };
        let siteRootUrl = scope.store.getState().siteRootUrl;
        let requestUrl = siteRootUrl + responseObject.meta["options_view_url"] + "&nid=" + responseObject.node["id"] + "&pid=" + responseObject.node["page_id"];
        let recordId = scope.store.getState().recordId;
        if (recordId) {
            requestUrl += "&recId=" + recordId;
        }
        let requestBody = responseObject.node["options"];
        axios$1.post(requestUrl, requestBody, requestConfig)
            .then(function (response) {
            let nodeOptionsStack = document.getElementById("wv-node-options-stack");
            let nodeDiv = document.createElement("div");
            nodeDiv.id = "node-options-" + scope.activeNodeId;
            nodeDiv.innerHTML = response.data;
            nodeOptionsStack.appendChild(nodeDiv);
            runScripts(nodeDiv);
            var customEvent = new Event("WvPbManager_Options_Loaded");
            var payload = new WvPbEventPayload();
            payload.node = responseObject.node;
            payload.component_name = responseObject.node["component_name"];
            customEvent["payload"] = payload;
            document.dispatchEvent(customEvent);
            scope.setOptionsModalState(true);
            scope.isOptionsLoading = false;
        })
            .catch(function (error) {
            if (error.response) {
                if (error.response.data) {
                    errorMessage = error.response.data;
                }
                else {
                    errorMessage = error.response.statusText;
                }
            }
            else if (error.message) {
                errorMessage = error.message;
            }
            else {
                errorMessage = error;
            }
            if (errorMessage) {
                let nodeContainerPlaceholder = document.getElementById("wv-node-" + scope.activeNodeId);
                let errorDiv = document.createElement("div");
                errorDiv.classList.add("alert");
                errorDiv.classList.add("alert-danger");
                errorDiv.classList.add("m-1");
                errorDiv.classList.add("p-1");
                errorDiv.innerHTML = errorMessage;
                nodeContainerPlaceholder.appendChild(errorDiv);
                scope.isOptionsLoading = false;
            }
        });
    }
}
class WvPbInspector {
    constructor() {
        this.isHelpLoading = false;
        this.isOptionsLoading = false;
    }
    componentWillLoad() {
        let scope = this;
        scope.store.mapStateToProps(this, (state) => {
            return {
                activeNodeId: state.activeNodeId
            };
        });
        scope.store.mapDispatchToProps(this, {
            removeNode: removeNode,
            setOptionsModalState: setOptionsModalState,
            setHelpModalState: setHelpModalState
        });
    }
    deleteNodeHandler(event) {
        event.preventDefault();
        event.stopPropagation();
        RemoveNode(this);
    }
    showOptionsModalHandler(event) {
        event.preventDefault();
        event.stopPropagation();
        let nodeOptionsTemplate = document.getElementById("node-options-" + this.activeNodeId);
        if (nodeOptionsTemplate) {
            this.setOptionsModalState(true);
        }
        else {
            LoadOptionsTemplate(this);
        }
    }
    showHelpModalHandler(event) {
        event.preventDefault();
        event.stopPropagation();
        let nodeHelpTemplate = document.getElementById("node-help-" + this.activeNodeId);
        if (nodeHelpTemplate) {
            this.setHelpModalState(true);
        }
        else {
            LoadHelpTemplate(this);
        }
    }
    render() {
        let scope = this;
        return ([
            h("div", { class: "header" },
                h("div", { class: "title" }, "Inspector"),
                h("div", { class: "action pr-1" },
                    h(RenderAction, { scope: scope }))),
            h("div", { class: "body" },
                h(RenderComponentCard, { scope: scope }))
        ]);
    }
    static get is() { return "wv-pb-inspector"; }
    static get properties() { return {
        "activeNodeId": {
            "state": true
        },
        "isHelpLoading": {
            "state": true
        },
        "isOptionsLoading": {
            "state": true
        },
        "store": {
            "context": "store"
        }
    }; }
}

function symbolObservablePonyfill(root) {
	var result;
	var Symbol = root.Symbol;

	if (typeof Symbol === 'function') {
		if (Symbol.observable) {
			result = Symbol.observable;
		} else {
			result = Symbol('observable');
			Symbol.observable = result;
		}
	} else {
		result = '@@observable';
	}

	return result;
}

var root;

if (typeof self !== 'undefined') {
  root = self;
} else if (typeof window !== 'undefined') {
  root = window;
} else if (typeof global$1 !== 'undefined') {
  root = global$1;
} else if (typeof module !== 'undefined') {
  root = module;
} else {
  root = Function('return this')();
}

var result = symbolObservablePonyfill(root);

/**
 * These are private action types reserved by Redux.
 * For any unknown actions, you must return the current state.
 * If the current state is undefined, you must return the initial state.
 * Do not reference these action types directly in your code.
 */
var randomString = function randomString() {
  return Math.random().toString(36).substring(7).split('').join('.');
};

var ActionTypes = {
  INIT: "@@redux/INIT" + randomString(),
  REPLACE: "@@redux/REPLACE" + randomString(),
  PROBE_UNKNOWN_ACTION: function PROBE_UNKNOWN_ACTION() {
    return "@@redux/PROBE_UNKNOWN_ACTION" + randomString();
  }
};

/**
 * @param {any} obj The object to inspect.
 * @returns {boolean} True if the argument appears to be a plain object.
 */
function isPlainObject(obj) {
  if (typeof obj !== 'object' || obj === null) return false;
  var proto = obj;

  while (Object.getPrototypeOf(proto) !== null) {
    proto = Object.getPrototypeOf(proto);
  }

  return Object.getPrototypeOf(obj) === proto;
}

/**
 * Creates a Redux store that holds the state tree.
 * The only way to change the data in the store is to call `dispatch()` on it.
 *
 * There should only be a single store in your app. To specify how different
 * parts of the state tree respond to actions, you may combine several reducers
 * into a single reducer function by using `combineReducers`.
 *
 * @param {Function} reducer A function that returns the next state tree, given
 * the current state tree and the action to handle.
 *
 * @param {any} [preloadedState] The initial state. You may optionally specify it
 * to hydrate the state from the server in universal apps, or to restore a
 * previously serialized user session.
 * If you use `combineReducers` to produce the root reducer function, this must be
 * an object with the same shape as `combineReducers` keys.
 *
 * @param {Function} [enhancer] The store enhancer. You may optionally specify it
 * to enhance the store with third-party capabilities such as middleware,
 * time travel, persistence, etc. The only store enhancer that ships with Redux
 * is `applyMiddleware()`.
 *
 * @returns {Store} A Redux store that lets you read the state, dispatch actions
 * and subscribe to changes.
 */

function createStore(reducer, preloadedState, enhancer) {
  var _ref2;

  if (typeof preloadedState === 'function' && typeof enhancer === 'function' || typeof enhancer === 'function' && typeof arguments[3] === 'function') {
    throw new Error('It looks like you are passing several store enhancers to ' + 'createStore(). This is not supported. Instead, compose them ' + 'together to a single function');
  }

  if (typeof preloadedState === 'function' && typeof enhancer === 'undefined') {
    enhancer = preloadedState;
    preloadedState = undefined;
  }

  if (typeof enhancer !== 'undefined') {
    if (typeof enhancer !== 'function') {
      throw new Error('Expected the enhancer to be a function.');
    }

    return enhancer(createStore)(reducer, preloadedState);
  }

  if (typeof reducer !== 'function') {
    throw new Error('Expected the reducer to be a function.');
  }

  var currentReducer = reducer;
  var currentState = preloadedState;
  var currentListeners = [];
  var nextListeners = currentListeners;
  var isDispatching = false;

  function ensureCanMutateNextListeners() {
    if (nextListeners === currentListeners) {
      nextListeners = currentListeners.slice();
    }
  }
  /**
   * Reads the state tree managed by the store.
   *
   * @returns {any} The current state tree of your application.
   */


  function getState() {
    if (isDispatching) {
      throw new Error('You may not call store.getState() while the reducer is executing. ' + 'The reducer has already received the state as an argument. ' + 'Pass it down from the top reducer instead of reading it from the store.');
    }

    return currentState;
  }
  /**
   * Adds a change listener. It will be called any time an action is dispatched,
   * and some part of the state tree may potentially have changed. You may then
   * call `getState()` to read the current state tree inside the callback.
   *
   * You may call `dispatch()` from a change listener, with the following
   * caveats:
   *
   * 1. The subscriptions are snapshotted just before every `dispatch()` call.
   * If you subscribe or unsubscribe while the listeners are being invoked, this
   * will not have any effect on the `dispatch()` that is currently in progress.
   * However, the next `dispatch()` call, whether nested or not, will use a more
   * recent snapshot of the subscription list.
   *
   * 2. The listener should not expect to see all state changes, as the state
   * might have been updated multiple times during a nested `dispatch()` before
   * the listener is called. It is, however, guaranteed that all subscribers
   * registered before the `dispatch()` started will be called with the latest
   * state by the time it exits.
   *
   * @param {Function} listener A callback to be invoked on every dispatch.
   * @returns {Function} A function to remove this change listener.
   */


  function subscribe(listener) {
    if (typeof listener !== 'function') {
      throw new Error('Expected the listener to be a function.');
    }

    if (isDispatching) {
      throw new Error('You may not call store.subscribe() while the reducer is executing. ' + 'If you would like to be notified after the store has been updated, subscribe from a ' + 'component and invoke store.getState() in the callback to access the latest state. ' + 'See https://redux.js.org/api-reference/store#subscribe(listener) for more details.');
    }

    var isSubscribed = true;
    ensureCanMutateNextListeners();
    nextListeners.push(listener);
    return function unsubscribe() {
      if (!isSubscribed) {
        return;
      }

      if (isDispatching) {
        throw new Error('You may not unsubscribe from a store listener while the reducer is executing. ' + 'See https://redux.js.org/api-reference/store#subscribe(listener) for more details.');
      }

      isSubscribed = false;
      ensureCanMutateNextListeners();
      var index = nextListeners.indexOf(listener);
      nextListeners.splice(index, 1);
    };
  }
  /**
   * Dispatches an action. It is the only way to trigger a state change.
   *
   * The `reducer` function, used to create the store, will be called with the
   * current state tree and the given `action`. Its return value will
   * be considered the **next** state of the tree, and the change listeners
   * will be notified.
   *
   * The base implementation only supports plain object actions. If you want to
   * dispatch a Promise, an Observable, a thunk, or something else, you need to
   * wrap your store creating function into the corresponding middleware. For
   * example, see the documentation for the `redux-thunk` package. Even the
   * middleware will eventually dispatch plain object actions using this method.
   *
   * @param {Object} action A plain object representing “what changed”. It is
   * a good idea to keep actions serializable so you can record and replay user
   * sessions, or use the time travelling `redux-devtools`. An action must have
   * a `type` property which may not be `undefined`. It is a good idea to use
   * string constants for action types.
   *
   * @returns {Object} For convenience, the same action object you dispatched.
   *
   * Note that, if you use a custom middleware, it may wrap `dispatch()` to
   * return something else (for example, a Promise you can await).
   */


  function dispatch(action) {
    if (!isPlainObject(action)) {
      throw new Error('Actions must be plain objects. ' + 'Use custom middleware for async actions.');
    }

    if (typeof action.type === 'undefined') {
      throw new Error('Actions may not have an undefined "type" property. ' + 'Have you misspelled a constant?');
    }

    if (isDispatching) {
      throw new Error('Reducers may not dispatch actions.');
    }

    try {
      isDispatching = true;
      currentState = currentReducer(currentState, action);
    } finally {
      isDispatching = false;
    }

    var listeners = currentListeners = nextListeners;

    for (var i = 0; i < listeners.length; i++) {
      var listener = listeners[i];
      listener();
    }

    return action;
  }
  /**
   * Replaces the reducer currently used by the store to calculate the state.
   *
   * You might need this if your app implements code splitting and you want to
   * load some of the reducers dynamically. You might also need this if you
   * implement a hot reloading mechanism for Redux.
   *
   * @param {Function} nextReducer The reducer for the store to use instead.
   * @returns {void}
   */


  function replaceReducer(nextReducer) {
    if (typeof nextReducer !== 'function') {
      throw new Error('Expected the nextReducer to be a function.');
    }

    currentReducer = nextReducer;
    dispatch({
      type: ActionTypes.REPLACE
    });
  }
  /**
   * Interoperability point for observable/reactive libraries.
   * @returns {observable} A minimal observable of state changes.
   * For more information, see the observable proposal:
   * https://github.com/tc39/proposal-observable
   */


  function observable() {
    var _ref;

    var outerSubscribe = subscribe;
    return _ref = {
      /**
       * The minimal observable subscription method.
       * @param {Object} observer Any object that can be used as an observer.
       * The observer object should have a `next` method.
       * @returns {subscription} An object with an `unsubscribe` method that can
       * be used to unsubscribe the observable from the store, and prevent further
       * emission of values from the observable.
       */
      subscribe: function subscribe(observer) {
        if (typeof observer !== 'object' || observer === null) {
          throw new TypeError('Expected the observer to be an object.');
        }

        function observeState() {
          if (observer.next) {
            observer.next(getState());
          }
        }

        observeState();
        var unsubscribe = outerSubscribe(observeState);
        return {
          unsubscribe: unsubscribe
        };
      }
    }, _ref[result] = function () {
      return this;
    }, _ref;
  } // When a store is created, an "INIT" action is dispatched so that every
  // reducer returns their initial state. This effectively populates
  // the initial state tree.


  dispatch({
    type: ActionTypes.INIT
  });
  return _ref2 = {
    dispatch: dispatch,
    subscribe: subscribe,
    getState: getState,
    replaceReducer: replaceReducer
  }, _ref2[result] = observable, _ref2;
}

const initialState = {};
const rootReducer = (state = initialState, action) => {
    let newState = Object.assign({}, state);
    switch (action.type) {
        case SET_DRAKE:
            {
                newState["drake"] = action.payload;
            }
            return newState;
        case ADD_DRAKE_CONTAINER_ID:
            {
                let container = document.getElementById(action.payload);
                let drake = newState["drake"];
                drake.containers.push(container);
            }
            return newState;
        case SET_ACTIVE_NODE:
            {
                if (newState["activeNodeId"] && newState["activeNodeId"] === action.payload) {
                    newState["activeNodeId"] = null;
                }
                else {
                    newState["activeNodeId"] = action.payload;
                }
            }
            return newState;
        case HOVER_NODE:
            {
                newState["hoveredNodeId"] = action.payload;
                if (action.payload) {
                    newState["hoveredContainerId"] = null;
                }
            }
            return newState;
        case HOVER_CONTAINER:
            {
                newState["hoveredContainerId"] = action.payload;
                if (action.payload) {
                    newState["hoveredNodeId"] = null;
                }
            }
            return newState;
        case SET_NODE_CREATION:
            {
                if (action.payload) {
                    document.body.className += ' modal-open';
                    document.body.style.paddingRight = '17px';
                    let backdrop = document.createElement("div");
                    backdrop.className = "modal-backdrop show";
                    backdrop.id = "wv-pb-backdrop";
                    document.body.appendChild(backdrop);
                    newState["isCreateModalVisible"] = true;
                    newState["createdNode"] = action.payload;
                }
                else {
                    document.body.className = document.body.className.replace(" modal-open", "").replace("modal-open", "");
                    document.body.style.paddingRight = null;
                    var backdrop = document.getElementById("wv-pb-backdrop");
                    if (backdrop) {
                        backdrop.parentNode.removeChild(backdrop);
                    }
                    newState["isCreateModalVisible"] = false;
                    newState["createdNode"] = null;
                }
            }
            return newState;
        case ADD_NODE:
            {
                newState["activeNodeId"] = action.payload["id"];
                let node = action.payload;
                if (typeof node["options"] !== 'object') {
                    if (!node["options"]) {
                        node["options"] = {};
                    }
                    else {
                        node["options"] = JSON.parse(node["options"]);
                    }
                }
                newState["pageNodes"] = [...newState["pageNodes"], node];
                newState["pageNodeChangeIndex"]++;
                document.body.className = document.body.className.replace(" modal-open", "").replace("modal-open", "");
                document.body.style.paddingRight = null;
                var backdrop = document.getElementById("wv-pb-backdrop");
                if (backdrop) {
                    backdrop.parentNode.removeChild(backdrop);
                }
                newState["isCreateModalVisible"] = false;
                newState["createdNode"] = null;
            }
            return newState;
        case REMOVE_NODE:
            {
                newState["pageNodes"] = _.filter(newState["pageNodes"], function (record) {
                    return record["id"].toLowerCase() !== action.payload.toLowerCase();
                });
                newState["activeNodeId"] = null;
                newState["pageNodeChangeIndex"]++;
            }
            return newState;
        case UPDATE_NODE_OPTIONS:
            {
                let newNode = action.payload;
                if (newNode) {
                    if (typeof newNode["options"] !== 'object') {
                        if (!newNode["options"]) {
                            newNode["options"] = {};
                        }
                        else {
                            newNode["options"] = JSON.parse(newNode["options"]);
                        }
                    }
                    let nodeIndex = _.findIndex(newState["pageNodes"], function (record) { return record.id === newNode.id; });
                    if (nodeIndex > -1) {
                        let modifiedNode = newState["pageNodes"][nodeIndex];
                        modifiedNode["options"] = newNode["options"];
                        newState["pageNodes"] = _.filter(newState["pageNodes"], function (record) { return record.id !== newNode.id; });
                        newState["pageNodes"].push(modifiedNode);
                        newState["pageNodeChangeIndex"]++;
                    }
                }
            }
            return newState;
        case UPDATE_PAGE_NODES:
            {
                _.forEach(action.payload, function (newNode) {
                    if (typeof newNode["options"] !== 'object') {
                        if (!newNode["options"]) {
                            newNode["options"] = {};
                        }
                        else {
                            newNode["options"] = JSON.parse(newNode["options"]);
                        }
                    }
                    newState["pageNodes"] = action.payload;
                    newState["pageNodeChangeIndex"]++;
                });
            }
            return newState;
        case SET_OPTIONS_MODAL_STATE:
            {
                if (action.payload) {
                    document.body.className += ' modal-open';
                    document.body.style.paddingRight = '17px';
                    let backdrop = document.createElement("div");
                    backdrop.className = "modal-backdrop show";
                    backdrop.id = "wv-pb-backdrop";
                    document.body.appendChild(backdrop);
                    newState["isOptionsModalVisible"] = true;
                }
                else {
                    document.body.className = document.body.className.replace(" modal-open", "").replace("modal-open", "");
                    document.body.style.paddingRight = null;
                    var backdrop = document.getElementById("wv-pb-backdrop");
                    if (backdrop) {
                        backdrop.parentNode.removeChild(backdrop);
                    }
                    newState["isOptionsModalVisible"] = false;
                }
            }
            return newState;
        case SET_HELP_MODAL_STATE:
            {
                if (action.payload) {
                    document.body.className += ' modal-open';
                    document.body.style.paddingRight = '17px';
                    let backdrop = document.createElement("div");
                    backdrop.className = "modal-backdrop show";
                    backdrop.id = "wv-pb-backdrop";
                    document.body.appendChild(backdrop);
                    newState["isHelpModalVisible"] = true;
                }
                else {
                    document.body.className = document.body.className.replace(" modal-open", "").replace("modal-open", "");
                    document.body.style.paddingRight = null;
                    var backdrop = document.getElementById("wv-pb-backdrop");
                    if (backdrop) {
                        backdrop.parentNode.removeChild(backdrop);
                    }
                    newState["isHelpModalVisible"] = false;
                }
            }
            return newState;
        case ADD_RELOAD_NODE_IDS:
            {
                if (action.payload) {
                    newState["reloadNodeIdList"] = _.concat(newState["reloadNodeIdList"], action.payload);
                }
            }
            return newState;
        case REMOVE_RELOAD_NODE_IDS:
            {
                if (action.payload) {
                    newState["reloadNodeIdList"] = _.without(newState["reloadNodeIdList"], action.payload);
                }
            }
            return newState;
        default:
            return state;
    }
};

const configureStore = (preloadedState) => createStore(rootReducer, preloadedState);

class WvPbStore {
    constructor() {
        this.library = new Array();
        this.pageNodes = new Array();
        this.pageId = null;
        this.siteRootUrl = null;
        this.drake = new Object();
        this.activeNodeId = null;
        this.hoveredNodeId = null;
        this.hoveredContainerId = null;
        this.pageNodeChangeIndex = 1;
        this.isCreateModalVisible = false;
        this.createdNode = new Object;
        this.isOptionsModalVisible = false;
        this.isHelpModalVisible = false;
        this.reloadNodeIdList = new Array();
        this.componentMeta = new Object();
        this.recordId = null;
    }
}

var atoa = function atoa (a, n) { return Array.prototype.slice.call(a, n); };

var si = typeof setImmediate === 'function', tick;
if (si) {
  tick = function (fn) { setImmediate(fn); };
} else {
  tick = function (fn) { setTimeout(fn, 0); };
}

var tickyBrowser = tick;

var debounce = function debounce (fn, args, ctx) {
  if (!fn) { return; }
  tickyBrowser(function run () {
    fn.apply(ctx || null, args || []);
  });
};

var emitter = function emitter (thing, options) {
  var opts = options || {};
  var evt = {};
  if (thing === undefined) { thing = {}; }
  thing.on = function (type, fn) {
    if (!evt[type]) {
      evt[type] = [fn];
    } else {
      evt[type].push(fn);
    }
    return thing;
  };
  thing.once = function (type, fn) {
    fn._once = true; // thing.off(fn) still works!
    thing.on(type, fn);
    return thing;
  };
  thing.off = function (type, fn) {
    var c = arguments.length;
    if (c === 1) {
      delete evt[type];
    } else if (c === 0) {
      evt = {};
    } else {
      var et = evt[type];
      if (!et) { return thing; }
      et.splice(et.indexOf(fn), 1);
    }
    return thing;
  };
  thing.emit = function () {
    var args = atoa(arguments);
    return thing.emitterSnapshot(args.shift()).apply(this, args);
  };
  thing.emitterSnapshot = function (type) {
    var et = (evt[type] || []).slice(0);
    return function () {
      var args = atoa(arguments);
      var ctx = this || thing;
      if (type === 'error' && opts.throws !== false && !et.length) { throw args.length === 1 ? args[0] : args; }
      et.forEach(function emitter (listen) {
        if (opts.async) { debounce(listen, args, ctx); } else { listen.apply(ctx, args); }
        if (listen._once) { thing.off(type, listen); }
      });
      return thing;
    };
  };
  return thing;
};

var NativeCustomEvent = commonjsGlobal.CustomEvent;

function useNative () {
  try {
    var p = new NativeCustomEvent('cat', { detail: { foo: 'bar' } });
    return  'cat' === p.type && 'bar' === p.detail.foo;
  } catch (e) {
  }
  return false;
}

/**
 * Cross-browser `CustomEvent` constructor.
 *
 * https://developer.mozilla.org/en-US/docs/Web/API/CustomEvent.CustomEvent
 *
 * @public
 */

var customEvent = useNative() ? NativeCustomEvent :

// IE >= 9
'function' === typeof document.createEvent ? function CustomEvent (type, params) {
  var e = document.createEvent('CustomEvent');
  if (params) {
    e.initCustomEvent(type, params.bubbles, params.cancelable, params.detail);
  } else {
    e.initCustomEvent(type, false, false, void 0);
  }
  return e;
} :

// IE <= 8
function CustomEvent (type, params) {
  var e = document.createEventObject();
  e.type = type;
  if (params) {
    e.bubbles = Boolean(params.bubbles);
    e.cancelable = Boolean(params.cancelable);
    e.detail = params.detail;
  } else {
    e.bubbles = false;
    e.cancelable = false;
    e.detail = void 0;
  }
  return e;
};

var eventmap = [];
var eventname = '';
var ron = /^on/;

for (eventname in commonjsGlobal) {
  if (ron.test(eventname)) {
    eventmap.push(eventname.slice(2));
  }
}

var eventmap_1 = eventmap;

var doc = commonjsGlobal.document;
var addEvent = addEventEasy;
var removeEvent = removeEventEasy;
var hardCache = [];

if (!commonjsGlobal.addEventListener) {
  addEvent = addEventHard;
  removeEvent = removeEventHard;
}

var crossvent = {
  add: addEvent,
  remove: removeEvent,
  fabricate: fabricateEvent
};

function addEventEasy (el, type, fn, capturing) {
  return el.addEventListener(type, fn, capturing);
}

function addEventHard (el, type, fn) {
  return el.attachEvent('on' + type, wrap(el, type, fn));
}

function removeEventEasy (el, type, fn, capturing) {
  return el.removeEventListener(type, fn, capturing);
}

function removeEventHard (el, type, fn) {
  var listener = unwrap(el, type, fn);
  if (listener) {
    return el.detachEvent('on' + type, listener);
  }
}

function fabricateEvent (el, type, model) {
  var e = eventmap_1.indexOf(type) === -1 ? makeCustomEvent() : makeClassicEvent();
  if (el.dispatchEvent) {
    el.dispatchEvent(e);
  } else {
    el.fireEvent('on' + type, e);
  }
  function makeClassicEvent () {
    var e;
    if (doc.createEvent) {
      e = doc.createEvent('Event');
      e.initEvent(type, true, true);
    } else if (doc.createEventObject) {
      e = doc.createEventObject();
    }
    return e;
  }
  function makeCustomEvent () {
    return new customEvent(type, { detail: model });
  }
}

function wrapperFactory (el, type, fn) {
  return function wrapper (originalEvent) {
    var e = originalEvent || commonjsGlobal.event;
    e.target = e.target || e.srcElement;
    e.preventDefault = e.preventDefault || function preventDefault () { e.returnValue = false; };
    e.stopPropagation = e.stopPropagation || function stopPropagation () { e.cancelBubble = true; };
    e.which = e.which || e.keyCode;
    fn.call(el, e);
  };
}

function wrap (el, type, fn) {
  var wrapper = unwrap(el, type, fn) || wrapperFactory(el, type, fn);
  hardCache.push({
    wrapper: wrapper,
    element: el,
    type: type,
    fn: fn
  });
  return wrapper;
}

function unwrap (el, type, fn) {
  var i = find(el, type, fn);
  if (i) {
    var wrapper = hardCache[i].wrapper;
    hardCache.splice(i, 1); // free up a tad of memory
    return wrapper;
  }
}

function find (el, type, fn) {
  var i, item;
  for (i = 0; i < hardCache.length; i++) {
    item = hardCache[i];
    if (item.element === el && item.type === type && item.fn === fn) {
      return i;
    }
  }
}

var cache = {};
var start = '(?:^|\\s)';
var end = '(?:\\s|$)';

function lookupClass (className) {
  var cached = cache[className];
  if (cached) {
    cached.lastIndex = 0;
  } else {
    cache[className] = cached = new RegExp(start + className + end, 'g');
  }
  return cached;
}

function addClass (el, className) {
  var current = el.className;
  if (!current.length) {
    el.className = className;
  } else if (!lookupClass(className).test(current)) {
    el.className += ' ' + className;
  }
}

function rmClass (el, className) {
  el.className = el.className.replace(lookupClass(className), ' ').trim();
}

var classes = {
  add: addClass,
  rm: rmClass
};

var doc$1 = document;
var documentElement = doc$1.documentElement;

function dragula (initialContainers, options) {
	//console.log("dragula loaded boz");
  var len = arguments.length;
  if (len === 1 && Array.isArray(initialContainers) === false) {
    options = initialContainers;
    initialContainers = [];
  }
  var _mirror; // mirror image
  var _source; // source container
  var _item; // item being dragged
  var _offsetX; // reference x
  var _offsetY; // reference y
  var _moveX; // reference move x
  var _moveY; // reference move y
  var _initialSibling; // reference sibling when grabbed
  var _currentSibling; // reference sibling now
  var _copy; // item used for copying
  var _renderTimer; // timer for setTimeout renderMirrorImage
  var _lastDropTarget = null; // last container item was over
  var _grabbed; // holds mousedown context until first mousemove

  var o = options || {};
  if (o.moves === void 0) { o.moves = always; }
  if (o.accepts === void 0) { o.accepts = always; }
  if (o.invalid === void 0) { o.invalid = invalidTarget; }
  if (o.containers === void 0) { o.containers = initialContainers || []; }
  if (o.isContainer === void 0) { o.isContainer = never; }
  if (o.copy === void 0) { o.copy = false; }
  if (o.copySortSource === void 0) { o.copySortSource = false; }
  if (o.revertOnSpill === void 0) { o.revertOnSpill = false; }
  if (o.removeOnSpill === void 0) { o.removeOnSpill = false; }
  if (o.direction === void 0) { o.direction = 'vertical'; }
  if (o.ignoreInputTextSelection === void 0) { o.ignoreInputTextSelection = true; }
  if (o.mirrorContainer === void 0) { o.mirrorContainer = doc$1.body; }
	if (o.slideFactorY === void 0) { o.slideFactorY = 5; }
  if (o.slideFactorX === void 0) { o.slideFactorX = 5; }
  
  var drake = emitter({
    containers: o.containers,
    start: manualStart,
    end: end,
    cancel: cancel,
    remove: remove,
    destroy: destroy,
    canMove: canMove,
    dragging: false
  });

  if (o.removeOnSpill === true) {
    drake.on('over', spillOver).on('out', spillOut);
  }

  events();

  return drake;

  function isContainer (el) {
    return drake.containers.indexOf(el) !== -1 || o.isContainer(el);
  }

  function events (remove) {
    var op = remove ? 'remove' : 'add';
    touchy(documentElement, op, 'mousedown', grab);
    touchy(documentElement, op, 'mouseup', release);
  }

  function eventualMovements (remove) {
    var op = remove ? 'remove' : 'add';
    touchy(documentElement, op, 'mousemove', startBecauseMouseMoved);
  }

  function movements (remove) {
    var op = remove ? 'remove' : 'add';
    crossvent[op](documentElement, 'selectstart', preventGrabbed); // IE8
    crossvent[op](documentElement, 'click', preventGrabbed);
  }

  function destroy () {
    events(true);
    release({});
  }

  function preventGrabbed (e) {
    if (_grabbed) {
      e.preventDefault();
    }
  }

  function grab (e) {
    _moveX = e.clientX;
    _moveY = e.clientY;

    var ignore = whichMouseButton(e) !== 1 || e.metaKey || e.ctrlKey;
    if (ignore) {
      return; // we only care about honest-to-god left clicks and touch events
    }
    var item = e.target;
    var context = canStart(item);
    if (!context) {
      return;
    }
    _grabbed = context;
    eventualMovements();
    if (e.type === 'mousedown') {
      if (isInput(item)) { // see also: https://github.com/bevacqua/dragula/issues/208
        item.focus(); // fixes https://github.com/bevacqua/dragula/issues/176
      } else {
        e.preventDefault(); // fixes https://github.com/bevacqua/dragula/issues/155
      }
    }
  }

  function startBecauseMouseMoved (e) {
    if (!_grabbed) {
      return;
    }
    if (whichMouseButton(e) === 0) {
      release({});
      return; // when text is selected on an input and then dragged, mouseup doesn't fire. this is our only hope
    }
    // truthy check fixes #239, equality fixes #207
	if ((e.clientX !== void 0 && Math.abs(e.clientX - _moveX) <= (o.slideFactorX || 0)) &&
       (e.clientY !== void 0 && Math.abs(e.clientY - _moveY) <= (o.slideFactorY || 0))) {
		   console.log("boz");
      return;
    }
    if (o.ignoreInputTextSelection) {
      var clientX = getCoord('clientX', e);
      var clientY = getCoord('clientY', e);
      var elementBehindCursor = doc$1.elementFromPoint(clientX, clientY);
      if (isInput(elementBehindCursor)) {
        return;
      }
    }

    var grabbed = _grabbed; // call to end() unsets _grabbed
    eventualMovements(true);
    movements();
    end();
    start(grabbed);

    var offset = getOffset(_item);
    _offsetX = getCoord('pageX', e) - offset.left;
    _offsetY = getCoord('pageY', e) - offset.top;

    classes.add(_copy || _item, 'gu-transit');
    renderMirrorImage();
    drag(e);
  }

  function canStart (item) {
    if (drake.dragging && _mirror) {
      return;
    }
    if (isContainer(item)) {
      return; // don't drag container itself
    }
    var handle = item;
    while (getParent(item) && isContainer(getParent(item)) === false) {
      if (o.invalid(item, handle)) {
        return;
      }
      item = getParent(item); // drag target should be a top element
      if (!item) {
        return;
      }
    }
    var source = getParent(item);
    if (!source) {
      return;
    }
    if (o.invalid(item, handle)) {
      return;
    }

    var movable = o.moves(item, source, handle, nextEl(item));
    if (!movable) {
      return;
    }

    return {
      item: item,
      source: source
    };
  }

  function canMove (item) {
    return !!canStart(item);
  }

  function manualStart (item) {
    var context = canStart(item);
    if (context) {
      start(context);
    }
  }

  function start (context) {
    if (isCopy(context.item, context.source)) {
      _copy = context.item.cloneNode(true);
      drake.emit('cloned', _copy, context.item, 'copy');
    }

    _source = context.source;
    _item = context.item;
    _initialSibling = _currentSibling = nextEl(context.item);

    drake.dragging = true;
    drake.emit('drag', _item, _source);
  }

  function invalidTarget () {
    return false;
  }

  function end () {
    if (!drake.dragging) {
      return;
    }
    var item = _copy || _item;
    drop(item, getParent(item));
  }

  function ungrab () {
    _grabbed = false;
    eventualMovements(true);
    movements(true);
  }

  function release (e) {
    ungrab();

    if (!drake.dragging) {
      return;
    }
    var item = _copy || _item;
    var clientX = getCoord('clientX', e);
    var clientY = getCoord('clientY', e);
    var elementBehindCursor = getElementBehindPoint(_mirror, clientX, clientY);
    var dropTarget = findDropTarget(elementBehindCursor, clientX, clientY);
    if (dropTarget && ((_copy && o.copySortSource) || (!_copy || dropTarget !== _source))) {
      drop(item, dropTarget);
    } else if (o.removeOnSpill) {
      remove();
    } else {
      cancel();
    }
  }

  function drop (item, target) {
    var parent = getParent(item);
    if (_copy && o.copySortSource && target === _source) {
      parent.removeChild(_item);
    }
    if (isInitialPlacement(target)) {
      drake.emit('cancel', item, _source, _source);
    } else {
      drake.emit('drop', item, target, _source, _currentSibling);
    }
    cleanup();
  }

  function remove () {
    if (!drake.dragging) {
      return;
    }
    var item = _copy || _item;
    var parent = getParent(item);
    if (parent) {
      parent.removeChild(item);
    }
    drake.emit(_copy ? 'cancel' : 'remove', item, parent, _source);
    cleanup();
  }

  function cancel (revert) {
    if (!drake.dragging) {
      return;
    }
    var reverts = arguments.length > 0 ? revert : o.revertOnSpill;
    var item = _copy || _item;
    var parent = getParent(item);
    var initial = isInitialPlacement(parent);
    if (initial === false && reverts) {
      if (_copy) {
        if (parent) {
          parent.removeChild(_copy);
        }
      } else {
        _source.insertBefore(item, _initialSibling);
      }
    }
    if (initial || reverts) {
      drake.emit('cancel', item, _source, _source);
    } else {
      drake.emit('drop', item, parent, _source, _currentSibling);
    }
    cleanup();
  }

  function cleanup () {
    var item = _copy || _item;
    ungrab();
    removeMirrorImage();
    if (item) {
      classes.rm(item, 'gu-transit');
    }
    if (_renderTimer) {
      clearTimeout(_renderTimer);
    }
    drake.dragging = false;
    if (_lastDropTarget) {
      drake.emit('out', item, _lastDropTarget, _source);
    }
    drake.emit('dragend', item);
    _source = _item = _copy = _initialSibling = _currentSibling = _renderTimer = _lastDropTarget = null;
  }

  function isInitialPlacement (target, s) {
    var sibling;
    if (s !== void 0) {
      sibling = s;
    } else if (_mirror) {
      sibling = _currentSibling;
    } else {
      sibling = nextEl(_copy || _item);
    }
    return target === _source && sibling === _initialSibling;
  }

  function findDropTarget (elementBehindCursor, clientX, clientY) {
    var target = elementBehindCursor;
    while (target && !accepted()) {
      target = getParent(target);
    }
    return target;

    function accepted () {
      var droppable = isContainer(target);
      if (droppable === false) {
        return false;
      }

      var immediate = getImmediateChild(target, elementBehindCursor);
      var reference = getReference(target, immediate, clientX, clientY);
      var initial = isInitialPlacement(target, reference);
      if (initial) {
        return true; // should always be able to drop it right back where it was
      }
      return o.accepts(_item, target, _source, reference);
    }
  }

  function drag (e) {
    if (!_mirror) {
      return;
    }
    e.preventDefault();

    var clientX = getCoord('clientX', e);
    var clientY = getCoord('clientY', e);
    var x = clientX - _offsetX;
    var y = clientY - _offsetY;

    _mirror.style.left = x + 'px';
    _mirror.style.top = y + 'px';

    var item = _copy || _item;
    var elementBehindCursor = getElementBehindPoint(_mirror, clientX, clientY);
    var dropTarget = findDropTarget(elementBehindCursor, clientX, clientY);
    var changed = dropTarget !== null && dropTarget !== _lastDropTarget;
    if (changed || dropTarget === null) {
      out();
      _lastDropTarget = dropTarget;
      over();
    }
    var parent = getParent(item);
    if (dropTarget === _source && _copy && !o.copySortSource) {
      if (parent) {
        parent.removeChild(item);
      }
      return;
    }
    var reference;
    var immediate = getImmediateChild(dropTarget, elementBehindCursor);
    if (immediate !== null) {
      reference = getReference(dropTarget, immediate, clientX, clientY);
    } else if (o.revertOnSpill === true && !_copy) {
      reference = _initialSibling;
      dropTarget = _source;
    } else {
      if (_copy && parent) {
        parent.removeChild(item);
      }
      return;
    }
    if (
      (reference === null && changed) ||
      reference !== item &&
      reference !== nextEl(item)
    ) {
      _currentSibling = reference;
      dropTarget.insertBefore(item, reference);
      drake.emit('shadow', item, dropTarget, _source);
    }
    function moved (type) { drake.emit(type, item, _lastDropTarget, _source); }
    function over () { if (changed) { moved('over'); } }
    function out () { if (_lastDropTarget) { moved('out'); } }
  }

  function spillOver (el) {
    classes.rm(el, 'gu-hide');
  }

  function spillOut (el) {
    if (drake.dragging) { classes.add(el, 'gu-hide'); }
  }

  function renderMirrorImage () {
    if (_mirror) {
      return;
    }
    var rect = _item.getBoundingClientRect();
    _mirror = _item.cloneNode(true);
    _mirror.style.width = getRectWidth(rect) + 'px';
    _mirror.style.height = getRectHeight(rect) + 'px';
    classes.rm(_mirror, 'gu-transit');
    classes.add(_mirror, 'gu-mirror');
    o.mirrorContainer.appendChild(_mirror);
    touchy(documentElement, 'add', 'mousemove', drag);
    classes.add(o.mirrorContainer, 'gu-unselectable');
    drake.emit('cloned', _mirror, _item, 'mirror');
  }

  function removeMirrorImage () {
    if (_mirror) {
      classes.rm(o.mirrorContainer, 'gu-unselectable');
      touchy(documentElement, 'remove', 'mousemove', drag);
      getParent(_mirror).removeChild(_mirror);
      _mirror = null;
    }
  }

  function getImmediateChild (dropTarget, target) {
    var immediate = target;
    while (immediate !== dropTarget && getParent(immediate) !== dropTarget) {
      immediate = getParent(immediate);
    }
    if (immediate === documentElement) {
      return null;
    }
    return immediate;
  }

  function getReference (dropTarget, target, x, y) {
    var horizontal = o.direction === 'horizontal';
    var reference = target !== dropTarget ? inside() : outside();
    return reference;

    function outside () { // slower, but able to figure out any position
      var len = dropTarget.children.length;
      var i;
      var el;
      var rect;
      for (i = 0; i < len; i++) {
        el = dropTarget.children[i];
        rect = el.getBoundingClientRect();
        if (horizontal && (rect.left + rect.width / 2) > x) { return el; }
        if (!horizontal && (rect.top + rect.height / 2) > y) { return el; }
      }
      return null;
    }

    function inside () { // faster, but only available if dropped inside a child element
      var rect = target.getBoundingClientRect();
      if (horizontal) {
        return resolve(x > rect.left + getRectWidth(rect) / 2);
      }
      return resolve(y > rect.top + getRectHeight(rect) / 2);
    }

    function resolve (after) {
      return after ? nextEl(target) : target;
    }
  }

  function isCopy (item, container) {
    return typeof o.copy === 'boolean' ? o.copy : o.copy(item, container);
  }
}

function touchy (el, op, type, fn) {
  var touch = {
    mouseup: 'touchend',
    mousedown: 'touchstart',
    mousemove: 'touchmove'
  };
  var pointers = {
    mouseup: 'pointerup',
    mousedown: 'pointerdown',
    mousemove: 'pointermove'
  };
  var microsoft = {
    mouseup: 'MSPointerUp',
    mousedown: 'MSPointerDown',
    mousemove: 'MSPointerMove'
  };
  if (commonjsGlobal.navigator.pointerEnabled) {
    crossvent[op](el, pointers[type], fn);
  } else if (commonjsGlobal.navigator.msPointerEnabled) {
    crossvent[op](el, microsoft[type], fn);
  } else {
    crossvent[op](el, touch[type], fn);
    crossvent[op](el, type, fn);
  }
}

function whichMouseButton (e) {
  if (e.touches !== void 0) { return e.touches.length; }
  if (e.which !== void 0 && e.which !== 0) { return e.which; } // see https://github.com/bevacqua/dragula/issues/261
  if (e.buttons !== void 0) { return e.buttons; }
  var button = e.button;
  if (button !== void 0) { // see https://github.com/jquery/jquery/blob/99e8ff1baa7ae341e94bb89c3e84570c7c3ad9ea/src/event.js#L573-L575
    return button & 1 ? 1 : button & 2 ? 3 : (button & 4 ? 2 : 0);
  }
}

function getOffset (el) {
  var rect = el.getBoundingClientRect();
  return {
    left: rect.left + getScroll('scrollLeft', 'pageXOffset'),
    top: rect.top + getScroll('scrollTop', 'pageYOffset')
  };
}

function getScroll (scrollProp, offsetProp) {
  if (typeof commonjsGlobal[offsetProp] !== 'undefined') {
    return commonjsGlobal[offsetProp];
  }
  if (documentElement.clientHeight) {
    return documentElement[scrollProp];
  }
  return doc$1.body[scrollProp];
}

function getElementBehindPoint (point, x, y) {
  var p = point || {};
  var state = p.className;
  var el;
  p.className += ' gu-hide';
  el = doc$1.elementFromPoint(x, y);
  p.className = state;
  return el;
}

function never () { return false; }
function always () { return true; }
function getRectWidth (rect) { return rect.width || (rect.right - rect.left); }
function getRectHeight (rect) { return rect.height || (rect.bottom - rect.top); }
function getParent (el) { return el.parentNode === doc$1 ? null : el.parentNode; }
function isInput (el) { return el.tagName === 'INPUT' || el.tagName === 'TEXTAREA' || el.tagName === 'SELECT' || isEditable(el); }
function isEditable (el) {
  if (!el) { return false; } // no parents were editable
  if (el.contentEditable === 'false') { return false; } // stop the lookup
  if (el.contentEditable === 'true') { return true; } // found a contentEditable element in the chain
  return isEditable(getParent(el)); // contentEditable is set to 'inherit'
}

function nextEl (el) {
  return el.nextElementSibling || manually();
  function manually () {
    var sibling = el;
    do {
      sibling = sibling.nextSibling;
    } while (sibling && sibling.nodeType !== 1);
    return sibling;
  }
}

function getEventHost (e) {
  // on touchend event, we have to use `e.changedTouches`
  // see http://stackoverflow.com/questions/7192563/touchend-event-properties
  // see https://github.com/bevacqua/dragula/issues/34
  if (e.targetTouches && e.targetTouches.length) {
    return e.targetTouches[0];
  }
  if (e.changedTouches && e.changedTouches.length) {
    return e.changedTouches[0];
  }
  return e;
}

function getCoord (coord, e) {
  var host = getEventHost(e);
  var missMap = {
    pageX: 'clientX', // IE8
    pageY: 'clientY' // IE8
  };
  if (coord in missMap && !(coord in host) && missMap[coord] in host) {
    coord = missMap[coord];
  }
  return host[coord];
}

var dragula_1 = dragula;

function ProcessDropEvent(scope, moveObject, pageId, nodeId) {
    let requestConfig = {
        headers: {
            'Content-Type': 'application/json;charset=UTF-8',
            "Access-Control-Allow-Origin": "*",
        }
    };
    let siteRootUrl = scope.store.getState().siteRootUrl;
    let requestUrl = siteRootUrl + "/api/v3.0/page/" + pageId + "/node/" + nodeId + "/move";
    let recordId = scope.store.getState().recordId;
    if (recordId) {
        requestUrl += "?recId=" + recordId;
    }
    axios$1.post(requestUrl, moveObject, requestConfig)
        .then(function (response) {
        scope.updatePageNodes(response.data);
        window.setTimeout(function () {
            scope.addReloadNodeIds(scope.nodesPendingReload);
            scope.nodesPendingReload = new Array();
            window.setTimeout(function () {
                _.forEach(scope.nodesPendingReload, function (reloadNodeId) {
                    let componentObject = NodeUtils.GetNodeAndMeta(scope, reloadNodeId);
                    var customEvent = new Event("WvPbManager_Node_Moved");
                    var payload = new WvPbEventPayload();
                    payload.original_event = event;
                    payload.node = componentObject.node;
                    payload.component_name = componentObject.node["component_name"];
                    customEvent["payload"] = payload;
                    document.dispatchEvent(customEvent);
                });
                {
                    let componentObject = NodeUtils.GetNodeAndMeta(scope, nodeId);
                    var customEvent = new Event("WvPbManager_Node_Moved");
                    var payload = new WvPbEventPayload();
                    payload.original_event = event;
                    payload.node = componentObject.node;
                    payload.component_name = componentObject.node["component_name"];
                    customEvent["payload"] = payload;
                    document.dispatchEvent(customEvent);
                }
                var containerElList = document.querySelectorAll(".wv-container");
                _.forEach(containerElList, function (containerEl) {
                    containerEl.removeAttribute("style");
                });
            }, 10);
        }, 10);
    })
        .catch(function (error) {
        console.log(error);
        alert("An error occurred during the move");
        location.reload(true);
    });
}
function MoveAffectedNodesToStack(scope, moveInfo) {
    let nodeIds = new Array();
    let storePageNodes = scope.store.getState().pageNodes;
    NodeUtils.GetChildNodes(moveInfo.originParentNodeId, moveInfo.originContainerId, storePageNodes, nodeIds);
    NodeUtils.GetChildNodes(moveInfo.newParentNodeId, moveInfo.newContainerId, storePageNodes, nodeIds);
    _.forEach(nodeIds, function (nodeId) {
        NodeUtils.MoveNodeToStack(nodeId);
    });
    scope.nodesPendingReload = nodeIds;
}
function ShowTooltop(e) {
    var tooltip = document.querySelectorAll('.wv-pb-content .actions');
    for (var i = tooltip.length; i--;) {
        var tooltipEl = tooltip[i];
        tooltipEl.style.left = e.pageX + 15 + 'px';
        tooltipEl.style.top = e.pageY + 25 + 'px';
    }
}
function RemoveNode$1(scope) {
    let requestConfig = {
        headers: {
            'Content-Type': 'application/json;charset=UTF-8',
            "Access-Control-Allow-Origin": "*",
        }
    };
    let componentObject = NodeUtils.GetActiveNodeAndMeta(scope);
    let siteUrl = scope.store.getState().siteRootUrl;
    let requestUrl = siteUrl + "/api/v3.0/page/" + componentObject.node["page_id"] + "/node/" + componentObject.node["id"] + "/delete";
    let recordId = scope.store.getState().recordId;
    if (recordId) {
        requestUrl += "?recId=" + recordId;
    }
    axios$1.post(requestUrl, null, requestConfig)
        .then(function () {
        var customEvent = new Event("WvPbManager_Node_Removed");
        var payload = new WvPbEventPayload();
        payload.original_event = event;
        payload.node = componentObject.node;
        payload.component_name = componentObject.node["component_name"];
        customEvent["payload"] = payload;
        document.dispatchEvent(customEvent);
        scope.removeNode(scope.store.getState().activeNodeId);
    })
        .catch(function (error) {
        if (error.response) {
            alert(error.response.statusText + ":" + error.response.data);
        }
        else if (error.message) {
            alert(error.message);
        }
        else {
            alert(error);
        }
    });
}
class WvPageManager {
    constructor() {
        this.libraryJson = "[]";
        this.pageNodesJson = "";
        this.nodesPendingReload = new Array();
        this.pageNodes = new Array();
    }
    componentWillLoad() {
        let library = JSON.parse(this.libraryJson);
        let pageNodes = new Array();
        if (this.pageNodesJson) {
            pageNodes = JSON.parse(this.pageNodesJson);
        }
        _.forEach(pageNodes, function (node) {
            if (typeof node["options"] !== 'object') {
                if (!node["options"]) {
                    node["options"] = {};
                }
                else {
                    node["options"] = JSON.parse(node["options"]);
                }
            }
        });
        var initStore = new WvPbStore();
        initStore.library = library;
        initStore.pageNodes = pageNodes;
        initStore.siteRootUrl = this.siteRootUrl;
        initStore.pageId = this.pageId;
        initStore.recordId = this.recordId;
        _.forEach(library, function (component) {
            initStore.componentMeta[component["name"]] = component;
        });
        this.store.setStore(configureStore(initStore));
        this.store.mapStateToProps(this, (state) => {
            return {
                pageNodes: state.pageNodes
            };
        });
        this.store.mapDispatchToProps(this, {
            setDrake: setDrake,
            addReloadNodeIds: addReloadNodeIds,
            updatePageNodes: updatePageNodes,
            removeNode: removeNode
        });
        let scope = this;
        let drake = dragula_1({
            revertOnSpill: true,
            direction: 'vertical',
        });
        drake.on('drop', function (el, target, source) {
            let newIndex = 0;
            _.forEach(el.parentElement.childNodes, function (node) {
                if (node === el) {
                    return false;
                }
                newIndex++;
            });
            var moveInfo = {
                originContainerId: source.getAttribute("data-container-id"),
                originParentNodeId: source.getAttribute("data-parent-id"),
                newContainerId: target.getAttribute("data-container-id"),
                newParentNodeId: target.getAttribute("data-parent-id"),
                newIndex: newIndex,
            };
            let pageId = el.getAttribute("data-page-id");
            let nodeId = el.getAttribute("data-node-id");
            MoveAffectedNodesToStack(scope, moveInfo);
            ProcessDropEvent(scope, moveInfo, pageId, nodeId);
        });
        drake.on('drag', function () {
            var containerElList = document.querySelectorAll(".wv-container");
            _.forEach(containerElList, function (containerEl) {
                let elWidth = containerEl.offsetWidth;
                containerEl.setAttribute("style", "width:" + elWidth + "px");
            });
        });
        scope.setDrake(drake);
    }
    handleMouseMove(ev) {
        ShowTooltop(ev);
    }
    componentDidLoad() {
        let scope = this;
        document.addEventListener('keydown', function (ev) {
            switch (ev.key) {
                case "Escape":
                    var drake = scope.store.getState().drake;
                    if (drake.dragging) {
                        drake.cancel();
                    }
                    break;
                case "Delete":
                    let activeNodeId = scope.store.getState().activeNodeId;
                    let isOptionsModalVisible = scope.store.getState().isOptionsModalVisible;
                    let isHelpModalVisible = scope.store.getState().isHelpModalVisible;
                    let isCreateModalVisible = scope.store.getState().isCreateModalVisible;
                    if (activeNodeId && !isOptionsModalVisible && !isHelpModalVisible && !isCreateModalVisible) {
                        if (window.confirm('Are you sure you wish to delete the selected component?')) {
                            RemoveNode$1(scope);
                        }
                    }
                    break;
                default:
                    break;
            }
        }, false);
    }
    render() {
        let scope = this;
        let registeredComponentNameservices = [];
        let library = this.store.getState().library;
        return (h("div", { id: "wv-page-manager-wrapper" },
            h("div", { class: "row no-gutters" },
                h("div", { class: "col", style: { "overflow-x": "auto" } },
                    h("div", { class: "wv-pb-content" },
                        h("div", { class: "wb-pb-content-inner" },
                            h("wv-pb-node-container", { "parent-node-id": null, containerId: "" })))),
                h("div", { class: "col-auto", style: { width: "400px" } },
                    h("wv-pb-inspector", null),
                    h("wv-pb-tree", null))),
            this.pageNodes.map(function (node) {
                let nodeComponentName = node["component_name"];
                let componentNameIndex = _.findIndex(registeredComponentNameservices, function (x) { return x === nodeComponentName; });
                if (componentNameIndex === -1) {
                    let libObjIndex = _.findIndex(library, function (x) { return x["name"] == nodeComponentName; });
                    if (libObjIndex > -1) {
                        if (library[libObjIndex]["service_js_url"]) {
                            registeredComponentNameservices.push(nodeComponentName);
                            return (h("script", { key: node["id"], src: scope.siteRootUrl + library[libObjIndex]["service_js_url"] }));
                        }
                        else {
                            console.info("Service not found for " + nodeComponentName);
                            return null;
                        }
                    }
                }
                return null;
            }),
            h("div", { id: "wv-node-design-stack", class: "d-none" }),
            h("div", { id: "wv-node-options-stack", class: "d-none" }),
            h("div", { id: "wv-node-help-stack", class: "d-none" }),
            h("wv-create-modal", null),
            h("wv-help-modal", null),
            h("wv-options-modal", null)));
    }
    static get is() { return "wv-pb-manager"; }
    static get properties() { return {
        "libraryJson": {
            "type": String,
            "attr": "library-json"
        },
        "nodesPendingReload": {
            "state": true
        },
        "pageId": {
            "type": String,
            "attr": "page-id"
        },
        "pageNodes": {
            "state": true
        },
        "pageNodesJson": {
            "type": String,
            "attr": "page-nodes-json"
        },
        "recordId": {
            "type": String,
            "attr": "record-id"
        },
        "siteRootUrl": {
            "type": String,
            "attr": "site-root-url"
        },
        "store": {
            "context": "store"
        }
    }; }
    static get listeners() { return [{
            "name": "mousemove",
            "method": "handleMouseMove",
            "passive": true
        }]; }
}

function LoadTemplate(scope) {
    let responseObject = NodeUtils.GetNodeAndMeta(scope, scope.nodeId);
    let errorMessage = null;
    if (responseObject) {
        let requestConfig = {
            headers: {
                'Content-Type': 'application/json;charset=UTF-8',
                "Access-Control-Allow-Origin": "*",
            }
        };
        let siteRootUrl = scope.store.getState().siteRootUrl;
        let requestUrl = siteRootUrl + responseObject.meta["design_view_url"] + "&nid=" + responseObject.node["id"] + "&pid=" + responseObject.node["page_id"];
        let recordId = scope.store.getState().recordId;
        if (recordId) {
            requestUrl += "&recId=" + recordId;
        }
        let requestBody = responseObject.node["options"];
        scope.isLoading = true;
        axios$1.post(requestUrl, requestBody, requestConfig)
            .then(function (response) {
            scope.isLoading = false;
            let nodeContainerPlaceholder = document.getElementById("wv-node-" + scope.nodeId);
            let nodeDiv = document.createElement("div");
            nodeDiv.id = "node-design-" + scope.nodeId;
            nodeDiv.classList.add("wv-pb-node");
            nodeDiv.innerHTML = response.data;
            nodeContainerPlaceholder.appendChild(nodeDiv);
            runScripts(nodeDiv);
            var customEvent = new Event("WvPbManager_Design_Loaded");
            var payload = new WvPbEventPayload();
            payload.node = responseObject.node;
            payload.component_name = responseObject.node["component_name"];
            customEvent["payload"] = payload;
            document.dispatchEvent(customEvent);
        })
            .catch(function (error) {
            if (error.response) {
                if (error.response.data) {
                    errorMessage = error.response.data;
                }
                else {
                    errorMessage = error.response.statusText;
                }
            }
            else if (error.message) {
                errorMessage = error.message;
            }
            else {
                errorMessage = error;
            }
            if (errorMessage) {
                let nodeContainerPlaceholder = document.getElementById("wv-node-" + scope.nodeId);
                let errorDiv = document.createElement("div");
                errorDiv.classList.add("alert");
                errorDiv.classList.add("alert-danger");
                errorDiv.classList.add("m-1");
                errorDiv.classList.add("p-1");
                errorDiv.innerHTML = errorMessage;
                nodeContainerPlaceholder.appendChild(errorDiv);
                scope.isLoading = false;
            }
        });
    }
}
class WvNode {
    constructor() {
        this.isLoading = false;
    }
    componentWillLoad() {
        let scope = this;
        if (!scope.nodeId) {
            return;
        }
        this.store.mapStateToProps(this, (state) => {
            return {
                reloadNodeIdList: state.reloadNodeIdList
            };
        });
        this.store.mapDispatchToProps(this, {
            removeReloadNodeIds: removeReloadNodeIds
        });
    }
    nodeIndexUpdateHandler(newValue) {
        let scope = this;
        let reloadIndex = _.findIndex(newValue, function (reloadId) { return reloadId === scope.nodeId; });
        if (reloadIndex > -1) {
            if (!scope.el.parentElement.classList.contains("gu-mirror")) {
                let isMoveSuccess = NodeUtils.GetNodeFromStack(scope.nodeId);
                if (!isMoveSuccess) {
                    LoadTemplate(scope);
                }
                if (scope && typeof scope.removeReloadNodeIds === "function") {
                    scope.removeReloadNodeIds(scope.nodeId);
                }
            }
        }
    }
    render() {
        let scope = this;
        if (this.el.parentElement.classList.contains("gu-mirror")) {
            return null;
        }
        if (scope.isLoading) {
            return (h("wv-loading-pane", null));
        }
        return null;
    }
    static get is() { return "wv-pb-node"; }
    static get properties() { return {
        "el": {
            "elementRef": true
        },
        "isLoading": {
            "state": true
        },
        "nodeId": {
            "type": String,
            "attr": "node-id"
        },
        "reloadNodeIdList": {
            "state": true,
            "watchCallbacks": ["nodeIndexUpdateHandler"]
        },
        "store": {
            "context": "store"
        }
    }; }
}

class guid {
    static newGuid() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            const r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }
}

function GetContainerNodes(parentNodeId, containerId, pageNodes) {
    let containerNodes = new Array();
    _.forEach(pageNodes, function (node) {
        if (!parentNodeId && !node["parent_id"]) {
            if (!containerId && !node["container_id"]) {
                containerNodes.push(node);
            }
            else if (containerId && node["container_id"] && containerId.toLowerCase() === node["container_id"].toLowerCase()) {
                containerNodes.push(node);
            }
        }
        else if (parentNodeId && node["parent_id"] && parentNodeId.toLowerCase() === node["parent_id"].toLowerCase()) {
            if (!containerId && !node["container_id"]) {
                containerNodes.push(node);
            }
            else if (containerId && node["container_id"] && containerId.toLowerCase() === node["container_id"].toLowerCase()) {
                containerNodes.push(node);
            }
        }
    });
    return _.sortBy(containerNodes, ['weight']);
}
class WvContainer {
    constructor() {
        this.containerId = null;
        this.parentNodeId = null;
    }
    componentWillLoad() {
        this.store.mapStateToProps(this, (state) => {
            return {
                activeNodeId: state.activeNodeId,
                hoveredNodeId: state.hoveredNodeId,
                hoveredContainerId: state.hoveredContainerId,
                pageNodeChangeIndex: state.pageNodeChangeIndex
            };
        });
        this.store.mapDispatchToProps(this, {
            addDrakeContainerId: addDrakeContainerId,
            hoverContainer: hoverContainer,
            hoverNode: hoverNode,
            setActiveNode: setActiveNode,
            setNodeCreation: setNodeCreation,
            addReloadNodeIds: addReloadNodeIds
        });
    }
    componentDidLoad() {
        let scope = this;
        scope.addDrakeContainerId("wv-container-" + scope.parentNodeId + "-" + scope.containerId);
        let containerNodes = GetContainerNodes(scope.parentNodeId, scope.containerId, scope.store.getState().pageNodes);
        let loadNodeIdList = new Array();
        _.forEach(containerNodes, function (childNode) {
            loadNodeIdList.push(childNode["id"]);
        });
        scope.addReloadNodeIds(loadNodeIdList);
    }
    pageNodeIndexChangeHandler() {
        let scope = this;
        let containerHtmlId = "wv-container-" + scope.parentNodeId + "-" + scope.containerId;
        let container = document.getElementById(containerHtmlId);
        if (container) {
            let drake = scope.store.getState().drake;
            let drakeContainers = drake.containers;
            let currentDrakeIndex = _.findIndex(drakeContainers, function (drakeContainer) { return drakeContainer.id === containerHtmlId; });
            if (currentDrakeIndex == -1) {
                drake.containers.push(container);
            }
        }
    }
    hoverContainerHandler(event) {
        event.preventDefault();
        event.stopPropagation();
        let containerId = "wv-container-" + this.parentNodeId + "-" + this.containerId;
        this.hoverContainer(containerId);
    }
    unhoverContainerHandler(event) {
        event.preventDefault();
        event.stopPropagation();
        let closestHovarableNode = event.target.parentNode.closest(".wv-pb-node.hovarable");
        if (closestHovarableNode) {
            let elNodeIdAttr = closestHovarableNode.attributes["data-node-id"];
            if (elNodeIdAttr) {
                this.hoverNode(elNodeIdAttr.value);
            }
        }
        else {
            this.hoverContainer(null);
        }
    }
    nodeClickHandler(event, nodeId) {
        event.preventDefault();
        event.stopPropagation();
        this.setActiveNode(nodeId);
    }
    hoverNodeHandler(event) {
        event.preventDefault();
        event.stopPropagation();
        let nodeId = event.target.getAttribute("data-node-id");
        this.hoverNode(nodeId);
    }
    unhoverNodeHandler(event) {
        event.preventDefault();
        event.stopPropagation();
        let closestHovarableContainer = event.target.parentNode.closest(".wv-container-inner.hovarable");
        if (closestHovarableContainer) {
            let elNodeIdAttr = closestHovarableContainer.attributes["data-container-id"];
            if (elNodeIdAttr) {
                this.hoverContainer(elNodeIdAttr.value);
            }
        }
        else {
            this.hoverNode(null);
        }
    }
    addNodeHandler(event) {
        event.preventDefault();
        event.stopPropagation();
        let biggestWeight = 0;
        let containerNodes = GetContainerNodes(this.parentNodeId, this.containerId, this.store.getState().pageNodes);
        _.forEach(containerNodes, function (node) {
            if (node["weight"] > biggestWeight) {
                biggestWeight = node["weight"];
            }
        });
        let nodeObj = {
            id: guid.newGuid(),
            container_id: this.containerId,
            parent_id: this.parentNodeId,
            weight: biggestWeight + 1,
            page_id: this.store.getState().pageId,
            component_name: null,
            node_id: null,
            options: {},
            nodes: []
        };
        this.setNodeCreation(nodeObj);
    }
    render() {
        let scope = this;
        let containerNodes = GetContainerNodes(scope.parentNodeId, scope.containerId, scope.store.getState().pageNodes);
        let containerElId = "wv-container-" + scope.parentNodeId + "-" + scope.containerId;
        let containerClass = "";
        if (!scope.containerId) {
            containerClass += " first";
        }
        if (scope.hoveredContainerId == containerElId) {
            containerClass += " hovered";
        }
        if (containerNodes.length === 0) {
            containerClass += " empty";
        }
        let componentMeta = scope.store.getState().componentMeta;
        return (h("div", { class: "wv-container-inner hovarable " + containerClass, onClick: (e) => this.addNodeHandler(e), onMouseEnter: (event) => scope.hoverContainerHandler(event), onMouseLeave: (event) => scope.unhoverContainerHandler(event), "data-container-id": containerElId },
            h("div", { class: "actions" },
                h("i", { class: "fa fa-plus go-green" }),
                " add in ",
                scope.containerId),
            h("div", { class: "wv-container", id: containerElId, "data-parent-id": scope.parentNodeId, "data-container-id": scope.containerId }, containerNodes.map(function (node) {
                let nodeClass = "";
                if (scope.activeNodeId && node["id"] === scope.activeNodeId) {
                    nodeClass += " selected";
                }
                if (scope.hoveredNodeId && node["id"] === scope.hoveredNodeId && !scope.hoveredContainerId) {
                    nodeClass += " hovered";
                }
                if (componentMeta[node["component_name"]]["is_inline"]) {
                    nodeClass += " d-inline-block";
                }
                return (h("div", { key: node["id"], id: "wv-node-" + node["id"], class: "wv-node-wrapper draggable-node hovarable " + nodeClass, "data-node-id": node["id"], "data-page-id": node["page_id"], onClick: (event) => scope.nodeClickHandler(event, node["id"]), onMouseEnter: (event) => scope.hoverNodeHandler(event), onMouseLeave: (event) => scope.unhoverNodeHandler(event) },
                    h("div", { class: "actions" },
                        h("i", { class: "fa fa-search go-blue" }),
                        " select ",
                        componentMeta[node["component_name"]]["label"]),
                    h("wv-pb-node", { nodeId: node["id"] })));
            }))));
    }
    static get is() { return "wv-pb-node-container"; }
    static get properties() { return {
        "activeNodeId": {
            "state": true
        },
        "containerId": {
            "type": String,
            "attr": "container-id"
        },
        "hoveredContainerId": {
            "state": true
        },
        "hoveredNodeId": {
            "state": true
        },
        "pageNodeChangeIndex": {
            "state": true,
            "watchCallbacks": ["pageNodeIndexChangeHandler"]
        },
        "parentNodeId": {
            "type": String,
            "attr": "parent-node-id"
        },
        "store": {
            "context": "store"
        }
    }; }
}

class WvCreateModal$3 {
    constructor() {
        this.nodeId = "";
        this.isHelpModalVisible = false;
    }
    componentWillLoad() {
        this.store.mapStateToProps(this, (state) => {
            return {
                isHelpModalVisible: state.isHelpModalVisible,
            };
        });
        let nodeHelpTemplate = document.getElementById("node-help-" + this.nodeId);
        let helpModalPlaceholder = document.getElementById("modal-component-help-body");
        if (nodeHelpTemplate) {
            helpModalPlaceholder.appendChild(nodeHelpTemplate);
        }
    }
    helpModalVisibilityHandler(newValue, oldValue) {
        if (!newValue && oldValue) {
            let nodeHelpStack = document.getElementById("wv-node-help-stack");
            let nodeHelpTemplate = document.getElementById("node-help-" + this.nodeId);
            if (nodeHelpTemplate) {
                nodeHelpStack.appendChild(nodeHelpTemplate);
            }
        }
    }
    render() {
        return null;
    }
    static get is() { return "wv-show-help"; }
    static get properties() { return {
        "isHelpModalVisible": {
            "state": true,
            "watchCallbacks": ["helpModalVisibilityHandler"]
        },
        "nodeId": {
            "type": String,
            "attr": "node-id"
        },
        "store": {
            "context": "store"
        }
    }; }
}

class WvCreateModal$4 {
    constructor() {
        this.nodeId = "";
        this.isOptionsModalVisible = false;
    }
    componentWillLoad() {
        this.store.mapStateToProps(this, (state) => {
            return {
                isOptionsModalVisible: state.isOptionsModalVisible,
            };
        });
        let nodeOptionsTemplate = document.getElementById("node-options-" + this.nodeId);
        let OptionsModalPlaceholder = document.getElementById("modal-component-options-body");
        if (nodeOptionsTemplate) {
            OptionsModalPlaceholder.appendChild(nodeOptionsTemplate);
        }
    }
    optionsModalVisibilityHandler(newValue, oldValue) {
        if (!newValue && oldValue) {
            let nodeOptionsStack = document.getElementById("wv-node-options-stack");
            let nodeOptionsTemplate = document.getElementById("node-options-" + this.nodeId);
            if (nodeOptionsTemplate) {
                nodeOptionsStack.appendChild(nodeOptionsTemplate);
            }
        }
    }
    render() {
        return null;
    }
    static get is() { return "wv-show-options"; }
    static get properties() { return {
        "isOptionsModalVisible": {
            "state": true,
            "watchCallbacks": ["optionsModalVisibilityHandler"]
        },
        "nodeId": {
            "type": String,
            "attr": "node-id"
        },
        "store": {
            "context": "store"
        }
    }; }
}

export { WvCreateModal, WvCreateModal$1 as WvHelpModal, WvLoadingPane, WvCreateModal$2 as WvOptionsModal, WvPbInspector, WvPageManager as WvPbManager, WvNode as WvPbNode, WvContainer as WvPbNodeContainer, WvCreateModal$3 as WvShowHelp, WvCreateModal$4 as WvShowOptions };
