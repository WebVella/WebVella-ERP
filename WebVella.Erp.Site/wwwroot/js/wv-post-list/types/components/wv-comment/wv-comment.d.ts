import '../../stencil.core';
import { Store, Action } from '@stencil/redux';
export declare class WvComment {
    store: Store;
    comment: Object;
    removeComment: Action;
    componentWillLoad(): void;
    DeleteLinkHandler(event: UIEvent): void;
    render(): JSX.Element;
}
