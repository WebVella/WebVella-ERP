import * as action from '../../store/actions';
import _ from 'lodash';
function GetChildNodes(scope) {
    let childNodes = new Array();
    let pageNodes = scope.store.getState().pageNodes;
    let nodeId = scope.node["id"];
    _.forEach(pageNodes, function (node) {
        if (nodeId && node["parent_id"] && nodeId.toLowerCase() === node["parent_id"].toLowerCase() && nodeId.toLowerCase() !== node["id"].toLowerCase()) {
            childNodes.push(node);
        }
    });
    return _.sortBy(childNodes, ['weight']);
}
function GetMeta(scope) {
    let library = scope.store.getState().library;
    let metaIndex = _.findIndex(library, (x) => x["name"] === scope.node["component_name"]);
    if (metaIndex > -1) {
        return library[metaIndex];
    }
    return null;
}
export class WvPbTreeNode {
    constructor() {
        this.level = 0;
        this.activeNodeId = null;
        this.hoveredNodeId = null;
        this.pageNodeChangeIndex = 1;
    }
    componentWillLoad() {
        let scope = this;
        scope.store.mapStateToProps(this, (state) => {
            return {
                activeNodeId: state.activeNodeId,
                hoveredNodeId: state.hoveredNodeId,
                pageNodeChangeIndex: state.pageNodeChangeIndex
            };
        });
        scope.store.mapDispatchToProps(this, {
            setActiveNode: action.setActiveNode,
            hoverNode: action.hoverNode
        });
    }
    nodeClickHandler(event) {
        event.preventDefault();
        event.stopPropagation();
        this.setActiveNode(this.node["id"]);
    }
    hoverNodeHandler(event) {
        event.preventDefault();
        event.stopPropagation();
        this.hoverNode(this.node["id"]);
    }
    unhoverNodeHandler(event) {
        event.preventDefault();
        event.stopPropagation();
        this.hoverNode(null);
    }
    render() {
        let scope = this;
        let componentMeta = GetMeta(scope);
        let childNodes = GetChildNodes(scope);
        let iconClass = "fa-file";
        if (componentMeta["icon_class"]) {
            iconClass = componentMeta["icon_class"];
        }
        let activeClass = "";
        if (scope.activeNodeId && scope.activeNodeId === scope.node["id"]) {
            activeClass = " selected";
        }
        let hoveredClass = "";
        if (scope.hoveredNodeId && scope.hoveredNodeId === scope.node["id"]) {
            hoveredClass = " hovered";
        }
        let paddingLeftString = "15px";
        if (scope.level === 0) {
            paddingLeftString = "0px";
        }
        let nodeContainers = new Array();
        _.forEach(childNodes, function (node) {
            let ncId = node["container_id"];
            let ncIdIndex = _.findIndex(nodeContainers, function (record) { return record === ncId; });
            if (ncIdIndex === -1) {
                nodeContainers.push(ncId);
            }
        });
        nodeContainers = _.sortBy(nodeContainers);
        if (nodeContainers.length < 2) {
            return (h("div", { class: "tree-node level-" + (scope.level) + activeClass + hoveredClass, style: { paddingLeft: paddingLeftString }, onClick: (event) => scope.nodeClickHandler(event) },
                h("div", { class: "header", onMouseEnter: (event) => scope.hoverNodeHandler(event), onMouseLeave: (event) => scope.unhoverNodeHandler(event) },
                    h("span", { class: "icon " + iconClass }),
                    h("span", { class: "name" }, componentMeta["label"])),
                h("div", null, childNodes.map(function (childNode) {
                    return (h("wv-pb-tree-node", { key: childNode["id"], node: childNode, level: scope.level + 1 }));
                }))));
        }
        else {
            return (h("div", { class: "tree-node level-" + (scope.level) + activeClass + hoveredClass, style: { paddingLeft: paddingLeftString }, onClick: (event) => scope.nodeClickHandler(event) },
                h("div", { class: "header", onMouseEnter: (event) => scope.hoverNodeHandler(event), onMouseLeave: (event) => scope.unhoverNodeHandler(event) },
                    h("span", { class: "icon " + iconClass }),
                    h("span", { class: "name" }, componentMeta["label"])),
                nodeContainers.map(function (container) {
                    return (h("div", { key: container["id"] },
                        h("div", { style: { paddingLeft: "15px" }, class: "go-teal" }, container),
                        childNodes.map(function (childNode) {
                            if (childNode["container_id"] === container) {
                                return (h("wv-pb-tree-node", { key: childNode["id"], node: childNode, level: scope.level + 1 }));
                            }
                            else {
                                return null;
                            }
                        })));
                })));
        }
    }
    static get is() { return "wv-pb-tree-node"; }
    static get properties() { return {
        "activeNodeId": {
            "state": true
        },
        "hoveredNodeId": {
            "state": true
        },
        "level": {
            "type": Number,
            "attr": "level"
        },
        "node": {
            "type": "Any",
            "attr": "node"
        },
        "pageNodeChangeIndex": {
            "state": true
        },
        "store": {
            "context": "store"
        }
    }; }
}
