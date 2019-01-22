export default class WvPostListStore {
    constructor() {
        this.isDebug = false;
        this.posts = new Array();
        this.currentUser = null;
        this.editorConfig = null;
        this.relatedRecordId = null;
        this.relatedRecords = null;
        this.siteRootUrl = "http://localhost:2202";
        this.reloadPostIndex = 1;
    }
}
