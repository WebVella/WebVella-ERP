import * as action from '../../store/actions';
import _ from 'lodash';
import axios from 'axios';
import WvPbEventPayload from "../../models/WvPbEventPayload";
function AddNewComponent(scope, component) {
    let requestBody = scope.store.getState().createdNode;
    requestBody["component_name"] = component["name"];
    requestBody["options"] = JSON.stringify(requestBody["options"]);
    let requestConfig = {
        headers: {
            'Content-Type': 'application/json;charset=UTF-8',
            "Access-Control-Allow-Origin": "*",
        }
    };
    let siteRoot = scope.store.getState().siteRootUrl;
    let requestUrl = siteRoot + "/api/v3.0/page/" + requestBody["page_id"] + "/node/create";
    let recordId = scope.store.getState().recordId;
    if (recordId) {
        requestUrl += "?recId=" + recordId;
    }
    axios.post(requestUrl, requestBody, requestConfig)
        .then(function (response) {
        scope.addNode(response.data);
        window.setTimeout(function () {
            scope.addReloadNodeIds(response.data["id"]);
            var customEvent = new Event("WvPbManager_Node_Added");
            var payload = new WvPbEventPayload();
            payload.original_event = event;
            payload.node = requestBody;
            payload.component_name = requestBody["component_name"];
            customEvent["payload"] = payload;
            document.dispatchEvent(customEvent);
        }, 10);
    })
        .catch(function (error) {
        if (error.response) {
            if (error.response.data) {
                alert(error.response.data);
            }
            else {
                alert(error.response.statusText);
            }
        }
        else if (error.message) {
            alert(error.message);
        }
        else {
            alert(error);
        }
    });
}
function RecalculateComponentList(scope) {
    let library = scope.store.getState().library;
    let filteredLibrary = [];
    if (scope.filterString) {
        filteredLibrary = _.filter(library, function (x) {
            let state = x["label"].toLowerCase().includes(scope.filterString.toLowerCase());
            if (!state) {
                state = x["library"].toLowerCase().includes(scope.filterString.toLowerCase());
            }
            if (!state) {
                state = x["description"].toLowerCase().includes(scope.filterString.toLowerCase());
            }
            return state;
        });
    }
    else {
        filteredLibrary = library;
    }
    if (!filteredLibrary) {
        filteredLibrary = [];
    }
    switch (scope.sort) {
        case "usage":
            filteredLibrary = _.orderBy(filteredLibrary, ['usage_counter'], ['desc']);
            break;
        case "alpha":
            filteredLibrary = _.orderBy(filteredLibrary, ['label'], ['asc']);
            break;
        default:
            filteredLibrary = _.orderBy(filteredLibrary, ['last_used_on'], ['desc']);
            break;
    }
    let startIndex = (scope.page - 1) * scope.pageSize;
    let endIndex = startIndex + scope.pageSize;
    let filterTotal = filteredLibrary.length;
    if (endIndex > filterTotal) {
        endIndex = filterTotal;
    }
    scope.total = filterTotal;
    scope.pageCount = scope.total / scope.pageSize;
    scope.componentList = _.slice(filteredLibrary, startIndex, endIndex);
}
export class WvCreateModal {
    constructor() {
        this.isCreateModalVisible = false;
        this.page = 1;
        this.pageSize = 24;
        this.sort = "recent";
        this.componentList = new Array();
        this.total = 0;
        this.pageCount = 0;
        this.focused = false;
    }
    componentWillLoad() {
        let scope = this;
        scope.store.mapStateToProps(scope, (state) => {
            return {
                isCreateModalVisible: state.isCreateModalVisible,
            };
        });
        scope.store.mapDispatchToProps(scope, {
            setNodeCreation: action.setNodeCreation,
            addNode: action.addNode,
            addReloadNodeIds: action.addReloadNodeIds
        });
        document.body.className = document.body.className.replace(" modal-open", "").replace("modal-open", "");
        document.body.style.paddingRight = null;
        let backdrop = document.getElementById("wv-pb-backdrop");
        if (backdrop) {
            backdrop.parentNode.removeChild(backdrop);
        }
    }
    cancelNodeCreateHandler(event) {
        event.preventDefault();
        event.stopPropagation();
        this.filterString = "";
        this.setNodeCreation(null);
    }
    filterChangeHandler(event) {
        event.preventDefault();
        event.stopPropagation();
        this.filterString = event.target.value;
        this.page = 1;
    }
    selectComponent(event, component) {
        event.preventDefault();
        event.stopPropagation();
        AddNewComponent(this, component);
        this.filterString = "";
    }
    changeSort(ev, sort) {
        ev.preventDefault();
        this.sort = sort;
        this.page = 1;
    }
    changePage(ev, page) {
        ev.preventDefault();
        if (page > 0) {
            this.page = page;
        }
    }
    render() {
        let scope = this;
        RecalculateComponentList(scope);
        let showModal = scope.isCreateModalVisible;
        if (!showModal) {
            scope.focused = false;
            scope.filterString = "";
            this.page = 1;
            this.sort = "recent";
            return null;
        }
        if (!scope.focused) {
            window.setTimeout(function () {
                let inputEl = document.getElementById("wv-pb-select-component-input");
                if (inputEl) {
                    inputEl.focus();
                    scope.focused = true;
                }
            }, 10);
        }
        let rowArray = [0, 1, 2, 3];
        let pageArray = [];
        for (let i = 1; i < scope.pageCount + 1; i++) {
            pageArray.push(i);
        }
        return (h("div", { class: "modal show d-block", style: { paddingRight: "17px" } },
            h("div", { class: "modal-dialog modal-full" },
                h("div", { class: "modal-content" },
                    h("div", { class: "modal-header d-none" },
                        h("h5", { class: "modal-title" }, "Select component"),
                        h("button", { type: "button", class: "close", onClick: (e) => scope.cancelNodeCreateHandler(e) },
                            h("span", { "aria-hidden": "true" }, "\u00D7"))),
                    h("div", { class: "modal-body" },
                        h("nav", { class: "navbar navbar-expand-lg navbar-light mb-3 go-bkg-blue-gray-light" },
                            h("div", { class: "flex-grow-1" },
                                h("ul", { class: "nav nav-pills" },
                                    h("li", { class: "nav-item" },
                                        h("a", { class: "nav-link " + (scope.sort === "recent" ? "active" : ""), href: "#", onClick: (e) => scope.changeSort(e, "recent") }, "Recent")),
                                    h("li", { class: "nav-item" },
                                        h("a", { class: "nav-link " + (scope.sort === "usage" ? "active" : ""), href: "#", onClick: (e) => scope.changeSort(e, "usage") }, "Most Used")),
                                    h("li", { class: "nav-item" },
                                        h("a", { class: "nav-link " + (scope.sort === "alpha" ? "active" : ""), href: "#", onClick: (e) => scope.changeSort(e, "alpha") }, "Alphabetical")))),
                            h("div", { class: "form-inline" },
                                h("input", { class: "form-control form-control-sm", placeholder: "component name", onInput: (e) => scope.filterChangeHandler(e), id: "wv-pb-select-component-input" }))),
                        scope.componentList.map(function (record, index) {
                            record = record;
                            return (h("div", { class: "row" }, rowArray.map(function (subIndex) {
                                let compIndex = index + subIndex;
                                if (compIndex < scope.componentList.length && index % 4 === 0) {
                                    let component = scope.componentList[compIndex];
                                    let iconClass = "fa fa-file";
                                    if (component["icon_class"]) {
                                        iconClass = component["icon_class"];
                                    }
                                    return (h("div", { class: "col-6 col-lg-3", key: component["name"] },
                                        h("div", { class: "shadow-sm mb-4 card icon-card clickable", onClick: (e) => scope.selectComponent(e, component) },
                                            h("div", { class: "card-body p-1" },
                                                h("div", { class: "icon-card-body" },
                                                    h("i", { class: "icon " + iconClass }),
                                                    h("div", { class: "meta" },
                                                        h("div", { class: "title" }, component["label"]),
                                                        h("div", { class: "description" }, component["description"]),
                                                        h("div", { class: "library" }, component["library"])))))));
                                }
                            })));
                        }),
                        h("nav", { "aria-label": "Page navigation example" },
                            h("ul", { class: "pagination justify-content-center" },
                                h("li", { class: "page-item " + (scope.page > 1 ? "" : "disabled") },
                                    h("a", { class: "page-link", href: "#", onClick: (e) => scope.changePage(e, scope.page - 1) }, "Previous")),
                                pageArray.map(function (pageNum) {
                                    return (h("li", { class: "page-item " + (scope.page === pageNum ? "active" : "") },
                                        h("a", { class: "page-link", href: "#", onClick: (e) => scope.changePage(e, pageNum) }, pageNum)));
                                }),
                                h("li", { class: "page-item " + (scope.page >= scope.pageCount ? "disabled" : "") },
                                    h("a", { class: "page-link", href: "#", onClick: (e) => scope.changePage(e, scope.page + 1) }, "Next"))))),
                    h("div", { class: "modal-footer" },
                        h("button", { type: "button", class: "btn btn-white btn-sm", onClick: (e) => scope.cancelNodeCreateHandler(e) }, "Cancel"))))));
    }
    static get is() { return "wv-create-modal"; }
    static get properties() { return {
        "componentList": {
            "state": true
        },
        "filterString": {
            "state": true
        },
        "isCreateModalVisible": {
            "state": true
        },
        "page": {
            "state": true
        },
        "pageCount": {
            "state": true
        },
        "pageSize": {
            "state": true
        },
        "sort": {
            "state": true
        },
        "store": {
            "context": "store"
        },
        "total": {
            "state": true
        }
    }; }
}
