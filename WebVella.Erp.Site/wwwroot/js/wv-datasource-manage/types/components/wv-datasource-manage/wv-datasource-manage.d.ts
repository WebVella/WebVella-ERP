import '../../stencil.core';
import { Store, Action } from '@stencil/redux';
export declare class WvDatasourceManage {
    store: Store;
    show: boolean;
    datasourceId: string;
    pageDatasourceId: string;
    pageDatasourceName: string;
    pageDatasourceParams: string;
    apiRootUrl: string;
    setDatasource: Action;
    componentWillLoad(): void;
    componentWillUpdate(): void;
    render(): JSX.Element;
}
