import '../../stencil.core';
import { Store, Action } from '@stencil/redux';
export declare class WvCreateModal {
    store: Store;
    isHelpModalVisible: boolean;
    setHelpModalState: Action;
    componentWillLoad(): void;
    cancelHelpModalHandler(event: UIEvent): void;
    render(): JSX.Element;
}
