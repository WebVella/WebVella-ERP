import '../../stencil.core';
import { Store, Action } from '@stencil/redux';
export declare class WvPbTreeNode {
    store: Store;
    node: Object;
    level: number;
    activeNodeId: string;
    hoveredNodeId: string;
    pageNodeChangeIndex: number;
    setActiveNode: Action;
    hoverNode: Action;
    componentWillLoad(): void;
    nodeClickHandler(event: any): void;
    hoverNodeHandler(event: any): void;
    unhoverNodeHandler(event: any): void;
    render(): JSX.Element;
}
