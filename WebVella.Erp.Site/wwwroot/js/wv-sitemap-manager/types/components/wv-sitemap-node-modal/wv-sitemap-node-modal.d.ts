import '../../stencil.core';
import { EventEmitter } from '../../stencil.core';
export declare class WvSitemapNodeModal {
    nodeObj: Object;
    nodePageDict: Object;
    apiRoot: string;
    appId: string;
    nodeAuxData: Object;
    submitResponse: Object;
    wvSitemapManagerNodeModalCloseEvent: EventEmitter;
    wvSitemapManagerNodeSubmittedEvent: EventEmitter;
    wvSitemapManagerNodeAuxDataUpdateEvent: EventEmitter;
    modalNodeObj: Object;
    componentWillLoad(): void;
    componentDidUnload(): void;
    LoadData(): void;
    closeModal(): void;
    handleSubmit(e: any): void;
    handleChange(event: any): void;
    handleCheckboxChange(event: any): void;
    handleSelectChange(event: any): void;
    render(): JSX.Element;
}
