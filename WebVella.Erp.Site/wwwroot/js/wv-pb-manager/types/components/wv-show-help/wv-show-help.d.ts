import { Store } from '@stencil/redux';
export declare class WvCreateModal {
    store: Store;
    nodeId: string;
    isHelpModalVisible: boolean;
    componentWillLoad(): void;
    helpModalVisibilityHandler(newValue: boolean, oldValue: boolean): void;
    render(): any;
}
