import axios from 'axios';
function ErrorScreen(props) {
    return (h("div", { class: "go-red" }, props.error));
}
function ComponentBody(props) {
    let scope = props.scope;
    if (scope.error) {
        return (h(ErrorScreen, { error: scope.error }));
    }
    else {
        return (h("div", { id: "lazyload-" + scope.nodeId }));
    }
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
function ComponentLoadedCallback(scope) {
    let injectTarget = document.querySelector("#lazyload-" + scope.nodeId);
    if (injectTarget) {
        var injectorDiv = document.createElement("div");
        injectorDiv.setAttribute("id", "lazyload-injector-" + scope.nodeId);
        injectorDiv.innerHTML = scope.viewHtml;
        injectTarget.appendChild(injectorDiv);
        runScripts(injectorDiv);
    }
}
export class MyComponent {
    constructor() {
        this.isLoading = true;
        this.error = "";
    }
    componentWillLoad() {
        let scope = this;
        scope.isLoading = true;
        scope.viewHtml = "";
        let requestConfig = {
            headers: {
                'Content-Type': 'application/json;charset=UTF-8',
                "Access-Control-Allow-Origin": "*",
            }
        };
        let apiUrl = scope.siteRootUrl + "/api/v3.0/pc/" + scope.componentName + "/view/display?nid=" + scope.nodeId + "&pid=" + scope.pageId;
        let options = JSON.parse(scope.nodeOptions);
        axios.post(apiUrl, options, requestConfig)
            .then(function (response) {
            scope.viewHtml = response.data;
            scope.isLoading = false;
            window.setTimeout(function () {
                ComponentLoadedCallback(scope);
            }, 100);
        })
            .catch(function (error) {
            if (error.response) {
                scope.error = error.response.statusText + ":" + error.response.data;
            }
            else {
                scope.error = error.message;
            }
            scope.isLoading = false;
        });
    }
    render() {
        if (this.isLoading) {
            return h("div", { class: "loading-panel" }, "Loading...");
        }
        return (h(ComponentBody, { scope: this }));
    }
    static get is() { return "wv-lazyload"; }
    static get properties() { return {
        "componentName": {
            "type": String,
            "attr": "component-name"
        },
        "entityId": {
            "type": String,
            "attr": "entity-id"
        },
        "error": {
            "state": true
        },
        "isLoading": {
            "state": true
        },
        "nodeId": {
            "type": String,
            "attr": "node-id"
        },
        "nodeOptions": {
            "type": String,
            "attr": "node-options"
        },
        "pageId": {
            "type": String,
            "attr": "page-id"
        },
        "recordId": {
            "type": String,
            "attr": "record-id"
        },
        "siteRootUrl": {
            "type": String,
            "attr": "site-root-url"
        },
        "viewHtml": {
            "state": true
        }
    }; }
}
