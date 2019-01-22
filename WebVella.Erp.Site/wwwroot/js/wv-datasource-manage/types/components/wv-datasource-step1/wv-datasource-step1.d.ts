import '../../stencil.core';
import { Store, Action } from '@stencil/redux';
export declare class WvDatasourceStep1 {
    store: Store;
    libraryQueryString: string;
    libraryVersion: number;
    setDatasource: Action;
    componentWillLoad(): void;
    filterChangeHandler(event: any): void;
    selectDatasource(event: UIEvent, datasource: Object): void;
    render(): JSX.Element;
}
