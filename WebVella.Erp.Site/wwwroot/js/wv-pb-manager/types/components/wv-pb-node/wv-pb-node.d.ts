import '../../stencil.core';
import { Store, Action } from '@stencil/redux';
export declare class WvNode {
    el: HTMLElement;
    store: Store;
    nodeId: string;
    isLoading: boolean;
    reloadNodeIdList: Array<string>;
    removeReloadNodeIds: Action;
    componentWillLoad(): void;
    nodeIndexUpdateHandler(newValue: any): void;
    render(): JSX.Element;
}
