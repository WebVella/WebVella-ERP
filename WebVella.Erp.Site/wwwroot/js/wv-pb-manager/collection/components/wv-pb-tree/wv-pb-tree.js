import _ from 'lodash';
function GetRootNodes(pageNodes) {
    let rootNodes = new Array();
    _.forEach(pageNodes, function (node) {
        if (!node["parent_id"]) {
            rootNodes.push(node);
        }
    });
    return _.sortBy(rootNodes, ['weight']);
}
export class WvPbNodeContainer {
    constructor() {
        this.pageNodeChangeIndex = 1;
    }
    componentWillLoad() {
        let scope = this;
        scope.store.mapStateToProps(this, (state) => {
            return {
                pageNodeChangeIndex: state.pageNodeChangeIndex
            };
        });
    }
    render() {
        let rootNodes = GetRootNodes(this.store.getState().pageNodes);
        return ([
            h("div", { class: "header" },
                h("div", { class: "title" }, "Page Body")),
            h("div", { class: "body" }, rootNodes.map(function (node) {
                return (h("wv-pb-tree-node", { key: node["id"], level: 0, node: node }));
            }))
        ]);
    }
    static get is() { return "wv-pb-tree"; }
    static get properties() { return {
        "pageNodeChangeIndex": {
            "state": true
        },
        "store": {
            "context": "store"
        }
    }; }
}
