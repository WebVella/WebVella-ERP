import '../../stencil.core';
import { Store, Action } from '@stencil/redux';
export declare class WvPost {
    store: Store;
    post: Object;
    isReplyBoxVisible: boolean;
    isReplyBtnDisabled: boolean;
    ckEditor: Object;
    reloadPostIndex: number;
    removePost: Action;
    addComment: Action;
    componentWillLoad(): void;
    ReplyLinkHandler(event: UIEvent): void;
    DeleteLinkHandler(event: UIEvent): void;
    render(): JSX.Element;
}
