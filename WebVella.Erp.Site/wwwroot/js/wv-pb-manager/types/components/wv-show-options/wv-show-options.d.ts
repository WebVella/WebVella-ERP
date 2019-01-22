import { Store } from '@stencil/redux';
export declare class WvCreateModal {
    store: Store;
    nodeId: string;
    isOptionsModalVisible: boolean;
    componentWillLoad(): void;
    optionsModalVisibilityHandler(newValue: boolean, oldValue: boolean): void;
    render(): any;
}
