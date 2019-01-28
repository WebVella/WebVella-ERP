export default class WvTimeLogListStore {
    constructor() {
        this.isDebug = false;
        this.records = new Array();
        this.currentUser = null;
        this.relatedRecords = null;
        this.siteRootUrl = "http://localhost:2202";
        this.reloadPostIndex = 1;
        this.isBillable = true;
    }
}
