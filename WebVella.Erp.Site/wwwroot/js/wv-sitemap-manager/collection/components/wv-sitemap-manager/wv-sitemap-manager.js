import axios from 'axios';
export class WvSitemapManager {
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
        let requestConfig = {
            headers: {
                'Content-Type': 'application/json;charset=UTF-8',
                'Accept': 'application/json'
            }
        };
        let thisEl = this;
        let requestBody = JSON.stringify(submittedArea);
        axios.post(apiUrl, requestBody, requestConfig)
            .then(function (response) {
            let responseData = response.data;
            if (response.status !== 200 || responseData == null || !responseData["success"]) {
                thisEl.apiResponse = Object.assign({}, responseData);
                thisEl.managedArea = Object.assign({}, submittedArea);
                return;
            }
            thisEl.sitemapObj = Object.assign({}, responseData.object["sitemap"]);
            thisEl.nodePageDict = Object.assign({}, responseData.object["node_page_dict"]);
            thisEl.areaModalClose();
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
        let requestConfig = {
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json',
            }
        };
        axios.post(apiUrl, null, requestConfig)
            .then(function (response) {
            let responseData = response.data;
            if (response.status !== 200 || responseData == null || !responseData["success"]) {
                alert(responseData.message);
                return;
            }
            thisEl.sitemapObj = Object.assign({}, responseData.object["sitemap"]);
            thisEl.nodePageDict = Object.assign({}, responseData.object["node_page_dict"]);
            thisEl.areaModalClose();
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
        let requestConfig = {
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json',
            }
        };
        let thisEl = this;
        axios.post(apiUrl, JSON.stringify(submittedNode), requestConfig)
            .then(function (response) {
            let responseData = response.data;
            if (response.status !== 200 || responseData == null || !responseData["success"]) {
                thisEl.apiResponse = Object.assign({}, responseData);
                thisEl.managedNodeObj = Object.assign({}, event.detail);
                return;
            }
            thisEl.sitemapObj = Object.assign({}, responseData.object["sitemap"]);
            thisEl.nodePageDict = Object.assign({}, responseData.object["node_page_dict"]);
            thisEl.nodeModalCloseEventHandler();
            thisEl.nodeAuxDataUpdateEventHandler(null);
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
        let requestConfig = {
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json',
            }
        };
        let thisEl = this;
        axios.post(apiUrl, null, requestConfig)
            .then(function (response) {
            let responseData = response.data;
            if (response.status !== 200 || responseData == null || !responseData["success"]) {
                alert(responseData.message);
                return;
            }
            thisEl.sitemapObj = Object.assign({}, responseData.object["sitemap"]);
            thisEl.nodePageDict = Object.assign({}, responseData.object["node_page_dict"]);
            thisEl.nodeModalCloseEventHandler();
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
                appPages: event.detail.appPages,
                allEntityPages: event.detail.allEntityPages,
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
