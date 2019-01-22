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
export default runScripts;
