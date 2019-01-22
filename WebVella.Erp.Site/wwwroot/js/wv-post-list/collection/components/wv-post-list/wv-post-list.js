import { configureStore } from '../../store/index';
import _ from 'lodash';
import WvPostListStore from '../../models/WvPostListStore';
export class WvPostList {
    constructor() {
        this.posts = "[]";
        this.currentUser = "{}";
        this.isDebug = "false";
        this.relatedRecordId = null;
        this.relatedRecords = null;
        this.siteRootUrl = null;
        this.reloadPostIndex = 1;
    }
    componentWillLoad() {
        var initStore = new WvPostListStore();
        initStore.posts = JSON.parse(this.posts);
        initStore.currentUser = JSON.parse(this.currentUser);
        initStore.relatedRecordId = this.relatedRecordId;
        initStore.relatedRecords = this.relatedRecords;
        initStore.siteRootUrl = this.siteRootUrl;
        initStore.reloadPostIndex = 1;
        let config = new Object();
        config.language = "en";
        config.skin = 'moono-lisa';
        config.autoGrow_minHeight = 160;
        config.autoGrow_maxHeight = 600;
        config.autoGrow_bottomSpace = 10;
        config.autoGrow_onStartup = true;
        config.allowedContent = true;
        config.autoParagraph = false;
        config.toolbarLocation = 'top';
        let extraPluginsArray = ['divarea'];
        let removePluginsArray = [];
        extraPluginsArray.push("panel");
        extraPluginsArray.push("autogrow");
        config.toolbar = 'full';
        config.toolbar_full = [
            { name: 'basicstyles', items: ['Bold', 'Italic'] },
            { name: 'paragraph', items: ['NumberedList', 'BulletedList'] },
            { name: 'indent', items: ['Indent', 'Outdent'] },
            { name: 'links', items: ['Link', 'Unlink'] },
            { name: 'pasting', items: ['PasteText', 'PasteFromWord'] },
        ];
        removePluginsArray.push("uploadimage");
        removePluginsArray.push("uploadfile");
        if (extraPluginsArray.length > 0) {
            config.extraPlugins = _.join(extraPluginsArray, ",");
        }
        if (removePluginsArray.length > 0) {
            config.removePlugins = _.join(removePluginsArray, ",");
        }
        initStore.editorConfig = config;
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
        let postList = storeState.posts;
        return (h("div", { class: "pc-post-list" },
            postList.map(function (post) {
                return (h("wv-post", { key: post["id"], post: post }));
            }),
            h("wv-add-post", null)));
    }
    static get is() { return "wv-post-list"; }
    static get properties() { return {
        "currentUser": {
            "type": String,
            "attr": "current-user"
        },
        "isDebug": {
            "type": String,
            "attr": "is-debug"
        },
        "posts": {
            "type": String,
            "attr": "posts"
        },
        "relatedRecordId": {
            "type": String,
            "attr": "related-record-id"
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
