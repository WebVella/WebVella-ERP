import '../../stencil.core';
import { Store, Action } from '@stencil/redux';
export declare class WvPbInspector {
    store: Store;
    activeNodeId: string;
    isHelpLoading: boolean;
    isOptionsLoading: boolean;
    removeNode: Action;
    setOptionsModalState: Action;
    setHelpModalState: Action;
    componentWillLoad(): void;
    deleteNodeHandler(event: UIEvent): void;
    showOptionsModalHandler(event: UIEvent): void;
    showHelpModalHandler(event: UIEvent): void;
    render(): JSX.Element[];
}
