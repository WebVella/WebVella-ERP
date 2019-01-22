import '../../stencil.core';
import { Store, Action } from '@stencil/redux';
export declare class WvCreateModal {
    store: Store;
    filterString: string;
    isCreateModalVisible: boolean;
    page: number;
    pageSize: number;
    sort: string;
    componentList: Array<Object>;
    total: number;
    pageCount: number;
    private focused;
    setNodeCreation: Action;
    addNode: Action;
    addReloadNodeIds: Action;
    componentWillLoad(): void;
    cancelNodeCreateHandler(event: UIEvent): void;
    filterChangeHandler(event: any): void;
    selectComponent(event: UIEvent, component: Object): void;
    changeSort(ev: any, sort: any): void;
    changePage(ev: any, page: any): void;
    render(): JSX.Element;
}
