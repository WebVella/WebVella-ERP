export class WvCreateModal {
    constructor() {
        this.nodeId = "";
        this.isOptionsModalVisible = false;
    }
    componentWillLoad() {
        this.store.mapStateToProps(this, (state) => {
            return {
                isOptionsModalVisible: state.isOptionsModalVisible,
            };
        });
        let nodeOptionsTemplate = document.getElementById("node-options-" + this.nodeId);
        let OptionsModalPlaceholder = document.getElementById("modal-component-options-body");
        if (nodeOptionsTemplate) {
            OptionsModalPlaceholder.appendChild(nodeOptionsTemplate);
        }
    }
    optionsModalVisibilityHandler(newValue, oldValue) {
        if (!newValue && oldValue) {
            let nodeOptionsStack = document.getElementById("wv-node-options-stack");
            let nodeOptionsTemplate = document.getElementById("node-options-" + this.nodeId);
            if (nodeOptionsTemplate) {
                nodeOptionsStack.appendChild(nodeOptionsTemplate);
            }
        }
    }
    render() {
        return null;
    }
    static get is() { return "wv-show-options"; }
    static get properties() { return {
        "isOptionsModalVisible": {
            "state": true,
            "watchCallbacks": ["optionsModalVisibilityHandler"]
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
