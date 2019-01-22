import '../../stencil.core';
import { Store, Action } from '@stencil/redux';
export declare class WvDatasourceParamForm {
    el: HTMLStencilElement;
    store: Store;
    datasourceId: string;
    setLibrary: Action;
    componentWillLoad(): void;
    libraryVersionUpdate(): void;
    render(): JSX.Element;
}
