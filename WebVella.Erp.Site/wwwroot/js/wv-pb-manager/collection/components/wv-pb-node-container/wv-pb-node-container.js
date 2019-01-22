import * as action from '../../store/actions';
import _ from "lodash";
import guid from "../../utils/guid";
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
    if (containerNodes.length === 0) {
    }
    return _.sortBy(containerNodes, ['weight']);
}
export class WvContainer {
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
            addDrakeContainerId: action.addDrakeContainerId,
            hoverContainer: action.hoverContainer,
            hoverNode: action.hoverNode,
            setActiveNode: action.setActiveNode,
            setNodeCreation: action.setNodeCreation,
            addReloadNodeIds: action.addReloadNodeIds
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
