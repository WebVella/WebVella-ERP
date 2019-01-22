import '../../stencil.core';
import { Store, Action } from '@stencil/redux';
export declare class WvCreateModal {
    store: Store;
    isOptionsModalVisible: boolean;
    isSaveLoading: boolean;
    setOptionsModalState: Action;
    updateNodeOptions: Action;
    addReloadNodeIds: Action;
    componentWillLoad(): void;
    cancelOptionsModalHandler(event: UIEvent): void;
    saveOptionsModalHandler(event: UIEvent): void;
    render(): JSX.Element;
}
