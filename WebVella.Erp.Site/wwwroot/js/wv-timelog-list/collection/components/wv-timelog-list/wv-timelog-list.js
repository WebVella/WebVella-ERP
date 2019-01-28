import { configureStore } from '../../store/index';
import WvTimeLogListStore from '../../models/WvTimeLogListStore';
export class WvPostList {
    constructor() {
        this.records = "[]";
        this.currentUser = "{}";
        this.isDebug = "false";
        this.relatedRecords = null;
        this.siteRootUrl = null;
        this.isBillable = true;
        this.reloadPostIndex = 1;
    }
    componentWillLoad() {
        var initStore = new WvTimeLogListStore();
        initStore.records = JSON.parse(this.records);
        initStore.currentUser = JSON.parse(this.currentUser);
        initStore.relatedRecords = this.relatedRecords;
        initStore.siteRootUrl = this.siteRootUrl;
        initStore.isBillable = this.isBillable;
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
            h("wv-add-timelog", null),
            recordsList.map(function (post) {
                return (h("wv-timelog", { key: post["id"], post: post }));
            })));
    }
    static get is() { return "wv-timelog-list"; }
    static get properties() { return {
        "currentUser": {
            "type": String,
            "attr": "current-user"
        },
        "isBillable": {
            "type": Boolean,
            "attr": "is-billable"
        },
        "isDebug": {
            "type": String,
            "attr": "is-debug"
        },
        "records": {
            "type": String,
            "attr": "records"
        },
        "relatedRecords": {
            "type": String,
            "attr": "related-records"
        },
        "reloadPostIndex": {
            "state": true
        },
        "siteRootUrl": {
            "type": String,
            "attr": "site-root-url"
        },
        "store": {
            "context": "store"
        }
    }; }
}
