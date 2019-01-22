import '../../stencil.core';
export declare class WvSitemapManager {
    initData: string;
    appId: string;
    apiRoot: string;
    sitemapObj: Object;
    nodePageDict: Object;
    isAreaModalVisible: Boolean;
    managedArea: Object;
    isNodeModalVisible: Boolean;
    managedNodeObj: Object;
    apiResponse: Object;
    nodeAuxData: Object;
    componentWillLoad(): void;
    createArea(): void;
    areaManageEventHandler(event: CustomEvent): void;
    areaModalClose(): void;
    areaSubmittedEventHandler(event: CustomEvent): void;
    areaDeleteEventHandler(event: CustomEvent): void;
    nodeManageEventHandler(event: CustomEvent): void;
    nodeModalCloseEventHandler(): void;
    nodeSubmittedEventHandler(event: CustomEvent): void;
    nodeDeleteEventHandler(event: CustomEvent): void;
    nodeAuxDataUpdateEventHandler(event: CustomEvent): void;
    render(): JSX.Element;
}
