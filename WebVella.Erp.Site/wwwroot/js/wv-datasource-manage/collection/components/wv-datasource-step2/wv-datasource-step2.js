import _ from 'lodash';
export class WvDatasourceStep2 {
    constructor() {
        this.datasourceId = null;
        this.libraryVersion = 0;
        this.isParamInfoVisible = false;
    }
    componentWillLoad() {
        this.store.mapStateToProps(this, (state) => {
            return {
                datasourceId: state.datasourceId,
                libraryVersion: state.libraryVersion
            };
        });
    }
    libraryVersionUpdate() {
        this.el.forceUpdate();
    }
    showParamInfo(e) {
        e.preventDefault;
        this.isParamInfoVisible = !this.isParamInfoVisible;
    }
    render() {
        let scope = this;
        let library = scope.store.getState().library;
        if (library.length === 0) {
            return null;
        }
        let pageDatasourceId = scope.store.getState().pageDatasourceId;
        let pageDatasourceName = scope.store.getState().pageDatasourceName;
        let pageDatasourceParams = scope.store.getState().pageDatasourceParams;
        let datasource = _.find(library, (x) => x["id"] === scope.datasourceId);
        let iconClass = "fa fa-fw fa-database go-purple";
        if (datasource["type"] === 1) {
            iconClass = "fa fa-fw fa-code go-pink";
        }
        if (!pageDatasourceName) {
            pageDatasourceName = datasource["name"];
        }
        return (h("div", null,
            h("div", { class: "shadow-sm mb-3 card icon-card" },
                h("div", { class: "card-body p-1" },
                    h("div", { class: "icon-card-body" },
                        h("i", { class: "icon " + iconClass }),
                        h("div", { class: "meta" },
                            h("div", { class: "title" }, datasource["name"]),
                            h("div", { class: "description" }, datasource["description"]),
                            h("div", { class: "library" }, datasource["entity_name"]))))),
            h("input", { type: "hidden", name: "page_datasource_id", value: pageDatasourceId }),
            h("input", { type: "hidden", name: "datasource_id", value: datasource["id"] }),
            h("div", { class: "form-group erp-field text" },
                h("label", { class: "control-label label-stacked" }, "Page data property name:"),
                h("input", { type: "text", name: "page_datasource_name", value: pageDatasourceName, class: "form-control form-control-sm" })),
            h("h3", null,
                "Parameters ",
                h("small", null,
                    "(",
                    h("a", { href: "#", onClick: (e) => scope.showParamInfo(e) }, scope.isParamInfoVisible ? "hide info" : "show info"),
                    ")")),
            h("div", { class: scope.isParamInfoVisible ? "" : "d-none" },
                h("p", null,
                    "As parameter value you can submit ",
                    h("span", { class: "go-teal" }, "string"),
                    ", ",
                    h("span", { class: "go-teal" }, "number"),
                    ", \"",
                    h("span", { class: "go-teal" }, "true"),
                    "\", \"",
                    h("span", { class: "go-teal" }, "false"),
                    "\" or a reference to a datasource by using ",
                    h("span", { class: "go-teal" },
                        '{',
                        '{',
                        "datasourceName ?? default",
                        '}',
                        '}'),
                    " eg. ",
                    h("span", { class: "go-teal" },
                        '{',
                        '{',
                        "RequestQuery.returnUrl ?? /",
                        '}',
                        '}'))),
            datasource["parameters"].map(function (parameter) {
                let pageDataSourceValue = null;
                let paramIndex = _.findIndex(pageDatasourceParams, (x) => x["name"] === parameter["name"]);
                if (paramIndex > -1) {
                    pageDataSourceValue = pageDatasourceParams[paramIndex]["value"];
                }
                return (h("div", { class: "form-group datasource-param" },
                    h("div", { class: "input-group input-group-sm" },
                        h("div", { class: "input-group-prepend" },
                            h("div", { class: "input-group-text name" }, parameter["name"])),
                        h("div", { class: "input-group-prepend" },
                            h("div", { class: "input-group-text go-teal type" }, parameter["type"])),
                        h("input", { type: "text", name: "@_" + parameter["name"], value: pageDataSourceValue, placeholder: "use default", class: "form-control value" }),
                        h("div", { class: "input-group-append" },
                            h("div", { class: "input-group-text default" },
                                " ",
                                h("span", { class: "go-gray" }, "??"),
                                " ",
                                parameter["value"])))));
            }),
            datasource["parameters"].length === 0
                ? (h("div", { class: "alert alert-info" }, "This datasource has no parameters"))
                : (null)));
    }
    static get is() { return "wv-datasource-step2"; }
    static get properties() { return {
        "datasourceId": {
            "state": true
        },
        "el": {
            "elementRef": true
        },
        "isParamInfoVisible": {
            "state": true
        },
        "libraryVersion": {
            "state": true,
            "watchCallbacks": ["libraryVersionUpdate"]
        },
        "store": {
            "context": "store"
        }
    }; }
}
