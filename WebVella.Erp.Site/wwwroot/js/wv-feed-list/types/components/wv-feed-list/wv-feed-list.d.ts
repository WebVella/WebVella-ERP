import '../../stencil.core';
import { Store } from '@stencil/redux';
export declare class WvPostList {
    store: Store;
    records: string;
    isDebug: string;
    reloadPostIndex: number;
    componentWillLoad(): void;
    render(): JSX.Element;
}
