import '../../stencil.core';
import { EventEmitter } from '../../stencil.core';
export declare class WvSitemapAreaModal {
    area: Object;
    submitResponse: Object;
    wvSitemapManagerAreaModalCloseEvent: EventEmitter;
    wvSitemapManagerAreaSubmittedEvent: EventEmitter;
    modalArea: Object;
    componentWillLoad(): void;
    componentDidUnload(): void;
    closeModal(): void;
    handleSubmit(e: any): void;
    handleChange(event: any): void;
    handleCheckboxChange(event: any): void;
    render(): JSX.Element;
}
