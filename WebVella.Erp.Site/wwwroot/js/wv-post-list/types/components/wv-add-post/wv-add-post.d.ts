import '../../stencil.core';
import { Store, Action } from '@stencil/redux';
export declare class WvAddNew {
    store: Store;
    isReplyBoxVisible: boolean;
    isReplyBtnDisabled: boolean;
    ckEditor: Object;
    minutes: number;
    isBillable: boolean;
    body: string;
    addPost: Action;
    componentWillLoad(): void;
    componentDidLoad(): void;
    ReplyLinkHandler(event: UIEvent): void;
    minutesChange(ev: any): void;
    bodyChange(ev: any): void;
    billableChange(event: any): void;
    render(): JSX.Element;
}
