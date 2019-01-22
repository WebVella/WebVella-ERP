export default class NodeUtils {
    static MoveNodeToStack(nodeId: any): boolean;
    static GetNodeFromStack(nodeId: any): boolean;
    static GetNodeFromServerToStack(node: any, oldMeta: any, scope: any): void;
    static GetNodeAndMeta(scope: any, nodeId: any): {
        node: any;
        meta: any;
    };
    static GetActiveNodeAndMeta(scope: any): {
        node: any;
        meta: any;
    };
    static GetChildNodes(parentId?: string, containerId?: string, allNodes?: Array<Object>, result?: Array<string>, applyContainerId?: boolean): void;
}
