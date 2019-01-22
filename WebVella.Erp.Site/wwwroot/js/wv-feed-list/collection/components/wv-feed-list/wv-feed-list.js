import { configureStore } from '../../store/index';
import WvStore from '../../models/WvStore';
export class WvPostList {
    constructor() {
        this.records = "{}";
        this.isDebug = "false";
        this.reloadPostIndex = 1;
    }
    componentWillLoad() {
        var initStore = new WvStore();
        initStore.records = JSON.parse(this.records);
        initStore.reloadPostIndex = 1;
        if (this.isDebug.toLowerCase() === "true") {
            initStore.isDebug = true;
        }
        else {
            initStore.isDebug = false;
        }
        this.store.setStore(configureStore(initStore));
        this.store.mapStateToProps(this, (state) => {
            return {
                reloadPostIndex: state.reloadPostIndex
            };
        });
    }
    render() {
        let scope = this;
        let storeState = scope.store.getState();
        let recordsList = storeState.records;
        return (h("div", { class: "pc-post-list" },
            Object.keys(recordsList).map(function (group) {
                return (h("div", null,
                    h("div", { class: "group" }, group),
                    recordsList[group].map(function (record) {
                        return (h("wv-feed", { key: record["id"], record: record }));
                    })));
            }),
            h("div", { class: "alert alert-info " + (recordsList.length === 0 ? "" : "d-none") }, "No feeds found")));
    }
    static get is() { return "wv-feed-list"; }
    static get properties() { return {
        "isDebug": {
            "type": String,
            "attr": "is-debug"
        },
        "records": {
            "type": String,
            "attr": "records"
        },
        "reloadPostIndex": {
            "state": true
        },
        "store": {
            "context": "store"
        }
    }; }
}
