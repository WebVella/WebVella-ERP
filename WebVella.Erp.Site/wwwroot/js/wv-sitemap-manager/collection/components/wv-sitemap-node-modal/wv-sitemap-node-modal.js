import axios from 'axios';
export class WvSitemapNodeModal {
    constructor() {
        this.nodeObj = { areaId: null, node: null };
        this.nodePageDict = null;
        this.submitResponse = { message: "", errors: [], success: true };
        this.modalNodeObj = { areaId: null, node: {}, node_pages: [] };
    }
    componentWillLoad() {
        var backdropId = "wv-sitemap-manager-area-modal-backdrop";
        var backdropDomEl = document.getElementById(backdropId);
        if (!backdropDomEl) {
            var backdropEl = document.createElement('div');
            backdropEl.className = "modal-backdrop show";
            backdropEl.id = backdropId;
            document.body.appendChild(backdropEl);
        }
        if (this.nodeAuxData == null) {
            this.LoadData();
        }
        if (this.nodeObj["node"]) {
            this.modalNodeObj["node"] = Object.assign({}, this.nodeObj["node"]);
            if (!this.modalNodeObj["node"]["pages"]) {
                this.modalNodeObj["node"]["pages"] = [];
            }
        }
        else {
            this.modalNodeObj["node"] = { pages: [] };
        }
        this.modalNodeObj["areaId"] = this.nodeObj["areaId"];
        if (this.nodeObj["node"] && this.nodePageDict && this.nodePageDict[this.nodeObj["node"]["id"]]) {
            this.modalNodeObj["node_pages"] = this.nodePageDict[this.nodeObj["node"]["id"]];
            this.modalNodeObj["node_pages"].forEach(element => {
                this.modalNodeObj["node"]["pages"].push(element["value"]);
            });
        }
    }
    componentDidUnload() {
        var backdropId = "wv-sitemap-manager-area-modal-backdrop";
        var backdropDomEl = document.getElementById(backdropId);
        if (backdropDomEl) {
            backdropDomEl.remove();
        }
    }
    LoadData() {
        let apiUrl = this.apiRoot + "sitemap/node/get-aux-info" + "?appId=" + this.appId;
        let thisEl = this;
        axios.get(apiUrl, {
            method: 'GET',
            headers: new Headers({
                'Content-Type': 'application/json',
                'Accept-Encoding': 'gzip',
                'Accept': 'application/json',
            })
        })
            .then(function (response) {
            let responseData = response.data;
            if (response.status !== 200 || responseData == null || !responseData["success"]) {
                if (responseData != null) {
                    alert(responseData["message"]);
                }
                else {
                    alert("Error: " + response.status + " - " + response.statusText);
                }
                return;
            }
            var dataAuxObj = {};
            dataAuxObj["allEntities"] = responseData["object"]["all_entities"];
            dataAuxObj["nodeTypes"] = responseData["object"]["node_types"];
            dataAuxObj["allPages"] = responseData["object"]["all_pages"];
            dataAuxObj["nodePageDict"] = responseData["object"]["node_page_dict"];
            dataAuxObj["selectedNodeObj"] = thisEl.nodeObj;
            thisEl.wvSitemapManagerNodeAuxDataUpdateEvent.emit(dataAuxObj);
        })
            .catch(function (err) {
            alert(err.message);
        });
    }
    closeModal() {
        this.wvSitemapManagerNodeModalCloseEvent.emit();
    }
    handleSubmit(e) {
        e.preventDefault();
        this.wvSitemapManagerNodeSubmittedEvent.emit(this.modalNodeObj);
    }
    handleChange(event) {
        let propertyName = event.target.getAttribute('name');
        this.modalNodeObj["node"][propertyName] = event.target.value;
    }
    handleCheckboxChange(event) {
        let propertyName = event.target.getAttribute('name');
        let isChecked = event.target.checked;
        this.modalNodeObj["node"][propertyName] = isChecked;
    }
    handleSelectChange(event) {
        let propertyName = event.target.getAttribute('name');
        let newObj = Object.assign({}, this.modalNodeObj);
        newObj["node"][propertyName] = [];
        for (var i = 0; i < event.target.options.length; i++) {
            var option = event.target.options[i];
            if (option.selected) {
                newObj["node"][propertyName].push(String(option.value));
            }
        }
        if (!newObj["node"][propertyName] || newObj["node"][propertyName].length === 0) {
            newObj["node"][propertyName] = null;
        }
        else if (newObj["node"][propertyName].length == 1 && propertyName != "pages") {
            newObj["node"][propertyName] = newObj["node"][propertyName][0];
        }
        this.modalNodeObj = newObj;
    }
    render() {
        let modalTitle = "Manage node";
        if (!this.nodeObj["node"]) {
            modalTitle = "Create node";
        }
        if (this.nodeAuxData == null) {
            return (h("div", { class: "modal d-block" },
                h("div", { class: "modal-dialog modal-xl" },
                    h("div", { class: "modal-content" },
                        h("div", { class: "modal-header" },
                            h("h5", { class: "modal-title" }, modalTitle)),
                        h("div", { class: "modal-body", style: { minHeight: "300px" } },
                            h("i", { class: "fas fa-spinner fa-spin go-blue" }),
                            " Loading data ...")))));
        }
        if (!this.modalNodeObj["node"]["type"]) {
            this.modalNodeObj["node"]["type"] = String(this.nodeAuxData["nodeTypes"][0]["value"]);
        }
        if (!this.modalNodeObj["node"]["entity_id"]) {
            this.modalNodeObj["node"]["entity_id"] = String(this.nodeAuxData["allEntities"][0]["value"]);
        }
        var allPagesPlusNode = [];
        var addedPages = [];
        this.modalNodeObj["node_pages"].forEach(element => {
            allPagesPlusNode.push(element);
            addedPages.push(element["value"]);
        });
        this.nodeAuxData["allPages"].forEach(element => {
            if (addedPages.length == 0 || (addedPages.length > 0 && addedPages.indexOf(element["page_id"]) === -1)) {
                if (!element["node_id"] || element["node_id"] === this.modalNodeObj["node"]["id"]) {
                    var selectOption = {
                        value: element["page_id"],
                        label: element["page_name"]
                    };
                    allPagesPlusNode.push(selectOption);
                }
            }
        });
        return (h("div", { class: "modal d-block" },
            h("div", { class: "modal-dialog modal-lg" },
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
                                        h("input", { class: "form-control", name: "name", value: this.modalNodeObj["node"]["name"], onInput: (event) => this.handleChange(event) }))),
                                h("div", { class: "col col-sm-6" },
                                    h("div", { class: "form-group erp-field" },
                                        h("label", { class: "control-label" }, "Label"),
                                        h("input", { class: "form-control", name: "label", value: this.modalNodeObj["node"]["label"], onInput: (event) => this.handleChange(event) })))),
                            h("div", { class: "row" },
                                h("div", { class: "col col-sm-6" },
                                    h("div", { class: "form-group erp-field" },
                                        h("label", { class: "control-label" }, "Icon Class"),
                                        h("input", { class: "form-control", name: "icon_class", value: this.modalNodeObj["node"]["icon_class"], onInput: (event) => this.handleChange(event) }))),
                                h("div", { class: "col col-sm-6" },
                                    h("div", { class: "form-group erp-field" },
                                        h("label", { class: "control-label" }, "Weight"),
                                        h("input", { type: "number", step: 1, min: 1, class: "form-control", name: "weight", value: this.modalNodeObj["node"]["weight"], onInput: (event) => this.handleChange(event) })))),
                            h("div", { class: "row" },
                                h("div", { class: "col col-sm-6" },
                                    h("div", { class: "form-group erp-field" },
                                        h("label", { class: "control-label" }, "Type"),
                                        h("select", { class: "form-control", name: "type", onChange: (event) => this.handleSelectChange(event) }, this.nodeAuxData["nodeTypes"].map(function (type) {
                                            return (h("option", { value: type["value"], selected: type.value === String(this.modalNodeObj["node"]["type"]) }, type["label"]));
                                        }.bind(this))))),
                                String(this.modalNodeObj["node"]["type"]) === "1"
                                    ? (h("div", { class: "col col-sm-6" },
                                        h("div", { class: "form-group erp-field" },
                                            h("label", { class: "control-label" }, "Entity"),
                                            h("select", { class: "form-control", name: "entity_id", onChange: (event) => this.handleSelectChange(event) }, this.nodeAuxData["allEntities"].map(function (type) {
                                                return (h("option", { value: type["value"], selected: type.value === String(this.modalNodeObj["node"]["entity_id"]) }, type["label"]));
                                            }.bind(this))))))
                                    : null,
                                String(this.modalNodeObj["node"]["type"]) === "2"
                                    ? (h("div", { class: "col col-sm-6" },
                                        h("div", { class: "form-group erp-field" },
                                            h("label", { class: "control-label" }, "App Pages without nodes"),
                                            h("select", { class: "form-control", multiple: true, name: "pages", onChange: (event) => this.handleSelectChange(event) }, allPagesPlusNode.map(function (type) {
                                                let nodeSelected = false;
                                                if (this.modalNodeObj["node"]["pages"] && this.modalNodeObj["node"]["pages"].length > 0 && this.modalNodeObj["node"]["pages"].indexOf(type.value) > -1) {
                                                    nodeSelected = true;
                                                }
                                                return (h("option", { value: type["value"], selected: nodeSelected }, type["label"]));
                                            }.bind(this))))))
                                    : null,
                                String(this.modalNodeObj["node"]["type"]) === "3"
                                    ? (h("div", { class: "col col-sm-6" },
                                        h("div", { class: "form-group erp-field" },
                                            h("label", { class: "control-label" }, "Url"),
                                            h("input", { class: "form-control", name: "url", value: this.modalNodeObj["node"]["url"], onInput: (event) => this.handleChange(event) }))))
                                    : null),
                            h("div", { class: "alert alert-info" }, "Label and Description translations, and access are currently not managable")),
                        this.nodeAuxData == null
                            ? (null)
                            : (h("div", { class: "modal-footer" },
                                h("div", null,
                                    h("button", { type: "submit", class: "btn btn-green btn-sm " + (this.modalNodeObj["node"] == null ? "" : "d-none") },
                                        h("span", { class: "ti-plus" }),
                                        " Create node"),
                                    h("button", { type: "submit", class: "btn btn-blue btn-sm " + (this.modalNodeObj["node"] != null ? "" : "d-none") },
                                        h("span", { class: "ti-save" }),
                                        " Save node"),
                                    h("button", { type: "button", class: "btn btn-white btn-sm ml-1", onClick: () => this.closeModal() }, "Close")))))))));
    }
    static get is() { return "wv-sitemap-node-modal"; }
    static get properties() { return {
        "apiRoot": {
            "type": String,
            "attr": "api-root"
        },
        "appId": {
            "type": String,
            "attr": "app-id"
        },
        "modalNodeObj": {
            "state": true
        },
        "nodeAuxData": {
            "type": "Any",
            "attr": "node-aux-data"
        },
        "nodeObj": {
            "type": "Any",
            "attr": "node-obj"
        },
        "nodePageDict": {
            "type": "Any",
            "attr": "node-page-dict"
        },
        "submitResponse": {
            "type": "Any",
            "attr": "submit-response"
        }
    }; }
    static get events() { return [{
            "name": "wvSitemapManagerNodeModalCloseEvent",
            "method": "wvSitemapManagerNodeModalCloseEvent",
            "bubbles": true,
            "cancelable": true,
            "composed": true
        }, {
            "name": "wvSitemapManagerNodeSubmittedEvent",
            "method": "wvSitemapManagerNodeSubmittedEvent",
            "bubbles": true,
            "cancelable": true,
            "composed": true
        }, {
            "name": "wvSitemapManagerNodeAuxDataUpdateEvent",
            "method": "wvSitemapManagerNodeAuxDataUpdateEvent",
            "bubbles": true,
            "cancelable": true,
            "composed": true
        }]; }
}
