import * as action from '../../store/actions';
export class WvCreateModal {
    constructor() {
        this.isHelpModalVisible = false;
    }
    componentWillLoad() {
        this.store.mapStateToProps(this, (state) => {
            return {
                isHelpModalVisible: state.isHelpModalVisible,
            };
        });
        this.store.mapDispatchToProps(this, {
            setHelpModalState: action.setHelpModalState
        });
    }
    cancelHelpModalHandler(event) {
        event.preventDefault();
        event.stopPropagation();
        this.setHelpModalState(false);
    }
    render() {
        let scope = this;
        let showModal = scope.isHelpModalVisible;
        if (!showModal) {
            return null;
        }
        return (h("div", { class: "modal show d-block", style: { paddingRight: "17px" }, id: "modal-component-help" },
            h("div", { class: "modal-dialog modal-xl" },
                h("div", { class: "modal-content" },
                    h("div", { class: "modal-header" },
                        h("span", { class: "title" }, "Component help"),
                        h("span", { class: "aside" },
                            "wv-",
                            scope.store.getState().activeNodeId)),
                    h("div", { class: "modal-body", id: "modal-component-help-body" },
                        h("wv-show-help", { nodeId: scope.store.getState().activeNodeId })),
                    h("div", { class: "modal-footer" },
                        h("button", { type: "button", class: "btn btn-white btn-sm", onClick: (e) => scope.cancelHelpModalHandler(e) }, "Close"))))));
    }
    static get is() { return "wv-help-modal"; }
    static get properties() { return {
        "isHelpModalVisible": {
            "state": true
        },
        "store": {
            "context": "store"
        }
    }; }
}
