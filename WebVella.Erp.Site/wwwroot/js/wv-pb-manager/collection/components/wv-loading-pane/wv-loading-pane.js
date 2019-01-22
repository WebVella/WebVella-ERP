export class WvLoadingPane {
    render() {
        return (h("div", { class: "p-2" },
            h("i", { class: "fa fa-spin fa-spinner go-blue" }),
            " Loading..."));
    }
    static get is() { return "wv-loading-pane"; }
}
