import _ from 'lodash';
import axios from 'axios';
import runScripts from '../../utils/run-scripts';
import WvPbEventPayload from "../../models/WvPbEventPayload";
import NodeUtils from '../../utils/node';
import * as action from '../../store/actions';
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
        axios.post(requestUrl, requestBody, requestConfig)
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
export class WvNode {
    constructor() {
        this.isLoading = false;
        this.reloadNodeIdList = new Array();
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
            removeReloadNodeIds: action.removeReloadNodeIds
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
