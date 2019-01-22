import '../../stencil.core';
import { Store, Action } from '@stencil/redux';
export declare class WvPageManager {
    store: Store;
    libraryJson: string;
    pageNodesJson: string;
    siteRootUrl: string;
    pageId: string;
    recordId: string;
    nodesPendingReload: Array<string>;
    pageNodes: Array<object>;
    setDrake: Action;
    addReloadNodeIds: Action;
    updatePageNodes: Action;
    removeNode: Action;
    componentWillLoad(): void;
    handleMouseMove(ev: KeyboardEvent): void;
    componentDidLoad(): void;
    render(): JSX.Element;
}
