import '../../stencil.core';
import { Store, Action } from '@stencil/redux';
export declare class WvPost {
    store: Store;
    post: Object;
    isReplyBoxVisible: boolean;
    isReplyBtnDisabled: boolean;
    reloadPostIndex: number;
    removeTimelog: Action;
    componentWillLoad(): void;
    DeleteLinkHandler(event: UIEvent): void;
    render(): JSX.Element;
}
