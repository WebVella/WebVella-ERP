import * as actionType from "./action-types";
import _ from "lodash";
import moment from "moment";
const initialState = {};
const rootReducer = (state = initialState, action) => {
    let newState = Object.assign({}, state);
    switch (action.type) {
        case actionType.SET_DRAKE:
            {
                newState["drake"] = action.payload;
            }
            return newState;
        case actionType.ADD_DRAKE_CONTAINER_ID:
            {
                let container = document.getElementById(action.payload);
                let drake = newState["drake"];
                drake.containers.push(container);
            }
            return newState;
        case actionType.SET_ACTIVE_NODE:
            {
                if (newState["activeNodeId"] && newState["activeNodeId"] === action.payload) {
                    newState["activeNodeId"] = null;
                }
                else {
                    newState["activeNodeId"] = action.payload;
                }
            }
            return newState;
        case actionType.HOVER_NODE:
            {
                newState["hoveredNodeId"] = action.payload;
                if (action.payload) {
                    newState["hoveredContainerId"] = null;
                }
            }
            return newState;
        case actionType.HOVER_CONTAINER:
            {
                newState["hoveredContainerId"] = action.payload;
                if (action.payload) {
                    newState["hoveredNodeId"] = null;
                }
            }
            return newState;
        case actionType.SET_NODE_CREATION:
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
        case actionType.ADD_NODE:
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
                let componentName = node["component_name"];
                let libraryComponentIndex = _.findIndex(newState["library"], function (record) { return record["name"] === componentName; });
                if (libraryComponentIndex > -1) {
                    newState["library"][libraryComponentIndex]["usage_counter"]++;
                    newState["library"][libraryComponentIndex]["last_used_on"] = moment().format('YYYY-MM-DDTHH:mm:ss');
                }
            }
            return newState;
        case actionType.REMOVE_NODE:
            {
                newState["pageNodes"] = _.filter(newState["pageNodes"], function (record) {
                    return record["id"].toLowerCase() !== action.payload.toLowerCase();
                });
                newState["activeNodeId"] = null;
                newState["pageNodeChangeIndex"]++;
            }
            return newState;
        case actionType.UPDATE_NODE_OPTIONS:
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
        case actionType.UPDATE_PAGE_NODES:
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
        case actionType.SET_OPTIONS_MODAL_STATE:
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
        case actionType.SET_HELP_MODAL_STATE:
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
        case actionType.ADD_RELOAD_NODE_IDS:
            {
                if (action.payload) {
                    newState["reloadNodeIdList"] = _.concat(newState["reloadNodeIdList"], action.payload);
                }
            }
            return newState;
        case actionType.REMOVE_RELOAD_NODE_IDS:
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
export default rootReducer;
