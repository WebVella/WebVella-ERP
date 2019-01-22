import '../../stencil.core';
import { Store, Action } from '@stencil/redux';
export declare class WvContainer {
    store: Store;
    containerId: string;
    parentNodeId: string;
    activeNodeId: string;
    hoveredNodeId: string;
    hoveredContainerId: string;
    pageNodeChangeIndex: string;
    addDrakeContainerId: Action;
    setActiveNode: Action;
    hoverNode: Action;
    hoverContainer: Action;
    setNodeCreation: Action;
    addReloadNodeIds: Action;
    componentWillLoad(): void;
    componentDidLoad(): void;
    pageNodeIndexChangeHandler(): void;
    hoverContainerHandler(event: any): void;
    unhoverContainerHandler(event: any): void;
    nodeClickHandler(event: any, nodeId: any): void;
    hoverNodeHandler(event: any): void;
    unhoverNodeHandler(event: any): void;
    addNodeHandler(event: UIEvent): void;
    render(): JSX.Element;
}
