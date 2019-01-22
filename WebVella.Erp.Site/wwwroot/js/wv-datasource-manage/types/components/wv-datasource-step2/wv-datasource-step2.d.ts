import '../../stencil.core';
import { Store } from '@stencil/redux';
export declare class WvDatasourceStep2 {
    el: HTMLStencilElement;
    store: Store;
    datasourceId: string;
    libraryVersion: number;
    isParamInfoVisible: boolean;
    componentWillLoad(): void;
    libraryVersionUpdate(): void;
    showParamInfo(e: UIEvent): void;
    render(): JSX.Element;
}
