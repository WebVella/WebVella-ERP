export class WvSitemapAreaModal {
    constructor() {
        this.area = null;
        this.submitResponse = { message: "", errors: [] };
        this.modalArea = null;
    }
    componentWillLoad() {
        var backdropId = "wv-sitemap-manager-area-modal-backdrop";
        var backdropDomEl = document.getElementById(backdropId);
        if (!backdropDomEl) {
            var backdropEl = document.createElement('div');
            backdropEl.className = "modal-backdrop show";
            backdropEl.id = backdropId;
            document.body.appendChild(backdropEl);
            this.modalArea = Object.assign({}, this.area);
            delete this.modalArea["nodes"];
        }
    }
    componentDidUnload() {
        var backdropId = "wv-sitemap-manager-area-modal-backdrop";
        var backdropDomEl = document.getElementById(backdropId);
        if (backdropDomEl) {
            backdropDomEl.remove();
        }
    }
    closeModal() {
        this.wvSitemapManagerAreaModalCloseEvent.emit();
    }
    handleSubmit(e) {
        e.preventDefault();
        this.wvSitemapManagerAreaSubmittedEvent.emit(this.modalArea);
    }
    handleChange(event) {
        let propertyName = event.target.getAttribute('name');
        this.modalArea[propertyName] = event.target.value;
    }
    handleCheckboxChange(event) {
        let propertyName = event.target.getAttribute('name');
        let isChecked = event.target.checked;
        this.modalArea[propertyName] = isChecked;
    }
    render() {
        let modalTitle = "Manage area";
        if (!this.area) {
            modalTitle = "Create area";
        }
        return (h("div", { class: "modal d-block" },
            h("div", { class: "modal-dialog modal-xl" },
                h("div", { class: "modal-content" },
                    h("form", { onSubmit: (e) => this.handleSubmit(e) },
                        h("div", { class: "modal-header" },
                            h("h5", { class: "modal-title" }, modalTitle)),
                        h("div", { class: "modal-body" },
                            h("div", { class: "alert alert-danger " + (this.submitResponse["success"] ? "d-none" : "") }, this.submitResponse["message"]),
                            h("div", { class: "row" },
                                h("div", { class: "col col-sm-6" },
                                    h("div", { class: "form-group erp-field" },
                                        h("label", { class: "control-label" }, "Name"),
                                        h("input", { class: "form-control", name: "name", value: this.modalArea["name"], onInput: (event) => this.handleChange(event) }))),
                                h("div", { class: "col col-sm-6" },
                                    h("div", { class: "form-group erp-field" },
                                        h("label", { class: "control-label" }, "Label"),
                                        h("input", { class: "form-control", name: "label", value: this.modalArea["label"], onInput: (event) => this.handleChange(event) })))),
                            h("div", { class: "row" },
                                h("div", { class: "col col-sm-12" },
                                    h("div", { class: "form-group erp-field" },
                                        h("label", { class: "control-label" }, "Description"),
                                        h("textarea", { class: "form-control", style: { height: "60px" }, name: "description", onInput: (event) => this.handleChange(event) }, this.modalArea["description"])))),
                            h("div", { class: "row" },
                                h("div", { class: "col col-sm-6" },
                                    h("div", { class: "form-group erp-field" },
                                        h("label", { class: "control-label" }, "Weight"),
                                        h("input", { type: "number", step: 1, min: 1, class: "form-control", name: "weight", value: this.modalArea["weight"], onInput: (event) => this.handleChange(event) }))),
                                h("div", { class: "col col-sm-6" },
                                    h("div", { class: "form-group erp-field" },
                                        h("label", { class: "control-label" }, "Group names"),
                                        h("div", { class: "form-control-plaintext" },
                                            h("div", { class: "form-check" },
                                                h("label", { class: "form-check-label" },
                                                    h("input", { class: "form-check-input", type: "checkbox", name: "show_group_names", value: "true", checked: this.modalArea["show_group_names"], onChange: (event) => this.handleCheckboxChange(event) }),
                                                    " group names are visible")))))),
                            h("div", { class: "row" },
                                h("div", { class: "col col-sm-6" },
                                    h("div", { class: "form-group erp-field" },
                                        h("label", { class: "control-label" }, "Color"),
                                        h("input", { class: "form-control", name: "color", value: this.modalArea["color"], onInput: (event) => this.handleChange(event) }))),
                                h("div", { class: "col col-sm-6" },
                                    h("div", { class: "form-group erp-field" },
                                        h("label", { class: "control-label" }, "Icon Class"),
                                        h("input", { class: "form-control", name: "icon_class", value: this.modalArea["icon_class"], onInput: (event) => this.handleChange(event) })))),
                            h("div", { class: "alert alert-info" }, "Label and Description translations, and access are currently not managable")),
                        h("div", { class: "modal-footer" },
                            h("button", { type: "submit", class: "btn btn-green btn-sm " + (this.area == null ? "" : "d-none") },
                                h("span", { class: "fa fa-plus" }),
                                " Create area"),
                            h("button", { type: "submit", class: "btn btn-blue btn-sm " + (this.area != null ? "" : "d-none") },
                                h("span", { class: "far fa-save" }),
                                " Save area"),
                            h("button", { type: "button", class: "btn btn-white btn-sm ml-1", onClick: () => this.closeModal() }, "Close")))))));
    }
    static get is() { return "wv-sitemap-area-modal"; }
    static get properties() { return {
        "area": {
            "type": "Any",
            "attr": "area"
        },
        "modalArea": {
            "state": true
        },
        "submitResponse": {
            "type": "Any",
            "attr": "submit-response"
        }
    }; }
    static get events() { return [{
            "name": "wvSitemapManagerAreaModalCloseEvent",
            "method": "wvSitemapManagerAreaModalCloseEvent",
            "bubbles": true,
            "cancelable": true,
            "composed": true
        }, {
            "name": "wvSitemapManagerAreaSubmittedEvent",
            "method": "wvSitemapManagerAreaSubmittedEvent",
            "bubbles": true,
            "cancelable": true,
            "composed": true
        }]; }
}
