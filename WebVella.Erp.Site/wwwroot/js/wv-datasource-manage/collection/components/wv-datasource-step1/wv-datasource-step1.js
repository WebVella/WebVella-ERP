import * as action from '../../store/actions';
import _ from 'lodash';
export class WvDatasourceStep1 {
    constructor() {
        this.libraryQueryString = null;
        this.libraryVersion = 0;
    }
    componentWillLoad() {
        this.store.mapStateToProps(this, (state) => {
            return {
                libraryVersion: state.libraryVersion
            };
        });
        this.store.mapDispatchToProps(this, {
            setDatasource: action.setDatasource
        });
    }
    filterChangeHandler(event) {
        event.preventDefault();
        event.stopPropagation();
        this.libraryQueryString = event.target.value;
    }
    selectDatasource(event, datasource) {
        event.preventDefault();
        event.stopPropagation();
        let payload = {
            datasourceId: datasource["id"],
            pageDatasourceId: "",
            pageDatasourceName: "",
            pageDatasourceParams: new Array()
        };
        this.setDatasource(payload);
    }
    render() {
        let scope = this;
        let library = scope.store.getState().library;
        let filteredLibrary = new Array();
        if (scope.libraryQueryString) {
            filteredLibrary = _.filter(library, function (x) {
                let state = x["name"].toLowerCase().includes(scope.libraryQueryString.toLowerCase());
                if (!state) {
                    state = x["description"].toLowerCase().includes(scope.libraryQueryString.toLowerCase());
                }
                if (!state) {
                    state = x["entity_name"].toLowerCase().includes(scope.libraryQueryString.toLowerCase());
                }
                return state;
            });
        }
        else {
            filteredLibrary = library;
        }
        if (!filteredLibrary) {
            filteredLibrary = new Array();
        }
        window.setTimeout(function () {
            document.getElementById("wv-datasource-select-input").focus();
        }, 500);
        return (h("div", null,
            h("div", { class: "input-group input-group-sm mb-3" },
                h("div", { class: "input-group-prepend" },
                    h("div", { class: "input-group-text" },
                        h("i", { class: "fa fa-search" }))),
                h("input", { class: "form-control", value: scope.libraryQueryString, onInput: (e) => scope.filterChangeHandler(e), id: "wv-datasource-select-input" })),
            h("div", { class: "row" }, filteredLibrary.map(function (datasource) {
                let iconClass = "fa fa-fw fa-database go-purple";
                if (datasource["type"] === 1) {
                    iconClass = "fa fa-fw fa-code go-pink";
                }
                return (h("div", { class: "col-4" },
                    h("div", { class: "shadow-sm mb-4 card icon-card clickable", onClick: (e) => scope.selectDatasource(e, datasource) },
                        h("div", { class: "card-body p-1" },
                            h("div", { class: "icon-card-body" },
                                h("i", { class: "icon " + iconClass }),
                                h("div", { class: "meta" },
                                    h("div", { class: "title" }, datasource["name"]),
                                    h("div", { class: "description" }, datasource["description"]),
                                    h("div", { class: "library" }, datasource["entity_name"])))))));
            }))));
    }
    static get is() { return "wv-datasource-step1"; }
    static get properties() { return {
        "libraryQueryString": {
            "state": true
        },
        "libraryVersion": {
            "state": true
        },
        "store": {
            "context": "store"
        }
    }; }
}
