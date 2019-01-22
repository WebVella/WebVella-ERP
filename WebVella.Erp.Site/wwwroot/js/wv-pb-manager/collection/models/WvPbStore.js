export default class WvPbStore {
    constructor() {
        this.library = new Array();
        this.pageNodes = new Array();
        this.pageId = null;
        this.siteRootUrl = null;
        this.drake = new Object();
        this.activeNodeId = null;
        this.hoveredNodeId = null;
        this.hoveredContainerId = null;
        this.pageNodeChangeIndex = 1;
        this.isCreateModalVisible = false;
        this.createdNode = new Object;
        this.isOptionsModalVisible = false;
        this.isHelpModalVisible = false;
        this.reloadNodeIdList = new Array();
        this.componentMeta = new Object();
        this.recordId = null;
    }
}
