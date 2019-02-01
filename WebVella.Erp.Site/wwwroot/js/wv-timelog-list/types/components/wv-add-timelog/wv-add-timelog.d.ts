import '../../stencil.core';
import { Store, Action } from '@stencil/redux';
export declare class WvAddNew {
    store: Store;
    isReplyBoxVisible: boolean;
    isWarningVisible: boolean;
    minutes: number;
    isBillable: boolean;
    taskBody: string;
    loggedOn: string;
    datePickr: Object;
    addTimelog: Action;
    componentWillLoad(): void;
    componentDidLoad(): void;
    ReplyLinkHandler(event: UIEvent): void;
    minutesChange(ev: any): void;
    bodyChange(ev: any): void;
    loggedOnChange(ev: any): void;
    billableChange(event: any): void;
    render(): JSX.Element;
}
