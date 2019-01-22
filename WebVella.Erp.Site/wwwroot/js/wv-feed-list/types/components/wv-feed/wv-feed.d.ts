import '../../stencil.core';
import { Store } from '@stencil/redux';
export declare class WvPost {
    store: Store;
    record: Object;
    reloadPostIndex: number;
    componentWillLoad(): void;
    render(): JSX.Element;
}
