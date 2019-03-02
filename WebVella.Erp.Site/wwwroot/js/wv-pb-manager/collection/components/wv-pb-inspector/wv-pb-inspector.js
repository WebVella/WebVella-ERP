import _ from 'lodash';
import axios from 'axios';
import WvPbEventPayload from "../../models/WvPbEventPayload";
import * as action from "../../store/actions";
import runScripts from '../../utils/run-scripts';
import NodeUtils from '../../utils/node';
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
            h("i", { class: "fa fa-trash-alt" }),
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
    axios.post(requestUrl, null, requestConfig)
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
        axios.post(requestUrl, requestBody, requestConfig)
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
        axios.post(requestUrl, requestBody, requestConfig)
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
export class WvPbInspector {
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
            removeNode: action.removeNode,
            setOptionsModalState: action.setOptionsModalState,
            setHelpModalState: action.setHelpModalState
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
