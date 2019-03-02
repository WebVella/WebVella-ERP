export class WvSitemapManagerArea {
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
        var areaIconClass = "far fa-question-circle";
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
                        h("span", { class: "fa fa-trash-alt go-red" }),
                        " delete"),
                    h("button", { type: "button", class: "btn btn-link", onClick: () => this.manageArea() },
                        h("span", { class: "fa fa-cog" }),
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
                                        h("span", { class: "fa fa-trash-alt go-red" }),
                                        " delete"),
                                    h("button", { type: "button", class: "btn btn-link", onClick: () => areaCmpt.manageNode(node) },
                                        h("span", { class: "fa fa-cog" }),
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
