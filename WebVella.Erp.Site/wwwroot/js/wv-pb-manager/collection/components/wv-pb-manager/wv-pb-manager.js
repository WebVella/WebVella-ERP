import { configureStore } from '../../store/index';
import WvPbStore from '../../models/WvPbStore';
import * as action from '../../store/actions';
import dragula from 'dragula';
import _ from 'lodash';
import axios from 'axios';
import NodeUtils from '../../utils/node';
import WvPbEventPayload from "../../models/WvPbEventPayload";
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
    axios.post(requestUrl, moveObject, requestConfig)
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
export class WvPageManager {
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
            setDrake: action.setDrake,
            addReloadNodeIds: action.addReloadNodeIds,
            updatePageNodes: action.updatePageNodes,
            removeNode: action.removeNode
        });
        let scope = this;
        let drake = dragula({
            revertOnSpill: false,
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
                            RemoveNode(scope);
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
