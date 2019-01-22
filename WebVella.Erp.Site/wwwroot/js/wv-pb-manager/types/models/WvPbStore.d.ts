export default class WvPbStore {
    library: Array<Object>;
    pageNodes: Array<Object>;
    pageId: string;
    siteRootUrl: string;
    drake: Object;
    activeNodeId: string;
    hoveredNodeId: string;
    hoveredContainerId: string;
    pageNodeChangeIndex: number;
    isCreateModalVisible: boolean;
    createdNode: Object;
    isOptionsModalVisible: boolean;
    isHelpModalVisible: boolean;
    reloadNodeIdList: Array<string>;
    componentMeta: Object;
    recordId: string;
}
