export class WvCreateModal {
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
