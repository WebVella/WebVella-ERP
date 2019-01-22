import { h } from '../wv-sitemap-manager.core.js';

class WvSitemapAreaModal {
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
                                h("span", { class: "ti-plus" }),
                                " Create area"),
                            h("button", { type: "submit", class: "btn btn-blue btn-sm " + (this.area != null ? "" : "d-none") },
                                h("span", { class: "ti-save" }),
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

class WvSitemapManager {
    constructor() {
        this.sitemapObj = null;
        this.nodePageDict = null;
        this.isAreaModalVisible = false;
        this.managedArea = null;
        this.isNodeModalVisible = false;
        this.managedNodeObj = { areaId: null, node: null };
        this.apiResponse = { message: "", errors: [], success: true };
        this.nodeAuxData = null;
    }
    componentWillLoad() {
        if (this.initData) {
            var initDataObj = JSON.parse(this.initData);
            this.sitemapObj = initDataObj["sitemap"];
            this.nodePageDict = initDataObj["node_page_dict"];
        }
    }
    createArea() {
        this.isAreaModalVisible = true;
        this.managedArea = null;
    }
    areaManageEventHandler(event) {
        this.isAreaModalVisible = true;
        this.managedArea = Object.assign({}, event.detail);
    }
    areaModalClose() {
        this.isAreaModalVisible = false;
        this.managedArea = null;
        this.apiResponse = { message: "", errors: [], success: true };
    }
    areaSubmittedEventHandler(event) {
        this.apiResponse = { message: "", errors: [], success: true };
        let submittedArea = event.detail;
        let apiUrl = this.apiRoot + "sitemap/area";
        if (submittedArea != null && submittedArea["id"]) {
            apiUrl += "/" + submittedArea["id"];
        }
        apiUrl += "?appId=" + this.appId;
        let thisEl = this;
        fetch(apiUrl, {
            method: 'POST',
            headers: new Headers({
                'Content-Type': 'application/json',
                'Accept-Encoding': 'gzip',
                'Accept': 'application/json',
            }),
            body: JSON.stringify(submittedArea)
        })
            .then(function (response) {
            response.json().then(function (data) {
                let responseData = data;
                if (response.status !== 200 || responseData == null || !responseData["success"]) {
                    thisEl.apiResponse = Object.assign({}, responseData);
                    thisEl.managedArea = Object.assign({}, submittedArea);
                    return;
                }
                thisEl.sitemapObj = Object.assign({}, responseData.object["sitemap"]);
                thisEl.nodePageDict = Object.assign({}, responseData.object["node_page_dict"]);
                thisEl.areaModalClose();
            });
        })
            .catch(function (err) {
            var responseError = {
                success: false,
                message: err
            };
            thisEl.apiResponse = Object.assign({}, responseError);
            thisEl.managedArea = Object.assign({}, submittedArea);
        });
    }
    areaDeleteEventHandler(event) {
        this.apiResponse = { message: "", errors: [], success: true };
        let areaId = event.detail;
        let apiUrl = this.apiRoot + "sitemap/area/" + areaId + "/delete" + "?appId=" + this.appId;
        let thisEl = this;
        fetch(apiUrl, {
            method: 'POST',
            headers: new Headers({
                'Content-Type': 'application/json',
                'Accept-Encoding': 'gzip',
                'Accept': 'application/json',
            })
        })
            .then(function (response) {
            response.json().then(function (data) {
                let responseData = data;
                if (response.status !== 200 || responseData == null || !responseData["success"]) {
                    alert(responseData.message);
                    return;
                }
                thisEl.sitemapObj = Object.assign({}, responseData.object["sitemap"]);
                thisEl.nodePageDict = Object.assign({}, responseData.object["node_page_dict"]);
                thisEl.areaModalClose();
            });
        })
            .catch(function (err) {
            alert(err.message);
        });
    }
    nodeManageEventHandler(event) {
        this.isNodeModalVisible = true;
        this.managedNodeObj = Object.assign({}, event.detail);
    }
    nodeModalCloseEventHandler() {
        this.isNodeModalVisible = false;
        this.managedNodeObj = { areaId: null, node: null };
        this.apiResponse = { message: "", errors: [], success: true };
    }
    nodeSubmittedEventHandler(event) {
        this.apiResponse = { message: "", errors: [], success: true };
        let submittedNode = event.detail.node;
        let areaId = event.detail.areaId;
        let apiUrl = this.apiRoot + "sitemap/node";
        if (submittedNode != null && submittedNode["id"] != null) {
            apiUrl += "/" + submittedNode["id"];
        }
        apiUrl += "?appId=" + this.appId + "&areaId=" + areaId;
        let thisEl = this;
        fetch(apiUrl, {
            method: 'POST',
            headers: new Headers({
                'Content-Type': 'application/json',
                'Accept-Encoding': 'gzip',
                'Accept': 'application/json',
            }),
            body: JSON.stringify(submittedNode)
        })
            .then(function (response) {
            response.json().then(function (data) {
                let responseData = data;
                if (response.status !== 200 || responseData == null || !responseData["success"]) {
                    thisEl.apiResponse = Object.assign({}, responseData);
                    thisEl.managedNodeObj = Object.assign({}, event.detail);
                    return;
                }
                thisEl.sitemapObj = Object.assign({}, responseData.object["sitemap"]);
                thisEl.nodePageDict = Object.assign({}, responseData.object["node_page_dict"]);
                thisEl.nodeModalCloseEventHandler();
                thisEl.nodeAuxDataUpdateEventHandler(null);
            });
        })
            .catch(function (err) {
            var responseError = {
                success: false,
                message: err
            };
            thisEl.apiResponse = Object.assign({}, responseError);
            thisEl.managedNodeObj = Object.assign({}, event.detail);
        });
    }
    nodeDeleteEventHandler(event) {
        this.apiResponse = { message: "", errors: [], success: true };
        let nodeId = event.detail;
        let apiUrl = this.apiRoot + "sitemap/node/" + nodeId + "/delete" + "?appId=" + this.appId;
        let thisEl = this;
        fetch(apiUrl, {
            method: 'POST',
            headers: new Headers({
                'Content-Type': 'application/json',
                'Accept-Encoding': 'gzip',
                'Accept': 'application/json',
            })
        })
            .then(function (response) {
            response.json().then(function (data) {
                let responseData = data;
                if (response.status !== 200 || responseData == null || !responseData["success"]) {
                    alert(responseData.message);
                    return;
                }
                thisEl.sitemapObj = Object.assign({}, responseData.object["sitemap"]);
                thisEl.nodePageDict = Object.assign({}, responseData.object["node_page_dict"]);
                thisEl.nodeModalCloseEventHandler();
            });
        })
            .catch(function (err) {
            alert(err.message);
        });
    }
    nodeAuxDataUpdateEventHandler(event) {
        if (event != null) {
            var newNodeAuxData = {
                allEntities: event.detail.allEntities,
                nodeTypes: event.detail.nodeTypes,
                allPages: event.detail.allPages,
                nodePageDict: event.detail.nodePageDict
            };
            this.nodeAuxData = Object.assign({}, newNodeAuxData);
            this.managedNodeObj = Object.assign({}, event.detail["selectedNodeObj"]);
        }
        else {
            this.nodeAuxData = null;
            this.managedNodeObj = null;
        }
    }
    render() {
        return (h("div", { id: "sitemap-manager" },
            h("div", { class: "btn-group btn-group-sm mb-2" },
                h("button", { type: "button", class: "btn btn-white", onClick: () => this.createArea() },
                    h("span", { class: "fa fa-plus go-green" }),
                    " add area")),
            this.sitemapObj["areas"].map((area) => (h("wv-sitemap-manager-area", { area: area }))),
            this.isAreaModalVisible ? (h("wv-sitemap-area-modal", { submitResponse: this.apiResponse, area: this.managedArea })) : null,
            this.isNodeModalVisible ? (h("wv-sitemap-node-modal", { nodePageDict: this.nodePageDict, nodeAuxData: this.nodeAuxData, appId: this.appId, submitResponse: this.apiResponse, nodeObj: this.managedNodeObj, apiRoot: this.apiRoot })) : null));
    }
    static get is() { return "wv-sitemap-manager"; }
    static get properties() { return {
        "apiResponse": {
            "state": true
        },
        "apiRoot": {
            "type": String,
            "attr": "api-root"
        },
        "appId": {
            "type": String,
            "attr": "app-id"
        },
        "initData": {
            "type": String,
            "attr": "init-data"
        },
        "isAreaModalVisible": {
            "state": true
        },
        "isNodeModalVisible": {
            "state": true
        },
        "managedArea": {
            "state": true
        },
        "managedNodeObj": {
            "state": true
        },
        "nodeAuxData": {
            "state": true
        },
        "nodePageDict": {
            "state": true
        },
        "sitemapObj": {
            "state": true
        }
    }; }
    static get listeners() { return [{
            "name": "wvSitemapManagerAreaManageEvent",
            "method": "areaManageEventHandler"
        }, {
            "name": "wvSitemapManagerAreaModalCloseEvent",
            "method": "areaModalClose"
        }, {
            "name": "wvSitemapManagerAreaSubmittedEvent",
            "method": "areaSubmittedEventHandler"
        }, {
            "name": "wvSitemapManagerAreaDeleteEvent",
            "method": "areaDeleteEventHandler"
        }, {
            "name": "wvSitemapManagerNodeManageEvent",
            "method": "nodeManageEventHandler"
        }, {
            "name": "wvSitemapManagerNodeModalCloseEvent",
            "method": "nodeModalCloseEventHandler"
        }, {
            "name": "wvSitemapManagerNodeSubmittedEvent",
            "method": "nodeSubmittedEventHandler"
        }, {
            "name": "wvSitemapManagerNodeDeleteEvent",
            "method": "nodeDeleteEventHandler"
        }, {
            "name": "wvSitemapManagerNodeAuxDataUpdateEvent",
            "method": "nodeAuxDataUpdateEventHandler"
        }]; }
}

class WvSitemapManagerArea {
    manageArea() {
        this.wvSitemapManagerAreaManageEvent.emit(this.area);
    }
    deleteArea(event) {
        if (confirm("Are you sure?")) {
            this.wvSitemapManagerAreaDeleteEvent.emit(this.area["id"]);
        }
        else {
            event.preventDefault();
        }
    }
    createNode() {
        var submitObj = {
            node: null,
            areaId: this.area["id"]
        };
        this.wvSitemapManagerNodeManageEvent.emit(submitObj);
    }
    manageNode(node) {
        var submitObj = {
            node: node,
            areaId: this.area["id"]
        };
        this.wvSitemapManagerNodeManageEvent.emit(submitObj);
    }
    deleteNode(event, node) {
        if (confirm("Are you sure?")) {
            this.wvSitemapManagerNodeDeleteEvent.emit(node["id"]);
        }
        else {
            event.preventDefault();
        }
    }
    render() {
        var areaCmpt = this;
        var areaColor = "#999";
        if (this.area["color"]) {
            areaColor = this.area["color"];
        }
        var areaIconClass = "ti-help";
        if (this.area["icon_class"]) {
            areaIconClass = this.area["icon_class"];
        }
        return (h("div", { class: "sitemap-area mb-3" },
            h("div", { class: "area-header" },
                h("span", { class: "icon " + areaIconClass, style: { backgroundColor: areaColor } }),
                h("div", { class: "label" },
                    "(",
                    this.area["weight"],
                    ") ",
                    this.area["label"]),
                h("div", { class: "btn-group btn-group-sm action" },
                    h("button", { type: "button", class: "btn btn-link", onClick: (e) => this.deleteArea(e) },
                        h("span", { class: "ti-trash go-red" }),
                        " delete"),
                    h("button", { type: "button", class: "btn btn-link", onClick: () => this.manageArea() },
                        h("span", { class: "ti-settings" }),
                        " config"))),
            h("div", { class: "area-body " + (this.area["nodes"].length > 0 ? "" : "d-none") },
                h("button", { type: "button", class: "btn btn-white btn-sm", onClick: () => this.createNode() },
                    h("span", { class: "fa fa-plus" }),
                    " add area node"),
                h("table", { class: "table table-bordered table-sm mb-0 sitemap-nodes mt-3" },
                    h("thead", null,
                        h("tr", null,
                            h("th", { style: { width: "40px" } }, "w."),
                            h("th", { style: { width: "40px" } }, "icon"),
                            h("th", { style: { width: "200px" } }, "name"),
                            h("th", { style: { width: "auto" } }, "label"),
                            h("th", { style: { width: "200px" } }, "group"),
                            h("th", { style: { width: "100px" } }, "type"),
                            h("th", { style: { width: "160px" } }, "action"))),
                    h("tbody", null, this.area["nodes"].map(function (node) {
                        var typeString = "";
                        switch (node["type"]) {
                            case 1:
                                typeString = "entity list";
                                break;
                            case 2:
                                typeString = "application";
                                break;
                            case 3:
                                typeString = "url";
                                break;
                            case 4:
                                typeString = "site";
                                break;
                            default:
                                break;
                        }
                        return (h("tr", null,
                            h("td", null, node["weight"]),
                            h("td", null,
                                h("span", { class: "icon " + node["icon_class"] })),
                            h("td", null, node["name"]),
                            h("td", null, node["label"]),
                            h("td", null, node["group_name"]),
                            h("td", null, typeString),
                            h("td", null,
                                h("div", { class: "btn-group btn-group-sm action" },
                                    h("button", { type: "button", class: "btn btn-link", onClick: (e) => areaCmpt.deleteNode(e, node) },
                                        h("span", { class: "ti-trash go-red" }),
                                        " delete"),
                                    h("button", { type: "button", class: "btn btn-link", onClick: () => areaCmpt.manageNode(node) },
                                        h("span", { class: "ti-settings" }),
                                        " config")))));
                    })))),
            h("div", { class: "area-body " + (this.area["nodes"].length > 0 ? "d-none" : "") },
                h("button", { type: "button", class: "btn btn-white btn-sm", onClick: () => this.createNode() },
                    h("span", { class: "fa fa-plus" }),
                    " add area node"),
                h("div", { class: "alert alert-info mt-3" }, "No nodes in this area."))));
    }
    static get is() { return "wv-sitemap-manager-area"; }
    static get properties() { return {
        "area": {
            "type": "Any",
            "attr": "area"
        }
    }; }
    static get events() { return [{
            "name": "wvSitemapManagerAreaManageEvent",
            "method": "wvSitemapManagerAreaManageEvent",
            "bubbles": true,
            "cancelable": true,
            "composed": true
        }, {
            "name": "wvSitemapManagerAreaDeleteEvent",
            "method": "wvSitemapManagerAreaDeleteEvent",
            "bubbles": true,
            "cancelable": true,
            "composed": true
        }, {
            "name": "wvSitemapManagerNodeManageEvent",
            "method": "wvSitemapManagerNodeManageEvent",
            "bubbles": true,
            "cancelable": true,
            "composed": true
        }, {
            "name": "wvSitemapManagerNodeDeleteEvent",
            "method": "wvSitemapManagerNodeDeleteEvent",
            "bubbles": true,
            "cancelable": true,
            "composed": true
        }]; }
}

class WvSitemapNodeModal {
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
        fetch(apiUrl, {
            method: 'GET',
            headers: new Headers({
                'Content-Type': 'application/json',
                'Accept-Encoding': 'gzip',
                'Accept': 'application/json',
            })
        })
            .then(function (response) {
            response.json().then(function (data) {
                let responseData = data;
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
            });
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
                h("div", { class: "modal-dialog modal-lg" },
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

export { WvSitemapAreaModal, WvSitemapManager, WvSitemapManagerArea, WvSitemapNodeModal };
