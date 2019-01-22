import '../../stencil.core';
import { Store } from '@stencil/redux';
export declare class WvPbNodeContainer {
    store: Store;
    pageNodeChangeIndex: number;
    componentWillLoad(): void;
    render(): JSX.Element[];
}
